using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Reporte_Egresos_Unidad
    {
        public String nombreUnidad { get; set; }
        public String numeroPartida { get; set; }
        public String descPartida { get; set; }
        public String descPresupuesto { get; set; }
        public Double monto { get; set; }
    }
}
