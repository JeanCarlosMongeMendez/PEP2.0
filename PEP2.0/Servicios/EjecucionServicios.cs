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
        /// Eliminar una Ejecucion
        /// </summary>
        /// /// <param name="idEjecucion">String</param>
        public void EliminarEjecucion(int idEjecucion)
        {
            ejecucionDatos.eliminarEjecucion(idEjecucion);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 23/feb/2021
        /// Efecto: devuelve la lista de ejecuciones segun el periodo y proyecto seleccionado
        /// Requiere: periodo y proyecto
        /// Modifica: -
        /// Devuelve: lista de ejecuciones
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<Ejecucion> getEjecucionesPorPeriodoYProyecto(Periodo periodo, Proyectos proyecto)
        {
            return ejecucionDatos.getEjecucionesPorPeriodoYProyecto(periodo, proyecto);
        }
    }
}
