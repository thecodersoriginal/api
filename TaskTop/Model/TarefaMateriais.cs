using System;
using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class TarefaMateriais
    {
        public int TarefaId { get; set; }
        public int MaterialId { get; set; }
        public int Quantidade { get; set; }

        public Material Material { get; set; }
        public Tarefa Tarefa { get; set; }
    }
}
