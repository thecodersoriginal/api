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
        public string usuario { get; set; }
        public string senha { get; set; }
    }

    public class Operator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserType Type { get; set; }
    }
}
