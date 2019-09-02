using AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    /*
     * carrion lanamme
     * 26/10/2015
     * Clase para manejar los servicios de conexion
     */
    public class ConexionServicios
    {
        #region variables globales
        ConexionDatos conexionDatos = new ConexionDatos();
        #endregion

        #region metodos
        /*
         * carrion lanamme 
         * 23/10/2015
         * metodo que se utiliza para loguearse a la aplicacion 
         * recibe el nombre de usuario que se loguea
         * devuelve el id del rol del usuario y el nombre completo del usuario en un vector
         */
        public object[] loguearse(String usuario)
        {
            return conexionDatos.loguearse(usuario);
        }
        #endregion
    }
}