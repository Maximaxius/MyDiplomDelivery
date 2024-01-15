using MyDiplomDelivery.Enums;

namespace MyDiplomDelivery.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public string? Name { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Description { get; set; }

        public string? Comment { get; set; }
        public StatusType Status { get; set; }
        //когда разберусь с почтой 
        //public string? Email { get; set; }

    }
}
