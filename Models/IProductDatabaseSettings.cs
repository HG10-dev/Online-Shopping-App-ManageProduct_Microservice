namespace ManageProduct_Microservice.Models
{
    public interface IProductDatabaseSettings
    {
        public string ProductCollectionName { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
