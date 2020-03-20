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
        /*Kevin Picado
         20/03/20
        Metodo que inserta un archivo muestra en la base de datos
        devuelve el id del archivo ingresado*/
        public int insertarArchivoEjecucion(ArchivoEjecucion archivoMuestra)
        {
            return archivoEjecucionDatos.insertarArchivoMuestra(archivoMuestra);
        }
        public List<ArchivoEjecucion> obtenerArchivoEjecucion(int idEjecucion)
        {
            return archivoEjecucionDatos.getArchivosEjecucion(idEjecucion);
        }
        public void eliminarArchivoEjecucion(int idEjecucion)
        {
             archivoEjecucionDatos.eliminarArchivoEjecucion(idEjecucion);
        }
        public void eliminarArchivoEjecucionPorNombreYId(int idEjecucion,string nombreArchivo)
        {
            archivoEjecucionDatos.eliminarArchivoEjecucionPorNombreYID(idEjecucion,nombreArchivo);
        }
    }
}
