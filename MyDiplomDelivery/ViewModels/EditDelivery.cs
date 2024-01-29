using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;

namespace MyDiplomDelivery.ViewModels
{
    public class EditDelivery
    {
        public int DeliveryId { get; set; }
        public StatusType Status { get; set; }
        public DateTime CreationTime { get; set; }
        public int DeliverymanId { get; set; }
        public List<Order> Orders { get; set; }
    }
}
