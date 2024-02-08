using Microsoft.AspNetCore.Mvc;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels;
using System.Data;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using MyDiplomDelivery.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MyDiplomDelivery.ViewModels.Home;

namespace MyDiplomDelivery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _applicationContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationContext applicationContext)
        {
            _logger = logger;
            _applicationContext = applicationContext;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetOrderDetail([FromBody] OrderRequestViewModel request)
        {
            if (!string.IsNullOrEmpty(request?.Input))
            {
                var orderNumber = request!.Input.Trim();
                var order = await _applicationContext.Order.FirstOrDefaultAsync(order => order.Number == orderNumber);
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
    }
}