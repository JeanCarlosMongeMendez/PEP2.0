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
    /// kevin picado
    /// 07/febrero/2020
    /// Clase para administrar los servicios de Ejecucion
    /// </summary>
    public class EjecucionServicios
    {
        EjecucionDatos ejecucionDatos = new EjecucionDatos();

        /// <summary>
        /// Inserta una Ejecucion_Partida
        /// </summary>
        /// <param name="unidad">Unidad</param>
        /// /// <param name="numeroReferencia">string</param>
        public void InsertarEjecucionUnidad(Unidad unidad,string numeroReferencia, int respuesta)
        {
             ejecucionDatos.insertarEjecucionUnidad(unidad,numeroReferencia,respuesta); 
        }
        /// <summary>
        /// Inserta una Ejecucion_Partida
        /// </summary>
        /// <param name="partida">Partida</param>
        /// /// <param name="numeroReferencia">string</param>
        public void InsertarEjecucionPartidas(Partida partida, string numeroReferencia,int respuesta)
        {
            ejecucionDatos.insertarEjecucionPartidas(partida, numeroReferencia,respuesta);
        }
        /// <summary>
        /// Inserta una EjecucionPartidaMontoElelegido
        /// </summary>
        /// <param name="partidaUnidad">PartidaUnidad</param>
        /// /// <param name="numeroReferencia">String</param>
        public void InsertarEjecucionPartidaMontoElelegido(PartidaUnidad partidaUnidad, string numeroReferencia,int respuesta)
        {
            ejecucionDatos.insertarEjecucionMontoPartidaElegida(partidaUnidad, numeroReferencia,respuesta);
        }
        /// <summary>
        /// Inserta una Ejecucion
        /// </summary>
        /// <param name="ejecucion">Ejecucion</param>
        ///
        public int InsertarEjecucion(Ejecucion ejecucion)
        {
           return ejecucionDatos.insertarEjecucion(ejecucion);
        }
        /// <summary>
        /// Editar una Ejecucion
        /// </summary>
        /// <param name="ejecucion">Ejecucion</param>
        ///
        public void EditarEjecucion(Ejecucion ejecucion)
        {
           ejecucionDatos.actualizarEjecucion(ejecucion);
        }

        /// <summary>
        /// Eliminar una Ejecucion_Partida
        /// </summary>
        /// <param name="unidad">Unidad</param>
        /// /// <param name="respuesta">string</param>
        public void EliminarEjecucionUnidad( int respuesta)
        {
            ejecucionDatos.EliminarEjecucionUnidad( respuesta);
        }

        /// <summary>
        /// 
        ///Eliminar una Ejecucion_Partida
        /// </summary>
        /// <param name="partida">Partida</param>
        /// /// <param name="respuesta">string</param>
        public void EliminarEjecucionPartidas( int respuesta)
        {
            ejecucionDatos.eliminarEjecucionPartidas(respuesta);
        }

        /// <summary>
        /// Eliminar una EjecucionPartidaMontoElelegido
        /// </summary>
        /// <param name="partidaUnidad">PartidaUnidad</param>
        /// /// <param name="respuesta">String</param>
        public void EliminarEjecucionPartidaMontoElelegido(int respuesta)
        {
            ejecucionDatos.eliminarEjecucionMontoPartidaElegida(respuesta);
        }
    }
}
