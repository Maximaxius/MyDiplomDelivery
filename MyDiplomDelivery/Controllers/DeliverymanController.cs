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
            ///Везде проверку по ид пользлвателя DElivManID
            var user = await _userManager.GetUserAsync(HttpContext.User);
            //var delivery = await _applicationContext.Delivery.Include(t => t.DeliveryDetails).Where(x => x.DeliveryManId == user.Id).ToListAsync();
            //var orderIds = delivery.SelectMany(x => x.DeliveryDetails).Select(x => x.OrderId).ToList();
            //if(!orderIds.Contains(num))

            //var d = await _applicationContext.DeliveryDetail.Include(t => t.Order).Include(t => t.Delivery)
            //    .Where(t=>t.Delivery.DeliveryManId == user.Id).Where(t=>t.Order.Number==num).ToListAsync();
            //if (d.Count == 0)
            //{
            //    return RedirectToAction("Index");
            //}

            var deliveries = await _applicationContext.Delivery
                .Include(x => x.DeliveryDetails)
                    .ThenInclude(x => x.Order)
                .Where(x => x.DeliveryManId == user.Id).ToListAsync();

            var selectedOrders = deliveries.SelectMany(x => x.DeliveryDetails)
                .Select(x => x.Order)
                .ToList();

            var selectedOrderNumbers = selectedOrders.Select(x=>x.Number).ToList();

            if (!selectedOrderNumbers.Contains(num))
            {
                return RedirectToAction("Index");
            }

            var Order = await _applicationContext.Order.FirstOrDefaultAsync(order => order.Number == num );
            if (Order != null)
            {
                DeliveryManEditOrderViewModel viewModel = new DeliveryManEditOrderViewModel
                {
                    Number = Order.Number,
                    Status = Order.Status,
                };
                return View(viewModel);
            }
            return RedirectToAction("Index");
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
