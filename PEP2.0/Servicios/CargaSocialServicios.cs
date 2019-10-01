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
    /// Leonardo Carrion
    /// 20/sep/2019
    /// Clase para administrar los servicios de Carga Social
    /// </summary>
    public class CargaSocialServicios
    {
        CargaSocialDatos cargaSocialDatos = new CargaSocialDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: obtiene todas las escalas sociales de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de cargas sociales
        /// </summary>
        /// <returns></returns>
        public List<CargaSocial> getCargasSocialesActivas()
        {
            return cargaSocialDatos.getCargasSocialesActivas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 20//sep/2019
        /// Efecto: inserta en la base de datos una carga social
        /// Requiere: carga social
        /// Modifica: -
        /// Devuelve: id de la carga social
        /// </summary>
        /// <param name="cargaSocial"></param>
        /// <returns></returns>
        public int insertarCargaSocial(CargaSocial cargaSocial)
        {
            return cargaSocialDatos.insertarCargaSocial(cargaSocial);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: actualiza la carga social
        /// Requiere: carga social
        /// Modifica: la carga social que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="cargaSocial"></param>
        public void actualizarCargaSocial(CargaSocial cargaSocial)
        {
            cargaSocialDatos.actualizarCargaSocial(cargaSocial);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: elimina la carga social
        /// Requiere: carga social
        /// Modifica: base de datos eliminando la carga social
        /// Devuelve: -
        /// </summary>
        /// <param name="cargaSocial"></param>
        public void eliminarCargaSocial(CargaSocial cargaSocial)
        {
            cargaSocialDatos.eliminarCargaSocial(cargaSocial);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: actualiza la carga social de forma logica
        /// Requiere: carga social
        /// Modifica: la carga social que se encuentra en la base de datos cambia el activo e ingresa una nueva carga social
        /// Devuelve: id de craga social nueva
        /// </summary>
        /// <param name="cargaSocial"></param>
        public int actualizarCargaSocialLogica(CargaSocial cargaSocial)
        {
            return cargaSocialDatos.actualizarCargaSocialLogica(cargaSocial);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: elimina la carga social de forma logica
        /// Requiere: carga social
        /// Modifica: la carga social que se encuentra en la base de datos cambia el activo 
        /// Devuelve: -
        /// </summary>
        /// <param name="cargaSocial"></param>
        public void eliminarCargaSocialLogica(CargaSocial cargaSocial)
        {
            cargaSocialDatos.eliminarCargaSocialLogica(cargaSocial);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 23/sep/2019
        /// Efecto: devuelve una lista de cargas sociales dependiendo del periodo consultado
        /// Requiere: periodo
        /// Modifica: -
        /// Devuelve: lista de cargas sociales
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<CargaSocial> getCargasSocialesActivasPorPeriodo(Periodo periodo)
        {
            return cargaSocialDatos.getCargasSocialesActivasPorPeriodo(periodo);
        }
    }
}
