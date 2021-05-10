using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ReporteDetalleEjecucion
    {
        public int idEjecucion { get; set; }
        public Double monto { get; set; }
        public Double montoEjecucion { get; set; }
        public String numeroPartida { get; set; }
        public String descPartida { get; set; }
        public String descEstado { get; set; }
        public String nombreTramite { get; set; }
    }
}
