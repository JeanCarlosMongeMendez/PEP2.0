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
    /// 30/oct/2019
    /// Clase para administra las consultas de Mes
    /// </summary>
    public class MesDatos
    {
        ConexionDatos conexion = new ConexionDatos();

        /// <summary>
        /// Leonardo Carrion
        /// 30/oct/2019
        /// Efecto: devuelve lista de meses que se encuentran en la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: lista de meses
        /// </summary>
        /// <returns></returns>
        public List<Mes> getMeses()
        {
            SqlConnection sqlConnection = conexion.conexionPEP();
            List<Mes> meses = new List<Mes>();

            String consulta = @"select * from Mes order by numero;";

            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            SqlDataReader reader;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Mes mes = new Mes();

                mes.idMes = Convert.ToInt32(reader["id_mes"].ToString());
                mes.nombreMes = reader["nombre_mes"].ToString();
                mes.numero = Convert.ToInt32(reader["numero"].ToString());

                meses.Add(mes);
            }

            sqlConnection.Close();

            return meses;
        }
    }
}
