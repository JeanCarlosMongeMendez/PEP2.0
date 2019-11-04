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
    /// 01/nov/2019
    /// Clase para administrar los servicios de la entidad de Anualidad
    /// </summary>
    public class AnualidadServicios
    {
        AnualidadDatos anualidadDatos = new AnualidadDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: obtiene todas las anualidades de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de anualidades
        /// </summary>
        /// <returns></returns>
        public List<Anualidad> getAnualidades()
        {
            return anualidadDatos.getAnualidades();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: inserta en la base de datos una anualidad
        /// Requiere: anualidad
        /// Modifica: -
        /// Devuelve: id de la anualidad
        /// </summary>
        /// <param name="anualidad"></param>
        /// <returns></returns>
        public int insertarAnualidad(Anualidad anualidad)
        {
            return anualidadDatos.insertarAnualidad(anualidad); ;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: actualiza la anualidad
        /// Requiere: anualidad
        /// Modifica: la anualidad que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="anualidad"></param>
        public void actualizarAnualidad(Anualidad anualidad)
        {
            anualidadDatos.actualizarAnualidad(anualidad);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: elimina la anualidad
        /// Requiere: anualidad
        /// Modifica: base de datos eliminando la anualidad
        /// Devuelve: -
        /// </summary>
        /// <param name="anualidad"></param>
        public void eliminarAnualidad(Anualidad anualidad)
        {
            anualidadDatos.eliminarAnualidad(anualidad);
        }
    }
}
