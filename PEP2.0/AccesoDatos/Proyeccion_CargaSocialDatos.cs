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
    /// 04/nov/2019
    /// Clase para administrar las consultas sql de la entidad de Proyeccion_CargaSocial
    /// </summary>
    public class Proyeccion_CargaSocialDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: inserta en la base de datos la relacion de proyeccion y carga social
        /// Requiere: proyeccion, carga social y monto
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="proyeccion"></param>
        /// <returns></returns>
        public void insertarProyeccionCargaSocial(Proyeccion_CargaSocial proyeccion_CargaSocial)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"Insert Proyeccion_CargaSocial(id_proyeccion,id_carga_social,monto)
                                            values(@idProyeccion,@idCargaSocial,@monto)";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idProyeccion", proyeccion_CargaSocial.proyeccion.idProyeccion);
            command.Parameters.AddWithValue("@idCargaSocial", proyeccion_CargaSocial.cargaSocial.idCargaSocial);
            command.Parameters.AddWithValue("@monto", proyeccion_CargaSocial.monto);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: elimina la asociacion de proyeccion y carga social
        /// Requiere: proyeccion
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="periodo"></param>
        public void eliminarProyeccionCargaSocialPorProyeccion(Proyeccion proyeccion)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete from Proyeccion_CargaSocial where 
                                            id_proyeccion = @idProyeccion";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idProyeccion", proyeccion.idProyeccion);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 07/nov/2019
        /// Efecto: devuelve lista de proyeccion_cargas sociales segun la proyeccion ingresada
        /// Requiere: proyeccion a consultar
        /// Modifica: -
        /// Devuelve: lista proyecion_cargasSociales
        /// </summary>
        /// <param name="proyeccionConsulta"></param>
        /// <returns></returns>
        public List<Proyeccion_CargaSocial> getProyeccionCargaSocialPorProyeccionPorProyeccion(Proyeccion proyeccionConsulta)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Proyeccion_CargaSocial> listaProyeccionesCargaSociales = new List<Proyeccion_CargaSocial>();

            String consulta = @"select PC.*,P.ano_periodo,C.id_partida
                                            from Proyeccion_CargaSocial PC, Proyeccion P, CargaSocial C
                                            where PC.id_proyeccion = @idProyeccion and P.id_proyeccion = PC.id_proyeccion and C.id_carga_social = PC.id_carga_social;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idProyeccion", proyeccionConsulta.idProyeccion);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Proyeccion_CargaSocial proyeccion_CargaSocial = new Proyeccion_CargaSocial();
                Proyeccion proyeccion = new Proyeccion();
                CargaSocial cargaSocial = new CargaSocial();
                Periodo periodo = new Periodo();
                Partida partida = new Partida();

                proyeccion.idProyeccion = Convert.ToInt32(reader["id_proyeccion"].ToString());
                periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());
                proyeccion.periodo = periodo;
                cargaSocial.idCargaSocial = Convert.ToInt32(reader["id_carga_social"].ToString());
                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                cargaSocial.partida = partida;

                proyeccion_CargaSocial.proyeccion = proyeccion;
                proyeccion_CargaSocial.cargaSocial = cargaSocial;
                proyeccion_CargaSocial.monto = Convert.ToDouble(reader["monto"].ToString());

                listaProyeccionesCargaSociales.Add(proyeccion_CargaSocial);
            }

            sqlConnection.Close();

            return listaProyeccionesCargaSociales;
        }

    }
}
