using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels;
using MyDiplomDelivery.ViewModels.O;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SuccessAsync(int id)
        {
            var order = await _applicationContext.Order.FirstOrDefaultAsync(order => order.Id == id);

            if (order == null)
            {
                return RedirectToAction("Create", "Order");
            }

            var viewModel = new SuccessViewModel
            {
                Number = order.Number
            };
            return View(viewModel);
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
                    Order = Order
                };
                return View(viewModel);         
            }            
            return RedirectToAction("Index");
        }
        

        [HttpPost]
        public async Task<IActionResult> Edit(OrderEditOrderViewModel newOrder)
        {
            _applicationContext.Entry(newOrder.Order).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Privacy() //для авто генерации пользователей 
        {
            for (int i = 0; i <= 5; i++)
            {
                string userEmail = $"user{i}@gmail.com";
                string password = "_Aa123456";
                User user = new User
                {
                    Email = userEmail,
                    UserName = userEmail,
                    FirstName = "FirstName",
                    SecondName = "SecondName",
                    LastName = "LastName",
                    IsActive = true,
                };
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, "Deliver");

                
                Deliveryman deliveryman = new Deliveryman
                {
                    userId = user.Id,
                    FirstName = $"A{i}",
                    SecondName = "B",
                    LastName = "C",
                    IsActive = true,
                };
                await _applicationContext.Deliveryman.AddAsync(deliveryman);

                Order order = new Order
                {
                    Number = Guid.NewGuid().ToString("N"),
                    From = "asd",
                    Name = $"{i}",
                    Status=StatusType.Todo
                };
                await _applicationContext.Order.AddAsync(order);
            }
            return RedirectToAction("UserList", "Roles");
        }
    }
}
