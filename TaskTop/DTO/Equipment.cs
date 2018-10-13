using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTop.DTO
{
    public class Equipment
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public bool inUse { get; set; }
        public bool active { get; set; }
    }
}
