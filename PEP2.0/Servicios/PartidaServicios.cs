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
    }
}
