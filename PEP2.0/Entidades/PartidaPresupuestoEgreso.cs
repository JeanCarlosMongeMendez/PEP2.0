using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{

    /// <summary>
    /// Josseline M
    /// Entidad encargada de llevar el control de asigación de partidas y su debido monto
    /// </summary>
   public class PartidaPresupuestoEgreso
    {
        public int idPartida { get; set; }
        public string numeroPartida { get; set; }
        public double montoTotal { get; set; }
        public string descripcion { get; set; }
    }
}
