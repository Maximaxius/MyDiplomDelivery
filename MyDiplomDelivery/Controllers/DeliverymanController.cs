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
    [Authorize(Roles = "DeliveryMan")]
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
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var deliveryDetails = await _applicationContext.DeliveryDetail
                .Include(t => t.Order)
                .Include(t => t.Delivery)
                .Where(t => t.Delivery.DeliveryManId == user.Id && (t.Order.Status == StatusType.InProgress
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
        public async Task<IActionResult> Order(string num)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var deliveries = await _applicationContext.Delivery
                .Include(x => x.DeliveryDetails)
                    .ThenInclude(x => x.Order)
                .Where(x => x.DeliveryManId == user.Id).ToListAsync();

            var selectedOrder = deliveries.SelectMany(x => x.DeliveryDetails)
                .Select(x => x.Order)
                .FirstOrDefault(x => x.Number == num);

            if (selectedOrder==null)
            {
                return RedirectToAction("Index");
            }
                        
            DeliveryManEditOrderViewModel viewModel = new DeliveryManEditOrderViewModel
            {
                Number = selectedOrder.Number,
                Status = selectedOrder.Status,
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Order(DeliveryManEditOrderViewModel editOrder)
        {
            var Order = await _applicationContext.Order.FirstOrDefaultAsync(order => order.Number == editOrder.Number);
            Order!.Status = editOrder.Status;

            _applicationContext.Entry(Order).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
