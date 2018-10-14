namespace TaskTop.Authorization.Model
{
    public enum UserType
    {
        Admin = 1,
        Estoque = 2,
        Supervisor = 3,
        Funcionario = 4
    };

    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class Operator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserType Type { get; set; }
    }
}
