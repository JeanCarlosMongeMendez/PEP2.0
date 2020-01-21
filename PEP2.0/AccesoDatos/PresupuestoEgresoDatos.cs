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
    /// Clase para administrar las consultas sql del presupuesto de egreso
    /// </summary>
    public class PresupuestoEgresoDatos
    {

        private ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Inserta un nuevo presupuesto de egreso
        /// </summary>
        /// <param name="presupuestoEgreso">Presupuesto de Egreso</param>
        public int InsertarPresupuestoEgreso(PresupuestoEgreso presupuestoEgreso)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            int idPresupuestoEgreso = 0;
            sqlConnection.Open();

            try
            {
                SqlCommand sqlCommand = new SqlCommand("insert into Presupuesto_Egreso(id_unidad,  plan_estrategico_operacional, montoTotal) " +
                        "output INSERTED.id_presupuesto_egreso values(@id_unidad_, @plan_estrategico_operacional_, @montoTotal_);", sqlConnection);
                //El estado por defecto es false=Pendiente, mas tarde se cambiara a Aprobado
                sqlCommand.Parameters.AddWithValue("@id_unidad_", presupuestoEgreso.unidad.idUnidad);
                sqlCommand.Parameters.AddWithValue("@plan_estrategico_operacional_", presupuestoEgreso.planEstrategicoOperacional);
                sqlCommand.Parameters.AddWithValue("@montoTotal_", presupuestoEgreso.montoTotal);

                idPresupuestoEgreso = (int)sqlCommand.ExecuteScalar();

            }
            catch (SqlException ex)
            {
                //Utilidades.ErrorBitacora(ex.Message, "Error al insertar el periodo");
            }

            sqlConnection.Close();

            return idPresupuestoEgreso;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: actualiza dado de plan estrategico del presupuesto de egreso 
        /// Requiere: presupuesto de egreso a modificar
        /// Modifica: dato de plan estrategico del presupuesto egreso
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoEgreso"></param>
        /// <returns></returns>
        public void actualizarPlanEstrategicoPresupuestoEgreso(PresupuestoEgreso presupuestoEgreso)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand(@"update Presupuesto_Egreso set plan_estrategico_operacional=@planEstrategicoOperacional where id_presupuesto_egreso = @idPresupuestoEgreso", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@planEstrategicoOperacional", presupuestoEgreso.planEstrategicoOperacional);
            sqlCommand.Parameters.AddWithValue("@idPresupuestoEgreso", presupuestoEgreso.idPresupuestoEgreso);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 08/oct/2019
        /// Efecto: devuelve los presupuestos de egresos de la unidad seleccionada
        /// Requiere: unidad
        /// Modifica: -
        /// Devuelve: lista de presupuestos de egresos
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public List<PresupuestoEgreso> getPresupuestosEgresosPorUnidad(Unidad unidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<PresupuestoEgreso> presupuestoEgresos = new List<PresupuestoEgreso>();

            SqlCommand sqlCommand = new SqlCommand(@"select PE.*
                                                                                            from Presupuesto_Egreso PE
                                                                                            where PE.id_unidad = @idUnidad ", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idUnidad", unidad.idUnidad);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                PresupuestoEgreso presupuestoEgreso = new PresupuestoEgreso();
                Unidad unidadBD = new Unidad();

                unidadBD.idUnidad = Convert.ToInt32(reader["id_unidad"].ToString());

                presupuestoEgreso.unidad = unidadBD;
                presupuestoEgreso.idPresupuestoEgreso = Convert.ToInt32(reader["id_presupuesto_egreso"].ToString());
                presupuestoEgreso.planEstrategicoOperacional = reader["plan_estrategico_operacional"].ToString();
                presupuestoEgreso.montoTotal = Convert.ToDouble(reader["montoTotal"].ToString());
                
                presupuestoEgresos.Add(presupuestoEgreso);
            }

            sqlConnection.Close();

            return presupuestoEgresos;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/oct/2019
        /// Efecto: actualiza el monto total del presupuesto de egresos
        /// Requiere: presupuesto de egresos
        /// Modifica: monto total
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoEgreso"></param>
        public void actualizarMontoPresupuestoEgreso(PresupuestoEgreso presupuestoEgreso)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand(@"update Presupuesto_Egreso set montoTotal=@montoTotal where id_presupuesto_egreso = @idPresupuestoEgreso", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@montoTotal", presupuestoEgreso.montoTotal);
            sqlCommand.Parameters.AddWithValue("@idPresupuestoEgreso", presupuestoEgreso.idPresupuestoEgreso);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/oct/2019
        /// Efecto: devuelve el presupuesto de egreso segun el id consultado
        /// Requiere: presupuesto de egreso
        /// Modifica: -
        /// Devuelve: presupuesto de egreso
        /// </summary>
        /// <param name="presupuestoEgresoConsulta"></param>
        /// <returns></returns>
        public PresupuestoEgreso getPresupuestosEgresosPorId(PresupuestoEgreso presupuestoEgresoConsulta)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            PresupuestoEgreso presupuestoEgreso = new PresupuestoEgreso();

            SqlCommand sqlCommand = new SqlCommand(@"select PE.*
                                                                                            from Presupuesto_Egreso PE
                                                                                            where PE.id_presupuesto_egreso = @idPresupuestoEgreso ", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idPresupuestoEgreso", presupuestoEgresoConsulta.idPresupuestoEgreso);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                Unidad unidadBD = new Unidad();

                unidadBD.idUnidad = Convert.ToInt32(reader["id_unidad"].ToString());

                presupuestoEgreso.unidad = unidadBD;
                presupuestoEgreso.idPresupuestoEgreso = Convert.ToInt32(reader["id_presupuesto_egreso"].ToString());
                presupuestoEgreso.planEstrategicoOperacional = reader["plan_estrategico_operacional"].ToString();
                presupuestoEgreso.montoTotal = Convert.ToDouble(reader["montoTotal"].ToString());
            }

            sqlConnection.Close();

            return presupuestoEgreso;
        }
      

    }
}
