using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Overbeered.VkArchiver;

/// <summary>
/// Билдер для вк Архиватора
/// </summary>
public class VkArchiverBuilder
{
    private VkArchiver _vkArchiver;

    public VkArchiverBuilder() : this(NullLogger<VkArchiver>.Instance)
    {
    }

    public VkArchiverBuilder(ILogger<VkArchiver> logger)
    {
        _vkArchiver = new VkArchiver(logger);
    }

    public VkArchiverBuilder SetLogin(string login)
    {
        _vkArchiver.Login = login;
        return this;
    }

    public VkArchiverBuilder SetPassword(string password)
    {
        _vkArchiver.Password = password;
        return this;
    }

    public VkArchiverBuilder SetApplicationId(ulong applicationId)
    {
        _vkArchiver.ApplicationId = applicationId;
        return this;
    }

    public VkArchiver Build()
    {
        return _vkArchiver;
    }
}