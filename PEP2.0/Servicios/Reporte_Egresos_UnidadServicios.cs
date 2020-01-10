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
    /// 09/ene/2020
    /// Clase para administrar los servicios de reporte egresos por unidades
    /// </summary>
    public class Reporte_Egresos_UnidadServicios
    {
        Reporte_Egresos_UnidadDatos reporte_Egresos_UnidadDatos = new Reporte_Egresos_UnidadDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 09/ene/2020
        /// Efecto: devuelve la lista para llenar el reporte
        /// Requiere: proyecto
        /// Modifica: -
        /// Devuelve: lista de Reporte egreso
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<Reporte_Egresos_Unidad> getReporteEgresosPorUnidades(Proyectos proyecto)
        {
            return reporte_Egresos_UnidadDatos.getReporteEgresosPorUnidades(proyecto);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/ene/2020
        /// Efecto: devuelve la lista para llenar el reporte
        /// Requiere: unidad
        /// Modifica: -
        /// Devuelve: lista de Reporte egreso
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public List<Reporte_Egresos_Unidad> getReporteEgresosPorUnidades(Unidad unidad)
        {
            return reporte_Egresos_UnidadDatos.getReporteEgresosPorUnidades(unidad);
        }
    }
}
