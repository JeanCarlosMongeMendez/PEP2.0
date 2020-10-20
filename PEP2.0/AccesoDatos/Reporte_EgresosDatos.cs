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
    /// 21/oct/2019
    /// Clase para administrar las consultas de reporte egresos
    /// </summary>
    public class Reporte_EgresosDatos
    {
        ConexionDatos conexion = new ConexionDatos();


        /// <summary>
        /// Leonardo Carrion
        /// 21/oct/2019
        /// Efecto: devuelve la lista para llenar el reporte
        /// Requiere: Periodo y proyecto
        /// Modifica: -
        /// Devuelve: lista de Reporte egreso
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<Reporte_Egresos> getReporteEgresos(Periodo periodo, Proyectos proyecto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Reporte_Egresos> egresos = new List<Reporte_Egresos>();

            String consulta = @"select P2.numero_partida as numero_partida_padre,P2.descripcion_partida as descripcion_partida_padre,P.numero_partida as numero_partida_hija,P.descripcion_partida as descripcion_partida_hija,U.nombre_unidad,PEP.monto,PEP.id_linea
                                from Partida P,Partida P2, Proyecto PR,Unidad U, Presupuesto_Egreso PE, Presupuesto_Egreso_Partida PEP
                                where P.disponible = 1 and P.id_partida_padre is not null and P2.id_partida = P.id_partida_padre
                                and PR.id_proyecto = @idProyecto and U.id_proyecto = PR.id_proyecto and PE.id_unidad = U.id_unidad and
                                PEP.id_presupuesto_egreso = PE.id_presupuesto_egreso and P.id_partida = PEP.id_partida
                                union
                                select P2.numero_partida as numero_partida_padre,P2.descripcion_partida as descripcion_partida_padre,P.numero_partida as numero_partida_hija,P.descripcion_partida as descripcion_partida_hija,U.nombre_unidad, 0 as monto,0 as id_linea
                                from Unidad U, Partida P, Partida P2
                                Where U.id_proyecto = @idProyecto and P.disponible = 1 and P.id_partida_padre is not null and P2.id_partida = P.id_partida_padre
                                and P.ano_periodo = @anoPeriodo and P.id_partida not in (select id_partida from Presupuesto_Egreso_Partida where id_presupuesto_egreso in(
                                select id_presupuesto_egreso from Presupuesto_Egreso where id_unidad in 
                                (select id_unidad from Unidad where id_proyecto in (select id_proyecto from Proyecto where id_proyecto=@idProyecto))
                                ))
                                order by P.numero_partida;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idProyecto", proyecto.idProyecto);
            sqlCommand.Parameters.AddWithValue("@anoPeriodo", periodo.anoPeriodo);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {

                Reporte_Egresos egreso = new Reporte_Egresos();

                egreso.numeroPartidaPadre = reader["numero_partida_padre"].ToString();
                egreso.descripcionPartidaPadre = reader["descripcion_partida_padre"].ToString();
                egreso.numeroPartidaHija = reader["numero_partida_hija"].ToString();
                egreso.descripcionPartidaHija = reader["descripcion_partida_hija"].ToString();
                egreso.nombreUnidad = reader["nombre_unidad"].ToString();
                egreso.monto = Convert.ToDouble(reader["monto"].ToString());

                egresos.Add(egreso);
            }

            sqlConnection.Close();

            return egresos;
        }
    }
}
