using Overbeered.VkArchiver.Core.Enum;

namespace Overbeered.VkArchiver.Core.Models;

/// <summary>
/// Модель для архиватора
/// </summary>
public class VkArchive
{
    /// <summary>
    /// Токен
    /// </summary>
    public string? Token { get; private set; }

    /// <summary>
    /// Путь для сохранения файлов
    /// </summary>
    public string? Path { get; private set; }

    /// <summary>
    /// Тип медиа
    /// </summary>
    public FromMedia FromMedia { get; private set; }

    /// <summary>
    /// Флаг чата/диалога 
    /// </summary>
    public FromPeer FromPeer { get; private set; }

    public VkArchive(string? token, string? path, FromMedia fromMedia, FromPeer fromPeer)
    {
        Token = token;
        Path = path;
        FromMedia = fromMedia;
        FromPeer = fromPeer;
    }
}