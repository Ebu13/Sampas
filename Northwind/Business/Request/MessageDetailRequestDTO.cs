namespace Northwind.Business.Request
{
    public class MessageDetailRequestDTO
    {
        public int MessageId { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public string? MessageText { get; set; }

        public DateTime? SentDate { get; set; }

        public string? ReceiverFirstName_LastName { get; set; }

        public string? SenderFirstName_LastName { get; set; }
    }
}
