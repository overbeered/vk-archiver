using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Overbeered.VkArchiver.Core.Models;

namespace Overbeered.VkArchiver.Services.AuthService.Middlewares;

/// <summary>
/// Проверка JWT токена
/// </summary>
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtMiddleware> _logger;

    public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var cookieToken = context.Request.Headers["Cookie"].FirstOrDefault()?.Split(" ").Last();

        if (cookieToken != null)
        {
            context.Items[nameof(AuthData)] = new AuthData(cookieToken);
        }

        await _next(context);
    }
}