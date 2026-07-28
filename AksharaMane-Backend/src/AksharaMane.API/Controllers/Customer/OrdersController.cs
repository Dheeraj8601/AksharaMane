using AksharaMane.Application.Common;
using AksharaMane.Application.DTOs.Order;
using AksharaMane.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AksharaMane.API.Controllers.Customer
{
    [ApiController]
    [Route("api/orders")]
    [AllowAnonymous]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(
            IOrderService orderService)
        {
            _orderService = orderService;
        }

        [EnableRateLimiting("PublicOrderPolicy")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateOrderRequestDto request,
            CancellationToken cancellationToken)
        {
            var result =
                await _orderService.CreateAsync(
                    request,
                    cancellationToken);

            return Created(
                $"/api/orders/track",
                ApiResponse<OrderResponseDto>
                    .SuccessResponse(
                        result,
                        "Order placed successfully."));
        }

        [EnableRateLimiting("PublicOrderPolicy")]
        [HttpPost("track")]
        public async Task<IActionResult> Track(
            [FromBody] TrackOrderRequestDto request,
            CancellationToken cancellationToken)
        {
            var result =
                await _orderService.TrackAsync(
                    request,
                    cancellationToken);

            return Ok(
                ApiResponse<OrderResponseDto>
                    .SuccessResponse(result));
        }

        [EnableRateLimiting("PublicOrderPolicy")]
        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel(
            [FromBody] CancelOrderRequestDto request,
            CancellationToken cancellationToken)
        {
            var result =
                await _orderService.CancelByCustomerAsync(
                    request,
                    cancellationToken);

            return Ok(
                ApiResponse<OrderResponseDto>
                    .SuccessResponse(
                        result,
                        "Order cancelled successfully."));
        }
    }
}
