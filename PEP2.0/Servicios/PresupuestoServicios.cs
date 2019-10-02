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
            return presupuestoDatos.getPresupuestosIngresosPorProyecto(proyecto);
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
            presupuestoDatos.actualizarPresupuestoIngreso(presupuestoIngreso);
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
            presupuestoDatos.actualizarEstadoPresupuestoIngreso(presupuestoIngreso);
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


            #endregion
        }
}
