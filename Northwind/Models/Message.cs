using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Northwind.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public string? MessageText { get; set; }

    public DateTime? SentDate { get; set; }

    public virtual Employee Receiver { get; set; } = null!;

    public virtual Employee Sender { get; set; } = null!;
}
