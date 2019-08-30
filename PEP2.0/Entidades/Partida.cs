using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    /// <summary>
    /// Adrián Serrano
    /// 27/may/2019
    /// Clase para administrar la entidad de Partida
    /// </summary>
    public class Partida
    {
        public int idPartida { get; set; }
        public string numeroPartida { get; set; }
        public string descripcionPartida { get; set; }
        public Partida partidaPadre { get; set; }
        public Periodo periodo { get; set; }
        //TestCommit
    }
}
