﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MyDiplomDelivery.Controllers
{
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

            var viewModel = new AllOrderViewModel
            {
                Orders = list
            };

            return View(viewModel);
        }




        [HttpGet]
        public IActionResult Create()
        {
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

        [HttpGet]
        public async Task<IActionResult> SuccessAsync(int id)
        {
            var order = await _applicationContext.Order.FirstOrDefaultAsync(order => order.Id == id);

            if (order == null)
            {
                return RedirectToAction("Create", "Order");
            }

            var viewModel = new OrderViewModel
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
                return View(Order);           
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            _applicationContext.Entry(order).State = EntityState.Modified;
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
                    UserName = userEmail
                };
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, "deliver");
            }
            return RedirectToAction("UserList", "Roles");
        }
    }
}