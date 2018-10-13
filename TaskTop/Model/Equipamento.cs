using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class Equipamento
    {
        public Equipamento()
        {
            TarefaEquipamentos = new HashSet<TarefaEquipamentos>();
        }

        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public bool EmUso { get; set; }
        public bool Ativo { get; set; }

        public ICollection<TarefaEquipamentos> TarefaEquipamentos { get; set; }
    }
}
