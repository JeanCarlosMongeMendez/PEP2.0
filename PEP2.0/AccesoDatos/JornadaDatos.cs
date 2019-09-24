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
    /// 18/sep/2019
    /// Clase para administrar las consultas sql de la entidad de Jornada
    /// </summary>
    public class JornadaDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 18/sep/2019
        /// Efecto: obtiene todas las jornadas de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de jornadas
        /// </summary>
        /// <returns></returns>
        public List<Jornada> getJornadasActivas()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Jornada> listaJornadas = new List<Jornada>();

            String consulta = @"select * from Jornada where activo = 'True' order by desc_jornada;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Jornada jornada = new Jornada();

                jornada.idJornada = Convert.ToInt32(reader["id_jornada"].ToString());
                jornada.descJornada = reader["desc_jornada"].ToString();
                jornada.activo = Convert.ToBoolean(reader["activo"].ToString());
                jornada.porcentajeJornada = Convert.ToDouble(reader["porcentaje_jornada"].ToString());

                listaJornadas.Add(jornada);
            }

            sqlConnection.Close();

            return listaJornadas;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 18//sep/2019
        /// Efecto: inserta en la base de datos una jornada
        /// Requiere: jornada
        /// Modifica: -
        /// Devuelve: id de la jornada insertada
        /// </summary>
        /// <param name="jornada"></param>
        /// <returns></returns>
        public int insertarJornada(Jornada jornada)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"Insert Jornada(desc_jornada,activo,porcentaje_jornada)
                                            values(@descJornada,@activo,@porcentajeJornada)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@descJornada", jornada.descJornada);
            command.Parameters.AddWithValue("@activo", jornada.activo);
            command.Parameters.AddWithValue("@porcentajeJornada", jornada.porcentajeJornada);

            sqlConnection.Open();
            int idJornada = Convert.ToInt32(command.ExecuteScalar());
            sqlConnection.Close();

            return idJornada;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 18/sep/2019
        /// Efecto: actualiza la jornada
        /// Requiere: jornada
        /// Modifica: la jornada que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="jornada"></param>
        public void actualizarJornada(Jornada jornada)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update Jornada set desc_jornada = @descJornada, activo = @activo, porcentaje_jornada = @porcentajeJornada
                                            where id_jornada = @idJornada";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idJornada", jornada.idJornada);
            command.Parameters.AddWithValue("@descJornada", jornada.descJornada);
            command.Parameters.AddWithValue("@activo", jornada.activo);
            command.Parameters.AddWithValue("@porcentajeJornada", jornada.porcentajeJornada);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 18/sep/2019
        /// Efecto: elimina la jornada
        /// Requiere: jornada
        /// Modifica: base de datos eliminando la jornada
        /// Devuelve: -
        /// </summary>
        /// <param name="jornada"></param>
        public void eliminarJornada(Jornada jornada)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete from Jornada where 
                                            id_jornada = @idJornada";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idJornada", jornada.idJornada);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 19/sep/2019
        /// Efecto: actualiza la jornada de forma logica
        /// Requiere: jornada
        /// Modifica: la jornada que se encuentra en la base de datos cambia el activo e ingresa una nueva jornada
        /// Devuelve: id de jornada nueva
        /// </summary>
        /// <param name="jornada"></param>
        public int actualizarJornadaLogica(Jornada jornada)
        {
            Jornada jornadaNueva = new Jornada();
            jornadaNueva.descJornada = jornada.descJornada;
            jornadaNueva.porcentajeJornada = jornada.porcentajeJornada;
            jornadaNueva.activo = true;

            jornada.activo = false;
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update Jornada set activo = @activo
                                            where id_jornada = @idJornada";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idJornada", jornada.idJornada);
            command.Parameters.AddWithValue("@activo", jornada.activo);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();

            return insertarJornada(jornadaNueva);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 19/sep/2019
        /// Efecto: elimina la jornada de forma logica
        /// Requiere: jornada
        /// Modifica: la jornada que se encuentra en la base de datos cambia el activo 
        /// Devuelve: -
        /// </summary>
        /// <param name="jornada"></param>
        public void eliminarJornadaLogica(Jornada jornada)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update Jornada set activo = @activo
                                            where id_jornada = @idJornada";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idJornada", jornada.idJornada);
            command.Parameters.AddWithValue("@activo", false);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

    }
}
