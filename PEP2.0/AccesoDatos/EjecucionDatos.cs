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
    /// Kevin Picado
    /// 07/febrero/2020
    /// Clase para administrar las consultas sql del Ejecucion
    /// </summary>
    public class EjecucionDatos
    {
        private ConexionDatos conexion = new ConexionDatos();
      
      
        /// <summary>
        /// Inserta Ejecucion
        /// </summary>
        /// <param name="ejecucion">ejecucion</param>
        public int insertarEjecucion(Ejecucion ejecucion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            int respuesta = 0;
            sqlConnection.Open();
            String consulta = @"insert Ejecucion(id_estado,ano_periodo,id_proyecto,monto,id_tipo_tramite,numero_referencia,descripcion_tramite_otro,realizado_por,fecha) output INSERTED.id_ejecucion
                                            values(@id_estado,@ano_periodo,@id_proyecto,@monto,@id_tipo_tramite,@numero_referencia,@descripcion_tramite_otro,@realizadoPor,@fecha)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@id_estado", ejecucion.estadoEjecucion.idEstado);
            command.Parameters.AddWithValue("@ano_periodo", ejecucion.anoPeriodo);
            command.Parameters.AddWithValue("@id_proyecto", ejecucion.idProyecto);
            command.Parameters.AddWithValue("@monto", ejecucion.monto);
            command.Parameters.AddWithValue("@id_tipo_tramite", ejecucion.tipoTramite.idTramite);
            command.Parameters.AddWithValue("@numero_referencia", ejecucion.numeroReferencia);
            command.Parameters.AddWithValue("@descripcion_tramite_otro", ejecucion.descripcionEjecucionOtro);
            command.Parameters.AddWithValue("@realizadoPor", ejecucion.realizadoPor);
            command.Parameters.AddWithValue("@fecha", DateTime.Now);

            respuesta = (int)command.ExecuteScalar();

            
            sqlConnection.Close();
            return respuesta;
        }
        
        /// <summary>
        /// Actulizar Ejecucion
        /// </summary>
        /// <param name="ejecucion">ejecucion</param>
        /// <param name="respuesta"></param>
        public void actualizarEjecucion(Ejecucion ejecucion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update Ejecucion set id_estado=@id_estado,ano_periodo=@ano_periodo ,id_proyecto=@id_proyecto,monto=@monto,id_tipo_tramite=@id_tipo_tramite,numero_referencia=@numero_referencia,descripcion_tramite_otro=@descripcion_tramite_otro
                                 where id_ejecucion=@id_ejecucion";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);
            command.Parameters.AddWithValue("@id_ejecucion",ejecucion.idEjecucion);
            command.Parameters.AddWithValue("@id_estado", ejecucion.estadoEjecucion.idEstado);
            command.Parameters.AddWithValue("@ano_periodo", ejecucion.anoPeriodo);
            command.Parameters.AddWithValue("@id_proyecto", ejecucion.idProyecto);
            command.Parameters.AddWithValue("@monto", ejecucion.monto);
            command.Parameters.AddWithValue("@id_tipo_tramite", ejecucion.tipoTramite.idTramite);
            command.Parameters.AddWithValue("@numero_referencia", ejecucion.numeroReferencia);
            command.Parameters.AddWithValue("@descripcion_tramite_otro", ejecucion.descripcionEjecucionOtro);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
    
        /// <summary>
        /// Eliminar Ejecucion
        /// </summary>
        /// <param name="idEjecucion">ejecucion</param>
       
        public void eliminarEjecucion(int idEjecucion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete Ejecucion 
                                 where id_ejecucion=@id_ejecucion";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);
            command.Parameters.AddWithValue("@id_ejecucion",idEjecucion);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 23/feb/2021
        /// Efecto: devuelve la lista de ejecuciones segun el periodo y proyecto seleccionado
        /// Requiere: periodo y proyecto
        /// Modifica: -
        /// Devuelve: lista de ejecuciones
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<Ejecucion> getEjecucionesPorPeriodoYProyecto(Periodo periodo, Proyectos proyecto)
        {
            List<Ejecucion> listaEjecucion = new List<Ejecucion>();
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"select descripcion_estado,monto,numero_referencia,nombre_tramite,E.id_ejecucion,T.id_tramite,descripcion_tramite_otro, E.realizado_por,E.fecha
                                      from EstadoEjecucion Es,Ejecucion E,Tipos_tramite T
                                      where E.id_proyecto=@idProyecto and E.ano_periodo=@Periodo and E.id_tipo_tramite= T.id_tramite and E.id_estado= Es.id_estado order by  id_ejecucion desc";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@Periodo", periodo.anoPeriodo);
            command.Parameters.AddWithValue("@idProyecto", proyecto.idProyecto);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                EstadoEjecucion estadoEjecucion = new EstadoEjecucion();
                TipoTramite tipoTramite = new TipoTramite();
                Ejecucion ejecucion = new Ejecucion();
                ejecucion.idEjecucion = Convert.ToInt32(reader["id_ejecucion"].ToString());
                ejecucion.monto = Convert.ToDouble(reader["monto"].ToString());
                ejecucion.numeroReferencia = Convert.ToString(reader["numero_referencia"].ToString());
                tipoTramite.nombreTramite = Convert.ToString(reader["nombre_tramite"].ToString());
                tipoTramite.idTramite = Convert.ToInt32(reader["id_tramite"].ToString());
                ejecucion.tipoTramite = tipoTramite;
                estadoEjecucion.descripcion = Convert.ToString(reader["descripcion_estado"].ToString());
                ejecucion.estadoEjecucion = estadoEjecucion;
                ejecucion.descripcionEjecucionOtro = Convert.ToString(reader["descripcion_tramite_otro"].ToString());
                ejecucion.realizadoPor = reader["realizado_por"].ToString();
                ejecucion.fecha = Convert.ToDateTime(reader["fecha"].ToString());
                listaEjecucion.Add(ejecucion);
            }
            sqlConnection.Close();
            return listaEjecucion;
        }


    }
}

