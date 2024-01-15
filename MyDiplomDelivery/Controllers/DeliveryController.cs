using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels;

namespace MyDiplomDelivery.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserManager<User> _userManager;

        public DeliveryController(
            ApplicationContext applicationContext,
            UserManager<User> userManager)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //var roles = await _applicationContext.Roles.Where(t => t.Name =="deliv").ToListAsync();
            //var userRoles = await _applicationContext.UserRoles.ToListAsync();
            //var users = await _applicationContext.Users.ToListAsync();

            //var deliv =;
            //var Orders = await _applicationContext.Users.Where(t => t.);

            //List<string> dman = new List<string>();
            //foreach (var userRole in userRoles)
            //{
            //    foreach (var user in users)
            //        if (userRole. == )
            //}
            //dman = 

            var orders = await _applicationContext.Order.Where(t => t.Status== StatusType.Todo).ToListAsync();
            List<string> ord = new List<string>();
            foreach (var order in orders) {
                ord.Add(order.Name);
            }
            ViewBag.ord = new SelectList(ord);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var log = new Order
                {
                    Number = Guid.NewGuid().ToString("N"),
                    Name = model.Name,
                    Description = model.Description,
                    From = model.From,
                    To = model.To,
                    Status = StatusType.Todo
                };

                await _applicationContext.Order.AddAsync(log);
                await _applicationContext.SaveChangesAsync();

                var order = await _applicationContext.Order.FirstOrDefaultAsync(order => order.Number == log.Number);

                return RedirectToAction("Success", "Order", new { id = order.Id });
            }

            return View(model);
        }

    }
}
