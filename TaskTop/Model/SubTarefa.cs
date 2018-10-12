using System;
using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class SubTarefa
    {
        public SubTarefa()
        {
            SubTarefaEquipamentos = new HashSet<SubTarefaEquipamentos>();
            SubTarefaMateriais = new HashSet<SubTarefaMateriais>();
        }

        public int Id { get; set; }
        public DateTime IniciadoEm { get; set; }
        public DateTime FinalizadoEm { get; set; }
        public int TarefaId { get; set; }
        public int UsuarioId { get; set; }

        public Tarefa Tarefa { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<SubTarefaEquipamentos> SubTarefaEquipamentos { get; set; }
        public ICollection<SubTarefaMateriais> SubTarefaMateriais { get; set; }
    }
}
