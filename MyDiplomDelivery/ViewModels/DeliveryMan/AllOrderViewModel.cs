using MyDiplomDelivery.Enums;

namespace MyDiplomDelivery.ViewModels.DeliveryMan
{
    public class AllOrderViewModel
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public string? Name { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Description { get; set; }
        public string? Comment { get; set; }
        public StatusType Status { get; set; }
    }
}
