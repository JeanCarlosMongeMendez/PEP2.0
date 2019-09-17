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
    /// Clase para administrar el CRUD para los periodos
    /// </summary>
    public class PeriodoDatos
    {
        private ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Obtiene todos los periodos de la base de datos
        /// </summary>
        /// <returns>Retorna una lista <code>LinkedList<Periodo></code> que contiene todos los periodos</returns>
        public LinkedList<Periodo> ObtenerTodos()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();

            String consulta = @"select ano_periodo, habilitado from Periodo where disponible=1;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());
                periodo.habilitado = Boolean.Parse(reader["habilitado"].ToString());

                periodos.AddLast(periodo);
            }

            sqlConnection.Close();

            return periodos;
        }

        /// <summary>
        /// Obtiene los periodos mayores al periodo actual de la base de datos
        /// </summary>
        /// <returns>Retorna una lista <code>LinkedList<Periodo></code> que contiene los periodos</returns>
        //public LinkedList<Periodo> ObtenerPeriodosSiguientes()
        //{
        //    SqlConnection sqlConnection = conexion.conexionCTL();
        //    SqlConnection sqlConnectionPeriodoActual = conexion.conexionCTL();
        //    LinkedList<Periodo> periodos = new LinkedList<Periodo>();

        //    SqlDataReader reader;
        //    SqlDataReader readerPeriodoActual;
        //    sqlConnectionPeriodoActual.Open();

        //    String consultaPeriodoActual = @"select ano_periodo from Periodo where habilitado=1 AND disponible=1;";
        //    SqlCommand sqlCommandPeriodoActual = new SqlCommand(consultaPeriodoActual, sqlConnectionPeriodoActual);

        //    readerPeriodoActual = sqlCommandPeriodoActual.ExecuteReader();

        //    if (readerPeriodoActual.Read())
        //    {
        //        int periodoActual = Convert.ToInt32(readerPeriodoActual["ano_periodo"].ToString());

        //        sqlConnection.Open();

        //        SqlCommand sqlCommand = new SqlCommand("select ano_periodo, habilitado from Periodo where ano_periodo >= @ano_periodo_ AND disponible=1 order by ano_periodo;", sqlConnection);
        //        sqlCommand.Parameters.AddWithValue("@ano_periodo_", periodoActual);

        //        reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            Periodo periodo = new Periodo();
        //            periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());
        //            periodo.habilitado = Boolean.Parse(reader["habilitado"].ToString());

        //            periodos.AddLast(periodo);
        //        }
        //    }
            
        //    sqlConnection.Close();

        //    return periodos;
        //}

        /// <summary>
        /// Inserta la entidad Periodo en la base de datos
        /// </summary>
        /// <param name="periodo">Elemento de tipo <code>Periodo</code> que va a ser insertado</param>
        /// <returns>Retorna un valor <code>int</code> con el identificador del periodo insertado</returns>
        public int Insertar(Periodo periodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("insert into Periodo(ano_periodo, habilitado, disponible) output INSERTED.ano_periodo values(@ano_periodo_, @habilitado_, @disponible_);", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@ano_periodo_", periodo.anoPeriodo);
            sqlCommand.Parameters.AddWithValue("@habilitado_", periodo.habilitado);
            sqlCommand.Parameters.AddWithValue("@disponible_", true);

            sqlConnection.Open();
            int anoPeriodo = 0;

            try
            {
                anoPeriodo = (int)sqlCommand.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                //Utilidades.ErrorBitacora(ex.Message, "Error al insertar el periodo");
            }

            sqlConnection.Close();

            return anoPeriodo;
        }

        /// <summary>
        /// Primer deshabilita todos los periodos existentes, y habilita el periodo indicado según los criterios del usuario
        /// </summary>
        /// <param name="anoPeriodo">Valor de tipo <code>int</code> que representa el año del periodo que se desea habilitar</param>
        /// <returns>Retorna si el periodo fue habilitado</returns>
        public bool HabilitarPeriodo(int anoPeriodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            bool habilitado = false;
            SqlCommand sqlCommandDeshabilitarTodos = new SqlCommand("update Periodo set habilitado=@habilitado_;", sqlConnection);
            sqlCommandDeshabilitarTodos.Parameters.AddWithValue("@habilitado_", 0);

            SqlCommand sqlCommandHabilitarPeriodo = new SqlCommand("update Periodo set habilitado=@habilitado_ output INSERTED.ano_periodo where ano_periodo=@ano_periodo_;", sqlConnection);
            sqlCommandHabilitarPeriodo.Parameters.AddWithValue("@ano_periodo_", anoPeriodo);
            sqlCommandHabilitarPeriodo.Parameters.AddWithValue("@habilitado_", 1);

            sqlConnection.Open();
            sqlCommandDeshabilitarTodos.ExecuteScalar();
            int idPeriodo = (int)sqlCommandHabilitarPeriodo.ExecuteScalar();

            if (idPeriodo > 0)
            {
                habilitado = true;
            }

            sqlConnection.Close();

            return habilitado;
        }

        /// <summary>
        /// Adrián Serrano
        /// 7/14/2019
        /// Efecto: actualiza un periodo
        /// Requiere: Periodo a modificar
        /// Modifica: Periodo
        /// Devuelve: -
        /// </summary>
        /// <param name="edificio"></param>
        //public void actualizarEdificio(Periodo periodo)
        //{
        //    SqlConnection sqlConnection = conexion.conexionCMEC();

        //    SqlCommand sqlCommand = new SqlCommand("update Periodo set ano_periodo=@ano_periodo_ where id_edificio=@id_edificio;"
        //        , sqlConnection);
        //    sqlCommand.Parameters.AddWithValue("@nombre", edificio.nombre);
        //    sqlCommand.Parameters.AddWithValue("@id_edificio", edificio.idEdificio);

        //    sqlConnection.Open();
        //    sqlCommand.ExecuteReader();

        //    sqlConnection.Close();
        //}

        // <summary>
        // Adrián Serrano
        // 7/14/2019
        // Efecto: Elimina un periodo de forma logica de la base de datos
        // Requiere: Periodo
        // Modifica: -
        // Devuelve: -
        // </summary>
        // <param name="idPeriodo"></param>
        public void EliminarPeriodo(int anoPeriodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand("update Periodo set disponible=0 where ano_periodo=@ano_periodo_;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@ano_periodo_", anoPeriodo);

            sqlConnection.Open();

            sqlCommand.ExecuteScalar();

            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/sep/2019
        /// Efecto: devuelve los periodos que se encuentran en las planillas de fundevi
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de periodos
        /// </summary>
        /// <returns></returns>
        public List<Periodo> getPeriodosPlanillasFundevi()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Periodo> periodos = new List<Periodo>();

            String consulta = @"select P.* from Periodo P, Planilla_fundevi PF where P.ano_periodo = PF.ano_periodo and disponible=1;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());
                periodo.habilitado = Boolean.Parse(reader["habilitado"].ToString());

                periodos.Add(periodo);
            }

            sqlConnection.Close();

            return periodos;
        }

    }
}
