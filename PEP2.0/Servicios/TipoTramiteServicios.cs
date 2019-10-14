using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class TipoTramiteServicios
    {

        TipoTramitesDatos tipoTramiteDatos = new TipoTramitesDatos();

        /// <summary>
        /// Josseline M
        /// Este método se encarga de obtener el tipo de tramite existente para el tipo de proyecto UCR o Fundevi
        /// </summary>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<TipoTramite> obtenerTipoTramitesPorProyecto(Proyectos proyecto)
        {
            return this.tipoTramiteDatos.obtenerTipoTramitesPorProyecto(proyecto);
        }
     }
}
