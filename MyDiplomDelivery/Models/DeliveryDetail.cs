namespace MyDiplomDelivery.Models
{
    public class DeliveryDetail
    {
        public int Id { get; set; }
        public int DeliveryId { get; set; }
        public Delivery Delivery { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
