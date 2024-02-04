﻿using MyDiplomDelivery.Enums;

namespace MyDiplomDelivery.ViewModels.Delivery
{
    public class AllDeliveryViewModel
    {
        public int Id { get; set; }
        //public int DeliverymanId { get; set; }
        public string DeliverymanId { get; set; }
        public DateTime CreationTime { get; set; }
        public StatusType Status { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
    }
}
