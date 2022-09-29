using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Overbeered.VkArchiver.Core.Models;

namespace Overbeered.VkArchiver.Services.AuthService.Filters;

/// <summary>
/// Фильтр авторизации
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext filterContext)
    {
        var user = filterContext.HttpContext.Items[nameof(AuthData)];

        if (user == null)
        {
            filterContext.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}