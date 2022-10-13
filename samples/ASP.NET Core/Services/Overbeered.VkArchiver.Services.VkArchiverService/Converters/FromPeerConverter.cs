namespace Overbeered.VkArchiver.Services.VkArchiverService.Converters;

internal static class FromPeerConverter
{
    public static Core.Enum.FromPeer? Converter(FromPeer fromPeer)
    {
        return fromPeer switch
        {
            FromPeer.All => Core.Enum.FromPeer.All,
            FromPeer.Chats => Core.Enum.FromPeer.Chats,
            FromPeer.Dialogs => Core.Enum.FromPeer.Dialogs,
            _ => null,
        };
    }

    public static FromPeer? Converter(Core.Enum.FromPeer fromPeer)
    {
        return fromPeer switch
        {
            Core.Enum.FromPeer.All => FromPeer.All,
            Core.Enum.FromPeer.Chats => FromPeer.Chats,
            Core.Enum.FromPeer.Dialogs => FromPeer.Dialogs,
            _ => null,
        };
    }
}

