using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTop.DTO
{
    public class GroupUser
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Group
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<GroupUser> users { get; set; }
    }
}
