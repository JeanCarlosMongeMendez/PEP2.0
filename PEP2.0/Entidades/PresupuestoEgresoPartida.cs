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
        public int idPartida { get; set; }
        public double monto { get; set; }
        public string descripcion { get; set; }
    }
}
