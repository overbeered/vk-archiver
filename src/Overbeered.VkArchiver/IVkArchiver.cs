namespace Overbeered.VkArchiver
{
    /// <summary>
    /// Архиватор
    /// </summary>
    public interface IVkArchiver
    {
        /// <summary>
        /// Возвращает все фотографии из всех чатов/диалогов в зависимости от флага
        /// </summary>
        /// <param name="from">Флаг</param>
        /// <returns>Чаты/диалоги с файлами</returns>
        Task<IEnumerable<Peer>> ArchivePhotosAsync(From from);

        /// <summary>
        /// Возвращает фотографию из чата/диалога по его названию
        /// </summary>
        /// <param name="name">Название чата/диалога</param>
        /// <returns>Чат/диалог с файлами</returns>
        Task<Peer?> ArchivePhotosAsync(string name);
    }
}
