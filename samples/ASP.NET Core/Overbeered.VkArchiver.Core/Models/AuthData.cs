namespace Overbeered.VkArchiver.Core.Models;

/// <summary>
/// Модель для авторизации
/// </summary>
public class AuthData
{
    /// <summary>
    /// Токен VK
    /// </summary>
    public string Token { get; private set; }

    public AuthData(string token)
    {
        Token = token;
    }
}