using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class Usuario
    {
        public Usuario()
        {
            AlertaDestinoNavigation = new HashSet<Alerta>();
            AlertaOrigemNavigation = new HashSet<Alerta>();
            EstoqueHistorico = new HashSet<EstoqueHistorico>();
            SubTarefa = new HashSet<SubTarefa>();
            Tarefa = new HashSet<Tarefa>();
            UsuarioGrupos = new HashSet<UsuarioGrupos>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public byte[] Senha { get; set; }
        public byte[] Chave { get; set; }
        public string Permissao { get; set; }

        public ICollection<Alerta> AlertaDestinoNavigation { get; set; }
        public ICollection<Alerta> AlertaOrigemNavigation { get; set; }
        public ICollection<EstoqueHistorico> EstoqueHistorico { get; set; }
        public ICollection<SubTarefa> SubTarefa { get; set; }
        public ICollection<Tarefa> Tarefa { get; set; }
        public ICollection<UsuarioGrupos> UsuarioGrupos { get; set; }
    }
}
