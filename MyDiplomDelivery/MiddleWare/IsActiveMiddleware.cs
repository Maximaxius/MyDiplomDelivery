using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyDiplomDelivery.Contexts;
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

                var accountHomeIndexPath = "/";
                var accountHomeGetOrderDetailPath = "/Home/GetOrderDetail"; 
                var accountAccessDeniedPath = "/Account/AccessDenied";
                var accountAccountLogoutPath = "/Account/Logout";
                var accountOrderCreatePath = "/Order/Create";

                var applicationContext = context.RequestServices.GetService(typeof(ApplicationContext)) as ApplicationContext;
                int num = await applicationContext.Order.CountAsync();      //Кривое доступ к созданию заказа
                var accountOrderSuccessPath = $"/Order/Success/{num}";

                var ExcludePaths = new List<string> { accountHomeGetOrderDetailPath, accountHomeIndexPath, accountAccessDeniedPath, accountAccountLogoutPath, accountOrderCreatePath, accountOrderSuccessPath };

                if (!user.IsActive && !ExcludePaths.Contains(context.Request.Path))
                {                    
                    // Пользователь не активен, перенаправляем на страницу "AccessDenied"
                    context.Response.Redirect("/Account/AccessDenied");

                    return;
                }

            }

            await _next(context);
        }
    }
}
