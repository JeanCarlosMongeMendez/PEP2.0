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
    /// 14/jun/2019
    /// Clase para administrar los servicios de planilla
    /// </summary>
    public class PlanillaServicios
    {
        PlanillaDatos planillaDatos = new PlanillaDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 14/jun/2019
        /// Efecto: obtiene todas las planillas de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de planillas
        /// </summary>
        /// <returns></returns>
        public List<Planilla> getPlanillas()
        {
            return planillaDatos.getPlanillas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 14/jun/2019
        /// Efecto: inserta en la base de datos una planilla
        /// Requiere: planilla
        /// Modifica: -
        /// Devuelve: id de la planilla insertada
        /// </summary>
        /// <param name="planilla"></param>
        /// <returns></returns>
        public int insertarPlanilla(Planilla planilla)
        {
            return planillaDatos.insertarPlanilla(planilla);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 08/jul/2019
        /// Efecto: actualiza la planilla
        /// Requiere: planilla
        /// Modifica: la planilla que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="planilla"></param>
        public void actualizarPlanilla(Planilla planilla)
        {
            planillaDatos.actualizarPlanilla(planilla);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/jul/2019
        /// Efecto: eliminar la planilla
        /// Requiere: planilla
        /// Modifica: la planilla que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="planilla"></param>
        public void eliminarPlanilla(Planilla planilla)
        {
            planillaDatos.eliminarPlanilla(planilla);
        }

    }
}