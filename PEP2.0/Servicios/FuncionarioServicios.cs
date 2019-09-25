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

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 18/09/2019
        /// Efecto : Guarda un funcionario
        /// Requiere : Funcionario que se desea guardar
        /// Modifica : Funcionarios
        /// Devuelve : true si se insertó correctamente, false si falló
        /// </summary>
        /// <param name="funcionario"></param>
        /// <returns></returns>
        public bool guardar(Funcionario funcionario)
        {
            return funcionarioDatos.guardar(funcionario);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 25/09/2019
        /// Efecto : Modifica un funcionario
        /// Requiere : Funcionario que se desea modificar
        /// Modifica : Funcionarios
        /// Devuelve : true si se modificó correctamente, false si falló
        /// </summary>
        /// <param name="funcionario"></param>
        /// <returns></returns>
        public bool modificar(Funcionario funcionario)
        {
            return funcionarioDatos.modificar(funcionario);
        }
    }
}