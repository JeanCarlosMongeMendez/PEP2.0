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
    /// 30/sep/2019
    /// Clase para administrar las consultas sql de la entidad de Estadp de presupuesto de ingreso
    /// </summary>
    public class EstadoPresupIngresoDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 30/sep/2019
        /// Efecto: obtiene el estado segun la palabra ingresada
        /// Requiere: String de desc estado
        /// Modifica: -
        /// Devuelve: estado presupuesto ingreso
        /// </summary>
        /// <returns></returns>
        public EstadoPresupIngreso getEstadoPresupIngresoPorNombre(String descEstado)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            EstadoPresupIngreso estadoPresupIngreso = new EstadoPresupIngreso();

            String consulta = @"select * from Estado_presup_ingreso where desc_estado = @descEstado;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@descEstado", descEstado);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                estadoPresupIngreso.idEstadoPresupIngreso = Convert.ToInt32(reader["id_estado_presup_ingreso"].ToString());
                estadoPresupIngreso.descEstado = reader["desc_estado"].ToString();
            }

            sqlConnection.Close();

            return estadoPresupIngreso;
        }
    }
}
