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
    /// Clase para administrar la entidad de Proyeccion
    /// </summary>
    public class Proyeccion
    {
        public int idProyeccion { get; set; }
        public Funcionario funcionario { get; set; }
        public Mes mes { get; set; }
        public Double montoSalario { get; set; }
        public Double montoCargasTotal { get; set; }
        public Periodo periodo { get; set; }
    }
}
