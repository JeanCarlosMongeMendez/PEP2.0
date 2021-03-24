using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Entidades
{
    public class PartidaUnidad
    {
        public int idPartida { get; set; }
        public int idUnidad { get; set; }
        public String numeroPartida { get; set; }
        public double monto { get; set; }
        public double montoDisponible { get; set; }
        public String nombreUnidad { get; set; }
    }
}
