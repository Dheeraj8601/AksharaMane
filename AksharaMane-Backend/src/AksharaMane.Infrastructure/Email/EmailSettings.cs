using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Infrastructure.Email
{
    public class EmailSettings
    {
        public const string SectionName = "EmailSettings";

        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FromEmail { get; set; } = string.Empty;

        public string FromName { get; set; } = "AksharaMane";

        public bool UseSsl { get; set; } = true;
    }
}
