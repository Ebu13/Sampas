using System;
using System.Collections.Generic;

namespace Northwind.Models;

public partial class MessagesCustomerToEmployee
{
    public int MessageId { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public string? MessageText { get; set; }

    public DateTime? SentDate { get; set; }
}
