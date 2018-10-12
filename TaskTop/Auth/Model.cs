namespace TaskTop.Authorization.Model
{
    public enum UsuarioTipo
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
        public string Nome { get; set; }
        public string Email { get; set; }
        public UsuarioTipo Tipo { get; set; }
    }
}
