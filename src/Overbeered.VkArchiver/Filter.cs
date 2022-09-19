namespace Overbeered.VkArchiver
{
    /// <summary>
    /// Фильтр
    /// </summary>
    internal class Filter
    {
        /// <summary>
        /// Фильтр спецсимволов для создания директории
        /// </summary>
        /// <param name="str">Строка, для фильтрации</param>
        /// <returns>Отфильтрованная строка</returns>
        public static string Directories(string str)
        {
            var charsToRemove = new List<char>() { '/', ':', '*', '?', '"', '<', '>', '|', '.' };
            charsToRemove.ForEach(c => str = str.Replace(c.ToString(), String.Empty));
            str = str.Insert(str.IndexOf('\\'), ":");
            return str;
        }
    }
}
