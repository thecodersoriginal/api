using System;
using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class TarefaEquipamentos
    {
        public int TarefaId { get; set; }
        public int EquipamentoId { get; set; }

        public Equipamento Equipamento { get; set; }
        public Tarefa Tarefa { get; set; }
    }
}
