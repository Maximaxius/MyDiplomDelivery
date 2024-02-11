using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyDiplomDelivery.Models;
using System.Data;

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

                //if (!user.IsActive && context.Request.Path != "/Account/AccessDenied" && !context.User.IsInRole("User"))
                //{
                //    //if (context.User.IsInRole("DeliveryMan") || context.User.IsInRole("Manager"))
                //    //{
                //    //    context.Response.Redirect("/Account/Logout");
                //    //    return;
                //    //}
                //    // Пользователь не активен, перенаправляем на страницу "AccessDenied"
                //    context.Response.Redirect("/Account/AccessDenied");
                                        
                //    return;                    
                //}

                //if (context.Request.Path == "/Account/AccessDenied")
                //{
                //    context.Response.Redirect("/Account/Logout");
                //    context.Response.Redirect("/");
                //    return;
                //}

            }

            await _next(context);
        }
    }
}
