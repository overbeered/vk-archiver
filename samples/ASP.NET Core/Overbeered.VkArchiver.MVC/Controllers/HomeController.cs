using Microsoft.AspNetCore.Mvc;
using Overbeered.VkArchiver.Core.Services;
using Overbeered.VkArchiver.Datatransfer.HttpDto;

namespace Overbeered.VkArchiver.MVC.Controllers;

// +79228121838
// 8206863

/// <summary>
/// Стартовый контроллер
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAuthService _authService;

    public HomeController(IAuthService authService, ILogger<HomeController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public IActionResult Index() => View();

    /// <summary>
    /// Авторизация
    /// </summary>
    /// <param name="authVkRequest">Модель для запроса на архивацию файлов</param>
    /// <returns>VK токен для авторизации</returns>
    /// <response code="200">Токин</response>
    /// <response code="400">Значения невалидны</response>
    /// <response code="500">Ошибка на стороне сервера</response>
    [HttpPost("/home/login")]
    public async Task<IActionResult> LoginAsync([FromForm] AuthVkRequest authVkRequest)
    {
        if (authVkRequest.Login == null) return BadRequest();
        if (authVkRequest.Password == null) return BadRequest();
        if (authVkRequest.ApplicationId == null) return BadRequest();
        try
        {
            var auth = await _authService.CreateTokenAsync(authVkRequest.Login, authVkRequest.Password, authVkRequest.ApplicationId.Value);

            var response = new
            {
                access_token = auth.Token,
            };

            return Json(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in controller {controllerName} in method {methodName}.",
                 nameof(HomeController), nameof(LoginAsync));

            return StatusCode(500);
        }
    }
}