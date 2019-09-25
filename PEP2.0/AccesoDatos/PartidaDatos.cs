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
    /// Adrián Serrano
    /// 27/may/2019
    /// Clase para administrar el CRUD para las Partidas
    /// </summary>
    public class PartidaDatos
    {
        private ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Obtiene las partidas de la base de datos segun el periodo
        /// </summary>
        /// <returns>Retorna una lista <code>LinkedList<Partida></code> que contiene las partidas</returns>
        public LinkedList<Partida> ObtenerPorPeriodo(int anoPeriodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            LinkedList<Partida> partidas = new LinkedList<Partida>();

            String consulta = @"select id_partida, numero_partida, descripcion_partida, id_partida_padre from Partida where ano_periodo=@ano_periodo_ AND disponible=1 order by numero_partida;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@ano_periodo_", anoPeriodo);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Partida partida = new Partida();
                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partida.numeroPartida = reader["numero_partida"].ToString();
                partida.descripcionPartida = reader["descripcion_partida"].ToString();

                //Si la partida padre contiene un valor se le agrega, sino se deja como nulo
                if (!DBNull.Value.Equals(reader["id_partida_padre"]))
                {
                    partida.partidaPadre = new Partida();
                    partida.partidaPadre.idPartida = Convert.ToInt32(reader["id_partida_padre"].ToString());
                }else
                {
                    partida.partidaPadre = null;
                }

                partidas.AddLast(partida);
            }

            sqlConnection.Close();

            return partidas;
        }
        
        /// <summary>
        /// Inserta las partidas
        /// </summary>
        /// <param name="partida">La partida que se desea insertar</param>
        public int Insertar(Partida partida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            int resultado = 0;

            if (partida.partidaPadre != null) //Es hija
            {
                SqlCommand sqlConsultaDuplicadosHija = new SqlCommand("select id_partida from Partida where numero_partida=@numero_partida_ AND id_partida_padre=@id_partida_padre_ AND ano_periodo=@ano_periodo_ AND disponible=1;", sqlConnection);
                sqlConsultaDuplicadosHija.Parameters.AddWithValue("@numero_partida_", partida.numeroPartida);
                sqlConsultaDuplicadosHija.Parameters.AddWithValue("@id_partida_padre_", partida.partidaPadre.idPartida);
                sqlConsultaDuplicadosHija.Parameters.AddWithValue("@ano_periodo_", partida.periodo.anoPeriodo);

                SqlDataReader readerHija;
                sqlConnection.Open();

                readerHija = sqlConsultaDuplicadosHija.ExecuteReader();

                if (!readerHija.Read())
                {
                    sqlConnection.Close();

                    SqlCommand sqlCommandHija = new SqlCommand(
                        "insert into Partida(numero_partida, descripcion_partida, id_partida_padre, ano_periodo, disponible) " +
                        "output INSERTED.id_partida values(@numero_partida_, @descripcion_partida_, @id_partida_padre_, @ano_periodo_, @disponible_);", sqlConnection);
                    sqlCommandHija.Parameters.AddWithValue("@numero_partida_", partida.numeroPartida);
                    sqlCommandHija.Parameters.AddWithValue("@descripcion_partida_", partida.descripcionPartida);
                    sqlCommandHija.Parameters.AddWithValue("@ano_periodo_", partida.periodo.anoPeriodo);
                    sqlCommandHija.Parameters.AddWithValue("@id_partida_padre_", partida.partidaPadre.idPartida);
                    sqlCommandHija.Parameters.AddWithValue("@disponible_", 1);

                    sqlConnection.Open();
                    resultado = (int)sqlCommandHija.ExecuteScalar();
                }
                else
                {
                    resultado = -1;
                }

                sqlConnection.Close();
            }
            else //Es padre
            {
                SqlCommand sqlConsultaDuplicadosPadre = new SqlCommand("select id_partida from Partida where numero_partida=@numero_partida_ AND ano_periodo=@ano_periodo_ AND disponible=1;", sqlConnection);
                sqlConsultaDuplicadosPadre.Parameters.AddWithValue("@numero_partida_", partida.numeroPartida);
                sqlConsultaDuplicadosPadre.Parameters.AddWithValue("@ano_periodo_", partida.periodo.anoPeriodo);

                SqlDataReader readerPadre;
                sqlConnection.Open();

                readerPadre = sqlConsultaDuplicadosPadre.ExecuteReader();

                if (!readerPadre.Read())
                {
                    sqlConnection.Close();

                    SqlCommand sqlCommandPadre = new SqlCommand(
                "insert into Partida(numero_partida, descripcion_partida, ano_periodo, disponible) " +
                "output INSERTED.id_partida values(@numero_partida_, @descripcion_partida_, @ano_periodo_, @disponible_);", sqlConnection);
                    sqlCommandPadre.Parameters.AddWithValue("@numero_partida_", partida.numeroPartida);
                    sqlCommandPadre.Parameters.AddWithValue("@descripcion_partida_", partida.descripcionPartida);
                    sqlCommandPadre.Parameters.AddWithValue("@ano_periodo_", partida.periodo.anoPeriodo);
                    sqlCommandPadre.Parameters.AddWithValue("@disponible_", 1);

                    sqlConnection.Open();
                    resultado = (int)sqlCommandPadre.ExecuteScalar();
                }

                sqlConnection.Close();
            }

            return resultado;
        }

        /// <summary>
        /// Recibe un numero de partida y un ano de periodo, si devuelve un ID significa que ya la partida padre existe y no se debe insertar de nuevo
        /// </summary>
        /// <param name="numeroPartida">Valor de tipo <code>int</code> que corresponde al numero de la partida</param>
        /// <param name="anoPeriodo">Valor de tipo <code>int</code> que corresponde al ano del periodo</param>
        /// <returns>Retorna el elemento de tipo <code>int</code> que coincida con los parametros dados</returns>
        public int PadresDuplicados(string numeroPartida, int anoPeriodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("select id_partida from Partida where numero_partida=@numero_partida_ AND ano_periodo=@ano_periodo_ AND disponible=1;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@numero_partida_", numeroPartida);
            sqlCommand.Parameters.AddWithValue("@ano_periodo_", anoPeriodo);

            int idPartida = 0;
            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                idPartida = Convert.ToInt32(reader["id_partida"].ToString());
            }

            sqlConnection.Close();

            return idPartida;
        }

        /// <summary>
        /// Obtiene una partida basado en su identificador
        /// </summary>
        /// <param name="idPartida">Valor de tipo <code>int</code> que corresponde a la partida a buscar</param>
        /// <returns>Retorna el elemento de tipo <code>Partida</code> que coincida con el identificador dado</returns>
        public Partida ObtenerPorId(int idPartida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("select id_partida, numero_partida, descripcion_partida, id_partida_padre, ano_periodo " +
                "from Partida where id_partida=@id_partida_ AND disponible=1 order by numero_partida;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_partida_", idPartida);

            Partida partida = new Partida();

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partida.numeroPartida = reader["numero_partida"].ToString();
                partida.descripcionPartida = reader["descripcion_partida"].ToString();

                //Si la partida padre contiene un valor se le agrega, sino se deja como nulo
                if (!DBNull.Value.Equals(reader["id_partida_padre"]))
                {
                    partida.partidaPadre = new Partida();
                    partida.partidaPadre.idPartida = Convert.ToInt32(reader["id_partida_padre"].ToString());
                }
                else
                {
                    partida.partidaPadre = null;
                }

                partida.periodo = new Periodo();
                partida.periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());
            }

            sqlConnection.Close();

            return partida;
        }

        /// <summary>
        /// Transfiere las partidas seleccionadas de un periodo a otro
        /// </summary>
        /// <param name="partidasId">Lista de identificadores de las partidas que se desean transferir al nuevo periodo</param>
        /// <param name="anoPeriodo">Nuevo periodo a los que las partidas serán transferidas</param>
        /// <returns>Retorna si las partidas y los periodos fueron guardados</returns>
        public bool Guardar(LinkedList<int> partidasId, int anoPeriodo)
        {
            //transferir padre tambien cuando transfiero las hijas

            SqlConnection sqlConnection = conexion.conexionPEP();
            bool guardado = false;
            int nuevoId = 0;
            int nuevoIdPadre = 0;

            foreach (int idPartida in partidasId)
            {
                Partida partida = ObtenerPorId(idPartida);

                /*
                 * Verifica si el periodo de las partidas que se desean guardar es igual al periodo indicado,
                 * esto para evitar partidas duplicadas en el mismo periodo
                 * De lo contrario asigna el nuevo periodo a la partida para duplicar su información cambiando el periodo al que pertenece
                 */
                if (partida.periodo.anoPeriodo != anoPeriodo)
                {
                    //Duplicar tambien los padres
                    Partida partidaPadre = ObtenerPorId(partida.partidaPadre.idPartida);
                    partidaPadre.periodo.anoPeriodo = anoPeriodo;

                    nuevoIdPadre = PadresDuplicados(partidaPadre.numeroPartida, anoPeriodo);

                    if (nuevoIdPadre == 0)
                    {
                        nuevoIdPadre = Insertar(partidaPadre);
                    }

                    partidaPadre.idPartida = nuevoIdPadre;
                    
                    partida.periodo.anoPeriodo = anoPeriodo;
                    partida.partidaPadre = partidaPadre;
                    nuevoId = Insertar(partida);

                    if (nuevoId > 0)
                    {
                        guardado = true;
                    }
                    else
                    {
                        guardado = false;
                    }
                }
            }

            return guardado;
        }

        // <summary>
        // Adrián Serrano
        // 7/14/2019
        // Efecto: Elimina una partida de forma logica de la base de datos
        // Requiere: Partida
        // Modifica: -
        // Devuelve: -
        // </summary>
        // <param name="idPartida"></param>
        public void EliminarPartida(int idPartida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand("update Partida set disponible=0 where id_partida=@id_partida_;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_partida_", idPartida);

            sqlConnection.Open();

            sqlCommand.ExecuteScalar();

            sqlConnection.Close();
        }

        // <summary>
        // Adrián Serrano
        // 7/21/2019
        // Efecto: Actualiza una partida de la base de datos
        // Requiere: Partida
        // Modifica: -
        // Devuelve: -
        // </summary>
        // <param name="Proyecto"></param>
        public void ActualizarPartida(Partida partida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand("update Partida set numero_partida=@numero_partida_, descripcion_partida=@descripcion_partida_ where id_partida=@id_partida_;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_partida_", partida.idPartida);
            sqlCommand.Parameters.AddWithValue("@numero_partida_", partida.numeroPartida);
            sqlCommand.Parameters.AddWithValue("@descripcion_partida_", partida.descripcionPartida);

            sqlConnection.Open();

            sqlCommand.ExecuteScalar();

            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 25/sep/2019
        /// Efecto: devuelve la partida que cumple con los datos ingresados de numero de partida y periodo
        /// Requiere: partida y periodo
        /// Modifica: -
        /// Devuelve: partida
        /// </summary>
        /// <param name="partida"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public Partida getPartidaPorNumeroYPeriodo(Partida partida, Periodo periodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            Partida partidaBD = new Partida();

            String consulta = @"select * from Partida
                                            where numero_partida = @numeroPartida and ano_periodo = @anoPeriodo";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@numeroPartida", partida.numeroPartida);
            sqlCommand.Parameters.AddWithValue("@anoPeriodo", periodo.anoPeriodo);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                partidaBD.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partidaBD.numeroPartida = reader["numero_partida"].ToString();
                partidaBD.descripcionPartida = reader["descripcion_partida"].ToString();

                //Si la partida padre contiene un valor se le agrega, sino se deja como nulo
                if (!DBNull.Value.Equals(reader["id_partida_padre"]))
                {
                    partidaBD.partidaPadre = new Partida();
                    partidaBD.partidaPadre.idPartida = Convert.ToInt32(reader["id_partida_padre"].ToString());
                }
                else
                {
                    partidaBD.partidaPadre = null;
                }

                partidaBD.periodo = new Periodo();
                partidaBD.periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());
            }

            sqlConnection.Close();

            return partidaBD;
        }
    }
}
