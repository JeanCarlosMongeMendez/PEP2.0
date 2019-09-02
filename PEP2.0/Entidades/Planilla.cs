using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Leonardo Carrion
    /// 14/jun/2019
    /// Clase para administrar la entidad de Planilla
    /// </summary>
    public class Planilla
    {
        public int idPlanilla { get; set; }
        public Double anualidad1 { get; set; }
        public Double anualidad2 { get; set; }
        public Periodo periodo { get; set; }
    }
}
