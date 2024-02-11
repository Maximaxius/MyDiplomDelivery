using MyDiplomDelivery.Enums;
using MyDiplomDelivery.Models;

namespace MyDiplomDelivery.ViewModels.DeliveryMan
{
    public class DeliveryManEditOrderViewModel
    {
        public string? Number { get; set; }
        public StatusType Status { get; set; }
    }
}
