namespace Northwind.Business.Request
{
    public class SupplierDetailRequestDTO
    {
        public int? SupplierID { get; set; }
        public string? SupplierName { get; set; }
        public string? ContactName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public int? ProductID { get; set; }
        public string? ProductName { get; set; }
        public int? CategoryID { get; set; }
        public string? CategoryName { get; set; }
    }
}
