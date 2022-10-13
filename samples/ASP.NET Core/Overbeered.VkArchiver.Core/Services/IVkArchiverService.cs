using Overbeered.VkArchiver.Core.Models;

namespace Overbeered.VkArchiver.Core.Services;

/// <summary>
/// Сервис для архивации файлов VK
/// </summary>
public interface IVkArchiverService
{
    /// <summary>
    /// Сохраняет все файлы из всех чатов/диалогов в зависимости от флага и типа медии
    /// </summary>
    /// <param name="vkArchive"></param>
    Task ArchiveAsync(VkArchive vkArchive);

    /// <summary>
    /// Сохраняет все файлы в зависимости от типа меди и названия чата/диалога
    /// </summary>
    /// <param name="vkArchiverName"></param>
    Task ArchiveAsync(VkArchiverName vkArchiverName);
}