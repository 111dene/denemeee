namespace ProductApp.Api.ServiceRegistrations
{
    public class MessagingOptions
    {
        public const string Messaging = "Messaging";//const

        public MassTransitOptions MassTransit { get; set; }//
    }

    public class MassTransitOptions///
    {
        public bool Enabled { get; set; }//true ise masstransit kullanılır, false ise kullanılmaz
        public string Host { get; set; }// RabbitMQ host adresi
        public string Username { get; set; }// RabbitMQ kullanıcı adı (guest)
        public string Password { get; set; }// RabbitMQ şifresi (guest)
    }
}


//masstransit bir message bus kütüphanesidir
//masstransit yapınlandırmalarını tutar