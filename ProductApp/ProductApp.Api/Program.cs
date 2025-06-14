using FluentValidation;
using FluentValidation.AspNetCore;
using ProductApp.Api.EndpointMappings;
using ProductApp.Api.ExceptionHandlers;
using ProductApp.Api.ServiceRegistrations;
using ProductApp.Application.Products.Commands;
using ProductApp.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure layer register
builder.Services.AddInfrastructure(builder.Configuration);

// MediatR register
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

// FluentValidation register
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// API services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Messaging services register
builder.Services.RegisterMessagingServices(builder.Configuration);

// Exception handlers
builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.AddExceptionHandler<DefaultExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Exception handling middleware
app.UseExceptionHandler();

// Register endpoints
app.RegisterProductEndpoints();

app.Run();