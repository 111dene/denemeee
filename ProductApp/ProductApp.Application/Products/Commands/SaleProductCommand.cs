using MassTransit;
using MediatR;
using ProductApp.Application.Common;
using ProductApp.Application.Products.Inputs;
using ProductApp.Domain.Aggregates.Product.Exceptions;


namespace ProductApp.Application.Products.Commands;

public class ProcessSaleCommand : IRequest<ProcessSaleResult>
{
    public ProcessSaleCommandInput Input { get; }

    private ProcessSaleCommand(ProcessSaleCommandInput input)
    {
        Input = input;
    }

    public static ProcessSaleCommand Create(ProcessSaleCommandInput input)
    {
        return new ProcessSaleCommand(input);
    }
}

public class ProcessSaleResult
{
    public string OrderId { get; set; }
    public List<SaleItem> Results { get; set; } = new();
}

public class SaleItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int SoldQuantity { get; set; }
    public int RemainingStock { get; set; }
}

public sealed class ProcessSaleCommandHandler : IRequestHandler<ProcessSaleCommand, ProcessSaleResult>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IProductReadRepository productReadRepository;
    private readonly IPublishEndpoint publishEndpoint;

    public ProcessSaleCommandHandler(
        IUnitOfWork unitOfWork,
        IProductReadRepository productReadRepository,
        IPublishEndpoint publishEndpoint)
    {
        this.unitOfWork = unitOfWork;
        this.productReadRepository = productReadRepository;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task<ProcessSaleResult> Handle(ProcessSaleCommand request, CancellationToken cancellationToken)
    {
        var result = new ProcessSaleResult
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
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}. Available: {product.Stock}, Requested: {item.Quantity}");
            }

            // Stok düşür
            product.ReduceStock(item.Quantity); // Bu metodu Product domain'ine ekleyeceğiz

            result.Results.Add(new SaleItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                SoldQuantity = item.Quantity,
                RemainingStock = product.Stock
            });

            // Stock düştü eventi yayınla
            var stockReducedEvent = new StockReducedEvent
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ReducedQuantity = item.Quantity,
                RemainingStock = product.Stock,
                OrderId = request.Input.OrderId,
                OccurredOn = DateTime.UtcNow,
                SequenceNumber = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() // Sıralama için
            };

            await publishEndpoint.Publish(stockReducedEvent, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}