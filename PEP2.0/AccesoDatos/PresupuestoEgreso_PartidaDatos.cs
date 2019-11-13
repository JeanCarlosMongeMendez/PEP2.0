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
    /// Clase para administrar las consultas sql de la relacion entre presupuesto de egresos y partida
    /// </summary>
    public class PresupuestoEgreso_PartidaDatos
    {
        private ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 08/oct/2019
        /// Efecto: devuelve la lista de partidas con los montos segun el presupuesto de egreso
        /// Requiere: presupuesto de egreso
        /// Modifica: -
        /// Devuelve: lista de presupuesto egreso partidas
        /// </summary>
        /// <param name="presupuestoEgreso"></param>
        /// <returns></returns>
        public List<PresupuestoEgresoPartida> getPresupuestoEgresoPartidas(PresupuestoEgreso presupuestoEgreso)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartida = new List<PresupuestoEgresoPartida>();

            SqlCommand sqlCommand = new SqlCommand(@"select PEP.*, P.descripcion_partida,P.numero_partida,EP.descripcion_estado_presupuesto 
                                                                                            from Presupuesto_Egreso_Partida PEP, Partida P,Estado_presupuestos EP
                                                                                            where PEP.id_presupuesto_egreso = @idPresupuestoEgreso and P.id_partida = PEP.id_partida and P.id_partida_padre is not null
                                                                                            and P.ano_periodo = (select ano_periodo from Proyecto where id_proyecto = (
                                                                                            select id_proyecto from Unidad where id_unidad = (select id_unidad from Presupuesto_Egreso where id_presupuesto_egreso = @idPresupuestoEgreso)
                                                                                            )) and P.disponible = 'True' and EP.id_estado_presupuesto = PEP.id_estado_presupuesto
                                                                                            union
                                                                                            select @idPresupuestoEgreso as id_presupuesto_egreso, P.id_partida, 0 as monto, '' as descripcion,(select id_estado_presupuesto from Estado_presupuestos where descripcion_estado_presupuesto='Guardar') as id_estado_presupuesto,1 as id_linea, P.descripcion_partida,P.numero_partida,'Espera' as descripcion_estado_presupuesto
                                                                                            from Partida P
                                                                                            where P.id_partida not in ( Select PEP.id_partida from Presupuesto_Egreso_Partida PEP where PEP.id_presupuesto_egreso = @idPresupuestoEgreso
                                                                                            ) and P.id_partida_padre is not null and P.ano_periodo = (select ano_periodo from Proyecto where id_proyecto = (
                                                                                            select id_proyecto from Unidad where id_unidad = (select id_unidad from Presupuesto_Egreso where id_presupuesto_egreso = @idPresupuestoEgreso)
                                                                                            )) and P.disponible = 'True'
                                                                                            order by P.numero_partida", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idPresupuestoEgreso", presupuestoEgreso.idPresupuestoEgreso);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                PresupuestoEgresoPartida presupuestoEgresoPartida = new PresupuestoEgresoPartida();
                Partida partida = new Partida();
                EstadoPresupuesto estadoPresupuesto = new EstadoPresupuesto();

                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partida.descripcionPartida = reader["descripcion_partida"].ToString();
                partida.numeroPartida = reader["numero_partida"].ToString();

                estadoPresupuesto.idEstadoPresupuesto = Convert.ToInt32(reader["id_estado_presupuesto"].ToString());
                estadoPresupuesto.descripcionEstado = reader["descripcion_estado_presupuesto"].ToString();

                presupuestoEgresoPartida.partida = partida;
                presupuestoEgresoPartida.estadoPresupuesto = estadoPresupuesto;
                presupuestoEgresoPartida.idPresupuestoEgreso = Convert.ToInt32(reader["id_presupuesto_egreso"].ToString());
                presupuestoEgresoPartida.monto = Convert.ToDouble(reader["monto"].ToString());
                presupuestoEgresoPartida.descripcion = reader["descripcion"].ToString();
                presupuestoEgresoPartida.idLinea = Convert.ToInt32(reader["id_linea"].ToString());

                listaPresupuestoEgresoPartida.Add(presupuestoEgresoPartida);
            }

            sqlConnection.Close();

            return listaPresupuestoEgresoPartida;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 08/oct/2019
        /// Efecto: inserta en la base de datos la relacion entre presupuesto de egresos y partida
        /// Requiere: presupuesto de egreso partida
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoEgresoPartida"></param>
        public void insertarPresupuestoEgreso_Partida(PresupuestoEgresoPartida presupuestoEgresoPartida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"insert Presupuesto_Egreso_Partida (id_presupuesto_egreso,id_partida,monto,descripcion,id_estado_presupuesto) 
                                            values(@idPresupuestoEgreso,@idPartida,@monto,@descripcion,@idEstadoPresupuesto)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idPresupuestoEgreso", presupuestoEgresoPartida.idPresupuestoEgreso);
            command.Parameters.AddWithValue("@idPartida", presupuestoEgresoPartida.partida.idPartida);
            command.Parameters.AddWithValue("@monto", presupuestoEgresoPartida.monto);
            command.Parameters.AddWithValue("@descripcion", presupuestoEgresoPartida.descripcion);
            command.Parameters.AddWithValue("@idEstadoPresupuesto", presupuestoEgresoPartida.estadoPresupuesto.idEstadoPresupuesto);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 09/oct/2019
        /// Efecto: devuelve lista de presupuestos de egresos partidas segun los datos consultados
        /// Requiere: presupuesto de egreso y partida
        /// Modifica: -
        /// Devuelve: lista de presupuesto de egresos
        /// </summary>
        /// <param name="presupuestoEgresoPartidaConsulta"></param>
        /// <returns></returns>
        public List<PresupuestoEgresoPartida> getPresupuestoEgresoPartidasPorPartidaYPresupEgreso(PresupuestoEgresoPartida presupuestoEgresoPartidaConsulta)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartida = new List<PresupuestoEgresoPartida>();

            SqlCommand sqlCommand = new SqlCommand(@"select PEP.*, P.descripcion_partida,P.numero_partida,EP.descripcion_estado_presupuesto 
                                                                                            from Presupuesto_Egreso_Partida PEP, Partida P,Estado_presupuestos EP
                                                                                            where PEP.id_presupuesto_egreso = @idPresupuestoEgreso and PEP.id_partida = @idPartida and P.id_partida = PEP.id_partida and P.id_partida_padre is not null
                                                                                            and P.ano_periodo = (select ano_periodo from Proyecto where id_proyecto = (
                                                                                            select id_proyecto from Unidad where id_unidad = (select id_unidad from Presupuesto_Egreso where id_presupuesto_egreso = @idPresupuestoEgreso)
                                                                                            )) and P.disponible = 'True' and EP.id_estado_presupuesto = PEP.id_estado_presupuesto", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idPresupuestoEgreso", presupuestoEgresoPartidaConsulta.idPresupuestoEgreso);
            sqlCommand.Parameters.AddWithValue("@idPartida", presupuestoEgresoPartidaConsulta.partida.idPartida);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                PresupuestoEgresoPartida presupuestoEgresoPartida = new PresupuestoEgresoPartida();
                Partida partida = new Partida();
                EstadoPresupuesto estadoPresupuesto = new EstadoPresupuesto();

                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partida.descripcionPartida = reader["descripcion_partida"].ToString();
                partida.numeroPartida = reader["numero_partida"].ToString();

                estadoPresupuesto.idEstadoPresupuesto = Convert.ToInt32(reader["id_estado_presupuesto"].ToString());
                estadoPresupuesto.descripcionEstado = reader["descripcion_estado_presupuesto"].ToString();

                presupuestoEgresoPartida.partida = partida;
                presupuestoEgresoPartida.estadoPresupuesto = estadoPresupuesto;
                presupuestoEgresoPartida.idPresupuestoEgreso = Convert.ToInt32(reader["id_presupuesto_egreso"].ToString());
                presupuestoEgresoPartida.monto = Convert.ToDouble(reader["monto"].ToString());
                presupuestoEgresoPartida.descripcion = reader["descripcion"].ToString();
                presupuestoEgresoPartida.idLinea = Convert.ToInt32(reader["id_linea"].ToString());

                listaPresupuestoEgresoPartida.Add(presupuestoEgresoPartida);
            }

            sqlConnection.Close();

            return listaPresupuestoEgresoPartida;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto: actualiza los datos del presupuesto de egresos partida 
        /// Requiere: presupuesto de egresos partida
        /// Modifica: monto, descripcion y estado
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoEgresoPartida"></param>
        public void actualizarPresupuestoEgreso_Partida(PresupuestoEgresoPartida presupuestoEgresoPartida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update Presupuesto_Egreso_Partida set monto = @monto, descripcion = @descripcion, id_estado_presupuesto = @idEstadoPresupuesto
                                            where id_presupuesto_egreso = @idPresupuestoEgreso and id_partida = @idPartida and id_linea = @idLinea";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idPresupuestoEgreso", presupuestoEgresoPartida.idPresupuestoEgreso);
            command.Parameters.AddWithValue("@idPartida", presupuestoEgresoPartida.partida.idPartida);
            command.Parameters.AddWithValue("@monto", presupuestoEgresoPartida.monto);
            command.Parameters.AddWithValue("@descripcion", presupuestoEgresoPartida.descripcion);
            command.Parameters.AddWithValue("@idEstadoPresupuesto", presupuestoEgresoPartida.estadoPresupuesto.idEstadoPresupuesto);
            command.Parameters.AddWithValue("@idLinea", presupuestoEgresoPartida.idLinea);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto: elimina de la base de datos el presupuesto de egreso partida 
        /// Requiere: presupuesto de egreso partida
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoEgresoPartida"></param>
        public void eliminarPresupuestoEgreso_Partida(PresupuestoEgresoPartida presupuestoEgresoPartida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete Presupuesto_Egreso_Partida where id_presupuesto_egreso = @idPresupuestoEgreso and id_partida = @idPartida and id_linea = @idLinea";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idPresupuestoEgreso", presupuestoEgresoPartida.idPresupuestoEgreso);
            command.Parameters.AddWithValue("@idPartida", presupuestoEgresoPartida.partida.idPartida);
            command.Parameters.AddWithValue("@idLinea", presupuestoEgresoPartida.idLinea);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto: actualiza el estado del presupuesto de egreso partida
        /// Requiere: presupuesto egreso partida
        /// Modifica: el estado
        /// Devuelve: -
        /// </summary>
        /// <param name="presupuestoEgresoPartida"></param>
        public void actualizarEstadoPresupuestoEgreso_Partida(PresupuestoEgresoPartida presupuestoEgresoPartida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update Presupuesto_Egreso_Partida set id_estado_presupuesto = @idEstadoPresupuesto
                                            where id_presupuesto_egreso = @idPresupuestoEgreso and id_partida = @idPartida and id_linea = @idLinea";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idPresupuestoEgreso", presupuestoEgresoPartida.idPresupuestoEgreso);
            command.Parameters.AddWithValue("@idPartida", presupuestoEgresoPartida.partida.idPartida);
            command.Parameters.AddWithValue("@idLinea", presupuestoEgresoPartida.idLinea);
            command.Parameters.AddWithValue("@idEstadoPresupuesto", presupuestoEgresoPartida.estadoPresupuesto.idEstadoPresupuesto);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }
        public List<PresupuestoEgresoPartida> ObtenerPorPartida(string idPartida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartida = new List<PresupuestoEgresoPartida>();

            String consulta = @"select id_presupuesto_egreso,id_partida,monto, descripcion,es.id_estado_presupuesto,id_linea  from Presupuesto_Egreso_Partida Pre ,Estado_presupuestos es

                                 where id_partida=@id_partida_  and pre.id_estado_presupuesto=es.id_estado_presupuesto and pre.id_estado_presupuesto=2;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_partida_", idPartida);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                PresupuestoEgresoPartida presupuestoEgresoPartida = new PresupuestoEgresoPartida();
                EstadoPresupuesto estadoPresupuesto = new EstadoPresupuesto();
                estadoPresupuesto.idEstadoPresupuesto = Convert.ToInt32(reader["id_estado_presupuesto"].ToString());
                presupuestoEgresoPartida.estadoPresupuesto = estadoPresupuesto;
                presupuestoEgresoPartida.idPresupuestoEgreso = Convert.ToInt32(reader["id_presupuesto_egreso"].ToString());
                presupuestoEgresoPartida.monto = Convert.ToDouble(reader["monto"].ToString());
                presupuestoEgresoPartida.descripcion = reader["descripcion"].ToString();
                presupuestoEgresoPartida.idLinea = Convert.ToInt32(reader["id_linea"].ToString());

                listaPresupuestoEgresoPartida.Add(presupuestoEgresoPartida);

            }

            sqlConnection.Close();

            return listaPresupuestoEgresoPartida;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/nov/2019
        /// Efecto: devuelve lista de presupuestos de egresos partidas segun el presupuesto de egresos y descripcion
        /// Requiere: presupuesto de egreso y descripcion
        /// Modifica: -
        /// Devuelve: lista presupuestos egresos partida
        /// </summary>
        /// <param name="presupuestoEgresoConsulta"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public List<PresupuestoEgresoPartida> getPresupuestoEgresoPartidasPorPresupEgresoYDesc(PresupuestoEgreso presupuestoEgresoConsulta,String desc)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartida = new List<PresupuestoEgresoPartida>();

            SqlCommand sqlCommand = new SqlCommand(@"select PEP.*, P.descripcion_partida,P.numero_partida,EP.descripcion_estado_presupuesto 
                                                                                            from Presupuesto_Egreso_Partida PEP, Partida P,Estado_presupuestos EP
                                                                                            where PEP.id_presupuesto_egreso = @idPresupuestoEgreso and PEP.descripcion =@descripcion  and P.id_partida = PEP.id_partida and P.id_partida_padre is not null
                                                                                            and P.ano_periodo = (select ano_periodo from Proyecto where id_proyecto = (
                                                                                            select id_proyecto from Unidad where id_unidad = (select id_unidad from Presupuesto_Egreso where id_presupuesto_egreso = @idPresupuestoEgreso)
                                                                                            )) and P.disponible = 'True' and EP.id_estado_presupuesto = PEP.id_estado_presupuesto", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idPresupuestoEgreso", presupuestoEgresoConsulta.idPresupuestoEgreso);
            sqlCommand.Parameters.AddWithValue("@descripcion", desc);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                PresupuestoEgresoPartida presupuestoEgresoPartida = new PresupuestoEgresoPartida();
                Partida partida = new Partida();
                EstadoPresupuesto estadoPresupuesto = new EstadoPresupuesto();

                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partida.descripcionPartida = reader["descripcion_partida"].ToString();
                partida.numeroPartida = reader["numero_partida"].ToString();

                estadoPresupuesto.idEstadoPresupuesto = Convert.ToInt32(reader["id_estado_presupuesto"].ToString());
                estadoPresupuesto.descripcionEstado = reader["descripcion_estado_presupuesto"].ToString();

                presupuestoEgresoPartida.partida = partida;
                presupuestoEgresoPartida.estadoPresupuesto = estadoPresupuesto;
                presupuestoEgresoPartida.idPresupuestoEgreso = Convert.ToInt32(reader["id_presupuesto_egreso"].ToString());
                presupuestoEgresoPartida.monto = Convert.ToDouble(reader["monto"].ToString());
                presupuestoEgresoPartida.descripcion = reader["descripcion"].ToString();
                presupuestoEgresoPartida.idLinea = Convert.ToInt32(reader["id_linea"].ToString());

                listaPresupuestoEgresoPartida.Add(presupuestoEgresoPartida);
            }

            sqlConnection.Close();

            return listaPresupuestoEgresoPartida;
        }
    }
}