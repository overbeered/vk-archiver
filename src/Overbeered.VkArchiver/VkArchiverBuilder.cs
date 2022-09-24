namespace Overbeered.VkArchiver;

/// <summary>
/// Билдер для вк Архиватора
/// </summary>
public class VkArchiverBuilder
{
    private VkArchiver _vkArchiver;

    public VkArchiverBuilder()
    {
        _vkArchiver = new VkArchiver();
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