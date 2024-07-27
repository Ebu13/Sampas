namespace Northwind.Business.Request
{
    public class OrderRequestDTO
    {
        public int OrderId { get; set; }

        public int? CustomerId { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime OrderDate { get; set; }

        public int? ShipperId { get; set; }
    }
}
