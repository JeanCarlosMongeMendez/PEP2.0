using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class ParametrosDatos
    {

        ConexionDatos conexion = new ConexionDatos();
        /// <summary>
        /// Leonardo Carrion
        /// 12/jun/2019
        /// Efecto: obtiene todas los funcionarios de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de funcionarios
        /// </summary>
        /// <returns></returns>
        public List<Parametros> getCorreosDestinatarios()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Parametros> parametros = new List<Parametros>();

            String consulta = @"select * from Parametros  where nombre_parametro='correo' order by id_parametro";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Parametros parametro = new Parametros();
                parametro.idParametro= Convert.ToInt32(reader["id_parametro"].ToString());
                parametro.nombreParametro = reader["nombre_parametro"].ToString();
                parametro.valor = reader["valor"].ToString();
                parametros.Add(parametro);
            }

            sqlConnection.Close();

            return parametros;
        }

    }
}
