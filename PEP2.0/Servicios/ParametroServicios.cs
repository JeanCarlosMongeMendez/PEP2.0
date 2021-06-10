using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ParametroServicios
    {
        ParametrosDatos parametrosDatos=new ParametrosDatos();


        public List<Parametros>getCorreosDestinatarios()
        {
            return parametrosDatos.getCorreosDestinatarios();
        }
    }
}
