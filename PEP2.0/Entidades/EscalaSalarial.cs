using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Leonardo Carrion
    /// 10/jun/2019
    /// Clase para administrar la entidad de Escala Salarial
    /// </summary>
    public class EscalaSalarial
    {
        public int idEscalaSalarial { get; set; }
        public String descEscalaSalarial { get; set; }
        public Double salarioBase1 { get; set; }
        public int topeEscalafones { get; set; }
        public Double porentajeEscalafones { get; set; }
        public Periodo periodo { get; set; }
        public Double salarioBase2 { get; set; }
    }
}
