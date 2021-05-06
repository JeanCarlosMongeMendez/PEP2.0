using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    /// <summary>
    /// Leonardo Carrion
    /// 04/may/2021
    /// </summary>
    public class Reporte_DetalleEjecucionesServicios
    {
        Reporte_DetalleEjecucionesDatos reporte_DetalleEjecucionesDatos = new Reporte_DetalleEjecucionesDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 04/may/2021
        /// Efecto: devuelve la lista para el reporte
        /// Requiere: id_proyecto, id_año y id_unidad
        /// Modifica: -
        /// Devuelve: lista de reporte detalle ejecucion
        /// </summary>
        /// <param name="idProyecto"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="idUnidad"></param>
        /// <returns></returns>
        public List<ReporteDetalleEjecucion> getReporteEgresosPorUnidades(int idProyecto, int idPeriodo, int idUnidad)
        {
            return reporte_DetalleEjecucionesDatos.getReporteEgresosPorUnidades(idProyecto, idPeriodo, idUnidad);
        }
    }
}
