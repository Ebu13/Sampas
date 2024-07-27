using System;
using System.Collections.Generic;

namespace Northwind.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? Notes { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
