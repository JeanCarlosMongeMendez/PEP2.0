using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    /// <summary>
    /// Adrián Serrano
    /// 17/may/2019
    /// Clase para administrar los servicios de Unidad
    /// </summary>
    public class UnidadServicios
    {
        UnidadDatos unidadDatos = new UnidadDatos();

        public LinkedList<Unidad> ObtenerPorProyecto(int idProyecto)
        {
            return this.unidadDatos.ObtenerPorProyecto(idProyecto);
        }

        public int Insertar(Unidad unidad)
        {
            return this.unidadDatos.Insertar(unidad);
        }

        public Unidad ObtenerPorId(int idUnidad)
        {
            return this.unidadDatos.ObtenerPorId(idUnidad);
        }

        public void EliminarUnidad(int idUnidad)
        {
            this.unidadDatos.EliminarUnidad(idUnidad);
        }

        public void ActualizarUnidad(Unidad unidad)
        {
            this.unidadDatos.ActualizarUnidad(unidad);
        }
    }
}
