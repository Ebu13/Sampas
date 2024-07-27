namespace Northwind.Business.Request
{

    public partial class EmployeeRequestDTO
    {
        public int EmployeeId { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Notes { get; set; }

        public string? Password { get; set; }
    }
}