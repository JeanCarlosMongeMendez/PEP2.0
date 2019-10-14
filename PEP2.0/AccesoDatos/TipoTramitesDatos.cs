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
    /// Josseline M
    /// Clase encargada de consumir datos de la base
    /// </summary>
    public class TipoTramitesDatos
    {

        private ConexionDatos conexion = new ConexionDatos();
       
        /// <summary>
        /// Josseline M
        /// Este método se encarga de obtener el tipo de tramite existente para el tipo de proyecto UCR o Fundevi
        /// </summary>
        /// <param name="proyecto"></param>
        /// <returns></returns>
        public List<TipoTramite> obtenerTipoTramitesPorProyecto(Proyectos proyecto)
        {
            List<TipoTramite> tramites = new List<TipoTramite>();
            SqlConnection sqlConnection = conexion.conexionPEP();
           
            if (proyecto.esUCR)
            {
                SqlCommand sqlCommand = new SqlCommand("select id_tramite,nombre_tramite,Es_UCR" +
             " from  Tipos_tramite where Es_UCR=1; ", sqlConnection);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    TipoTramite tramite = new TipoTramite();
                    tramite.idTramite = Convert.ToInt32(reader["id_tramite"].ToString());
                    tramite.nombreTramite = reader["nombre_tramite"].ToString();
                    tramite.EsUCR = Convert.ToInt32(reader["ES_UCR"].ToString());
                    tramites.Add(tramite);

                }

                sqlConnection.Close();
            }else
            {
                SqlCommand sqlCommand = new SqlCommand("select id_tramite,nombre_tramite,Es_UCR" +
             " from  Tipos_tramite where Es_UCR=0; ", sqlConnection);
               
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    TipoTramite tramite = new TipoTramite();
                    tramite.idTramite = Convert.ToInt32(reader["id_tramite"].ToString());
                    tramite.EsUCR = Convert.ToInt32(reader["ES_UCR"].ToString());
                    tramite.nombreTramite = reader["nombre_tramite"].ToString();
                    tramites.Add(tramite);

                }

                sqlConnection.Close();
            }
            
            
            return tramites;
        }


    }
}
