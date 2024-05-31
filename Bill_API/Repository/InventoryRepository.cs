using Bill_API.DTOs;
using Bill_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bill_API.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly Bill_DBContext _context;

        public InventoryRepository(Bill_DBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Inventory>> GetInventories()
        {
            return await _context.Inventories.ToListAsync();
        }
        public async Task<Inventory> GetInventoryById(int id)
        {
            return await _context.Inventories.FindAsync(id);
        }
        public async Task<Inventory> EditInventoryNoJoin(int id, Inventory inventory)
        {
            _context.Entry(inventory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return inventory;
        }
        public async Task<bool> DeleteInventory(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return false;
            }
            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<InventoryDto> GetInfoByBillNo(int billNo)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Customer)
                .Include(i => i.InventoryProducts)
                    .ThenInclude(ip => ip.Product)
                .FirstOrDefaultAsync(i => i.BillNo == billNo);

            if (inventory == null)
            {
                return null;
            }

            var inventoryDto = MapInventoryToDto(inventory);
            return inventoryDto;
        }

        public async Task<InventoryDto> CreateInventory(InventoryDto inventoryDto)
        {
            if (inventoryDto == null)
            {
                throw new ArgumentNullException(nameof(inventoryDto));
            }

            var inventory = MapDtoToInventory(inventoryDto);

            _context.Inventories.Add(inventory);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Error saving data");
            }

            var savedInventoryDto = MapInventoryToDto(inventory);
            return savedInventoryDto;
        }

        public async Task<InventoryDto> EditInventory(int inventoryId, InventoryDto inventoryDto)
        {
            var existingInventory = await _context.Inventories
                .Include(i => i.InventoryProducts)
                .FirstOrDefaultAsync(i => i.Id == inventoryId);

            if (existingInventory == null)
            {
                throw new ArgumentException($"Inventory with ID {inventoryId} not found.");
            }

            existingInventory.Date = inventoryDto.Date;
            existingInventory.BillNo = inventoryDto.BillNo;
            existingInventory.CustomerId = inventoryDto.CustomerId;
            existingInventory.TotalDiscount = inventoryDto.TotalDiscount;
            existingInventory.TotalBillAmount = inventoryDto.TotalBillAmount;
            existingInventory.DueAmount = inventoryDto.DueAmount;
            existingInventory.PaidAmount = inventoryDto.PaidAmount;

            var existingProductIds = existingInventory.InventoryProducts.Select(p => p.Id).ToList();
            var dtoProductIds = inventoryDto.InventoryProducts.Select(p => p.Id).ToList();

            foreach (var existingProduct in existingInventory.InventoryProducts.ToList())
            {
                if (!dtoProductIds.Contains(existingProduct.Id))
                {
                    _context.InventoryProducts.Remove(existingProduct);
                }
            }

            foreach (var productDto in inventoryDto.InventoryProducts)
            {
                var existingProduct = existingInventory.InventoryProducts.FirstOrDefault(p => p.Id == productDto.Id);

                if (existingProduct == null)
                {
                    existingInventory.InventoryProducts.Add(new InventoryProduct
                    {
                        ProductId = productDto.ProductId,
                        Rate = productDto.Rate,
                        Qty = productDto.Qty,
                        Discount = productDto.Discount
                    });
                }
                else
                {
                    existingProduct.ProductId = productDto.ProductId;
                    existingProduct.Rate = productDto.Rate;
                    existingProduct.Qty = productDto.Qty;
                    existingProduct.Discount = productDto.Discount;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error updating inventory", ex);
            }

            return MapInventoryToDto(existingInventory);
        }

        private InventoryDto MapInventoryToDto(Inventory inventory)
        {
            return new InventoryDto
            {
                Id = inventory.Id,
                Date = inventory.Date,
                BillNo = inventory.BillNo,
                CustomerId = inventory.CustomerId,
                CustomerName = inventory.Customer?.Name ?? "Unknown",
                TotalDiscount = inventory.TotalDiscount,
                TotalBillAmount = inventory.TotalBillAmount,
                DueAmount = inventory.DueAmount,
                PaidAmount = inventory.PaidAmount,
                InventoryProducts = inventory.InventoryProducts?.Select(ip => new InventoryProductDto
                {
                    Id = ip.Id,
                    ProductId = ip.ProductId,
                    ProductName = ip.Product?.Name ?? "Unknown",
                    Rate = ip.Rate,
                    Qty = ip.Qty,
                    Discount = ip.Discount
                }).ToList()
            };
        }

        private Inventory MapDtoToInventory(InventoryDto inventoryDto)
        {
            return new Inventory
            {
                Date = inventoryDto.Date,
                BillNo = inventoryDto.BillNo,
                CustomerId = inventoryDto.CustomerId,
                TotalDiscount = inventoryDto.TotalDiscount,
                TotalBillAmount = inventoryDto.TotalBillAmount,
                DueAmount = inventoryDto.DueAmount,
                PaidAmount = inventoryDto.PaidAmount,
                InventoryProducts = inventoryDto.InventoryProducts?.Select(ip => new InventoryProduct
                {
                    ProductId = ip.ProductId,
                    Rate = ip.Rate,
                    Qty = ip.Qty,
                    Discount = ip.Discount
                }).ToList()
            };
        }
    }
}







//using Bill_API.DTOs;
//using Bill_API.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Bill_API.Repository
//{
//	public class InventoryRepository : IInventoryRepository
//	{
//		private readonly Bill_DBContext _context;

//		public InventoryRepository(Bill_DBContext context)
//		{
//			_context = context;
//		}
//        public async Task<List<Inventory>> GetInventories()
//        {
//            return await _context.Inventories.ToListAsync();
//        }

//        public async Task<InventoryDto> GetInfoByBillNo(int billNo)
//		{
//			var inventory = await _context.Inventories
//				.Include(i => i.Customer)
//				.Include(i => i.InventoryProducts)
//					.ThenInclude(ip => ip.Product)
//				.FirstOrDefaultAsync(i => i.BillNo == billNo);

//			if (inventory == null)
//			{
//				return null;
//			}

//			// Mapping logic to InventoryDto
//			var inventoryDto = MapInventoryToDto(inventory);
//			return inventoryDto;
//		}

//		public async Task<InventoryDto> CreateInventory(InventoryDto inventoryDto)
//		{
//			if (inventoryDto == null)
//			{
//				throw new ArgumentNullException(nameof(inventoryDto));
//			}

//			// Mapping logic to Inventory entity
//			var inventory = MapDtoToInventory(inventoryDto);

//			// Add the new inventory to the context
//			_context.Inventories.Add(inventory);

//			try
//			{
//				// Save changes to the database
//				await _context.SaveChangesAsync();
//			}
//			catch (DbUpdateException)
//			{
//				// Handle any errors that may occur during database update
//				throw new InvalidOperationException("Error saving data");
//			}

//			// Mapping logic to InventoryDto
//			var savedInventoryDto = MapInventoryToDto(inventory);
//			return savedInventoryDto;
//		}

//		private InventoryDto MapInventoryToDto(Inventory inventory)
//		{
//			return new InventoryDto
//			{
//				Id = inventory.Id,
//				Date = inventory.Date,
//				BillNo = inventory.BillNo,
//				CustomerId = inventory.CustomerId,
//				CustomerName = inventory.Customer.Name,
//				TotalDiscount = inventory.TotalDiscount,
//				TotalBillAmount = inventory.TotalBillAmount,
//				DueAmount = inventory.DueAmount,
//				PaidAmount = inventory.PaidAmount,
//				InventoryProducts = inventory.InventoryProducts?.Select(ip => new InventoryProductDto
//				{
//					Id = ip.Id,
//					ProductId = ip.ProductId,
//					ProductName = ip.Product?.Name,
//					Rate = ip.Rate,
//					Qty = ip.Qty,
//					Discount = ip.Discount
//				}).ToList()
//			};
//		}

//		private Inventory MapDtoToInventory(InventoryDto inventoryDto)
//		{
//			return new Inventory
//			{
//				Date = inventoryDto.Date,
//				BillNo = inventoryDto.BillNo,
//				CustomerId = inventoryDto.CustomerId,
//				TotalDiscount = inventoryDto.TotalDiscount,
//				TotalBillAmount = inventoryDto.TotalBillAmount,
//				DueAmount = inventoryDto.DueAmount,
//				PaidAmount = inventoryDto.PaidAmount,
//				InventoryProducts = inventoryDto.InventoryProducts?.Select(ip => new InventoryProduct
//				{
//					ProductId = ip.ProductId,
//					Rate = ip.Rate,
//					Qty = ip.Qty,
//					Discount = ip.Discount
//				}).ToList()
//			};
//		}
//		public async Task<InventoryDto> EditInventory(int inventoryId, InventoryDto inventoryDto)
//		{
//			var existingInventory = await _context.Inventories
//				.Include(i => i.InventoryProducts)
//				.FirstOrDefaultAsync(i => i.Id == inventoryId);

//			if (existingInventory == null)
//			{
//				throw new ArgumentException($"Inventory with ID {inventoryId} not found.");
//			}

//			// Update properties from DTO
//			existingInventory.Date = inventoryDto.Date;
//			existingInventory.BillNo = inventoryDto.BillNo;
//			existingInventory.CustomerId = inventoryDto.CustomerId;
//			existingInventory.TotalDiscount = inventoryDto.TotalDiscount;
//			existingInventory.TotalBillAmount = inventoryDto.TotalBillAmount;
//			existingInventory.DueAmount = inventoryDto.DueAmount;
//			existingInventory.PaidAmount = inventoryDto.PaidAmount;

//			// Update related InventoryProducts
//			foreach (var productDto in inventoryDto.InventoryProducts)
//			{
//				var existingProduct = existingInventory.InventoryProducts.FirstOrDefault(p => p.Id == productDto.Id);

//				if (existingProduct == null)
//				{
//					// If the product is not found, it means it's a new product, so add it
//					existingInventory.InventoryProducts.Add(new InventoryProduct
//					{
//						ProductId = productDto.ProductId,
//						Rate = productDto.Rate,
//						Qty = productDto.Qty,
//						Discount = productDto.Discount
//					});
//				}
//				else
//				{
//					// Update existing product
//					existingProduct.ProductId = productDto.ProductId;
//					existingProduct.Rate = productDto.Rate;
//					existingProduct.Qty = productDto.Qty;
//					existingProduct.Discount = productDto.Discount;
//				}
//			}

//			// Remove any existing products not present in the DTO
//			foreach (var existingProduct in existingInventory.InventoryProducts.ToList())
//			{
//				if (!inventoryDto.InventoryProducts.Any(p => p.Id == existingProduct.Id))
//				{
//					_context.InventoryProducts.Remove(existingProduct);
//				}
//			}

//			// Save changes to the database
//			try
//			{
//				await _context.SaveChangesAsync();
//			}
//			catch (DbUpdateException ex)
//			{
//				// Log and handle any database update errors
//				throw new InvalidOperationException("Error updating inventory", ex);
//			}

//			// Return updated inventory DTO
//			return MapInventoryToDto(existingInventory);
//		}
//	}
//}
