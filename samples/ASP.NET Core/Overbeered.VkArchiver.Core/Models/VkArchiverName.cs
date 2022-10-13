using Overbeered.VkArchiver.Core.Enum;

namespace Overbeered.VkArchiver.Core.Models;

/// <summary>
/// Модель для архиватора по названию чата/диалога
/// </summary>
public class VkArchiverName
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
    /// Название чата/диалога
    /// </summary>
    public string? Name { get; private set; }

    /// <summary>
    /// Тип медиа
    /// </summary>
    public FromMedia FromMedia { get; private set; }

    public VkArchiverName(string? token, string? path, string? name, FromMedia fromMedia)
    {
        Token = token;
        Path = path;
        Name = name;
        FromMedia = fromMedia;
    }
}