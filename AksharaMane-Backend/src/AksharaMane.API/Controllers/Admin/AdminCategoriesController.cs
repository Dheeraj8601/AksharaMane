using AksharaMane.Application.Common;
using AksharaMane.Application.DTOs.Category;
using AksharaMane.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AksharaMane.API.Controllers.Admin;

[ApiController]
[Route("api/admin/categories")]
public class AdminCategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public AdminCategoriesController(
        ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(ApiResponse<List<CategoryResponseDto>>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var categories =
            await _categoryService.GetAllAsync(
                cancellationToken);

        return Ok(
            ApiResponse<List<CategoryResponseDto>>
                .SuccessResponse(categories));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(ApiResponse<CategoryResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var category =
            await _categoryService.GetByIdAsync(
                id,
                cancellationToken);

        return Ok(
            ApiResponse<CategoryResponseDto>
                .SuccessResponse(category));
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(ApiResponse<CategoryResponseDto>),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var category =
            await _categoryService.CreateAsync(
                request,
                cancellationToken);

        var response =
            ApiResponse<CategoryResponseDto>
                .SuccessResponse(
                    category,
                    "Category created successfully.");

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(
        typeof(ApiResponse<CategoryResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var category =
            await _categoryService.UpdateAsync(
                id,
                request,
                cancellationToken);

        return Ok(
            ApiResponse<CategoryResponseDto>
                .SuccessResponse(
                    category,
                    "Category updated successfully."));
    }

    [HttpPatch("{id:int}/toggle-status")]
    [ProducesResponseType(
        typeof(ApiResponse<object>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleStatus(
        int id,
        CancellationToken cancellationToken)
    {
        await _categoryService.ToggleStatusAsync(
            id,
            cancellationToken);

        return Ok(
            ApiResponse<object>
                .SuccessResponse(
                    new { id },
                    "Category status updated successfully."));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _categoryService.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }
}