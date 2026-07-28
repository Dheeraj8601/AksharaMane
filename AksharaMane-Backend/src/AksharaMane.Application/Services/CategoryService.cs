using AksharaMane.Application.Common.Exceptions;
using AksharaMane.Application.DTOs.Category;
using AksharaMane.Application.Interfaces.Repositories;
using AksharaMane.Application.Interfaces.Services;
using AksharaMane.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AksharaMane.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryResponseDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var categories =
            await _categoryRepository.GetAllAsync(cancellationToken);

        return categories
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<List<CategoryResponseDto>> GetActiveAsync(
        CancellationToken cancellationToken = default)
    {
        var categories =
            await _categoryRepository.GetActiveAsync(cancellationToken);

        return categories
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<CategoryResponseDto> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var category =
            await _categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(
                $"Category with ID {id} was not found.");
        }

        return MapToResponse(category);
    }

    public async Task<CategoryResponseDto> CreateAsync(
        CreateCategoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = request.Name.Trim();

        var alreadyExists =
            await _categoryRepository.ExistsByNameAsync(
                normalizedName,
                cancellationToken: cancellationToken);

        if (alreadyExists)
        {
            throw new ConflictException(
                $"A category named '{normalizedName}' already exists.");
        }

        var category = new Category
        {
            Name = normalizedName,
            Description = request.Description?.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _categoryRepository.AddAsync(
            category,
            cancellationToken);

        await _categoryRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(category);
    }

    public async Task<CategoryResponseDto> UpdateAsync(
        int id,
        UpdateCategoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var category =
            await _categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(
                $"Category with ID {id} was not found.");
        }

        var normalizedName = request.Name.Trim();

        var alreadyExists =
            await _categoryRepository.ExistsByNameAsync(
                normalizedName,
                excludeId: id,
                cancellationToken: cancellationToken);

        if (alreadyExists)
        {
            throw new ConflictException(
                $"Another category named '{normalizedName}' already exists.");
        }

        category.Name = normalizedName;
        category.Description = request.Description?.Trim();
        category.IsActive = request.IsActive;
        category.UpdatedAt = DateTime.UtcNow;

        _categoryRepository.Update(category);

        await _categoryRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(category);
    }

    public async Task ToggleStatusAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var category =
            await _categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(
                $"Category with ID {id} was not found.");
        }

        category.IsActive = !category.IsActive;
        category.UpdatedAt = DateTime.UtcNow;

        _categoryRepository.Update(category);

        await _categoryRepository.SaveChangesAsync(
            cancellationToken);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var category =
            await _categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(
                $"Category with ID {id} was not found.");
        }

        _categoryRepository.Delete(category);

        try
        {
            await _categoryRepository.SaveChangesAsync(
                cancellationToken);
        }
        catch (Exception)
        {
            throw new ConflictException(
                "This category cannot be deleted because it is being used by one or more books.");
        }
    }

    private static CategoryResponseDto MapToResponse(
        Category category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
}