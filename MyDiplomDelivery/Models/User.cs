using Microsoft.AspNetCore.Identity;
using MyDiplomDelivery.Enums;

namespace MyDiplomDelivery.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondName { get; set; }
        public bool IsActive { get; set; }
        public List<Delivery> Deliveries { get; set; }
    }
}
