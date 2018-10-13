using System;
using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class Tarefa
    {
        public Tarefa()
        {
            EstoqueHistorico = new HashSet<EstoqueHistorico>();
            SubTarefa = new HashSet<SubTarefa>();
        }

        public int Id { get; set; }
        public DateTime AgendadaEm { get; set; }
        public int UsuarioId { get; set; }
        public int? RepetirEm { get; set; }

        public Usuario Usuario { get; set; }
        public ICollection<EstoqueHistorico> EstoqueHistorico { get; set; }
        public ICollection<SubTarefa> SubTarefa { get; set; }
    }
}
