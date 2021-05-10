using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class CajaChica
    {
        public int idCajaChica { get; set; }
        public int anoPeriodo { get; set; }
        public int idProyedto { get; set; }
        public DateTime fecha { get; set; }
        public string realizadoPor { get; set; }
        public double monto { get; set; }
        public string comentario { get; set; }
        public string numeroCajaChica { get; set; }

        public EstadoCajaChica idEstadoCajaChica { get; set; }
    }
}
