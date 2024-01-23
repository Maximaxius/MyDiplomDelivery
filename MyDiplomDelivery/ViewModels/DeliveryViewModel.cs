using MyDiplomDelivery.Enums;

namespace MyDiplomDelivery.ViewModels
{
    public class DeliveryViewModel
    {
        public int Id { get; set; }
        public int DeliverymanId { get; set; }
        public DateTime CreationTime { get; set; }
        public StatusType Status { get; set; }
    }
}
