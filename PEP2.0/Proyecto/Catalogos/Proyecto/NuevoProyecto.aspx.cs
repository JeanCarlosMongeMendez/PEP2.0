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
    /// <summary>
    /// Adrián Serrano
    /// 20/may/2019
    /// Clase para crear un nuevo proyecto
    /// </summary>
    public partial class NuevoProyecto : System.Web.UI.Page
    {
        #region variables globales
        ProyectoServicios proyectoServicios = new ProyectoServicios();
        PeriodoServicios periodoServicios = new PeriodoServicios();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtNombreProyecto.Attributes.Add("oninput", "validarTexto(this)");
                txtCodigoProyecto.Attributes.Add("oninput", "validarTexto(this)");
                CargarPeriodos();

            }
        }

        #endregion

        #region logica

        private void CargarPeriodos()
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            PeriodosDDL.Items.Clear();
            periodos = this.periodoServicios.ObtenerTodos();

            if (periodos.Count > 0)
            {
                foreach (Periodo periodo in periodos)
                {
                    string nombre;

                    if (periodo.habilitado)
                    {
                        nombre = periodo.anoPeriodo.ToString() + " (Actual)";
                    }
                    else
                    {
                        nombre = periodo.anoPeriodo.ToString();
                    }

                    ListItem itemPeriodo = new ListItem(nombre, periodo.anoPeriodo.ToString());
                    PeriodosDDL.Items.Add(itemPeriodo);
                }

                if (Session["periodo"] != null)
                {
                    string anoHabilitado = Session["periodo"].ToString();
                    PeriodosDDL.Items.FindByValue(anoHabilitado).Selected = true;
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

            #region validacion periodo
            if (Session["periodo"] == null)
            {
                validados = false;
            }
            #endregion

            #region validacion nombre proyecto
            String nombreProyecto = txtNombreProyecto.Text;

            if (nombreProyecto.Trim() == "")
            {
                txtNombreProyecto.CssClass = "form-control alert-danger";
                divNombreProyectoIncorrecto.Style.Add("display", "block");

                validados = false;
            }
            #endregion

            #region validacion codigo proyecto
            String codigoProyecto = txtCodigoProyecto.Text;

            if (codigoProyecto.Trim() == "")
            {
                txtCodigoProyecto.CssClass = "form-control alert-danger";
                divCodigoProyectoIncorrecto.Style.Add("display", "block");

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
        protected void txtxNombreProyecto_TextChanged(object sender, EventArgs e)
        {
            txtNombreProyecto.CssClass = "form-control";
            lblNombreProyectoIncorrecto.Visible = false;
        }

        /// <summary>
        /// Metodo que se activa cuando se cambia el codigo
        /// </summary>
        protected void txtxCodigoProyecto_TextChanged(object sender, EventArgs e)
        {
            txtCodigoProyecto.CssClass = "form-control";
            lblCodigoProyectoIncorrecto.Visible = false;
        }

        /// <summary>
        /// Metodo que se activa cuando se da click al boton de guardar
        /// valida que todos los campos se hayan ingrsado correctamente 
        /// y guarda los datos en la base de datos 
        /// redirecciona a la pantalla de administacion de periodos
        /// </summary>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //se validan los campos antes de guardar los datos en la base de datos
            if (validarCampos())
            {
                Proyectos proyecto = new Proyectos();
                proyecto.nombreProyecto = txtNombreProyecto.Text;
                proyecto.codigo = txtCodigoProyecto.Text;
                proyecto.esUCR = Convert.ToBoolean(ddlEsUCRProyecto.SelectedValue);
                proyecto.periodo = new Periodo();
                proyecto.periodo.anoPeriodo = Convert.ToInt32(PeriodosDDL.SelectedValue.ToString());

                int respuesta = proyectoServicios.Insertar(proyecto);

                if (respuesta > 0)
                {
                    String url = Page.ResolveUrl("~/Catalogos/Periodos/AdministrarPeriodo.aspx");
                    Response.Redirect(url);
                }else if (respuesta == -1)
                {
                    //Ya existe un proyecto con el mismo codigo en el periodo seleccionado
                }
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