using System.Collections.Generic;

namespace TaskTop.DTO
{
    public class UserGroup
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string login { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public int type { get; set; }
        public double? rate { get; set; }
        public List<UserGroup> groups { get; set; }
    }
}