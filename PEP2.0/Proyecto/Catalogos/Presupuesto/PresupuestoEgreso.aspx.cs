using Servicios;
using Entidades;
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
    public partial class PresupuestoEgreso : System.Web.UI.Page
    {
        #region variables globales
        private PeriodoServicios periodoServicios;
        private ProyectoServicios proyectoServicios;
        private UnidadServicios unidadServicios;
        private PartidaServicios partidaServicios;
        private PresupuestoServicios presupuestoServicios;
        readonly PagedDataSource pgsource = new PagedDataSource();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            PEP.Utilidades.escogerMenu(Page, rolesPermitidos);

            this.periodoServicios = new PeriodoServicios();
            this.proyectoServicios = new ProyectoServicios();
            this.unidadServicios = new UnidadServicios();
            this.partidaServicios = new PartidaServicios();
            this.presupuestoServicios = new PresupuestoServicios();

            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());

                Session["idPresupuestoEgreso"] = 0;
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
            int anoHabilitado = 0;

            if (periodos.Count > 0)
            {
                foreach (Periodo periodo in periodos)
                {
                    string nombre;

                    if (periodo.habilitado)
                    {
                        nombre = periodo.anoPeriodo.ToString() + " (Actual)";
                        anoHabilitado = periodo.anoPeriodo;
                    }
                    else
                    {
                        nombre = periodo.anoPeriodo.ToString();
                    }

                    ListItem itemPeriodo = new ListItem(nombre, periodo.anoPeriodo.ToString());
                    PeriodosDDL.Items.Add(itemPeriodo);
                }

                if (anoHabilitado != 0)
                {
                    PeriodosDDL.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                }

                CargarProyectos();
                MostrarDatosTabla();

                LlenarTabla();
            }
        }

        private void CargarProyectos()
        {
            ProyectosDDL.Items.Clear();

            if (!PeriodosDDL.SelectedValue.Equals(""))
            {
                LinkedList<Proyectos> proyectos = new LinkedList<Proyectos>();
                proyectos = this.proyectoServicios.ObtenerPorPeriodo(Int32.Parse(PeriodosDDL.SelectedValue));

                if (proyectos.Count > 0)
                {
                    foreach (Proyectos proyecto in proyectos)
                    {
                        ListItem itemLB = new ListItem(proyecto.nombreProyecto, proyecto.idProyecto.ToString());
                        ProyectosDDL.Items.Add(itemLB);
                    }

                    CargarUnidades();
                }
            }
        }

        private void CargarUnidades()
        {
            UnidadesDDL.Items.Clear();

            if (!ProyectosDDL.SelectedValue.Equals(""))
            {
                LblPresupuestoIngreso.Text = "El monto disponible para el proyecto seleccionado es " + String.Format("{0:N}", MontoPresupuestoIngreso()) + " colones";

                LinkedList<Unidad> unidades = new LinkedList<Unidad>();
                unidades = this.unidadServicios.ObtenerPorProyecto(Int32.Parse(ProyectosDDL.SelectedValue));

                foreach (Unidad unidad in unidades)
                {
                    ListItem itemLB = new ListItem(unidad.nombreUnidad, unidad.idUnidad.ToString());
                    UnidadesDDL.Items.Add(itemLB);
                }

                LlenarTabla();
            }
        }

        private void LlenarTabla()
        {
            if (!UnidadesDDL.SelectedValue.Equals(""))
            {
                LinkedList<Entidades.PresupuestoEgreso> presupuestoEgresos = this.presupuestoServicios.ObtenerPorUnidad(Int32.Parse(UnidadesDDL.SelectedValue));
                double totalIngreso = MontoPresupuestoIngreso();
                bool aprobadoGuardar = true;
                bool aprobadoAprobar = false;

                if (presupuestoEgresos.Count == 0)
                {
                    txtPAO.Text = "";

                    foreach (RepeaterItem item in rpPartida.Items)
                    {
                        TextBox tbMonto = (TextBox)item.FindControl("TbMonto");
                        Label lbTotal = (Label)item.FindControl("LBTotal");

                        HiddenField IdPartida = (HiddenField)item.FindControl("HFIdPartida");
                        int idPartida = Convert.ToInt32(IdPartida.Value.ToString());

                        TextBox tbDescripcion = (TextBox)item.FindControl("TbDescripcion");

                        tbMonto.Text = String.Format("{0:N}", "0");
                        lbTotal.Text = String.Format("{0:N}", "0");
                        tbDescripcion.Text = "";
                    }
                }

                foreach (Entidades.PresupuestoEgreso presupuestoEgreso in presupuestoEgresos)
                {
                    txtPAO.Text = presupuestoEgreso.planEstrategicoOperacional;

                    foreach (RepeaterItem item in rpPartida.Items)
                    {
                        TextBox tbMonto = (TextBox)item.FindControl("TbMonto");
                        Label lbTotal = (Label)item.FindControl("LBTotal");
                        TextBox tbDescripcion = (TextBox)item.FindControl("TbDescripcion");

                        HiddenField IdPartida = (HiddenField)item.FindControl("HFIdPartida");
                        int idPartida = Convert.ToInt32(IdPartida.Value.ToString());

                        if (presupuestoEgreso.presupuestoEgresoPartidas != null)
                        {
                            foreach (PresupuestoEgresoPartida presupuestoEgresoPartida in presupuestoEgreso.presupuestoEgresoPartidas)
                            {
                                if (idPartida == presupuestoEgresoPartida.idPartida)
                                {
                                    if (presupuestoEgreso.estado)
                                    {
                                        double total = Convert.ToDouble(lbTotal.Text);
                                        tbMonto.Text = String.Format("{0:N}", "0");
                                        lbTotal.Text = String.Format("{0:N}", (total + presupuestoEgresoPartida.monto).ToString());
                                    }
                                    else
                                    {
                                        tbMonto.Text = String.Format("{0:N}", presupuestoEgresoPartida.monto.ToString());
                                    }
                                    tbDescripcion.Text = presupuestoEgresoPartida.descripcion;
                                }
                            }
                        }
                    }

                    if (presupuestoEgreso.estado)
                    {
                        totalIngreso =  totalIngreso - presupuestoEgreso.montoTotal;
                        aprobadoGuardar = true;
                        aprobadoAprobar = false;
                    }
                    else
                    {
                        aprobadoGuardar = false;
                        aprobadoAprobar = true;
                        Session["idPresupuestoEgreso"]  = presupuestoEgreso.idPresupuestoEgreso;
                    }
                }

                btnGuardar.Enabled = aprobadoGuardar;
                btnAprobar.Enabled = aprobadoAprobar;
                LblPresupuestoIngreso.Text = "El monto disponible para el proyecto seleccionado es " + String.Format("{0:N}", totalIngreso) + " colones";
            }
        }

        /// <summary>
        /// LLena y muestra la tabla de proyectos basado en el periodo seleccionado
        /// </summary>
        private void MostrarDatosTabla()
        {
            if (PeriodosDDL.Items.Count > 0)
            {
                LinkedList<Partida> listaPartidas = new LinkedList<Partida>();
                LinkedList<Partida> listaPartidasDT = new LinkedList<Partida>();
                listaPartidas = partidaServicios.ObtenerPorPeriodo(Convert.ToInt32(PeriodosDDL.SelectedValue));

                foreach (Partida partida in listaPartidas)
                {
                    if (partida.partidaPadre != null)
                    {
                        listaPartidasDT.AddLast(partida);
                    }
                }

                var dt = listaPartidasDT;
                pgsource.DataSource = dt;
                pgsource.AllowPaging = false;

                ViewState["TotalPaginas"] = pgsource.PageCount;

                rpPartida.DataSource = pgsource;
                rpPartida.DataBind();
            }
        }

        private double MontoPresupuestoIngreso()
        {
            LinkedList<Entidades.PresupuestoIngreso> presupuestoIngresos = new LinkedList<Entidades.PresupuestoIngreso>();
            presupuestoIngresos = this.presupuestoServicios.ObtenerPorProyecto(Int32.Parse(ProyectosDDL.SelectedValue));
            double montoTotalPresupuestoIngreso = 0;

            foreach (Entidades.PresupuestoIngreso presupuestoIngreso in presupuestoIngresos)
            {
                if (presupuestoIngreso.estado)
                {
                    montoTotalPresupuestoIngreso += presupuestoIngreso.monto;
                }
            }

            return montoTotalPresupuestoIngreso;
        }
        #endregion

        #region eventos
        protected void Unidades_OnChanged(object sender, EventArgs e)
        {
            LlenarTabla();
        }

        protected void Periodos_OnChanged(object sender, EventArgs e)
        {
            MostrarDatosTabla();
            CargarProyectos();
        }

        protected void Proyectos_OnChanged(object sender, EventArgs e)
        {
            CargarUnidades();
        }

        protected void Guardar_Click(object sender, EventArgs e)
        {
            if (UnidadesDDL.Items.Count > 0)
            {
                Session["idPresupuestoEgreso"] = 0;

                Entidades.PresupuestoEgreso presupuestoEgreso = new Entidades.PresupuestoEgreso();
                presupuestoEgreso.idUnidad = Convert.ToInt32(UnidadesDDL.SelectedValue);
                presupuestoEgreso.planEstrategicoOperacional = txtPAO.Text;
                LinkedList<PresupuestoEgresoPartida> presupuestoEgresoPartidas = new LinkedList<PresupuestoEgresoPartida>();
                double monto = 0;

                foreach (RepeaterItem item in rpPartida.Items)
                {

                    //private int y;

                    PresupuestoEgresoPartida presupuestoEgresoPartida = new PresupuestoEgresoPartida();

                    TextBox tbMonto = (TextBox)item.FindControl("TbMonto");
                    TextBox tbDescripcion = (TextBox)item.FindControl("TbDescripcion");

                    HiddenField IdPartida = (HiddenField)item.FindControl("HFIdPartida");
                    int idPartida = Convert.ToInt32(IdPartida.Value.ToString());

                    monto += Convert.ToDouble(tbMonto.Text);

                    presupuestoEgresoPartida.idPartida = idPartida;
                    presupuestoEgresoPartida.monto = Convert.ToDouble(tbMonto.Text);
                    presupuestoEgresoPartida.descripcion = tbDescripcion.Text;

                    presupuestoEgresoPartidas.AddLast(presupuestoEgresoPartida);
                }

                presupuestoEgreso.montoTotal = monto;
                presupuestoEgreso.presupuestoEgresoPartidas = presupuestoEgresoPartidas;

                int idPresupuestoEgreso = this.presupuestoServicios.InsertarPresupuestoEgreso(presupuestoEgreso);
                Session["idPresupuestoEgreso"] = idPresupuestoEgreso;

                LlenarTabla();
            }
        }

        protected void Aprobar_Click(object sender, EventArgs e)
        {
            if (UnidadesDDL.Items.Count > 0)
            {
                //No se puede hacer con sesion porque no siempre la acciona de aprobar va despues de guardar
                if (Session["idPresupuestoEgreso"] != null)
                {
                    int idPresupuestoEgreso = Convert.ToInt32(Session["idPresupuestoEgreso"]);

                    if (idPresupuestoEgreso > 0)
                    {
                        double montoPresupuestoIngreso = MontoPresupuestoIngreso();
                        double montoTotalPresupuestoEgreso = 0;

                        foreach (RepeaterItem item in rpPartida.Items)
                        {
                            TextBox tbMonto = (TextBox)item.FindControl("TbMonto");
                            montoTotalPresupuestoEgreso += Convert.ToDouble(tbMonto.Text.ToString());
                        }

                        if (montoTotalPresupuestoEgreso <= montoPresupuestoIngreso)
                        {
                            int respuesta = this.presupuestoServicios.AprobarPresupuestoEgreso(idPresupuestoEgreso);

                            if (respuesta != 0)
                            {
                                LlenarTabla();
                            }
                        }
                        else
                        {
                            Toastr("warning", "No es posible realizar la aprobación debido a que no hay suficientes ingresos");
                        }
                    }
                }
            }
        }

        #endregion

        #region Util
        private void Toastr(string tipo, string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr." + tipo + "('" + mensaje + "');", true);
        }
        #endregion
    }
}