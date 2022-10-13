using Overbeered.VkArchiver.Core.Enum;

namespace Overbeered.VkArchiver.Datatransfer.HttpDto;

/// <summary>
/// Модель для запроса на архивацию файлов
/// </summary>
public class VkArchiverRequest
{
    /// <summary>
    /// Путь для сохранения файлов
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Тип медиа
    /// </summary>
    public FromMedia FromMedia { get; set; }

    /// <summary>
    /// Флаг чата/диалога 
    /// </summary>
    public FromPeer FromPeer { get; set; }
}