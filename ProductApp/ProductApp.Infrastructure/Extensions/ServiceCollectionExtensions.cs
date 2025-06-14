using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApp.Application;
using ProductApp.Application.Common;
using ProductApp.Application.Products.Mappers;
using ProductApp.Infrastructure.Persistance.EntityFrameworkCore;
using ProductApp.Infrastructure.Persistance.EntityFrameworkCore.Products;

namespace ProductApp.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ProductDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Unit of Work
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ProductDbContext>());

        // Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        //services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Mappers
        //services.AddScoped<ProductMapper>();
        services.AddSingleton<ProductMapper>();//Mapper stateless, Singleton olmalı dedi gpt abi

        return services;
    }
}

//DI containera eklenen servisler