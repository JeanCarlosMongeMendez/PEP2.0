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
    /// 12/abr/2021
    /// Clase para administrar las consultas de reporte ejeuciones
    /// </summary>
    public class Reporte_EjecucuionesDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 12/abr/2021
        /// Efecto: devuelve la lista para llenar el reporte
        /// Requiere: Periodo y proyecto
        /// Modifica: -
        /// Devuelve: lista de Reporte ejecuciones
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<Reporte_Ejecuciones> getReporteEjecuciones(Periodo periodo, Proyectos proyecto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Reporte_Ejecuciones> ejecuciones = new List<Reporte_Ejecuciones>();

            String consulta = @"select P2.numero_partida as numero_partida_padre,P2.descripcion_partida as descripcion_partida_padre,P.numero_partida as 
numero_partida_hija,P.descripcion_partida as descripcion_partida_hija,U.nombre_unidad,P.id_partida,U.id_unidad,Sum(PEP.monto) as monto
from Partida P,Partida P2, Proyecto PR,Unidad U, Presupuesto_Egreso PE, Presupuesto_Egreso_Partida PEP
where P.disponible = 1 and P.id_partida_padre is not null and P2.id_partida = P.id_partida_padre
and PR.id_proyecto = @idProyecto and U.id_proyecto = PR.id_proyecto and PE.id_unidad = U.id_unidad and
PEP.id_presupuesto_egreso = PE.id_presupuesto_egreso and P.id_partida = PEP.id_partida
group by P2.numero_partida,P2.descripcion_partida,P.numero_partida,P.descripcion_partida,U.nombre_unidad,P.id_partida,U.id_unidad
union
select P2.numero_partida as numero_partida_padre,P2.descripcion_partida as descripcion_partida_padre,P.numero_partida as numero_partida_hija,P.descripcion_partida as 
descripcion_partida_hija,U.nombre_unidad,P.id_partida,U.id_unidad, 0 as monto
from Unidad U, Partida P, Partida P2
Where U.id_proyecto = @idProyecto and P.disponible = 1 and P.id_partida_padre is not null and P2.id_partida = P.id_partida_padre
and P.ano_periodo = @anoPeriodo and P.id_partida not in (select id_partida from Presupuesto_Egreso_Partida where id_presupuesto_egreso in(
select id_presupuesto_egreso from Presupuesto_Egreso where id_unidad in 
(select id_unidad from Unidad where id_proyecto in (select id_proyecto from Proyecto where id_proyecto=@idProyecto))
))
group by P2.numero_partida,P2.descripcion_partida,P.numero_partida,P.descripcion_partida,U.nombre_unidad,P.id_partida,U.id_unidad;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idProyecto", proyecto.idProyecto);
            sqlCommand.Parameters.AddWithValue("@anoPeriodo", periodo.anoPeriodo);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {

                Reporte_Ejecuciones egreso = new Reporte_Ejecuciones();

                egreso.numeroPartidaPadre = reader["numero_partida_padre"].ToString();
                egreso.descripcionPartidaPadre = reader["descripcion_partida_padre"].ToString();
                egreso.numeroPartidaHija = reader["numero_partida_hija"].ToString();
                egreso.descripcionPartidaHija = reader["descripcion_partida_hija"].ToString();
                egreso.nombreUnidad = reader["nombre_unidad"].ToString();
                egreso.monto = Convert.ToDouble(reader["monto"].ToString());
                egreso.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                egreso.idUnidad = Convert.ToInt32(reader["id_unidad"].ToString());

                ejecuciones.Add(egreso);
            }

            sqlConnection.Close();

            SqlConnection sqlConnection2 = conexion.conexionPEP();

            foreach (Reporte_Ejecuciones reporte_Ejecuciones in ejecuciones)
            {
                consulta = @"select (select Sum(EUP.monto) from Ejecucion_Unidad_Partida EUP where EUP.id_partida = @idPartida and EUP.id_unidad = @idUnidad) as monto_ejecutado";
                sqlCommand = new SqlCommand(consulta, sqlConnection2);
                sqlCommand.Parameters.AddWithValue("@idPartida", reporte_Ejecuciones.idPartida);
                sqlCommand.Parameters.AddWithValue("@idUnidad", reporte_Ejecuciones.idUnidad);

                sqlConnection2.Open();
                reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    if (String.IsNullOrEmpty(reader["monto_ejecutado"].ToString()))
                    {
                        reporte_Ejecuciones.montoEjecutado = 0;
                    }
                    else
                    {
                        reporte_Ejecuciones.montoEjecutado = Convert.ToDouble(reader["monto_ejecutado"].ToString());
                    }
                }
                else
                {
                    reporte_Ejecuciones.montoEjecutado = 0;
                }
                sqlConnection2.Close();
            }

            return ejecuciones;
        }

    }
}
