using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Contexts;
using MyDiplomDelivery.Models;
using MyDiplomDelivery.ViewModels.Admin;
using MyDiplomDelivery.ViewModels.Delivery;

namespace MyDiplomDelivery.Controllers
{
    //[Authorize(Roles = "Admin")]
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


    }
}
