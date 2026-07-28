using AksharaMane.Application.Common;
using AksharaMane.Application.Common.Exceptions;
using AksharaMane.Application.Common.Models;
using AksharaMane.Application.DTOs.Order;
using AksharaMane.Application.Interfaces.Repositories;
using AksharaMane.Application.Interfaces.Services;
using AksharaMane.Domain.Entities;
using AksharaMane.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IEmailService _emailService;
        private readonly IOrderEmailTemplateService _emailTemplateService;
        private readonly ILogger<OrderService> _logger;
        public OrderService(
        IOrderRepository orderRepository,
        IBookRepository bookRepository,
        IEmailService emailService,
        IOrderEmailTemplateService emailTemplateService,
        ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _logger = logger;
        }

        public async Task<OrderResponseDto> CreateAsync(
            CreateOrderRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var book = await _bookRepository.GetByIdAsync(
                request.BookId,
                trackChanges: true,
                cancellationToken);

            if (book is null || !book.IsActive)
            {
                throw new NotFoundException(
                    "Book was not found.");
            }

            if (!book.Category.IsActive)
            {
                throw new BadRequestException(
                    "This book is currently unavailable.");
            }

            if (request.Quantity <= 0)
            {
                throw new BadRequestException(
                    "Quantity must be greater than zero.");
            }

            if (book.StockQuantity < request.Quantity)
            {
                throw new ConflictException(
                    $"Only {book.StockQuantity} item(s) are available.");
            }

            var totalAmount =
                book.Price * request.Quantity;

            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                BookId = book.Id,
                Quantity = request.Quantity,
                UnitPrice = book.Price,
                TotalAmount = totalAmount,

                CustomerName =
                    request.CustomerName.Trim(),

                Email =
                    request.Email.Trim().ToLowerInvariant(),

                Phone = request.Phone.Trim(),

                AddressLine1 =
                    request.AddressLine1.Trim(),

                AddressLine2 =
                    request.AddressLine2?.Trim(),

                City = request.City.Trim(),

                State = request.State.Trim(),

                Pincode = request.Pincode.Trim(),

                PaymentMethod =
                    PaymentMethod.CashOnDelivery,

                Status = OrderStatus.Placed,

                CreatedAt = DateTime.UtcNow
            };

            book.StockQuantity -= request.Quantity;
            book.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.AddAsync(
                order,
                cancellationToken);

            await _orderRepository.SaveChangesAsync(
                cancellationToken);

            var createdOrder =
                await _orderRepository.GetByIdAsync(
                    order.Id,
                    cancellationToken: cancellationToken);

            var emailBody = _emailTemplateService.BuildOrderPlacedEmail(createdOrder!);

            await SendEmailSafelyAsync(
                        new EmailMessage
                        {
                            ToEmail = createdOrder!.Email,
                            Subject = $"Order Confirmed - {createdOrder.OrderNumber}",
                            HtmlBody = emailBody
                        },
                        cancellationToken
                        );

            return MapToResponse(createdOrder!);
        }

        public async Task<OrderResponseDto> TrackAsync(
    TrackOrderRequestDto request,
    CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.OrderNumber))
            {
                throw new BadRequestException(
                    "Order number is required.");
            }

            if (string.IsNullOrWhiteSpace(request.EmailOrPhone))
            {
                throw new BadRequestException(
                    "Email address or phone number is required.");
            }

            var normalizedOrderNumber =
                request.OrderNumber.Trim().ToUpperInvariant();

            var order =
                await _orderRepository.GetByOrderNumberAsync(
                    normalizedOrderNumber,
                    cancellationToken);

            if (order is null ||
                !IsCustomerMatch(order, request.EmailOrPhone))
            {
                // Do not reveal whether the order number exists.
                throw new NotFoundException(
                    "Order details could not be found.");
            }

            return MapToResponse(order);
        }

        public async Task<OrderResponseDto> CancelByCustomerAsync(
    CancelOrderRequestDto request,
    CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.OrderNumber))
            {
                throw new BadRequestException(
                    "Order number is required.");
            }

            if (string.IsNullOrWhiteSpace(request.EmailOrPhone))
            {
                throw new BadRequestException(
                    "Email address or phone number is required.");
            }

            var normalizedOrderNumber =
                request.OrderNumber.Trim().ToUpperInvariant();

            var order =
                await _orderRepository.GetByOrderNumberForUpdateAsync(
                    normalizedOrderNumber,
                    cancellationToken);

            if (order is null ||
                !IsCustomerMatch(order, request.EmailOrPhone))
            {
                throw new NotFoundException(
                    "Order details could not be found.");
            }

            if (order.Status == OrderStatus.Cancelled)
            {
                throw new ConflictException(
                    "This order is already cancelled.");
            }

            if (order.Status == OrderStatus.Shipped ||
                order.Status == OrderStatus.Delivered)
            {
                throw new ConflictException(
                    "The order cannot be cancelled after shipping.");
            }

            order.Status = OrderStatus.Cancelled;
            order.CancellationReason =
                string.IsNullOrWhiteSpace(request.Reason)
                    ? "Cancelled by customer"
                    : request.Reason.Trim();

            order.CancelledAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;

            var book = await _bookRepository.GetByIdAsync(
                order.BookId,
                trackChanges: true,
                cancellationToken);

            if (book is not null)
            {
                book.StockQuantity += order.Quantity;
                book.UpdatedAt = DateTime.UtcNow;
            }

            await _orderRepository.SaveChangesAsync(
                cancellationToken);

            var updatedOrder =
                await _orderRepository.GetByIdAsync(
                    order.Id,
                    cancellationToken: cancellationToken);

            if (updatedOrder is null)
            {
                throw new NotFoundException(
                    "Updated order could not be loaded.");
            }

            var emailBody =
                _emailTemplateService.BuildOrderStatusEmail(
                    updatedOrder);

            await SendEmailSafelyAsync(
                new EmailMessage
                {
                    ToEmail = updatedOrder.Email,
                    Subject =
                        $"Order Cancelled - {updatedOrder.OrderNumber}",
                    HtmlBody = emailBody
                },
                cancellationToken);

            return MapToResponse(updatedOrder);
        }

        public async Task<PagedResult<OrderResponseDto>> GetAdminOrdersAsync(OrderFilterRequestDto request,CancellationToken cancellationToken = default)
        {
            NormalizePagination(request);

            var query = _orderRepository.GetQueryable().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.OrderNumber.Contains(search) ||
                    x.CustomerName.Contains(search) ||
                    x.Email.Contains(search) ||
                    x.Phone.Contains(search) ||
                    x.Book.Title.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (!Enum.TryParse<OrderStatus>(
                        request.Status,
                        true,
                        out var status))
                {
                    throw new BadRequestException(
                        "Invalid order status.");
                }

                query = query.Where(
                    x => x.Status == status);
            }

            if (request.FromDate.HasValue)
            {
                query = query.Where(
                    x => x.CreatedAt >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                var endDate =
                    request.ToDate.Value.Date.AddDays(1);

                query = query.Where(
                    x => x.CreatedAt < endDate);
            }

            if (request.MinimumAmount.HasValue)
            {
                query = query.Where(
                    x => x.TotalAmount >= request.MinimumAmount.Value);
            }

            if (request.MaximumAmount.HasValue)
            {
                query = query.Where(
                    x => x.TotalAmount <= request.MaximumAmount.Value);
            }

            var totalCount =
                await query.CountAsync(cancellationToken);

            var descending = request.SortDirection.Equals( "desc",StringComparison.OrdinalIgnoreCase);

            query = request.SortBy.Trim().ToLowerInvariant() switch
            {
                "ordernumber" => descending
                    ? query.OrderByDescending(x => x.OrderNumber)
                    : query.OrderBy(x => x.OrderNumber),

                "customername" => descending
                    ? query.OrderByDescending(x => x.CustomerName)
                    : query.OrderBy(x => x.CustomerName),

                "totalamount" => descending
                    ? query.OrderByDescending(x => x.TotalAmount)
                    : query.OrderBy(x => x.TotalAmount),

                "status" => descending
                    ? query.OrderByDescending(x => x.Status)
                    : query.OrderBy(x => x.Status),

                _ => descending
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt)
            };

            var orders = await query
                .Skip(
                    (request.PageNumber - 1) *
                    request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderResponseDto
                {
                    Id = x.Id,
                    OrderNumber = x.OrderNumber,
                    BookId = x.BookId,
                    BookTitle = x.Book.Title,
                    BookImageUrl = x.Book.ImageUrl,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalAmount = x.TotalAmount,
                    CustomerName = x.CustomerName,
                    Email = x.Email,
                    Phone = x.Phone,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    City = x.City,
                    State = x.State,
                    Pincode = x.Pincode,
                    PaymentMethod = x.PaymentMethod.ToString(),
                    Status = x.Status.ToString(),
                    CancellationReason = x.CancellationReason,
                    CancelledAt = x.CancelledAt,
                    CanCancel = x.Status == OrderStatus.Placed || x.Status == OrderStatus.Confirmed,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<OrderResponseDto>
            {
                Items = orders,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<OrderResponseDto> GetAdminOrderByIdAsync(int id,CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.GetByIdAsync(
                id,
                cancellationToken: cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(
                    $"Order with ID {id} was not found.");
            }

            return MapToResponse(order);
        }

        public async Task<OrderResponseDto> UpdateStatusAsync(int id,UpdateOrderStatusRequestDto request,CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.GetByIdAsync(
                id,
                trackChanges: true,
                cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(
                    $"Order with ID {id} was not found.");
            }

            ValidateStatusTransition(
                order.Status,
                request.Status);

            if (request.Status == OrderStatus.Cancelled &&
                order.Status != OrderStatus.Cancelled)
            {
                var book = await _bookRepository.GetByIdAsync(
                    order.BookId,
                    trackChanges: true,
                    cancellationToken);

                if (book is not null)
                {
                    book.StockQuantity += order.Quantity;
                    book.UpdatedAt = DateTime.UtcNow;
                }
            }

            order.Status = request.Status;
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.SaveChangesAsync(
                cancellationToken);

            var updatedOrder =
                await _orderRepository.GetByIdAsync(
                    id,
                    cancellationToken: cancellationToken);

            var emailBody = _emailTemplateService.BuildOrderStatusEmail(updatedOrder!);

            await SendEmailSafelyAsync(
                        new EmailMessage
                        {
                            ToEmail = updatedOrder!.Email,
                            Subject =
                                     $"Order Status Updated - {updatedOrder.OrderNumber}",
                            HtmlBody = emailBody
                         },
    cancellationToken);

            return MapToResponse(updatedOrder!);
        }

        private static void ValidateStatusTransition(OrderStatus currentStatus,OrderStatus newStatus)
        {
            if (currentStatus == newStatus)
            {
                return;
            }

            if (currentStatus == OrderStatus.Delivered)
            {
                throw new ConflictException(
                    "A delivered order cannot be changed.");
            }

            if (currentStatus == OrderStatus.Cancelled)
            {
                throw new ConflictException(
                    "A cancelled order cannot be changed.");
            }

            var isValid = currentStatus switch
            {
                OrderStatus.Placed =>
                    newStatus is
                        OrderStatus.Confirmed or
                        OrderStatus.Cancelled,

                OrderStatus.Confirmed =>
                    newStatus is
                        OrderStatus.Shipped or
                        OrderStatus.Cancelled,

                OrderStatus.Shipped =>
                    newStatus is
                        OrderStatus.Delivered,

                _ => false
            };

            if (!isValid)
            {
                throw new ConflictException(
                    $"Order status cannot change from " +
                    $"{currentStatus} to {newStatus}.");
            }
        }

        private static string GenerateOrderNumber()
        {
            var datePart =
                DateTime.UtcNow.ToString("yyyyMMdd");

            var randomPart =
                Random.Shared.Next(100000, 999999);

            return $"AM-{datePart}-{randomPart}";
        }

        private static void NormalizePagination(OrderFilterRequestDto request)
        {
            if (request.PageNumber < 1)
            {
                request.PageNumber = 1;
            }

            if (request.PageSize < 1)
            {
                request.PageSize = 10;
            }

            if (request.PageSize > 100)
            {
                request.PageSize = 100;
            }
        }

        private static OrderResponseDto MapToResponse(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                BookId = order.BookId,
                BookTitle = order.Book.Title,
                BookImageUrl = order.Book.ImageUrl,
                Quantity = order.Quantity,
                UnitPrice = order.UnitPrice,
                TotalAmount = order.TotalAmount,
                CustomerName = order.CustomerName,
                Email = order.Email,
                Phone = order.Phone,
                AddressLine1 = order.AddressLine1,
                AddressLine2 = order.AddressLine2,
                City = order.City,
                State = order.State,
                Pincode = order.Pincode,
                PaymentMethod = order.PaymentMethod.ToString(),
                Status = order.Status.ToString(),
                CancellationReason = order.CancellationReason,
                CancelledAt = order.CancelledAt,

                CanCancel =
                    order.Status == OrderStatus.Placed ||
                    order.Status == OrderStatus.Confirmed,

                CreatedAt = order.CreatedAt
            };
        }

        private async Task SendEmailSafelyAsync( EmailMessage message,CancellationToken cancellationToken)
        {
            try
            {
                await _emailService.SendAsync(
                    message,
                    cancellationToken);
            }
            catch(Exception exception)
            {
                _logger.LogError(
                            exception,
                            "Order email could not be sent to {Email}",
                            message.ToEmail);
            }
        }

        private static bool IsCustomerMatch(Order order,string emailOrPhone)
        {
            var value = emailOrPhone.Trim();

            var emailMatches = string.Equals(
                order.Email,
                value,
                StringComparison.OrdinalIgnoreCase);

            var normalizedInputPhone = NormalizePhone(value);
            var normalizedOrderPhone = NormalizePhone(order.Phone);

            var phoneMatches =
                !string.IsNullOrWhiteSpace(normalizedInputPhone) &&
                normalizedInputPhone == normalizedOrderPhone;

            return emailMatches || phoneMatches;
        }

        private static string NormalizePhone(string value)
        {
            return new string(
                value.Where(char.IsDigit).ToArray());
        }
    }
}
