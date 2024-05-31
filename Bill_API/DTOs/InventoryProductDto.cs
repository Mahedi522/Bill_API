namespace Bill_API.DTOs
{
    public class InventoryProductDto
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public double Rate { get; set; }
        public int Qty { get; set; }
        public double Discount { get; set; }
    }
}
