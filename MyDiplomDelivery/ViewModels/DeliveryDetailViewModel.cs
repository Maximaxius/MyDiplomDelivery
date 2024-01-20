using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyDiplomDelivery.ViewModels
{
    public class DeliveryDetailViewModel
    {
        public string? selectDeliveryMan { get; set; }
        public List<SelectListItem>? deliveryManList { get; set; }

        public List<SelectListItem>? MultiSelectOptions { get; set; }
        public List<string>? SelectedOptions { get; set; }
    }
}
