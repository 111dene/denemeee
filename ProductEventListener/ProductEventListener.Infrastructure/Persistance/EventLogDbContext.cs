using Microsoft.EntityFrameworkCore;
using ProductEventListener.Domain.Entities;

namespace ProductEventListener.Infrastructure.Persistance
{
    public class EventLogDbContext : DbContext
    {
        public EventLogDbContext(DbContextOptions<EventLogDbContext> options)
            : base(options) { }

        public DbSet<ProductEventLog> ProductEventLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEventLog>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.OccurredOn).IsRequired();

            });
        }
    }
}