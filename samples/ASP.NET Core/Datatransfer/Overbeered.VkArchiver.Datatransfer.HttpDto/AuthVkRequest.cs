namespace Overbeered.VkArchiver.Datatransfer.HttpDto;

/// <summary>
/// Модель для запроса на авторизацию
/// </summary>
public class AuthVkRequest
{
    /// <summary>
    /// Логин в VK
    /// </summary>
    public string? Login { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Id приложения
    /// </summary>
    public ulong? ApplicationId { get; set; }
}