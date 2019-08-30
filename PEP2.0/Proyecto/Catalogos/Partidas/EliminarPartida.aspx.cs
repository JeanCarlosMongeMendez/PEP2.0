using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Servicios;
using Entidades;

namespace Proyecto.Catalogos.Partidas
{
    public partial class EliminarPartida : System.Web.UI.Page
    {
        #region variables globales
        PartidaServicios partidaServicios = new PartidaServicios();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["partidaEliminar"] != null)
                {
                    Partida partida = (Partida)Session["partidaEliminar"];
                    txtNumeroPartida.Text = partida.numeroPartida;
                    txtDescripcionPartida.Text = partida.descripcionPartida;

                    Partida partidaPadre = new Partida();
                    partidaPadre = this.partidaServicios.ObtenerPorId(partida.partidaPadre.idPartida);
                    txtPadre.Text = partidaPadre.numeroPartida + " - " + partidaPadre.descripcionPartida;
                }
            }
        }
        #endregion

        #region eventos

        /// <summary>
        /// Adrián Serrano
        /// 7/21/2019
        /// Efecto: Metodo que se activa cuando se le da click al boton de eliminar
        /// redirecciona a la pantalla de adminstracion de partidas
        /// Elimina logicamente la partida de la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Session["partidaEliminar"] != null)
            {
                Partida partida = (Partida)Session["partidaEliminar"];
                String url = Page.ResolveUrl("~/Catalogos/Partidas/AdministrarPartida.aspx");

                try
                {
                    partidaServicios.EliminarPartida(partida.idPartida);
                    Response.Redirect(url);
                }
                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// Adrián Serrano
        /// 7/21/2019
        /// Efecto:Metodo que se activa cuando se le da click al boton cancelar 
        /// redirecciona a la pantalla de adminstracion de partidas
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/Partidas/AdministrarPartida.aspx");
            Response.Redirect(url);
        }

        #endregion
    }
}