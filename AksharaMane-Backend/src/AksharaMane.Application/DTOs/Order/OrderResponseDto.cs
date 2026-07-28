using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.DTOs.Order
{
    public class OrderResponseDto
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } =
            string.Empty;

        public int BookId { get; set; }

        public string BookTitle { get; set; } =
            string.Empty;

        public string? BookImageUrl { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalAmount { get; set; }

        public string CustomerName { get; set; } =
            string.Empty;

        public string Email { get; set; } =
            string.Empty;

        public string Phone { get; set; } =
            string.Empty;

        public string AddressLine1 { get; set; } =
            string.Empty;

        public string? AddressLine2 { get; set; }

        public string City { get; set; } =
            string.Empty;

        public string State { get; set; } =
            string.Empty;

        public string Pincode { get; set; } =
            string.Empty;

        public string PaymentMethod { get; set; } =
            string.Empty;

        public string Status { get; set; } =
            string.Empty;

        public DateTime CreatedAt { get; set; }

        public string? CancellationReason { get; set; }

        public DateTime? CancelledAt { get; set; }

        public bool CanCancel { get; set; }
    }
}
