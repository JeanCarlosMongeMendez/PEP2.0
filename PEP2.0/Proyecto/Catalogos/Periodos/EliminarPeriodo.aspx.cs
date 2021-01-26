using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Servicios;
using Entidades;
using PEP;

namespace Proyecto.Catalogos.Periodos
{
    public partial class EliminarPeriodo : System.Web.UI.Page
    {
        #region variables globales
        PeriodoServicios periodoServicios = new PeriodoServicios();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);
            if (!IsPostBack)
            {
                if (Session["periodoEliminar"] != null)
                {
                    Periodo periodo = (Periodo)Session["periodoEliminar"];
                    txtAnoPeriodo.Text = periodo.anoPeriodo.ToString();
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
        /// Elimina logicamente el periodo de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Session["periodoEliminar"] != null)
            {
                Periodo periodo = (Periodo)Session["periodoEliminar"];
                String url = Page.ResolveUrl("~/Catalogos/Periodos/AdministrarPeriodo.aspx");

                try
                {
                    periodoServicios.EliminarPeriodo(periodo.anoPeriodo);
                    Response.Redirect(url);
                }
                catch(Exception ex)
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