using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
   public class ArchivoEjecucionDatos
    {
        ConexionDatos conexion = new ConexionDatos();
        /*Kevin Picado
         20/03/20
         Metodo que inserta un archivo muestra en la base de datos
         devuelve el id del archivo ingresado*/
        public int insertarArchivoMuestra(ArchivoEjecucion archivoEjecucion)
        {
            SqlConnection connection = conexion.conexionPEP();

            String consulta
                = @"INSERT Archivo_Ejecucion (id_ejecucion,fecha_creacion, creado_por,nombre_archivo,ruta_archivo) output INSERTED.id_ejecucion 
                    VALUES (@idEjecucion,@fechaCreacion, @creadoPor,@nombreArchivo,@rutaArchivo);";

            SqlCommand command = new SqlCommand(consulta, connection);
            command.Parameters.AddWithValue("@idEjecucion", archivoEjecucion.idEjecucion);
           
            command.Parameters.AddWithValue("@fechaCreacion", archivoEjecucion.fechaCreacion);
            command.Parameters.AddWithValue("@creadoPor", archivoEjecucion.creadoPor);
            command.Parameters.AddWithValue("@nombreArchivo", archivoEjecucion.nombreArchivo);
            command.Parameters.AddWithValue("@rutaArchivo", archivoEjecucion.rutaArchivo);

            connection.Open();
            int idArchivo = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();

            return idArchivo;
        }
        /*
       * Kevin Picado
       * 20/03/20
       *recupera todos los archivos de muestras de la base de datos
       *retorna una lista de archivos
       */
        public List<ArchivoEjecucion> getArchivosEjecucion(int idEjecucion)
        {

            List<ArchivoEjecucion> listaArchivosEjecucion = new List<ArchivoEjecucion>();

            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"select id_ejecucion,ruta_archivo,nombre_archivo,creado_por
                from Archivo_Ejecucion 
                where id_ejecucion=@idEjecucion";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idEjecucion", Convert.ToInt32(idEjecucion));
           

            SqlDataReader reader;
            sqlConnection.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                ArchivoEjecucion archivoEjecucion = new ArchivoEjecucion();
               
                archivoEjecucion.idEjecucion = Convert.ToInt32(reader["id_Ejecucion"].ToString());
                archivoEjecucion.rutaArchivo = reader["ruta_archivo"].ToString();
                archivoEjecucion.nombreArchivo = reader["nombre_archivo"].ToString();
                archivoEjecucion.creadoPor = reader["creado_por"].ToString();
                listaArchivosEjecucion.Add(archivoEjecucion);
            }

            sqlConnection.Close();

            return listaArchivosEjecucion;
        }
        /*
         * Kevin Picado
         * 20/03/20
         * metodo que elimina un archivo muestra en la base de datos
         */
        public void eliminarArchivoEjecucionPorNombreYID(int idEjecucion,string nombreArchivo)
        {

            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand("Delete Archivo_Ejecucion " +
                                                    "where id_ejecucion = @idEjecucion and nombre_archivo = @nombreArchivo;", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idEjecucion",idEjecucion);
            sqlCommand.Parameters.AddWithValue("@nombreArchivo", nombreArchivo);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();

            sqlConnection.Close();

           
        }
        /*
        * Kevin Picado
        * 20/03/20
        * metodo que elimina un archivo muestra en la base de datos
        */
        public void eliminarArchivoEjecucion(int idEjecucion)
        {

            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand("Delete Archivo_Ejecucion " +
                                                    "where id_ejecucion = @idEjecucion ;", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idEjecucion", idEjecucion);
          
            sqlConnection.Open();
            sqlCommand.ExecuteReader();

            sqlConnection.Close();


        }
    }
}

