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
        /// /// <param name="numeroReferencia">Unidad</param>
        public void InsertarEjecucionUnidad(Unidad unidad,string numeroReferencia)
        {
             ejecucionDatos.insertarEjecucionUnidad(unidad,numeroReferencia); 
        }
        /// <summary>
        /// Inserta una Ejecucion_Partida
        /// </summary>
        /// <param name="partida">Unidad</param>
        /// /// <param name="numeroReferencia">Unidad</param>
        public void InsertarEjecucionPartidas(Partida partida, string numeroReferencia)
        {
            ejecucionDatos.insertarEjecucionPartidas(partida, numeroReferencia);
        }
        /// <summary>
        /// Inserta una EjecucionPartidaMontoElelegido
        /// </summary>
        /// <param name="partidaUnidad">Unidad</param>
        /// /// <param name="numeroReferencia">Unidad</param>
        public void InsertarEjecucionPartidaMontoElelegido(PartidaUnidad partidaUnidad, string numeroReferencia)
        {
            ejecucionDatos.insertarEjecucionMontoPartidaElegida(partidaUnidad, numeroReferencia);
        }
    }
}
