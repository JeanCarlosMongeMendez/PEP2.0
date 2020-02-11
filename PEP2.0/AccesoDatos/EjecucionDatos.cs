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
    /// Kevin Picado
    /// 07/febrero/2020
    /// Clase para administrar las consultas sql del Ejecucion
    /// </summary>
    public class EjecucionDatos
    {
        private ConexionDatos conexion = new ConexionDatos();
        /// <summary>
        /// Inserta una EjecucionUnidad
        /// </summary>
        /// <param name="unidad">Unidad</param>
        public void insertarEjecucionUnidad(Unidad unidad, string numeroReferencia)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"insert Unidad_ejecucion (numero_referencia,id_unidad,nombre_unidad) 
                                            values(@numero_referencia,@id_unidad,@nombre_unidad)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@numero_referencia", Convert.ToString(numeroReferencia));
            command.Parameters.AddWithValue("@id_unidad", unidad.idUnidad);
            command.Parameters.AddWithValue("@nombre_unidad", unidad.nombreUnidad);



            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }


        /// <summary>
        /// Inserta una PartidaEjecucion
        /// </summary>
        /// <param name="partida">Presupuesto de Egreso</param>
        public void insertarEjecucionPartidas(Partida partida,string numeroReferencia)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"insert Partida_ejecucion (numero_referencia,id_partida,numero_partida,descripcion_partida) 
                                            values(@numero_referencia,@id_partida,@numero_partida,@descripcion_partida)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@numero_referencia", Convert.ToString(numeroReferencia));
            command.Parameters.AddWithValue("@id_partida", partida.idPartida);
            command.Parameters.AddWithValue("@numero_partida", partida.numeroPartida);
            command.Parameters.AddWithValue("@descripcion_partida", partida.descripcionPartida);



            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }


        /// <summary>
        /// Inserta EjecucionMontoPartidaElegida
        /// </summary>
        /// <param name="partida">Presupuesto de Egreso</param>
        public void insertarEjecucionMontoPartidaElegida(PartidaUnidad partidaUnidad, string numeroReferencia)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"insert EjecucionMontoPartidasElegidas (numero_referencia,id_partida,id_unidad,monto,monto_disponible,numero_partida) 
                                            values(@numero_referencia,@id_partida,@id_unidad,@monto,@monto_disponible,@numero_partida)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@numero_referencia", Convert.ToString(numeroReferencia));
            command.Parameters.AddWithValue("@id_partida", partidaUnidad.IdPartida);
            command.Parameters.AddWithValue("@id_unidad", partidaUnidad.IdUnidad);
            command.Parameters.AddWithValue("@monto", partidaUnidad.Monto);
            command.Parameters.AddWithValue("@monto_disponible", partidaUnidad.MontoDisponible);
            command.Parameters.AddWithValue("@numero_partida", partidaUnidad.NumeroPartida);



            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
        /// <summary>
        /// Inserta EjecucionMontoPartidaElegida
        /// </summary>
        /// <param name="partida">Presupuesto de Egreso</param>
        public void insertarEjecucion(Ejecucion ejecucion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"insert EjecucionMontoPartidasElegidas (numero_referencia,id_partida,id_unidad,monto,monto_disponible,numero_partida) 
                                            values(@numero_referencia,@id_partida,@id_unidad,@monto,@monto_disponible,@numero_partida)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@numero_referencia", ejecucion);
            command.Parameters.AddWithValue("@id_partida", ejecucion);
            command.Parameters.AddWithValue("@id_unidad", ejecucion);
            command.Parameters.AddWithValue("@monto", ejecucion);
            command.Parameters.AddWithValue("@monto_disponible", ejecucion);
            command.Parameters.AddWithValue("@numero_partida", ejecucion);



            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
    }
}

