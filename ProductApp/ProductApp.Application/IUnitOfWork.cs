namespace ProductApp.Application
{
    public interface IUnitOfWork// Birim İş Birliği arayüzü
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);//değişiklikleri kaydetme işlemi
    }
}
