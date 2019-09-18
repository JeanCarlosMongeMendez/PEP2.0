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
    /// Adrián Serrano
    /// 24/may/2019
    /// Clase para administrar los servicios de Presupuesto
    /// </summary>
    public class PresupuestoServicios
    {
        PresupuestoDatos presupuestoDatos = new PresupuestoDatos();

        #region PRESUPUESTO DE INGRESO
        public int InsertarPresupuestoIngreso(PresupuestoIngreso presupuestoIngreso)
        {
            return this.presupuestoDatos.InsertarPresupuestoIngreso(presupuestoIngreso);
        }

        public LinkedList<PresupuestoIngreso> ObtenerPorProyecto(int idProyecto)
        {
            return this.presupuestoDatos.ObtenerPorProyecto(idProyecto);
        }

        public int AprobarPresupuestoIngreso(int idPresupuesto)
        {
            return this.presupuestoDatos.AprobarPresupuestoIngreso(idPresupuesto);
        }

        public PresupuestoIngreso ObtenerPorId(int idPresupuesto)
        {
            return this.presupuestoDatos.ObtenerPorId(idPresupuesto);
        }

        public int EliminarPresupuestoIngreso(int idPresupuesto)
        {
            return this.presupuestoDatos.EliminarPresupuestoIngreso(idPresupuesto);
        }
        #endregion

        #region PRESUPUESTO DE EGRESO
        public LinkedList<PresupuestoEgreso> ObtenerPorUnidad(int idUnidad)
        {
            return presupuestoDatos.ObtenerPorUnidad(idUnidad);
        }

        /// <summary>
        /// Retorna una lista de presupuestos egresos a partir de una unidad
        /// </summary>
        /// <param name="idUnidad"></param>
        /// <returns></returns>
        public LinkedList<PresupuestoEgreso> ObtenerPorUnidadEgresos(int idUnidad)
        {
            return presupuestoDatos.ObtenerPorUnidadEgresos(idUnidad);
        }

        public int InsertarPresupuestoEgreso(PresupuestoEgreso presupuestoEgreso)
        {
            return this.presupuestoDatos.InsertarPresupuestoEgreso(presupuestoEgreso);
        }

        public int AprobarPresupuestoEgreso(int idPresupuestoEgreso)
        {
            return this.presupuestoDatos.AprobarPresupuestoEgreso(idPresupuestoEgreso);
        }
        /// <summary>
        /// Josseline M
        /// Este metodo retorna una  lista de presupuestos egresos de acuerdo al proyecto perteneciente
        /// </summary>
        /// <param name="idProyecto"></param>
        /// <returns></returns>
        public LinkedList<PresupuestoEgreso> ObtenerPresupuestoPorProyecto(int idUnidad, int idProyecto)
        {
            return this.presupuestoDatos.ObtenerPresupuestoPorProyecto(idUnidad, idProyecto);
        }

        public LinkedList<PresupuestoEgresoPartida> presupuestoEgresoPartidasPorPresupuesto(PresupuestoEgresoPartida presupuestoEgresoPartidaF)
        {
            return presupuestoDatos.presupuestoEgresoPartidasPorPresupuesto(presupuestoEgresoPartidaF);
        }
        
        /// <summary>
        /// Josseline Matamoros Cordero
        /// Inserta una nueva partida a un proyecto
        /// </summary>
        /// <param name="presupuestoEgreso"></param>
        public void InsertarPresupuestoEgresoPartida(PresupuestoEgresoPartida presupuestoEgresoP)
        {
            presupuestoDatos.InsertarPresupuestoEgresoPartida(presupuestoEgresoP);
        }
        #endregion
    }
}
