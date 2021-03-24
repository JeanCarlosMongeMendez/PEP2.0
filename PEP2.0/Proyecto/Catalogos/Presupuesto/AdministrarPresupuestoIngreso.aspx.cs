using Entidades;
using PEP;
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
    public partial class AdministrarPresupuestoIngreso : System.Web.UI.Page
    {
        #region variables globales
        PeriodoServicios periodoServicios = new PeriodoServicios();
        ProyectoServicios proyectoServicios = new ProyectoServicios();
        PresupuestoIngresoServicios presupuestoIngresoServicios = new PresupuestoIngresoServicios();
        EstadoPresupIngresoServicios estadoPresupIngresoServicios = new EstadoPresupIngresoServicios();
        static Proyectos proyectoSeleccionado = new Proyectos();
        static Entidades.PresupuestoIngreso presupuestoIngresoSeleccionado = new Entidades.PresupuestoIngreso();
        #endregion

        #region paginacion
        readonly PagedDataSource pgsource = new PagedDataSource();
        int primerIndex, ultimoIndex, primerIndex2, ultimoIndex2;
        private int elmentosMostrar = 10;
        private int paginaActual
        {
            get
            {
                if (ViewState["paginaActual"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["paginaActual"]);
            }
            set
            {
                ViewState["paginaActual"] = value;
            }
        }

        private int paginaActual2
        {
            get
            {
                if (ViewState["paginaActual2"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["paginaActual2"]);
            }
            set
            {
                ViewState["paginaActual2"] = value;
            }
        }
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
                Session["listaProyectos"] = null;
                Session["listaProyectosFiltrada"] = null;

                Session["listaPresupuestosIngresos"] = null;
                llenarDdlPeriodos();

                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

                List<Proyectos> listaProyectos = proyectoServicios.getProyectosPorPeriodo(periodo);

                Session["listaProyectos"] = listaProyectos;
                Session["listaProyectosFiltrada"] = listaProyectos;

                mostrarDatosTabla();
            }
        }
        #endregion

        #region logica
        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: llean el DropDownList con los periodos que se encuentran en la base de datos
        /// Requiere: - 
        /// Modifica: DropDownList
        /// Devuelve: -
        /// </summary>
        public void llenarDdlPeriodos()
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            ddlPeriodo.Items.Clear();
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
                    ddlPeriodo.Items.Add(itemPeriodo);
                }

                if (anoHabilitado != 0)
                {
                    ddlPeriodo.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                }
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 29/sep/2019
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTabla()
        {
            List<Proyectos> listaProyectos = (List<Proyectos>)Session["listaProyectos"];

            String nombre = "";

            if (!String.IsNullOrEmpty(txtBuscarNombre.Text))
            {
                nombre = txtBuscarNombre.Text;
            }

            List<Proyectos> listaProyectosFiltrada = (List<Proyectos>)listaProyectos.Where(proyecto => proyecto.nombreProyecto.ToUpper().Contains(nombre.ToUpper())).ToList();

            Session["listaProyectosFiltrada"] = listaProyectosFiltrada;

            var dt = listaProyectosFiltrada;
            pgsource.DataSource = dt;
            pgsource.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina.Text = "Página " + (paginaActual + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior.Enabled = !pgsource.IsFirstPage;
            lbSiguiente.Enabled = !pgsource.IsLastPage;
            lbPrimero.Enabled = !pgsource.IsFirstPage;
            lbUltimo.Enabled = !pgsource.IsLastPage;

            rpProyectos.DataSource = pgsource;
            rpProyectos.DataBind();

            //metodo que realiza la paginacion
            Paginacion();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: carga la tabla de ingresos y pone la paginacion
        /// Requiere: lista de presupuestos de ingresos en variable de session
        /// Modifica: datos de la tabla de ingresos
        /// Devuelve: -
        /// </summary>
        private void cargarDatosTblIngresos()
        {
            List<Entidades.PresupuestoIngreso> listaSession = (List<Entidades.PresupuestoIngreso>)Session["listaPresupuestosIngresos"];

            Double montoTotal = 0, montoAprobado = 0;

            foreach (Entidades.PresupuestoIngreso presupuestoIngreso in listaSession)
            {
                if (!presupuestoIngreso.estadoPresupIngreso.descEstado.Equals("Guardar"))
                {
                    montoAprobado += presupuestoIngreso.monto;
                }
                montoTotal += presupuestoIngreso.monto;
            }

            if (montoTotal != montoAprobado)
            {
                btnNuevoIngreso.Visible = false;
            }
            else
            {
                btnNuevoIngreso.Visible = true;
            }

            lblMontoAprobado.Text = "Monto total aprobado: ₡"+montoAprobado;
            lblMontoTotal.Text = "Monto total: ₡"+montoTotal;

            //lista solicitudes
            var dt2 = listaSession;
            pgsource.DataSource = dt2;
            pgsource.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual2;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas2"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina2.Text = "Página " + (paginaActual2 + 1) + " de " + pgsource.PageCount + " (" + dt2.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior2.Enabled = !pgsource.IsFirstPage;
            lbSiguiente2.Enabled = !pgsource.IsLastPage;
            lbPrimero2.Enabled = !pgsource.IsFirstPage;
            lbUltimo2.Enabled = !pgsource.IsLastPage;

            rpIngresos.DataSource = listaSession;
            rpIngresos.DataBind();

            //metodo que realiza la paginacion
            Paginacion2();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: realiza la paginacion
        /// Requiere: -
        /// Modifica: paginacion mostrada en pantalla
        /// Devuelve: -
        /// </summary>
        private void Paginacion()
        {
            var dt = new DataTable();
            dt.Columns.Add("IndexPagina"); //Inicia en 0
            dt.Columns.Add("PaginaText"); //Inicia en 1

            primerIndex = paginaActual - 2;
            if (paginaActual > 2)
                ultimoIndex = paginaActual + 2;
            else
                ultimoIndex = 4;

            //se revisa que la ultima pagina sea menor que el total de paginas a mostrar, sino se resta para que muestre bien la paginacion
            if (ultimoIndex > Convert.ToInt32(ViewState["TotalPaginas"]))
            {
                ultimoIndex = Convert.ToInt32(ViewState["TotalPaginas"]);
                primerIndex = ultimoIndex - 4;
            }

            if (primerIndex < 0)
                primerIndex = 0;

            //se crea el numero de paginas basado en la primera y ultima pagina
            for (var i = primerIndex; i < ultimoIndex; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaginacion.DataSource = dt;
            rptPaginacion.DataBind();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: realiza la paginacion
        /// Requiere: -
        /// Modifica: paginacion mostrada en pantalla
        /// Devuelve: -
        /// </summary>
        private void Paginacion2()
        {
            var dt = new DataTable();
            dt.Columns.Add("IndexPagina"); //Inicia en 0
            dt.Columns.Add("PaginaText"); //Inicia en 1

            primerIndex2 = paginaActual2 - 2;
            if (paginaActual2 > 2)
                ultimoIndex2 = paginaActual2 + 2;
            else
                ultimoIndex2 = 4;

            //se revisa que la ultima pagina sea menor que el total de paginas a mostrar, sino se resta para que muestre bien la paginacion
            if (ultimoIndex2 > Convert.ToInt32(ViewState["TotalPaginas2"]))
            {
                ultimoIndex2 = Convert.ToInt32(ViewState["TotalPaginas2"]);
                primerIndex2 = ultimoIndex2 - 4;
            }

            if (primerIndex2 < 0)
                primerIndex2 = 0;

            //se crea el numero de paginas basado en la primera y ultima pagina
            for (var i = primerIndex2; i < ultimoIndex2; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaginacion2.DataSource = dt;
            rptPaginacion2.DataBind();
        }
        #endregion

        #region eventos
        #region eventos paginacion
        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: se devuelve a la primera pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Primer pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrimero_Click(object sender, EventArgs e)
        {
            paginaActual = 0;
            mostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo_Click(object sender, EventArgs e)
        {
            paginaActual = (Convert.ToInt32(ViewState["TotalPaginas"]) - 1);
            mostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: se devuelve a la pagina anterior y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Anterior pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAnterior_Click(object sender, EventArgs e)
        {
            paginaActual -= 1;
            mostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: se devuelve a la pagina siguiente y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Siguiente pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSiguiente_Click(object sender, EventArgs e)
        {
            paginaActual += 1;
            mostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: actualiza la la pagina actual y muestra los datos de la misma
        /// Requiere: -
        /// Modifica: elementos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptPaginacion_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual = Convert.ToInt32(e.CommandArgument.ToString());
            mostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: marca el boton de la pagina seleccionada
        /// Requiere: dar clic al boton de paginacion
        /// Modifica: color del boton seleccionado
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptPaginacion_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion");
            if (lnkPagina.CommandArgument != paginaActual.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: se devuelve a la primera pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Primer pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrimero2_Click(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            cargarDatosTblIngresos();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo2_Click(object sender, EventArgs e)
        {
            paginaActual2 = (Convert.ToInt32(ViewState["TotalPaginas2"]) - 1);
            cargarDatosTblIngresos();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: se devuelve a la pagina anterior y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Anterior pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAnterior2_Click(object sender, EventArgs e)
        {
            paginaActual2 -= 1;
            cargarDatosTblIngresos();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: se devuelve a la pagina siguiente y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Siguiente pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSiguiente2_Click(object sender, EventArgs e)
        {
            paginaActual2 += 1;
            cargarDatosTblIngresos();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: actualiza la la pagina actual y muestra los datos de la misma
        /// Requiere: -
        /// Modifica: elementos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptPaginacion2_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual2 = Convert.ToInt32(e.CommandArgument.ToString());
            cargarDatosTblIngresos();
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: marca el boton de la pagina seleccionada
        /// Requiere: dar clic al boton de paginacion
        /// Modifica: color del boton seleccionado
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptPaginacion2_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion2");
            if (lnkPagina.CommandArgument != paginaActual2.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }
        
        #endregion

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
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
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            List<Proyectos> listaProyectos = proyectoServicios.getProyectosPorPeriodo(periodo);

            Session["listaProyectos"] = listaProyectos;
            Session["listaProyectosFiltrada"] = listaProyectos;

            mostrarDatosTabla();

            PanelIngresos.Visible = false;
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: filtra la tabla segun los datos ingresados en los filtros
        /// Requiere: dar clic en el boton de flitrar e ingresar datos en los filtros
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            paginaActual = 0;
            mostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
        /// Efecto: muestra los datos de presupuesto de ingreso del proyecto seleccionado
        /// Requiere: darle clic al boton de "Seleccionar" 
        /// Modifica: tabla de ingresos segun el proyecto seleccionado
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelccionar_Click(object sender, EventArgs e)
        {
            int idProyecto = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Proyectos> listaProyectos = (List<Proyectos>)Session["listaProyectosFiltrada"];

            foreach (Proyectos proyecto in listaProyectos)
            {
                if (proyecto.idProyecto == idProyecto)
                {
                    proyectoSeleccionado= proyecto;
                    break;
                }
            }

            List<Entidades.PresupuestoIngreso> listaPresupuestosIngresos = presupuestoIngresoServicios.getPresupuestosIngresosPorProyecto(proyectoSeleccionado);
            Session["listaPresupuestosIngresos"] = listaPresupuestosIngresos;

            cargarDatosTblIngresos();

            lblIngresos.Text = "Proyecto seleccionado: "+proyectoSeleccionado.nombreProyecto;

            PanelIngresos.Visible = true;
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 30/sep/2019
        /// Efecto: levanta el modal para ingresar nuevo ingresos al proyecto seleccionado
        /// Requiere: dar clic en el boton de "Nuevo ingreso"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoIngreso_Click(object sender, EventArgs e)
        {

            lblProyectoNuevoModal.Text = proyectoSeleccionado.nombreProyecto;

            List<Entidades.PresupuestoIngreso> listaPresupuestosIngresos = (List<Entidades.PresupuestoIngreso>)Session["listaPresupuestosIngresos"];

            if (listaPresupuestosIngresos.Count > 0)
            {
                lblTipoIngresoModalNuevo.Text = "Adicional";
            }
            else
            {
                lblTipoIngresoModalNuevo.Text = "Inicial";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoIngreso();", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 30/sep/2019
        /// Efecto: Guarda en la base de datos el ingreso en el proyecto seleccionado
        /// Requiere: llenar el dato de monto y darle clic al boton de "Guardar"
        /// Modifica: tabla de ingresos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoIngresoModal_Click(object sender, EventArgs e)
        {
            Entidades.PresupuestoIngreso presupuestoIngreso = new Entidades.PresupuestoIngreso();
            presupuestoIngreso.proyecto = proyectoSeleccionado;

            List<Entidades.PresupuestoIngreso> listaPresupuestosIngresos = (List<Entidades.PresupuestoIngreso>)Session["listaPresupuestosIngresos"];

            if (listaPresupuestosIngresos.Count > 0)
            {
                presupuestoIngreso.esInicial = false;
            }
            else
            {
                presupuestoIngreso.esInicial = true;
            }

            Double monto = 0;
            String txtMonto = txtMontoModalNuevo.Text.Replace(".", ",");
            if (Double.TryParse(txtMonto, out monto))
            {
                txtMontoModalNuevo.Text = monto.ToString();
            }

            presupuestoIngreso.monto = monto;

            EstadoPresupIngreso estadoPresupIngreso = estadoPresupIngresoServicios.getEstadoPresupIngresoPorNombre("Guardar");
            presupuestoIngreso.estadoPresupIngreso = estadoPresupIngreso;

            presupuestoIngresoServicios.InsertarPresupuestoIngreso(presupuestoIngreso);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoIngreso", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoIngreso').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se ingreso correctamente el presupuesto" + "');", true);

            listaPresupuestosIngresos = presupuestoIngresoServicios.getPresupuestosIngresosPorProyecto(proyectoSeleccionado);
            Session["listaPresupuestosIngresos"] = listaPresupuestosIngresos;

            cargarDatosTblIngresos();
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 01/oct/2019
        /// Efecto: levanta el modal para editar el presupuesto de ingreso
        /// Requiere: dar clic en el boton de "Editar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int idPresupuestoIngreso = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Entidades.PresupuestoIngreso> listaPresupuestosIngresos = (List<Entidades.PresupuestoIngreso>)Session["listaPresupuestosIngresos"];

            foreach (Entidades.PresupuestoIngreso presupuestoIngreso in listaPresupuestosIngresos)
            {
                if (presupuestoIngreso.idPresupuestoIngreso == idPresupuestoIngreso)
                {
                    presupuestoIngresoSeleccionado = presupuestoIngreso;
                    break;
                }
            }

            lblProyectoEditarModal.Text = proyectoSeleccionado.nombreProyecto;
            lblTipoIngresoModalEditar.Text = presupuestoIngresoSeleccionado.esInicial ? "Inicial" : "Adicional";

            txtMontoModalEditar.Text = presupuestoIngresoSeleccionado.monto.ToString();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarIngreso();", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 01/oct/2019
        /// Efecto: actualiza el presupuesto de ingreso seleccionado
        /// Requiere: dar clic al boton de "Actualizar"
        /// Modifica: el monto del presupuesto de ingreso seleccionado
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizarIngresoModalEditar_Click(object sender, EventArgs e)
        {
            Entidades.PresupuestoIngreso presupuestoIngreso = presupuestoIngresoSeleccionado;

            Double monto = 0;
            String txtMonto = txtMontoModalEditar.Text.Replace(".", ",");
            if (Double.TryParse(txtMonto, out monto))
            {
                txtMontoModalEditar.Text = monto.ToString();
            }

            presupuestoIngreso.monto = monto;

            presupuestoIngresoServicios.actualizarPresupuestoIngreso(presupuestoIngreso);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarIngreso", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarIngreso').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se actualizo correctamente el presupuesto" + "');", true);

            List<Entidades.PresupuestoIngreso> listaPresupuestosIngresos = presupuestoIngresoServicios.getPresupuestosIngresosPorProyecto(proyectoSeleccionado);
            Session["listaPresupuestosIngresos"] = listaPresupuestosIngresos;

            cargarDatosTblIngresos();
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 01/oct/2019
        /// Efecto: levanta el modal de eliminar
        /// Requiere: dar clic en el boton de "Eliminar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int idPresupuestoIngreso = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Entidades.PresupuestoIngreso> listaPresupuestosIngresos = (List<Entidades.PresupuestoIngreso>)Session["listaPresupuestosIngresos"];

            foreach (Entidades.PresupuestoIngreso presupuestoIngreso in listaPresupuestosIngresos)
            {
                if (presupuestoIngreso.idPresupuestoIngreso == idPresupuestoIngreso)
                {
                    presupuestoIngresoSeleccionado = presupuestoIngreso;
                    break;
                }
            }

            lblProyectoEliminarModal.Text = proyectoSeleccionado.nombreProyecto;
            lblTipoIngresoModalEliminar.Text = presupuestoIngresoSeleccionado.esInicial ? "Inicial" : "Adicional";

            txtMontoModalEliminar.Text = presupuestoIngresoSeleccionado.monto.ToString();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarIngreso();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/oct/2019
        /// Efecto: eliminar el presupuesto de ingreso seleccionado
        /// Requiere: dar clic en el boton de "Eliminar"
        /// Modifica: tabla de presupuestos de ingreso
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarIngresoModalEliminar_Click(object sender, EventArgs e)
        {
            presupuestoIngresoServicios.EliminarPresupuestoIngreso(presupuestoIngresoSeleccionado.idPresupuestoIngreso);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarIngreso", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarIngreso').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se elimino correctamente el presupuesto" + "');", true);

            List<Entidades.PresupuestoIngreso> listaPresupuestosIngresos = presupuestoIngresoServicios.getPresupuestosIngresosPorProyecto(proyectoSeleccionado);
            Session["listaPresupuestosIngresos"] = listaPresupuestosIngresos;

            cargarDatosTblIngresos();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 02/oct/2019
        /// Efecto: levanta el modal para aprobar el presupuesto de ingreso
        /// Requiere: dar clic al boton  de "Aprobar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            int idPresupuestoIngreso = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Entidades.PresupuestoIngreso> listaPresupuestosIngresos = (List<Entidades.PresupuestoIngreso>)Session["listaPresupuestosIngresos"];

            foreach (Entidades.PresupuestoIngreso presupuestoIngreso in listaPresupuestosIngresos)
            {
                if (presupuestoIngreso.idPresupuestoIngreso == idPresupuestoIngreso)
                {
                    presupuestoIngresoSeleccionado = presupuestoIngreso;
                    break;
                }
            }

            lblProyectoAprobarModal.Text = proyectoSeleccionado.nombreProyecto;
            lblTipoIngresoModalAprobar.Text = presupuestoIngresoSeleccionado.esInicial ? "Inicial" : "Adicional";

            txtMontoModalAprobar.Text = presupuestoIngresoSeleccionado.monto.ToString();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalAprobarIngreso();", true);
        }

        protected void btnAprobarIngresoModalAprobar_Click(object sender, EventArgs e)
        {
            EstadoPresupIngreso estadoPresupIngreso = estadoPresupIngresoServicios.getEstadoPresupIngresoPorNombre("Registrar");
            presupuestoIngresoSeleccionado.estadoPresupIngreso = estadoPresupIngreso;

            presupuestoIngresoServicios.actualizarEstadoPresupuestoIngreso(presupuestoIngresoSeleccionado);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalAprobarIngreso", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalAprobarIngreso').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se aprobo correctamente el presupuesto" + "');", true);

            List<Entidades.PresupuestoIngreso> listaPresupuestosIngresos = presupuestoIngresoServicios.getPresupuestosIngresosPorProyecto(proyectoSeleccionado);
            Session["listaPresupuestosIngresos"] = listaPresupuestosIngresos;

            cargarDatosTblIngresos();
        }
        
        #endregion
    }
}