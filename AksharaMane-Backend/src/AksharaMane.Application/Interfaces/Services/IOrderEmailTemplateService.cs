using AksharaMane.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Interfaces.Services
{
    public interface IOrderEmailTemplateService
    {
        string BuildOrderPlacedEmail(Order order);

        string BuildOrderStatusEmail(Order order);
    }
}
