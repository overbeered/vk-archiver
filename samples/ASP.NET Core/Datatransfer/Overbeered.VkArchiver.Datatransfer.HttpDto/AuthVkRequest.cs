using System.ComponentModel.DataAnnotations;

namespace Overbeered.VkArchiver.Datatransfer.HttpDto;

/// <summary>
/// Модель для запроса на авторизацию
/// </summary>
public class AuthVkRequest
{
    /// <summary>
    /// Логин в VK
    /// </summary>
    [Required]
    public string? Login { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    public string? Password { get; set; }

    /// <summary>
    /// Id приложения
    /// </summary>
    [Required]
    public ulong? ApplicationId { get; set; }
}