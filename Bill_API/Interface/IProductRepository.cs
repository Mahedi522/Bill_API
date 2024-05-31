using Bill_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bill_API.Repository
{
	public interface IProductRepository
	{
		Task<List<Product>> GetProducts();
		Task<Product> GetProductById(int id);
		Task<Product> CreateProduct(Product product);
		Task<Product> EditProduct(int id, Product product);
		Task<bool> DeleteProduct(int id);
        Task<ProductNames> GetProductNames();
    }
}
