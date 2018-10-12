namespace TaskTop.Model
{
    public partial class EstoqueHistorico
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public string Tipo { get; set; }
        public int UsuarioId { get; set; }
        public int MaterialId { get; set; }
        public int TarefaId { get; set; }

        public Material Material { get; set; }
        public Tarefa Tarefa { get; set; }
        public Usuario Usuario { get; set; }
    }
}
