namespace ProductApp.Shared.Events
{
    public record CreateProductEvent
    {
        public Guid ProductId { get; init; }
        public DateTime OccurredOn { get; init; }
    }
}