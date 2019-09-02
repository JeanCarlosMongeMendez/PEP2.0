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
    /// <summary>
    /// Adrián Serrano
    /// 20/may/2019
    /// Clase para crear una nueva unidad
    /// </summary>
    public partial class NuevaUnidad : System.Web.UI.Page
    {
        #region variables globales
        ProyectoServicios proyectoServicios = new ProyectoServicios();
        UnidadServicios unidadServicios = new UnidadServicios();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtNombreUnidad.Attributes.Add("oninput", "validarTexto(this)");
                txtCoordinadorUnidad.Attributes.Add("oninput", "validarTexto(this)");
                CargarProyectos();
            }
        }

        #endregion

        #region logica
        private void CargarProyectos()
        {
            if (Session["periodo"] != null)
            {
                LinkedList<Proyectos> proyectos = new LinkedList<Proyectos>();
                proyectos = this.proyectoServicios.ObtenerPorPeriodo(Int32.Parse(Session["periodo"].ToString()));

                if (proyectos.Count > 0)
                {
                    foreach (Proyectos proyecto in proyectos)
                    {
                        if (proyecto.esUCR)
                        {
                            ListItem itemProyecto = new ListItem(proyecto.nombreProyecto, proyecto.idProyecto.ToString());
                            ProyectosDDL.Items.Add(itemProyecto);
                        }
                    }

                    if (Session["proyecto"] != null)
                    {
                        string proyectoHabilitado = Session["proyecto"].ToString();
                        ProyectosDDL.Items.FindByValue(proyectoHabilitado).Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Metodo que valida los campos que debe ingresar el usuario
        /// </summary>
        /// <returns>Devuelve true si todos los campos esta con datos correctos sino devuelve false
        /// y marcar lo campos para que el usuario vea cuales son los campos que se encuentran mal</returns>
        public Boolean validarCampos()
        {
            Boolean validados = true;

            #region validacion proyecto
            if (Session["proyecto"] == null)
            {
                validados = false;
            }
            #endregion

            #region validacion nombre unidad
            String nombreUnidad = txtNombreUnidad.Text;

            if (nombreUnidad.Trim() == "")
            {
                txtNombreUnidad.CssClass = "form-control alert-danger";
                divNombreUnidadIncorrecto.Style.Add("display", "block");

                validados = false;
            }
            #endregion

            #region validacion coordinador unidad
            String coordinadorUnidad = txtCoordinadorUnidad.Text;

            if (coordinadorUnidad.Trim() == "")
            {
                txtCoordinadorUnidad.CssClass = "form-control alert-danger";
                divCoordinadorUnidadIncorrecto.Style.Add("display", "block");

                validados = false;
            }
            #endregion

            return validados;
        }

        #endregion

        #region eventos

        /// <summary>
        /// Metodo que se activa cuando se cambia el nombre
        /// </summary>
        protected void txtxNombreUnidad_TextChanged(object sender, EventArgs e)
        {
            txtNombreUnidad.CssClass = "form-control";
            lblNombreUnidadIncorrecto.Visible = false;
        }

        /// <summary>
        /// Metodo que se activa cuando se cambia el coordinador
        /// </summary>
        protected void txtxCoordinadorUnidad_TextChanged(object sender, EventArgs e)
        {
            txtCoordinadorUnidad.CssClass = "form-control";
            lblCoordinadorUnidadIncorrecto.Visible = false;
        }

        /// <summary>
        /// Metodo que se activa cuando se da click al boton de guardar
        /// valida que todos los campos se hayan ingresado correctamente 
        /// y guarda los datos en la base de datos 
        /// redirecciona a la pantalla de administacion de periodos
        /// </summary>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //se validan los campos antes de guardar los datos en la base de datos
            if (validarCampos())
            {
                Unidad unidad = new Unidad();
                unidad.nombreUnidad = txtNombreUnidad.Text;
                unidad.coordinador = txtCoordinadorUnidad.Text;
                unidad.proyecto = new Proyectos();
                unidad.proyecto.idProyecto = Convert.ToInt32(ProyectosDDL.SelectedValue.ToString());

                unidadServicios.Insertar(unidad);

                String url = Page.ResolveUrl("~/Catalogos/Periodos/AdministrarPeriodo.aspx");
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Metodo que se activa cuando se le da click al boton cancelar
        /// redirecciona a la pantalla de adminstracion de periodos
        /// </summary>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/Periodos/AdministrarPeriodo.aspx");
            Response.Redirect(url);
        }

        #endregion
    }
}