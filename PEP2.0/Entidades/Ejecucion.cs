using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class Ejecucion
    {
        public int idEjecucion { get; set; }
        public EstadoEjecucion estadoEjecucion { get; set; }
        public  int anoPeriodo { get; set; }
        public int idProyecto { get; set; }
        public double monto { get; set; }
        public TipoTramite tipoTramite { get; set; }
        public string numeroReferencia { get; set; }
        public Partida partida { get; set; }
        public Unidad unidad { get; set; }
        public string descripcionEjecucionOtro { get; set; }
        public String realizadoPor { get; set; }
        public DateTime fecha { get; set; }
    }
}
