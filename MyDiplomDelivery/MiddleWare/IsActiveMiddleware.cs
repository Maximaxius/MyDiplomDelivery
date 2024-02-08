using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyDiplomDelivery.Models;

namespace MyDiplomDelivery.MiddleWare
{
    public class IsActiveMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IsActiveMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                // Получаем информацию о пользователе, включая статус IsActive
                using var scope = _serviceScopeFactory.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var user = await userManager.GetUserAsync(context.User);

                //var isActive = context.User.FindFirst("IsActive")?.Value;

                //if (!user.IsActive && context.Request.Path != "/Account/AccessDenied")
                //{
                //    // Пользователь не активен, перенаправляем на страницу "AccessDenied"
                //    context.Response.Redirect("/Account/AccessDenied");
                //    return;
                //}
            }

            await _next(context);
        }
    }
}
