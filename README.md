# **vk-archiver**

Приложение использует библиотеку [VkNet.AudioBypass](https://github.com/flowersne/VkNet.AudioBypass), расширение библиотеки [Vk Api for .NET](https://github.com/vknet/vk) для обхода ограничения к методам Audio и Messages.

## **Что делает?**

Приложение создает архив по названию диалога или беседы с загруженными фотографиями или документами. 

## **Как работает ?**

В консоли:

Без логгера
```c#
...
var vk = new VkArchiver().CreateBuilder().SetLogin("login").SetPassword("password").SetApplicationId(8206863).Build();

await vk.AuthorizeAsync();
await vk.ArchiveAsync(@"D:\test");
...
```
С логгером [Serilog](https://serilog.net/)
```c#
...
using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var vk = new VkArchiver(log).CreateBuilder().SetLogin("login").SetPassword("password").SetApplicationId(8206863).Build();

await vk.AuthorizeAsync();
await vk.ArchiveAsync(@"D:\test", "Name");
...
```
В ASP Startup:
```c#
...
services.AddTransient<IVkArchiver, VkArchiver>();
...
```
**Примеры приложений:**
* [Консольное приложение](https://github.com/overbeered/vk-archiver/tree/feature/2-design-archiver-interface/samples/Console)  
* [Веб приложение](https://github.com/overbeered/vk-archiver/tree/feature/2-design-archiver-interface/samples/ASP.NET%20Core)