using System;
using System.Collections.Generic;

namespace TaskTop.DTO
{
    public class TaskEquipment
    {
        public int id { get; set; }
        public string description { get; set; }
    }

    public class TaskMaterial
    {
        public int id { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
    }

    public class TaskDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime scheduledAt { get; set; }
        public DateTime? startedAt { get; set; }
        public DateTime? finishedAt { get; set; }
        public DateTime? interruptedAt { get; set; }

        public int senderId { get; set; }
        public string senderName { get; set; }
        public int receiverId { get; set; }
        public string receiverName { get; set; }
        public int? repeatInDays { get; set; }
        
        public List<TaskEquipment> equipments { get; set; }
        public List<TaskMaterial> materials { get; set; }
    }
}
