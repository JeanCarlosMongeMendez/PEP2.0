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
    /// Jean Carlos Monge Mendez
    /// 16/10/2019
    /// Clase para administrar los servicios de JornadaUnidadFuncionarioDatos
    /// </summary>
    public class JornadaUnidadFuncionarioServicios
    {
        private JornadaUnidadFuncionarioDatos jornadaDatos = new JornadaUnidadFuncionarioDatos();

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 16/10/2019
        /// Efecto: obtiene todas las jornadaUnidadFuncionario de un funcionario de la base de datos
        /// Requiere: idFuncionario
        /// Modifica: -
        /// Devuelve: lista de jornadaUnidadFuncionario
        /// </summary>
        /// <param name="idFuncionario"></param>
        /// <param name="idProyecto"></param>
        /// <returns></returns>
        public List<JornadaUnidadFuncionario> getJornadaUnidadFuncionario(int idFuncionario, int idProyecto)
        {
            return jornadaDatos.getJornadaUnidadFuncionario(idFuncionario, idProyecto);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 16/10/2019
        /// Efecto: inserta en la base de datos una jornadaUnidadFuncionario
        /// Requiere: jornadaUnidadFuncionario
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="jornadaUnidadFuncionario"></param>
        /// <returns></returns>
        public void insertarJornadaUnidadFuncionario(JornadaUnidadFuncionario jornadaUnidadFuncionario)
        {
            jornadaDatos.insertarJornadaUnidadFuncionario(jornadaUnidadFuncionario);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 16/10/2019
        /// Efecto: actualiza la jornadaUnidadFuncionario
        /// Requiere: jornadaUnidadFuncionario
        /// Modifica: la jornada que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="jornadaUnidadFuncionario"></param>
        public void actualizarJornadaUnidadFuncionario(JornadaUnidadFuncionario jornadaUnidadFuncionario)
        {
            jornadaDatos.actualizarJornadaUnidadFuncionario(jornadaUnidadFuncionario);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 16/10/2019
        /// Efecto: elimina la jornadaUnidadFuncionario
        /// Requiere: jornadaUnidadFuncionario
        /// Modifica: base de datos eliminando la jornada
        /// Devuelve: -
        /// </summary>
        /// <param name="jornadaUnidadFuncionario"></param>
        public void eliminarJornadaUnidadFuncionario(JornadaUnidadFuncionario jornadaUnidadFuncionario)
        {
            jornadaDatos.eliminarjornadaUnidadFuncionario(jornadaUnidadFuncionario);
        }
    }
}
