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
    /// Clase para administrar los servicios de Presupuesto egreso partida
    /// </summary>
    public class PresupuestoEgreso_PartidaServicios
    {
        PresupuestoEgreso_PartidaDatos presupuestoEgreso_PartidaDatos = new PresupuestoEgreso_PartidaDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 08/oct/2019
        /// Efecto: devuelve la lista de partidas con los montos segun el presupuesto de egreso
        /// Requiere: presupuesto de egreso
        /// Modifica: -
        /// Devuelve: lista de presupuesto egreso partidas
        /// </summary>
        /// <param name="presupuestoEgreso"></param>
        /// <returns></returns>
        public List<PresupuestoEgresoPartida> getPresupuestoEgresoPartidas(PresupuestoEgreso presupuestoEgreso)
        {
            return presupuestoEgreso_PartidaDatos.getPresupuestoEgresoPartidas(presupuestoEgreso);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 08/oct/2019
        /// Efecto: inserta en la base de datos la relacion entre presupuesto de egresos y partida
        /// Requiere: presupuesto de egreso partida
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoEgresoPartida"></param>
        public void insertarPresupuestoEgreso_Partida(PresupuestoEgresoPartida presupuestoEgresoPartida)
        {
            presupuestoEgreso_PartidaDatos.insertarPresupuestoEgreso_Partida(presupuestoEgresoPartida);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 09/oct/2019
        /// Efecto: devuelve lista de presupuestos de egresos partidas segun los datos consultados
        /// Requiere: presupuesto de egreso y partida
        /// Modifica: -
        /// Devuelve: lista de presupuesto de egresos
        /// </summary>
        /// <param name="presupuestoEgresoPartidaConsulta"></param>
        /// <returns></returns>
        public List<PresupuestoEgresoPartida> getPresupuestoEgresoPartidasPorPartidaYPresupEgreso(PresupuestoEgresoPartida presupuestoEgresoPartidaConsulta)
        {
            return presupuestoEgreso_PartidaDatos.getPresupuestoEgresoPartidasPorPartidaYPresupEgreso(presupuestoEgresoPartidaConsulta);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto: actualiza los datos del presupuesto de egresos partida 
        /// Requiere: presupuesto de egresos partida
        /// Modifica: monto, descripcion y estado
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoEgresoPartida"></param>
        public void actualizarPresupuestoEgreso_Partida(PresupuestoEgresoPartida presupuestoEgresoPartida)
        {
            presupuestoEgreso_PartidaDatos.actualizarPresupuestoEgreso_Partida(presupuestoEgresoPartida);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto: elimina de la base de datos el presupuesto de egreso partida 
        /// Requiere: presupuesto de egreso partida
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoEgresoPartida"></param>
        public void eliminarPresupuestoEgreso_Partida(PresupuestoEgresoPartida presupuestoEgresoPartida)
        {
            presupuestoEgreso_PartidaDatos.eliminarPresupuestoEgreso_Partida(presupuestoEgresoPartida);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto: actualiza el estado del presupuesto de egreso partida
        /// Requiere: presupuesto egreso partida
        /// Modifica: el estado
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoEgresoPartida"></param>
        public void actualizarEstadoPresupuestoEgreso_Partida(PresupuestoEgresoPartida presupuestoEgresoPartida)
        {
            presupuestoEgreso_PartidaDatos.actualizarEstadoPresupuestoEgreso_Partida(presupuestoEgresoPartida);
        }
    }
}
