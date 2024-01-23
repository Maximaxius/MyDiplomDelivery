using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyDiplomDelivery.ViewModels
{
    public class DeliveryDetailViewModel
    {
        public int SelectDeliveryMan { get; set; }
        public List<SelectListItem>? DeliveryManList { get; set; }

        public List<SelectListItem>? OrdersList { get; set; }
        public List<int>? SelectedOrders { get; set; }
    }
}
