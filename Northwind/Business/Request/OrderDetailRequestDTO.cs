namespace Northwind.Business.Request
{
    public class OrderDetailRequestDTO
    {
        public int OrderDetailId { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

    }
}
