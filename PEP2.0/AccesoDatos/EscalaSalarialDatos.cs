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
    /// 10/jun/2019
    /// Clase para administra las consultas de Escala Salarial
    /// </summary>
    public class EscalaSalarialDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 10/jun/2019
        /// Efecto: obtiene todas las escalas salariales de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de escalas salariales
        /// </summary>
        /// <returns></returns>
        public List<EscalaSalarial> getEscalasSalariales()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<EscalaSalarial> escalas = new List<EscalaSalarial>();

            String consulta = @"select * from EscalaSalarial order by desc_salarial;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());

                EscalaSalarial escalaSalarial = new EscalaSalarial();

                escalaSalarial.idEscalaSalarial = Convert.ToInt32(reader["id_escala_salarial"].ToString());
                escalaSalarial.descEscalaSalarial = reader["desc_escala_salarial"].ToString();
                escalaSalarial.salarioBase1 = Convert.ToDouble(reader["salario_base_1"].ToString());
                escalaSalarial.salarioBase2 = Convert.ToDouble(reader["salario_base_2"].ToString());
                escalaSalarial.topeEscalafones = Convert.ToInt32(reader["tope_escalafones"].ToString());
                escalaSalarial.porentajeEscalafones = Convert.ToDouble(reader["porcentaje_escalafones"].ToString());
                escalaSalarial.periodo = periodo;

                escalas.Add(escalaSalarial);
            }

            sqlConnection.Close();

            return escalas;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/jun/2019
        /// Efecto: obtiene todas las escalas salariales de la base de datos segun un periodo especifico
        /// Requiere: periodo
        /// Modifica: -
        /// Devuelve: lista de escalas salariales
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<EscalaSalarial> getEscalasSalarialesPorPeriodo(Periodo periodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<EscalaSalarial> escalas = new List<EscalaSalarial>();

            String consulta = @"select ES.* from EscalaSalarial ES where ES.ano_periodo = @anoPeriodo order by ES.desc_escala_salarial;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@anoPeriodo", periodo.anoPeriodo);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                EscalaSalarial escalaSalarial = new EscalaSalarial();

                escalaSalarial.idEscalaSalarial = Convert.ToInt32(reader["id_escala_salarial"].ToString());
                escalaSalarial.descEscalaSalarial = reader["desc_escala_salarial"].ToString();
                escalaSalarial.salarioBase1 = Convert.ToDouble(reader["salario_base_1"].ToString());
                escalaSalarial.salarioBase2 = Convert.ToDouble(reader["salario_base_2"].ToString());
                escalaSalarial.topeEscalafones = Convert.ToInt32(reader["tope_escalafones"].ToString());
                escalaSalarial.porentajeEscalafones = Convert.ToDouble(reader["porcentaje_escalafones"].ToString());
                escalaSalarial.periodo = periodo;

                escalas.Add(escalaSalarial);
            }

            sqlConnection.Close();

            return escalas;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 13/jun/2019
        /// Efecto: inserta en la base de datos una escala salarial
        /// Requiere: escala salarial
        /// Modifica: -
        /// Devuelve: id de la escala insertada
        /// </summary>
        /// <param name="escalaSalarial"></param>
        /// <returns></returns>
        public int insertarEscalaSalarial(EscalaSalarial escalaSalarial)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"Insert EscalaSalarial(desc_escala_salarial,salario_base_1,salario_base_2,tope_escalafones,porcentaje_escalafones,ano_periodo)
                                            values(@descEscalaSalarial,@salarioBase1,@salarioBase2,@topeEscalafones,@porcentajeEscalafones,@anoPeriodo);SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@descEscalaSalarial", escalaSalarial.descEscalaSalarial);
            command.Parameters.AddWithValue("@salarioBase1", escalaSalarial.salarioBase1);
            command.Parameters.AddWithValue("@salarioBase2", escalaSalarial.salarioBase2);
            command.Parameters.AddWithValue("@topeEscalafones", escalaSalarial.topeEscalafones);
            command.Parameters.AddWithValue("@porcentajeEscalafones", escalaSalarial.porentajeEscalafones);
            command.Parameters.AddWithValue("@anoPeriodo", escalaSalarial.periodo.anoPeriodo);

            sqlConnection.Open();
            int idEscalaSalarial = Convert.ToInt32(command.ExecuteScalar());
            sqlConnection.Close();

            return idEscalaSalarial;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 25/jun/2019
        /// Efecto: actualiza la escala salarial
        /// Requiere: escala salarial 
        /// Modifica: la escala salarial que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="escalaSalarial"></param>
        public void actualizarEscalaSalarial(EscalaSalarial escalaSalarial)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update EscalaSalarial set desc_escala_salarial = @descEscalaSalarial, salario_base_1 = @salarioBase1, salario_base_2 = @salarioBase2, 
                                            tope_escalafones = @topeEscalafones, porcentaje_escalafones = @porcentajeEscalafones, ano_periodo = @anoPeriodo where 
                                            id_escala_salarial = @idEscalaSalarial";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idEscalaSalarial", escalaSalarial.idEscalaSalarial);
            command.Parameters.AddWithValue("@descEscalaSalarial", escalaSalarial.descEscalaSalarial);
            command.Parameters.AddWithValue("@salarioBase1", escalaSalarial.salarioBase1);
            command.Parameters.AddWithValue("@salarioBase2", escalaSalarial.salarioBase2);
            command.Parameters.AddWithValue("@topeEscalafones", escalaSalarial.topeEscalafones);
            command.Parameters.AddWithValue("@porcentajeEscalafones", escalaSalarial.porentajeEscalafones);
            command.Parameters.AddWithValue("@anoPeriodo", escalaSalarial.periodo.anoPeriodo);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/jul/2019
        /// Efecto: elimina la escala salarial
        /// Requiere: escala salarial 
        /// Modifica: base de datos eliminando la escala salarial
        /// Devuelve: -
        /// </summary>
        /// <param name="escalaSalarial"></param>
        public void eliminarEscalaSalarial(EscalaSalarial escalaSalarial)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete from EscalaSalarial where 
                                            id_escala_salarial = @idEscalaSalarial";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idEscalaSalarial", escalaSalarial.idEscalaSalarial);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

    }
}