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
    /// 30/sep/2019
    /// Clase para administrar los servicios de estado de presupuesto de ingresos
    /// </summary>
    public class EstadoPresupIngresoServicios
    {
        EstadoPresupIngresoDatos estadoPresupIngresoDatos = new EstadoPresupIngresoDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 30/sep/2019
        /// Efecto: obtiene el estado segun la palabra ingresada
        /// Requiere: String de desc estado
        /// Modifica: -
        /// Devuelve: estado presupuesto ingreso
        /// </summary>
        /// <returns></returns>
        public EstadoPresupIngreso getEstadoPresupIngresoPorNombre(String descEstado)
        {
            return estadoPresupIngresoDatos.getEstadoPresupIngresoPorNombre(descEstado);
        }
    }
}
