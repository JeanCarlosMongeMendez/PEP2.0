using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class PresupuestoEgreso
    {
        public int idPresupuestoEgreso { get; set; }
        public Unidad unidad { get; set; }
        public String planEstrategicoOperacional { get; set; }
        public Double montoTotal { get; set; }
    }
}
