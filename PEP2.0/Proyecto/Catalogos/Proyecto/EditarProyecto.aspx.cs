using Servicios;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PEP;

namespace Proyecto.Catalogos.Proyecto
{
    public partial class EditarProyecto : System.Web.UI.Page
    {
        #region variables globales
        ProyectoServicios proyectoServicios = new ProyectoServicios();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);
            if (!IsPostBack)
            {
                if (Session["proyectoEditar"] != null)
                {
                    Proyectos proyecto = (Proyectos)Session["proyectoEditar"];
                    txtNombreProyecto.Text = proyecto.nombreProyecto;
                    txtNombreProyecto.Attributes.Add("oninput", "validarTexto(this)");

                    txtCodigoProyecto.Text = proyecto.codigo;
                    txtCodigoProyecto.Attributes.Add("oninput", "validarTexto(this)");
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

            #region validacion nombre Proyecto
            String NombreProyecto = txtNombreProyecto.Text;

            if (NombreProyecto.Trim() == "")
            {
                txtNombreProyecto.CssClass = "form-control alert-danger";
                divNombreProyectoIncorrecto.Style.Add("display", "block");
                lblNombreProyectoIncorrecto.Visible = true;

                validados = false;
            }
            #endregion

            #region validacion codigo Proyecto
            String CodigoProyecto = txtCodigoProyecto.Text;

            if (CodigoProyecto.Trim() == "")
            {
                txtCodigoProyecto.CssClass = "form-control alert-danger";
                divCodigoProyectoIncorrecto.Style.Add("display", "block");
                lblCodigoProyectoIncorrecto.Visible = true;

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
        protected void txtNombreProyecto_Changed(object sender, EventArgs e)
        {
            txtNombreProyecto.CssClass = "form-control";
            lblNombreProyecto.Visible = false;
        }

        /// <summary>
        /// Adrián Serrano
        /// 7/14/2019
        /// Efecto: Metodo que se activa cuando se cambia el codigo
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void txtCodigoProyecto_Changed(object sender, EventArgs e)
        {
            txtCodigoProyecto.CssClass = "form-control";
            lblCodigoProyecto.Visible = false;
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
                if (Session["proyectoEditar"] != null)
                {
                    Proyectos proyecto = (Proyectos)Session["proyectoEditar"];
                    proyecto.nombreProyecto = txtNombreProyecto.Text;
                    proyecto.codigo = txtCodigoProyecto.Text;

                    proyectoServicios.ActualizarProyecto(proyecto);

                    String url = Page.ResolveUrl("~/Catalogos/Periodos/AdministrarPeriodo.aspx");
                    Response.Redirect(url);
                }
            }


        }

        /// <summary>
        /// Adrián Serrano
        /// 5/9/2019
        /// Efecto:Metodo que se activa cuando se le da click al boton cancelar 
        /// redirecciona a la pantalla de adminstracion de Edificio
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