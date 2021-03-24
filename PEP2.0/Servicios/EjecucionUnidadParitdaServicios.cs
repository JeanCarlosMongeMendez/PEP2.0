using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using Entidades;

namespace Servicios
{
    public class EjecucionUnidadParitdaServicios
    {
        EjecucionUnidadParitdaDatos ejecucionUnidadParitdaDatos = new EjecucionUnidadParitdaDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 05/mar/2021
        /// Efecto: devuelve en monto disponible de una partida en especifico segun la unidad
        /// Requiere: unidad y partida
        /// Modifica: -
        /// Devuelve: monto disponible
        /// </summary>
        /// <param name="unidad"></param>
        /// <param name="partida"></param>
        /// <returns></returns>
        public Double getMontoDisponible(Unidad unidad, Partida partida)
        {
            return ejecucionUnidadParitdaDatos.getMontoDisponible(unidad, partida);
        }

        /// <summary>
        /// Leonarrdo Carrion
        /// 10/mar/2021
        /// Efecto: guarda en la base de datos en la tabla de Ejecucion_Unidad_Partida
        /// Requiere: datos a ingresar en la tabla
        /// Modifca: -
        /// Devuelve: -
        /// </summary>
        /// <param name="idEjecucion"></param>
        /// <param name="idUnidad"></param>
        /// <param name="idPartida"></param>
        /// <param name="monto"></param>
        public void insertarEjecucionUnidadPartida(int idEjecucion, int idUnidad, int idPartida, Double monto)
        {
            ejecucionUnidadParitdaDatos.insertarEjecucionUnidadPartida(idEjecucion, idUnidad, idPartida, monto);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/mar/2021
        /// Efecto: devuelve la lista de unidades segun la ejecucion ingresada
        /// Requiere: ejecucion
        /// Modifica: -
        /// Devuelve: lista de unidades
        /// </summary>
        /// <param name="ejecucion"></param>
        /// <returns></returns>
        public List<Unidad> getUnidadesPorEjecucion(Ejecucion ejecucion)
        {
            return ejecucionUnidadParitdaDatos.getUnidadesPorEjecucion(ejecucion);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 17/mar/2021
        /// Efecto: devuelve la lista de partidas segun la ejecucion ingresada
        /// Requiere: ejecucion
        /// Modifica: -
        /// Devuelve: lista de partidas
        /// </summary>
        /// <param name="ejecucion"></param>
        /// <returns></returns>
        public List<Partida> getPartidasPorEjecucion(Ejecucion ejecucion)
        {
            return ejecucionUnidadParitdaDatos.getPartidasPorEjecucion(ejecucion);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/mar/2021
        /// Efecto: devuelve la lista de unidades partidas y monto segun la ejecucion ingresada
        /// Requiere: ejecucion
        /// Modifica: -
        /// Devuelve: lista de unidades partidas
        /// </summary>
        /// <param name="ejecucion"></param>
        /// <returns></returns>
        public List<PartidaUnidad> getUnidadesPartidasMontoPorEjecucion(Ejecucion ejecucion)
        {
            return ejecucionUnidadParitdaDatos.getUnidadesPartidasMontoPorEjecucion(ejecucion);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 19/mar/2021
        /// Efecto: elimina ejecucionUnidadPartida
        /// Requiere: idEjecucion
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="idEjecucion"></param>
        public void eliminarEjecucionUnidadPartidaPorEjecucion(int idEjecucion)
        {
            ejecucionUnidadParitdaDatos.eliminarEjecucionUnidadPartidaPorEjecucion(idEjecucion);
        }

    }
}
