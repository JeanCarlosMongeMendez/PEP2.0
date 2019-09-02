using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Servicios;
using Entidades;

namespace Proyecto.Catalogos.Unidades
{
    public partial class EliminarUnidad : System.Web.UI.Page
    {
        #region variables globales
        UnidadServicios unidadServicios = new UnidadServicios();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["unidadEliminar"] != null)
                {
                    Unidad unidad = (Unidad)Session["unidadEliminar"];
                    txtNombreUnidad.Text = unidad.nombreUnidad;
                    txtCoordinadorUnidad.Text = unidad.coordinador;
                }
            }
        }

        #endregion

        #region eventos
        /// <summary>
        /// Adrián Serrano
        /// 7/14/2019
        /// Efecto: Metodo que se activa cuando se le da click al boton de eliminar
        /// redirecciona a la pantalla de adminstracion de periodos
        /// Elimina logicamente la unidad de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Session["unidadEliminar"] != null)
            {
                Unidad unidad = (Unidad)Session["unidadEliminar"];
                String url = Page.ResolveUrl("~/Catalogos/Periodos/AdministrarPeriodo.aspx");

                try
                {
                    unidadServicios.EliminarUnidad(unidad.idUnidad);
                    Response.Redirect(url);
                }
                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// Adrián Serrano
        /// 7/14/2019
        /// Efecto:Metodo que se activa cuando se le da click al boton cancelar 
        /// redirecciona a la pantalla de adminstracion de periodos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/Periodos/AdministrarPeriodo.aspx");
            Response.Redirect(url);
        }

        #endregion
    }
}