using Bill_API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bill_API.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly Bill_DBContext context;

		public ProductRepository(Bill_DBContext context)
		{
			this.context = context;
		}

		public async Task<List<Product>> GetProducts()
		{
			return await context.Products.ToListAsync();
		}

		public async Task<Product> GetProductById(int id)
		{
			return await context.Products.FindAsync(id);
		}

		public async Task<Product> CreateProduct(Product product)
		{
			await context.Products.AddAsync(product);
			await context.SaveChangesAsync();
			return product;
		}

		public async Task<Product> EditProduct(int id, Product product)
		{
			context.Entry(product).State = EntityState.Modified;
			await context.SaveChangesAsync();
			return product;
		}

		public async Task<bool> DeleteProduct(int id)
		{
			var product = await context.Products.FindAsync(id);
			if (product == null)
			{
				return false;
			}
			context.Products.Remove(product);
			await context.SaveChangesAsync();
			return true;
		}
        public async Task<ProductNames> GetProductNames()
		{
            ProductNames pNames = new ProductNames();
            pNames.PNameList = new List<SelectListItem>();

            var data = await context.Products.ToListAsync();

            pNames.PNameList.Add(new SelectListItem
            {
                Text = "Select Product Name",
                Value = ""
            });

            foreach (var item in data)
            {
                pNames.PNameList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            return pNames;

        }
    }
}
