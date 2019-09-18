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
    /// 17/may/2019
    /// Clase para administrar los servicios de Periodo
    /// </summary>
    public class PeriodoServicios
    {
        PeriodoDatos periodoDatos = new PeriodoDatos();

        public LinkedList<Periodo> ObtenerTodos()
        {
            return periodoDatos.ObtenerTodos();
        }

        //public LinkedList<Periodo> ObtenerPeriodosSiguientes()
        //{
        //    return periodoDatos.ObtenerPeriodosSiguientes();
        //}

        public int Insertar(Periodo periodo)
        {
            return periodoDatos.Insertar(periodo);
        }

        public bool HabilitarPeriodo(int anoPeriodo)
        {
            return periodoDatos.HabilitarPeriodo(anoPeriodo);
        }

        public void EliminarPeriodo(int anoPeriodo)
        {
            periodoDatos.EliminarPeriodo(anoPeriodo);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/sep/2019
        /// Efecto: devuelve los periodos que se encuentran en las planillas de fundevi
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de periodos
        /// </summary>
        /// <returns></returns>
        public List<Periodo> getPeriodosPlanillasFundevi()
        {
            return periodoDatos.getPeriodosPlanillasFundevi();
        }
    }
}
