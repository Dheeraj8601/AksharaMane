using AksharaMane.Application.Common;
using AksharaMane.Application.DTOs.Book;
using AksharaMane.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AksharaMane.API.Controllers.Admin
{
        [ApiController]
        [Route("api/admin/books")]
        public class AdminBooksController : ControllerBase
        {
            private readonly IBookService _bookService;

            public AdminBooksController(
                IBookService bookService)
            {
                _bookService = bookService;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll(
                [FromQuery] BookFilterRequestDto request,
                CancellationToken cancellationToken)
            {
                var result =
                    await _bookService.GetAdminBooksAsync(
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
                        customerView: false,
                        cancellationToken);

                return Ok(
                    ApiResponse<BookResponseDto>
                        .SuccessResponse(result));
            }

            [HttpPost]
            [Consumes("multipart/form-data")]
            public async Task<IActionResult> Create(
                [FromForm] CreateBookRequestDto request,
                CancellationToken cancellationToken)
            {
                var result =
                    await _bookService.CreateAsync(
                        request,
                        cancellationToken);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.Id },
                    ApiResponse<BookResponseDto>
                        .SuccessResponse(
                            result,
                            "Book created successfully."));
            }

            [HttpPut("{id:int}")]
            [Consumes("multipart/form-data")]
            public async Task<IActionResult> Update(
                int id,
                [FromForm] UpdateBookRequestDto request,
                CancellationToken cancellationToken)
            {
                var result =
                    await _bookService.UpdateAsync(
                        id,
                        request,
                        cancellationToken);

                return Ok(
                    ApiResponse<BookResponseDto>
                        .SuccessResponse(
                            result,
                            "Book updated successfully."));
            }

            [HttpPatch("{id:int}/toggle-status")]
            public async Task<IActionResult> ToggleStatus(
                int id,
                CancellationToken cancellationToken)
            {
                await _bookService.ToggleStatusAsync(
                    id,
                    cancellationToken);

                return Ok(
                    ApiResponse<object>
                        .SuccessResponse(
                            new { id },
                            "Book status updated successfully."));
            }

            [HttpDelete("{id:int}")]
            public async Task<IActionResult> Delete(
                int id,
                CancellationToken cancellationToken)
            {
                await _bookService.DeleteAsync(
                    id,
                    cancellationToken);

                return NoContent();
            }
        }
    }

