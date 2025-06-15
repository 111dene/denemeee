using MassTransit;
using MediatR;
using ProductApp.Application.Common;
using ProductApp.Application.Products.Inputs;
using ProductApp.Application.Products.Outputs;
using ProductApp.Domain.Aggregates.Product.DomainEvents;
using ProductApp.Domain.Aggregates.Product.Exceptions;

namespace ProductApp.Application.Products.Commands;

public class SaleProductCommand : IRequest<SaleProductOutput> // SaleProductResult değil, SaleProductOutput olmalı
{
    public SaleProductCommandInput Input { get; }

    private SaleProductCommand(SaleProductCommandInput input)
    {
        Input = input;
    }

    public static SaleProductCommand Create(SaleProductCommandInput input)
    {
        return new SaleProductCommand(input);
    }
}


public sealed class SaleProductCommandHandler : IRequestHandler<SaleProductCommand, SaleProductOutput>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IProductReadRepository productReadRepository;
    private readonly IPublishEndpoint publishEndpoint;

    public SaleProductCommandHandler(
        IUnitOfWork unitOfWork,
        IProductReadRepository productReadRepository,
        IPublishEndpoint publishEndpoint)
    {
        this.unitOfWork = unitOfWork;
        this.productReadRepository = productReadRepository;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task<SaleProductOutput> Handle(SaleProductCommand request, CancellationToken cancellationToken)
    {
        var result = new SaleProductOutput
        {
            OrderId = request.Input.OrderId
        };

        foreach (var item in request.Input.Items)
        {
            var product = await productReadRepository.GetByIdAsync(item.ProductId, cancellationToken);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            if (product.Stock < item.Quantity)
            {
                throw new InvalidOperationException($"Ürün {product.Name} için yetersiz stok. Mevcut: {product.Stock}, İstenen: {item.Quantity}");
            }

            // Stok düşür
            product.ReduceStock(item.Quantity);

            result.Items.Add(new SaleProductItemOutput
            {
                ProductId = product.Id,
                ProductName = product.Name,
                SoldQuantity = item.Quantity,
                RemainingStock = product.Stock
            });

            // Event yayınla
            var stockReducedEvent = new ReduceStockEvent
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ReducedQuantity = item.Quantity,
                RemainingStock = product.Stock,
                OrderId = request.Input.OrderId,
                OccurredOn = DateTime.UtcNow,
                SequenceNumber = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            await publishEndpoint.Publish(stockReducedEvent, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}