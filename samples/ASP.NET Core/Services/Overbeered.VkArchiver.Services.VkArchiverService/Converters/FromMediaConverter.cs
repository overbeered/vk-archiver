namespace Overbeered.VkArchiver.Services.VkArchiverService.Converters;

internal static class FromMediaConverter
{
    public static Core.Enum.FromMedia? Converter(FromMedia fromMedia)
    {
        return fromMedia switch
        {
            FromMedia.Photo => Core.Enum.FromMedia.Photo,
            FromMedia.Doc => Core.Enum.FromMedia.Doc,
            _ => null,
        };
    }

    public static FromMedia? Converter(Core.Enum.FromMedia fromMedia)
    {
        return fromMedia switch
        {
            Core.Enum.FromMedia.Photo => FromMedia.Photo,
            Core.Enum.FromMedia.Doc => FromMedia.Doc,
            _ => null,
        };
    }
}