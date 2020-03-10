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
        public List<Partida> ObtenerPorPeriodo(int anoPeriodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Partida> partidas = new List<Partida>();

            String consulta = @"select ph.id_partida, ph.numero_partida, ph.descripcion_partida, ph.id_partida_padre,ph.esUCR, ph.ano_periodo, pp.numero_partida AS numero_partida_padre, pp.descripcion_partida AS descripcion_padre from Partida ph left join Partida pp ON ph.id_partida_padre = pp.id_partida 

where ph.ano_periodo=@ano_periodo_ AND ph.disponible=1 order by ph.numero_partida;";

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
                partida.esUCR = Boolean.Parse(reader["esUCR"].ToString());
                partida.periodo = new Periodo();
                partida.periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());

                //Si la partida padre contiene un valor se le agrega, sino se deja como nulo
                if (!DBNull.Value.Equals(reader["id_partida_padre"]))
                {
                    partida.partidaPadre = new Partida();
                    partida.partidaPadre.idPartida = Convert.ToInt32(reader["id_partida_padre"].ToString());
                    partida.partidaPadre.descripcionPartida = reader["descripcion_padre"].ToString();
                    partida.partidaPadre.numeroPartida = reader["numero_partida_padre"].ToString();

                }
                else
                {
                    partida.partidaPadre = null;
                }

                partidas.Add(partida);
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
                        "insert into Partida(numero_partida, descripcion_partida, id_partida_padre, ano_periodo, disponible,esUCR) " +
                        "output INSERTED.id_partida values(@numero_partida_, @descripcion_partida_, @id_partida_padre_, @ano_periodo_, @disponible_,@esUCR_);", sqlConnection);
                    sqlCommandHija.Parameters.AddWithValue("@numero_partida_", partida.numeroPartida);
                    sqlCommandHija.Parameters.AddWithValue("@descripcion_partida_", partida.descripcionPartida);
                    sqlCommandHija.Parameters.AddWithValue("@ano_periodo_", partida.periodo.anoPeriodo);
                    sqlCommandHija.Parameters.AddWithValue("@id_partida_padre_", partida.partidaPadre.idPartida);
                    sqlCommandHija.Parameters.AddWithValue("@disponible_", 1);
                    sqlCommandHija.Parameters.AddWithValue("@esUCR_", partida.esUCR);


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
                "insert into Partida(numero_partida, descripcion_partida, ano_periodo,disponible,esUCR) " +
                "output INSERTED.id_partida values(@numero_partida_, @descripcion_partida_, @ano_periodo_, @disponible_,@esUCR_);", sqlConnection);
                    sqlCommandPadre.Parameters.AddWithValue("@numero_partida_", partida.numeroPartida);
                    sqlCommandPadre.Parameters.AddWithValue("@descripcion_partida_", partida.descripcionPartida);
                    sqlCommandPadre.Parameters.AddWithValue("@ano_periodo_", partida.periodo.anoPeriodo);
                    sqlCommandPadre.Parameters.AddWithValue("@disponible_", 1);
                    sqlCommandPadre.Parameters.AddWithValue("@esUCR_", partida.esUCR);
                    sqlConnection.Open();
                    resultado = (int)sqlCommandPadre.ExecuteScalar();
                }
                else
                {
                    sqlConnection.Close();
                    sqlConnection.Open();
                    resultado = (int)sqlConsultaDuplicadosPadre.ExecuteScalar();
                }

                sqlConnection.Close();
            }

            return resultado;
        }




        /// <summary>
        /// Obtiene una partida basado en su identificador
        /// </summary>
        /// <param name="idPartida">Valor de tipo <code>int</code> que corresponde a la partida a buscar</param>
        /// <returns>Retorna el elemento de tipo <code>Partida</code> que coincida con el identificador dado</returns>
        public Partida ObtenerPorId(int idPartida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("select id_partida, numero_partida, descripcion_partida,esUCR ,id_partida_padre, ano_periodo " +
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
                partida.esUCR = Convert.ToBoolean(reader["esUCR"].ToString());

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
        /// Obtiene una partida basado en su identificador
        /// </summary>
        /// <param name="tipoPartida">Valor de tipo <code>int</code> que corresponde a la partida a buscar</param>
        /// <returns>Retorna el elemento de tipo <code>Partida</code> que coincida con el identificador dado</returns>
        public List<Partida> obtenerPorTipoPartidaYPeriodo(bool tipoPartida, int anioPeriodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Partida> partidas = new List<Partida>();

            String consulta = @"select id_partida, numero_partida, descripcion_partida, id_partida_padre, ano_periodo,esUCR from Partida where esUCR=@esUCR AND ano_periodo=@ano_periodo AND disponible=1 order by numero_partida;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@esUCR", tipoPartida);
            sqlCommand.Parameters.AddWithValue("@ano_periodo", anioPeriodo);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Partida partida = new Partida();
                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partida.numeroPartida = reader["numero_partida"].ToString();
                partida.descripcionPartida = reader["descripcion_partida"].ToString();
                partida.esUCR = Convert.ToBoolean(reader["esUCR"].ToString());
                partida.periodo = new Periodo();
                partida.periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());

                partidas.Add(partida);


            }
            return partidas;
        }


        /// <summary>
        /// Josseline M
        /// Este método se encarga de obtener las partidas en funcion al proyecto, periodo y unidades seleccionadas
        /// </summary>
        /// <param name="proyecto"></param>
        /// <param name="unidades"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<Partida> ObtienePartidaPorPeriodoUnidadProyecto(int proyecto, LinkedList<int> unidades, int periodo)
        {
            List<Partida> partidas = new List<Partida>();
            SqlConnection sqlConnection = conexion.conexionPEP();
            foreach (int idUnidad in unidades)
            {
                SqlCommand sqlCommand = new SqlCommand("select Partida.id_partida, numero_partida, descripcion_partida,esUCR ,PE.monto" +
               " from Proyecto,Unidad,Periodo,Partida,Presupuesto_Egreso_Partida PE ,Presupuesto_egreso P" +
               " where Periodo.ano_periodo=@periodo and  Proyecto.ano_periodo=Periodo.ano_periodo AND Partida.ano_periodo=Proyecto.ano_periodo" +
               " and Unidad.id_proyecto=Proyecto.id_proyecto and Proyecto.id_proyecto = @proyecto and Unidad.id_unidad=@unidad and Partida.id_partida=PE.id_partida  and Partida.disponible=1 and P.id_presupuesto_egreso=PE.id_presupuesto_egreso and Unidad.id_unidad=P.id_unidad and PE.id_estado_presupuesto=2  order by numero_partida;", sqlConnection);

                //SqlCommand sqlCommand = new SqlCommand("select id_partida,numero_partida,descripcion_partida,esUCR" +
                //    "from Presupuesto_Egreso PE,Proyecto P,Unidad U"+
                //    "where PE.id_unidad=@unidad and U.id_unidad=Pe.id_unidad and U.id_proyecto=@proyecto  and P.id_proyecto=U.id_peroyecto and P.ano_periodo=@periodo ;",sqlConnection);
                sqlCommand.Parameters.AddWithValue("@periodo", periodo);
                sqlCommand.Parameters.AddWithValue("@proyecto", proyecto);
                sqlCommand.Parameters.AddWithValue("@unidad", idUnidad);
                SqlDataReader reader;
                sqlConnection.Open();
                reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    Partida partida = new Partida();
                    PresupuestoEgreso presupuestoEgreso = new PresupuestoEgreso();
                    partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                    partida.numeroPartida = reader["numero_partida"].ToString();
                    partida.descripcionPartida = reader["descripcion_partida"].ToString();
                    partida.esUCR = Convert.ToBoolean(reader["esUCR"].ToString());
                    partida.idUnidad = idUnidad;
                    double montoTotal= Convert.ToDouble(reader["monto"].ToString());

                    //if (montoTotal != 0)
                    //{
                        List<Partida> listaTemp = partidas.Where(partidaBD => partidaBD.idPartida == partida.idPartida).ToList();

                        if (listaTemp.Count == 0)
                            partidas.Add(partida);
                    //}
                }

                sqlConnection.Close();
            }


            return partidas;
        }

        // <summary>
        // Adrián Serrano
        // 7/14/2019
        // Efecto: Elimina una partida de forma logica de la base de datos
        // Requiere: Partida
        // Modifica: -
        // Devuelve: -
        // Editado por: Jesus Torres
        // 11/oct/2019
        // Se cambia la consulta a BD, de manera que realice el eliminado logico, de partidas con el ID del padre y todas sus hijas
        // </summary>
        // <param name="idPartida"></param>
        public void EliminarPartida(int idPartida, int periodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand("update Partida set disponible=0 where (id_partida = @id_partida_ OR id_partida_padre = @id_partida_) AND ano_periodo = @periodo_;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_partida_", idPartida);
            sqlCommand.Parameters.AddWithValue("@periodo_", periodo);

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
                partidaBD.esUCR = Convert.ToBoolean(reader["esUCR"].ToString());

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


        /// <summary>
        /// Jesus Torres
        /// 10/oct/2019
        /// Efecto: busca en base de datos las partidas con igual id de padre al parametro enviadop
        /// Requiere:  id de partida padre
        /// Modifica: 
        /// Devuelve: -
        /// <param name="idPartidaPadre">Valor de tipo <code>int</code> que corresponde a la partida padre a buscar</param>
        /// <returns>Retorna el elemento de tipo <code>Partida</code> que coincida con el identificador dado</returns>
        public List<Partida> obtenerPorIdPartidaPadre(int idPartidaPadre, int periodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Partida> partidas = new List<Partida>();

            String consulta = @"select id_partida, numero_partida, descripcion_partida, id_partida_padre, ano_periodo,esUCR from Partida where id_partida_padre=@id_partida_padre AND ano_periodo=@ano_periodo AND disponible=1 order by numero_partida;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_partida_padre", idPartidaPadre);
            sqlCommand.Parameters.AddWithValue("@ano_periodo", periodo);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Partida partida = new Partida();
                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partida.numeroPartida = reader["numero_partida"].ToString();
                partida.descripcionPartida = reader["descripcion_partida"].ToString();
                partida.esUCR = Convert.ToBoolean(reader["esUCR"].ToString());
                partida.periodo = new Periodo();
                partida.periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());

                partidas.Add(partida);


            }
            return partidas;
        }


        /// Josseline M
        /// Obtiene una partida a partir en su numeroPartida
        /// </summary>
        /// <param name="idPartida">Valor de tipo <code>int</code> que corresponde a la partida a buscar</param>
        /// <returns>Retorna el elemento de tipo <code>Partida</code> que coincida con el identificador dado</returns>
        public Partida ObtenerPorNumeroPartida(string numeroPartida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("select id_partida, numero_partida, descripcion_partida, id_partida_padre, ano_periodo, esUCR " +
                "from Partida where numero_partida=@numero_partida_ AND disponible=1 order by numero_partida;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@numero_partida_", numeroPartida);

            Partida partida = new Partida();


            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();



            if (reader.Read())
            {
                partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                partida.numeroPartida = reader["numero_partida"].ToString();
                partida.descripcionPartida = reader["descripcion_partida"].ToString();
                partida.esUCR = Convert.ToBoolean(reader["esUCR"].ToString());

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
        public Partida ObtenerPorIdYNumeroPartida(int idPartida, string numeroPartida)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("select id_partida, numero_partida, descripcion_partida, id_partida_padre, ano_periodo, esUCR " +
                "from Partida where id_partida=@id_partida_ and numero_partida=@numero_partida_ AND disponible=1 order by numero_partida;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@numero_partida_", numeroPartida);
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
                partida.esUCR = Convert.ToBoolean(reader["esUCR"].ToString());

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
        public Partida ObtienePartidaPorPeriodoUnidadProyectoYNumeroUnidad(int proyecto,int idUnidad, int periodo, string numeroPartida)
        {
            Partida partida = new Partida();
            SqlConnection sqlConnection = conexion.conexionPEP();
            
                SqlCommand sqlCommand = new SqlCommand("select id_partida,Partida.numero_partida, descripcion_partida,esUCR " +
               "from Proyecto,Unidad,Periodo,Partida " +
               "where Periodo.ano_periodo=@periodo and  Proyecto.ano_periodo=Periodo.ano_periodo AND Partida.ano_periodo=Proyecto.ano_periodo" +
               " and Unidad.id_proyecto=Proyecto.id_proyecto and Proyecto.id_proyecto = @proyecto and Unidad.id_unidad=@unidad and Partida.numero_partida=@numeroPartida and Partida.disponible=1  order by numero_partida;", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@periodo", periodo);
                sqlCommand.Parameters.AddWithValue("@proyecto", proyecto);
                sqlCommand.Parameters.AddWithValue("@unidad", idUnidad);
                sqlCommand.Parameters.AddWithValue("@numeroPartida", numeroPartida);
                 SqlDataReader reader;
                sqlConnection.Open();
                reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                   
                    partida.idPartida = Convert.ToInt32(reader["id_partida"].ToString());
                    partida.numeroPartida = reader["numero_partida"].ToString();
                    partida.descripcionPartida = reader["descripcion_partida"].ToString();
                    partida.esUCR = Convert.ToBoolean(reader["esUCR"].ToString());
                    partida.idUnidad = idUnidad;

                    //List<Partida> listaTemp = partida;

                    
                       
                }

                sqlConnection.Close();
            


            return partida;
        }
    }
}
