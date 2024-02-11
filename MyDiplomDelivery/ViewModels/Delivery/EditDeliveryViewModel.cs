using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;

namespace MyDiplomDelivery.ViewModels.Delivery
{
    public class EditDeliveryViewModel
    {
        public int DeliveryId { get; set; }
        public StatusType Status { get; set; }
        public DateTime CreationTime { get; set; }
        public string? DeliveryManId { get; set; }
        public List<Order> Orders { get; set; }
    }
}
