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
    /// Clase para administrar los servicios de proyeccion carga social
    /// </summary>
    public class Proyeccion_CargaSocialServicios
    {
        Proyeccion_CargaSocialDatos proyeccion_CargaSocialDatos = new Proyeccion_CargaSocialDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: inserta en la base de datos la relacion de proyeccion y carga social
        /// Requiere: proyeccion, carga social y monto
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="proyeccion"></param>
        /// <returns></returns>
        public void insertarProyeccionCargaSocial(Proyeccion_CargaSocial proyeccion_CargaSocial)
        {
            proyeccion_CargaSocialDatos.insertarProyeccionCargaSocial(proyeccion_CargaSocial);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: elimina la asociacion de proyeccion y carga social
        /// Requiere: proyeccion
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="periodo"></param>
        public void eliminarProyeccionCargaSocialPorProyeccion(Proyeccion proyeccion)
        {
            proyeccion_CargaSocialDatos.eliminarProyeccionCargaSocialPorProyeccion(proyeccion);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 07/nov/2019
        /// Efecto: devuelve lista de proyeccion_cargas sociales segun la proyeccion ingresada
        /// Requiere: proyeccion a consultar
        /// Modifica: -
        /// Devuelve: lista proyecion_cargasSociales
        /// </summary>
        /// <param name="proyeccionConsulta"></param>
        /// <returns></returns>
        public List<Proyeccion_CargaSocial> getProyeccionCargaSocialPorProyeccionPorProyeccion(Proyeccion proyeccionConsulta)
        {
            return proyeccion_CargaSocialDatos.getProyeccionCargaSocialPorProyeccionPorProyeccion(proyeccionConsulta);
        }
    }
}
