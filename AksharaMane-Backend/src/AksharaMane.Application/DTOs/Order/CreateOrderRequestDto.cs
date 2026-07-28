using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.DTOs.Order
{
    public class CreateOrderRequestDto
    {
        [Range(1, int.MaxValue)]
        public int BookId { get; set; }

        [Range(1, 10)]
        public int Quantity { get; set; } = 1;

        [Required]
        [StringLength(150)]
        public string CustomerName { get; set; } =
            string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } =
            string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } =
            string.Empty;

        [Required]
        [StringLength(250)]
        public string AddressLine1 { get; set; } =
            string.Empty;

        [StringLength(250)]
        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; } =
            string.Empty;

        [Required]
        [StringLength(100)]
        public string State { get; set; } =
            string.Empty;

        [Required]
        [RegularExpression(
            @"^[1-9][0-9]{5}$",
            ErrorMessage = "Enter a valid 6-digit pincode.")]
        public string Pincode { get; set; } =
            string.Empty;
    }
}
