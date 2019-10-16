using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class JornadaUnidadFuncionario
    {
        public int idUnidad { get; set; }
        public int idJornada { get; set; }
        public string descUnidad { get; set; }
        public int idFuncionario { get; set; }
        public double jornadaAsignada { get; set; }
    }
}
