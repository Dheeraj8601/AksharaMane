using AksharaMane.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessage message,CancellationToken cancellationToken = default);
    }
}
