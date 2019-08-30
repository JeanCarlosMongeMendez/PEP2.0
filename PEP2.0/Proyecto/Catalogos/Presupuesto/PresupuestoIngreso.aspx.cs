using Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proyecto.Catalogos.Presupuesto
{
    /// <summary>
    /// Adrián Serrano
    /// 17/may/2019
    /// Clase de lógica para controlar la vista de Presupuesto Ingreso
    /// </summary>
    public partial class PresupuestoIngreso : System.Web.UI.Page
    {
        #region variables globales
        PeriodoServicios periodoServicios = new PeriodoServicios();
        ProyectoServicios proyectoServicios = new ProyectoServicios();
        PresupuestoServicios presupuestoServicios = new PresupuestoServicios();
        #endregion

        readonly PagedDataSource pgsourceProyectos = new PagedDataSource();
        readonly PagedDataSource pgsourcePresupuestos = new PagedDataSource();

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            PEP.Utilidades.escogerMenu(Page, rolesPermitidos);

            if (!Page.IsPostBack)
            {
                Session["listaProyectos"] = null;
                Session["proyecto"] = null;
                divMontoPresupuesto.Visible = false;

                LlenarPeriodosDDL();
                MostrarDatosTablaProyectos();
            }
        }
        #endregion

        #region logica


        /// <summary>
        /// Metodo que valida los campos que debe ingresar el usuario
        /// </summary>
        /// <returns>Devuelve true si todos los campos esta con datos correctos sino devuelve false
        /// y marcar lo campos para que el usuario vea cuales son los campos que se encuentran mal</returns>
        public Boolean validarCampos()
        {
            Boolean validados = true;

            #region validacion monto presupuesto
            String monto = montoPresupuesto.Text;

            if (monto.Trim() == "")
            {
                montoPresupuesto.CssClass = "form-control alert-danger";
                divMontoPresupuestoIncorrecto.Style.Add("display", "block");

                validados = false;
            }
            else
            {
                montoPresupuesto.CssClass = "form-control";
                divMontoPresupuestoIncorrecto.Style.Add("display", "none");

                validados = true;
            }
            #endregion

            #region validacion formato monto presupuesto
            String montoPresupuestoFormato = montoPresupuesto.Text;

            var esNumero = int.TryParse(montoPresupuestoFormato, out int n);

            if (!esNumero)
            {
                montoPresupuesto.CssClass = "form-control alert-danger";
                divMontoPresupuestoIncorrecto.Style.Add("display", "block");

                validados = false;
            }
            else
            {
                montoPresupuesto.CssClass = "form-control";
                divMontoPresupuestoIncorrecto.Style.Add("display", "none");

                validados = true;
            }
            #endregion

            return validados;
        }


        #region prueba
        private void LlenarPeriodosDDL()
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            periodos = this.periodoServicios.ObtenerTodos();
            int anoHabilitado = 0;

            foreach (Periodo periodo in periodos)
            {
                string ano = "";
                if (periodo.habilitado)
                {
                    ano = periodo.anoPeriodo.ToString() + " (Actual)";
                    anoHabilitado = periodo.anoPeriodo;
                }
                else
                {
                    ano = periodo.anoPeriodo.ToString();
                }
                
                ListItem item = new ListItem(ano, periodo.anoPeriodo.ToString());
                PeriodosDDL.Items.Add(item);
            }

            if (anoHabilitado != 0)
            {
                PeriodosDDL.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
            }
        }

        /// <summary>
        /// LLena y muestra la tabla de proyectos basado en el periodo seleccionado
        /// </summary>
        private void MostrarDatosTablaProyectos()
        {
            divMontoPresupuesto.Visible = false;

            if (PeriodosDDL.Items.Count > 0)
            {
                LinkedList<Proyectos> listaProyectos = new LinkedList<Proyectos>();
                listaProyectos = proyectoServicios.ObtenerPorPeriodo(Convert.ToInt32(PeriodosDDL.SelectedValue));
                Session["listaProyectos"] = listaProyectos;

                var dt = listaProyectos;
                pgsourceProyectos.DataSource = dt;
                pgsourceProyectos.AllowPaging = false;

                ViewState["TotalPaginas"] = pgsourceProyectos.PageCount;
                rpProyectos.DataSource = pgsourceProyectos;
                rpProyectos.DataBind();
            }
        }

        /// <summary>
        /// LLena y muestra la tabla de presupuestos basado en el proyecto seleccionado
        /// </summary>
        private void MostrarDatosTablaPresupuestos()
        {
            if (Session["proyecto"] != null)
            {
                int idProyecto = Convert.ToInt32(Session["proyecto"]);
                LinkedList<Entidades.PresupuestoIngreso> presupuestoIngresos = this.presupuestoServicios.ObtenerPorProyecto(idProyecto);

                var dt = presupuestoIngresos;
                pgsourcePresupuestos.DataSource = dt;
                pgsourcePresupuestos.AllowPaging = false;

                ViewState["TotalPaginas"] = pgsourcePresupuestos.PageCount;

                rpPresupuestos.DataSource = pgsourcePresupuestos;
                rpPresupuestos.DataBind();

                if (presupuestoIngresos.Count > 0)
                {
                    bool inicialAprobado = false;
                    bool noAprobado = false;
                    foreach (Entidades.PresupuestoIngreso presupuestoIngreso in presupuestoIngresos)
                    {
                        if (presupuestoIngreso.esInicial && presupuestoIngreso.estado)
                        {
                            inicialAprobado = true;

                        }
                        else if (!presupuestoIngreso.estado)
                        {
                            noAprobado = true;
                        }
                    }

                    if (inicialAprobado)
                    {
                        TipoPresupuesto.Text = "Adicional";
                    }
                    else
                    {
                        TipoPresupuesto.Text = "Inicial";
                    }

                    if (noAprobado)
                    {
                        btnGuardar.Enabled = false;
                    }
                    else
                    {
                        btnGuardar.Enabled = true;
                    }
                }
                else
                {
                    TipoPresupuesto.Text = "Inicial";
                }

                divMontoPresupuesto.Visible = true;
            }
        }

        #endregion

        #endregion

        #region eventos

        //protected void btnEliminar_Click(object sender, EventArgs e)
        //{
        //    int idContacto = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

        //    List<Contacto> listaContactos = (List<Contacto>)Session["listaContactosFiltrada"];

        //    Contacto contactoEliminar = new Contacto();

        //    foreach (Contacto contacto in listaContactos)
        //    {
        //        if (contacto.idContacto == idContacto)
        //        {
        //            contactoEliminar = contacto;
        //            break;
        //        }
        //    }

        //    Session["contactoEliminar"] = contactoEliminar;

        //    String url = Page.ResolveUrl("~/Administracion/EliminarContacto.aspx");
        //    Response.Redirect(url);
        //}

        protected void Guardar_Click(object sender, EventArgs e)
        {
            if (PeriodosDDL.Items.Count > 0)
            {
                if (validarCampos())
                {
                    int idProyecto = 0;
                    int numeroSeleccionados = 0;

                    foreach (RepeaterItem item in rpProyectos.Items)
                    {
                        CheckBox cbProyecto = (CheckBox)item.FindControl("cbProyecto");

                        if (cbProyecto.Checked)
                        {
                            HiddenField IdProyecto = (HiddenField)item.FindControl("HFIdProyecto");
                            idProyecto = Convert.ToInt32(IdProyecto.Value.ToString());
                            numeroSeleccionados++;
                        }
                    }

                    if (idProyecto != 0 && numeroSeleccionados == 1)
                    {
                        if (!montoPresupuesto.Text.Trim().Equals(""))
                        {
                            Entidades.PresupuestoIngreso presupuestoIngreso = new Entidades.PresupuestoIngreso();
                            presupuestoIngreso.estado = false;
                            presupuestoIngreso.monto = Convert.ToDouble(montoPresupuesto.Text);

                            bool esInicial;
                            if (TipoPresupuesto.Text.Equals("Inicial"))
                            {
                                esInicial = true;
                            }
                            else
                            {
                                esInicial = false;

                            }

                            presupuestoIngreso.esInicial = esInicial;
                            presupuestoIngreso.proyecto = new Proyectos();
                            presupuestoIngreso.proyecto.idProyecto = idProyecto;

                            this.presupuestoServicios.InsertarPresupuestoIngreso(presupuestoIngreso);

                            Session["proyecto"] = idProyecto.ToString();
                            MostrarDatosTablaPresupuestos();
                            validarCampos();
                            montoPresupuesto.Text = "";
                        }
                    }
                }
            }
        }

        protected void Mostrar_Click(object sender, EventArgs e)
        {
            if (PeriodosDDL.Items.Count > 0)
            {
                int idProyecto = 0;
                int numeroSeleccionados = 0;

                foreach (RepeaterItem item in rpProyectos.Items)
                {
                    CheckBox cbProyecto = (CheckBox)item.FindControl("cbProyecto");

                    if (cbProyecto.Checked)
                    {
                        HiddenField IdProyecto = (HiddenField)item.FindControl("HFIdProyecto");
                        idProyecto = Convert.ToInt32(IdProyecto.Value.ToString());
                        numeroSeleccionados++;
                    }
                }

                if (idProyecto != 0 && numeroSeleccionados == 1)
                {
                    Session["proyecto"] = idProyecto.ToString();
                    MostrarDatosTablaPresupuestos();
                }
                else
                {
                    divMontoPresupuesto.Visible = false;
                }
            }
        }

        protected void Aprobar_OnClick(object sender, EventArgs e)
        {
            int idPresupuesto = Convert.ToInt32((((Button)(sender)).CommandArgument).ToString());

            if (idPresupuesto > 0)
            {
                int respuesta = this.presupuestoServicios.AprobarPresupuestoIngreso(idPresupuesto);

                if (respuesta > 0)
                {
                    //Button botonAprobar = (Button)(sender);
                    //botonAprobar.Enabled = false;

                    MostrarDatosTablaPresupuestos();
                }
            }
        }

        protected void Eliminar_OnClick(object sender, EventArgs e)
        {
            int idPresupuesto = Convert.ToInt32((((Button)(sender)).CommandArgument).ToString());

            if (idPresupuesto > 0)
            {
                if (!this.presupuestoServicios.ObtenerPorId(idPresupuesto).estado)
                {
                    int respuesta = this.presupuestoServicios.EliminarPresupuestoIngreso(idPresupuesto);

                    if (respuesta > 0)
                    {
                        MostrarDatosTablaPresupuestos();
                    }
                }
                else
                {
                    //ya el presupuesto se encuentra aprobado
                }
            }
        }

        protected void PeriodosDDL_OnChanged(object sender, EventArgs e)
        {
            MostrarDatosTablaProyectos();
        }
        #endregion
    }
}