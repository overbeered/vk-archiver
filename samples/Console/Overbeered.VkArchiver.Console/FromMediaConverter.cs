namespace Overbeered.VkArchiver.Console;

internal class FromMediaConverter
{
    public static FromMedia? Converter(string media)
    {
        return media.ToLower() switch
        {
            "photo" => FromMedia.Photo,
            "doc" => FromMedia.Doc,
            _ => null,
        };
    }
}
