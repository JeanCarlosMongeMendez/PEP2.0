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
    /// Leonardo Carrion.
    /// 06/may/2019
    /// Clase para administrar las consultas sql para la tabla de Contacto
    /// </summary>
    public class ContactoDatos
    {
        private ConexionDatos conexion = new ConexionDatos();

        public List<Contacto> getContactos2()
        {
            List<Contacto> listaContactos = new List<Contacto>();

            //SqlConnection sqlConnection = conexion.conexionCTL();

            //String consulta
            //    = @"SELECT * from Contacto";

            //SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

            //SqlDataReader reader;
            //sqlConnection.Open();
            //reader = sqlCommand.ExecuteReader();

            //while (reader.Read())
            //{
            //    Contacto contacto = new Contacto();

            //    contacto.idContacto = Convert.ToInt32(reader["id_contacto"].ToString());
            //    contacto.nombreContacto = reader["nombre_contacto"].ToString();

            //    listaContactos.Add(contacto);
            //}

            //sqlConnection.Close();

            for (int i = 0; i < 30; i++) { 
            Contacto contacto = new Contacto();
            contacto.idContacto = i;
            contacto.nombreContacto = "Contacto"+i;

            listaContactos.Add(contacto);
        }

            return listaContactos;
        }

    }
}
