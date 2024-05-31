using Bill_API.Models;
using Bill_API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Bill_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerAPIController : ControllerBase
	{
		private readonly ICustomerRepository customerRepository;

		public CustomerAPIController(ICustomerRepository customerRepository)
		{
			this.customerRepository = customerRepository;
		}

		[HttpGet(nameof(GetCustomers))]
		public async Task<ActionResult<List<Customer>>> GetCustomers()
		{
			var customers = await customerRepository.GetCustomers();
			return Ok(customers);
		}

		[HttpGet(nameof(GetCustomerById) + "/{id}")]
		public async Task<ActionResult<Customer>> GetCustomerById(int id)
		{
			var customer = await customerRepository.GetCustomerById(id);
			if (customer == null)
			{
				return NotFound();
			}
			return Ok(customer);
		}

		[HttpPost(nameof(CreateCustomer))]
		public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
		{
			var createdCustomer = await customerRepository.CreateCustomer(customer);
			return Ok(createdCustomer);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<Customer>> EditCustomer(int id, Customer customer)
		{
			if (id != customer.Id)
			{
				return BadRequest();
			}

			var updatedCustomer = await customerRepository.EditCustomer(id, customer);
			return Ok(updatedCustomer);
		}

		[HttpDelete(nameof(DeleteCustomer) + "/{id}")]
		public async Task<ActionResult> DeleteCustomer(int id)
		{
			var result = await customerRepository.DeleteCustomer(id);
			if (!result)
			{
				return BadRequest();
			}
			return Ok();
		}
		[HttpGet(nameof(GetCustomersNames))]
        public async Task<ActionResult<CustomersNames>> GetCustomersNames()
		{
            var cNames = await customerRepository.GetCustomersNames();
            return Ok(cNames);
        }

    }
}
