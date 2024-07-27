namespace Northwind.Business.Request
{
    public class ComprehensiveOrderDetailRequestDTO
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public string? EmployeeFirstName { get; set; }
        public string? EmployeeLastName { get; set; }
        public int Quantity { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return Quantity * Price;
            }
        }
    }
}
