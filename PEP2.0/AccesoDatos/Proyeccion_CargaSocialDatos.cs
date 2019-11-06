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
    /// 04/nov/2019
    /// Clase para administrar las consultas sql de la entidad de Proyeccion_CargaSocial
    /// </summary>
    public class Proyeccion_CargaSocialDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: inserta en la base de datos la relacion de proyeccion y carga social
        /// Requiere: proyeccion, carga social y monto
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="proyeccion"></param>
        /// <returns></returns>
        public void insertarProyeccionCargaSocial(Proyeccion_CargaSocial proyeccion_CargaSocial)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"Insert Proyeccion_CargaSocial(id_proyeccion,id_carga_social,monto)
                                            values(@idProyeccion,@idCargaSocial,@monto)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idProyeccion", proyeccion_CargaSocial.proyeccion.idProyeccion);
            command.Parameters.AddWithValue("@idCargaSocial", proyeccion_CargaSocial.cargaSocial.idCargaSocial);
            command.Parameters.AddWithValue("@monto", proyeccion_CargaSocial.monto);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: elimina la asociacion de proyeccion y carga social
        /// Requiere: proyeccion
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="periodo"></param>
        public void eliminarProyeccionCargaSocialPorProyeccion(Proyeccion proyeccion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete from Proyeccion_CargaSocial where 
                                            id_proyeccion = @idProyeccion";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idProyeccion", proyeccion.idProyeccion);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

    }
}
