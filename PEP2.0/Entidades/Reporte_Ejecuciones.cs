using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Reporte_Ejecuciones
    {
        public String numeroPartidaPadre { get; set; }
        public String descripcionPartidaPadre { get; set; }
        public String numeroPartidaHija { get; set; }
        public String descripcionPartidaHija { get; set; }
        public String nombreUnidad { get; set; }
        public Double monto { get; set; }
        public Double montoEjecutado { get; set; }
        public int idPartida { get; set; }
        public int idUnidad { get; set; }
    }
}
