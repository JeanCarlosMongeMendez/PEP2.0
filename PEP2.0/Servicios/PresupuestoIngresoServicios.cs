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
    /// 08/oct/2019
    /// Clase para administrar los servicios de presupuesto de ingresos
    /// </summary>
    public class PresupuestoIngresoServicios
    {
        PresupuestoIngresoDatos presupuestoIngresoDatos = new PresupuestoIngresoDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: devuelve lista de presupuestos de ingresos segun el proyecto ingresado
        /// Requiere: proyecto a consultar
        /// Modifica: -
        /// Devuelve: lista de presupuestos de ingresos
        /// </summary>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<PresupuestoIngreso> getPresupuestosIngresosPorProyecto(Proyectos proyecto)
        {
            return presupuestoIngresoDatos.getPresupuestosIngresosPorProyecto(proyecto); ;
        }

        /// <summary>
        /// Inserta el presupuesto de ingreso
        /// </summary>
        /// <param name="presupuestoIngreso">Presupuesto de ingreso a insertar</param>
        public int InsertarPresupuestoIngreso(PresupuestoIngreso presupuestoIngreso)
        {
            return presupuestoIngresoDatos.InsertarPresupuestoIngreso(presupuestoIngreso);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/oct/2019
        /// Efecto: actualiza dado de monto del presupuesto de ingreso 
        /// Requiere: presupuesto de ingreso a modificar
        /// Modifica: dato de monto del presupuesto ingreso
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoIngreso"></param>
        /// <returns></returns>
        public void actualizarPresupuestoIngreso(PresupuestoIngreso presupuestoIngreso)
        {
            presupuestoIngresoDatos.actualizarPresupuestoIngreso(presupuestoIngreso);
        }

        /// <summary>
        /// Eliminar un presupuesto de ingreso
        /// </summary>
        /// <param name="idPresupuesto">Id del presupuesto a eliminar</param>
        public int EliminarPresupuestoIngreso(int idPresupuesto)
        {
            return presupuestoIngresoDatos.EliminarPresupuestoIngreso(idPresupuesto); ;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 02/oct/2019
        /// Efecto: actualiza dado de estado del presupuesto de ingreso 
        /// Requiere: presupuesto de ingreso a modificar
        /// Modifica: dato de estado del presupuesto ingreso
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoIngreso"></param>
        /// <returns></returns>
        public void actualizarEstadoPresupuestoIngreso(PresupuestoIngreso presupuestoIngreso)
        {
            presupuestoIngresoDatos.actualizarEstadoPresupuestoIngreso(presupuestoIngreso);
        }
    }
}
