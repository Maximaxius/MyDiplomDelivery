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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roleId = _applicationContext.Roles.FirstOrDefault(t => t.Name == "deliver")?.Id;
            var userRoles = await _applicationContext.UserRoles.Where(t => t.RoleId == roleId).ToListAsync();
            var users = await _applicationContext.Deliveryman.ToListAsync();

            //4 работает  GPT
            var model = new DeliveryDetailViewModel();
            model.deliveryManList = new List<SelectListItem>();
            List<Deliveryman> asd = new List<Deliveryman>();
            foreach (var userRole in userRoles)
            {
                foreach (var user in users)
                    if (userRole.UserId == user.userId && user.IsActive == true)
                    {
                        model.deliveryManList.Add(new SelectListItem { Text = $"{user.FirstName} {user.SecondName}", Value = user.id.ToString() });
                    }
            }
            //5 работает  https://www.youtube.com/watch?v=A1yJVmtIDXA
            //var options = new List<SelectListItem>();
            //foreach (var userRole in userRoles)
            //{
            //    foreach (var user in users)
            //        if (userRole.UserId == user.userId && user.IsActive == true)
            //        {
            //            options.Add(new SelectListItem { Text = $"{user.FirstName} {user.SecondName}", Value = user.id.ToString() });
            //        }
            //}
            //var model = new DeliveryDetailViewModel
            //{
            //    deliveryManList = options
            //};

            //////////////////////////////////////////////////////////////////////////////////
            /////GPT
            model.MultiSelectOptions = new List<SelectListItem>();
            var orders = await _applicationContext.Order.Where(t => t.Status == StatusType.Todo).ToListAsync();
            foreach (var order in orders)
            {
                model.MultiSelectOptions.Add(new SelectListItem { Text = $"{order.Name}", Value = order.Id.ToString() });
            }

            ///MY
            //var orders = await _applicationContext.Order.Where(t => t.Status == StatusType.Todo).ToListAsync();
            //List<string> ord = new List<string>();
            //foreach (var order in orders)
            //{
            //    ord.Add(order.Name!);
            //}
            //ViewBag.ord = new SelectList(ord);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DeliveryDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var delivery = new Delivery
                {
                    DeliverymanId = Int32.Parse(model.selectDeliveryMan),
                    Status = StatusType.Todo,
                    CreationTime = DateTime.Now,
                };
                await _applicationContext.Delivery.AddAsync(delivery);
                await _applicationContext.SaveChangesAsync();

                foreach (var item in model.SelectedOptions)
                {
                    var deliveryDetail = new DeliveryDetail
                    {
                        DeliveryId = delivery.Id,
                        OrderId = Int32.Parse(item)
                    };
                    await _applicationContext.DeliveryDetail.AddAsync(deliveryDetail);
                    await _applicationContext.SaveChangesAsync();
                }


                return RedirectToAction("Index", "Home");

            }

            return BadRequest();
        }

    }
}
