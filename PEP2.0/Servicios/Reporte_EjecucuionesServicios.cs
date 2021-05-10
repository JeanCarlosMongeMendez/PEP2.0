using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{

    public class Reporte_EjecucuionesServicios
    {
        Reporte_EjecucuionesDatos reporte_EgresosDatos = new Reporte_EjecucuionesDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 12/abr/2021
        /// Efecto: devuelve la lista para llenar el reporte
        /// Requiere: Periodo y proyecto
        /// Modifica: -
        /// Devuelve: lista de Reporte ejecuciones
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<Reporte_Ejecuciones> getReporteEjecuciones(Periodo periodo, Proyectos proyecto)
        {
            return reporte_EgresosDatos.getReporteEjecuciones(periodo, proyecto);
        }
    }
}
