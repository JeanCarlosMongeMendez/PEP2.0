using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using AccesoDatos;

namespace Servicios
{
    /// <summary>
    /// Juan Pablo Solano Brenes
    /// 20/06/2019
    /// Clase que administra los funcionarios fundevi
    /// </summary>
    public class FuncionariosFundeviServicios
    {
        FuncionarioFundeviDatos fundeviDatos = new FuncionarioFundeviDatos();

        public Boolean InsertFuncionario(FuncionarioFundevi funcionario)
        {
            return fundeviDatos.InsertFuncionario(funcionario);
        }
        public List<FuncionarioFundevi> GetAll()
        {
            return fundeviDatos.GetAllFuncionario();
        }
        public List<FuncionarioFundevi> GetFuncionariosPorPlanilla(PlanillaFundevi planilla)
        {
            return fundeviDatos.GetFuncionariosPorPlanilla(planilla);
        }
        public Boolean actualizarSalario(FuncionarioFundevi funcionario, int salario)
        {
            return fundeviDatos.actualizarSalario(funcionario, salario);
        }

        public FuncionarioFundevi GetFuncionario(int idFuncionario)
        {
            return fundeviDatos.GetFuncionario(idFuncionario);
        }
        public Boolean InsertarFuncionarios(LinkedList<FuncionarioFundevi> funcionarios, int idPlanilla)
        {
            return fundeviDatos.InsertarFuncionarios(funcionarios, idPlanilla);
        }

        public Boolean EliminarFuncionario(int idPlanilla)
        {
            return fundeviDatos.EliminarFuncionarios(idPlanilla);
        }
        public Boolean EliminarUnFuncionario(FuncionarioFundevi funcionario, int idPlanilla)
        {
            return fundeviDatos.EliminarFuncionario(funcionario, idPlanilla);
        }
        public Boolean EditarFuncionario(FuncionarioFundevi funcionario)
        {
            return fundeviDatos.EditarFuncionario(funcionario);
        }
    }
}