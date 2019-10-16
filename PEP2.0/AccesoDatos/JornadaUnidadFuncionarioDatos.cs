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
    /// Jean Carlos Monge Mendez
    /// 16/10/2019
    /// Clase para administrar las consultas sql de la entidad de JornadaUnidadFuncionario
    /// </summary>
    public class JornadaUnidadFuncionarioDatos
    {
        private ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 16/10/2019
        /// Efecto: obtiene todas las jornadaUnidadFuncionarioUnidadFuncionario de un funcionario de la base de datos
        /// Requiere: idFuncionario
        /// Modifica: -
        /// Devuelve: lista de jornadaUnidadFuncionario
        /// </summary>
        /// <param name="idFuncionario"></param>
        /// <param name="idUnidad"></param>
        /// <returns></returns>
        public List<JornadaUnidadFuncionario> getJornadaUnidadFuncionario(int idFuncionario, int idProyecto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<JornadaUnidadFuncionario> listaJornadaUnidadFuncionarios = new List<JornadaUnidadFuncionario>();
            String consulta = @"select * from JornadaUnidadFuncionario ju JOIN Jornada j ON ju.id_jornada = j.id_jornada 
            JOIN Unidad u ON ju.id_unidad = u.id_unidad 
            WHERE id_funcionario = @idFuncionario AND u.id_proyecto = @idProyecto";
            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idFuncionario", idFuncionario);
            sqlCommand.Parameters.AddWithValue("@idProyecto", idProyecto);
            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                JornadaUnidadFuncionario jornadaUnidadFuncionario = new JornadaUnidadFuncionario();
                jornadaUnidadFuncionario.idFuncionario = Convert.ToInt32(reader["id_Funcionario"].ToString());
                jornadaUnidadFuncionario.descUnidad = reader["nombre_unidad"].ToString();
                jornadaUnidadFuncionario.idUnidad = Convert.ToInt32(reader["id_unidad"].ToString());
                jornadaUnidadFuncionario.idJornada = Convert.ToInt32(reader["id_jornada"].ToString());
                jornadaUnidadFuncionario.jornadaAsignada = Convert.ToDouble(reader["porcentaje_jornada"].ToString());
                listaJornadaUnidadFuncionarios.Add(jornadaUnidadFuncionario);
            }
            sqlConnection.Close();
            return listaJornadaUnidadFuncionarios;
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 16//10/2019
        /// Efecto: inserta en la base de datos una jornadaUnidadFuncionario
        /// Requiere: jornadaUnidadFuncionario
        /// Modifica: -
        /// Devuelve: id de la jornadaUnidadFuncionario insertada
        /// </summary>
        /// <param name="jornadaUnidadFuncionario"></param>
        /// <returns></returns>
        public int insertarJornadaUnidadFuncionario(JornadaUnidadFuncionario jornadaUnidadFuncionario)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            String consulta = @"Insert JornadaUnidadFuncionario(id_unidad, id_funcionario, id_jornada)
                                            values(@unidad ,@funcionario ,@jornada)";
            SqlCommand command = new SqlCommand(consulta, sqlConnection);
            command.Parameters.AddWithValue("@unidad", jornadaUnidadFuncionario.idUnidad);
            command.Parameters.AddWithValue("@funcionario", jornadaUnidadFuncionario.idFuncionario);
            command.Parameters.AddWithValue("@jornada", jornadaUnidadFuncionario.idJornada);
            sqlConnection.Open();
            int idjornadaUnidadFuncionario = Convert.ToInt32(command.ExecuteScalar());
            sqlConnection.Close();
            return idjornadaUnidadFuncionario;
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 16/10/2019
        /// Efecto: actualiza la jornadaUnidadFuncionario
        /// Requiere: jornadaUnidadFuncionario
        /// Modifica: la jornadaUnidadFuncionario que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="jornadaUnidadFuncionario"></param>
        public void actualizarJornadaUnidadFuncionario(JornadaUnidadFuncionario jornadaUnidadFuncionario)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            String consulta = @"update JornadaUnidadFuncionario 
            SET id_jornada = @jornada
            WHERE id_funcionario = @idFuncionario 
            AND id_unidad = @idUnidad";
            SqlCommand command = new SqlCommand(consulta, sqlConnection);
            command.Parameters.AddWithValue("@jornada", jornadaUnidadFuncionario.idJornada);
            command.Parameters.AddWithValue("@idFuncionario", jornadaUnidadFuncionario.idJornada);
            command.Parameters.AddWithValue("@idUnidad", jornadaUnidadFuncionario.idJornada);
            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 16/10/2019
        /// Efecto: elimina la jornadaUnidadFuncionario
        /// Requiere: jornadaUnidadFuncionario
        /// Modifica: base de datos eliminando la jornadaUnidadFuncionario
        /// Devuelve: -
        /// </summary>
        /// <param name="jornadaUnidadFuncionario"></param>
        public void eliminarjornadaUnidadFuncionario(JornadaUnidadFuncionario jornadaUnidadFuncionario)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            String consulta = @"DELETE FROM JornadaUnidadFuncionario 
            WHERE id_funcionario = @idFuncionario AND id_unidad = @idUnidad";
            SqlCommand command = new SqlCommand(consulta, sqlConnection);
            command.Parameters.AddWithValue("@idFuncionario", jornadaUnidadFuncionario.idFuncionario);
            command.Parameters.AddWithValue("@idUnidad", jornadaUnidadFuncionario.idUnidad);
            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
    }
}
