using MyDiplomDelivery.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyDiplomDelivery.ViewModels.O
{
    public class CreateOrderViewModel
    {
        public string? Number { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? From { get; set; }

        [Required]
        public string? To { get; set; }
        public string? Description { get; set; }
        public string? Comment { get; set; }
        public StatusType Status { get; set; }
    }
}
