using AksharaMane.Application.Common;
using AksharaMane.Application.DTOs.Order;
using AksharaMane.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AksharaMane.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/orders")]
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public AdminOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] OrderFilterRequestDto request,CancellationToken cancellationToken)
        {
            var result =
                await _orderService.GetAdminOrdersAsync(
                    request,
                    cancellationToken);

            return Ok(
                ApiResponse<PagedResult<OrderResponseDto>>
                    .SuccessResponse(result));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            var result =
                await _orderService.GetAdminOrderByIdAsync(
                    id,
                    cancellationToken);

            return Ok(
                ApiResponse<OrderResponseDto>
                    .SuccessResponse(result));
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id,[FromBody] UpdateOrderStatusRequestDto request,CancellationToken cancellationToken)
        {
            var result =
                await _orderService.UpdateStatusAsync(
                    id,
                    request,
                    cancellationToken);

            return Ok(
                ApiResponse<OrderResponseDto>
                    .SuccessResponse(
                        result,
                        "Order status updated successfully."));
        }
    }
}
