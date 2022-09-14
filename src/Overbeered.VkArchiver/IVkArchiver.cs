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
        /// <param name="path">Путь для сохранения фотографий</param>
        /// <param name="fromPeer">Флаг</param>
        /// <param name="fromMedia">Тип медиа</param>
        Task ArchiveAsync(string path, FromPeer fromPeer = FromPeer.All, FromMedia fromMedia = FromMedia.Photo);

        /// <summary>
        /// Возвращает фотографию из чата/диалога по названию
        /// </summary>
        /// <param name="path">Путь для сохранения фотографий</param>
        /// <param name="name">Название чата/диалога</param>
        /// <param name="fromMedia">Тип медиа</param>
        Task ArchiveAsync(string path, string name, FromMedia fromMedia = FromMedia.Photo);
    }
}
