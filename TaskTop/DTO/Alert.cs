using System;

namespace TaskTop.DTO
{
    public class Alert
    {
        public int id { get; set; }
        public string message { get; set; }
        public int senderId { get; set; }
        public string senderName { get; set; }
        public int receiverId { get; set; }
        public string receiverName { get; set; }
        public DateTime? readAt { get; set; }
    }
}
