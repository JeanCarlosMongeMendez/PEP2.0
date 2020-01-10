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
    /// Adrián Serrano
    /// 27/may/2019
    /// Clase para administrar los servicios de Partida
    /// </summary>
    public class PartidaServicios
    {
        PartidaDatos partidaDatos = new PartidaDatos();

        public int Insertar(Partida partida)
        {
            return this.partidaDatos.Insertar(partida);
        }

        public List<Partida> ObtenerPorPeriodo(int anoPeriodo)
        {
            return this.partidaDatos.ObtenerPorPeriodo(anoPeriodo);
        }

        public Partida ObtenerPorId(int idPartida)
        {
            return this.partidaDatos.ObtenerPorId(idPartida);
        }

        //public bool Guardar(LinkedList<int> partidasId, int anoPeriodo)
        //{
        //    return this.partidaDatos.Guardar(partidasId, anoPeriodo);
        //}

        public void ActualizarPartida(Partida partida)
        {
            this.partidaDatos.ActualizarPartida(partida);
        }

        public void EliminarPartida(int idPartida, int periodo)
        {
            this.partidaDatos.EliminarPartida(idPartida, periodo);
        }

        public List<Partida> ObtienePartidaPorPeriodoUnidadProyecto(int proyecto, LinkedList<int> unidad, int periodo)
        {
            return this.partidaDatos.ObtienePartidaPorPeriodoUnidadProyecto(proyecto, unidad, periodo);
        }
        /// <summary>
        /// Josseline M
        /// Obtiene una partida a partir en su numeroPartida
        /// </summary>
        /// <param name="idPartida">Valor de tipo <code>int</code> que corresponde a la partida a buscar</param>
        /// <returns>Retorna el elemento de tipo <code>Partida</code> que coincida con el identificador dado</returns>
        public Partida ObtenerPorNumeroPartida(string numeroPartida)
        {
            return this.partidaDatos.ObtenerPorNumeroPartida(numeroPartida);
        }

        // <summary>
        /// Leonardo Carrion
        /// 25/sep/2019
        /// Efecto: devuelve la partida que cumple con los datos ingresados de numero de partida y periodo
        /// Requiere: partida y periodo
        /// Modifica: -
        /// Devuelve: partida
        /// </summary>
        /// <param name="partida"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public Partida getPartidaPorNumeroYPeriodo(Partida partida, Periodo periodo)
        {
            return partidaDatos.getPartidaPorNumeroYPeriodo(partida, periodo);
        }


        // <summary>
        /// Jesus Torres
        /// 10/oct/2019
        /// Efecto: devuelve la lista de partidas con mismo padre
        /// Requiere: partida y periodo
        /// Modifica: -
        /// Devuelve: lista de partidas
        /// </summary>
        /// <param name="partida"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<Partida> obtenerPorIdPartidaPadre(int partida, int periodo)
        {
            return partidaDatos.obtenerPorIdPartidaPadre(partida, periodo);
        }


        // <summary>
        /// Mariela Calvo
        /// 18/oct/2019
        /// Efecto: devuelve la lista de partidas con mismo padre
        /// Requiere: partida y periodo
        /// Modifica: -
        /// Devuelve: lista de partidas
        /// </summary>
        /// <param name="partida"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<Partida> obtenerPorTipoPartidaYPeriodo(Boolean tipoPartida, int anioPeriodo)
        {
            return partidaDatos.obtenerPorTipoPartidaYPeriodo(tipoPartida, anioPeriodo);
        }
        public Partida obtenerPorIdPartidaYNumeroPartida(int partida, string numeroPartida)
        {
            return partidaDatos.ObtenerPorIdYNumeroPartida(partida, numeroPartida);
        }
        public Partida ObtienePartidaPorPeriodoUnidadProyectoYNumeroUnidad(int proyecto, int idunidad, int periodo, string numeroPartida)
        {
            return this.partidaDatos.ObtienePartidaPorPeriodoUnidadProyectoYNumeroUnidad(proyecto, idunidad, periodo,numeroPartida);
        }

       

    }
}
