using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    /// <summary>
    /// Leonardo Carrion
    /// 06/may/2019
    /// Clase para administrar los servicios de Contacto
    /// </summary>
    public class ContactoServicios
    {
        ContactoDatos contactoDatos = new ContactoDatos();

        public List<Contacto> getContactos2()
        {
            return contactoDatos.getContactos2();
        }
    }
}
