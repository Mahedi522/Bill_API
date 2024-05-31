using Bill_API.DTOs;
using Bill_API.Models;
using Bill_API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bill_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InventoryAPIController : ControllerBase
	{
		private readonly IInventoryRepository _inventoryRepository;

		public InventoryAPIController(IInventoryRepository inventoryRepository)
		{
			_inventoryRepository = inventoryRepository;
		}
		[HttpGet]
        public async Task<ActionResult<List<Inventory>>> GetInventories()
        {
            var products = await _inventoryRepository.GetInventories();
            return Ok(products);
        }
        [HttpGet(nameof(GetInventoryById) + "/{id}")]
        public async Task<ActionResult<Inventory>> GetInventoryById(int id)
        {
            var inventory = await _inventoryRepository.GetInventoryById(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult<Inventory>> EditInventoryNoJoin(int id, Inventory inventory)
        //{
        //    if (id != inventory.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var updatedProduct = await _inventoryRepository.EditInventoryNoJoin(id, inventory);
        //    return Ok(updatedProduct);
        //}

        [HttpDelete(nameof(DeleteInventory) + "/{id}")]
        public async Task<ActionResult> DeleteInventory(int id)
        {
            var result = await _inventoryRepository.DeleteInventory(id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }


        [HttpGet(nameof(GetInfoByBillNo) + "/{billNo}")]
		public async Task<ActionResult<InventoryDto>> GetInfoByBillNo(int billNo)
		{
			var inventoryDto = await _inventoryRepository.GetInfoByBillNo(billNo);

			if (inventoryDto == null)
			{
				return NotFound();
			}

			return Ok(inventoryDto);
		}

		[HttpPost(nameof(CreateInventory))]
		public async Task<ActionResult<InventoryDto>> CreateInventory(InventoryDto inventoryDto)
		{
			if (inventoryDto == null)
			{
				return BadRequest("Inventory data is null");
			}

			var createdInventoryDto = await _inventoryRepository.CreateInventory(inventoryDto);

			return CreatedAtAction(nameof(GetInfoByBillNo), new { billNo = createdInventoryDto.BillNo }, createdInventoryDto);
		}
        [HttpPut("{inventoryId}")]
        public async Task<ActionResult<InventoryDto>> EditInventory(int inventoryId, InventoryDto inventoryDto)
        {
            try
            {
                var updatedInventoryDto = await _inventoryRepository.EditInventory(inventoryId, inventoryDto);
                return Ok(updatedInventoryDto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpPut(nameof(EditInventory) + "/{inventoryId}")]
        //public async Task<ActionResult<InventoryDto>> EditInventory(int inventoryId, InventoryDto inventoryDto)
        //{
        //    try
        //    {
        //        var updatedInventoryDto = await _inventoryRepository.EditInventory(inventoryId, inventoryDto);
        //        return Ok(updatedInventoryDto);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }
}