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

            String consulta = @"select f.*, es.id_escala_salarial, es.desc_escala_salarial, 
                es.salario_base_1 AS escala_salario_base_1, es.salario_base_2 AS escala_salario_base_2, 
                jl.*
                FROM Funcionario f 
                JOIN EscalaSalarial es ON f.id_escala_salarial = es.id_escala_salarial
                JOIN Jornada jl ON jl.id_jornada = f.id_jornada_laboral 
                ORDER BY nombre_funcionario;";

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
                escalaSalarial.descEscalaSalarial = reader["desc_escala_salarial"].ToString();
                escalaSalarial.salarioBase1 = Convert.ToDouble(reader["escala_salario_base_1"].ToString());
                escalaSalarial.salarioBase2 = Convert.ToDouble(reader["escala_salario_base_2"].ToString());

                Jornada jornada = new Jornada();
                jornada.idJornada = Convert.ToInt32(reader["id_jornada"].ToString());
                jornada.porcentajeJornada = Convert.ToDouble(reader["porcentaje_jornada"].ToString());
                jornada.descJornada = reader["desc_jornada"].ToString();

                Funcionario funcionario = new Funcionario();
                funcionario.planilla = planilla;
                funcionario.escalaSalarial = escalaSalarial;
                funcionario.JornadaLaboral = jornada;
                funcionario.fechaIngreso = Convert.ToDateTime(reader["fecha_ingreso"].ToString());
                funcionario.salarioBase1 = Convert.ToDouble(reader["salario_base_1"].ToString());
                funcionario.noEscalafones1 = Convert.ToInt32(reader["no_escalafones_1"].ToString());
                funcionario.montoEscalafones1 = Convert.ToDouble(reader["monto_escalafones_1"].ToString());
                funcionario.porcentajeAnualidad1 = Convert.ToDouble(reader["porcentaje_anualidad_1"].ToString());
                funcionario.montoAnualidad1 = Convert.ToDouble(reader["monto_anualidad_1"].ToString());
                funcionario.salarioContratacion1 = Convert.ToDouble(reader["salario_contratacion_1"].ToString());
                funcionario.salarioEnero = Convert.ToDouble(reader["salario_enero"].ToString());
                funcionario.conceptoPagoLey = Convert.ToDouble(reader["concepto_pago_ley"].ToString());
                funcionario.salarioBase2 = Convert.ToDouble(reader["salario_base_2"].ToString());
                funcionario.noEscalafones2 = Convert.ToInt32(reader["no_escalafones_2"].ToString());
                funcionario.montoEscalafones2 = Convert.ToDouble(reader["monto_escalafones_2"].ToString());
                funcionario.porcentajeAnualidad2 = Convert.ToDouble(reader["porcentaje_anualidad_2"].ToString());
                funcionario.montoAnualidad2 = Convert.ToDouble(reader["monto_anualidad_2"].ToString());
                funcionario.salarioContratacion2 = Convert.ToDouble(reader["salario_contratacion_2"].ToString());
                funcionario.salarioJunio = Convert.ToDouble(reader["salario_junio"].ToString());
                funcionario.salarioPromedio = Convert.ToDouble(reader["salario_promedio"].ToString());
                funcionario.salarioPropuesto = Convert.ToDouble(reader["salario_propuesto"].ToString());
                funcionario.observaciones = reader["observaciones"].ToString();
                funcionario.nombreFuncionario = reader["nombre_funcionario"].ToString();
                funcionario.porcentajeSumaSalario = Convert.ToDouble(reader["porcentaje_suma_salario"].ToString());
                funcionario.idFuncionario = Convert.ToInt32(reader["id_funionario"].ToString());

                funcionarios.Add(funcionario);
            }

            sqlConnection.Close();

            return funcionarios;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/jun/2019
        /// Efecto: obtiene todas los funcionarios de un periodo de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de funcionarios
        /// </summary>
        /// <returns></returns>
        public List<Funcionario> getFuncionarios(int idPlanilla)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Funcionario> funcionarios = new List<Funcionario>();

            String consulta = @"select f.*, es.id_escala_salarial, es.desc_escala_salarial, 
                es.salario_base_1 AS escala_salario_base_1, es.salario_base_2 AS escala_salario_base_2, 
                jl.*
                FROM Funcionario f 
                JOIN EscalaSalarial es ON f.id_escala_salarial = es.id_escala_salarial
                JOIN Jornada jl ON jl.id_jornada = f.id_jornada_laboral 
                WHERE id_planilla = @planilla
                ORDER BY nombre_funcionario;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@planilla", idPlanilla);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Planilla planilla = new Planilla();
                planilla.idPlanilla = Convert.ToInt32(reader["id_planilla"].ToString());

                EscalaSalarial escalaSalarial = new EscalaSalarial();
                escalaSalarial.idEscalaSalarial = Convert.ToInt32(reader["id_escala_salarial"].ToString());
                escalaSalarial.descEscalaSalarial = reader["desc_escala_salarial"].ToString();
                escalaSalarial.salarioBase1 = Convert.ToDouble(reader["escala_salario_base_1"].ToString());
                escalaSalarial.salarioBase2 = Convert.ToDouble(reader["escala_salario_base_2"].ToString());

                Jornada jornada = new Jornada();
                jornada.idJornada = Convert.ToInt32(reader["id_jornada"].ToString());
                jornada.porcentajeJornada = Convert.ToDouble(reader["porcentaje_jornada"].ToString());
                jornada.descJornada = reader["desc_jornada"].ToString();

                Funcionario funcionario = new Funcionario();
                funcionario.planilla = planilla;
                funcionario.escalaSalarial = escalaSalarial;
                funcionario.JornadaLaboral = jornada;
                funcionario.fechaIngreso = Convert.ToDateTime(reader["fecha_ingreso"].ToString());
                funcionario.salarioBase1 = Convert.ToDouble(reader["salario_base_1"].ToString());
                funcionario.noEscalafones1 = Convert.ToInt32(reader["no_escalafones_1"].ToString());
                funcionario.montoEscalafones1 = Convert.ToDouble(reader["monto_escalafones_1"].ToString());
                funcionario.porcentajeAnualidad1 = Convert.ToDouble(reader["porcentaje_anualidad_1"].ToString());
                funcionario.montoAnualidad1 = Convert.ToDouble(reader["monto_anualidad_1"].ToString());
                funcionario.salarioContratacion1 = Convert.ToDouble(reader["salario_contratacion_1"].ToString());
                funcionario.salarioEnero = Convert.ToDouble(reader["salario_enero"].ToString());
                funcionario.conceptoPagoLey = Convert.ToDouble(reader["concepto_pago_ley"].ToString());
                funcionario.salarioBase2 = Convert.ToDouble(reader["salario_base_2"].ToString());
                funcionario.noEscalafones2 = Convert.ToInt32(reader["no_escalafones_2"].ToString());
                funcionario.montoEscalafones2 = Convert.ToDouble(reader["monto_escalafones_2"].ToString());
                funcionario.porcentajeAnualidad2 = Convert.ToDouble(reader["porcentaje_anualidad_2"].ToString());
                funcionario.montoAnualidad2 = Convert.ToDouble(reader["monto_anualidad_2"].ToString());
                funcionario.salarioContratacion2 = Convert.ToDouble(reader["salario_contratacion_2"].ToString());
                funcionario.salarioJunio = Convert.ToDouble(reader["salario_junio"].ToString());
                funcionario.salarioPromedio = Convert.ToDouble(reader["salario_promedio"].ToString());
                funcionario.salarioPropuesto = Convert.ToDouble(reader["salario_propuesto"].ToString());
                funcionario.observaciones = reader["observaciones"].ToString();
                funcionario.nombreFuncionario = reader["nombre_funcionario"].ToString();
                funcionario.porcentajeSumaSalario = Convert.ToDouble(reader["porcentaje_suma_salario"].ToString());
                funcionario.idFuncionario = Convert.ToInt32(reader["id_funionario"].ToString());

                funcionarios.Add(funcionario);
            }

            sqlConnection.Close();

            return funcionarios;
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 18/09/2019
        /// Efecto : Guarda en la base de datos un funcionario
        /// Requiere : Funcionario que se desea guardar en la base de datos
        /// Modifica : Base de datos, tabla Funcionario
        /// Devuelve : true si se insertó correctamente, false si falló
        /// </summary>
        /// <param name="funcionario"></param>
        /// <returns></returns>
        public bool guardar(Funcionario funcionario)
        {
            bool result = false;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommandInsertar = new SqlCommand();
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection = conexion.conexionPEP();
                sqlCommandInsertar = new SqlCommand("INSERT INTO Funcionario " +
                    "(id_planilla ,id_escala_salarial ,fecha_ingreso ,salario_base_1 ,no_escalafones_1" +
                    ",monto_escalafones_1 ,porcentaje_anualidad_1 ,monto_anualidad_1 ,salario_contratacion_1" +
                    ",salario_enero ,concepto_pago_ley ,salario_base_2 ,no_escalafones_2 ,monto_escalafones_2" +
                    ",porcentaje_anualidad_2 ,monto_anualidad_2 ,salario_contratacion_2 ,salario_junio" +
                    ",salario_promedio ,salario_propuesto ,observaciones ,nombre_funcionario, porcentaje_suma_salario, " +
                    "id_jornada_laboral)" +
                    "VALUES (@id_planilla, @id_escala_salarial, @fecha_ingreso, @salario_base_1," +
                    "@no_escalafones_1, @monto_escalafones_1, @porcentaje_anualidad_1," +
                    "@monto_anualidad_1, @salario_contratacion_1, @salario_enero," +
                    "@concepto_pago_ley, @salario_base_2, @no_escalafones_2," +
                    "@monto_escalafones_2, @porcentaje_anualidad_2, @monto_anualidad_2," +
                    "@salario_contratacion_2, @salario_junio, @salario_promedio, @salario_propuesto," +
                    "@observaciones, @nombre_funcionario, @porcentaje_suma_salario, @id_jornada) ", sqlConnection);

                sqlCommandInsertar.Parameters.AddWithValue("@id_planilla", funcionario.planilla.idPlanilla);
                sqlCommandInsertar.Parameters.AddWithValue("@id_escala_salarial", funcionario.escalaSalarial.idEscalaSalarial);
                sqlCommandInsertar.Parameters.AddWithValue("@fecha_ingreso", funcionario.fechaIngreso);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_base_1", funcionario.salarioBase1);
                sqlCommandInsertar.Parameters.AddWithValue("@no_escalafones_1", funcionario.noEscalafones1);
                sqlCommandInsertar.Parameters.AddWithValue("@monto_escalafones_1", funcionario.montoEscalafones1);
                sqlCommandInsertar.Parameters.AddWithValue("@porcentaje_anualidad_1", funcionario.porcentajeAnualidad1);
                sqlCommandInsertar.Parameters.AddWithValue("@monto_anualidad_1", funcionario.montoAnualidad1);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_contratacion_1", funcionario.salarioContratacion1);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_enero", funcionario.salarioEnero);
                sqlCommandInsertar.Parameters.AddWithValue("@concepto_pago_ley", funcionario.conceptoPagoLey);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_base_2", funcionario.salarioBase2);
                sqlCommandInsertar.Parameters.AddWithValue("@no_escalafones_2", funcionario.noEscalafones2);
                sqlCommandInsertar.Parameters.AddWithValue("@monto_escalafones_2", funcionario.montoEscalafones2);
                sqlCommandInsertar.Parameters.AddWithValue("@porcentaje_anualidad_2", funcionario.porcentajeAnualidad2);
                sqlCommandInsertar.Parameters.AddWithValue("@monto_anualidad_2", funcionario.montoAnualidad2);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_contratacion_2", funcionario.salarioContratacion2);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_junio", funcionario.salarioJunio);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_promedio", funcionario.salarioPromedio);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_propuesto", funcionario.salarioPropuesto);
                sqlCommandInsertar.Parameters.AddWithValue("@observaciones", funcionario.observaciones);
                sqlCommandInsertar.Parameters.AddWithValue("@nombre_funcionario", funcionario.nombreFuncionario);
                sqlCommandInsertar.Parameters.AddWithValue("@porcentaje_suma_salario", funcionario.porcentajeSumaSalario);
                sqlCommandInsertar.Parameters.AddWithValue("@id_jornada", funcionario.JornadaLaboral.idJornada);

                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommandInsertar.Transaction = sqlTransaction;
                sqlCommandInsertar.ExecuteNonQuery();
                sqlTransaction.Commit();
                result = true;
            }
            catch
            {
                sqlTransaction.Rollback();
            }
            finally
            {
                sqlConnection.Close();
            }
            return result;
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 25/09/2019
        /// Efecto : Edita un funcionario en la base de datos
        /// Requiere : Funcionario que se desea editar en la base de datos
        /// Modifica : Base de datos, tabla Funcionario
        /// Devuelve : true si se modificó correctamente, false si falló
        /// </summary>
        /// <param name="funcionario"></param>
        /// <returns></returns>
        public bool modificar(Funcionario funcionario)
        {
            bool result = false;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommandInsertar = new SqlCommand();
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection = conexion.conexionPEP();
                sqlCommandInsertar = new SqlCommand("UPDATE Funcionario " +
                    "SET id_planilla = @id_planilla," +
                    "id_escala_salarial = @id_escala_salarial," +
                    "fecha_ingreso = @fecha_ingreso," +
                    "salario_base_1 = @salario_base_1," +
                    "no_escalafones_1 = @no_escalafones_1," +
                    "monto_escalafones_1 = @monto_escalafones_1," +
                    "porcentaje_anualidad_1 = @porcentaje_anualidad_1," +
                    "monto_anualidad_1 = @monto_anualidad_1," +
                    "salario_contratacion_1 = @salario_contratacion_1," +
                    "salario_enero = @salario_enero," +
                    "concepto_pago_ley = @concepto_pago_ley," +
                    "salario_base_2 = @salario_base_2," +
                    "no_escalafones_2 = @no_escalafones_2," +
                    "monto_escalafones_2 = @monto_escalafones_2," +
                    "porcentaje_anualidad_2 = @porcentaje_anualidad_2," +
                    "monto_anualidad_2 = @monto_anualidad_2," +
                    "salario_contratacion_2 = @salario_contratacion_2," +
                    "salario_junio = @salario_junio," +
                    "salario_promedio = @salario_promedio," +
                    "salario_propuesto = @salario_propuesto," +
                    "observaciones = @observaciones," +
                    "nombre_funcionario = @nombre_funcionario," +
                    "id_jornada_laboral = @id_jornada," +
                    "porcentaje_suma_salario = @porcentaje_suma_salario " +
                    "WHERE id_funionario = @id", sqlConnection);

                sqlCommandInsertar.Parameters.AddWithValue("@id_planilla", funcionario.planilla.idPlanilla);
                sqlCommandInsertar.Parameters.AddWithValue("@id_escala_salarial", funcionario.escalaSalarial.idEscalaSalarial);
                sqlCommandInsertar.Parameters.AddWithValue("@fecha_ingreso", funcionario.fechaIngreso);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_base_1", funcionario.salarioBase1);
                sqlCommandInsertar.Parameters.AddWithValue("@no_escalafones_1", funcionario.noEscalafones1);
                sqlCommandInsertar.Parameters.AddWithValue("@monto_escalafones_1", funcionario.montoEscalafones1);
                sqlCommandInsertar.Parameters.AddWithValue("@porcentaje_anualidad_1", funcionario.porcentajeAnualidad1);
                sqlCommandInsertar.Parameters.AddWithValue("@monto_anualidad_1", funcionario.montoAnualidad1);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_contratacion_1", funcionario.salarioContratacion1);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_enero", funcionario.salarioEnero);
                sqlCommandInsertar.Parameters.AddWithValue("@concepto_pago_ley", funcionario.conceptoPagoLey);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_base_2", funcionario.salarioBase2);
                sqlCommandInsertar.Parameters.AddWithValue("@no_escalafones_2", funcionario.noEscalafones2);
                sqlCommandInsertar.Parameters.AddWithValue("@monto_escalafones_2", funcionario.montoEscalafones2);
                sqlCommandInsertar.Parameters.AddWithValue("@porcentaje_anualidad_2", funcionario.porcentajeAnualidad2);
                sqlCommandInsertar.Parameters.AddWithValue("@monto_anualidad_2", funcionario.montoAnualidad2);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_contratacion_2", funcionario.salarioContratacion2);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_junio", funcionario.salarioJunio);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_promedio", funcionario.salarioPromedio);
                sqlCommandInsertar.Parameters.AddWithValue("@salario_propuesto", funcionario.salarioPropuesto);
                sqlCommandInsertar.Parameters.AddWithValue("@observaciones", funcionario.observaciones);
                sqlCommandInsertar.Parameters.AddWithValue("@nombre_funcionario", funcionario.nombreFuncionario);
                sqlCommandInsertar.Parameters.AddWithValue("@id_jornada", funcionario.JornadaLaboral.idJornada);
                sqlCommandInsertar.Parameters.AddWithValue("@porcentaje_suma_salario", funcionario.porcentajeSumaSalario);
                sqlCommandInsertar.Parameters.AddWithValue("@id", funcionario.idFuncionario);

                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommandInsertar.Transaction = sqlTransaction;
                sqlCommandInsertar.ExecuteNonQuery();
                sqlTransaction.Commit();
                result = true;
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            return result;
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 25/09/2019
        /// Efecto : Elimina un funcionario en la base de datos
        /// Requiere : Funcionario que se desea eliminar en la base de datos
        /// Modifica : Base de datos, tabla Funcionario
        /// Devuelve : true si se eliminó correctamente, false si falló
        /// </summary>
        /// <param name="funcionario"></param>
        /// <returns></returns>
        public bool eliminar(Funcionario funcionario)
        {
            bool result = false;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommandEliminar = new SqlCommand();
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection = conexion.conexionPEP();
                sqlCommandEliminar = new SqlCommand("DELETE Funcionario WHERE id_funionario = @id", sqlConnection);
                sqlCommandEliminar.Parameters.AddWithValue("@id", funcionario.idFuncionario);
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommandEliminar.Transaction = sqlTransaction;
                sqlCommandEliminar.ExecuteNonQuery();
                sqlTransaction.Commit();
                result = true;
            }
            catch
            {
                sqlTransaction.Rollback();
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
            return result;
        }

    }
}