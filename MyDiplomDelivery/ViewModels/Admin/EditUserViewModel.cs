using MyDiplomDelivery.Models;

namespace MyDiplomDelivery.ViewModels.Admin
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondName { get; set; }
        public bool IsActive { get; set; }

    }
}
