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
    /// 31/oct/2019
    /// Clase para administrar las consultas sql de la entidad de Anualidad
    /// </summary>
    public class AnualidadDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: obtiene todas las anualidades de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de anualidades
        /// </summary>
        /// <returns></returns>
        public List<Anualidad> getAnualidades()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Anualidad> listaAnualidades = new List<Anualidad>();

            String consulta = @"select A.*
                                            from Anualidad A
                                            order by A.ano_periodo;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Anualidad anualidad = new Anualidad();
                Periodo periodo = new Periodo();

                periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());
                anualidad.periodo = periodo;
                anualidad.idAnualidad = Convert.ToInt32(reader["id_anualidad"].ToString());
                anualidad.porcentaje = Convert.ToDouble(reader["porcentaje"].ToString());

                listaAnualidades.Add(anualidad);
            }

            sqlConnection.Close();

            return listaAnualidades;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: inserta en la base de datos una anualidad
        /// Requiere: anualidad
        /// Modifica: -
        /// Devuelve: id de la anualidad
        /// </summary>
        /// <param name="anualidad"></param>
        /// <returns></returns>
        public int insertarAnualidad(Anualidad anualidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"Insert Anualidad(porcentaje,ano_periodo)
                                            values(@porcentaje,@anoPeriodo)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@porcentaje", anualidad.porcentaje);
            command.Parameters.AddWithValue("@anoPeriodo", anualidad.periodo.anoPeriodo);

            sqlConnection.Open();
            int idAnualidad = Convert.ToInt32(command.ExecuteScalar());
            sqlConnection.Close();

            return idAnualidad;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: actualiza la anualidad
        /// Requiere: anualidad
        /// Modifica: la anualidad que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="anualidad"></param>
        public void actualizarAnualidad(Anualidad anualidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update Anualidad set porcentaje = @porcentaje, ano_periodo = @anoPeriodo
                                            where id_anualidad = @idAnualida";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idAnualida", anualidad.idAnualidad);
            command.Parameters.AddWithValue("@porcentaje", anualidad.porcentaje);
            command.Parameters.AddWithValue("@anoPeriodo", anualidad.periodo.anoPeriodo);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: elimina la anualidad
        /// Requiere: anualidad
        /// Modifica: base de datos eliminando la anualidad
        /// Devuelve: -
        /// </summary>
        /// <param name="anualidad"></param>
        public void eliminarAnualidad(Anualidad anualidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete from Anualidad where 
                                            id_anualidad = @idAnualidad";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idAnualidad", anualidad.idAnualidad);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
    }
}
