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
    /// Clase para administrar los servicios de presupuesto de egresos
    /// </summary>
    public class PresupuestoEgresosServicios
    {
        PresupuestoEgresoDatos presupuestoEgresoDatos = new PresupuestoEgresoDatos();

        /// <summary>
        /// Inserta un nuevo presupuesto de egreso
        /// </summary>
        /// <param name="presupuestoEgreso">Presupuesto de Egreso</param>
        public int InsertarPresupuestoEgreso(PresupuestoEgreso presupuestoEgreso)
        {
            return presupuestoEgresoDatos.InsertarPresupuestoEgreso(presupuestoEgreso); ;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: actualiza dado de plan estrategico del presupuesto de egreso 
        /// Requiere: presupuesto de egreso a modificar
        /// Modifica: dato de plan estrategico del presupuesto egreso
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoEgreso"></param>
        /// <returns></returns>
        public void actualizarPlanEstrategicoPresupuestoEgreso(PresupuestoEgreso presupuestoEgreso)
        {
            presupuestoEgresoDatos.actualizarPlanEstrategicoPresupuestoEgreso(presupuestoEgreso);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 08/oct/2019
        /// Efecto: devuelve los presupuestos de egresos de la unidad seleccionada
        /// Requiere: unidad
        /// Modifica: -
        /// Devuelve: lista de presupuestos de egresos
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public List<PresupuestoEgreso> getPresupuestosEgresosPorUnidad(Unidad unidad)
        {
            return presupuestoEgresoDatos.getPresupuestosEgresosPorUnidad(unidad);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/oct/2019
        /// Efecto: actualiza el monto total del presupuesto de egresos
        /// Requiere: presupuesto de egresos
        /// Modifica: monto total
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoEgreso"></param>
        public void actualizarMontoPresupuestoEgreso(PresupuestoEgreso presupuestoEgreso)
        {
            presupuestoEgresoDatos.actualizarMontoPresupuestoEgreso(presupuestoEgreso);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/oct/2019
        /// Efecto: devuelve el presupuesto de egreso segun el id consultado
        /// Requiere: presupuesto de egreso
        /// Modifica: -
        /// Devuelve: presupuesto de egreso
        /// </summary>
        /// <param name="presupuestoEgresoConsulta"></param>
        /// <returns></returns>
        public PresupuestoEgreso getPresupuestosEgresosPorId(PresupuestoEgreso presupuestoEgresoConsulta)
        {
            return presupuestoEgresoDatos.getPresupuestosEgresosPorId(presupuestoEgresoConsulta);
        }
    }
}
