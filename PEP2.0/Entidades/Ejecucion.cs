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
        public int idestado { get; set; }
        public  int anoPeriodo { get; set; }
        public int idProyecto { get; set; }
        public double monto { get; set; }
        public int idTipoTramite { get; set; }
        public string numeroReferencia { get; set; }
        public Partida idPartida { get; set; }
        public Unidad idUnidad { get; set; }

    }
}
