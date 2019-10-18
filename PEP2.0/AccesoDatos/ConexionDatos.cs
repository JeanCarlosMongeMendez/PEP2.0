using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class ConexionDatos
    {
        
        /*
         * Jonathan Fonseca Vallejos
         * 13/ene/2017
         * Método que lee el del archivo los datos del servidor y establece la conexión con el servidor Login.
         */
        public SqlConnection conexionLogin()
        {
            return new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["LOGINConnectionString"].ConnectionString);
        }

        /*
         * Jonathan Fonseca Vallejos
         * 13/ene/2017
         * Método que lee el del archivo los datos del servidor y establece la conexión con el servidor de Control de Trabajos de Laboratorio.
         */
        public SqlConnection conexionPEP()
        {
            return new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["PEPconnectionString"].ConnectionString);
        }
        
        /*
         * carrion lanamme 
         * 23/10/2015
         * metodo que se utiliza para loguearse a la aplicacion 
         * recibe el nombre de usuario que se loguea
         * devuelve el id del rol del usuario y el nombre completo del usuario en un vector
         */
        public object[] loguearse(String usuario)
        {
            object[] rolNombreCompleto = new object[2];
            SqlConnection sqlConnection = conexionLogin();
            SqlCommand sqlCommand = new SqlCommand("select R.id_rol, U.nombre_completo from " +
                "Rol R, Usuario U, Aplicacion A, Usuario_Rol_Aplicacion URA " +
                "where A.nombre_aplicacion='PEP' and U.usuario=@usuario and URA.id_aplicacion=A.id_aplicacion and " +
                "URA.id_usuario = u.id_usuario and R.id_rol = URA.id_rol ;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@usuario", usuario.ToLower());
            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {

                int rol = Int32.Parse(reader.GetValue(0).ToString());
                String nombreCompleto = reader.GetValue(1).ToString();

                rolNombreCompleto[0] = rol;
                rolNombreCompleto[1] = nombreCompleto;
            }

            sqlConnection.Close();

            return rolNombreCompleto;
        }
    }
}
