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
    /// 22/may/2019
    /// Clase para administrar el CRUD para los presupuestos
    /// </summary>
    public class PresupuestoDatos
    {
        private ConexionDatos conexion = new ConexionDatos();

        #region PRESUPUESTO DE INGRESO

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
                SqlCommand sqlCommand = new SqlCommand("insert into Presupuesto_Ingreso(estado, monto, es_inicial, id_proyecto) " +
                    "output INSERTED.id_presupuesto_ingreso values(@estado_, @monto_, @es_inicial_, @id_proyecto_);", sqlConnection);
                //El estado por defecto es false=Guardado, mas tarde se cambiara a Aprobado
                sqlCommand.Parameters.AddWithValue("@estado_", presupuestoIngreso.estado);
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
        /// Aprueba el presupuesto dado
        /// </summary>
        /// <param name="idPresupuesto">Id del Presupuesto</param>
        public int AprobarPresupuestoIngreso(int idPresupuesto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            int respuesta = 0;
            sqlConnection.Open();

            try
            {
                SqlCommand sqlCommand = new SqlCommand("update Presupuesto_Ingreso set estado=1 output INSERTED.id_presupuesto_ingreso where id_presupuesto_ingreso=@id_presupuesto_ingreso_;", sqlConnection);
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
        /// Obtiene los presupuestos por proyectos
        /// </summary>
        /// <returns>Retorna una lista <code>LinkedList<PresupuestoIngreso></code> que contiene todos los presupuestos segun el proyecto</returns>
        public LinkedList<PresupuestoIngreso> ObtenerPorProyecto(int idProyecto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            LinkedList<PresupuestoIngreso> presupuestoIngresos = new LinkedList<PresupuestoIngreso>();

            SqlCommand sqlCommand = new SqlCommand("SELECT id_presupuesto_ingreso, estado, monto, es_inicial, id_proyecto FROM Presupuesto_Ingreso where id_proyecto=@id_proyecto_;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_proyecto_", idProyecto);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                PresupuestoIngreso presupuestoIngreso = new PresupuestoIngreso();
                presupuestoIngreso.idPresupuestoIngreso = Convert.ToInt32(reader["id_presupuesto_ingreso"].ToString());
                presupuestoIngreso.estado = Convert.ToBoolean(reader["estado"].ToString());
                presupuestoIngreso.monto = Convert.ToDouble(reader["monto"].ToString());
                presupuestoIngreso.esInicial = Convert.ToBoolean(reader["es_inicial"].ToString());

                presupuestoIngreso.proyecto = new Proyectos();
                presupuestoIngreso.proyecto.idProyecto = Convert.ToInt32(reader["id_proyecto"].ToString());

                presupuestoIngresos.AddLast(presupuestoIngreso);
            }

            sqlConnection.Close();

            return presupuestoIngresos;
        }

        /// <summary>
        /// Obtiene el presupuesto por ID
        /// </summary>
        /// <returns>Retorna un presupuesto de ingreso</returns>
        public PresupuestoIngreso ObtenerPorId(int idPresupuesto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand("SELECT id_presupuesto_ingreso, estado, monto, es_inicial, id_proyecto FROM Presupuesto_Ingreso where id_presupuesto_ingreso=@id_presupuesto_ingreso_;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_presupuesto_ingreso_", idPresupuesto);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            PresupuestoIngreso presupuestoIngreso = new PresupuestoIngreso();

            if (reader.Read())
            {
                presupuestoIngreso.idPresupuestoIngreso = Convert.ToInt32(reader["id_presupuesto_ingreso"].ToString());
                presupuestoIngreso.estado = Convert.ToBoolean(reader["estado"].ToString());
                presupuestoIngreso.monto = Convert.ToDouble(reader["monto"].ToString());
                presupuestoIngreso.esInicial = Convert.ToBoolean(reader["es_inicial"].ToString());

                presupuestoIngreso.proyecto = new Proyectos();
                presupuestoIngreso.proyecto.idProyecto = Convert.ToInt32(reader["id_proyecto"].ToString());
            }

            sqlConnection.Close();

            return presupuestoIngreso;
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

        #endregion PRESUPUESTO DE INGRESO

        #region PRESUPUESTO DE EGRESO

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
                SqlCommand sqlCommand = new SqlCommand("insert into Presupuesto_Egreso(id_unidad, estado, descripcion_estado, plan_estrategico_operacional, montoTotal) " +
                        "output INSERTED.id_presupuesto_egreso values(@id_unidad_, @estado_, @descripcion_estado_, @plan_estrategico_operacional_, @montoTotal_);", sqlConnection);
                //El estado por defecto es false=Pendiente, mas tarde se cambiara a Aprobado
                sqlCommand.Parameters.AddWithValue("@id_unidad_", presupuestoEgreso.idUnidad);
                sqlCommand.Parameters.AddWithValue("@estado_", false);
                sqlCommand.Parameters.AddWithValue("@descripcion_estado_", "");
                sqlCommand.Parameters.AddWithValue("@plan_estrategico_operacional_", presupuestoEgreso.planEstrategicoOperacional);
                sqlCommand.Parameters.AddWithValue("@montoTotal_", presupuestoEgreso.montoTotal);

                idPresupuestoEgreso = (int)sqlCommand.ExecuteScalar();
                InsertarPresupuestoEgresoPartida(idPresupuestoEgreso, presupuestoEgreso.presupuestoEgresoPartidas);
            }
            catch (SqlException ex)
            {
                //Utilidades.ErrorBitacora(ex.Message, "Error al insertar el periodo");
            }

            sqlConnection.Close();

            return idPresupuestoEgreso;
        }

        /// <summary>
        /// Asocia cada una de las partidas con su respectivo monto
        /// </summary>
        /// <param name="presupuestoEgresoPartida">Presupuesto de egreso de partidas relacionadas con el presupuesto de egreso</param>
        private void InsertarPresupuestoEgresoPartida(int idPresupuestoEgreso, LinkedList<PresupuestoEgresoPartida> presupuestoEgresoPartidas)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            sqlConnection.Open();

            try
            {
                if (idPresupuestoEgreso > 0)
                {
                    foreach (PresupuestoEgresoPartida presupuestoEgresoPartida in presupuestoEgresoPartidas)
                    {
                        SqlCommand sqlCommand = new SqlCommand("insert into Presupuesto_Egreso_Partida(id_presupuesto_egreso, id_partida, monto, descripcion) " +
                        "values(@id_presupuesto_egreso_, @id_partida_, @monto_, @descripcion_);", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id_presupuesto_egreso_", idPresupuestoEgreso);
                        sqlCommand.Parameters.AddWithValue("@id_partida_", presupuestoEgresoPartida.idPartida);
                        sqlCommand.Parameters.AddWithValue("@monto_", presupuestoEgresoPartida.monto);
                        sqlCommand.Parameters.AddWithValue("@descripcion_", presupuestoEgresoPartida.descripcion);

                        sqlCommand.ExecuteScalar();
                    }
                }
            }
            catch (SqlException ex)
            {
                //Utilidades.ErrorBitacora(ex.Message, "Error al insertar el periodo");
            }

            sqlConnection.Close();
        }

        public int AprobarPresupuestoEgreso(int idPresupuestoEgreso)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            sqlConnection.Open();
            int respuesta = 0;

            try
            {
                SqlCommand sqlCommand = new SqlCommand("update Presupuesto_Egreso set estado=1 output INSERTED.id_presupuesto_egreso " +
                    "where id_presupuesto_egreso=@id_presupuesto_egreso_;", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id_presupuesto_egreso_", idPresupuestoEgreso);

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
        /// Obtiene los presupuesto por unidad
        /// </summary>
        /// <returns>Retorna varios presupuesto de egreso</returns>
        public LinkedList<PresupuestoEgreso> ObtenerPorUnidad(int idUnidad)
        {
            SqlConnection sqlConnectionPresupuestoEgreso = conexion.conexionPEP();
            SqlConnection sqlConnectionPresupuestoEgresoPartida = conexion.conexionPEP();

            SqlCommand sqlCommandPresupuestoEgreso = new SqlCommand("SELECT id_presupuesto_egreso, id_unidad, estado, montoTotal, descripcion_estado, plan_estrategico_operacional FROM Presupuesto_Egreso where id_unidad=@id_unidad_;", sqlConnectionPresupuestoEgreso);
            sqlCommandPresupuestoEgreso.Parameters.AddWithValue("@id_unidad_", idUnidad);

            SqlDataReader readerPresupuestoEgreso;
            sqlConnectionPresupuestoEgreso.Open();
            readerPresupuestoEgreso = sqlCommandPresupuestoEgreso.ExecuteReader();

            LinkedList<PresupuestoEgreso> presupuestoEgresos = new LinkedList<PresupuestoEgreso>();

            while (readerPresupuestoEgreso.Read())
            {
                PresupuestoEgreso presupuestoEgreso = new PresupuestoEgreso();

                presupuestoEgreso.idPresupuestoEgreso = Convert.ToInt32(readerPresupuestoEgreso["id_presupuesto_egreso"].ToString());
                presupuestoEgreso.idUnidad = Convert.ToInt32(readerPresupuestoEgreso["id_unidad"].ToString());
                presupuestoEgreso.estado = Convert.ToBoolean(readerPresupuestoEgreso["estado"].ToString());
                presupuestoEgreso.descripcionEstado = readerPresupuestoEgreso["descripcion_estado"].ToString();
                presupuestoEgreso.planEstrategicoOperacional = readerPresupuestoEgreso["plan_estrategico_operacional"].ToString();
                presupuestoEgreso.montoTotal = Convert.ToDouble(readerPresupuestoEgreso["montoTotal"].ToString());

                presupuestoEgreso.presupuestoEgresoPartidas = new LinkedList<PresupuestoEgresoPartida>();
                SqlCommand sqlCommandPresupuestoEgresoPartidas = new SqlCommand("SELECT id_presupuesto_egreso, id_partida, monto, descripcion FROM Presupuesto_Egreso_Partida where id_presupuesto_egreso=@id_presupuesto_egreso_;", sqlConnectionPresupuestoEgresoPartida);
                sqlCommandPresupuestoEgresoPartidas.Parameters.AddWithValue("@id_presupuesto_egreso_", presupuestoEgreso.idPresupuestoEgreso);

                SqlDataReader readerPresupuestoEgresoPartidas;
                sqlConnectionPresupuestoEgresoPartida.Open();
                readerPresupuestoEgresoPartidas = sqlCommandPresupuestoEgresoPartidas.ExecuteReader();

                while (readerPresupuestoEgresoPartidas.Read())
                {
                    PresupuestoEgresoPartida presupuestoEgresoPartida = new PresupuestoEgresoPartida();
                    presupuestoEgresoPartida.idPresupuestoEgreso = Convert.ToInt32(readerPresupuestoEgresoPartidas["id_presupuesto_egreso"].ToString());
                    presupuestoEgresoPartida.idPartida = Convert.ToInt32(readerPresupuestoEgresoPartidas["id_partida"].ToString());
                    presupuestoEgresoPartida.monto = Convert.ToDouble(readerPresupuestoEgresoPartidas["monto"].ToString());
                    presupuestoEgresoPartida.descripcion = readerPresupuestoEgresoPartidas["descripcion"].ToString();

                    presupuestoEgreso.presupuestoEgresoPartidas.AddLast(presupuestoEgresoPartida);
                }

                presupuestoEgresos.AddLast(presupuestoEgreso);
                sqlConnectionPresupuestoEgresoPartida.Close();
            }

            sqlConnectionPresupuestoEgreso.Close();

            return presupuestoEgresos;
        }

        #endregion PRESUPUESTO DE EGRESO
    }
}
