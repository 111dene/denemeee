using Microsoft.EntityFrameworkCore;
using ProductApp.Application;
using ProductApp.Domain.Aggregates.Product;
using ProductApp.Infrastructure.Persistance.EntityFrameworkCore.Products.EntityConfigurations;

namespace ProductApp.Infrastructure.Persistance.EntityFrameworkCore
{
    public class ProductDbContext : DbContext, IUnitOfWork 
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}

//DbContext sınıfı, Entity Framework Core'un DbContext sınıfından türetilir ve uygulamanın veri erişim katmanını temsil eder.