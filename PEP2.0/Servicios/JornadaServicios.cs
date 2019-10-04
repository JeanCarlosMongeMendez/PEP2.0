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
    /// 18/sep/2019
    /// Clase para administrar los servicios de Jornada
    /// </summary>
    public class JornadaServicios
    {
        JornadaDatos jornadaDatos = new JornadaDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 18/sep/2019
        /// Efecto: obtiene todas las jornadas de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de jornadas
        /// </summary>
        /// <returns></returns>
        public List<Jornada> getJornadasActivas()
        {
            return jornadaDatos.getJornadasActivas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 18//sep/2019
        /// Efecto: inserta en la base de datos una jornada
        /// Requiere: jornada
        /// Modifica: -
        /// Devuelve: id de la jornada insertada
        /// </summary>
        /// <param name="jornada"></param>
        /// <returns></returns>
        public int insertarJornada(Jornada jornada)
        {
            return jornadaDatos.insertarJornada(jornada);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 18/sep/2019
        /// Efecto: actualiza la jornada
        /// Requiere: jornada
        /// Modifica: la jornada que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="jornada"></param>
        public void actualizarJornada(Jornada jornada)
        {
            jornadaDatos.actualizarJornada(jornada);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 18/sep/2019
        /// Efecto: elimina la jornada
        /// Requiere: jornada
        /// Modifica: base de datos eliminando la jornada
        /// Devuelve: -
        /// </summary>
        /// <param name="jornada"></param>
        public void eliminarJornada(Jornada jornada)
        {
            jornadaDatos.eliminarJornada(jornada);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 19/sep/2019
        /// Efecto: actualiza la jornada de forma logica
        /// Requiere: jornada
        /// Modifica: la jornada que se encuentra en la base de datos cambia el activo e ingresa una nueva jornada
        /// Devuelve: id de jornada nueva
        /// </summary>
        /// <param name="jornada"></param>
        public int actualizarJornadaLogica(Jornada jornada)
        {
            return jornadaDatos.actualizarJornadaLogica(jornada);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 19/sep/2019
        /// Efecto: elimina la jornada de forma logica
        /// Requiere: jornada
        /// Modifica: la jornada que se encuentra en la base de datos cambia el activo 
        /// Devuelve: -
        /// </summary>
        /// <param name="jornada"></param>
        public void eliminarJornadaLogica(Jornada jornada)
        {
            jornadaDatos.eliminarJornadaLogica(jornada);
        }
    }
}
