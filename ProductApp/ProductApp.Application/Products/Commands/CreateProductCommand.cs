using MediatR;
using ProductApp.Application.Common;
using ProductApp.Application.Products.Inputs;
using ProductApp.Domain.Aggregates.Product;
using ProductApp.Domain.Aggregates.Product.Exceptions;

namespace ProductApp.Application.Products.Commands;

public class CreateProductCommand : IRequest//ürün oluşturma işlemini başlatmak için gerekli verileri taşıyan bir komut sınıfı
{
    public CreateProductCommandInput Input { get; }//uı ve API katmanlarından gelen verileri tutar

    private CreateProductCommand(CreateProductCommandInput input)
    {
        Input = input;//dışarıdan alınan input verisi sınıfın input özelliğine atanır
    }

    public static CreateProductCommand Create(CreateProductCommandInput input)
    {
        return new CreateProductCommand(input);//komut oluşturulurken dışarıdan gelen input verisi ile nesneyi oluşturuyoruz
    }
}

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>//commandi işler,repository ve business logic işlemlerini gerçekleştirir
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IProductRepository productRepository;
    private readonly IProductReadRepository productReadRepository;



    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository, IProductReadRepository productReadRepository)//constructor injection ile bağımlılıkları alırız
    {
        this.unitOfWork = unitOfWork;
        this.productRepository = productRepository;
        this.productReadRepository = productReadRepository;
    }

    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)//komutu request parametresiyle handlera iletir
    {
        // Aynı isimde ürün kontrolü(neden burada yaptım tam emin olamadım)
        var productExists = await productReadRepository.ExistsWithNameAsync(request.Input.Name, cancellationToken);//bi seri db check yaptık varmı yokmu diye

        if (productExists)
        {
            throw new DuplicateProductNameException();//varsa hata fırlattık
        }

        // Domain validation ProductCreateModel içinde yapılacak
        var productCreateModel = new ProductCreateModel//inputtan gelen verilerle yeni bir ProductCreateModel oluşturuyoruz
        {
            Name = request.Input.Name,
            Price = request.Input.Price,
            Stock = request.Input.Stock
        };

        var product = Product.Create(productCreateModel);//domain modelini oluuruyoruz. Product sınıfının Create metodu ile ProductCreateModel'i kullanarak yeni bir Product nesnesi oluşturuyoruz.

        await productRepository.CreateAsync(product, cancellationToken).ConfigureAwait(false);//db ye kaydet için repositorye gönderiyoruz
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);//tüm değişiklikleri kaydet


    }
}