using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels;

namespace MyDiplomDelivery.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserManager<User> _userManager;

        public DeliveryController(
            ApplicationContext applicationContext,
            UserManager<User> userManager)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var Deliverys = await _applicationContext.Delivery.Include(x=>x.Deliveryman).ToListAsync();
            var DeliveryMans = await _applicationContext.Deliveryman.ToListAsync();

            var list = new List<AllDeliveryViewModel>();
            foreach (var delivery in Deliverys)
            {
                var log = new AllDeliveryViewModel
                {
                    Id = delivery.Id,
                    CreationTime= delivery.CreationTime,
                    Status =   delivery.Status,
                    DeliverymanId= delivery.DeliverymanId,
                    FirstName = delivery.Deliveryman.FirstName,
                    SecondName = delivery.Deliveryman.SecondName,
                    LastName = delivery.Deliveryman.LastName,
                };
                list.Add(log);
            }
            
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roleId = _applicationContext.Roles.FirstOrDefault(t => t.Name == "deliver")?.Id;
            var userRoles = await _applicationContext.UserRoles.Where(t => t.RoleId == roleId).ToListAsync();
            var users = await _applicationContext.Deliveryman.ToListAsync();

            //4 работает  GPT
            var model = new DeliveryDetailViewModel();
            model.DeliveryManList = new List<SelectListItem>();
            List<Deliveryman> asd = new List<Deliveryman>();
            foreach (var userRole in userRoles)
            {
                foreach (var user in users)
                    if (userRole.UserId == user.userId && user.IsActive == true)
                    {
                        model.DeliveryManList.Add(new SelectListItem { Text = $"{user.FirstName} {user.SecondName}", Value = user.id.ToString() });
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
        public async Task<IActionResult> Create(DeliveryDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                //создание Delivery
                var delivery = new Delivery
                {
                    DeliverymanId = model.SelectDeliveryMan,
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
                    order.Status= StatusType.InProgress;
                    _applicationContext.Entry(order).State = EntityState.Modified;
                    await _applicationContext.SaveChangesAsync();
                }



                return RedirectToAction("Index", "Home");

            }

            return BadRequest();
        }

    }
}
