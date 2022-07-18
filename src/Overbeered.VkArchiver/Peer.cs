namespace Overbeered.VkArchiver
{
    /// <summary>
    /// Чат/диалог с файлами
    /// </summary>
    public class Peer
    {
        /// <summary>
        /// Название чата/диалога
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Файлы загрузки
        /// </summary>
        public IEnumerable<FileDownload> FileDownloads { get; set; }

        public Peer(string name, IEnumerable<FileDownload> fileDownloads)
        {
            Name = name;
            FileDownloads = fileDownloads;
        }
    }
}
