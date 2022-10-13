namespace Overbeered.VkArchiver;

/// <summary>
/// URI и имя скачанного файла
/// </summary>
internal class UriFile
{
    /// <summary>
    /// URI файла
    /// </summary>
    public Uri? Uri { get; private set; }

    /// <summary>
    /// Имя файла
    /// </summary>
    public string Name { get; private set; }

    public UriFile(Uri? uri, string name)
    {
        Uri = uri;
        Name = name;
    }
}