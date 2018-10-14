using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTop.DTO
{
    public class StockHistory
    {
        public int id { get; set; }
        public int quantity { get; set; }
        public string type { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public int materialId { get; set; }
        public string materialName { get; set; }
        public int taskId { get; set; }
        public string taskName { get; set; }
        public DateTime date { get; set; }
    }
}
