namespace Overbeered.VkArchiver
{
    /// <summary>
    /// Файл загрузки
    /// </summary>
    public class FileDownload
    {
        /// <summary>
        /// Название файла с расширением
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Поток файла
        /// </summary>
        public Stream Stream { get; private set; }

        public FileDownload(string name, Stream stream)
        {
            Name = name;
            Stream = stream;
        }
    }
}
