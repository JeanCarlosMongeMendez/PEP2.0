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
    /// Adrián Serrano
    /// 17/may/2019
    /// Clase para administrar el CRUD para las unidades
    /// </summary>
    public class UnidadDatos
    {
        private ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Obtiene todos las unidades del proyecto especificado
        /// </summary>
        /// <param name="idProyecto">Valor de tipo <code>int</code> para filtrar la búsqueda</param>
        /// <returns>Retorna la lista <code>LinkedList<Unidad></code> que contiene las unidades que correspondan al periodo especificado</returns>
        public List<Unidad> ObtenerPorProyecto(int idProyecto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Unidad> unidades = new List<Unidad>();
            SqlCommand sqlCommand = new SqlCommand("select id_unidad, nombre_unidad, coordinador from Unidad where id_proyecto=@id_proyecto_ AND disponible=1;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_proyecto_", idProyecto);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Unidad unidad = new Unidad();
                unidad.idUnidad = Convert.ToInt32(reader["id_unidad"].ToString());
                unidad.nombreUnidad = reader["nombre_unidad"].ToString();
                unidad.coordinador = reader["coordinador"].ToString();
                unidades.Add(unidad);
            }

            sqlConnection.Close();

            return unidades;
        }

        /// <summary>
        /// Inserta la entidad Unidad en la base de datos
        /// </summary>
        /// <param name="unidad">Elemento de tipo <code>Unidad</code> que va a ser insertado</param>
        /// <returns>Retorna un valor <code>int</code> con el identificador de la unidad insertada</returns>
        public int Insertar(Unidad unidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("insert into Unidad(nombre_unidad, coordinador, id_proyecto, disponible) output INSERTED.id_unidad " +
                "values(@nombre_unidad_, @coordinador_, @id_proyecto_, @disponible_);", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@nombre_unidad_", unidad.nombreUnidad);
            sqlCommand.Parameters.AddWithValue("@coordinador_", unidad.coordinador);
            sqlCommand.Parameters.AddWithValue("@id_proyecto_", unidad.proyecto.idProyecto);
            sqlCommand.Parameters.AddWithValue("@disponible_", 1);

            sqlConnection.Open();
            int idUnidad = (int)sqlCommand.ExecuteScalar();

            sqlConnection.Close();

            return idUnidad;
        }

        /// <summary>
        /// Obtiene una unidad basada en su identificador
        /// </summary>
        /// <param name="idUnidad">Valor de tipo <code>int</code> que corresponde a la unidad a buscar</param>
        /// <returns>Retorna el elemento de tipo <code>Unidad</code> que coincida con el identificador dado</returns>
        public Unidad ObtenerPorId(int idUnidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("select id_unidad, nombre_unidad, coordinador from Unidad where id_unidad=@id_unidad_ AND disponible=1;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_unidad_", idUnidad);

            Unidad unidad = new Unidad();

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                unidad.idUnidad = Convert.ToInt32(reader["id_unidad"].ToString());
                unidad.nombreUnidad = reader["nombre_unidad"].ToString();
                unidad.coordinador = reader["coordinador"].ToString();
            }

            sqlConnection.Close();

            return unidad;
        }

        // <summary>
        // Adrián Serrano
        // 7/14/2019
        // Efecto: Elimina una unidad de forma logica de la base de datos
        // Requiere: Unidad
        // Modifica: -
        // Devuelve: -
        // </summary>
        // <param name="idUnidad"></param>
        public void EliminarUnidad(int idUnidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand("update Unidad set disponible=0 where id_unidad=@id_unidad_;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_unidad_", idUnidad);

            sqlConnection.Open();

            sqlCommand.ExecuteScalar();

            sqlConnection.Close();
        }

        // <summary>
        // Adrián Serrano
        // 7/14/2019
        // Efecto: Actualiza una unidad de la base de datos
        // Requiere: Unidad
        // Modifica: -
        // Devuelve: -
        // </summary>
        // <param name="Unidad"></param>
        public void ActualizarUnidad(Unidad unidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand("update Unidad set nombre_unidad=@nombre_unidad_, coordinador=@coordinador_ where id_unidad=@id_unidad_;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_unidad_", unidad.idUnidad);
            sqlCommand.Parameters.AddWithValue("@nombre_unidad_", unidad.nombreUnidad);
            sqlCommand.Parameters.AddWithValue("@coordinador_", unidad.coordinador);

            sqlConnection.Open();

            sqlCommand.ExecuteScalar();

            sqlConnection.Close();
        }
    }
}
