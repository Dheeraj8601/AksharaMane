using AksharaMane.Application.Common;
using AksharaMane.Application.DTOs.Book;
using AksharaMane.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AksharaMane.API.Controllers.Customer
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks(
            [FromQuery] BookFilterRequestDto request,
            CancellationToken cancellationToken)
        {
            var result =
                await _bookService.GetCustomerBooksAsync(
                    request,
                    cancellationToken);

            return Ok(
                ApiResponse<PagedResult<BookResponseDto>>
                    .SuccessResponse(result));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var result =
                await _bookService.GetByIdAsync(
                    id,
                    customerView: true,
                    cancellationToken);

            return Ok(
                ApiResponse<BookResponseDto>
                    .SuccessResponse(result));
        }
    }
}
