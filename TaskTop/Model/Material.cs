using System.Collections.Generic;

namespace TaskTop.Model
{
    public partial class Material
    {
        public Material()
        {
            EstoqueHistorico = new HashSet<EstoqueHistorico>();
            SubTarefaMateriais = new HashSet<SubTarefaMateriais>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeAtual { get; set; }

        public ICollection<EstoqueHistorico> EstoqueHistorico { get; set; }
        public ICollection<SubTarefaMateriais> SubTarefaMateriais { get; set; }
    }
}
