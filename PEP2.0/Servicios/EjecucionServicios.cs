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
        /// <param name="numeroReferencia">string</param>
        /// <param name="respuesta">string</param>
        public void InsertarEjecucionUnidad(Unidad unidad,string numeroReferencia, int respuesta)
        {
             ejecucionDatos.insertarEjecucionUnidad(unidad,numeroReferencia,respuesta); 
        }
        /// <summary>
        /// Inserta una Ejecucion_Partida
        /// </summary>
        /// <param name="partida">Partida</param>
        ///  <param name="numeroReferencia">string</param>
        ///  <param name="respuesta">string</param>
        public void InsertarEjecucionPartidas(Partida partida, string numeroReferencia,int respuesta)
        {
            ejecucionDatos.insertarEjecucionPartidas(partida, numeroReferencia,respuesta);
        }
        /// <summary>
        /// Inserta una EjecucionPartidaMontoElelegido
        /// </summary>
        /// <param name="partidaUnidad">PartidaUnidad</param>
        ///  <param name="numeroReferencia">String</param>
        /// <param name="respuesta">String</param>
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
        
        public void EditarEjecucion(Ejecucion ejecucion)
        {
           ejecucionDatos.actualizarEjecucion(ejecucion);
        }
        /// <summary>
        /// Consultar una Ejecucion
        /// </summary>
        /// <param name="Proyecto">Ejecucion</param>
        /// <param name="Periodo">Ejecucion</param>

        public List<Ejecucion> ConsultarEjecucion(string Periodo,string Proyecto)
        {
            return ejecucionDatos.consultaEjecucion(Periodo,Proyecto);
        }

        /// <summary>
        /// Eliminar una Ejecucion_Partida
        /// </summary>
        /// <param name="respuesta">string</param>
        public void EliminarEjecucionUnidad( int respuesta)
        {
            ejecucionDatos.EliminarEjecucionUnidad( respuesta);
        }

        /// <summary>
        /// 
        ///Eliminar una Ejecucion_Partida
        /// </summary>
        /// /// <param name="respuesta">string</param>
        public void EliminarEjecucionPartidas( int respuesta)
        {
            ejecucionDatos.eliminarEjecucionPartidas(respuesta);
        }

        /// <summary>
        /// Eliminar una EjecucionPartidaMontoElelegido
        /// </summary>
        /// /// <param name="respuesta">String</param>
        public void EliminarEjecucionPartidaMontoElelegido(int respuesta)
        {
            ejecucionDatos.eliminarEjecucionMontoPartidaElegida(respuesta);
        }
        /// <summary>
        /// CONSULTA Monto disponible de la partida
        /// </summary>
        /// <param name="idPartida">String</param>
        /// <param name="idPresupuestoEgreso">String</param>
        public double ConsultaMontoDisponiblePartida(String idPartida,String idPresupuestoEgreso)
        {
            return ejecucionDatos.consultarMontoDiponible(idPartida, idPresupuestoEgreso);
        }
        /// <summary>
        /// Consultar una UnidadEjecucion
        /// </summary>
        /// <param name="idEjecucion">Ejecucion</param>
       

        public List<Unidad> ConsultarUnidadEjecucion(int idEjecucion)
        {
            return ejecucionDatos.ConsultarEjecucionUnidad(idEjecucion);
        }


        /// <summary>
        /// Consultar una PartidaEjecucion
        /// </summary>
        /// <param name="idEjecucion">Ejecucion</param>

        public List<Partida> ConsultarPartidaEjecucion(int idEjecucion)
        {
            return ejecucionDatos.ConsultarEjecucionPartidas(idEjecucion);
        }

        /// <summary>
        /// Consultar una EjecucionMontoPartida
        /// </summary>
        /// <param name="idEjecucion">Ejecucion</param>

        public List<PartidaUnidad> ConsultarEjecucionMontoPartida(int idEjecucion)
        {
            return ejecucionDatos.ConsultarEjecucionMontoPartidaElegida(idEjecucion);
        }


        /// <summary>
        /// Eliminar una Ejecucion
        /// </summary>
        /// /// <param name="idEjecucion">String</param>
        public void EliminarEjecucion(int idEjecucion)
        {
            ejecucionDatos.eliminarEjecucion(idEjecucion);
        }

        /// <summary>
        /// Traer estado de la ejecucion
        /// </summary>
        /// /// <param name="idEjecucion">String</param>
        public int EstadoEjecucion(int idEjecucion)
        {
            return ejecucionDatos.ConsultarEjecucionEstado(idEjecucion);
        }
       
    }
}
