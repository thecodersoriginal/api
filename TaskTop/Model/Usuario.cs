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
            TarefaDestinoNavigation = new HashSet<Tarefa>();
            TarefaOrigemNavigation = new HashSet<Tarefa>();
            UsuarioGrupos = new HashSet<UsuarioGrupos>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public byte[] Senha { get; set; }
        public byte[] Chave { get; set; }
        public int Tipo { get; set; }

        public ICollection<Alerta> AlertaDestinoNavigation { get; set; }
        public ICollection<Alerta> AlertaOrigemNavigation { get; set; }
        public ICollection<EstoqueHistorico> EstoqueHistorico { get; set; }
        public ICollection<Tarefa> TarefaDestinoNavigation { get; set; }
        public ICollection<Tarefa> TarefaOrigemNavigation { get; set; }
        public ICollection<UsuarioGrupos> UsuarioGrupos { get; set; }
    }
}
