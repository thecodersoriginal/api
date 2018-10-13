using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTop.DTO
{
    public class TaskDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime scheduledIn { get; set; }
        public DateTime? iniciatedIn { get; set; }
        public DateTime? finishedIn { get; set; }
        public int origin { get; set; }
        public int receiver { get; set; }
        public int? repeatIn { get; set; }
        public DateTime? interruptedIn { get; set; }

        public User DestinoNavigation { get; set; }
        public User OrigemNavigation { get; set; }
        public List<TaskEquipment> taskEquipments { get; set; }
        public List<TaskMaterial> taskMaterials { get; set; }
    }
}
