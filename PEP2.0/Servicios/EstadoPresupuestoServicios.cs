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
    /// 04/oct/2019
    /// clase para administrar los servicios del estado presupuesto
    /// </summary>
    public class EstadoPresupuestoServicios
    {
        EstadoPresupuestoDatos estadoPresupuestoDatos = new EstadoPresupuestoDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: obtiene el estado segun la palabra ingresada
        /// Requiere: String de desc estado
        /// Modifica: -
        /// Devuelve: estado presupuesto
        /// </summary>
        /// <returns></returns>
        public EstadoPresupuesto getEstadoPresupuestoPorNombre(String descEstado)
        {
            return estadoPresupuestoDatos.getEstadoPresupuestoPorNombre(descEstado);
        }
    }
}
