using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using AccesoDatos;

namespace Servicios
{
    public class EstadoEjecucionServicios
    {
        EstadoEjecucionDatos estadoEjecucionDatos = new EstadoEjecucionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 10/mar/2021
        /// Efecto: devuelve el estado segun la descripcion ingresada
        /// Requiere: nombre estado
        /// Modifica: -
        /// Devuelve: estadoEjecucion
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        public EstadoEjecucion getEstadoEjecucionSegunNombre(String estado)
        {
            return estadoEjecucionDatos.getEstadoEjecucionSegunNombre(estado);
        }

    }
}
