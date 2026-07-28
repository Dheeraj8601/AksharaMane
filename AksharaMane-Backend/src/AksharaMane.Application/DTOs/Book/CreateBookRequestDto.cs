using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.DTOs.Book
{
    public class CreateBookRequestDto
    {
        [Required(ErrorMessage = "Book title is required.")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(150)]
        public string Author { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Select a valid category.")]
        public int CategoryId { get; set; }

        [Range(1, 100000, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [StringLength(50)]
        public string Language { get; set; } = "English";

        [Range(1, 100000)]
        public int? Pages { get; set; }

        [StringLength(150)]
        public string? Publisher { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        public IFormFile? Image { get; set; }
    }
}
