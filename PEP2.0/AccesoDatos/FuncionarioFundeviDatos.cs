using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using Entidades;

namespace AccesoDatos
{
    public class FuncionarioFundeviDatos
    {
        private ConexionDatos conexion = new ConexionDatos();
        /// <summary>
        /// Juan Solano Brenes
        /// 20/06/2019
        /// Metodo encargado de insertar nuevos funcionarios
        /// </summary>
        /// <param name="funcionario"></param>
        /// <returns>true si es insertado con exito</returns>
        public Boolean InsertFuncionario(FuncionarioFundevi funcionario)
        {
            Boolean flag = false;
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("insert into Funcionario_fundevi(id_planilla,nombre_funcionario,salario)  " +
                "values(@id_planilla,@nombre_funcionario,@salario);", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_planilla", funcionario.idPlanilla);
            sqlCommand.Parameters.AddWithValue("@nombre_funcionario", funcionario.nombre);
            sqlCommand.Parameters.AddWithValue("@salario", funcionario.salario);
            sqlConnection.Open();
            if (sqlCommand.ExecuteNonQuery() != -1)
            {
                flag = true;
            }
            sqlConnection.Close();
            return flag;
        }

        /// <summary>
        /// Juan Solano Brenes
        /// Obtiene todos los funcionarios
        /// 20/06/2019
        /// </summary>
        /// <returns>Una lista de funcionarios</returns>
        public List<FuncionarioFundevi> GetAllFuncionario()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<FuncionarioFundevi> funcionarios = new List<FuncionarioFundevi>();
            String consulta = @"select id_funcionario,id_planilla,nombre_funcionario,salario from Funcionario_fundevi";
            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                FuncionarioFundevi funcionario = new FuncionarioFundevi();
                funcionario.idFuncionario = Convert.ToInt32(reader["id_funcionario"].ToString());
                funcionario.idPlanilla = Convert.ToInt32(reader["id_planilla"].ToString());
                funcionario.nombre = reader["nombre_funcionario"].ToString();
                funcionario.salario = Convert.ToInt32(reader["salario"].ToString());
                funcionarios.Add(funcionario);
            }
            return funcionarios;
        }


        /// <summary>
        /// Juan Solano Brenes
        /// 21/06/2019
        /// Obtiene los funcionarios de una planilla
        /// </summary>
        /// <param name="planilla"></param>
        /// <returns>lista de funcionariosFundevi</returns>
        public List<FuncionarioFundevi> GetFuncionariosPorPlanilla(PlanillaFundevi planilla)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<FuncionarioFundevi> funcionarios = new List<FuncionarioFundevi>();
            String consulta = @"select id_funcionario,id_planilla,nombre_funcionario,salario from Funcionario_fundevi  
                              where id_planilla=" + planilla.idPlanilla;
            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                FuncionarioFundevi funcionario = new FuncionarioFundevi();
                funcionario.idFuncionario = Convert.ToInt32(reader["id_funcionario"].ToString());
                funcionario.idPlanilla = Convert.ToInt32(reader["id_planilla"].ToString());
                funcionario.nombre = reader["nombre_funcionario"].ToString();
                funcionario.salario = Convert.ToInt32(reader["salario"].ToString());
                funcionarios.Add(funcionario);
            }
            return funcionarios;
        }
        /// <summary>
        /// Juan Solano Brenes
        /// 21/06/2019
        /// Actualizar el salario de un funcionario
        /// </summary>
        /// <param name="fundevi"></param>
        /// <param name="salario"></param>
        /// <returns>verdadero si el ajuste de salario se guardo exitosamente</returns>
        public Boolean actualizarSalario(FuncionarioFundevi funcionario, int salario)
        {
            Boolean flag = false;
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("update Funcionario_fundevi set salario=" + salario +
                "  where id_funcionario=" + funcionario.idFuncionario, sqlConnection);
            sqlConnection.Open();
            if (sqlCommand.ExecuteNonQuery() != -1)
            {
                flag = true;
            }
            sqlConnection.Close();
            return flag;
        }


        /// <summary>
        /// Juan Solano Brenes
        /// Recibe un id de funcionario y retorna el funcionario asociado
        /// </summary>
        /// <param name="idFuncionario"></param>
        /// <returns> un funcionarioFundevi</returns>
        public FuncionarioFundevi GetFuncionario(int idFuncionario)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<FuncionarioFundevi> funcionarios = new List<FuncionarioFundevi>();
            String consulta = "select id_funcionario,id_planilla,nombre_funcionario,salario from Funcionario_fundevi where id_funcionario=" + idFuncionario;
            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();
            FuncionarioFundevi funcionario = null;
            while (reader.Read())
            {
                funcionario = new FuncionarioFundevi();
                funcionario.idFuncionario = Convert.ToInt32(reader["id_funcionario"].ToString());
                funcionario.idPlanilla = Convert.ToInt32(reader["id_planilla"].ToString());
                funcionario.nombre = reader["nombre_funcionario"].ToString();
                funcionario.salario = Convert.ToInt32(reader["salario"].ToString());

            }
            return funcionario;
        }

        /// <summary>
        /// Juan Solano Brenes
        /// 03/06/2019
        /// Insertar varios funcionarios fundevi
        /// </summary>
        /// <param name="funcionarios"></param>
        /// <param name="idPlanilla"></param>
        /// <returns>verdadero si todos los funcionarios fueron asignados a una planilla correctamente</returns>
        public Boolean InsertarFuncionarios(LinkedList<FuncionarioFundevi> funcionarios, int idPlanilla)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            Boolean flag = false;
            foreach (FuncionarioFundevi funcionario in funcionarios)
            {

                if (funcionario.idPlanilla != idPlanilla)
                {
                    funcionario.idPlanilla = idPlanilla;
                    if (InsertFuncionario(funcionario))
                    {
                        flag = true;
                    }

                }


            }
            return flag;
        }


        /// <summary>
        /// Juan Solano Brenes
        /// Borra los funcionarios fundevi de una planilla
        /// 03/06/2019
        /// </summary>
        /// <param name="idPlanilla"></param>
        /// <returns></returns>
        public Boolean EliminarFuncionarios(int idPlanilla)
        {
            Boolean flag = false;
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("delete from Funcionario_fundevi " +
                "  where id_planilla=" + idPlanilla, sqlConnection);
            sqlConnection.Open();
            if (sqlCommand.ExecuteNonQuery() != -1)
            {
                flag = true;
            }
            sqlConnection.Close();
            return flag;
        }


        /// <summary>
        /// Juan Solano Brenes
        /// 17/07/2019
        /// Borra un funcionario de una planilla
        /// </summary>
        /// <param name="funcionario"></param>
        /// <param name="idPlanilla"></param>
        /// <returns> devuelve verdadero si el funcionario es eliminado correctamente</returns>
        public Boolean EliminarFuncionario(FuncionarioFundevi funcionario, int idPlanilla)
        {
            Boolean flag = false;
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("delete from Funcionario_fundevi " +
                "  where id_planilla=" + idPlanilla + " and id_funcionario=" + funcionario.idFuncionario, sqlConnection);
            sqlConnection.Open();
            if (sqlCommand.ExecuteNonQuery() != -1)
            {
                flag = true;
            }
            sqlConnection.Close();
            return flag;
        }


        /// <summary>
        /// Juan Solano Brenes
        /// 17/07/2019
        /// Edita la información de un funcionario
        /// </summary>
        /// <param name="funcionario"></param>
        /// <returns>devuelve verdadero si la información del funcionario se ha actualizado</returns>
        public Boolean EditarFuncionario(FuncionarioFundevi funcionario)
        {
            Boolean flag = false;
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("update Funcionario_fundevi set nombre_funcionario='" + funcionario.nombre +
                    "', salario=" + funcionario.salario +
                    " where id_funcionario=" + funcionario.idFuncionario, sqlConnection);
            sqlConnection.Open();
            if (sqlCommand.ExecuteNonQuery() != -1)
            {
                flag = true;
            }
            sqlConnection.Close();
            return flag;
        }


    }
}