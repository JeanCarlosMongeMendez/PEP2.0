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
        public List<PlanillaFundevi> GetPlanillaFundevi()
        {
            return datos.GetPlanillaFundevi();
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
    }
}