using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class PresupuestoEgreso
    {
        public int idPresupuestoEgreso { get; set; }
        public int idUnidad { get; set; }
        public string planEstrategicoOperacional { get; set; }
        public bool estado { get; set; }
        public string descripcionEstado { get; set; }
        public double montoTotal { get; set; }
        public string descripcion { get; set; }
        public LinkedList<PresupuestoEgresoPartida> presupuestoEgresoPartidas { get; set; }
    }
}
