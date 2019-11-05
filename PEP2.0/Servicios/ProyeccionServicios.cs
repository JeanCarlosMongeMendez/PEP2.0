using Entidades;
using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    /// <summary>
    /// Leonardo Carrion
    /// 04/nov/2019
    /// Clase para administrar los servicios de Proyeccion
    /// </summary>
    public class ProyeccionServicios
    {
        ProyeccionDatos proyeccionDatos = new ProyeccionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: obtiene todas las proyeccion de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de proyecciones
        /// </summary>
        /// <returns></returns>
        public List<Proyeccion> getProyecciones()
        {
            return proyeccionDatos.getProyecciones();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: inserta en la base de datos una proyeccion
        /// Requiere: proyeccion
        /// Modifica: -
        /// Devuelve: id de la proyeccion
        /// </summary>
        /// <param name="proyeccion"></param>
        /// <returns></returns>
        public int insertarProyeccion(Proyeccion proyeccion)
        {
            return proyeccionDatos.insertarProyeccion(proyeccion);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: actualiza la proyeccion
        /// Requiere: proyeccion
        /// Modifica: la proyeccion que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="proyeccion"></param>
        public void actualizarProyeccion(Proyeccion proyeccion)
        {
            proyeccionDatos.actualizarProyeccion(proyeccion);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: elimina la proyeccion
        /// Requiere: proyeccion
        /// Modifica: base de datos eliminando la proyeccion
        /// Devuelve: -
        /// </summary>
        /// <param name="proyeccion"></param>
        public void eliminarProyeccion(Proyeccion proyeccion)
        {
            proyeccionDatos.eliminarProyeccion(proyeccion);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: elimina las proyecciones que se encuentran en el periodo seleccionado  y del funcionario seleccionado
        /// Requiere: periodo y funcionario
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="periodo"></param>
        public void eliminarProyeccionPorPeriodo(Periodo periodo, Funcionario funcionario)
        {
            proyeccionDatos.eliminarProyeccionPorPeriodo(periodo, funcionario);
        }


        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: obtiene todas las proyeccion de la base de datos del periodo y funcionario consultado
        /// Requiere: periodo y funcionario
        /// Modifica: -
        /// Devuelve: lista de proyecciones
        /// </summary>
        /// <returns></returns>
        public List<Proyeccion> getProyeccionesPorPeriodoYFuncionario(Periodo periodoConsulta, Funcionario funcionarioConsulta)
        {
            return proyeccionDatos.getProyeccionesPorPeriodoYFuncionario(periodoConsulta,funcionarioConsulta);
        }

    }
}
