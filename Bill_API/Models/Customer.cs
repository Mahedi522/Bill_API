using System;
using System.Collections.Generic;

namespace Bill_API.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Inventories = new HashSet<Inventory>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Address { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
    }
}
