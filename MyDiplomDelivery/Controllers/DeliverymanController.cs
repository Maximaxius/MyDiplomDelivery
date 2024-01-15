using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels;
using System;

namespace MyDiplomDelivery.Controllers
{
    public class DeliverymanController : Controller
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserManager<User> _userManager;

        public DeliverymanController(
            ApplicationContext applicationContext,
            UserManager<User> userManager)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
        }

        //[Authorize(Roles = "deliver")]
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
        public async Task<IActionResult> Create(DeliverymanViewModel model)
        {
            if (ModelState.IsValid)
            {
                var log = new Deliveryman
                {
                    FirstName = model.FirstName,
                    SecondName= model.SecondName,
                    LastName = model.LastName,
                    IsActive = model.IsActive
                };

                await _applicationContext.Deliveryman.AddAsync(log);
                await _applicationContext.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Order(string num)
        {
            var Order = await _applicationContext.Order.FirstOrDefaultAsync(order => order.Number == num);
            if (Order != null)
            {
                return View(Order);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Order(Order order)
        {

            _applicationContext.Entry(order).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }









        //[HttpGet]
        //public async Task<IActionResult> Edit(string num)
        //{
        //    var Order = await _applicationContext.Order.FirstOrDefaultAsync(order => order.Number == num);
        //    if (Order != null)
        //    {
        //        return View(Order);
        //    }

        //    return RedirectToAction("Index", "Home");
        //}
        //Поменятьь и спросить как нарисовал
        //[HttpPost]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var deliveryman = await _applicationContext.Deliveryman.FirstOrDefaultAsync(order => order.id == id);
        //    deliveryman.IsActive = !deliveryman.IsActive;
        //    _applicationContext.Entry(deliveryman).State = EntityState.Modified;
        //    await _applicationContext.SaveChangesAsync();
        //    return RedirectToAction("Index", "Home");
        //}
    }
}
