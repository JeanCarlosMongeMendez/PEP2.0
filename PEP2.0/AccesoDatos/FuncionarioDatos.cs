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
    /// 12/jul/2019
    /// Clase para administrar las consultas de base de datos de funcionario
    /// </summary>
    public class FuncionarioDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 12/jun/2019
        /// Efecto: obtiene todas los funcionarios de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de funcionarios
        /// </summary>
        /// <returns></returns>
        public List<Funcionario> getFuncionarios()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Funcionario> funcionarios = new List<Funcionario>();

            String consulta = @"select * from Funcionario order by nombre_funcionario;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Planilla planilla = new Planilla();
                planilla.idPlanilla = Convert.ToInt32(reader["id_planilla"].ToString());

                EscalaSalarial escalaSalarial = new EscalaSalarial();
                escalaSalarial.idEscalaSalarial = Convert.ToInt32(reader["id_escala_salarial"].ToString());

                Funcionario funcionario = new Funcionario();
                funcionario.planilla = planilla;
                funcionario.escalaSalarial = escalaSalarial;
                funcionario.fechaIngreso = Convert.ToDateTime(reader["fecha_ingreso"].ToString());
                funcionario.salarioBase1 = Convert.ToDouble(reader["salario_base_1"].ToString());
                funcionario.formulaSalarioBase1 = reader["formula_salario_base_1"].ToString();
                funcionario.noEscalafones1 = Convert.ToInt32(reader["no_escalafones_1"].ToString());
                funcionario.montoEscalafones1 = Convert.ToDouble(reader["monto_escalafones_1"].ToString());
                funcionario.porcentajeAnualidad1 = Convert.ToDouble(reader["porcentaje_anualidad_1"].ToString());
                funcionario.montoAnualidad1 = Convert.ToDouble(reader["monto_anualidad_1"].ToString());
                funcionario.salarioContratacion1 = Convert.ToDouble(reader["salario_contratacion_1"].ToString());
                funcionario.salarioEnero = Convert.ToDouble(reader["salario_enero"].ToString());
                funcionario.conceptoPagoLey1 = Convert.ToDouble(reader["concepto_pago_ley_1"].ToString());
                funcionario.salarioBase2 = Convert.ToDouble(reader["salario_base_2"].ToString());
                funcionario.formulaSalarioBase2 = reader["formula_salario_base_2"].ToString();
                funcionario.noEscalafones2 = Convert.ToInt32(reader["no_escalafones_2"].ToString());
                funcionario.montoEscalafones2 = Convert.ToDouble(reader["monto_escalafones_2"].ToString());
                funcionario.porcentajeAnualidad2 = Convert.ToDouble(reader["porcentaje_anualidad_2"].ToString());
                funcionario.montoAnualidad2 = Convert.ToDouble(reader["monto_anualidad_2"].ToString());
                funcionario.salarioContratacion2 = Convert.ToDouble(reader["salario_contratacion_2"].ToString());
                funcionario.salarioJunio = Convert.ToDouble(reader["salario_junio"].ToString());
                funcionario.conceptoPagoLey2 = Convert.ToDouble(reader["concepto_pago_ley_2"].ToString());
                funcionario.salarioPromedio = Convert.ToDouble(reader["salario_promedio"].ToString());
                funcionario.salarioPropuesto = Convert.ToDouble(reader["salario_propuesto"].ToString());
                funcionario.observaciones = reader["observaciones"].ToString();
                funcionario.nombreFuncionario = reader["nombre_funcionario"].ToString();

                funcionarios.Add(funcionario);
            }

            sqlConnection.Close();

            return funcionarios;
        }
    }
}