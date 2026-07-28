using AksharaMane.Application.Common.Models;
using AksharaMane.Application.Interfaces.Services;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;


namespace AksharaMane.Infrastructure.Email
{
    public class MailKitEmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<MailKitEmailService> _logger;

        public MailKitEmailService(
            IOptions<EmailSettings> options,
            ILogger<MailKitEmailService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task SendAsync(
            EmailMessage message,
            CancellationToken cancellationToken = default)
        {
            ValidateSettings();

            var email = new MimeMessage();

            email.From.Add(
                new MailboxAddress(
                    _settings.FromName,
                    _settings.FromEmail));

            email.To.Add(
                MailboxAddress.Parse(message.ToEmail));

            email.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message.HtmlBody
            };

            email.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();

            try
            {
                var socketOption = _settings.UseSsl
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.Auto;

                await smtpClient.ConnectAsync(
                    _settings.Host,
                    _settings.Port,
                    socketOption,
                    cancellationToken);

                await smtpClient.AuthenticateAsync(
                    _settings.Username,
                    _settings.Password,
                    cancellationToken);

                await smtpClient.SendAsync(
                    email,
                    cancellationToken);

                await smtpClient.DisconnectAsync(
                    true,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Failed to send email to {Email}",
                    message.ToEmail);

                throw;
            }
        }

        private void ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(_settings.Host))
            {
                throw new InvalidOperationException(
                    "Email host is missing.");
            }

            if (string.IsNullOrWhiteSpace(_settings.Username))
            {
                throw new InvalidOperationException(
                    "Email username is missing.");
            }

            if (string.IsNullOrWhiteSpace(_settings.Password))
            {
                throw new InvalidOperationException(
                    "Email password is missing.");
            }

            if (string.IsNullOrWhiteSpace(_settings.FromEmail))
            {
                throw new InvalidOperationException(
                    "Sender email is missing.");
            }
        }
    }
}
