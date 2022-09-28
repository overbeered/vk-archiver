namespace Overbeered.VkArchiver;

/// <summary>
/// Архиватор
/// </summary>
public interface IVkArchiver
{
    /// <summary>
    /// Токен
    /// </summary>
    public string? Token { get; set; }

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
    /// Выйти из системы
    /// </summary>
    Task LogOutAsync();

    /// <summary>
    /// Сохраняет все файлы из всех чатов/диалогов в зависимости от флага и типа медии
    /// </summary>
    /// <param name="path">Путь для сохранения фотографий</param>
    /// <param name="fromPeer">Флаг</param>
    /// <param name="fromMedia">Тип медиа</param>
    Task ArchiveAsync(string path, FromPeer fromPeer = FromPeer.All, FromMedia fromMedia = FromMedia.Photo);

    /// <summary>
    /// Сохраняет все файлы по названию чата/диалога в зависимости от типа меди
    /// </summary>
    /// <param name="path">Путь для сохранения файлов</param>
    /// <param name="name">Название чата/диалога</param>
    /// <param name="fromMedia">Тип медиа</param>
    Task ArchiveAsync(string path, string name, FromMedia fromMedia = FromMedia.Photo);

    /// <summary>
    /// Создает билдер
    /// </summary>
    /// <returns>Билдер</returns>
    VkArchiverBuilder CreateBuilder();
}