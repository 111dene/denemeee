namespace ProductApp.Domain.Aggregates.Product.DomainEvents
{
    public record AddProductEvent
    {
        public Guid ProductId { get; init; }
        public DateTime OccurredOn { get; init; }
    }
}
