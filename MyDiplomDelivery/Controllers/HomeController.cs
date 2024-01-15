using Microsoft.AspNetCore.Mvc;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels;
using System.Data;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using MyDiplomDelivery.Contexts;
using Microsoft.EntityFrameworkCore;

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


        [HttpPost]
        public async Task<IActionResult> GetOrderDetail([FromBody] OrderRequest request)
        {
            if (!string.IsNullOrEmpty(request?.Input))
            {
                var order = await _applicationContext.Order.FirstOrDefaultAsync(order => order.Number == request!.Input);
                if (order == null)
                {
                    return NotFound();
                }
                var viewModel = new OrderResponse
                {
                    Comment = order.Comment,
                    Status = order.Status.ToString(),
                };
                return Ok(viewModel);
            }
            return BadRequest();
        }

        
        public IActionResult Index()
        {
            return View();
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