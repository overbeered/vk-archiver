using Overbeered.VkArchiver.Core.Enum;

namespace Overbeered.VkArchiver.Datatransfer.HttpDto;

/// <summary>
/// Модель для запроса на архивацию файлов по названию чата/диалога
/// </summary>
public class VkArchiverNameRequest
{
    /// <summary>
    /// Путь для сохранения файлов
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Название чата/диалога
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Тип медиа
    /// </summary>
    public FromMedia FromMedia { get; set; }
}