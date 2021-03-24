using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace AccesoDatos
{
    public class EjecucionUnidadParitdaDatos
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
id_ejecucion in (select id_ejecucion from Ejecucion where id_estado!=1)) as monto_disponible";

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
        /// Leonarrdo Carrion
        /// 10/mar/2021
        /// Efecto: guarda en la base de datos en la tabla de Ejecucion_Unidad_Partida
        /// Requiere: datos a ingresar en la tabla
        /// Modifca: -
        /// Devuelve: -
        /// </summary>
        /// <param name="idEjecucion"></param>
        /// <param name="idUnidad"></param>
        /// <param name="idPartida"></param>
        /// <param name="monto"></param>
        public void insertarEjecucionUnidadPartida(int idEjecucion, int idUnidad, int idPartida, Double monto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand("INSERT Ejecucion_Unidad_Partida (id_ejecucion,id_unidad,id_partida,monto) " +
                                                    "values(@idEjecucion,@idUnidad,@idPartida,@monto);", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idEjecucion", idEjecucion);
            sqlCommand.Parameters.AddWithValue("@idUnidad", idUnidad);
            sqlCommand.Parameters.AddWithValue("@idPartida", idPartida);
            sqlCommand.Parameters.AddWithValue("@monto", monto);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();

            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 19/mar/2021
        /// Efecto: elimina ejecucionUnidadPartida
        /// Requiere: idEjecucion
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="idEjecucion"></param>
        public void eliminarEjecucionUnidadPartidaPorEjecucion(int idEjecucion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("delete from Ejecucion_Unidad_Partida where id_ejecucion = @idEjecucion;", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idEjecucion", idEjecucion);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();

            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/mar/2021
        /// Efecto: devuelve la lista de unidades segun la ejecucion ingresada
        /// Requiere: ejecucion
        /// Modifica: -
        /// Devuelve: lista de unidades
        /// </summary>
        /// <param name="ejecucion"></param>
        /// <returns></returns>
        public List<Unidad> getUnidadesPorEjecucion(Ejecucion ejecucion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            List<Unidad> listaUnidades = new List<Unidad>();

            String consulta = @"select * from Unidad where id_unidad in (
select distinct id_unidad from Ejecucion_Unidad_Partida where id_ejecucion=@idEjecucion
)";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idEjecucion", ejecucion.idEjecucion);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Unidad unidad = new Unidad();
                unidad.idUnidad = Convert.ToInt32(reader["id_unidad"].ToString());
                unidad.nombreUnidad = reader["nombre_unidad"].ToString();
                unidad.coordinador = reader["coordinador"].ToString();
                listaUnidades.Add(unidad);
            }

            sqlConnection.Close();

            return listaUnidades;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 17/mar/2021
        /// Efecto: devuelve la lista de partidas segun la ejecucion ingresada
        /// Requiere: ejecucion
        /// Modifica: -
        /// Devuelve: lista de partidas
        /// </summary>
        /// <param name="ejecucion"></param>
        /// <returns></returns>
        public List<Partida> getPartidasPorEjecucion(Ejecucion ejecucion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            List<Partida> listaPartidas = new List<Partida>();

            String consulta = @"select * from Partida where id_partida in (
select distinct id_partida from Ejecucion_Unidad_Partida where id_ejecucion=@idEjecucion
)";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idEjecucion", ejecucion.idEjecucion);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Partida partida = new Partida();
                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partida.numeroPartida = reader["numero_partida"].ToString();
                partida.descripcionPartida = reader["descripcion_partida"].ToString();
                partida.esUCR = Boolean.Parse(reader["esUCR"].ToString());
                partida.periodo = new Periodo();
                partida.periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());
                listaPartidas.Add(partida);
            }

            sqlConnection.Close();

            return listaPartidas;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/mar/2021
        /// Efecto: devuelve la lista de unidades partidas y monto segun la ejecucion ingresada
        /// Requiere: ejecucion
        /// Modifica: -
        /// Devuelve: lista de unidades partidas
        /// </summary>
        /// <param name="ejecucion"></param>
        /// <returns></returns>
        public List<PartidaUnidad> getUnidadesPartidasMontoPorEjecucion(Ejecucion ejecucion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            List<PartidaUnidad> listaUnidadesPartidas = new List<PartidaUnidad>();

            String consulta = @"
select P.*,U.*,eup.monto from Partida P, Unidad U, Ejecucion_Unidad_Partida EUP
where EUP.id_ejecucion =@idEjecucion and P.id_partida = EUP.id_partida and U.id_unidad = EUP.id_unidad
";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idEjecucion", ejecucion.idEjecucion);

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
