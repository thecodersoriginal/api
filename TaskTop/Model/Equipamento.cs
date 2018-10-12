using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class Equipamento
    {
        public Equipamento()
        {
            SubTarefaEquipamentos = new HashSet<SubTarefaEquipamentos>();
        }

        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public bool EmUso { get; set; }

        public ICollection<SubTarefaEquipamentos> SubTarefaEquipamentos { get; set; }
    }
}
