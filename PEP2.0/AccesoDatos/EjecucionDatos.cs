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
        /// Inserta una EjecucionUnidad
        /// </summary>
        /// <param name="unidad">Unidad</param>
        ///  <param name="numeroReferencia"></param>
        ///  <param name="respuesta"></param>
        public void insertarEjecucionUnidad(Unidad unidad, string numeroReferencia,int respuesta)
        {


            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"insert Unidad_ejecucion (numero_referencia,id_unidad,nombre_unidad,id_ejecucion)
                                            values(@numero_referencia,@id_unidad,@nombre_unidad,@id_ejecucion)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@numero_referencia", Convert.ToString(numeroReferencia));
            command.Parameters.AddWithValue("@id_unidad", unidad.idUnidad);
            command.Parameters.AddWithValue("@nombre_unidad", unidad.nombreUnidad);
            command.Parameters.AddWithValue("@id_ejecucion", respuesta);


            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }


        /// <summary>
        /// Inserta una PartidaEjecucion
        /// </summary>
        /// <param name="partida">partida</param>
        /// <param name="numeroReferencia"></param>
        /// <param name="respuesta"></param>
        public void insertarEjecucionPartidas(Partida partida,string numeroReferencia, int respuesta)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"insert Partida_ejecucion (numero_referencia,id_partida,numero_partida,descripcion_partida,id_ejecucion) 
                                            values(@numero_referencia,@id_partida,@numero_partida,@descripcion_partida,@id_ejecucion)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@numero_referencia", Convert.ToString(numeroReferencia));
            command.Parameters.AddWithValue("@id_partida", partida.idPartida);
            command.Parameters.AddWithValue("@numero_partida", partida.numeroPartida);
            command.Parameters.AddWithValue("@descripcion_partida", partida.descripcionPartida);
            command.Parameters.AddWithValue("@id_ejecucion", respuesta);



            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }


        /// <summary>
        /// Inserta EjecucionMontoPartidaElegida
        /// </summary>
        /// <param name="partidaUnidad">PartidaUnidad</param>
        /// <param name="numeroReferencia"></param>
        /// <param name="respuesta"></param>
        public void insertarEjecucionMontoPartidaElegida(PartidaUnidad partidaUnidad, string numeroReferencia, int respuesta)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"insert EjecucionMontoPartidasElegidas (numero_referencia,id_partida,id_unidad,monto,monto_disponible,numero_partida,id_ejecucion) 
                                            values(@numero_referencia,@id_partida,@id_unidad,@monto,@monto_disponible,@numero_partida,@id_ejecucion)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@numero_referencia", Convert.ToString(numeroReferencia));
            command.Parameters.AddWithValue("@id_partida", partidaUnidad.IdPartida);
            command.Parameters.AddWithValue("@id_unidad", partidaUnidad.IdUnidad);
            command.Parameters.AddWithValue("@monto", partidaUnidad.Monto);
            command.Parameters.AddWithValue("@monto_disponible", partidaUnidad.MontoDisponible);
            command.Parameters.AddWithValue("@numero_partida", partidaUnidad.NumeroPartida);
            command.Parameters.AddWithValue("@id_ejecucion", respuesta);


            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
        /// <summary>
        /// Inserta Ejecucion
        /// </summary>
        /// <param name="ejecucion">ejecucion</param>
        public int insertarEjecucion(Ejecucion ejecucion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            int respuesta = 0;
            sqlConnection.Open();
            String consulta = @"insert Ejecucion(id_estado,ano_periodo,id_proyecto,monto,id_tipo_tramite,numero_referencia) output INSERTED.id_ejecucion
                                            values(@id_estado,@ano_periodo,@id_proyecto,@monto,@id_tipo_tramite,@numero_referencia)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@id_estado", ejecucion.idestado);
            command.Parameters.AddWithValue("@ano_periodo", ejecucion.anoPeriodo);
            command.Parameters.AddWithValue("@id_proyecto", ejecucion.idProyecto);
            command.Parameters.AddWithValue("@monto", ejecucion.monto);
            command.Parameters.AddWithValue("@id_tipo_tramite", ejecucion.idTipoTramite);
            command.Parameters.AddWithValue("@numero_referencia", ejecucion.numeroReferencia);
            respuesta = (int)command.ExecuteScalar();

            
            sqlConnection.Close();
            return respuesta;
        }


        /// <summary>
        /// Eliminar una EjecucionUnidad
        /// </summary>
        /// <param name="respuesta">respuesta</param>
        public void EliminarEjecucionUnidad(int respuesta)
        {


            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete  Unidad_ejecucion where id_ejecucion=@id_ejecucion";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@id_ejecucion", respuesta);


            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
        /// <summary>
        /// Eliminar una PartidaEjecucion
        /// </summary>
        /// <param name="respuesta"></param>
        public void eliminarEjecucionPartidas( int respuesta)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete Partida_ejecucion where id_ejecucion=@id_ejecucion" ;
                                           

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            
           
            command.Parameters.AddWithValue("@id_ejecucion", respuesta);



            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
        /// <summary>
        /// Eliminar EjecucionMontoPartidaElegida
        /// </summary>
        /// <param name="respuesta"></param>
        public void eliminarEjecucionMontoPartidaElegida(int respuesta)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete EjecucionMontoPartidasElegidas where id_ejecucion=@id_ejecucion"; 
                                        

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

           
           
            command.Parameters.AddWithValue("@id_ejecucion", respuesta);


            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
        /// <summary>
        /// Actulizar Ejecucion
        /// </summary>
        /// <param name="ejecucion">ejecucion</param>
        /// <param name="respuesta"></param>
        public void actualizarEjecucion(Ejecucion ejecucion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update Ejecucion set id_estado=@id_estado,ano_periodo=@ano_periodo ,id_proyecto=@id_proyecto,monto=@monto,id_tipo_tramite=@id_tipo_tramite,numero_referencia=@numero_referencia 
                                 where id_ejecucion=@id_ejecucion";


            SqlCommand command = new SqlCommand(consulta, sqlConnection);
            command.Parameters.AddWithValue("@id_ejecucion",ejecucion.idEjecucion);
            command.Parameters.AddWithValue("@id_estado", ejecucion.idestado);
            command.Parameters.AddWithValue("@ano_periodo", ejecucion.anoPeriodo);
            command.Parameters.AddWithValue("@id_proyecto", ejecucion.idProyecto);
            command.Parameters.AddWithValue("@monto", ejecucion.monto);
            command.Parameters.AddWithValue("@id_tipo_tramite", ejecucion.idTipoTramite);
            command.Parameters.AddWithValue("@numero_referencia", ejecucion.numeroReferencia);
           

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
        /// <summary>
        /// Consulta Monto disponible
        /// </summary>
        /// <param name="idPartida"></param>
        ///  <param name="idPresupuestoEgreso"></param>
        public double consultarMontoDiponible(string idPartida,string idPresupuestoEgreso)
        {
            Double monto=0;
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"select (
                                         select SUM(monto) as monto from Presupuesto_Egreso_Partida
                                         where id_partida=@idPartida and id_presupuesto_egreso= @idPresupuestoEgreso and id_estado_presupuesto=2 ) - ISNULL((
                                         select SUM(EMPE.monto) as monto_ejecutado from EjecucionMontoPartidasElegidas EMPE where EMPE.id_partida=@idPartida 
                                         and EMPE.id_ejecucion  =( (select E.id_ejecucion from Ejecucion E where E.id_ejecucion=EMPE.id_ejecucion and E.id_estado=2 ))),0)as montoDisponible";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idPartida",Convert.ToInt32(idPartida));
            command.Parameters.AddWithValue("@idPresupuestoEgreso", Convert.ToInt32(idPresupuestoEgreso));

            SqlDataReader reader;
            sqlConnection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
               monto = Convert.ToDouble(reader["montoDisponible"].ToString());
            }
            sqlConnection.Close();
            return monto;
        }


    }
}

