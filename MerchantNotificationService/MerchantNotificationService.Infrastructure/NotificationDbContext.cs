using MerchantNotificationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MerchantNotificationService.Infrastructure.Persistence;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<ProductMerchant> ProductMerchants { get; set; }
    public DbSet<NotificationInbox> NotificationInbox { get; set; }
    public DbSet<NotificationOutbox> NotificationOutbox { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Merchant Configuration
        modelBuilder.Entity<Merchant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(300).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // ProductMerchant Configuration
        modelBuilder.Entity<ProductMerchant>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.MerchantId });
            entity.HasOne(e => e.Merchant)
                  .WithMany()
                  .HasForeignKey(e => e.MerchantId);
            entity.HasIndex(e => e.ProductId);
        });

        // NotificationInbox Configuration
        modelBuilder.Entity<NotificationInbox>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ProductId, e.SequenceNumber }).IsUnique();
            entity.HasIndex(e => e.IsProcessed);
        });

        // NotificationOutbox Configuration
        modelBuilder.Entity<NotificationOutbox>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ProductId, e.SequenceNumber });
            entity.HasIndex(e => e.IsSent);
            entity.HasIndex(e => e.NextRetryAt);
        });
    }
}