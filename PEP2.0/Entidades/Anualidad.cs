using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Leonardo Carrion
    /// 31/oct/2019
    /// Clase para administrar la entidad de Anualidad
    /// </summary>
    public class Anualidad
    {
        public int idAnualidad { get; set; }
        public Double porcentaje { get; set; }
        public Periodo periodo { get; set; }
    }
}
