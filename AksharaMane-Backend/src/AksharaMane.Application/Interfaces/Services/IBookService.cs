using AksharaMane.Application.Common;
using AksharaMane.Application.DTOs.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Interfaces.Services
{
    public interface IBookService
    {
        Task<PagedResult<BookResponseDto>> GetCustomerBooksAsync(
            BookFilterRequestDto request,
            CancellationToken cancellationToken = default);

        Task<PagedResult<BookResponseDto>> GetAdminBooksAsync(
            BookFilterRequestDto request,
            CancellationToken cancellationToken = default);

        Task<BookResponseDto> GetByIdAsync(
            int id,
            bool customerView,
            CancellationToken cancellationToken = default);

        Task<BookResponseDto> CreateAsync(
            CreateBookRequestDto request,
            CancellationToken cancellationToken = default);

        Task<BookResponseDto> UpdateAsync(
            int id,
            UpdateBookRequestDto request,
            CancellationToken cancellationToken = default);

        Task ToggleStatusAsync(int id,CancellationToken cancellationToken = default);

        Task DeleteAsync(int id,CancellationToken cancellationToken = default);
    }
}
