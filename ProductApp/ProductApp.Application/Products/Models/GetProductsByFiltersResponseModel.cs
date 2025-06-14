using ProductApp.Domain.Aggregates.Product;

namespace ProductApp.Application.Products.Models;

public class GetProductsByFiltersResponseModel
{
    public List<Product> Products { get; set; } = new();// Başlangıçta boş bir liste olarak tanımlanır, böylece null referans hatası önlenir
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}