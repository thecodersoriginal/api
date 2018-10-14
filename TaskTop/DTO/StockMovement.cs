using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTop.DTO
{
    public class StockMovement
    {
        public int id { get; set; }
        public int quantity { get; set; }
        public string type { get; set; }
        public int userId { get; set; }
        public int materialId { get; set; }
        public int taskId { get; set; }
        public DateTime data { get; set; }
    }
}
