using AksharaMane.Application.Common;
using AksharaMane.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateAsync(
            CreateOrderRequestDto request,
            CancellationToken cancellationToken = default);

        Task<OrderResponseDto> TrackAsync(
            TrackOrderRequestDto request,
            CancellationToken cancellationToken = default);

        Task<OrderResponseDto> CancelByCustomerAsync(
            CancelOrderRequestDto request,
            CancellationToken cancellationToken = default);

        Task<PagedResult<OrderResponseDto>> GetAdminOrdersAsync(
            OrderFilterRequestDto request,
            CancellationToken cancellationToken = default);

        Task<OrderResponseDto> GetAdminOrderByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<OrderResponseDto> UpdateStatusAsync(
            int id,
            UpdateOrderStatusRequestDto request,
            CancellationToken cancellationToken = default);
    }
}
