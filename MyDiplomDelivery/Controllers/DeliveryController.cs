using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels.Delivery;
using System.Data;

namespace MyDiplomDelivery.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class DeliveryController : Controller
    {
        private readonly ApplicationContext _applicationContext;

        public DeliveryController(
            ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var deliveries = await _applicationContext.Delivery
                .Include(x => x.DeliveryMan)
                .ToListAsync();

            var collection = new List<AllDeliveryViewModel>();
            foreach (var delivery in deliveries)
            {
                var viewModel = new AllDeliveryViewModel
                {
                    Id = delivery.Id,
                    CreationTime = delivery.CreationTime,
                    Status = delivery.Status,
                    DeliveryManId = delivery.DeliveryMan.Id,
                    FirstName = delivery.DeliveryMan.FirstName,
                    SecondName = delivery.DeliveryMan.SecondName,
                    LastName = delivery.DeliveryMan.LastName,
                };

                collection.Add(viewModel);
            }

            return View(collection);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roleId = _applicationContext.Roles.FirstOrDefault(x => x.Name == "DeliveryMan")?.Id;
            
            var deliveryMans = await _applicationContext.UserRoles
                .Where(x => x.RoleId == roleId)
                .ToListAsync();
            
            var users = await _applicationContext.Users
                .Where(x => x.IsActive)
                .ToListAsync();

            var viewModel = new DeliveryCreateViewModel
            {
                DeliveryManList = new List<SelectListItem>()
            };

            foreach (var deliveryMan in deliveryMans)
            {
                foreach (var user in users)
                {
                    if (deliveryMan.UserId == user.Id && user.IsActive == true)
                    {
                        viewModel.DeliveryManList.Add(new SelectListItem
                        {
                            Text = $"{user.FirstName} {user.SecondName} {user.LastName}",
                            Value = user.Id.ToString()
                        });
                    }
                }
            }

            viewModel.OrdersList = new List<SelectListItem>();

            var orders = await _applicationContext.Order
                .Where(x => x.Status == StatusType.Todo)
                .ToListAsync();

            foreach (var order in orders)
            {
                viewModel.OrdersList.Add(new SelectListItem { Text = $"{order.Name}", Value = order.Id.ToString() });
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DeliveryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                //создание Delivery
                var delivery = new Delivery
                {
                    DeliveryManId = model.SelectDeliveryMan,
                    Status = StatusType.Todo,
                    CreationTime = DateTime.Now,
                };

                await _applicationContext.Delivery.AddAsync(delivery);
                await _applicationContext.SaveChangesAsync();

                //создание DeliveryDetail
                foreach (var item in model.SelectedOrders)
                {
                    var deliveryDetail = new DeliveryDetail
                    {
                        DeliveryId = delivery.Id,
                        OrderId = item
                    };

                    await _applicationContext.DeliveryDetail.AddAsync(deliveryDetail);
                    await _applicationContext.SaveChangesAsync();
                }

                //Смена статуса заказов
                foreach (var item in model.SelectedOrders)
                {
                    var order = await _applicationContext.Order.FirstOrDefaultAsync(x => x.Id == item);  
                    order!.Status = StatusType.InProgress;
                    _applicationContext.Entry(order).State = EntityState.Modified;

                    await _applicationContext.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Delivery");
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var deliveryDetails = await _applicationContext.DeliveryDetail
                .Include(x => x.Order)
                .Include(x => x.Delivery)
                .Where(x => x.Delivery.Id == id)
                .ToListAsync();

            var delivery = await _applicationContext.Delivery.FirstOrDefaultAsync(x => x.Id == id);

            var orders = new List<Order>();
            foreach (var deliveryOrders in deliveryDetails)
            {
                var order = new Order
                {
                    Name = deliveryOrders.Order.Name,
                    Description = deliveryOrders.Order.Description,
                    Status = deliveryOrders.Order.Status,
                };

                orders.Add(order);
            }

            var viewModel = new EditDeliveryViewModel
            {
                Orders = orders,
                DeliveryId = delivery!.Id,
                CreationTime = delivery.CreationTime,
                DeliveryManId = delivery.DeliveryManId,
                Status = delivery.Status
            };

            if (viewModel != null)
            {
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditDeliveryViewModel editDelivery)
        {
            var delivery = await _applicationContext.Delivery.FirstAsync(x => x.Id == editDelivery.DeliveryId);
            delivery.Status = editDelivery.Status;
            _applicationContext.Entry(delivery).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
