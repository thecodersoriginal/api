namespace TaskTop.Model
{
    public partial class SubTarefaMateriais
    {
        public int SubTarefaId { get; set; }
        public int MaterialId { get; set; }
        public int Quantidade { get; set; }

        public Material Material { get; set; }
        public SubTarefa SubTarefa { get; set; }
    }
}
