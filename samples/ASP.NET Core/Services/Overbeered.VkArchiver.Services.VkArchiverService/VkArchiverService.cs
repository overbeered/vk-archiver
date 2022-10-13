using Overbeered.VkArchiver.Core.Models;
using Overbeered.VkArchiver.Core.Services;
using Overbeered.VkArchiver.Services.VkArchiverService.Converters;

namespace Overbeered.VkArchiver.Services.VkArchiverService;

public class VkArchiverService : IVkArchiverService
{
    private readonly IVkArchiver _vkArchiver;

    public VkArchiverService(IVkArchiver vkArchiver)
    {
        _vkArchiver = vkArchiver;
    }

    public async Task ArchiveAsync(VkArchive vkArchive)
    {
        await _vkArchiver.AuthorizeAsync(vkArchive.Token!);
        await _vkArchiver.ArchiveAsync(vkArchive.Path!,
            FromPeerConverter.Converter(vkArchive.FromPeer)!.Value,
            FromMediaConverter.Converter(vkArchive.FromMedia)!.Value);

        await _vkArchiver.LogOutAsync();
    }

    public async Task ArchiveAsync(VkArchiverName vkArchiverName)
    {
        await _vkArchiver.AuthorizeAsync(vkArchiverName.Token!);
        await _vkArchiver.ArchiveAsync(vkArchiverName.Path!,
            vkArchiverName.Name!,
            FromMediaConverter.Converter(vkArchiverName.FromMedia)!.Value);

        await _vkArchiver.LogOutAsync();
    }
}