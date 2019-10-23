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
    /// 21/oct/2019
    /// Clase para administrar los servicios de reporte egresos
    /// </summary>
    public class Reporte_EgresosServicios
    {
        Reporte_EgresosDatos reporte_EgresosDatos = new Reporte_EgresosDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 21/oct/2019
        /// Efecto: devuelve la lista para llenar el reporte
        /// Requiere: Periodo y proyecto
        /// Modifica: -
        /// Devuelve: lista de Reporte egreso
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<Reporte_Egresos> getReporteEgresos(Periodo periodo, Proyectos proyecto)
        {
            return reporte_EgresosDatos.getReporteEgresos(periodo,proyecto);
        }
    }
}
