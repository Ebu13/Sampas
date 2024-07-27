using Northwind.Models;

namespace Northwind.Business.Request
{
    public class ProductDetailRequestDTO
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Unit { get; set; }
        public decimal? Price { get; set; }
        public string? CategoryName { get; set; }
        public string? SupplierContactName { get; set; }
    }
}
