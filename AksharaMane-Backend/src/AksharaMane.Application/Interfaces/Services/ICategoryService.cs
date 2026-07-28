using AksharaMane.Application.DTOs.Category;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AksharaMane.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<List<CategoryResponseDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<List<CategoryResponseDto>> GetActiveAsync(
        CancellationToken cancellationToken = default);

    Task<CategoryResponseDto> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<CategoryResponseDto> CreateAsync(
        CreateCategoryRequestDto request,
        CancellationToken cancellationToken = default);

    Task<CategoryResponseDto> UpdateAsync(
        int id,
        UpdateCategoryRequestDto request,
        CancellationToken cancellationToken = default);

    Task ToggleStatusAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}