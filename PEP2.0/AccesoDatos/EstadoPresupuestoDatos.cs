using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    /// <summary>
    /// Leonardo Carrion
    /// 04/oct/2019
    /// Clase para administrar 
    /// </summary>
    public class EstadoPresupuestoDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: obtiene el estado segun la palabra ingresada
        /// Requiere: String de desc estado
        /// Modifica: -
        /// Devuelve: estado presupuesto
        /// </summary>
        /// <returns></returns>
        public EstadoPresupuesto getEstadoPresupuestoPorNombre(String descEstado)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            EstadoPresupuesto estadoPresupuesto = new EstadoPresupuesto();

            String consulta = @"select * from Estado_presupuestos where descripcion_estado_presupuesto = @descEstado;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@descEstado", descEstado);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                estadoPresupuesto.idEstadoPresupuesto = Convert.ToInt32(reader["id_estado_presupuesto"].ToString());
                estadoPresupuesto.descripcionEstado = reader["descripcion_estado_presupuesto"].ToString();
            }

            sqlConnection.Close();

            return estadoPresupuesto;
        }
    }
}
