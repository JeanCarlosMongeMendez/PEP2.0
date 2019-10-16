using Servicios;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proyecto.Catalogos.Partidas
{
    public partial class EditarPartida : System.Web.UI.Page
    {
        #region variables globales
        PartidaServicios partidaServicios = new PartidaServicios();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["partidaEditar"] != null)
                {
                    Partida partida = (Partida)Session["partidaEditar"];
                    txtNumeroPartida.Text = partida.numeroPartida;
                    txtNumeroPartida.Attributes.Add("oninput", "validarTexto(this)");

                    txtDescripcionPartida.Text = partida.descripcionPartida;
                    txtDescripcionPartida.Attributes.Add("oninput", "validarTexto(this)");
                }
            }
        }

        #endregion

        #region logica


        /// <summary>
        /// Adrián Serrano
        /// 7/21/2019
        /// Efecto:Metodo que valida los campos que debe ingresar el usuario
        /// devuelve true si todos los campos esta con datos correctos
        /// sino devuelve false y marcar lo campos para que el usuario vea cuales son los campos que se encuntran mal
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public Boolean validarCampos()
        {
            Boolean validados = true;

            #region validacion numero partida
            String NumeroPartida = txtNumeroPartida.Text;

            if (NumeroPartida.Trim() == "")
            {
                txtNumeroPartida.CssClass = "form-control alert-danger";
                divNumeroPartidaIncorrecto.Style.Add("display", "block");
                lblNumeroPartidaIncorrecto.Visible = true;

                validados = false;
            }
            #endregion

            #region validacion descripcion partida
            String DescripcionPartida = txtDescripcionPartida.Text;

            if (DescripcionPartida.Trim() == "")
            {
                txtDescripcionPartida.CssClass = "form-control alert-danger";
                divDescripcionPartidaIncorrecto.Style.Add("display", "block");
                lblDescripcionPartidaIncorrecto.Visible = true;

                validados = false;
            }
            #endregion

            return validados;
        }

        #endregion

        #region eventos
        /// <summary>
        /// Adrián Serrano
        /// 7/14/2019
        /// Efecto: Metodo que se activa cuando se cambia el numero
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void txtNumeroPartida_Changed(object sender, EventArgs e)
        {
            txtNumeroPartida.CssClass = "form-control";
            lblNumeroPartida.Visible = false;
        }

        /// <summary>
        /// Adrián Serrano
        /// 7/14/2019
        /// Efecto: Metodo que se activa cuando se cambia la descripcion
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void txtDescripcionPartida_Changed(object sender, EventArgs e)
        {
            txtDescripcionPartida.CssClass = "form-control";
            lblDescripcionPartida.Visible = false;
        }

        /// <summary>
        /// Adrián Serrano
        /// 7/14/2019
        /// Efecto: Metodo que se activa cuando se le da click al boton de actualizar
        /// valida que todos los campos se hayan ingresado correctamente y guarda los datos en la base de datos
        /// redireccion a la pantalla de Administracion de Partidas
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnActualiza_Click(object sender, EventArgs e)
        {
            //se validan los campos antes de actualizar los datos en la base de datos
            if (validarCampos())
            {
                if (Session["partidaEditar"] != null)
                {
                    Partida partida = (Partida)Session["partidaEditar"];
                    partida.numeroPartida = txtNumeroPartida.Text;
                    partida.descripcionPartida = txtDescripcionPartida.Text;
                    
                    partidaServicios.ActualizarPartida(partida);

                    String url = Page.ResolveUrl("~/Catalogos/Partidas/AdministrarPartida.aspx");
                    Response.Redirect(url);
                }
            }
        }

        /// <summary>
        /// Adrián Serrano
        /// 5/9/2019
        /// Efecto:Metodo que se activa cuando se le da click al boton cancelar 
        /// redirecciona a la pantalla de adminstracion de Partidas
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