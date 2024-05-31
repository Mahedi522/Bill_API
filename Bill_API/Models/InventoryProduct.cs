using System;
using System.Collections.Generic;

namespace Bill_API.Models
{
    public partial class InventoryProduct
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public double Rate { get; set; }
        public int Qty { get; set; }
        public double Discount { get; set; }

        public virtual Inventory Inventory { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
