using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class CajaChicaServicios
    {
       CajaChicaDatos cajaChicaDatos = new CajaChicaDatos();

        /// <summary>
        /// Editar una Ejecucion
        /// </summary>
        /// <param name="ejecucion">Ejecucion</param>

        public int InsertarCajaChica(CajaChica cajaChica)
        {
            return cajaChicaDatos.insertarCajaChica(cajaChica);
        }
        /// <summary>
        /// Editar una Ejecucion
        /// </summary>
        /// <param name="ejecucion">Ejecucion</param>

        public void EditarCajaChica(CajaChica cajaChica)
        {
            cajaChicaDatos.actualizarCajaChica(cajaChica);
        }
        /// <summary>
        ///Kevin Picado
        /// 23/04/2021
        /// Efecto: devuelve la lista de ejecuciones segun el periodo y proyecto seleccionado
        /// Requiere: periodo y proyecto
        /// Modifica: -
        /// Devuelve: lista de ejecuciones
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<CajaChica> getCajaChicaPorPeriodoYProyecto(Periodo periodo, Proyectos proyecto)
        {
            return cajaChicaDatos.getCajaChicaPorPeriodoYProyecto(periodo, proyecto);
        }

        public string getNumeroSolicitudCajaChica(int año)
        {
            return cajaChicaDatos.getNumeroSolicitudCajaChica(año);
        }
        public void eliminarCajaChica(int idCajaChica)
        {
            cajaChicaDatos.eliminarCajaChica(idCajaChica);
        }
        public void actualizarEnviadoCajaChica(int idCajaChica,Boolean Enviado)
        {
            cajaChicaDatos.actualizarEnviadoCajaChica(idCajaChica,Enviado);
        }
    }
}
