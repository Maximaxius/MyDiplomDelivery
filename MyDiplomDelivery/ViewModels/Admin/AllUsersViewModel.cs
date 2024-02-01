using MyDiplomDelivery.Enums;

namespace MyDiplomDelivery.ViewModels.Admin
{
    public class AllUsersViewModel
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; }
        public List<string>? Role { get; set; }
    }
}
