using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace AccesoDatos
{
    public class EstadoEjecucionDatos
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
        public EstadoEjecucion getEstadoEjecucionSegunNombre(String estado)
        {
            EstadoEjecucion estadoEjecucion = new EstadoEjecucion();

            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"SELECT * from EstadoEjecucion where descripcion_estado = @descripcion";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@descripcion", estado);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                estadoEjecucion.idEstado = Convert.ToInt32(reader["id_estado"].ToString());
                estadoEjecucion.descripcion = reader["descripcion_estado"].ToString();
            }

            sqlConnection.Close();

            return estadoEjecucion;
        }
    }
}
