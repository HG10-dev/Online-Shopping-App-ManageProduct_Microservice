using ManageProduct_Microservice.Models;
using MongoDB.Driver;

namespace ManageProduct_Microservice.Services
{
    public class ProductService: IProductService
    {
        private readonly IMongoCollection<Product> products;

        public ProductService(IMongoClient mongoClient, IProductDatabaseSettings settings) 
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            products = database.GetCollection<Product>(settings.ProductCollectionName);
        }

        public async Task CreateAsync(Product product)
        {
            await products.InsertOneAsync(product);
        }

        public async Task<List<Product>> GetAsync()
        {
            List<Product> allProducts = await products.Find(Product => true).ToListAsync();
            return allProducts;
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            Product item = await products.Find(p => p.Name.Equals(name)).FirstOrDefaultAsync();
            return item;
        }
        
        public async Task<Product> GetByIdAsync(string id)
        {
            Product item = await products.Find(p => p.Id.Equals(id)).FirstOrDefaultAsync();
            return item;
        }

        public async Task RemoveAsync(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq("Id", id);
            await products.DeleteOneAsync(filter);
        }

        public async Task UpdateAsync(string id, Product product)
        {
            await products.ReplaceOneAsync(p =>p.Id.Equals(id), product);
        }
    }
}
