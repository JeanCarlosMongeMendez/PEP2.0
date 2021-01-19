using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class SubUnidadDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        ///  Leonardo Carrion
        ///  11/nov/2020
        ///  Efecto: obtiene todas las subunidades segun la unidad de la base de datos
        ///  Requiere: unidad
        ///  Modifica: -
        ///  Devuelve: lista de sub unidades
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public List<SubUnidad> getSubUnidadesPorUnidad(Unidad unidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<SubUnidad> listaSUbUnidades = new List<SubUnidad>();

            String consulta = @"select SU.* from Sub_Unidad SU where SU.id_unidad = @idUnidad order by SU.nombre";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idUnidad", unidad.idUnidad);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                SubUnidad subUnidad = new SubUnidad();

                subUnidad.activo = Convert.ToBoolean(reader["activo"].ToString());
                subUnidad.idSubUnidad = Convert.ToInt32(reader["id_sub_unidad"].ToString());
                subUnidad.nombre = reader["nombre"].ToString();
                subUnidad.unidad = unidad;

                listaSUbUnidades.Add(subUnidad);
            }

            sqlConnection.Close();

            return listaSUbUnidades;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12//nov/2020
        /// Efecto: inserta en la base de datos una sub unidad
        /// Requiere: sub unidad
        /// Modifica: -
        /// Devuelve: id de la sub unidad
        /// </summary>
        /// <param name="subUnidad"></param>
        /// <returns></returns>
        public int insertarSubUnidad(SubUnidad subUnidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"Insert Sub_Unidad(id_unidad,nombre,activo)
                                            values(@idUnidad,@nombre,@activo);SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idUnidad", subUnidad.unidad.idUnidad);
            command.Parameters.AddWithValue("@nombre", subUnidad.nombre);
            command.Parameters.AddWithValue("@activo", subUnidad.activo);

            sqlConnection.Open();
            int idSubUnidad = Convert.ToInt32(command.ExecuteScalar());
            sqlConnection.Close();

            return idSubUnidad;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/nov/2020
        /// Efecto: actualiza la sub unidad
        /// Requiere: sub unidad
        /// Modifica: la sub unidad que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="subUnidad"></param>
        public void actualizarSubUnidad(SubUnidad subUnidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update Sub_Unidad set id_unidad = @idUnidad, activo = @activo, nombre = @nombre
                                            where id_sub_unidad = @idSubUnidad";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idSubUnidad", subUnidad.idSubUnidad);
            command.Parameters.AddWithValue("@idUnidad", subUnidad.unidad.idUnidad);
            command.Parameters.AddWithValue("@activo", subUnidad.activo);
            command.Parameters.AddWithValue("@nombre", subUnidad.nombre);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/nov/2020
        /// Efecto: elimina la sub_unidad
        /// Requiere: sub unidad
        /// Modifica: base de datos eliminando la sub unidad
        /// Devuelve: -
        /// </summary>
        /// <param name="subUnidad"></param>
        public void eliminarSubUnidad(SubUnidad subUnidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete from Sub_unidad where 
                                            id_sub_unidad = @idSubUnidad";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idSubUnidad", subUnidad.idSubUnidad);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
    }
}
