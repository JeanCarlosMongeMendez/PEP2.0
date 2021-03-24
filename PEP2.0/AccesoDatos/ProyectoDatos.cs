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
    /// 17/may/2019
    /// Clase para administrar el CRUD para los proyectos
    /// </summary>
    public class ProyectoDatos
    {
        private ConexionDatos conexion = new ConexionDatos();
        private UnidadDatos unidadDatos = new UnidadDatos();

        /// <summary>
        /// Obtiene todos los proyectos del periodo especificado
        /// </summary>
        /// <param name="anoPeriodo">Valor de tipo <code>int</code> para filtrar la búsqueda</param>
        /// <returns>Retorna la lista <code>LinkedList<Proyecto></code> que contiene los proyectos que correspondan al periodo especificado</returns>
        public LinkedList<Proyectos> ObtenerPorPeriodo(int anoPeriodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            LinkedList<Proyectos> proyectos = new LinkedList<Proyectos>();

            SqlCommand sqlCommand = new SqlCommand("select id_proyecto, nombre_proyecto, codigo, es_UCR" +
                " from Proyecto where ano_periodo=@ano_periodo_ AND disponible=1; ", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@ano_periodo_", anoPeriodo);

            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Proyectos proyecto = new Proyectos();
                proyecto.idProyecto = Convert.ToInt32(reader["id_proyecto"].ToString());
                proyecto.nombreProyecto = reader["nombre_proyecto"].ToString();
                proyecto.codigo = reader["codigo"].ToString();
                proyecto.esUCR = Boolean.Parse(reader["es_UCR"].ToString());
                proyectos.AddLast(proyecto);
            }

            sqlConnection.Close();

            return proyectos;
        }

        /// <summary>
        /// Obtiene un proyecto basado en su identificador
        /// </summary>
        /// <param name="idProyecto">Valor de tipo <code>int</code> que corresponde al proyecto a buscar</param>
        /// <returns>Retorna el elemento de tipo <code>Proyecto</code> que coincida con el identificador dado</returns>
        public Proyectos ObtenerPorId(int idProyecto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            SqlCommand sqlCommand = new SqlCommand("select id_proyecto, nombre_proyecto, codigo, es_UCR, ano_periodo " +
                "from Proyecto where id_proyecto=@id_proyecto_ AND disponible=1;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_proyecto_", idProyecto);

            Proyectos proyecto = new Proyectos();

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                proyecto.idProyecto = Convert.ToInt32(reader["id_proyecto"].ToString());
                proyecto.nombreProyecto = reader["nombre_proyecto"].ToString();
                proyecto.codigo = reader["codigo"].ToString();
                proyecto.esUCR = Boolean.Parse(reader["es_UCR"].ToString());
                proyecto.periodo = new Periodo();
                proyecto.periodo.anoPeriodo = Convert.ToInt32(reader["ano_periodo"].ToString());
            }

            sqlConnection.Close();

            return proyecto;
        }


        /// <summary>
        /// Inserta la entidad Proyecto en la base de datos
        /// </summary>
        /// <param name="proyecto">Elemento de tipo <code>Proyecto</code> que va a ser insertado</param>
        /// <returns>Retorna un valor <code>int</code> con el identificador del proyecto insertado</returns>
        public int Insertar(Proyectos proyecto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            String consulta = @"Insert Proyecto(nombre_proyecto, codigo, es_UCR, ano_periodo, disponible) values(@nombre_proyecto, @codigo, @es_UCR, @ano_periodo, @disponible); select scope_identity()";

            SqlCommand command = new SqlCommand(consulta, sqlConnection);
            command.Parameters.AddWithValue("@nombre_proyecto", proyecto.nombreProyecto);
            command.Parameters.AddWithValue("@codigo", proyecto.codigo);
            command.Parameters.AddWithValue("@es_UCR", proyecto.esUCR);
            command.Parameters.AddWithValue("@ano_periodo", proyecto.periodo.anoPeriodo);
            command.Parameters.AddWithValue("@disponible", 1);

            sqlConnection.Open();
            int idProyecto = Convert.ToInt32(command.ExecuteScalar());
            return idProyecto;
        }


        public int InsertarProyecto(Proyectos proyecto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            int respuesta = 0;

            SqlCommand sqlConsultaDuplicados = new SqlCommand("select id_proyecto from Proyecto where codigo=@codigo_ AND ano_periodo=@ano_periodo_ AND disponible=1;", sqlConnection);
            sqlConsultaDuplicados.Parameters.AddWithValue("@codigo_", proyecto.codigo);
            sqlConsultaDuplicados.Parameters.AddWithValue("@ano_periodo_", proyecto.periodo.anoPeriodo);

            SqlDataReader reader;
            sqlConnection.Open();

            reader = sqlConsultaDuplicados.ExecuteReader();

            if (!reader.Read())
            {
                sqlConnection.Close();

                SqlCommand sqlCommand = new SqlCommand("insert into Proyecto(nombre_proyecto, codigo, es_UCR, ano_periodo, disponible)" +
                " output INSERTED.id_proyecto values(@nombre_proyecto_, @codigo_, @es_UCR_, @ano_periodo_, @disponible_);", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@nombre_proyecto_", proyecto.nombreProyecto);
                sqlCommand.Parameters.AddWithValue("@codigo_", proyecto.codigo);
                sqlCommand.Parameters.AddWithValue("@es_UCR_", proyecto.esUCR);
                sqlCommand.Parameters.AddWithValue("@ano_periodo_", proyecto.periodo.anoPeriodo);
                sqlCommand.Parameters.AddWithValue("@disponible_", 1);

                sqlConnection.Open();
                respuesta = (int)sqlCommand.ExecuteScalar();

                //Si el proyecto creado es de tipo FUNDEVI, se crea una unidad con valores por defecto del mismo nombre del proyecto
                if (!proyecto.esUCR)
                {
                    Unidad unidad = new Unidad();
                    unidad.nombreUnidad = proyecto.nombreProyecto;
                    unidad.coordinador = "Sin coordinador";
                    unidad.proyecto = new Proyectos();
                    unidad.proyecto.idProyecto = respuesta;
                    this.unidadDatos.Insertar(unidad);
                }
            }
            else
            {
                //Si retorna -1 significa que ya existe un proyecto con el mismo codigo en el mismo periodo
                respuesta = -1;
            }

            sqlConnection.Close();

            return respuesta;

        }

        /// <summary>
        /// Transfiere los proyectos seleccionados de un periodo a otro, así tambien sus unidades
        /// </summary>
        /// <param name="proyectosId">Lista de identificadores de los proyectos que se desean transferir al nuevo periodo</param>
        /// <param name="anoPeriodo">Nuevo periodo a los que los proyectos serán transferidos</param>
        /// <returns>Retorna si los proyectos y los periodos fueron guardados</returns>
        public int Guardar(LinkedList<int> proyectosId, int anoPeriodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            int respuesta = 0;

            foreach (int idProyecto in proyectosId)
            {
                Proyectos proyecto = ObtenerPorId(idProyecto);

                if (proyecto.periodo.anoPeriodo != anoPeriodo)
                {
                    /**
                     * Verifica si el periodo de los proyectos que se desean guardar es igual al periodo indicado,
                     * esto para evitar proyectos duplicados en el mismo periodo
                     * De lo contrario asigna el nuevo periodo al proyecto para duplicar su información cambiando el periodo al que pertenece
                     **/
                    proyecto.periodo.anoPeriodo = anoPeriodo;

                    respuesta = Insertar(proyecto);

                    if (respuesta > 0)
                    {
                        if (proyecto.esUCR)
                        {
                            List<Unidad> unidades = new List<Unidad>();
                            unidades = this.unidadDatos.ObtenerPorProyecto(idProyecto);

                            foreach (Unidad unidad in unidades)
                            {
                                unidad.proyecto = new Proyectos();
                                unidad.proyecto.idProyecto = respuesta;
                                this.unidadDatos.Insertar(unidad);
                            }
                        }
                    }
                }
            }

            return respuesta;
        }

        // <summary>
        // Adrián Serrano
        // 7/14/2019
        // Efecto: Elimina un proyecto de forma logica de la base de datos
        // Requiere: Proyecto
        // Modifica: -
        // Devuelve: -
        // </summary>
        // <param name="idProyecto"></param>
        public void EliminarProyecto(int idProyecto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand("update Proyecto set disponible=0 where id_proyecto=@id_proyecto_;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_proyecto_", idProyecto);

            sqlConnection.Open();

            sqlCommand.ExecuteScalar();

            sqlConnection.Close();
        }

        // <summary>
        // Adrián Serrano
        // 7/14/2019
        // Efecto: Actualiza un proyecto de la base de datos
        // Requiere: Proyecto
        // Modifica: -
        // Devuelve: -
        // </summary>
        // <param name="Proyecto"></param>
        public void ActualizarProyecto(Proyectos proyecto)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();

            SqlCommand sqlCommand = new SqlCommand("update Proyecto set nombre_proyecto=@nombre_proyecto_, codigo=@codigo_ where id_proyecto=@id_proyecto_;", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id_proyecto_", proyecto.idProyecto);
            sqlCommand.Parameters.AddWithValue("@nombre_proyecto_", proyecto.nombreProyecto);
            sqlCommand.Parameters.AddWithValue("@codigo_", proyecto.codigo);

            sqlConnection.Open();

            sqlCommand.ExecuteScalar();

            sqlConnection.Close();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: devuelve los proyectos que se encuentran en el periodo consultado
        /// Requiere: periodo
        /// Modifica: -
        /// Devuelve: lista de proyectos
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        public List<Proyectos> getProyectosPorPeriodo(Periodo periodo)
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Proyectos> proyectos = new List<Proyectos>();

            SqlCommand sqlCommand = new SqlCommand("select id_proyecto, nombre_proyecto, codigo, es_UCR" +
                " from Proyecto where ano_periodo=@ano_periodo_ AND disponible=1; ", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@ano_periodo_", periodo.anoPeriodo);

            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Proyectos proyecto = new Proyectos();
                proyecto.idProyecto = Convert.ToInt32(reader["id_proyecto"].ToString());
                proyecto.nombreProyecto = reader["nombre_proyecto"].ToString();
                proyecto.codigo = reader["codigo"].ToString();
                proyecto.esUCR = Boolean.Parse(reader["es_UCR"].ToString());
                proyectos.Add(proyecto);
            }

            sqlConnection.Close();

            return proyectos;
        }
    }
}
