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

        /// <summary>
        /// Josseline M
        /// Almacena un nuevo presupuesto de egreso
        /// </summary>
        /// <param name="presupuestoEgreso"></param>
        /// <returns></returns>
        public int InsertarPresupuestoEgreso(PresupuestoEgreso presupuestoEgreso)
        {
            return this.presupuestoDatos.InsertarPresupuestoEgreso(presupuestoEgreso);
        }
        
        public int AprobarPresupuestoEgreso(PresupuestoEgreso presupuesto)
        {
            return presupuestoDatos.AprobarPresupuestoEgresoPorMonto(presupuesto);
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

        /// <summary>
        /// Retorna un listado con los detalles de egresos partidas a partir de un presupesto
        /// </summary>
        /// <param name="presupuestoEgresoPartidaF"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Guarda el avance obtenido en el añadimiento de partidas
        /// </summary>
        /// <param name="presupuestoE"></param>
        public void guardarPartidasPresupuestoEgreso(LinkedList<PresupuestoEgreso> presupuestosE)
        {
            this.presupuestoDatos.guardarPartidasPresupuestoEgreso(presupuestosE);
        }

        /// <summary>
        /// Este método se encarga de actualizar las descripciones y montos de las partidas egresos
        /// </summary>
        /// <param name="presupuesto"></param>
        public void editarPresupuestoEgresoPartida(PresupuestoEgresoPartida presupuestoEgresoPartida)
        {
            this.presupuestoDatos.editarPresupuestoEgresoPartida(presupuestoEgresoPartida);
        }

            #endregion
        }
}
