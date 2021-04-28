using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
   public  class EstadoCajaChicaServicios
    {
        EstadoCajaChicaDatos estadoCajaChicaDatos = new EstadoCajaChicaDatos();

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
        public EstadoCajaChica getEstadoCajaChicaSegunNombre(String estado)
        {
            return estadoCajaChicaDatos.getEstadoCajaChicaSegunNombre(estado);
        }
    }
}
