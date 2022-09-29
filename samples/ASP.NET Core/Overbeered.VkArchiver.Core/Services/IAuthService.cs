using Overbeered.VkArchiver.Core.Models;

namespace Overbeered.VkArchiver.Core.Services;

/// <summary>
/// Сервис для авторизации
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Создания VK токена
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <param name="applicationId">Id приложения</param>
    /// <returns>Модель для авторизации</returns>
    Task<AuthData> CreateTokenAsync(string login, string password, ulong applicationId);
}