using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Leonardo Carrion
    /// 20/sep/2019
    /// Clase para administrar la entidad de Carga Social
    /// </summary>
    public class CargaSocial
    {
        public int idCargaSocial { get; set; }
        public String descCargaSocial { get; set; }
        public Double porcentajeCargaSocial { get; set; }
        public Boolean activo { get; set; }
        public Partida partida { get; set; }
        public Periodo periodo { get; set; }
    }
}
