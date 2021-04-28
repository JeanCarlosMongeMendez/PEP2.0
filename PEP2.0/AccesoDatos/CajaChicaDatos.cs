using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
namespace AccesoDatos
{
    public class CajaChicaDatos
    {
        private ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        ///Kevin Picado
        /// 23/04/2021
        /// Efecto: devuelve la lista de ejecuciones segun el periodo y proyecto seleccionado
        /// Requiere: periodo y proyecto
        /// Modifica: -
        /// Devuelve: lista de ejecuciones
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<CajaChica> getCajaChicaPorPeriodoYProyecto(Periodo periodo, Proyectos proyecto)
        {
            List<CajaChica> listaCajaChica = new List<CajaChica>();
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"select descripcion,monto,S.id_solicitud_caja_chica, S.realizado_por,S.fecha
                                      from Estado_Caja_Chica EC,Solicitud_Caja_Chica S
                                      where S.id_proyecto=@idProyecto and S.ano_periodo=@Periodo and S.id_estado_caja_chica= EC.id_estado_caja_chica order by  id_solicitud_caja_chica desc";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@Periodo", periodo.anoPeriodo);
            command.Parameters.AddWithValue("@idProyecto", proyecto.idProyecto);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                EstadoCajaChica estadoCajaChica = new EstadoCajaChica();
               
                CajaChica cajaChica = new CajaChica();
                cajaChica.idCajaChica = Convert.ToInt32(reader["id_solicitud_caja_chica"].ToString());
                cajaChica.monto = Convert.ToDouble(reader["monto"].ToString());
                estadoCajaChica.descripcion = Convert.ToString(reader["descripcion"].ToString());
                cajaChica.idEstadoCajaChica = estadoCajaChica;
                cajaChica.realizadoPor = reader["realizado_por"].ToString();
                cajaChica.fecha = Convert.ToDateTime(reader["fecha"].ToString());
                listaCajaChica.Add(cajaChica);
            }
            sqlConnection.Close();
            return listaCajaChica ;
        }
        /// <summary>
        /// Actulizar Ejecucion
        /// </summary>
        /// <param name="cajaChica">ejecucion</param>
        /// <param name="respuesta"></param>
        public void actualizarCajaChica(CajaChica cajaChica)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update Solicitud_Caja_Chica set id_estado_caja_chica=@id_estado,ano_periodo=@ano_periodo ,id_proyecto=@id_proyecto,monto=@monto
                                 where id_solicitud_caja_chica=@id_CajaChica";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);
            command.Parameters.AddWithValue("@id_CajaChica", cajaChica.idCajaChica);
            command.Parameters.AddWithValue("@id_estado", cajaChica.idEstadoCajaChica.idEstadoCajaChica);
            command.Parameters.AddWithValue("@ano_periodo", cajaChica.anoPeriodo);
            command.Parameters.AddWithValue("@id_proyecto", cajaChica.idProyedto);
            command.Parameters.AddWithValue("@monto", cajaChica.monto);
            

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }


        public string getNumeroSolicitudCajaChica(int año)
        {
           
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"select Max(numero) as numero,ano_periodo
                                      from Solicitud_Caja_Chica S
                                      where S.ano_periodo=@año 
                                      group by ano_periodo";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@año", año);
            

            SqlDataReader reader;
            sqlConnection.Open();
            reader = command.ExecuteReader();
            int numero=0 ;
           
            while (reader.Read())
            {
                numero = Convert.ToInt32(reader["numero"].ToString());
                numero = numero + 1;
              
            }
            sqlConnection.Close();
            return  Convert.ToString(numero);
        }
    }
}
