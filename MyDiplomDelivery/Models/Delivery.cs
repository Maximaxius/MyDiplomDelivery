using MyDiplomDelivery.Enums;

namespace MyDiplomDelivery.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public User DeliveryMan { get; set; }
        public string Deliverymanid { get; set; }       // нужно DeliveryManId но не создает (как я понял изза sql CAPS)
        public DateTime CreationTime { get; set; }
        public StatusType Status { get; set; }
        public List<DeliveryDetail> DeliveryDetails { get; set; }
    }
}
