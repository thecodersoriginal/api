namespace TaskTop.Model
{
    public partial class UsuarioGrupos
    {
        public int UsuarioId { get; set; }
        public int GrupoId { get; set; }

        public Grupo Grupo { get; set; }
        public Usuario Usuario { get; set; }
    }
}
