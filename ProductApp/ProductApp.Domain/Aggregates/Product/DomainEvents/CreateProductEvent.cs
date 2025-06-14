namespace ProductApp.Domain.Aggregates.Product.DomainEvents//domain eventin propertyleri
{
    public record CreateProductEvent
    {
        public Guid ProductId { get; init; }
        public DateTime OccurredOn { get; init; }
    }
}


//TODO: shared bir projecta taşıyabilirim. DomainEventBase sınıfını kullanabilirim.