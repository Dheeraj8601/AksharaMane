using AksharaMane.Domain.Common;
using System.Collections.Generic;

namespace AksharaMane.Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string Language { get; set; } = "English";

    public int? Pages { get; set; }

    public string? Publisher { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public Category Category { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}