using MyDiplomDelivery.Enums;

namespace MyDiplomDelivery.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public int DeliveryDetailId { get; set; }
        public int DeliverymanId { get; set; }
        public DateTime Email { get; set; }
        public StatusType Status { get; set; }
    }
}
