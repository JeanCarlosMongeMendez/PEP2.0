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

        public LinkedList<Partida> ObtenerPorPeriodo(int anoPeriodo)
        {
            return this.partidaDatos.ObtenerPorPeriodo(anoPeriodo);
        }

        public Partida ObtenerPorId(int idPartida)
        {
            return this.partidaDatos.ObtenerPorId(idPartida);
        }

        public bool Guardar(LinkedList<int> partidasId, int anoPeriodo)
        {
            return this.partidaDatos.Guardar(partidasId, anoPeriodo);
        }

        public void ActualizarPartida(Partida partida)
        {
            this.partidaDatos.ActualizarPartida(partida);
        }

        public void EliminarPartida(int idPartida)
        {
            this.partidaDatos.EliminarPartida(idPartida);
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
            return partidaDatos.getPartidaPorNumeroYPeriodo(partida,periodo);
        }

    }
}
