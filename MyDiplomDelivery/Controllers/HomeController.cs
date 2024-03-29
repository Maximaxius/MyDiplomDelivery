using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.ViewModels;
using MyDiplomDelivery.ViewModels.Home;
using System.Diagnostics;

namespace MyDiplomDelivery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _applicationContext;

        public HomeController(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderDetail([FromBody] OrderRequestViewModel request)
        {
            if (!string.IsNullOrEmpty(request?.Input))
            {
                var orderNumber = request!.Input.Trim();
                var order = await _applicationContext.Order.FirstOrDefaultAsync(x => x.Number == orderNumber);
                if (order == null)
                {
                    return NotFound();
                }

                var viewModel = new OrderResponseViewModel
                {
                    Comment = order.Comment,
                    Status = order.Status.ToString(),
                };

                return Ok(viewModel);
            }

            return BadRequest();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}