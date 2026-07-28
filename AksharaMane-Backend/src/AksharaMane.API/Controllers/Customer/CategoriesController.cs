using AksharaMane.Application.Common;
using AksharaMane.Application.DTOs.Category;
using AksharaMane.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AksharaMane.API.Controllers.Customer;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(
        ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(ApiResponse<List<CategoryResponseDto>>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveCategories(
        CancellationToken cancellationToken)
    {
        var categories =
            await _categoryService.GetActiveAsync(
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

        if (!category.IsActive)
        {
            return NotFound(
                ApiResponse<object>.FailureResponse(
                    "Category was not found."));
        }

        return Ok(
            ApiResponse<CategoryResponseDto>
                .SuccessResponse(category));
    }
}