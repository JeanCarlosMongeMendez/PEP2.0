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
    /// 04/nov/2019
    /// Clase para administrar las consultas sql de la entidad de Proyeccion
    /// </summary>
    public class ProyeccionDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: obtiene todas las proyeccion de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de proyecciones
        /// </summary>
        /// <returns></returns>
        public List<Proyeccion> getProyecciones()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Proyeccion> listaProyecciones = new List<Proyeccion>();

            String consulta = @"select P.*, F.nombre_funcionario, M.nombre_mes, M.numero
                                            from Proyeccion P, Funcionario F, Mes M
                                            where F.id_funcionario = P.id_funcionario and M.id_mes = P.id_mes
                                            order by F.id_funcionario, M.numero ;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Proyeccion proyeccion = new Proyeccion();
                Periodo periodo = new Periodo();
                Mes mes = new Mes();
                Funcionario funcionario = new Funcionario();

                periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());
                proyeccion.periodo = periodo;

                mes.idMes = Convert.ToInt32(reader["id_mes"].ToString());
                mes.nombreMes = reader["nombre_mes"].ToString();
                mes.numero = Convert.ToInt32(reader["numero"].ToString());

                funcionario.idFuncionario = Convert.ToInt32(reader["id_funcionario"].ToString());
                funcionario.nombreFuncionario = reader["nombre_funcionario"].ToString();

                proyeccion.idProyeccion = Convert.ToInt32(reader["id_proyeccion"].ToString());
                proyeccion.funcionario = funcionario;
                proyeccion.mes = mes;
                proyeccion.montoSalario = Convert.ToDouble(reader["monto_salario"].ToString());
                proyeccion.montoCargasTotal = Convert.ToDouble(reader["monto_cargas_total"].ToString());

                listaProyecciones.Add(proyeccion);
            }

            sqlConnection.Close();

            return listaProyecciones;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: inserta en la base de datos una proyeccion
        /// Requiere: proyeccion
        /// Modifica: -
        /// Devuelve: id de la proyeccion
        /// </summary>
        /// <param name="proyeccion"></param>
        /// <returns></returns>
        public int insertarProyeccion(Proyeccion proyeccion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"Insert Proyeccion(id_funcionario,id_mes,monto_salario,monto_cargas_total,ano_periodo)
                                            values(@idfuncionario,@idMes,@montoSalario,@montoCargasTotal,@anoPeriodo);SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idfuncionario", proyeccion.funcionario.idFuncionario);
            command.Parameters.AddWithValue("@idMes", proyeccion.mes.idMes);
            command.Parameters.AddWithValue("@montoSalario", proyeccion.montoSalario);
            command.Parameters.AddWithValue("@montoCargasTotal", proyeccion.montoCargasTotal);
            command.Parameters.AddWithValue("@anoPeriodo", proyeccion.periodo.anoPeriodo);

            sqlConnection.Open();
            int idProyeccion = Convert.ToInt32(command.ExecuteScalar());
            sqlConnection.Close();

            return idProyeccion;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: actualiza la proyeccion
        /// Requiere: proyeccion
        /// Modifica: la proyeccion que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="proyeccion"></param>
        public void actualizarProyeccion(Proyeccion proyeccion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update Proyeccion set id_funcionario = @idFuncionario, id_mes = @idMes, monto_salario = @montoSalario, monto_cargas_total = @montoCargasTotal, ano_periodo = @anoPeriodo
                                            where id_proyeccion = @idProyeccion";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idProyeccion", proyeccion.idProyeccion);
            command.Parameters.AddWithValue("@idFuncionario", proyeccion.funcionario.idFuncionario);
            command.Parameters.AddWithValue("@idMes", proyeccion.mes.idMes);
            command.Parameters.AddWithValue("@montoSalario", proyeccion.montoSalario);
            command.Parameters.AddWithValue("@montoCargasTotal", proyeccion.montoCargasTotal);
            command.Parameters.AddWithValue("@anoPeriodo", proyeccion.periodo.anoPeriodo);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: elimina la proyeccion
        /// Requiere: proyeccion
        /// Modifica: base de datos eliminando la proyeccion
        /// Devuelve: -
        /// </summary>
        /// <param name="proyeccion"></param>
        public void eliminarProyeccion(Proyeccion proyeccion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete from Proyeccion where 
                                            id_proyeccion = @idProyeccion";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idProyeccion", proyeccion.idProyeccion);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: elimina las proyecciones que se encuentran en el periodo seleccionado  y del funcionario seleccionado
        /// Requiere: periodo y funcionario
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="periodo"></param>
        public void eliminarProyeccionPorPeriodo(Periodo periodo, Funcionario funcionario)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete from Proyeccion where 
                                            ano_periodo = @anoPeriodo and id_funcionario = @idFuncionario";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@anoPeriodo", periodo.anoPeriodo);
            command.Parameters.AddWithValue("@idFuncionario", funcionario.idFuncionario);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: obtiene todas las proyeccion de la base de datos del periodo y funcionario consultado
        /// Requiere: periodo y funcionario
        /// Modifica: -
        /// Devuelve: lista de proyecciones
        /// </summary>
        /// <returns></returns>
        public List<Proyeccion> getProyeccionesPorPeriodoYFuncionario(Periodo periodoConsulta, Funcionario funcionarioConsulta)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Proyeccion> listaProyecciones = new List<Proyeccion>();

            String consulta = @"select P.*, F.nombre_funcionario, M.nombre_mes, M.numero
                                            from Proyeccion P, Funcionario F, Mes M
                                            where P.ano_periodo = @anoPeriodo and P.id_funcionario = @idFuncionario and F.id_funionario = P.id_funcionario and M.id_mes = P.id_mes
                                            order by F.id_funionario, M.numero ;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@anoPeriodo", periodoConsulta.anoPeriodo);
            sqlCommand.Parameters.AddWithValue("@idFuncionario", funcionarioConsulta.idFuncionario);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Proyeccion proyeccion = new Proyeccion();
                Periodo periodo = new Periodo();
                Mes mes = new Mes();
                Funcionario funcionario = new Funcionario();

                periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());
                proyeccion.periodo = periodo;

                mes.idMes = Convert.ToInt32(reader["id_mes"].ToString());
                mes.nombreMes = reader["nombre_mes"].ToString();
                mes.numero = Convert.ToInt32(reader["numero"].ToString());

                funcionario.idFuncionario = Convert.ToInt32(reader["id_funcionario"].ToString());
                funcionario.nombreFuncionario = reader["nombre_funcionario"].ToString();

                proyeccion.idProyeccion = Convert.ToInt32(reader["id_proyeccion"].ToString());
                proyeccion.funcionario = funcionario;
                proyeccion.mes = mes;
                proyeccion.montoSalario = Convert.ToDouble(reader["monto_salario"].ToString());
                proyeccion.montoCargasTotal = Convert.ToDouble(reader["monto_cargas_total"].ToString());

                listaProyecciones.Add(proyeccion);
            }

            sqlConnection.Close();

            return listaProyecciones;
        }
    }
}
