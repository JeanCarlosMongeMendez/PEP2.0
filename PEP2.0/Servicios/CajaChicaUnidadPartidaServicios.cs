using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class CajaChicaUnidadPartidaServicios
    {
        CajaChicaUnidadPartidaDatos cajaChicaUnidadPartidaDatos = new CajaChicaUnidadPartidaDatos();
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
            return cajaChicaUnidadPartidaDatos.getMontoDisponible(unidad, partida);
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
        public List<PartidaUnidad>getUnidadesPartidasMontoPorCajaChica(CajaChica cajaChica)
        {
            return cajaChicaUnidadPartidaDatos.getUnidadesPartidasMontoPorCajaChica(cajaChica);
        }

        public void insertarCajaChicaPartidaUnidad(int idCajaChica,int idunidad, int idpartida,Double monto)
        {
            cajaChicaUnidadPartidaDatos.insertarCajaChicaUnidadPartida(idCajaChica,idunidad,idpartida,monto) ;
        }
        public List<Unidad> getUnidadesPorCajaChica(CajaChica cajaChica)
        {
            return cajaChicaUnidadPartidaDatos.getUnidadesPorCajaChica(cajaChica);
        }
        public List<Partida> getPartidasPorCajaChica(CajaChica cajaChica)
        {

            return cajaChicaUnidadPartidaDatos.getPartidasPorCajaChica(cajaChica);
        }
        public void eliminarCajaChicaUnidadPartidaPorCajaChica(int idcajaChica)
        {
            cajaChicaUnidadPartidaDatos.eliminarCajaChicaUnidadPartidaPorEjecucion(idcajaChica);
        }
    }

}
