using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class PresupuestoEgresoPartida
    {
        public int idPresupuestoEgreso { get; set; }
        public Partida partida { get; set; }
        public Double monto { get; set; }
        public String descripcion { get; set; }
        public EstadoPresupuesto estadoPresupuesto { get; set; }
        public int idLinea { get; set; }
    }
}
