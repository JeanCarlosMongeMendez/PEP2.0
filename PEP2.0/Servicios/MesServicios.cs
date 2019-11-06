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
    /// 30/oct/2019
    /// Clase para administrar los servicios de mes
    /// </summary>
    public class MesServicios
    {
        MesDatos mesDatos = new MesDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 30/oct/2019
        /// Efecto: devuelve lista de meses que se encuentran en la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de meses
        /// </summary>
        /// <returns></returns>
        public List<Mes> getMeses()
        {
            return mesDatos.getMeses();
        }
    }
}
