using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Leonardo Carrion
    /// 21/oct/2019
    /// Clase para administrar la entidad del reporte de egresos
    /// </summary>
    public class Reporte_Egresos
    {
        public String numeroPartidaPadre { get; set; }
        public String descripcionPartidaPadre { get; set; }
        public String numeroPartidaHija { get; set; }
        public String descripcionPartidaHija { get; set; }
        public String nombreUnidad { get; set; }
        public Double monto { get; set; }
    }
}
