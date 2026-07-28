using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.DTOs.Order
{
    public class TrackOrderRequestDto
    {
        [Required]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        public string EmailOrPhone { get; set; } = string.Empty;
    }
}
