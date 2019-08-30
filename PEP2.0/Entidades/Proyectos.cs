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
    /// Debe llamarse Proyectos para que no haya confusion en los espacios de nombres
    /// Clase para administrar la entidad de Proyecto
    /// </summary>
    public class Proyectos
    {
        public int idProyecto { get; set; }
        public string nombreProyecto { get; set; }
        public string codigo { get; set; }
        public bool esUCR { get; set; }
        public Periodo periodo { get; set; }
    }
}
