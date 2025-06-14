namespace ProductEventListener.Domain.Entities
{
    public class ProductEventLog
    {
        public Guid ProductId { get; set; }
        public DateTime OccurredOn { get; set; }
        
    }
}