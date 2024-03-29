using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels.O;

namespace MyDiplomDelivery.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class OrderController : Controller
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserManager<User> _userManager;

        public OrderController(
            ApplicationContext applicationContext,
            UserManager<User> userManager)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var orders = await _applicationContext.Order.ToListAsync();

            var collection = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                var viewModel = new OrderViewModel
                {
                    Id = order.Id,
                    Number = order.Number,
                    Name = order.Name,
                    Description = order.Description,
                    From = order.From,
                    To = order.To,
                    Status = order.Status,
                    Comment = order.Comment
                };

                collection.Add(viewModel);
            }

            return View(collection);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = null;
                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    userId = user.Id;
                }

                var order = new Order
                {
                    Number = Guid.NewGuid().ToString("N"),
                    Name = model.Name,
                    Description = model.Description,
                    From = model.From,
                    To = model.To,
                    Status = StatusType.Todo,
                    UserId = userId
                };

                await _applicationContext.Order.AddAsync(order);
                await _applicationContext.SaveChangesAsync();

                var selectedOrder = await _applicationContext.Order.FirstOrDefaultAsync(x => x.Number == order.Number);

                TempData["num"] = order.Number;

                return RedirectToAction("Success", "Order");
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Success()
        {
            if (TempData.TryGetValue("num", out var data))
            {
                var num = data.ToString();

                var viewModel = new SuccessViewModel
                {
                    Number = num
                };

                return View(viewModel);
            }

            return RedirectToAction("Create", "Order");
        }

        public async Task<IActionResult> Delete(string num)
        {
            var order = await _applicationContext.Order.FirstOrDefaultAsync(x => x.Number == num);
            if (order != null)
            {
                _applicationContext.Order.Remove(order);
                await _applicationContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string num)
        {
            var order = await _applicationContext.Order.FirstOrDefaultAsync(x => x.Number == num);
            if (order != null)
            {
                var viewModel = new OrderEditOrderViewModel
                {
                    Name = order.Name,
                    Description = order.Description,
                    From = order.From,
                    To = order.To,
                    Status = order.Status,
                    Number = order.Number,
                    UserId = order.UserId,
                    Id = order.Id,
                    Comment = order.Comment,
                };

                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderEditOrderViewModel newOrder)
        {
            var selectedOrder = await _applicationContext.Order.FirstOrDefaultAsync(x => x.Id == newOrder.Id);

            selectedOrder.Name = newOrder.Name;
            selectedOrder.Description = newOrder.Description;
            selectedOrder.From = newOrder.From;
            selectedOrder.To = newOrder.To;
            selectedOrder.Status = newOrder.Status;
            selectedOrder.Number = newOrder.Number;
            selectedOrder.UserId = newOrder.UserId;
            selectedOrder.Id = newOrder.Id;
            selectedOrder.Comment = newOrder.Comment;

            _applicationContext.Entry(selectedOrder).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> HistoryAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var orders = await _applicationContext.Order
                .Where(x => x.UserId == user.Id)
                .ToListAsync();

            var collection = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                var viewModel = new OrderViewModel
                {
                    Number = order.Number,
                    Name = order.Name,
                    Description = order.Description,
                    From = order.From,
                    To = order.To,
                    Status = order.Status,
                    Comment = order.Comment
                };

                collection.Add(viewModel);
            }

            return View(collection);
        }
    }
}
