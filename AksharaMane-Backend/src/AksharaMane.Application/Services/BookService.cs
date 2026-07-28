using AksharaMane.Application.Common;
using AksharaMane.Application.Common.Exceptions;
using AksharaMane.Application.DTOs.Book;
using AksharaMane.Application.Interfaces.Repositories;
using AksharaMane.Application.Interfaces.Services;
using AksharaMane.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileStorageService _fileStorageService;

        public BookService(
            IBookRepository bookRepository,
            ICategoryRepository categoryRepository,
            IFileStorageService fileStorageService)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<PagedResult<BookResponseDto>>
            GetCustomerBooksAsync(
                BookFilterRequestDto request,
                CancellationToken cancellationToken = default)
        {
            NormalizePagination(request);

            var query = _bookRepository
                .GetQueryable()
                .AsNoTracking()
                .Where(x =>
                    x.IsActive &&
                    x.Category.IsActive);

            query = ApplyFilters(query, request);

            return await CreatePagedResultAsync(
                query,
                request,
                cancellationToken);
        }

        public async Task<PagedResult<BookResponseDto>>
            GetAdminBooksAsync(
                BookFilterRequestDto request,
                CancellationToken cancellationToken = default)
        {
            NormalizePagination(request);

            var query = _bookRepository
                .GetQueryable()
                .AsNoTracking();

            if (request.IsActive.HasValue)
            {
                query = query.Where(
                    x => x.IsActive == request.IsActive.Value);
            }

            query = ApplyFilters(query, request);

            return await CreatePagedResultAsync(
                query,
                request,
                cancellationToken);
        }

        public async Task<BookResponseDto> GetByIdAsync(
            int id,
            bool customerView,
            CancellationToken cancellationToken = default)
        {
            var book = await _bookRepository.GetByIdAsync(
                id,
                cancellationToken: cancellationToken);

            if (book is null)
            {
                throw new NotFoundException(
                    $"Book with ID {id} was not found.");
            }

            if (customerView &&
                (!book.IsActive || !book.Category.IsActive))
            {
                throw new NotFoundException(
                    "Book was not found.");
            }

            return MapToResponse(book);
        }

        public async Task<BookResponseDto> CreateAsync(
            CreateBookRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var category =
                await _categoryRepository.GetByIdAsync(
                    request.CategoryId,
                    cancellationToken);

            if (category is null)
            {
                throw new NotFoundException(
                    "Selected category was not found.");
            }

            string? imageUrl = null;

            if (request.Image is not null)
            {
                imageUrl = await _fileStorageService.UploadAsync(
                    request.Image,
                    "books",
                    cancellationToken);
            }

            var book = new Book
            {
                Title = request.Title.Trim(),
                Author = request.Author.Trim(),
                CategoryId = request.CategoryId,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Language = request.Language.Trim(),
                Pages = request.Pages,
                Publisher = request.Publisher?.Trim(),
                Description = request.Description?.Trim(),
                ImageUrl = imageUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _bookRepository.AddAsync(
                book,
                cancellationToken);

            await _bookRepository.SaveChangesAsync(
                cancellationToken);

            var createdBook =
                await _bookRepository.GetByIdAsync(
                    book.Id,
                    cancellationToken: cancellationToken);

            return MapToResponse(createdBook!);
        }

        public async Task<BookResponseDto> UpdateAsync(
            int id,
            UpdateBookRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var book =
                await _bookRepository.GetByIdAsync(
                    id,
                    trackChanges: true,
                    cancellationToken);

            if (book is null)
            {
                throw new NotFoundException(
                    $"Book with ID {id} was not found.");
            }

            var category =
                await _categoryRepository.GetByIdAsync(
                    request.CategoryId,
                    cancellationToken);

            if (category is null)
            {
                throw new NotFoundException(
                    "Selected category was not found.");
            }

            var oldImageUrl = book.ImageUrl;

            if (request.Image is not null)
            {
                book.ImageUrl =
                    await _fileStorageService.UploadAsync(
                        request.Image,
                        "books",
                        cancellationToken);
            }

            book.Title = request.Title.Trim();
            book.Author = request.Author.Trim();
            book.CategoryId = request.CategoryId;
            book.Price = request.Price;
            book.StockQuantity = request.StockQuantity;
            book.Language = request.Language.Trim();
            book.Pages = request.Pages;
            book.Publisher = request.Publisher?.Trim();
            book.Description = request.Description?.Trim();
            book.IsActive = request.IsActive;
            book.UpdatedAt = DateTime.UtcNow;

            _bookRepository.Update(book);

            await _bookRepository.SaveChangesAsync(
                cancellationToken);

            if (request.Image is not null)
            {
                _fileStorageService.Delete(oldImageUrl);
            }

            var updatedBook =
                await _bookRepository.GetByIdAsync(
                    id,
                    cancellationToken: cancellationToken);

            return MapToResponse(updatedBook!);
        }

        public async Task ToggleStatusAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var book =
                await _bookRepository.GetByIdAsync(
                    id,
                    trackChanges: true,
                    cancellationToken);

            if (book is null)
            {
                throw new NotFoundException(
                    $"Book with ID {id} was not found.");
            }

            book.IsActive = !book.IsActive;
            book.UpdatedAt = DateTime.UtcNow;

            await _bookRepository.SaveChangesAsync(
                cancellationToken);
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var book =
                await _bookRepository.GetByIdAsync(
                    id,
                    trackChanges: true,
                    cancellationToken);

            if (book is null)
            {
                throw new NotFoundException(
                    $"Book with ID {id} was not found.");
            }

            var imageUrl = book.ImageUrl;

            _bookRepository.Delete(book);

            try
            {
                await _bookRepository.SaveChangesAsync(
                    cancellationToken);
            }
            catch
            {
                throw new ConflictException(
                    "This book cannot be deleted because it is associated with an order.");
            }

            _fileStorageService.Delete(imageUrl);
        }

        private static IQueryable<Book> ApplyFilters(
            IQueryable<Book> query,
            BookFilterRequestDto request)
        {
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.Title.Contains(search) ||
                    x.Author.Contains(search) ||
                    x.Publisher != null &&
                    x.Publisher.Contains(search));
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(
                    x => x.CategoryId ==
                         request.CategoryId.Value);
            }

            return query.OrderByDescending(x => x.CreatedAt);
        }

        private static async Task<PagedResult<BookResponseDto>>
            CreatePagedResultAsync(
                IQueryable<Book> query,
                BookFilterRequestDto request,
                CancellationToken cancellationToken)
        {
            var totalCount =
                await query.CountAsync(cancellationToken);

            var books = await query
                .Skip(
                    (request.PageNumber - 1) *
                    request.PageSize)
                .Take(request.PageSize)
                .Select(x => new BookResponseDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Price = x.Price,
                    StockQuantity = x.StockQuantity,
                    Language = x.Language,
                    Pages = x.Pages,
                    Publisher = x.Publisher,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<BookResponseDto>
            {
                Items = books,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        private static void NormalizePagination(
            BookFilterRequestDto request)
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

        private static BookResponseDto MapToResponse(
            Book book)
        {
            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CategoryId = book.CategoryId,
                CategoryName = book.Category.Name,
                Price = book.Price,
                StockQuantity = book.StockQuantity,
                Language = book.Language,
                Pages = book.Pages,
                Publisher = book.Publisher,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                IsActive = book.IsActive,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt
            };
        }
    }
}
