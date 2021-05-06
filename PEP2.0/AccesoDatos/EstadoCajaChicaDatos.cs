using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
   public class EstadoCajaChicaDatos
    {


        private ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 10/mar/2021
        /// Efecto: devuelve el estado segun la descripcion ingresada
        /// Requiere: nombre estado
        /// Modifica: -
        /// Devuelve: estadoEjecucion
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        public EstadoCajaChica getEstadoCajaChicaSegunNombre(String estado)
        {
            EstadoCajaChica estadoCajaChica = new EstadoCajaChica();

            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"SELECT * from Estado_Caja_Chica where descripcion = @descripcion";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@descripcion", estado);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                estadoCajaChica.idEstadoCajaChica = Convert.ToInt32(reader["id_estado_caja_chica"].ToString());
                estadoCajaChica.descripcion = reader["descripcion"].ToString();
            }

            sqlConnection.Close();

            return estadoCajaChica;
        }
    }
}

