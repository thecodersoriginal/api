using System;
using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class Material
    {
        public Material()
        {
            EstoqueHistorico = new HashSet<EstoqueHistorico>();
            TarefaMateriais = new HashSet<TarefaMateriais>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeAtual { get; set; }

        public ICollection<EstoqueHistorico> EstoqueHistorico { get; set; }
        public ICollection<TarefaMateriais> TarefaMateriais { get; set; }
    }
}
