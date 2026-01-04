namespace laundrycucikilat.Models
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string OrdersCollectionName { get; set; } = string.Empty;
        public string ContactMessagesCollectionName { get; set; } = string.Empty;
    }
}