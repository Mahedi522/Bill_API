namespace Bill_API.DTOs
{
    public class InventoryDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int BillNo { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalBillAmount { get; set; }
        public double DueAmount { get; set; }
        public double PaidAmount { get; set; }
        public List<InventoryProductDto> InventoryProducts { get; set; }
    }
}
