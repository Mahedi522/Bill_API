using System;
using System.Collections.Generic;

namespace Bill_API.Models
{
    public partial class Inventory
    {
        public Inventory()
        {
            InventoryProducts = new HashSet<InventoryProduct>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int BillNo { get; set; }
        public int CustomerId { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalBillAmount { get; set; }
        public double DueAmount { get; set; }
        public double PaidAmount { get; set; }

        public virtual Customer? Customer { get; set; } = null!;
        public virtual ICollection<InventoryProduct>? InventoryProducts { get; set; }
    }
}
