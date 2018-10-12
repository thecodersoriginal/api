namespace TaskTop.Model
{
    public partial class SubTarefaEquipamentos
    {
        public int SubTarefaId { get; set; }
        public int EquipamentoId { get; set; }

        public Equipamento Equipamento { get; set; }
        public SubTarefa SubTarefa { get; set; }
    }
}
