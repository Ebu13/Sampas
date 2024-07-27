
namespace Northwind.Business.Request
{
    public class CustomerRequestDTO
    {
        public int CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? ContactName { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? Password { get; set; }

    }
}
