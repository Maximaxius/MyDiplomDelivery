using Microsoft.AspNetCore.Identity;

namespace MyDiplomDelivery.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondName { get; set; }
        public bool IsActive { get; set; }
        public List<Delivery> Deliveries { get; set; }
        public List<Order> Orders { get; set; }
    }
}
