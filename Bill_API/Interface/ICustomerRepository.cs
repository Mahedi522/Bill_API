using Bill_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bill_API.Repository
{
	public interface ICustomerRepository
	{
		Task<List<Customer>> GetCustomers();
		Task<Customer> GetCustomerById(int id);
		Task<Customer> CreateCustomer(Customer customer);
		Task<Customer> EditCustomer(int id, Customer customer);
		Task<bool> DeleteCustomer(int id);
		Task<CustomersNames> GetCustomersNames();

    }
}
