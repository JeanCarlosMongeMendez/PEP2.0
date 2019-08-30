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
            return this.presupuestoDatos.ObtenerPorUnidad(idUnidad);
        }

        public int InsertarPresupuestoEgreso(PresupuestoEgreso presupuestoEgreso)
        {
            return this.presupuestoDatos.InsertarPresupuestoEgreso(presupuestoEgreso);
        }

        public int AprobarPresupuestoEgreso(int idPresupuestoEgreso)
        {
            return this.presupuestoDatos.AprobarPresupuestoEgreso(idPresupuestoEgreso);
        }
        #endregion
    }
}
