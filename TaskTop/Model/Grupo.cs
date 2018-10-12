using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class Grupo
    {
        public Grupo()
        {
            UsuarioGrupos = new HashSet<UsuarioGrupos>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }

        public ICollection<UsuarioGrupos> UsuarioGrupos { get; set; }
    }
}
