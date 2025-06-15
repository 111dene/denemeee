using MerchantNotificationService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure Services (tüm servisleri buradan register ediyoruz)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Development environment configuration
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Run();