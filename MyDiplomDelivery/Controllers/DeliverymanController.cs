using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels;
using MyDiplomDelivery.ViewModels.DeliveryMan;
using System;
using System.Security.Claims;

namespace MyDiplomDelivery.Controllers
{
    [Authorize(Roles = "Deliver")]
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
        public async Task<IActionResult> IndexAsync()
        {
            ///проверка на активность 
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ////спецификация на админа мб куданибуть еще

            var deliveryDetails = await _applicationContext.DeliveryDetail
                .Include(t => t.Order)
                .Include(t => t.Delivery)
                .Where(t => t.Delivery.Deliverymanid == user.Id && (t.Order.Status == StatusType.InProgress
                || t.Order.Status == StatusType.Cancelled
                || t.Order.Status == StatusType.Completed)).ToListAsync();
            
            var list = new List<AllOrderViewModel>();

            foreach (var deliveryDetail in deliveryDetails)
            {
                var log = new AllOrderViewModel
                {
                    Name = deliveryDetail.Order.Name,
                    Description = deliveryDetail.Order.Description,
                    From = deliveryDetail.Order.From,
                    To = deliveryDetail.Order.To,
                    Status = deliveryDetail.Order.Status,
                    Number = deliveryDetail.Order.Number,
                };
                list.Add(log);
            }

            return View(list);
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
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var log = new Deliveryman
                {
                    userId = user.Id,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
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
                DeliveryManEditOrderViewModel viewModel = new DeliveryManEditOrderViewModel
                {
                    Order = Order
                };
                return View(viewModel);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Order(DeliveryManEditOrderViewModel editOrder)
        {
            _applicationContext.Entry(editOrder.Order).State = EntityState.Modified;
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
