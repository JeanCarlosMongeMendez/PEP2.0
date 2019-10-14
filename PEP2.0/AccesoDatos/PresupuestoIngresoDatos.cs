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
    /// 08/oct/2019
    /// Clase para administrar las consultas sql del presupuesto de ingreso
    /// </summary>
    public class PresupuestoIngresoDatos
    {
        private ConexionDatos conexion = new ConexionDatos(); 

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: devuelve lista de presupuestos de ingresos segun el proyecto ingresado
        /// Requiere: proyecto a consultar
        /// Modifica: -
        /// Devuelve: lista de presupuestos de ingresos
        /// </summary>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<PresupuestoIngreso> getPresupuestosIngresosPorProyecto(Proyectos proyecto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<PresupuestoIngreso> presupuestoIngresos = new List<PresupuestoIngreso>();

            SqlCommand sqlCommand = new SqlCommand("SELECT PI.id_presupuesto_ingreso, PI.id_estado_presup_ingreso, PI.monto, PI.es_inicial, PI.id_proyecto, EPI.desc_estado FROM Presupuesto_Ingreso PI, Estado_presup_ingreso EPI where id_proyecto=@id_proyecto_ and EPI.id_estado_presup_ingreso = PI.id_estado_presup_ingreso;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_proyecto_", proyecto.idProyecto);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                PresupuestoIngreso presupuestoIngreso = new PresupuestoIngreso();
                EstadoPresupIngreso estadoPresupIngreso = new EstadoPresupIngreso();
                estadoPresupIngreso.idEstadoPresupIngreso = Convert.ToInt32(reader["id_estado_presup_ingreso"].ToString());
                estadoPresupIngreso.descEstado = reader["desc_estado"].ToString();

                presupuestoIngreso.idPresupuestoIngreso = Convert.ToInt32(reader["id_presupuesto_ingreso"].ToString());
                presupuestoIngreso.estadoPresupIngreso = estadoPresupIngreso;
                presupuestoIngreso.monto = Convert.ToDouble(reader["monto"].ToString());
                presupuestoIngreso.esInicial = Convert.ToBoolean(reader["es_inicial"].ToString());

                presupuestoIngreso.proyecto = new Proyectos();
                presupuestoIngreso.proyecto.idProyecto = Convert.ToInt32(reader["id_proyecto"].ToString());

                presupuestoIngresos.Add(presupuestoIngreso);
            }

            sqlConnection.Close();

            return presupuestoIngresos;
        }

        /// <summary>
        /// Inserta el presupuesto de ingreso
        /// </summary>
        /// <param name="presupuestoIngreso">Presupuesto de ingreso a insertar</param>
        public int InsertarPresupuestoIngreso(PresupuestoIngreso presupuestoIngreso)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            int respuesta = 0;
            sqlConnection.Open();

            try
            {
                SqlCommand sqlCommand = new SqlCommand("insert into Presupuesto_Ingreso(id_estado_presup_ingreso, monto, es_inicial, id_proyecto) " +
                    "output INSERTED.id_presupuesto_ingreso values(@idEstadoPresupIngreso, @monto_, @es_inicial_, @id_proyecto_);", sqlConnection);
                //El estado por defecto es false=Guardado, mas tarde se cambiara a Aprobado
                sqlCommand.Parameters.AddWithValue("@idEstadoPresupIngreso", presupuestoIngreso.estadoPresupIngreso.idEstadoPresupIngreso);
                sqlCommand.Parameters.AddWithValue("@monto_", presupuestoIngreso.monto);
                sqlCommand.Parameters.AddWithValue("@es_inicial_", presupuestoIngreso.esInicial);
                sqlCommand.Parameters.AddWithValue("@id_proyecto_", presupuestoIngreso.proyecto.idProyecto);

                respuesta = (int)sqlCommand.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                //Utilidades.ErrorBitacora(ex.Message, "Error al insertar el periodo");
            }

            sqlConnection.Close();

            return respuesta;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/oct/2019
        /// Efecto: actualiza dado de monto del presupuesto de ingreso 
        /// Requiere: presupuesto de ingreso a modificar
        /// Modifica: dato de monto del presupuesto ingreso
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoIngreso"></param>
        /// <returns></returns>
        public void actualizarPresupuestoIngreso(PresupuestoIngreso presupuestoIngreso)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand(@"update Presupuesto_Ingreso set monto=@monto where id_presupuesto_ingreso = @idPresupuestoIngreso", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@monto", presupuestoIngreso.monto);
            sqlCommand.Parameters.AddWithValue("@idPresupuestoIngreso", presupuestoIngreso.idPresupuestoIngreso);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Eliminar un presupuesto de ingreso
        /// </summary>
        /// <param name="idPresupuesto">Id del presupuesto a eliminar</param>
        public int EliminarPresupuestoIngreso(int idPresupuesto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            sqlConnection.Open();
            int respuesta = 0;

            try
            {
                SqlCommand sqlCommand = new SqlCommand("delete from Presupuesto_Ingreso output deleted.id_presupuesto_ingreso where id_presupuesto_ingreso=@id_presupuesto_ingreso_;", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id_presupuesto_ingreso_", idPresupuesto);

                respuesta = (int)sqlCommand.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                //Utilidades.ErrorBitacora(ex.Message, "");
            }

            sqlConnection.Close();

            return respuesta;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 02/oct/2019
        /// Efecto: actualiza dado de estado del presupuesto de ingreso 
        /// Requiere: presupuesto de ingreso a modificar
        /// Modifica: dato de estado del presupuesto ingreso
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoIngreso"></param>
        /// <returns></returns>
        public void actualizarEstadoPresupuestoIngreso(PresupuestoIngreso presupuestoIngreso)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand(@"update Presupuesto_Ingreso set id_estado_presup_ingreso=@idEstadoPresupIngreso where id_presupuesto_ingreso = @idPresupuestoIngreso", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idEstadoPresupIngreso", presupuestoIngreso.estadoPresupIngreso.idEstadoPresupIngreso);
            sqlCommand.Parameters.AddWithValue("@idPresupuestoIngreso", presupuestoIngreso.idPresupuestoIngreso);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();
            sqlConnection.Close();
        }
    }
}
