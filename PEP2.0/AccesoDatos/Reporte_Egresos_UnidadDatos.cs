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
    /// 09/ene/2020
    /// Clase para administrar las consultas de reporte egresos por unidad
    /// </summary>
    public class Reporte_Egresos_UnidadDatos
    {
        ConexionDatos conexion = new ConexionDatos();


        /// <summary>
        /// Leonardo Carrion
        /// 09/ene/2020
        /// Efecto: devuelve la lista para llenar el reporte
        /// Requiere: proyecto
        /// Modifica: -
        /// Devuelve: lista de Reporte egreso
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<Reporte_Egresos_Unidad> getReporteEgresosPorUnidades(Proyectos proyecto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Reporte_Egresos_Unidad> egresos = new List<Reporte_Egresos_Unidad>();

            String consulta = @"select U.nombre_unidad, PA.numero_partida, PA.descripcion_partida, P.descripcion, P.monto
                                            from Presupuesto_Egreso_Partida P, Unidad U, Partida PA
                                            where U.id_proyecto= @idProyecto and P.id_presupuesto_egreso in (
                                            select id_presupuesto_egreso from Presupuesto_Egreso where id_unidad = U.id_unidad)
                                            and PA.id_partida = P.id_partida and P.id_estado_presupuesto = (select id_estado_presupuesto from Estado_presupuestos where descripcion_estado_presupuesto ='Aprobar')
                                            order by U.id_unidad, PA.numero_partida;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idProyecto", proyecto.idProyecto);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Reporte_Egresos_Unidad egreso = new Reporte_Egresos_Unidad();

                egreso.nombreUnidad = reader["nombre_unidad"].ToString();
                egreso.numeroPartida = reader["numero_partida"].ToString(); 
                egreso.descPartida = reader["descripcion_partida"].ToString();
                egreso.descPresupuesto = reader["descripcion"].ToString();
                egreso.monto = Convert.ToDouble(reader["monto"].ToString());

                egresos.Add(egreso);
            }

            sqlConnection.Close();

            return egresos;
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 10/ene/2020
        /// Efecto: devuelve la lista para llenar el reporte
        /// Requiere: unidad
        /// Modifica: -
        /// Devuelve: lista de Reporte egreso
        /// </summary>
        /// <param name="unidad"></param>
        /// <returns></returns>
        public List<Reporte_Egresos_Unidad> getReporteEgresosPorUnidades(Unidad unidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Reporte_Egresos_Unidad> egresos = new List<Reporte_Egresos_Unidad>();

            String consulta = @"select U.nombre_unidad, PA.numero_partida, PA.descripcion_partida, P.descripcion, P.monto
                                            from Presupuesto_Egreso_Partida P, Unidad U, Partida PA
                                            where U.id_unidad= @idUnidad and P.id_presupuesto_egreso in (
                                            select id_presupuesto_egreso from Presupuesto_Egreso where id_unidad = U.id_unidad)
                                            and PA.id_partida = P.id_partida and P.id_estado_presupuesto = (select id_estado_presupuesto from Estado_presupuestos where descripcion_estado_presupuesto ='Aprobar')
                                            order by U.id_unidad, PA.numero_partida;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idUnidad", unidad.idUnidad);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Reporte_Egresos_Unidad egreso = new Reporte_Egresos_Unidad();

                egreso.nombreUnidad = reader["nombre_unidad"].ToString();
                egreso.numeroPartida = reader["numero_partida"].ToString();
                egreso.descPartida = reader["descripcion_partida"].ToString();
                egreso.descPresupuesto = reader["descripcion"].ToString();
                egreso.monto = Convert.ToDouble(reader["monto"].ToString());

                egresos.Add(egreso);
            }

            sqlConnection.Close();

            return egresos;
        }
    }
}