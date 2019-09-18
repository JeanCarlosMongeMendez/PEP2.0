using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using Entidades;

namespace Servicios
{
    /// <summary>
    /// Juan Solano Brenes
    /// 20/06/2019
    /// Administra los planillasFundevi
    /// </summary>
    public class PlanillaFundeviServicios
    {
        PlanillaFundeviDatos datos = new PlanillaFundeviDatos();

        public Boolean Insertar(int anoPeriodo)
        {
            return datos.Insertar(anoPeriodo);
        }
        public List<PlanillaFundevi> GetPlanillasFundevi()
        {
            return datos.GetPlanillasFundevi();
        }

        public PlanillaFundevi GetPlanilla(int periodo)
        {
            return datos.getPlanilla(periodo);
        }

        public PlanillaFundevi getPlanilla(int ano)
        {
            return datos.getPlanilla(ano);
        }

        public Boolean EliminarPlanilla(int iPlanilla)
        {
            return datos.eliminarPlanilla(iPlanilla);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/sep/2019
        /// Efecto: devuelve 'True' si existe una planilla fundevi con el periodo seleccionado
        /// Requiere: periodo a consultar
        /// Modifica: -
        /// Devuelve: true si existe una planilla con el periodo seleccionado o false de lo contrario
        /// </summary>
        /// <param name="anno"></param>
        /// <returns></returns>
        public Boolean existePlanilla(int anno)
        {
            return datos.existePlanilla(anno);
        }
    }
}