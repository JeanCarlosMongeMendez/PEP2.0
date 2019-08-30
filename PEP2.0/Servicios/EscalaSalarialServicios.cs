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
    /// 10/jun/2019
    /// Clase para administrar los servicios de Escala salarial
    /// </summary>
    public class EscalaSalarialServicios
    {
        EscalaSalarialDatos escalaSalarialDatos = new EscalaSalarialDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 10/jun/2019
        /// Efecto: obtiene todas las escalas salariales de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de escalas salariales
        /// </summary>
        /// <returns></returns>
        public List<EscalaSalarial> getEscalasSalariales()
        {
            return escalaSalarialDatos.getEscalasSalariales();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/jun/2019
        /// Efecto: obtiene todas las escalas salariales de la base de datos segun un periodo especifico
        /// Requiere: periodo
        /// Modifica: -
        /// Devuelve: lista de escalas salariales
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<EscalaSalarial> getEscalasSalarialesPorPeriodo(Periodo periodo)
        {
            return escalaSalarialDatos.getEscalasSalarialesPorPeriodo(periodo);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 13/jun/2019
        /// Efecto: inserta en la base de datos una escala salarial
        /// Requiere: escala salarial
        /// Modifica: -
        /// Devuelve: id de la escala insertada
        /// </summary>
        /// <param name="escalaSalarial"></param>
        /// <returns></returns>
        public int insertarEscalaSalarial(EscalaSalarial escalaSalarial)
        {
            return escalaSalarialDatos.insertarEscalaSalarial(escalaSalarial);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 25/jun/2019
        /// Efecto: actualiza la escala salarial
        /// Requiere: escala salarial 
        /// Modifica: la escala salarial que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="escalaSalarial"></param>
        public void actualizarEscalaSalarial(EscalaSalarial escalaSalarial)
        {
            escalaSalarialDatos.actualizarEscalaSalarial(escalaSalarial);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/jul/2019
        /// Efecto: elimina la escala salarial
        /// Requiere: escala salarial 
        /// Modifica: base de datos eliminando la escala salarial
        /// Devuelve: -
        /// </summary>
        /// <param name="escalaSalarial"></param>
        public void eliminarEscalaSalarial(EscalaSalarial escalaSalarial)
        {
            escalaSalarialDatos.eliminarEscalaSalarial(escalaSalarial);
        }

    }
}
