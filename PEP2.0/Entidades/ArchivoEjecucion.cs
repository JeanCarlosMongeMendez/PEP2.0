using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ArchivoEjecucion
    {
        public int idEjecucion { get; set; }
        public int idArchivoEjecucion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public String creadoPor { get; set; }
        public String nombreArchivo { get; set; }
        public String rutaArchivo { get; set; }
    }
}
