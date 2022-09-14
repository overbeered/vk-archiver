using VkNet.Enums.SafetyEnums;

namespace Overbeered.VkArchiver.Converters
{
    internal static class MediaTypeConverter
    {
        public static MediaType? Converter(FromMedia fromMedia)
        {
            return fromMedia switch
            {
                FromMedia.Photo => MediaType.Photo,
                FromMedia.Doc => MediaType.Doc,
                _ => null,
            };
        }
    }
}
