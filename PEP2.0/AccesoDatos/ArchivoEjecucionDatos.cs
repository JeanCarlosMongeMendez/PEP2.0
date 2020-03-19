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
        /*Leonardo Carrion
         09/09/2016
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
       *Leonardo Carrion
       *08/09/2016
       *recupera todos los archivos de muestras de la base de datos
       *retorna una lista de archivos
       */
        public List<ArchivoEjecucion> getArchivosEjecucion(int idEjecucion)
        {

            List<ArchivoEjecucion> listaArchivosEjecucion = new List<ArchivoEjecucion>();

            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"select id_ejecucion,ruta_archivo
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
               

                listaArchivosEjecucion.Add(archivoEjecucion);
            }

            sqlConnection.Close();

            return listaArchivosEjecucion;
        }
    }
}
