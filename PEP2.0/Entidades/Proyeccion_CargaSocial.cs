using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Leonardo Carrion
    /// 30/oct/2019
    /// Clase para administrar la entidad de Proyecccion carga social
    /// </summary>
    public class Proyeccion_CargaSocial
    {
        public Proyeccion proyeccion { get; set; }
        public CargaSocial cargaSocial { get; set; }
        public Double monto { get; set; }
    }
}
