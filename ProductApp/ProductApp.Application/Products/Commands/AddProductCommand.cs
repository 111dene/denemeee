using MassTransit;
using MediatR;
using ProductApp.Application.Common;
using ProductApp.Application.Products.Inputs;
using ProductApp.Domain.Aggregates.Product.DomainEvents;
using ProductApp.Domain.Aggregates.Product.Exceptions;


namespace ProductApp.Application.Products.Commands;

public class AddProductCommand : IRequest<int>
{
    public AddProductCommandInput Input { get; }// Bu sınıf, ürün ekleme işlemi için gerekli verileri taşır.

    private AddProductCommand(AddProductCommandInput input)
    {
        Input = input;// Dışarıdan alınan input verisi sınıfın Input özelliğine atanır.
    }

    public static AddProductCommand Create(AddProductCommandInput input)
    {
        return new AddProductCommand(input);// Komut oluşturulurken dışarıdan gelen input verisi ile nesneyi oluşturuyoruz.
    }
}

public sealed class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>//bağımlılıkları alır ve komutu işler
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly IProductReadRepository productReadRepository;

    public AddProductCommandHandler(// Constructor injection ile bağımlılıklar alınıyor.
        IUnitOfWork unitOfWork,
        IProductReadRepository productReadRepository,
        IPublishEndpoint publishEndpoint)
    {
        this.unitOfWork = unitOfWork;
        this.publishEndpoint = publishEndpoint;
        this.productReadRepository = productReadRepository;
    }

    public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)//güncellenmiş stok miktarını döndüren bir komut işleyici
    {
        
        var product = await productReadRepository.GetByNameAsync(request.Input.ProductName, cancellationToken);// Ürün ismine göre veritabanından ürünü alıyoruz.

        if (product == null)
        {
            throw new ProductNotFoundException();
        }

        
        product.AddStock(request.Input.Quantity);// Ürüne stok ekliyoruz. Bu işlem, Product sınıfının AddStock metodunu kullanarak yapılır.


        await unitOfWork.SaveChangesAsync(cancellationToken);

        
        var createProductEvent = new AddProductEvent
        {
            ProductId = product.Id,
            OccurredOn = DateTime.UtcNow
        };

        await publishEndpoint.Publish(createProductEvent, cancellationToken);//masstransitin event yayınlama özelliği

        return product.Stock;
    }
}