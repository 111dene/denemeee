namespace ProductApp.Domain.Aggregates.Product.DomainEvents
{
    public record ReduceStockEvent
    {
        public Guid ProductId { get; init; }
        public string ProductName { get; init; }
        public int ReducedQuantity { get; init; }
        public int RemainingStock { get; init; }
        public string OrderId { get; init; }
        public DateTime OccurredOn { get; init; }
        public long SequenceNumber { get; init; } // Sıralama için kritik
    }
}