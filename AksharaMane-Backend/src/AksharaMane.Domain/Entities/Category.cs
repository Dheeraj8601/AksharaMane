using AksharaMane.Domain.Common;
using System.Collections.Generic;

namespace AksharaMane.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Book> Books { get; set; } = new List<Book>();
}