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
    /// 14/jun/2019
    /// Clase para administra las consultas de Planilla
    /// </summary>
    public class PlanillaDatos
    {

        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 14/jun/2019
        /// Efecto: obtiene todas las planillas de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de planillas
        /// </summary>
        /// <returns></returns>
        public List<Planilla> getPlanillas()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Planilla> planillas = new List<Planilla>();

            String consulta = @"select * from Planilla order by ano_periodo desc;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());

                Planilla planilla = new Planilla();

                planilla.idPlanilla = Convert.ToInt32(reader["id_planilla"].ToString());
                planilla.anualidad1 = Convert.ToDouble(reader["anualidad_1"].ToString());
                planilla.anualidad2 = Convert.ToDouble(reader["anualidad_2"].ToString());
                planilla.periodo = periodo;

                planillas.Add(planilla);
            }

            sqlConnection.Close();

            return planillas;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 14/jun/2019
        /// Efecto: inserta en la base de datos una planilla
        /// Requiere: planilla
        /// Modifica: -
        /// Devuelve: id de la planilla insertada
        /// </summary>
        /// <param name="planilla"></param>
        /// <returns></returns>
        public int insertarPlanilla(Planilla planilla)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"Insert Planilla(anualidad_1,anualidad_2,ano_periodo)
                                            values(@anualidad1,@anualidad2,@anoPeriodo)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@anualidad1", planilla.anualidad1);
            command.Parameters.AddWithValue("@anualidad2", planilla.anualidad2);
            command.Parameters.AddWithValue("@anoPeriodo", planilla.periodo.anoPeriodo);

            sqlConnection.Open();
            int idPlanilla = Convert.ToInt32(command.ExecuteScalar());
            sqlConnection.Close();

            return idPlanilla;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 08/jul/2019
        /// Efecto: actualiza la planilla
        /// Requiere: planilla
        /// Modifica: la planilla que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="planilla"></param>
        public void actualizarPlanilla(Planilla planilla)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update Planilla set anualidad_1 = @anualidad1, anualidad_2 = @anualidad2, ano_periodo = @anoPeriodo
                                            where id_planilla = @idPlanilla";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@anualidad1", planilla.anualidad1);
            command.Parameters.AddWithValue("@anualidad2", planilla.anualidad2);
            command.Parameters.AddWithValue("@idPlanilla", planilla.idPlanilla);
            command.Parameters.AddWithValue("@anoPeriodo", planilla.periodo.anoPeriodo);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/jul/2019
        /// Efecto: eliminar la planilla
        /// Requiere: planilla
        /// Modifica: la planilla que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="planilla"></param>
        public void eliminarPlanilla(Planilla planilla)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete Planilla where id_planilla = @idPlanilla";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idPlanilla", planilla.idPlanilla);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

    }
}