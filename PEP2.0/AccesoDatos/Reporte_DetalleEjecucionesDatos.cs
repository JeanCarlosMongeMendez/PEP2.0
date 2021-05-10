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
    /// 04/may/2021
    /// </summary>
    public class Reporte_DetalleEjecucionesDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 04/may/2021
        /// Efecto: devuelve la lista para el reporte
        /// Requiere: id_proyecto, id_año y id_unidad
        /// Modifica: -
        /// Devuelve: lista de reporte detalle ejecucion
        /// </summary>
        /// <param name="idProyecto"></param>
        /// <param name="idPeriodo"></param>
        /// <param name="idUnidad"></param>
        /// <returns></returns>
        public List<ReporteDetalleEjecucion> getReporteEgresosPorUnidades(int idProyecto, int idPeriodo, int idUnidad)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<ReporteDetalleEjecucion> ejecuciones = new List<ReporteDetalleEjecucion>();

            String consulta = @"select eup.*, E.monto as monto_ejecucion,P.numero_partida, P.descripcion_partida,EE.descripcion_estado,TT.nombre_tramite
from Ejecucion_Unidad_Partida eup, Ejecucion E, Partida P,EstadoEjecucion EE, Tipos_tramite TT
where eup.id_ejecucion in (
select id_ejecucion from Ejecucion where id_proyecto = @idProyecto and ano_periodo= @idPeriodo and (id_estado =2 or id_estado=3))
and eup.id_unidad =@idUnidad and E.id_ejecucion = eup.id_ejecucion and P.id_partida = EUP.id_partida and EE.id_estado = E.id_estado and
TT.id_tramite = E.id_tipo_tramite
order by numero_partida";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idProyecto", idProyecto);
            sqlCommand.Parameters.AddWithValue("@idPeriodo", idPeriodo);
            sqlCommand.Parameters.AddWithValue("@idUnidad", idUnidad);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                ReporteDetalleEjecucion ejecucion = new ReporteDetalleEjecucion();

                ejecucion.descEstado = reader["descripcion_estado"].ToString();
                ejecucion.descPartida = reader["descripcion_partida"].ToString();
                ejecucion.idEjecucion = Convert.ToInt32(reader["id_ejecucion"].ToString());
                ejecucion.monto = Convert.ToDouble(reader["monto"].ToString());
                ejecucion.montoEjecucion = Convert.ToDouble(reader["monto_ejecucion"].ToString());
                ejecucion.nombreTramite = reader["nombre_tramite"].ToString();
                ejecucion.numeroPartida = reader["numero_partida"].ToString();

                ejecuciones.Add(ejecucion);
            }

            sqlConnection.Close();

            return ejecuciones;
        }
    }
}
