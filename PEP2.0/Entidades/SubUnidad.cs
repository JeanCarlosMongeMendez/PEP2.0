using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Clase para administrar la entidad de Sub unidad
    /// </summary>
    public class SubUnidad
    {
        public int idSubUnidad { get; set; }
        public Unidad unidad { get; set; }
        public String nombre { get; set; }
        public Boolean activo { get; set; }
    }
}
