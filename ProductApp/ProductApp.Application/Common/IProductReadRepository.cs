using ProductApp.Application.Products.Models;
using ProductApp.Domain.Aggregates.Product;

namespace ProductApp.Application.Common
{
    public interface IProductReadRepository//okuma işlemleri için repository arayüzü
    {
        Task<GetProductsByFiltersResponseModel> GetProductsByFilters(GetProductByFilterRequestModel requestModel, CancellationToken cancellationToken);//filtreye göre ürünleri getirme işlemi
        Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken);//ürün ismine göre varlık kontrolü
        Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken);//ürün id sine göre getirme işlemi
        Task<Product> GetByNameAsync(string name, CancellationToken cancellationToken);//ürün ismine göre getirme işlemi
    }
}
//burada dönüş tipi olarak GetProductsByFiltersResponseModel kullanıyoruz ve parametre olarak GetProductByFilterRequestModel alıyoruz. Bu, ürünleri filtrelemek için gerekli olan bilgileri içerir.