using Entidades;
using PEP;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proyecto.Catalogos.Presupuesto
{
    public partial class AdministrarPresupuestoEgresos : System.Web.UI.Page
    {
        #region variables globales
        PeriodoServicios periodoServicios = new PeriodoServicios();
        ProyectoServicios proyectoServicios = new ProyectoServicios();
        UnidadServicios unidadServicios = new UnidadServicios();
        PresupuestoEgresosServicios presupuestoEgresosServicios = new PresupuestoEgresosServicios();
        PresupuestoIngresoServicios presupuestoIngresoServicios = new PresupuestoIngresoServicios();
        EstadoPresupuestoServicios estadoPresupuestoServicios = new EstadoPresupuestoServicios();
        PresupuestoEgreso_PartidaServicios presupuestoEgreso_PartidaServicios = new PresupuestoEgreso_PartidaServicios();
        static Entidades.PresupuestoEgreso presupuestoEgresoSeleccionado = new Entidades.PresupuestoEgreso();
        static PresupuestoEgresoPartida presupuestoEgresoPartidaSeleccionado = new PresupuestoEgresoPartida();
        static String[] info;
        static Double montoAprobado = 0, montoIngresos = 0;
        #endregion

        #region paginacion
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);
            if (!IsPostBack)
            {
                Session["listaPresupuestoEgresos"] = null;
                Session["listaPresupuestoEgresoPartidas"] = null;
                Session["listaPresupuestoEgresoPartidasEditar"] = null;
                Session["listaUnidades"] = null;
                llenarDdlPeriodos();

                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodos.SelectedValue);
            }
        }
        #endregion

        #region logica
        /// <summary>
        /// Leonardo Carrion
        /// 02/oct/2019
        /// Efecto: llean el DropDownList con los periodos que se encuentran en la base de datos
        /// Requiere: - 
        /// Modifica: DropDownList
        /// Devuelve: -
        /// </summary>
        public void llenarDdlPeriodos()
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            ddlPeriodos.Items.Clear();
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
                    ddlPeriodos.Items.Add(itemPeriodo);
                }

                if (anoHabilitado != 0)
                {
                    ddlPeriodos.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                }
            }

            CargarProyectos();
        }

        private void CargarProyectos()
        {
            ddlProyectos.Items.Clear();

            if (!ddlPeriodos.SelectedValue.Equals(""))
            {
                LinkedList<Proyectos> proyectos = new LinkedList<Proyectos>();
                proyectos = proyectoServicios.ObtenerPorPeriodo(Int32.Parse(ddlPeriodos.SelectedValue));

                if (proyectos.Count > 0)
                {
                    foreach (Proyectos proyecto in proyectos)
                    {
                        ListItem itemLB = new ListItem(proyecto.nombreProyecto, proyecto.idProyecto.ToString());
                        ddlProyectos.Items.Add(itemLB);
                    }

                    CargarUnidades();
                }
            }
        }

        private void CargarUnidades()
        {
            montoAprobado = 0;
            montoIngresos = 0;

            ddlUnidades.Items.Clear();

            if (!ddlProyectos.SelectedValue.Equals(""))
            {
                List<Unidad> unidades = new List<Unidad>();
                unidades = this.unidadServicios.ObtenerPorProyecto(Int32.Parse(ddlProyectos.SelectedValue));
                Session["listaUnidades"] = unidades;
                foreach (Unidad unidad in unidades)
                {
                    ListItem itemLB = new ListItem(unidad.nombreUnidad, unidad.idUnidad.ToString());
                    ddlUnidades.Items.Add(itemLB);
                }

            }

            cargarPlanEstrategico();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: muestra el plan estrategico y desabilita los botones de nuevo editar y aprobar segun sea el caso
        /// Requiere: unidad
        /// Modifica: datos de la tabla de plan estrategico y visibilidad de botones
        /// Devuelve: -
        /// </summary>
        private void cargarPlanEstrategico()
        {
            Unidad unidad = new Unidad();
            unidad.idUnidad = Convert.ToInt32(ddlUnidades.SelectedValue);

            List<Entidades.PresupuestoEgreso> listaPresupuestoEgresos = presupuestoEgresosServicios.getPresupuestosEgresosPorUnidad(unidad);

            if (listaPresupuestoEgresos.Count < 1)
            {
                listaPresupuestoEgresos = new List<Entidades.PresupuestoEgreso>();
                rpPlanEstrategico.DataSource = listaPresupuestoEgresos;
                btnNuevoPlan.Visible = true;
            }
            else
            {
                List<Entidades.PresupuestoEgreso> listaTemp = new List<Entidades.PresupuestoEgreso>();
                listaTemp.Add(listaPresupuestoEgresos.First());
                rpPlanEstrategico.DataSource = listaTemp;
                btnNuevoPlan.Visible = false;
            }
            rpPlanEstrategico.DataBind();

            Session["listaPresupuestoEgresos"] = listaPresupuestoEgresos;

            cargarPartidas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 08/oct/2019
        /// Efecto: carga las partidas con los montos segun la unidad seleccionada
        /// Requiere: seleccionar una unidad
        /// Modifica: tabla de partidas con los montos
        /// Devuelve: -
        /// </summary>
        private void cargarPartidas()
        {
            montoAprobado = 0;
            montoIngresos = 0;

            List<Entidades.PresupuestoEgreso> listaPresupuestoEgresos = (List<Entidades.PresupuestoEgreso>)Session["listaPresupuestoEgresos"];
            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartidas = new List<PresupuestoEgresoPartida>();
            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartidasTemp = new List<PresupuestoEgresoPartida>();

            foreach (Entidades.PresupuestoEgreso presupuestoEgreso in listaPresupuestoEgresos)
            {
                listaPresupuestoEgresoPartidas.AddRange(presupuestoEgreso_PartidaServicios.getPresupuestoEgresoPartidas(presupuestoEgreso));
            }

            foreach (PresupuestoEgresoPartida presupuestoEgresoPartida in listaPresupuestoEgresoPartidas)
            {
                Double monto = presupuestoEgresoPartida.monto;
                String descripcion = presupuestoEgresoPartida.descripcion;
                Boolean encontrado = false;
                foreach (PresupuestoEgresoPartida presupuestoEgresoPartidaTemp in listaPresupuestoEgresoPartidasTemp)
                {
                    if (presupuestoEgresoPartida.partida.idPartida == presupuestoEgresoPartidaTemp.partida.idPartida)
                    {
                        monto += presupuestoEgresoPartidaTemp.monto;
                        descripcion += "\n-------------------\n" + presupuestoEgresoPartidaTemp.descripcion;
                        encontrado = true;
                        presupuestoEgresoPartidaTemp.descripcion = descripcion;
                        presupuestoEgresoPartidaTemp.monto = monto;
                        if (!presupuestoEgresoPartida.estadoPresupuesto.descripcionEstado.Equals(presupuestoEgresoPartidaTemp.estadoPresupuesto.descripcionEstado))
                        {
                            presupuestoEgresoPartidaTemp.estadoPresupuesto.descripcionEstado = "Guardar";
                        }
                        break;
                    }
                }

                if (!encontrado)
                {
                    listaPresupuestoEgresoPartidasTemp.Add(presupuestoEgresoPartida);
                }
            }

            List<Unidad> unidades = (List<Unidad>)Session["listaUnidades"];
            foreach (Unidad unidad in unidades)
            {
                montoAprobado += (Double)presupuestoEgresosServicios.getPresupuestosEgresosPorUnidad(unidad).Sum(presupuesto => presupuesto.montoTotal);
            }

            lblMontoAprobado.Text = "El monto total aprobado es de ₡" + montoAprobado;

            Proyectos proyecto = new Proyectos();
            proyecto.idProyecto = Int32.Parse(ddlProyectos.SelectedValue);
            List<Entidades.PresupuestoIngreso> listaPresupuestosIngresos = presupuestoIngresoServicios.getPresupuestosIngresosPorProyecto(proyecto);

            foreach (Entidades.PresupuestoIngreso presupuestoIngreso in listaPresupuestosIngresos)
            {
                if (!presupuestoIngreso.estadoPresupIngreso.descEstado.Equals("Guardar"))
                {
                    montoIngresos += presupuestoIngreso.monto;
                }
            }

            lblMontoIngresos.Text = "El monto de ingreso total aprobado es de ₡" + montoIngresos;

            lblMontoGuardado.Text = "Monto total ingresado en la unidad es de ₡" + (Double)listaPresupuestoEgresoPartidasTemp.Sum(presupuesto => presupuesto.monto);

            Session["listaPresupuestoEgresoPartidas"] = listaPresupuestoEgresoPartidasTemp;

            rpPartidas.DataSource = listaPresupuestoEgresoPartidasTemp;
            rpPartidas.DataBind();
        }
        #endregion

        #region eventos
        /// <summary>
        /// Leonardo Carrion
        /// 02/oct/2019
        /// Efecto: cambia los datos de la tabla segun el periodo seleccionado
        /// Requiere: cambiar periodo
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodos.SelectedValue);

            CargarProyectos();
            CargarUnidades();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: cargar la tabla de plan estrategico y muestra los montos de las partidas
        /// Requiere: cambiar la unidad
        /// Modifica: plan estrategico y montos de las partidas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarUnidades();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: Muestra el plan estrategico de la unidad seleccionada y los montos de las partidas de dicha unidad
        /// Requiere: cambiar la unidad
        /// Modifica: plan estrategico y datos de las partidas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUnidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarPlanEstrategico();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 03/oct/2019
        /// Efecto: levanta modal para ingresar el plan estrategico
        /// Requiere: dar clic de boton de "Nuevo plan estrategico operacional"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoPlan_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoPlan();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 03/oct/2019
        /// Efecto: guarda los datos del plan estrategico del presupuesto de egresos
        /// Requiere: dar clic al boton de "Guardar" y llenar el campo de descripcion del plan estrategico
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoPlanModal_Click(object sender, EventArgs e)
        {
            if (txtDescNuevoPlanModal.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoPlan", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoPlan').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe ingresar un texto en el plan estrategico" + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoPlan();", true);
            }
            else
            {
                Unidad unidad = new Unidad();
                unidad.idUnidad = Convert.ToInt32(ddlUnidades.SelectedValue);

                Entidades.PresupuestoEgreso presupuestoEgreso = new Entidades.PresupuestoEgreso();

                presupuestoEgreso.unidad = unidad;
                presupuestoEgreso.montoTotal = 0;
                presupuestoEgreso.planEstrategicoOperacional = txtDescNuevoPlanModal.Text;

                presupuestoEgresosServicios.InsertarPresupuestoEgreso(presupuestoEgreso);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoPlan", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoPlan').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se ingreso correctamente el plan estrategico" + "');", true);
                cargarPlanEstrategico();
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: levanta el modal para editar el plan estrategico
        /// Requiere: dar clic al boton de "Editar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int idPresupuestoEgreso = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Entidades.PresupuestoEgreso> listaPresupuestoEgresos = (List<Entidades.PresupuestoEgreso>)Session["listaPresupuestoEgresos"];

            foreach (Entidades.PresupuestoEgreso presupuestoEgreso in listaPresupuestoEgresos)
            {
                if (presupuestoEgreso.idPresupuestoEgreso == idPresupuestoEgreso)
                {
                    presupuestoEgresoSeleccionado = presupuestoEgreso;
                    break;
                }
            }

            txtDescEditarPlanModal.Text = presupuestoEgresoSeleccionado.planEstrategicoOperacional;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarPlan();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: actualiza la descripcion del plan estrategico
        /// Requiere: dar clic al boton de "Actualizar" y poner algun texto en la descripcion
        /// Modifica: el plan estrategico del presupuesto seleccionado
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditarPlanModal_Click(object sender, EventArgs e)
        {
            if (txtDescEditarPlanModal.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarPlan", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarPlan').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe ingresar un texto en el plan estrategico" + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarPlan();", true);
            }
            else
            {
                presupuestoEgresoSeleccionado.planEstrategicoOperacional = txtDescEditarPlanModal.Text;

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarPlan", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarPlan').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se actualizo correctamente el plan estrategico" + "');", true);
                presupuestoEgresosServicios.actualizarPlanEstrategicoPresupuestoEgreso(presupuestoEgresoSeleccionado);

                cargarPlanEstrategico();

            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: levanta el modal para eliminar el plan estrategico
        /// Requiere: dar clic al boton de "Eliminar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int idPresupuestoEgreso = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Entidades.PresupuestoEgreso> listaPresupuestoEgresos = (List<Entidades.PresupuestoEgreso>)Session["listaPresupuestoEgresos"];

            foreach (Entidades.PresupuestoEgreso presupuestoEgreso in listaPresupuestoEgresos)
            {
                if (presupuestoEgreso.idPresupuestoEgreso == idPresupuestoEgreso)
                {
                    presupuestoEgresoSeleccionado = presupuestoEgreso;
                    break;
                }
            }

            txtDescEliminarPlanModal.Text = presupuestoEgresoSeleccionado.planEstrategicoOperacional;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarPlan();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/oct/2019
        /// Efecto: elimina el plan estrategico seleccionado
        /// Requiere: dar clic en el boton de "Eliminar"
        /// Modifica: tabla de plan estrategico
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarPlanModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarPlan", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarPlan').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se elimino correctamente el plan estrategico" + "');", true);
            presupuestoEgresoSeleccionado.planEstrategicoOperacional = "";
            presupuestoEgresosServicios.actualizarPlanEstrategicoPresupuestoEgreso(presupuestoEgresoSeleccionado);

            cargarPlanEstrategico();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 09/oct/2019
        /// Efecto: guarda los montos y descripciones que se ingresan
        /// Requiere: dar clic al boton de "Guardar"
        /// Modifica: habilita botones y deshabilita campos para llenar
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartidas = (List<PresupuestoEgresoPartida>)Session["listaPresupuestoEgresoPartidas"];
            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartidasTemp = new List<PresupuestoEgresoPartida>();

            foreach (RepeaterItem item in rpPartidas.Items)
            {
                HiddenField hdIdPartida = (HiddenField)item.FindControl("hdIdPartida");
                TextBox txtMonto = (TextBox)item.FindControl("txtMonto");
                TextBox txtDescripcion = (TextBox)item.FindControl("txtDescripcion");

                foreach (Entidades.PresupuestoEgresoPartida presupuestoEgresoPartida in listaPresupuestoEgresoPartidas)
                {
                    int idPartida = Convert.ToInt32(hdIdPartida.Value);
                    if (idPartida == presupuestoEgresoPartida.partida.idPartida)
                    {
                        String txtMont = txtMonto.Text.Replace(".", ",");
                        if (Double.TryParse(txtMont, out Double monto))
                        {
                            txtMonto.Text = monto.ToString();
                        }
                        presupuestoEgresoPartida.monto = monto;
                        presupuestoEgresoPartida.descripcion = txtDescripcion.Text;

                        EstadoPresupuesto estadoPresupuesto = new EstadoPresupuesto();
                        estadoPresupuesto = estadoPresupuestoServicios.getEstadoPresupuestoPorNombre("Guardar");

                        presupuestoEgresoPartida.estadoPresupuesto = estadoPresupuesto;

                        listaPresupuestoEgresoPartidasTemp.Add(presupuestoEgresoPartida);

                        if (presupuestoEgresoPartida.monto > 0 || !String.IsNullOrEmpty(presupuestoEgresoPartida.descripcion))
                        {
                            if (!txtMonto.ReadOnly)
                            {
                                presupuestoEgreso_PartidaServicios.insertarPresupuestoEgreso_Partida(presupuestoEgresoPartida);
                            }
                        }
                    }
                }
            }

            cargarPartidas();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se guardo correctamente el presupuesto" + "');", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 09/oct/2019
        /// Efecto: levanta el modal para aregar monto y descripcion 
        /// Requiere: dar clic al boton de "Agregar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarPartida_Click(object sender, EventArgs e)
        {
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartidas = (List<PresupuestoEgresoPartida>)Session["listaPresupuestoEgresoPartidas"];

            foreach (PresupuestoEgresoPartida presupuestoEgresoPartida in listaPresupuestoEgresoPartidas)
            {
                if (presupuestoEgresoPartida.partida.idPartida == idPartida)
                {
                    presupuestoEgresoPartidaSeleccionado = presupuestoEgresoPartida;
                    break;
                }
            }
            txtMontoModalAgregarPartida.Text = "";
            txtDescripcionModalAgregarPartida.Text = "";
            lblPartidaSeleccionadaModalAgregar.Text = presupuestoEgresoPartidaSeleccionado.partida.numeroPartida + " " + presupuestoEgresoPartidaSeleccionado.partida.descripcionPartida;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalAgregarPartida();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 09/oct/2019
        /// Efecto: Guarda la informacion del monto y descripcion en la base de datos
        /// Requiere: monto y descripcion y dar clic al boton de "Guardar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarModalPartida_Click(object sender, EventArgs e)
        {
            String txtMont = txtMontoModalAgregarPartida.Text.Replace(".", ",");
            if (Double.TryParse(txtMont, out Double monto))
            {
                txtMontoModalAgregarPartida.Text = monto.ToString();
            }

            if (String.IsNullOrEmpty(txtDescripcionModalAgregarPartida.Text))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalAgregarPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalAgregarPartida').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Se debe agregar una descripcion" + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalAgregarPartida();", true);
            }
            else
            {
                EstadoPresupuesto estadoPresupuesto = new EstadoPresupuesto();
                estadoPresupuesto = estadoPresupuestoServicios.getEstadoPresupuestoPorNombre("Guardar");

                PresupuestoEgresoPartida presupuestoEgresoPartida = new PresupuestoEgresoPartida();
                presupuestoEgresoPartida.estadoPresupuesto = estadoPresupuesto;
                presupuestoEgresoPartida.descripcion = txtDescripcionModalAgregarPartida.Text;
                presupuestoEgresoPartida.monto = monto;
                presupuestoEgresoPartida.partida = presupuestoEgresoPartidaSeleccionado.partida;
                presupuestoEgresoPartida.idPresupuestoEgreso = presupuestoEgresoPartidaSeleccionado.idPresupuestoEgreso;

                presupuestoEgreso_PartidaServicios.insertarPresupuestoEgreso_Partida(presupuestoEgresoPartida);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalAgregarPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalAgregarPartida').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se guardo correctamente la información" + "');", true);

                cargarPartidas();
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 09/oct/2019
        /// Efecto: levanta modal para ver la descripcion de la partida seleccionada, agrega todas las descripciones
        /// Requiere: dar clic en el boton de "Ver"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVerDescripcion_Click(object sender, EventArgs e)
        {
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartidas = (List<PresupuestoEgresoPartida>)Session["listaPresupuestoEgresoPartidas"];

            foreach (PresupuestoEgresoPartida presupuestoEgresoPartida in listaPresupuestoEgresoPartidas)
            {
                if (presupuestoEgresoPartida.partida.idPartida == idPartida)
                {
                    presupuestoEgresoPartidaSeleccionado = presupuestoEgresoPartida;
                    break;
                }
            }

            lblPartidaSeleccionadaModalVerDescripcion.Text = presupuestoEgresoPartidaSeleccionado.partida.numeroPartida + " " + presupuestoEgresoPartidaSeleccionado.partida.descripcionPartida;
            txtDescripcionModalVerDescripcionPartida.Text = presupuestoEgresoPartidaSeleccionado.descripcion;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalVerDescripcionPartida();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 09/oct/2019
        /// Efecto: levanta el modal con los egresos que se han realizado en la partida seleccionada
        /// Requiere: dar clic en el boton de "Editar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditarPartida_Click(object sender, EventArgs e)
        {
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartidas = (List<PresupuestoEgresoPartida>)Session["listaPresupuestoEgresoPartidas"];

            foreach (PresupuestoEgresoPartida presupuestoEgresoPartida in listaPresupuestoEgresoPartidas)
            {
                if (presupuestoEgresoPartida.partida.idPartida == idPartida)
                {
                    presupuestoEgresoPartidaSeleccionado = presupuestoEgresoPartida;
                    break;
                }
            }

            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartidasEditar = presupuestoEgreso_PartidaServicios.getPresupuestoEgresoPartidasPorPartidaYPresupEgreso(presupuestoEgresoPartidaSeleccionado);

            Session["listaPresupuestoEgresoPartidasEditar"] = listaPresupuestoEgresoPartidasEditar;

            rpPartidasEditar.DataSource = listaPresupuestoEgresoPartidasEditar;
            rpPartidasEditar.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditar();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto: actualiza el monto y la descripcion del presupuesto partida que se encuentran en el modal
        /// Requiere:  dar clic al boton de "Actualizar"
        /// Modifica: monto y descripcion de los presupuetos partidas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizarModalEditar_Click(object sender, EventArgs e)
        {
            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartidasEditar = (List<PresupuestoEgresoPartida>)Session["listaPresupuestoEgresoPartidasEditar"];

            foreach (RepeaterItem item in rpPartidasEditar.Items)
            {
                HiddenField hdIdPartida = (HiddenField)item.FindControl("hdIdPartida");
                HiddenField hdIdPresupuesto = (HiddenField)item.FindControl("hdIdPresupuesto");
                HiddenField hdIdLinea = (HiddenField)item.FindControl("hdIdLinea");
                TextBox txtMonto = (TextBox)item.FindControl("txtMonto");
                TextBox txtDescripcion = (TextBox)item.FindControl("txtDescripcion");

                foreach (Entidades.PresupuestoEgresoPartida presupuestoEgresoPartida in listaPresupuestoEgresoPartidasEditar)
                {
                    int idPartida = Convert.ToInt32(hdIdPartida.Value);
                    int idPresupuesto = Convert.ToInt32(hdIdPresupuesto.Value);
                    int idLinea = Convert.ToInt32(hdIdLinea.Value);
                    if (idPartida == presupuestoEgresoPartida.partida.idPartida && idPresupuesto == presupuestoEgresoPartida.idPresupuestoEgreso && idLinea == presupuestoEgresoPartida.idLinea)
                    {
                        String txtMont = txtMonto.Text.Replace(".", ",");
                        if (Double.TryParse(txtMont, out Double monto))
                        {
                            txtMonto.Text = monto.ToString();
                        }
                        presupuestoEgresoPartida.monto = monto;
                        presupuestoEgresoPartida.descripcion = txtDescripcion.Text;

                        presupuestoEgreso_PartidaServicios.actualizarPresupuestoEgreso_Partida(presupuestoEgresoPartida);
                    }
                }

            }

            cargarPartidas();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditar", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditar').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se actualizo correctamente la información" + "');", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto: guarda en el vector de info, la informacion del presupuesto egreso partida seleccionado y levanta el modal de confirmacion de eliminacion
        /// Requiere: dar clic al boton de "Eliminar"
        /// Modifica: - 
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarModalEditar_Click(object sender, EventArgs e)
        {
            info = (((LinkButton)(sender)).CommandArgument).ToString().Split('~');
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarEditar();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto: no elimina el presupuesto de egreso partida y levanta el modal de  editar presupuesto egreso partida
        /// Requiere: dar clic al boton de "No"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNoEliminarEditar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarEditar", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarEditar').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditar();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto:
        /// Requiere:
        /// Modifica:
        /// Devuelve:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSiEliminarEditar_Click(object sender, EventArgs e)
        {
            PresupuestoEgresoPartida presupuestoEgresoPartida = new PresupuestoEgresoPartida();
            Partida partida = new Partida();
            partida.idPartida = Convert.ToInt32(info[0]);
            presupuestoEgresoPartida.idLinea = Convert.ToInt32(info[2]);
            presupuestoEgresoPartida.idPresupuestoEgreso = Convert.ToInt32(info[1]);
            presupuestoEgresoPartida.partida = partida;
            presupuestoEgreso_PartidaServicios.eliminarPresupuestoEgreso_Partida(presupuestoEgresoPartida);

            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartidasEditar = presupuestoEgreso_PartidaServicios.getPresupuestoEgresoPartidasPorPartidaYPresupEgreso(presupuestoEgresoPartidaSeleccionado);

            Session["listaPresupuestoEgresoPartidasEditar"] = listaPresupuestoEgresoPartidasEditar;

            rpPartidasEditar.DataSource = listaPresupuestoEgresoPartidasEditar;
            rpPartidasEditar.DataBind();

            cargarPartidas();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se elimino correctamente la información" + "');", true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarEditar", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarEditar').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditar();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto: guarda en el vector de info, la informacion del presupuesto egreso partida seleccionado y levanta el modal de confirmacion de aprobacion
        /// Requiere: dar clic al boton de "Aprobar"
        /// Modifica: - 
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAprobarModalEditar_Click(object sender, EventArgs e)
        {
            info = (((LinkButton)(sender)).CommandArgument).ToString().Split('~');
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalAprobarEditar();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto: no aprueba el presupuesto de egreso partida y levanta el modal de editar
        /// Requiere: dar clic al boton de "No"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNoAprobarEditar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalAprobarEditar", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalAprobarEditar').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditar();", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 10/oct/2019
        /// Efecto: aprueba el presupuesto de egreso seleccionado
        /// Requiere: dar el boton de "Si"
        /// Modifica: el estado del prespuesto de egreso partida
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSiAprobarEditar_Click(object sender, EventArgs e)
        {
            PresupuestoEgresoPartida presupuestoEgresoPartida = new PresupuestoEgresoPartida();
            Partida partida = new Partida();
            EstadoPresupuesto estadoPresupuesto = new EstadoPresupuesto();
            estadoPresupuesto = estadoPresupuestoServicios.getEstadoPresupuestoPorNombre("Aprobar");
            partida.idPartida = Convert.ToInt32(info[0]);
            presupuestoEgresoPartida.idLinea = Convert.ToInt32(info[2]);
            presupuestoEgresoPartida.idPresupuestoEgreso = Convert.ToInt32(info[1]);
            presupuestoEgresoPartida.monto = Convert.ToDouble(info[3]);
            presupuestoEgresoPartida.partida = partida;
            presupuestoEgresoPartida.estadoPresupuesto = estadoPresupuesto;

            Double montoA = montoAprobado + presupuestoEgresoPartida.monto;

            if (montoIngresos >= montoA)
            {
                presupuestoEgreso_PartidaServicios.actualizarEstadoPresupuestoEgreso_Partida(presupuestoEgresoPartida);

                List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartidasEditar = presupuestoEgreso_PartidaServicios.getPresupuestoEgresoPartidasPorPartidaYPresupEgreso(presupuestoEgresoPartidaSeleccionado);
                presupuestoEgresoSeleccionado.idPresupuestoEgreso = presupuestoEgresoPartida.idPresupuestoEgreso;
                presupuestoEgresoSeleccionado = presupuestoEgresosServicios.getPresupuestosEgresosPorId(presupuestoEgresoSeleccionado); 
                presupuestoEgresoSeleccionado.montoTotal += presupuestoEgresoPartida.monto;
                presupuestoEgresosServicios.actualizarMontoPresupuestoEgreso(presupuestoEgresoSeleccionado);

                Session["listaPresupuestoEgresoPartidasEditar"] = listaPresupuestoEgresoPartidasEditar;

                rpPartidasEditar.DataSource = listaPresupuestoEgresoPartidasEditar;
                rpPartidasEditar.DataBind();

                cargarPartidas();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se aprobo correctamente la información" + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El monto aprobado es mayor al monto de ingresos. No se puede aprobar" + "');", true);
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalAprobarEditar", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalAprobarEditar').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditar();", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 11/oct/2019
        /// Efecto: levante modal de mensaje de aprobar
        /// Requiere: dar clic al boton de "Aprobar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalMensajeAprobar();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/oct/2019
        /// Efecto: revisa si se puede aprobar los montos ingresados en la unidad seleccionada, si se puede aprueba todos los montos
        /// Requiere: dar clic en el boton de "Si"
        /// Modifica: estado de presupuesto egresos partidas y el monto aprobado
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSiMensajeAprobar_Click(object sender, EventArgs e)
        {
            List<Entidades.PresupuestoEgreso> listaPresupuestoEgresos = (List<Entidades.PresupuestoEgreso>)Session["listaPresupuestoEgresos"];
            List<PresupuestoEgresoPartida> listaPresupuestoEgresoPartidas = new List<PresupuestoEgresoPartida>();

            foreach (Entidades.PresupuestoEgreso presupuestoEgreso in listaPresupuestoEgresos)
            {
                listaPresupuestoEgresoPartidas.AddRange(presupuestoEgreso_PartidaServicios.getPresupuestoEgresoPartidas(presupuestoEgreso));
            }

            Double monto = (Double)listaPresupuestoEgresoPartidas.Sum(presupuesto => presupuesto.monto);

            if(monto <= montoIngresos && montoIngresos>=(montoAprobado+monto))
            {
                foreach (PresupuestoEgresoPartida presupuestoEgresoPartida in listaPresupuestoEgresoPartidas)
                {
                    EstadoPresupuesto estadoPresupuesto = new EstadoPresupuesto();
                    estadoPresupuesto = estadoPresupuestoServicios.getEstadoPresupuestoPorNombre("Aprobar");
                    presupuestoEgresoPartida.estadoPresupuesto = estadoPresupuesto;

                    presupuestoEgreso_PartidaServicios.actualizarEstadoPresupuestoEgreso_Partida(presupuestoEgresoPartida);

                    presupuestoEgresoSeleccionado.idPresupuestoEgreso = presupuestoEgresoPartida.idPresupuestoEgreso;
                }

                presupuestoEgresoSeleccionado = presupuestoEgresosServicios.getPresupuestosEgresosPorId(presupuestoEgresoSeleccionado);
                presupuestoEgresoSeleccionado.montoTotal = monto;
                presupuestoEgresosServicios.actualizarMontoPresupuestoEgreso(presupuestoEgresoSeleccionado);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalMensajeAprobar", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalMensajeAprobar').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se aprobaron todos los montos guardados" + "');", true);

                cargarPartidas();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalMensajeAprobar", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalMensajeAprobar').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El monto que se va aprobar es mayor al monto de ingresos aprobados" + "');", true);
            }
            
        }
        #endregion

    }
}