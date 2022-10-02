using Microsoft.Extensions.Logging;

namespace Overbeered.VkArchiver.Console;

internal class Program
{
    static async Task Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug)
                .AddConsole();
        });
        var logVk = loggerFactory.CreateLogger<VkArchiver>();
        var logProgram = loggerFactory.CreateLogger<Program>();
        string login;
        string password;
        string name;
        FromMedia? media;
        var vkBuilder = new VkArchiverBuilder(logVk);

        logProgram.LogInformation("Введите login:");
        login = System.Console.ReadLine()!;
        logProgram.LogInformation("Введите password:");
        password = System.Console.ReadLine()!;
        logProgram.LogInformation("Введите name:");
        name = System.Console.ReadLine()!;
        logProgram.LogInformation("Введите mediaType (photo или doc):");
        media = FromMediaConverter.Converter(System.Console.ReadLine()!)!;

        var vk = vkBuilder.SetLogin(login)
            .SetPassword(password)
            .SetApplicationId(8206863)
            .Build();
        
        await vk.AuthorizeAsync();
        await vk.ArchiveAsync(@"D:\test", name, media.Value);
        await vk.LogOutAsync();
    }
}