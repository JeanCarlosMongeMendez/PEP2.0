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
    /// 20/sep/2019
    /// Clase para administrar las consultas sql de la entidad de Carga Social
    /// </summary>
    public class CargaSocialDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: obtiene todas las escalas sociales de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de cargas sociales
        /// </summary>
        /// <returns></returns>
        public List<CargaSocial> getCargasSocialesActivas()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<CargaSocial> listaCargasSociales = new List<CargaSocial>();

            String consulta = @"select CS.*, P.numero_partida, P.descripcion_partida
                                            from CargaSocial CS, Partida P 
                                            where CS.activo = 'True' and P.id_partida = CS.id_partida
                                            order by CS.desc_carga_social;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                CargaSocial cargaSocial = new CargaSocial();
                Periodo periodo = new Periodo();
                Partida partida = new Partida();

                periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());

                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partida.numeroPartida = reader["numero_partida"].ToString();
                partida.descripcionPartida = reader["descripcion_partida"].ToString();

                cargaSocial.idCargaSocial = Convert.ToInt32(reader["id_carga_social"].ToString());
                cargaSocial.descCargaSocial = reader["desc_carga_social"].ToString();
                cargaSocial.activo = Convert.ToBoolean(reader["activo"].ToString());
                cargaSocial.porcentajeCargaSocial = Convert.ToDouble(reader["porcentaje_carga_social"].ToString());
                cargaSocial.periodo = periodo;
                cargaSocial.partida = partida;

                listaCargasSociales.Add(cargaSocial);
            }

            sqlConnection.Close();

            return listaCargasSociales;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 20//sep/2019
        /// Efecto: inserta en la base de datos una carga social
        /// Requiere: carga social
        /// Modifica: -
        /// Devuelve: id de la carga social
        /// </summary>
        /// <param name="cargaSocial"></param>
        /// <returns></returns>
        public int insertarCargaSocial(CargaSocial cargaSocial)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"Insert CargaSocial(desc_carga_social,activo,porcentaje_carga_social,id_partida,ano_periodo)
                                            values(@descCargaSocial,@activo,@porcentajeCargaSocial,@idPartida,@anoPeriodo);SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@descCargaSocial", cargaSocial.descCargaSocial);
            command.Parameters.AddWithValue("@activo", cargaSocial.activo);
            command.Parameters.AddWithValue("@porcentajeCargaSocial", cargaSocial.porcentajeCargaSocial);
            command.Parameters.AddWithValue("@idPartida", cargaSocial.partida.idPartida);
            command.Parameters.AddWithValue("@anoPeriodo", cargaSocial.periodo.anoPeriodo);

            sqlConnection.Open();
            int idCargaSocial = Convert.ToInt32(command.ExecuteScalar());
            sqlConnection.Close();

            return idCargaSocial;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: actualiza la carga social
        /// Requiere: carga social
        /// Modifica: la carga social que se encuentra en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="cargaSocial"></param>
        public void actualizarCargaSocial(CargaSocial cargaSocial)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update CargaSocial set desc_carga_social = @descCargaSocial, activo = @activo, porcentaje_carga_social = @porcentajeCargaSocial, id_partida = @idPartida, ano_periodo = @anoPeriodo
                                            where id_carga_social = @idCargaSocial";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idCargaSocial", cargaSocial.idCargaSocial);
            command.Parameters.AddWithValue("@descCargaSocial", cargaSocial.descCargaSocial);
            command.Parameters.AddWithValue("@activo", cargaSocial.activo);
            command.Parameters.AddWithValue("@porcentajeCargaSocial", cargaSocial.porcentajeCargaSocial);
            command.Parameters.AddWithValue("@idPartida", cargaSocial.partida.idPartida);
            command.Parameters.AddWithValue("@anoPeriodo", cargaSocial.periodo.anoPeriodo);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: elimina la carga social
        /// Requiere: carga social
        /// Modifica: base de datos eliminando la carga social
        /// Devuelve: -
        /// </summary>
        /// <param name="cargaSocial"></param>
        public void eliminarCargaSocial(CargaSocial cargaSocial)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"delete from CargaSocial where 
                                            id_carga_social = @idCargaSocial";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idCargaSocial", cargaSocial.idCargaSocial);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: actualiza la carga social de forma logica
        /// Requiere: carga social
        /// Modifica: la carga social que se encuentra en la base de datos cambia el activo e ingresa una nueva carga social
        /// Devuelve: id de craga social nueva
        /// </summary>
        /// <param name="cargaSocial"></param>
        public int actualizarCargaSocialLogica(CargaSocial cargaSocial)
        {
            CargaSocial cargaSocialNueva = new CargaSocial();
            cargaSocialNueva.descCargaSocial = cargaSocial.descCargaSocial;
            cargaSocialNueva.porcentajeCargaSocial = cargaSocial.porcentajeCargaSocial;
            cargaSocialNueva.activo = true;
            cargaSocialNueva.partida = cargaSocial.partida;
            cargaSocialNueva.periodo = cargaSocial.periodo;

            cargaSocial.activo = false;
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update CargaSocial set activo = @activo
                                            where id_carga_social = @idCargaSocial";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idCargaSocial", cargaSocial.idCargaSocial);
            command.Parameters.AddWithValue("@activo", cargaSocial.activo);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();

            return insertarCargaSocial(cargaSocialNueva);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: elimina la carga social de forma logica
        /// Requiere: carga social
        /// Modifica: la carga social que se encuentra en la base de datos cambia el activo 
        /// Devuelve: -
        /// </summary>
        /// <param name="cargaSocial"></param>
        public void eliminarCargaSocialLogica(CargaSocial cargaSocial)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            String consulta = @"update CargaSocial set activo = @activo
                                            where id_carga_social = @idCargaSocial";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);

            command.Parameters.AddWithValue("@idCargaSocial", cargaSocial.idCargaSocial);
            command.Parameters.AddWithValue("@activo", false);

            sqlConnection.Open();
            command.ExecuteReader();
            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 23/sep/2019
        /// Efecto: devuelve una lista de cargas sociales dependiendo del periodo consultado
        /// Requiere: periodo
        /// Modifica: -
        /// Devuelve: lista de cargas sociales
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<CargaSocial> getCargasSocialesActivasPorPeriodo(Periodo periodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<CargaSocial> listaCargasSociales = new List<CargaSocial>();

            String consulta = @"select CS.*, P.numero_partida, P.descripcion_partida
                                            from CargaSocial CS, Partida P 
                                            where CS.ano_periodo = @anoPeriodo and CS.activo = 'True' and P.id_partida = CS.id_partida
                                            order by CS.desc_carga_social;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@anoPeriodo", periodo.anoPeriodo);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                CargaSocial cargaSocial = new CargaSocial();
                Periodo periodoBD = new Periodo();
                Partida partida = new Partida();

                periodoBD.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());

                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partida.numeroPartida = reader["numero_partida"].ToString();
                partida.descripcionPartida = reader["descripcion_partida"].ToString();

                cargaSocial.idCargaSocial = Convert.ToInt32(reader["id_carga_social"].ToString());
                cargaSocial.descCargaSocial = reader["desc_carga_social"].ToString();
                cargaSocial.activo = Convert.ToBoolean(reader["activo"].ToString());
                cargaSocial.porcentajeCargaSocial = Convert.ToDouble(reader["porcentaje_carga_social"].ToString());
                cargaSocial.periodo = periodoBD;
                cargaSocial.partida = partida;

                listaCargasSociales.Add(cargaSocial);
            }

            sqlConnection.Close();

            return listaCargasSociales;
        }

    }
}