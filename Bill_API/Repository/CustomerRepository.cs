using Bill_API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bill_API.Repository
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly Bill_DBContext context;

		public CustomerRepository(Bill_DBContext context)
		{
			this.context = context;
		}

		public async Task<List<Customer>> GetCustomers()
		{
			return await context.Customers.ToListAsync();
		}

		public async Task<Customer> GetCustomerById(int id)
		{
			return await context.Customers.FindAsync(id);
		}

		public async Task<Customer> CreateCustomer(Customer customer)
		{
			await context.Customers.AddAsync(customer);
			await context.SaveChangesAsync();
			return customer;
		}

		public async Task<Customer> EditCustomer(int id, Customer customer)
		{
			context.Entry(customer).State = EntityState.Modified;
			await context.SaveChangesAsync();
			return customer;
		}

		public async Task<bool> DeleteCustomer(int id)
		{
			var customer = await context.Customers.FindAsync(id);
			if (customer == null)
			{
				return false;
			}
			context.Customers.Remove(customer);
			await context.SaveChangesAsync();
			return true;
		}
		public async Task<CustomersNames> GetCustomersNames()
		{
			CustomersNames cNames = new CustomersNames();
			cNames.CNameList = new List<SelectListItem>();

			var data = await context.Customers.ToListAsync();

            cNames.CNameList.Add(new SelectListItem
            {
                Text = "Select Customer Name",
                Value = ""
            });

            foreach (var item in data)
			{
				cNames.CNameList.Add(new SelectListItem
				{
					Text = item.Name,
					Value = item.Id.ToString()
				});
			}
			return cNames;

        } 
	}
}
