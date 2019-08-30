using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Adrián Serrano
    /// 17/may/2019
    /// Clase para administrar la entidad de Unidad
    /// </summary>
    public class Unidad
    {
        public int idUnidad { get; set; }
        public string nombreUnidad { get; set; }
        public string coordinador { get; set; }
        public Proyectos proyecto { get; set; }
    }
}
