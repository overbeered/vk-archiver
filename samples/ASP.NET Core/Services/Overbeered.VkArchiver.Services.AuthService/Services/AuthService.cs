using Overbeered.VkArchiver.Core.Models;
using Overbeered.VkArchiver.Core.Services;

namespace Overbeered.VkArchiver.Services.AuthService.Services;

public class AuthService : IAuthService
{
    private readonly VkArchiverBuilder _vkBuilder;

    public AuthService(VkArchiverBuilder vkBuilder)
    {
        _vkBuilder = vkBuilder;
    }

    public async Task<AuthData> CreateTokenAsync(string login, string password, ulong applicationId)
    {
        var vk = _vkBuilder.SetLogin(login)
            .SetPassword(password)
            .SetApplicationId(applicationId)
            .Build();
        
        await vk.AuthorizeAsync();
        var token = vk.Token;
        await vk.LogOutAsync();

        return new AuthData(token!);
    }
}