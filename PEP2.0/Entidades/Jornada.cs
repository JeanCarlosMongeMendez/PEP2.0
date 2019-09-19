using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Leonardo Carrion
    /// 18/sep/2018
    /// clase para administrar la entidad de Jornada
    /// </summary>
    public class Jornada
    {
        public int idJornada { get; set; }
        public String descJornada { get; set; }
        public Boolean activo { get; set; }
        public Double porcentajeJornada { get; set; }
    }
}
