using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels.DeliveryMan;

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
                .Include(x => x.Order)
                .Include(x => x.Delivery)
                .Where(x => x.Delivery.DeliveryManId == user.Id
                    && (x.Order.Status == StatusType.InProgress
                        || x.Order.Status == StatusType.Cancelled
                        || x.Order.Status == StatusType.Completed))
                .ToListAsync();

            var collection = new List<AllOrderViewModel>();
            foreach (var deliveryDetail in deliveryDetails)
            {
                var viewModel = new AllOrderViewModel
                {
                    Name = deliveryDetail.Order.Name,
                    Description = deliveryDetail.Order.Description,
                    From = deliveryDetail.Order.From,
                    To = deliveryDetail.Order.To,
                    Status = deliveryDetail.Order.Status,
                    Number = deliveryDetail.Order.Number,
                };

                collection.Add(viewModel);
            }

            return View(collection);
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

            if (selectedOrder == null)
            {
                return RedirectToAction("Index");
            }

            var viewModel = new DeliveryManEditOrderViewModel
            {
                Number = selectedOrder.Number,
                Status = selectedOrder.Status,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Order(DeliveryManEditOrderViewModel editOrder)
        {
            var Order = await _applicationContext.Order.FirstOrDefaultAsync(x => x.Number == editOrder.Number);
            Order!.Status = editOrder.Status;
            _applicationContext.Entry(Order).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
