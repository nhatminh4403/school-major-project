namespace school_major_project.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsUser { get; set; }
    }
}