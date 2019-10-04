using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
namespace AccesoDatos
{

    /// <summary>
    /// Josseline M
    /// Clase encargada del manejo de información con base  de datos
    /// </summary>
    public class PartidaPresupuestoEgresoDatos
    {
        private ConexionDatos conexion = new ConexionDatos();
        /// <summary>
        /// Este metodo se encarga de obtener las partidas y habilitar para poder habilitar campo de 
        /// monto y descripcion
        /// </summary>
        /// <param name="anoPeriodo"></param>
        /// <returns></returns>
        public LinkedList<PartidaPresupuestoEgreso> ObtenerPartidaPresupuestoEgresoDatosPorPeriodo(int anoPeriodo)
        {

            LinkedList<PartidaPresupuestoEgreso> partidas = new LinkedList<PartidaPresupuestoEgreso>();
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlConnection sqlConnectionPresupuestoEgreso = conexion.conexionPEP();

            String consulta = "select id_partida, numero_partida, descripcion_partida from Partida where ano_periodo=@ano_periodo_ AND disponible=1 order by numero_partida;";
            String consultaPresupuesto = "select monto, descripcion from Presupuesto_Egreso_Partida where id_partida=@id_partida_;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@ano_periodo_", anoPeriodo);
            
            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                PartidaPresupuestoEgreso partida = new PartidaPresupuestoEgreso();
                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partida.numeroPartida = reader["numero_partida"].ToString();

                SqlCommand sqlCommand2 = new SqlCommand(consultaPresupuesto, sqlConnectionPresupuestoEgreso);
                sqlCommand2.Parameters.AddWithValue("@id_partida_", partida.idPartida);
                SqlDataReader reader2;
                sqlConnectionPresupuestoEgreso.Open();
                reader2 = sqlCommand2.ExecuteReader();

                while (reader2.Read())
                {
                    partida.montoTotal = Convert.ToDouble(reader2["monto"].ToString());
                    partida.descripcion =reader2["descripcion"].ToString();
                }

                sqlConnectionPresupuestoEgreso.Close();
                partidas.AddLast(partida);
            }

            sqlConnection.Close();

            return partidas;
            
        }

         
    }
}
