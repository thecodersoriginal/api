using System;

namespace TaskTop.Model
{
    public partial class Alerta
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        public int Origem { get; set; }
        public int Destino { get; set; }
        public DateTime? VisualizadaEm { get; set; }

        public Usuario DestinoNavigation { get; set; }
        public Usuario OrigemNavigation { get; set; }
    }
}
