using System;
using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class TarefaAvaliacao
    {
        public TarefaAvaliacao()
        {
            Tarefa = new HashSet<Tarefa>();
        }

        public int Id { get; set; }
        public int Nota { get; set; }
        public int NotaMaxima { get; set; }

        public ICollection<Tarefa> Tarefa { get; set; }
    }
}
