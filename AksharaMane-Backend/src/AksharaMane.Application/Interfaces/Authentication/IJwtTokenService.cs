using AksharaMane.Application.Common.Models;
using AksharaMane.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Interfaces.Authentication
{
    public interface IJwtTokenService
    {
        JwtTokenResult GenerateToken(AdminUser adminUser);
    }
}
