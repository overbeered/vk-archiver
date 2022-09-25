using VkNet;

namespace Overbeered.VkArchiver;

/// <summary>
/// Архиватор
/// </summary>
public interface IVkArchiver
{
    /// <summary>
    /// E-mail или телефон
    /// </summary>
    public string? Login { get; set; }

    /// <summary>
    /// Пароль для авторизации
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// ID приложения
    /// </summary>
    public ulong? ApplicationId { get; set; }

    /// <summary>
    /// Токен
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Сохраняет все файлы из всех чатов/диалогов в зависимости от флага и типа медии
    /// </summary>
    /// <param name="path">Путь для сохранения фотографий</param>
    /// <param name="fromPeer">Флаг</param>
    /// <param name="fromMedia">Тип медиа</param>
    Task ArchiveAsync(string path, FromPeer fromPeer = FromPeer.All, FromMedia fromMedia = FromMedia.Photo);

    /// <summary>
    /// Сохраняет все файлы в зависимости от флага и типа меди по названию чата/диалога
    /// </summary>
    /// <param name="path">Путь для сохранения файлов</param>
    /// <param name="name">Название чата/диалога</param>
    /// <param name="fromMedia">Тип медиа</param>
    Task ArchiveAsync(string path, string name, FromMedia fromMedia = FromMedia.Photo);

    /// <summary>
    /// Авторизация
    /// </summary>
    Task AuthorizeAsync();

    /// <summary>
    /// Авторизация по токену
    /// </summary>
    /// <param name="token">Токен</param>
    Task AuthorizeAsync(string token);

    /// <summary>
    /// Создает билдер
    /// </summary>
    /// <returns>Билдер</returns>
    VkArchiverBuilder CreateBuilder();
}