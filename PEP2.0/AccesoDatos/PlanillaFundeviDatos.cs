using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace AccesoDatos
{
    public class PlanillaFundeviDatos
    {
        private ConexionDatos conexion = new ConexionDatos();
        private FuncionarioFundeviDatos funcionarioDatos = new FuncionarioFundeviDatos();

        /// <summary>
        /// Juan Solano Brenes
        /// 14/06/2019
        /// Insertar una nueva planillaFundevi a la base de datos
        /// </summary>
        /// <param name="anoPeriodo"></param>
        /// <returns></returns>
        public Boolean Insertar(int anoPeriodo)
        {
            Boolean bandera = false;
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("insert into Planilla_fundevi(ano_periodo) " +
                "values(@ano_periodo);", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@ano_periodo", anoPeriodo);

            sqlConnection.Open();
            if (sqlCommand.ExecuteScalar() != null)
            {
                return bandera;
            }

            sqlConnection.Close();

            return bandera;
        }

        /// <summary>
        /// Juan Solano Brenes  
        /// 20/06/2019
        /// Devuelve todas las planillas de fundevi
        /// </summary>
        /// <returns>Retorna una lista de palnillasFundevi</returns>
        public List<PlanillaFundevi> GetPlanillaFundevi()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<PlanillaFundevi> planillas = new List<PlanillaFundevi>();
            SqlCommand sqlCommand = new SqlCommand("select * from Planilla_fundevi", sqlConnection);


            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                PlanillaFundevi planillaFundevi = new PlanillaFundevi();
                planillaFundevi.idPlanilla = Convert.ToInt32(reader["id_planilla"].ToString());
                planillaFundevi.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());

                planillas.Add(planillaFundevi);
            }

            sqlConnection.Close();
            return planillas;
        }

        /// <summary>
        /// Juan Solano Brenes
        /// 21/06/2019
        /// Devuelve la planilla del año que recibe
        /// </summary>
        /// <param name="anoPeriodo"></param>
        /// <returns>una planilla</returns>
        public PlanillaFundevi getPlanilla(int anoPeriodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            //String consulta = "select id_planilla,ano_periodo from Planilla_fundevi where ano_periodo=" + anoPeriodo;
            //SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("select id_planilla,ano_periodo " +
               "from Planilla_fundevi where ano_periodo=@ano;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@ano", anoPeriodo);
            PlanillaFundevi planillaFundevi = new PlanillaFundevi();
            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    planillaFundevi.idPlanilla = Convert.ToInt32(reader["id_planilla"].ToString());
                    planillaFundevi.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            sqlConnection.Close();
            return planillaFundevi;
        }
        /// <summary>
        /// Juan Solano Brenes
        /// Elimina la planilla de fundevi y los registros de funcionarios
        /// </summary>
        /// <returns>true si se elimino la planilla correctamente</returns>
        public Boolean eliminarPlanilla(int idPlanilla)
        {
            Boolean flag = false;
            SqlConnection sqlConnection = conexion.conexionPEP();

            if (funcionarioDatos.EliminarFuncionarios(idPlanilla))
            {
                String consulta = "delete from Planilla_fundevi where id_planilla=" + idPlanilla;
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                if (sqlCommand.ExecuteNonQuery() > 0)
                {
                    flag = true;
                }
            }
            return flag;
        }
    }
}