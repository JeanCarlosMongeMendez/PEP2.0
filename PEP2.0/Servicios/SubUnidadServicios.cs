using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class SubUnidadServicios
    {
        SubUnidadDatos subUnidadDatos = new SubUnidadDatos();

        /// <summary>
        ///  Leonardo Carrion
        ///  11/nov/2020
        ///  Efecto: obtiene todas las subunidades segun la unidad de la base de datos
        ///  Requiere: unidad
        ///  Modifica: -
        ///  Devuelve: lista de sub unidades
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public List<SubUnidad> getSubUnidadesPorUnidad(Unidad unidad)
        {
            return subUnidadDatos.getSubUnidadesPorUnidad(unidad);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12//nov/2020
        /// Efecto: inserta en la base de datos una sub unidad
        /// Requiere: sub unidad
        /// Modifica: -
        /// Devuelve: id de la sub unidad
        /// </summary>
        /// <param name="subUnidad"></param>
        /// <returns></returns>
        public int insertarSubUnidad(SubUnidad subUnidad)
        {
            return subUnidadDatos.insertarSubUnidad(subUnidad);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/nov/2020
        /// Efecto: actualiza la sub unidad
        /// Requiere: sub unidad
        /// Modifica: la sub unidad que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="subUnidad"></param>
        public void actualizarSubUnidad(SubUnidad subUnidad)
        {
            subUnidadDatos.actualizarSubUnidad(subUnidad);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/nov/2020
        /// Efecto: elimina la sub_unidad
        /// Requiere: sub unidad
        /// Modifica: base de datos eliminando la sub unidad
        /// Devuelve: -
        /// </summary>
        /// <param name="subUnidad"></param>
        public void eliminarSubUnidad(SubUnidad subUnidad)
        {
            subUnidadDatos.eliminarSubUnidad(subUnidad);
        }
    }
}
