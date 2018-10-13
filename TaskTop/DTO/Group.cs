using System.Collections.Generic;

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
