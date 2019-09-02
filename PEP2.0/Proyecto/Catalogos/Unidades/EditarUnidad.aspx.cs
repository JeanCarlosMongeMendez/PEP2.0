using Servicios;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proyecto.Catalogos.Unidades
{
    public partial class EditarUnidad : System.Web.UI.Page
    {
        #region variables globales
        UnidadServicios unidadServicios = new UnidadServicios();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["unidadEditar"] != null)
                {
                    Unidad unidad = (Unidad)Session["unidadEditar"];
                    txtNombreUnidad.Text = unidad.nombreUnidad;
                    txtNombreUnidad.Attributes.Add("oninput", "validarTexto(this)");

                    txtCoordinadorUnidad.Text = unidad.coordinador;
                    txtCoordinadorUnidad.Attributes.Add("oninput", "validarTexto(this)");
                }
            }
        }

        #endregion

        #region logica
        /// <summary>
        /// Adrián Serrano
        /// 7/14/2019
        /// Efecto:Metodo que valida los campos que debe ingresar el usuario
        /// devuelve true si todos los campos esta con datos correctos
        /// sino devuelve false y marcar lo campos para que el usuario vea cuales son los campos que se encuentran mal
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public Boolean validarCampos()
        {
            Boolean validados = true;

            #region validacion nombre Unidad
            String NombreUnidad = txtNombreUnidad.Text;

            if (NombreUnidad.Trim() == "")
            {
                txtNombreUnidad.CssClass = "form-control alert-danger";
                divNombreUnidadIncorrecto.Style.Add("display", "block");
                lblNombreUnidadIncorrecto.Visible = true;

                validados = false;
            }
            #endregion

            #region validacion coordinador unidad
            String CoordinadorUnidad = txtCoordinadorUnidad.Text;

            if (CoordinadorUnidad.Trim() == "")
            {
                txtCoordinadorUnidad.CssClass = "form-control alert-danger";
                divCoordinadorUnidadIncorrecto.Style.Add("display", "block");
                lblCoordinadorUnidadIncorrecto.Visible = true;

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
        /// Efecto: Metodo que se activa cuando se cambia el nombre
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void txtNombreUnidad_Changed(object sender, EventArgs e)
        {
            txtNombreUnidad.CssClass = "form-control";
            lblNombreUnidad.Visible = false;
        }

        /// <summary>
        /// Adrián Serrano
        /// 7/14/2019
        /// Efecto: Metodo que se activa cuando se cambia el coordinador
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void txtCodigoProyecto_Changed(object sender, EventArgs e)
        {
            txtCoordinadorUnidad.CssClass = "form-control";
            lblCoordinadorUnidad.Visible = false;
        }

        /// <summary>
        /// Adrián Serrano
        /// 7/14/2019
        /// Efecto: Metodo que se activa cuando se le da click al boton de actualizar
        /// valida que todos los campos se hayan ingresado correctamente y guarda los datos en la base de datos
        /// redireccion a la pantalla de Administracion de Periodos
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
                if (Session["unidadEditar"] != null)
                {
                    Unidad unidad = (Unidad)Session["unidadEditar"];
                    unidad.nombreUnidad = txtNombreUnidad.Text;
                    unidad.coordinador = txtCoordinadorUnidad.Text;
                    unidadServicios.ActualizarUnidad(unidad);

                    String url = Page.ResolveUrl("~/Catalogos/Periodos/AdministrarPeriodo.aspx");
                    Response.Redirect(url);
                }
            }


        }

        /// <summary>
        /// Adrián Serrano
        /// 5/9/2019
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