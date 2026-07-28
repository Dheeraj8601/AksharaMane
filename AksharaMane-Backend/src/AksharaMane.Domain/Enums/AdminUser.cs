using AksharaMane.Domain.Common;

namespace AksharaMane.Domain.Entities;

public class AdminUser : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "Admin";

    public bool IsActive { get; set; } = true;
}