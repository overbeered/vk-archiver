using Overbeered.VkArchiver.Core.Models;
using Overbeered.VkArchiver.Core.Services;

namespace Overbeered.VkArchiver.Services.AuthService.Services;

public class AuthService : IAuthService
{
    private readonly IVkArchiver _vkArchiver;

    public AuthService(IVkArchiver vkArchiver)
    {
        _vkArchiver = vkArchiver;
    }

    public async Task<AuthData> CreateTokenAsync(string login, string password, ulong applicationId)
    {
        var vk = _vkArchiver.CreateBuilder().SetLogin(login).SetPassword(password).SetApplicationId(applicationId).Build();
        await vk.AuthorizeAsync();
        var token = vk.Token;
        await vk.LogOutAsync();

        return new AuthData(token!);
    }
}