using System;
using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class Tarefa
    {
        public Tarefa()
        {
            EstoqueHistorico = new HashSet<EstoqueHistorico>();
            TarefaEquipamentos = new HashSet<TarefaEquipamentos>();
            TarefaMateriais = new HashSet<TarefaMateriais>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime AgendadaEm { get; set; }
        public DateTime IniciadoEm { get; set; }
        public DateTime FinalizadoEm { get; set; }
        public int Origem { get; set; }
        public int Destino { get; set; }
        public int? RepetirEm { get; set; }

        public Usuario DestinoNavigation { get; set; }
        public Usuario OrigemNavigation { get; set; }
        public ICollection<EstoqueHistorico> EstoqueHistorico { get; set; }
        public ICollection<TarefaEquipamentos> TarefaEquipamentos { get; set; }
        public ICollection<TarefaMateriais> TarefaMateriais { get; set; }
    }
}
