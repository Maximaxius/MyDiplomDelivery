using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyDiplomDelivery.Models;

namespace MyDiplomDelivery.Contexts
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Delivery> Delivery { get; set; }
        public virtual DbSet<DeliveryDetail> DeliveryDetail { get; set; }
        public virtual DbSet<Deliveryman> Deliveryman { get; set; }
        public virtual DbSet<Order> Order { get; set; }
    }
}
