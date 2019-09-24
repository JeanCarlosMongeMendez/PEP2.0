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
                SqlCommand sqlCommand = new SqlCommand("insert into Presupuesto_Egreso(id_unidad, estado,  plan_estrategico_operacional, montoTotal) " +
                        "output INSERTED.id_presupuesto_egreso values(@id_unidad_, @plan_estrategico_operacional_, @montoTotal_);", sqlConnection);
                //El estado por defecto es false=Pendiente, mas tarde se cambiara a Aprobado
                sqlCommand.Parameters.AddWithValue("@id_unidad_", presupuestoEgreso.idUnidad);

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
        /// Asocia cada una de las partidas con su respectivo monto
        /// </summary>
        /// <param name="presupuestoEgresoPartida">Presupuesto de egreso de partidas relacionadas con el presupuesto de egreso</param>
        public void InsertarPresupuestoEgresoPartida(PresupuestoEgresoPartida presupuestoEgresoP)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            sqlConnection.Open();

            try
            {

                SqlCommand sqlCommand = new SqlCommand("insert into Presupuesto_Egreso_Partida(id_presupuesto_egreso, id_partida, monto, descripcion) " +
                "values(@id_presupuesto_egreso_, @id_partida_, @monto_, @descripcion_);", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id_presupuesto_egreso_", presupuestoEgresoP.idPresupuestoEgreso);
                sqlCommand.Parameters.AddWithValue("@id_partida_", presupuestoEgresoP.idPartida);
                sqlCommand.Parameters.AddWithValue("@monto_", presupuestoEgresoP.monto);
                sqlCommand.Parameters.AddWithValue("@descripcion_", presupuestoEgresoP.descripcion);

                sqlCommand.ExecuteScalar();


            }
            catch (SqlException ex)
            {
                //Utilidades.ErrorBitacora(ex.Message, "Error al insertar el periodo");
            }

            sqlConnection.Close();
        }

        /// <summary>
        /// Josseline M
        /// Filtra los presupuestos egresos de acuerdo con el numero de partida 
        /// </summary>
        /// <param name="presupuestoEgresoPartidaF"></param>
        /// <returns></returns>
        public LinkedList<PresupuestoEgresoPartida> presupuestoEgresoPartidasPorPresupuesto(PresupuestoEgresoPartida presupuestoEgresoPartidaF)
        {
            SqlConnection sqlConnectionPresupuestoEgreso = conexion.conexionPEP();
            SqlConnection sqlConnectionPresupuestoEgresoPartida = conexion.conexionPEP();

            SqlCommand sqlCommandPresupuestoEgreso = new SqlCommand("SELECT id_presupuesto_egreso,id_partida,monto, descripcion FROM Presupuesto_Egreso_Partida Where id_presupuesto_egreso = @id_partida; ", sqlConnectionPresupuestoEgreso);

            sqlCommandPresupuestoEgreso.Parameters.AddWithValue("@id_partida", presupuestoEgresoPartidaF.idPartida);

            SqlDataReader readerPresupuestoEgreso;
            sqlConnectionPresupuestoEgreso.Open();
            readerPresupuestoEgreso = sqlCommandPresupuestoEgreso.ExecuteReader();

            LinkedList<PresupuestoEgresoPartida> presupuestoEgresosPartidaL = new LinkedList<PresupuestoEgresoPartida>();

            while (readerPresupuestoEgreso.Read())
            {

                PresupuestoEgresoPartida presupuestoEgresoPartida = new PresupuestoEgresoPartida();
                presupuestoEgresoPartida.idPresupuestoEgreso = Convert.ToInt32(readerPresupuestoEgreso["id_presupuesto_egreso"].ToString());
                presupuestoEgresoPartida.idPartida = Convert.ToInt32(readerPresupuestoEgreso["id_partida"].ToString());
                presupuestoEgresoPartida.monto = Convert.ToDouble(readerPresupuestoEgreso["monto"].ToString());
                presupuestoEgresoPartida.descripcion= readerPresupuestoEgreso["descripcion"].ToString();
                presupuestoEgresosPartidaL.AddLast(presupuestoEgresoPartida);
            }


            sqlConnectionPresupuestoEgresoPartida.Close();


            return presupuestoEgresosPartidaL;
        }
       /*
        /// <summary>
        /// Verifica si el monto total de las partidas de egresos es menor a la asignada al presupuesto
        /// </summary>
        /// <param name="idPresupuestoEgreso"></param>
        /// <returns></returns>
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
        }*/

       
        /// <summary>
        /// Josseline M 
        /// Este metodo retorna una lista de presuestos egresos destinados por unidad
        /// </summary>
        /// <param name="idUnidad"></param>
        /// <returns></returns>
        public LinkedList<PresupuestoEgreso> ObtenerPorUnidadEgresos(int idUnidad)
        {
            SqlConnection sqlConnectionPresupuestoEgreso = conexion.conexionPEP();
            SqlConnection sqlConnectionPresupuestoEgresoPartida = conexion.conexionPEP();

            SqlCommand sqlCommandPresupuestoEgreso = new SqlCommand("SELECT id_presupuesto_egreso, id_unidad, montoTotal, plan_estrategico_operacional FROM Presupuesto_Egreso where id_unidad=@id_unidad_;", sqlConnectionPresupuestoEgreso);
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
                presupuestoEgreso.planEstrategicoOperacional = readerPresupuestoEgreso["plan_estrategico_operacional"].ToString();
                presupuestoEgreso.montoTotal = Convert.ToDouble(readerPresupuestoEgreso["montoTotal"].ToString());

                presupuestoEgreso.presupuestoEgresoPartidas = new LinkedList<PresupuestoEgresoPartida>();
                SqlCommand sqlCommandPresupuestoEgresoPartidas = new SqlCommand("SELECT id_presupuesto_egreso, id_partida, monto FROM Presupuesto_Egreso_Partida where id_presupuesto_egreso=@id_presupuesto_egreso_;", sqlConnectionPresupuestoEgresoPartida);
                sqlCommandPresupuestoEgresoPartidas.Parameters.AddWithValue("@id_presupuesto_egreso_", presupuestoEgreso.idPresupuestoEgreso);


                presupuestoEgresos.AddLast(presupuestoEgreso);


            }
            sqlConnectionPresupuestoEgreso.Close();

            return presupuestoEgresos;
        }

        /// <summary>
        /// Obtiene los presupuesto por unidad
        /// </summary>
        /// <returns>Retorna varios presupuesto de egreso</returns>
        public LinkedList<PresupuestoEgreso> ObtenerPorUnidad(int idUnidad)
        {
            SqlConnection sqlConnectionPresupuestoEgreso = conexion.conexionPEP();
            SqlConnection sqlConnectionPresupuestoEgresoPartida = conexion.conexionPEP();

            SqlCommand sqlCommandPresupuestoEgreso = new SqlCommand("SELECT id_presupuesto_egreso, id_unidad, montoTotal, plan_estrategico_operacional FROM Presupuesto_Egreso where id_unidad=@id_unidad_;", sqlConnectionPresupuestoEgreso);
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
                presupuestoEgreso.planEstrategicoOperacional = readerPresupuestoEgreso["plan_estrategico_operacional"].ToString();
                presupuestoEgreso.montoTotal = Convert.ToDouble(readerPresupuestoEgreso["montoTotal"].ToString());

                presupuestoEgreso.presupuestoEgresoPartidas = new LinkedList<PresupuestoEgresoPartida>();
                SqlCommand sqlCommandPresupuestoEgresoPartidas = new SqlCommand("SELECT id_presupuesto_egreso, id_partida, monto FROM Presupuesto_Egreso_Partida where id_presupuesto_egreso=@id_presupuesto_egreso_;", sqlConnectionPresupuestoEgresoPartida);
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
                    
                    presupuestoEgreso.presupuestoEgresoPartidas.AddLast(presupuestoEgresoPartida);
                }

                presupuestoEgresos.AddLast(presupuestoEgreso);
                sqlConnectionPresupuestoEgresoPartida.Close();
            }

            sqlConnectionPresupuestoEgreso.Close();

            return presupuestoEgresos;
        }
       
        /// <summary>
        /// Retorna el presupuesto de los proyecto por proyecto y por unidad
        /// </summary>
        /// <param name="idProyecto"></param>
        /// <returns></returns>
        public LinkedList<PresupuestoEgreso> ObtenerPresupuestoPorProyecto(int idUnidad,int idProyecto)
        {
            SqlConnection sqlConnectionPresupuestoEgreso = conexion.conexionPEP();
            SqlConnection sqlConnectionPresupuestoEgresoPartida = conexion.conexionPEP();

            SqlCommand sqlCommandPresupuestoEgreso = new SqlCommand("SELECT id_presupuesto_egreso, Presupuesto_Egreso.id_unidad, montoTotal, plan_estrategico_operacional FROM Presupuesto_Egreso, Unidad where Presupuesto_Egreso.id_unidad=@id_unidad_ and Unidad.id_proyecto=@id_proyecto_; ", sqlConnectionPresupuestoEgreso);
            
            sqlCommandPresupuestoEgreso.Parameters.AddWithValue("@id_proyecto_", idProyecto);
            sqlCommandPresupuestoEgreso.Parameters.AddWithValue("@id_unidad_", idUnidad);
            SqlDataReader readerPresupuestoEgreso;
            sqlConnectionPresupuestoEgreso.Open();
            readerPresupuestoEgreso = sqlCommandPresupuestoEgreso.ExecuteReader();

            LinkedList<PresupuestoEgreso> presupuestoEgresos = new LinkedList<PresupuestoEgreso>();

            while (readerPresupuestoEgreso.Read())
            {
                PresupuestoEgreso presupuestoEgreso = new PresupuestoEgreso();

                presupuestoEgreso.idPresupuestoEgreso = Convert.ToInt32(readerPresupuestoEgreso["id_presupuesto_egreso"].ToString());
                double monto = ObtenerMontoPartida(presupuestoEgreso.idPresupuestoEgreso);
                ActualizarMontoTotalPresupuesto(presupuestoEgreso.idPresupuestoEgreso, monto);
                presupuestoEgreso.idUnidad = Convert.ToInt32(readerPresupuestoEgreso["id_unidad"].ToString());
                presupuestoEgreso.planEstrategicoOperacional = readerPresupuestoEgreso["plan_estrategico_operacional"].ToString();
              
                presupuestoEgreso.montoTotal = Convert.ToDouble(readerPresupuestoEgreso["montoTotal"].ToString());
                presupuestoEgreso.descripcion = ObtenerDescripcionesPartida(presupuestoEgreso.idPresupuestoEgreso);

                presupuestoEgreso.presupuestoEgresoPartidas = new LinkedList<PresupuestoEgresoPartida>();
                SqlCommand sqlCommandPresupuestoEgresoPartidas = new SqlCommand("SELECT id_presupuesto_egreso, id_partida, monto FROM Presupuesto_Egreso_Partida where id_presupuesto_egreso=@id_presupuesto_egreso_;", sqlConnectionPresupuestoEgresoPartida);
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

                    presupuestoEgreso.presupuestoEgresoPartidas.AddLast(presupuestoEgresoPartida);
                   
                }

                presupuestoEgresos.AddLast(presupuestoEgreso);
                sqlConnectionPresupuestoEgresoPartida.Close();
            }

            sqlConnectionPresupuestoEgreso.Close();

            return presupuestoEgresos;
        }
        
        /// <summary>
        /// Este metodo muestra las descripciones existentes para esa partida
        /// </summary>
        private string ObtenerDescripcionesPartida(int idPresupuestoE)
        {
            string descricpcion = "";
            SqlConnection sqlConnectionPresupuestoEgreso = conexion.conexionPEP();
            SqlConnection sqlConnectionPresupuestoEgresoPartida = conexion.conexionPEP();

            SqlCommand sqlCommandPresupuestoEgreso = new SqlCommand("SELECT descripcion FROM Presupuesto_Egreso_Partida where id_presupuesto_egreso = @id_presupuesto_egreso", sqlConnectionPresupuestoEgreso);

            sqlCommandPresupuestoEgreso.Parameters.AddWithValue("@id_presupuesto_egreso", idPresupuestoE);

            SqlDataReader readerPresupuestoEgreso;
            sqlConnectionPresupuestoEgreso.Open();
            readerPresupuestoEgreso = sqlCommandPresupuestoEgreso.ExecuteReader();
            while (readerPresupuestoEgreso.Read())
            {
                descricpcion += "-";
                descricpcion += readerPresupuestoEgreso["descripcion"].ToString()+"\n";
               descricpcion += "\n";
            }

            sqlConnectionPresupuestoEgreso.Close();
            return descricpcion;
        }
        
        /// <summary>
        /// Este metodo suma los montos existentes para esa partida
        /// </summary>
        private double ObtenerMontoPartida(int idPresupuestoE)
        {
            double monto = 0;
            SqlConnection sqlConnectionPresupuestoEgreso = conexion.conexionPEP();
             SqlCommand sqlCommandPresupuestoEgreso = new SqlCommand("SELECT monto FROM Presupuesto_Egreso_Partida where id_presupuesto_egreso = @id_presupuesto_egreso", sqlConnectionPresupuestoEgreso);

            sqlCommandPresupuestoEgreso.Parameters.AddWithValue("@id_presupuesto_egreso", idPresupuestoE);

            SqlDataReader readerPresupuestoEgreso;
            sqlConnectionPresupuestoEgreso.Open();

            readerPresupuestoEgreso = sqlCommandPresupuestoEgreso.ExecuteReader();
            while (readerPresupuestoEgreso.Read())
            {
                
                monto += Convert.ToDouble(readerPresupuestoEgreso["monto"]);
             
            }

       
            sqlConnectionPresupuestoEgreso.Close();
            return monto;
           
        }
       
        
        /// <summary>
        /// Josseline M
        /// Actualiza el monto total de presupuesto egreso a partir de idPresupuestoEgreso
        /// </summary>
        /// <param name="idPresupuestoE"></param>
        /// <param name="monto"></param>
        private void ActualizarMontoTotalPresupuesto(int idPresupuestoE, double monto)
        {
            SqlConnection sqlConnectionPresupuestoEgreso = conexion.conexionPEP();
            SqlCommand sqlCommandPresupuestoEgreso = new SqlCommand("Update Presupuesto_Egreso set montoTotal= @monto where id_presupuesto_egreso = @id_presupuesto_egreso", sqlConnectionPresupuestoEgreso);
            sqlCommandPresupuestoEgreso.Parameters.AddWithValue("@id_presupuesto_egreso", idPresupuestoE);
            sqlCommandPresupuestoEgreso.Parameters.AddWithValue("@monto", monto);
      
  
            sqlConnectionPresupuestoEgreso.Open();
            sqlCommandPresupuestoEgreso.ExecuteScalar();


            sqlConnectionPresupuestoEgreso.Close();
        }
       
        /// <summary>
        /// Este metodo retorna 1 de ser inferior el monto total del presupuesto egresos en compración al 
        /// diponible
        /// </summary>
        /// <param name="presupuesto"></param>
        /// <returns></returns>
        public int AprobarPresupuestoEgresoPorMonto(PresupuestoEgreso presupuesto)
        {
            int esMenor = 0;
            double montoDisponible = 0;
            double montoTotalPresupuestos = 0;
            SqlConnection sqlConnectionPresupuestoEgreso = conexion.conexionPEP();
          
            SqlCommand sqlCommandPresupuestoEgreso = new SqlCommand("select monto FROM Presupuesto_Egreso,Unidad,Proyecto, Presupuesto_Ingreso where where Unidad.id_unidad=@id_unidad_ and Unidad.id_proyecto=Proyecto.id_proyecto and Proyecto.id_proyecto=Presupuesto_Ingreso.id_proyecto", sqlConnectionPresupuestoEgreso);

            sqlCommandPresupuestoEgreso.Parameters.AddWithValue("@id_unidad_", presupuesto.idUnidad);
            SqlDataReader readerPresupuestoEgreso;
            sqlConnectionPresupuestoEgreso.Open();
            readerPresupuestoEgreso = sqlCommandPresupuestoEgreso.ExecuteReader();


            while (readerPresupuestoEgreso.Read())
            {

                montoDisponible = Convert.ToDouble(readerPresupuestoEgreso["monto"].ToString());

            }

            montoTotalPresupuestos = ObtenerMontoPartida(presupuesto.idPresupuestoEgreso);

            if (montoTotalPresupuestos<=montoDisponible)
            {
                actualizaEstadoPresupuestoEgreso(presupuesto.idPresupuestoEgreso,1);
              
                esMenor = 1;
            }

            sqlConnectionPresupuestoEgreso.Close();

            return esMenor;
        }

        /// <summary>
        /// Josseline M
        /// Este método actualiza el estado del presupuesto egreso es decir se la asigna 1 si este ha sido aprobado
        /// o 2 si ha sido guardado pero no aprobado
        /// </summary>
        /// <param name="idPresupuestoEgreso"></param>
        /// <param name="valorEstado"></param>
        private void actualizaEstadoPresupuestoEgreso(int idPresupuestoEgreso,int valorEstado)
        {
            SqlConnection sqlConnectionPresupuestoEgreso = conexion.conexionPEP();
            SqlCommand sqlCommandPresupuestoEgreso = new SqlCommand("UPDATE Presupuesto_Egreso  SET id_estado = @id_estado_ where id_presupuesto_egreso=@id_presupuesto_egreso_", sqlConnectionPresupuestoEgreso);
            sqlCommandPresupuestoEgreso.Parameters.AddWithValue("@id_presupuesto_egreso_", idPresupuestoEgreso);
            sqlCommandPresupuestoEgreso.Parameters.AddWithValue("@id_estado_", valorEstado);
            sqlConnectionPresupuestoEgreso.Open();
            sqlCommandPresupuestoEgreso.ExecuteScalar();
            sqlConnectionPresupuestoEgreso.Close();


        }

        /// <summary>
        /// Guarda el avance obtenido en el añadimiento de partidas
        /// </summary>
        /// <param name="presupuestoE"></param>
        public void guardarPartidasPresupuestoEgreso(LinkedList<PresupuestoEgreso> presupuestosE)
        {
            foreach (PresupuestoEgreso presupuestoIngresar in presupuestosE)
            {
                actualizaEstadoPresupuestoEgreso(presupuestoIngresar.idPresupuestoEgreso, 2);
            }
           
        }
       
        #endregion PRESUPUESTO DE EGRESO
    }
}
