using MyDiplomDelivery.Enums;

namespace MyDiplomDelivery.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public User DeliveryMan { get; set; }
        public string? DeliveryManId { get; set; }
        public DateTime CreationTime { get; set; }
        public StatusType Status { get; set; }
        public List<DeliveryDetail> DeliveryDetails { get; set; }
    }
}
