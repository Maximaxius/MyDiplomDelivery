using MyDiplomDelivery.Enums;

namespace MyDiplomDelivery.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public int DeliverymanId { get; set; }
        public Deliveryman Deliveryman { get; set; } //на User //DeliveryМan везде с большой
        public DateTime CreationTime { get; set; }
        public StatusType Status { get; set; }
        public List<DeliveryDetail> DeliveryDetails { get; set; }
    }
}
