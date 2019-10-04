using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class PartidaPresupuestoEgresoServicio
    {
        PartidaPresupuestoEgresoDatos partidaPresupuestoEgreso = new PartidaPresupuestoEgresoDatos();
        public LinkedList<PartidaPresupuestoEgreso> ObtenerPartidaPresupuestoEgresoDatosPorPeriodo(int anoPeriodo)
        {
            return partidaPresupuestoEgreso.ObtenerPartidaPresupuestoEgresoDatosPorPeriodo(anoPeriodo);
        }
     }
}
