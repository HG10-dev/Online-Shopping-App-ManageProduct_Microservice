using ManageProduct_Microservice.Models;

namespace ManageProduct_Microservice.Services
{
    public interface IProductService
    {
        public Task CreateAsync(Product product);
        public Task<List<Product>> GetAsync();
        public Task<Product> GetByIdAsync(string id);
        public Task<Product> GetByNameAsync(string id);
        public Task RemoveAsync(string id);
        public Task UpdateAsync(string id, Product product);

    }
}
