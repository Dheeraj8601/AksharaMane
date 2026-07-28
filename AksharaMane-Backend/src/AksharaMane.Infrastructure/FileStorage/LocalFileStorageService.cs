using AksharaMane.Application.Common.Exceptions;
using AksharaMane.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace AksharaMane.Infrastructure.FileStorage
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;

        private static readonly string[] AllowedExtensions =
        [
            ".jpg",
        ".jpeg",
        ".png",
        ".webp"
        ];

        private const long MaximumFileSize = 5 * 1024 * 1024;

        public LocalFileStorageService(
            IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadAsync(
            IFormFile file,
            string folderName,
            CancellationToken cancellationToken = default)
        {
            if (file.Length == 0)
            {
                throw new BadRequestException(
                    "The uploaded file is empty.");
            }

            if (file.Length > MaximumFileSize)
            {
                throw new BadRequestException(
                    "Image size cannot exceed 5 MB.");
            }

            var extension =
                Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
            {
                throw new BadRequestException(
                    "Only JPG, JPEG, PNG and WEBP images are allowed.");
            }

            var webRootPath = _environment.WebRootPath;

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(
                    _environment.ContentRootPath,
                    "wwwroot");
            }

            var uploadFolder = Path.Combine(
                webRootPath,
                "uploads",
                folderName);

            Directory.CreateDirectory(uploadFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";

            var absolutePath = Path.Combine(
                uploadFolder,
                fileName);

            await using var stream = new FileStream(
                absolutePath,
                FileMode.Create);

            await file.CopyToAsync(stream, cancellationToken);

            return $"/uploads/{folderName}/{fileName}";
        }

        public void Delete(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return;
            }

            var webRootPath = _environment.WebRootPath;

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(
                    _environment.ContentRootPath,
                    "wwwroot");
            }

            var cleanPath = relativePath
                .TrimStart('/')
                .Replace('/', Path.DirectorySeparatorChar);

            var absolutePath = Path.Combine(
                webRootPath,
                cleanPath);

            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }
        }
    }
}
