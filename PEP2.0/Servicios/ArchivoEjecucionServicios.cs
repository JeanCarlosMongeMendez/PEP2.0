using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ArchivoEjecucionServicios
    {
        ArchivoEjecucionDatos archivoEjecucionDatos = new ArchivoEjecucionDatos();
        /*Leonardo Carrion
        09/09/2016
        Metodo que inserta un archivo muestra en la base de datos
        devuelve el id del archivo ingresado*/
        public int insertarArchivoMuestra(ArchivoEjecucion archivoMuestra)
        {
            return archivoEjecucionDatos.insertarArchivoMuestra(archivoMuestra);
        }
        public List<ArchivoEjecucion> obtenerArchivoMuestra(int idEjecucion)
        {
            return archivoEjecucionDatos.getArchivosEjecucion(idEjecucion);
        }
    }
}
