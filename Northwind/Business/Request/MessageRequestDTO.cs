namespace Northwind.Business.Request
{
    public class MessageRequestDTO
    {
        public int MessageId { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public string? MessageText { get; set; }

        public DateTime? SentDate { get; set; }
    }
}
