using MyDiplomDelivery.Enums;

namespace MyDiplomDelivery.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? Number{ get; set; }
        public string? Name { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Description { get; set; }
        public string? Comment { get; set; }
        public StatusType Status { get; set; }

        public List<DeliveryDetail> DeliveryDetails { get; set; }

        //Добавть  время добавления ??? тип когда пользователь оставил/сделал заказ

    }
}
