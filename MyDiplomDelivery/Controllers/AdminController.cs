using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels.Admin;
using MyDiplomDelivery.ViewModels.Delivery;
using MyDiplomDelivery.ViewModels.O;

namespace MyDiplomDelivery.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserManager<User> _userManager;

        public AdminController(
            ApplicationContext applicationContext,
            UserManager<User> userManager)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var Users = await _applicationContext.Users.ToListAsync();
            var list = new List<AllUsersViewModel>();
            foreach (var user in Users)
            {
                var role = await _userManager.GetRolesAsync(user);
                var log = new AllUsersViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    Role = role.ToList(),
                };
                list.Add(log);
            }

            return View(list);
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _applicationContext.Users.FirstOrDefaultAsync(order => order.Id == userId);
            if (user != null)
            {
                EditUserViewModel viewModel = new EditUserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    SecondName= user.SecondName,
                };
                return View(viewModel);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel edit)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(edit.Id);
                if (user != null)
                {
                    user.FirstName = edit.FirstName;
                    user.SecondName = edit.SecondName;
                    user.LastName = edit.LastName;
                    user.IsActive = edit.IsActive;
                    await _userManager.UpdateAsync(user);
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AutoGenUserOrder() //для авто генерации пользователей 
        {
            for (int i = 0; i <= 5; i++)
            {
                string userEmail = $"user{i}@gmail.com";
                string password = "_Aa123456";
                User user = new User
                {
                    Email = userEmail,
                    UserName = userEmail,
                    FirstName = "FirstName",
                    SecondName = "SecondName",
                    LastName = "LastName",
                    IsActive = true,
                };
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, "DeliveryMan");

                Order order = new Order
                {
                    Number = Guid.NewGuid().ToString("N"),
                    From = "asd",
                    Name = $"{i}",
                    Status = StatusType.Todo
                };
                await _applicationContext.Order.AddAsync(order);
            }
            return RedirectToAction("UserList", "Roles");
        }

    }
}
