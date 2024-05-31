using Bill_API.DTOs;
using Bill_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bill_API.Repository
{
	public interface IInventoryRepository
	{
		Task<List<Inventory>> GetInventories();
		Task<Inventory> GetInventoryById(int id);
		Task<Inventory> EditInventoryNoJoin(int id, Inventory inventory);
        Task<bool> DeleteInventory(int id);
        Task<InventoryDto> GetInfoByBillNo(int billNo);
		Task<InventoryDto> CreateInventory(InventoryDto inventoryDto);
		Task<InventoryDto> EditInventory(int inventoryId, InventoryDto inventoryDto);
	}
}
