using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class PresupuestoIngreso
    {
        public int idPresupuestoIngreso { get; set; }
        public bool estado { get; set; }
        public double monto { get; set; }
        public bool esInicial { get; set; }
        public Proyectos proyecto { get; set; }
    }
}
