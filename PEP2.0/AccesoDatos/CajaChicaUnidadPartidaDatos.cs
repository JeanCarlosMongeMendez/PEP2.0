using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class CajaChicaUnidadPartidaDatos
    {
        private ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 05/mar/2021
        /// Efecto: devuelve en monto disponible de una partida en especifico segun la unidad
        /// Requiere: unidad y partida
        /// Modifica: -
        /// Devuelve: monto disponible
        /// </summary>
        /// <param name="unidad"></param>
        /// <param name="partida"></param>
        /// <returns></returns>
        public Double getMontoDisponible(Unidad unidad, Partida partida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            Double monto = 0;

            String consulta = @"select (select ISNULL(sum(monto),0) from Presupuesto_Egreso_Partida where id_presupuesto_egreso in (
select id_presupuesto_egreso from Presupuesto_Egreso where id_unidad = @idUnidad
) and id_partida = @idPartida and id_estado_presupuesto = (select id_estado_presupuesto from Estado_presupuestos where (descripcion_estado_presupuesto = 'Aprobar' or descripcion_estado_presupuesto='Comprometer')))
- (select ISNULL(sum(monto),0) from [dbo].[Ejecucion_Unidad_Partida] where id_unidad = @idUnidad and id_partida = @idPartida and 
id_ejecucion in (select id_ejecucion from Ejecucion where id_estado!=1))-(select ISNULL(sum(monto),0) from [dbo].[Caja_Chica_Unidad_Partida] where id_unidad = @idUnidad and id_partida = @idPartida and 
id_solicitud_caja_chica in (select id_solicitud_caja_chica from Solicitud_Caja_Chica where id_estado_caja_chica!=1)) as monto_disponible";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idUnidad", unidad.idUnidad);
            sqlCommand.Parameters.AddWithValue("@idPartida", partida.idPartida);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                monto = Convert.ToDouble(reader["monto_disponible"].ToString());
            }

            sqlConnection.Close();

            return monto;
        }
        /// <summary>
        /// Leonardo Carrion
        /// 12/mar/2021
        /// Efecto: devuelve la lista de unidades partidas y monto segun la ejecucion ingresada
        /// Requiere: ejecucion
        /// Modifica: -
        /// Devuelve: lista de unidades partidas
        /// </summary>
        /// <param name="cajaChica"></param>
        /// <returns></returns>
        public List<PartidaUnidad> getUnidadesPartidasMontoPorCajaChica(CajaChica cajaChica)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            List<PartidaUnidad> listaUnidadesPartidas = new List<PartidaUnidad>();

            String consulta = @"
select P.*,U.*,cup.monto 
from Partida P, Unidad U, Caja_Chica_Unidad_Partida CUP
where CUP.id_solicitud_caja_chica =@idCajaChica and P.id_partida = CUP.id_partida and U.id_unidad = CUP.id_unidad
";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idCajaChica", cajaChica.idCajaChica);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                //Unidad unidad = new Unidad();
                //unidad.idUnidad = Convert.ToInt32(reader["id_unidad"].ToString());
                //unidad.nombreUnidad = reader["nombre_unidad"].ToString();
                //unidad.coordinador = reader["coordinador"].ToString();
                //Partida partida = new Partida();
                //partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                //partida.numeroPartida = reader["numero_partida"].ToString();
                //partida.descripcionPartida = reader["descripcion_partida"].ToString();
                //partida.esUCR = Boolean.Parse(reader["esUCR"].ToString());
                //partida.periodo = new Periodo();
                //partida.periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());


                PartidaUnidad partidaUnidad = new PartidaUnidad();
                partidaUnidad.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partidaUnidad.idUnidad = Convert.ToInt32(reader["id_unidad"].ToString());
                partidaUnidad.monto = Convert.ToDouble(reader["monto"].ToString());
                partidaUnidad.nombreUnidad = reader["nombre_unidad"].ToString();
                partidaUnidad.numeroPartida = reader["numero_partida"].ToString();
                listaUnidadesPartidas.Add(partidaUnidad);
            }

            sqlConnection.Close();

            return listaUnidadesPartidas;
        }

    }
}

