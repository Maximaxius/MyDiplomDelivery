using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels.Delivery;
using System.Data;
using System.Xml.Linq;

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
            var Deliverys = await _applicationContext.Delivery.Include(x => x.DeliveryMan).ToListAsync();

            var list = new List<AllDeliveryViewModel>();
            foreach (var delivery in Deliverys)
            {
                var log = new AllDeliveryViewModel
                {
                    Id = delivery.Id,
                    CreationTime= delivery.CreationTime,
                    Status = delivery.Status,
                    DeliveryManId = delivery.DeliveryMan.Id,
                    FirstName = delivery.DeliveryMan.FirstName,
                    SecondName = delivery.DeliveryMan.SecondName,
                    LastName = delivery.DeliveryMan.LastName,
                };
                list.Add(log);
            }            
            return View(list);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roleId = _applicationContext.Roles.FirstOrDefault(t => t.Name == "DeliveryMan")?.Id;
            var deliveryMans = await _applicationContext.UserRoles.Where(t => t.RoleId == roleId).ToListAsync();
            var users = await _applicationContext.Users.Where(t=>t.IsActive == true).ToListAsync();
            
            var model = new DeliveryCreateViewModel();
            model.DeliveryManList = new List<SelectListItem>();
            foreach (var deliveryMan in deliveryMans)
            {
                foreach (var user in users)
                {
                    if (deliveryMan.UserId == user.Id && user.IsActive == true)
                    {
                        model.DeliveryManList.Add(new SelectListItem { Text = $"{user.FirstName} {user.SecondName} {user.LastName}", Value = user.Id.ToString() });
                    }
                }
            }

            model.OrdersList = new List<SelectListItem>();
            var orders = await _applicationContext.Order.Where(t => t.Status == StatusType.Todo).ToListAsync();
            foreach (var order in orders)
            {
                model.OrdersList.Add(new SelectListItem { Text = $"{order.Name}", Value = order.Id.ToString() });
            }
            return View(model);
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
                    var order = _applicationContext.Order.FirstOrDefault(t => t.Id == item);  // почемуто на await жалуется и не работает с ним
                    order!.Status= StatusType.InProgress;
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
            var deliveryDetails = await _applicationContext.DeliveryDetail.Include(t => t.Order).
                Include(t => t.Delivery).
                Where(t => t.Delivery.Id == id).ToListAsync();
            var delivery = await _applicationContext.Delivery.FirstOrDefaultAsync(t => t.Id == id);

            var orders = new List<Order>();
            foreach (var deliveryOrders in deliveryDetails)
            {
                var log = new Order
                {
                    Name = deliveryOrders.Order.Name,
                    Description = deliveryOrders.Order.Description,
                    Status = deliveryOrders.Order.Status,
                };
                orders.Add(log);
            }

            var list = new EditDeliveryViewModel
            {
                Orders = orders,
                DeliveryId = delivery!.Id,
                CreationTime = delivery.CreationTime,
                DeliveryManId = delivery.DeliveryManId,
                Status = delivery.Status
            };

            if (list != null)
            {
                return View(list);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditDeliveryViewModel editDelivery)
        {
            Delivery delivery = await _applicationContext.Delivery.FirstAsync(t => t.Id == editDelivery.DeliveryId);
            delivery.Status = editDelivery.Status;
            _applicationContext.Entry(delivery).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
