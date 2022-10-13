using Microsoft.AspNetCore.Mvc;
using Overbeered.VkArchiver.Core.Models;
using Overbeered.VkArchiver.Core.Services;
using Overbeered.VkArchiver.Datatransfer.HttpDto;
using Overbeered.VkArchiver.Services.AuthService.Filters;

namespace Overbeered.VkArchiver.MVC.Controllers;

/// <summary>
/// Контроллер для архивации 
/// </summary>
public class ArchiverController : Controller
{
    private readonly ILogger<ArchiverController> _logger;
    private readonly IVkArchiverService _vkArchiverService;

    public ArchiverController(ILogger<ArchiverController> logger, IVkArchiverService vkArchiverService)
    {
        _logger = logger;
        _vkArchiverService = vkArchiverService;
    }

    /// <summary>
    /// Стартовая страница для архивации 
    /// </summary>
    /// <returns>Стартовая страница для архивации</returns>
    [Authorize]
    [HttpGet("/archiver")]
    public IActionResult Index() => View();

    /// <summary>
    /// Архивация файлов VK
    /// </summary>
    /// <param name="vkArchiverRequest">Модель для запроса на архивацию файлов</param>
    /// <returns>Файлы архивированный</returns>
    /// <response code="200">Файлы архивированный</response>
    /// <response code="400">Значения невалидны</response>
    /// <response code="500">Ошибка на стороне сервера</response>
    [Authorize]
    [HttpPost("/archiver/archive")]
    public async Task<IActionResult> ArchiveAsync([FromForm] VkArchiverRequest vkArchiverRequest)
    {
        var user = (AuthData)HttpContext.Items[nameof(AuthData)]!;

        if (vkArchiverRequest.Path == null) return BadRequest();

        try
        {
            await _vkArchiverService.ArchiveAsync(new VkArchive(user.Token,
                vkArchiverRequest.Path,
                vkArchiverRequest.FromMedia,
                vkArchiverRequest.FromPeer));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in controller {controllerName} in method {methodName}.",
                 nameof(ArchiverController), nameof(ArchiveAsync));
            
            return StatusCode(500);
        }

        return Ok();
    }

    /// <summary>
    ///  Архивация файлов VK по названию чата/диалога
    /// </summary>
    /// <param name="vkArchiverNameRequest">Модель для запроса на архивацию файлов по названию чата/диалога</param>
    /// <returns>Файлы архивированный</returns>
    /// <response code="200">Файлы архивированный</response>
    /// <response code="400">Значения невалидны</response>
    /// <response code="500">Ошибка на стороне сервера</response>
    [Authorize]
    [HttpPost("/archiver/archive-name")]
    public async Task<IActionResult> ArchiveNameAsync([FromForm] VkArchiverNameRequest vkArchiverNameRequest)
    {
        var user = (AuthData)HttpContext.Items[nameof(AuthData)]!;
        
        if (vkArchiverNameRequest.Path == null) return BadRequest();
        if (vkArchiverNameRequest.Name == null) return BadRequest();

        try
        {
            await _vkArchiverService.ArchiveAsync(new VkArchiverName(user.Token,
                vkArchiverNameRequest.Path,
                vkArchiverNameRequest.Name,
                vkArchiverNameRequest.FromMedia));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in controller {controllerName} in method {methodName}.",
                 nameof(ArchiverController), nameof(ArchiveNameAsync));
            
            return StatusCode(500);
        }

        return Ok();
    }
}