using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels.O;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            var Orders = await _applicationContext.Order.ToListAsync();

            var list = new List<OrderViewModel>();
            foreach (var order in Orders)
            {
                var log = new OrderViewModel
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
                list.Add(log);
            }

            return View(list);
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

                var log = new Order
                {
                    Number = Guid.NewGuid().ToString("N"),
                    Name = model.Name,
                    Description = model.Description,
                    From = model.From,
                    To = model.To,
                    Status = StatusType.Todo,
                    UserId= userId
                };

                await _applicationContext.Order.AddAsync(log);
                await _applicationContext.SaveChangesAsync();

                var order = await _applicationContext.Order.FirstOrDefaultAsync(order => order.Number == log.Number);
                
                TempData["num"] = log.Number;

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
                string num = data.ToString();              

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
            var Order = await _applicationContext.Order.FirstOrDefaultAsync(order => order.Number == num);
            if (Order != null)
            {
                _applicationContext.Order.Remove(Order);
                await _applicationContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string num)
        {
            var Order = await _applicationContext.Order.FirstOrDefaultAsync(order => order.Number == num);
            if (Order != null)
            {
                OrderEditOrderViewModel viewModel = new OrderEditOrderViewModel
                {
                    Name = Order.Name,
                    Description = Order.Description,
                    From = Order.From,
                    To = Order.To,
                    Status = Order.Status,
                    Number = Order.Number,
                    UserId =Order.UserId,
                    Id = Order.Id,
                    Comment = Order.Comment,
                };
                return View(viewModel);         
            }            
            return RedirectToAction("Index");
        }
        

        [HttpPost]
        public async Task<IActionResult> Edit(OrderEditOrderViewModel newOrder)
        {
            var order = await _applicationContext.Order.FirstOrDefaultAsync(x=>x.Id==newOrder.Id);
            order.Name = newOrder.Name;
            order.Description = newOrder.Description;
            order.From = newOrder.From;
            order.To = newOrder.To;
            order.Status = newOrder.Status;
            order.Number = newOrder.Number;
            order.UserId = newOrder.UserId;
            order.Id = newOrder.Id;
            order.Comment = newOrder.Comment;
            _applicationContext.Entry(order).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> HistoryAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var Orders = await _applicationContext.Order.Where(x=>x.UserId==user.Id).ToListAsync();

            var list = new List<OrderViewModel>();
            foreach (var order in Orders)
            {
                var log = new OrderViewModel
                {
                    Number = order.Number,
                    Name = order.Name,
                    Description = order.Description,
                    From = order.From,
                    To = order.To,
                    Status = order.Status,
                    Comment = order.Comment
                };
                list.Add(log);
            }

            return View(list);
        }

    }
}
