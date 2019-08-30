using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Servicios;
using Entidades;

namespace Proyecto.Catalogos.Proyecto
{
    public partial class EliminarProyecto : System.Web.UI.Page
    {
        #region variables globales
        ProyectoServicios proyectoServicios = new ProyectoServicios();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["proyectoEliminar"] != null)
                {
                    Proyectos proyecto = (Proyectos)Session["proyectoEliminar"];
                    txtNombreProyecto.Text = proyecto.nombreProyecto;
                    txtCodigoProyecto.Text = proyecto.codigo;
                    if (proyecto.esUCR)
                    {
                        txtEsUCRProyecto.Text = "UCR";
                    }
                    else
                    {
                        txtEsUCRProyecto.Text = "FUNDEVI";
                    }
                    txPeriodoProyecto.Text = proyecto.periodo.anoPeriodo.ToString();
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
        /// Elimina logicamente el proyecto de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Session["proyectoEliminar"] != null)
            {
                Proyectos proyecto = (Proyectos)Session["proyectoEliminar"];
                String url = Page.ResolveUrl("~/Catalogos/Periodos/AdministrarPeriodo.aspx");

                try
                {
                    proyectoServicios.EliminarProyecto(proyecto.idProyecto);
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