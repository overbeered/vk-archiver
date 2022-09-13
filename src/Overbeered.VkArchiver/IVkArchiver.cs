namespace Overbeered.VkArchiver
{
    /// <summary>
    /// Архиватор
    /// </summary>
    public interface IVkArchiver
    {

        /// <summary>
        /// Сохраняет все фотографии из всех чатов/диалогов в зависимости от флага
        /// </summary>
        /// <param name="from">Флаг</param>
        /// <param name="path">Путь для сохранения фотографий</param>
        Task ArchivePhotosAsync(From from, string path);

        /// <summary>
        /// Возвращает фотографию из чата/диалога по названию
        /// </summary>
        /// <param name="name">Название чата/диалога</param>
        /// <param name="path">Путь для сохранения фотографий</param>
        Task ArchivePhotosAsync(string name, string path);
    }
}
