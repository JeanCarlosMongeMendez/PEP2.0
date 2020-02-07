using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    class Ejecucion
    {
        private ConexionDatos conexion = new ConexionDatos();
        public void insertarEjecucionUnidad(Unidad unidad, string numeroReferencia )
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"insert Unidad_ejecucion (numero_referencia,id_unidad,nombre_unidad) 
                                            values(@numero_referencia,@id_unidad,@nombre_unidad)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@numero_referencia",numeroReferencia);
            command.Parameters.AddWithValue("@id_unidad", unidad.idUnidad);
            command.Parameters.AddWithValue("@nombre_unidad", unidad.nombreUnidad);



            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
    }
}
