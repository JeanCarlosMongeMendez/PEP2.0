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
    /// 15/jul/2019
    /// Clase para administrar los servicios de funcionario
    /// </summary>
    public class FuncionarioServicios
    {
        FuncionarioDatos funcionarioDatos = new FuncionarioDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 12/jun/2019
        /// Efecto: obtiene todas los funcionarios de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de funcionarios
        /// </summary>
        /// <returns></returns>
        public List<Funcionario> getFuncionarios()
        {
            return funcionarioDatos.getFuncionarios();
        }

    }
}