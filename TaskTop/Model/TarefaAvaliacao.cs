using System;
using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class TarefaAvaliacao
    {
        public int Id { get; set; }
        public int TarefaId { get; set; }
        public int Nota { get; set; }
        public int NotaMaxima { get; set; }

        public Tarefa Tarefa { get; set; }
    }
}
