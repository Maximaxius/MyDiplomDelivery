namespace MyDiplomDelivery.Models
{
    public class Deliveryman //профиль
    {
        public int id { get; set; }
        public string? userId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondName { get; set; }
        public bool IsActive { get; set; }
        public List<Delivery> Deliveries { get; set; }
    }
}
