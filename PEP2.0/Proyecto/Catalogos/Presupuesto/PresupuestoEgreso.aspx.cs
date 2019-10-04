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
        private PartidaPresupuestoEgresoServicio partidaPresupuestoEgresoServicio;
        private PresupuestoServicios presupuestoServicios;
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
            PEP.Utilidades.escogerMenu(Page, rolesPermitidos);

            this.periodoServicios = new PeriodoServicios();
            this.proyectoServicios = new ProyectoServicios();
            this.unidadServicios = new UnidadServicios();
            this.partidaServicios = new PartidaServicios();
            this.presupuestoServicios = new PresupuestoServicios();
            partidaPresupuestoEgresoServicio = new PartidaPresupuestoEgresoServicio();
            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                Session["periodoSeleccionado"] = null;
                Session["listaPartidasPresupuestoEgresos"] = null;
                Session["partidasPorPeriodo"] = null;
                Session["ListaPresupuestoEgresoGuardar"] = null;
                PeriodosDDL.Items.Clear();
                ProyectosDDL.Items.Clear();
                CargarPeriodos();
                

            }
            else
            {
                Session["ListaPresupuestoEgresoGuardar"] = null;
                MostrarPartidas();
                
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
                        Session["proyecto"] = proyecto.idProyecto;
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

                MostrarPartidas();
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

                        //   tbMonto.Text = String.Format("{0:N}", "0");
                        // lbTotal.Text = String.Format("{0:N}", "0");
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
                        totalIngreso = totalIngreso - presupuestoEgreso.montoTotal;
                        aprobadoGuardar = true;
                        aprobadoAprobar = false;
                    }
                    else
                    {
                        aprobadoGuardar = false;
                        aprobadoAprobar = true;
                        Session["idPresupuestoEgreso"] = presupuestoEgreso.idPresupuestoEgreso;
                    }
                }

                btnGuardar.Enabled = aprobadoGuardar;
                btnAprobar.Enabled = aprobadoAprobar;
                LblPresupuestoIngreso.Text = "El monto disponible para el proyecto seleccionado es " + String.Format("{0:N}", totalIngreso) + " colones";
            }
        }

        /// <summary>
        /// Realiza la paginación de la tabla principal
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
        /// Realiza la tabla del modal es decir de la tabla con los detalles de los egresos
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

        private void MostrarPartidas()
        {
            if (ProyectosDDL.Items.Count > 0)
            {
                LinkedList<PartidaPresupuestoEgreso> partidas = new LinkedList<PartidaPresupuestoEgreso>();
                partidas = partidaPresupuestoEgresoServicio.ObtenerPartidaPresupuestoEgresoDatosPorPeriodo(Convert.ToInt32(PeriodosDDL.SelectedValue));
                Session["listaPartidasPresupuestoEgresos"] = partidas;
                var dt = partidas;
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

                rpPartida.DataSource = pgsource;
                rpPartida.DataBind();

                //metodo que realiza la paginacion
                Paginacion();

            }
            else
            {
                LinkedList<PartidaPresupuestoEgreso> partidas = new LinkedList<PartidaPresupuestoEgreso>();
                rpPartida.DataSource = partidas;
                rpPartida.DataBind();
            }
        }

        /// <summary>
        /// Muestra los presupuestos relacionados con con un proyecto y una unidad
        /// 
        /// </summary>
        private void MostrarDatosTabla()
        {
            /*
            if (ProyectosDDL.Items.Count > 0)
            {
                LinkedList<Entidades.PresupuestoEgreso> listaPresupuestosEgresos = presupuestoServicios.ObtenerPresupuestoPorProyecto(Convert.ToInt32(UnidadesDDL.SelectedValue), Convert.ToInt32(ProyectosDDL.SelectedValue));
                Session["idUnidadElegida"]= Convert.ToInt32(UnidadesDDL.SelectedValue) ;
                Session["ListaPresupuestoEgresoGuardar"] = listaPresupuestosEgresos;
                Session["periodoSeleccionado"] = Convert.ToInt32(PeriodosDDL.SelectedValue);
                var dt = listaPresupuestosEgresos;
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

                rpPartida.DataSource = pgsource;
                rpPartida.DataBind();

                //metodo que realiza la paginacion
                Paginacion();

            }
            else
            {
                LinkedList<Entidades.PresupuestoEgreso> listaPresupuestosEgresos = new LinkedList<Entidades.PresupuestoEgreso>();
                rpPartida.DataSource = listaPresupuestosEgresos;
                rpPartida.DataBind();
            }*/
        }

        /// <summary>
        /// Muestra una lista de detalles relacionados a una partida y a un egresos 
        /// </summary>
        private void presupuestoEgresosPorPartida()
        {
            LinkedList<Entidades.PresupuestoEgresoPartida> listaSession = (LinkedList<Entidades.PresupuestoEgresoPartida>)Session["listaPresupuestosEgresosPartida"];
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

            rpPartidaEgresoPartida.DataSource = pgsource;
            rpPartidaEgresoPartida.DataBind();

            //metodo que realiza la paginacion
            Paginacion2();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalMostrarPresupuestoEgresos", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalMostrarPresupuestoEgresos').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalMostrarPresupuestoEgresos();", true);

        }

        private double MontoPresupuestoIngreso()
        {
            LinkedList<Entidades.PresupuestoIngreso> presupuestoIngresos = new LinkedList<Entidades.PresupuestoIngreso>();
            //presupuestoIngresos = this.presupuestoServicios.ObtenerPorProyecto(Int32.Parse(ProyectosDDL.SelectedValue));
            double montoTotalPresupuestoIngreso = 0;

            foreach (Entidades.PresupuestoIngreso presupuestoIngreso in presupuestoIngresos)
            {
                 montoTotalPresupuestoIngreso += presupuestoIngreso.monto;
               /* if (presupuestoIngreso.estado)
                {
                    montoTotalPresupuestoIngreso += presupuestoIngreso.monto;
                }*/
            }

            return montoTotalPresupuestoIngreso;
        }

        #endregion

        #region eventos

        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
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
            presupuestoEgresosPorPartida();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
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
            presupuestoEgresosPorPartida();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
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
            presupuestoEgresosPorPartida();
        }
       
        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
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
            presupuestoEgresosPorPartida();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
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
            presupuestoEgresosPorPartida();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
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
       
        protected void Unidades_OnChanged(object sender, EventArgs e)
        {
            LlenarTabla();
        }

        protected void Periodos_OnChanged(object sender, EventArgs e)
        {

            CargarProyectos();
            CargarUnidades();
            MostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/abr/2019
        /// Efecto: se devuelve a la primera pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Primer pagina"
        /// Modifica: elementos mostrados en la tabla de contactos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrimero_Click(object sender, EventArgs e)
        {
            paginaActual = 0;
            MostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/abr/2019
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla de contactos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo_Click(object sender, EventArgs e)
        {
            paginaActual = (Convert.ToInt32(ViewState["TotalPaginas"]) - 1);
            MostrarDatosTabla();
        }

        protected void lbAnterior_Click(object sender, EventArgs e)
        {
            paginaActual -= 1;
            MostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/abr/2019
        /// Efecto: marca el boton de la pagina seleccionada
        /// Requiere: dar clic al boton de paginacion
        /// Modifica: color del boton seleccionado
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
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
        /// 10/abr/2019
        /// Efecto: se devuelve a la pagina siguiente y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Siguiente pagina"
        /// Modifica: elementos mostrados en la tabla de contactos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSiguiente_Click(object sender, EventArgs e)
        {
            paginaActual += 1;
            MostrarDatosTabla();
        }

        protected void rptPaginacion_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual = Convert.ToInt32(e.CommandArgument.ToString());
            MostrarDatosTabla();
        }
        /// <summary>
        ///  Josseline M
        /// este metodo insertar un nuevo registro de una partida apartir de la unidad y partidac
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoPresupuesto_Click(object sender, EventArgs e)
        {
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            txtMontoIngresarModal.CssClass = "form-control";
            txtIdPartida.CssClass = "form-control";
            txtdescripcionNuevaPartida.CssClass = "form-control";

            txtMontoIngresarModal.Text = "";
            txtIdPartida.Text = idPartida + "";
            txtdescripcionNuevaPartida.Text = "";

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalIngresarPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalIngresarPartida').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalIngresarPartida();", true);

        }
       
        /// <summary>
        /// Josseline M
        /// este metodo insertar un nuevo registro de una partida apartir de la unidad y partidac
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoIngresoPartidaModal_Click(object sender, EventArgs e)
        {
            PresupuestoEgresoPartida presupuestoEgresoPartida = new PresupuestoEgresoPartida();
            LinkedList<Entidades.PresupuestoEgreso> presupuestoEgresos = new LinkedList<Entidades.PresupuestoEgreso>();
            presupuestoEgresos = this.presupuestoServicios.ObtenerPorUnidadEgresos(Int32.Parse(UnidadesDDL.SelectedValue));
            int partidaBuscarPorPeriodo=(int)Session["periodoSeleccionado"];
            int idPartidaN = 0;
            LinkedList<Partida> partidas = new LinkedList<Partida>();
            partidas= partidaServicios.ObtenerPorPeriodo(partidaBuscarPorPeriodo);
            Session["partidasPorPeriodo"] = partidas;
            Double salario = 0;
            String txtMonto = txtMontoIngresarModal.Text.Replace(".", ",");
            if (Double.TryParse(txtMonto, out salario))
            {
                txtMontoIngresarModal.Text = salario.ToString();

                foreach (Entidades.PresupuestoEgreso presupuestoA in presupuestoEgresos)
                {
                    presupuestoEgresoPartida.idPresupuestoEgreso = presupuestoA.idPresupuestoEgreso;
                    presupuestoEgresoPartida.idPartida = Convert.ToInt32(txtIdPartida.Text);
                    presupuestoEgresoPartida.monto = salario;
                    presupuestoEgresoPartida.descripcion = txtdescripcionNuevaPartida.Text;
                    presupuestoServicios.InsertarPresupuestoEgresoPartida(presupuestoEgresoPartida);
                    MostrarDatosTabla();
                    Response.Redirect("PresupuestoEgreso.aspx");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalIngresarPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalIngresarPartida').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalIngresarPartida();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El monto asignado es incorrecto" + "');", true);
            }

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalIngresarPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalIngresarPartida').hide();", true);

        }

        /// <summary>
        /// Josseline M
        /// Muestra el detalle de las partidas de egreso en función al número de partida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVerPartidasEgreso_Click(object sender, EventArgs e)
        {
            Entidades.PresupuestoEgresoPartida presupuestoEgresoBuscar = new Entidades.PresupuestoEgresoPartida();

            LinkedList<Entidades.PresupuestoEgresoPartida> presupuestos = new LinkedList<Entidades.PresupuestoEgresoPartida>();
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            Session["idPartidaVer"] = idPartida;
           presupuestoEgresoBuscar.idPartida = idPartida;

            presupuestos = presupuestoServicios.presupuestoEgresoPartidasPorPresupuesto(presupuestoEgresoBuscar);
            Session["listaPresupuestosEgresosPartida"] = presupuestos;
            presupuestoEgresosPorPartida();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalMostrarPresupuestoEgresos", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalMostrarPresupuestoEgresos').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalMostrarPresupuestoEgresos();", true);

        }
        
        /// <summary>
        /// Josseline M
        /// Pemite editar las pantallas de editar presupuesto egreso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditarPresupuestoEgreso_Click(object sender, EventArgs e)
        {
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            LinkedList<Entidades.PresupuestoEgresoPartida> listaSession = (LinkedList<Entidades.PresupuestoEgresoPartida>)Session["listaPresupuestosEgresosPartida"];

            foreach (Entidades.PresupuestoEgresoPartida partidaEgreso in listaSession)
            {
                if (partidaEgreso.idPartida==idPartida)
                {

                    txtDescripcionEditar.Text = partidaEgreso.descripcion;
                    txtMontoNuevoEditar.Text = partidaEgreso.monto + "";
                    idPartidaEditar.Text = idPartida + "";
                    idPresupuestoEditar.Text = partidaEgreso.idPresupuestoEgreso+"";

                }
               
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarPartidaEgreso", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarPartidaEgreso').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarPartidaEgreso();", true);

        }

        /// <summary>
        /// Se encarga de eliminar el  una partida egreso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarPresupuestoEgreso_Click(object sender, EventArgs e)
        {
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            LinkedList<Entidades.PresupuestoEgresoPartida> listaSession = (LinkedList<Entidades.PresupuestoEgresoPartida>)Session["listaPresupuestosEgresosPartida"];

            foreach (Entidades.PresupuestoEgresoPartida partidaEgreso in listaSession)
            {
                if (partidaEgreso.idPartida == idPartida)
                {

                   
                    idPartidaEliminar.Text = idPartida + "";
                    idPresupuestoEliminar.Text = partidaEgreso.idPresupuestoEgreso + "";

                }

            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarPartidaEgreso", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarPartidaEgreso').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarPartidaEgreso();", true);

        }
        
        /// <summary>
        /// Almacena la actualización del presupuesto egreso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditarPartidaEgresoModal_Click(object sender, EventArgs e)
        {
            Entidades.PresupuestoEgresoPartida partidaEgreso = new Entidades.PresupuestoEgresoPartida();
            Double salario = 0;
            String txtMonto = txtMontoNuevoEditar.Text.Replace(".", ",");
            if (Double.TryParse(txtMonto, out salario))
            {
                partidaEgreso.idPresupuestoEgreso = Convert.ToInt32(idPresupuestoEditar.Text);
                partidaEgreso.idPartida = Convert.ToInt32(idPartidaEditar.Text);
                partidaEgreso.monto = salario;
                partidaEgreso.descripcion = txtDescripcionEditar.Text;
                presupuestoServicios.editarPresupuestoEgresoPartida(partidaEgreso);

                int idPartida = (int)Session["idPartidaVer"];
                PresupuestoEgresoPartida presupuestoEgresoBuscar = new PresupuestoEgresoPartida();
                presupuestoEgresoBuscar.idPartida = idPartida;

                LinkedList<PresupuestoEgresoPartida> presupuestos = new LinkedList<PresupuestoEgresoPartida>();

                presupuestos = presupuestoServicios.presupuestoEgresoPartidasPorPresupuesto(presupuestoEgresoBuscar);
                Session["listaPresupuestosEgresosPartida"] = presupuestos;
                presupuestoEgresosPorPartida();
                MostrarDatosTabla();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalMostrarPresupuestoEgresos", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalMostrarPresupuestoEgresos').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalMostrarPresupuestoEgresos();", true);


            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarPartidaEgreso", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarPartidaEgreso').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarPartidaEgreso();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El monto asignado es incorrecto" + "');", true);
            }
        }
        /// <summary>
        /// Almacena la actualización del presupuesto egreso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarPartidaEgresoModal_Click(object sender, EventArgs e)
        {
            Entidades.PresupuestoEgresoPartida partidaEgreso = new Entidades.PresupuestoEgresoPartida();
           
                partidaEgreso.idPresupuestoEgreso = Convert.ToInt32(idPresupuestoEliminar.Text);
                partidaEgreso.idPartida = Convert.ToInt32(idPartidaEliminar.Text);

              presupuestoServicios.eliminarPresupuestoEgresoPartida(partidaEgreso);

                int idPartida = (int)Session["idPartidaVer"];
                PresupuestoEgresoPartida presupuestoEgresoBuscar = new PresupuestoEgresoPartida();
                presupuestoEgresoBuscar.idPartida = idPartida;

                LinkedList<PresupuestoEgresoPartida> presupuestos = new LinkedList<PresupuestoEgresoPartida>();

                presupuestos = presupuestoServicios.presupuestoEgresoPartidasPorPresupuesto(presupuestoEgresoBuscar);
                Session["listaPresupuestosEgresosPartida"] = presupuestos;
                presupuestoEgresosPorPartida();
                MostrarDatosTabla();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalMostrarPresupuestoEgresos", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalMostrarPresupuestoEgresos').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalMostrarPresupuestoEgresos();", true);


           
        }

        protected void Proyectos_OnChanged(object sender, EventArgs e)
        {
            CargarUnidades();
        }
        
        /// <summary>
        /// Este metodo se encarga de actualizar el estado del presupuesto de egreso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Guardar_Click(object sender, EventArgs e)
        {
          
            LinkedList<Entidades.PresupuestoEgreso> presupuestosGuardar= (LinkedList<Entidades.PresupuestoEgreso> ) Session["ListaPresupuestoEgresoGuardar"];
            presupuestoServicios.guardarPartidasPresupuestoEgreso(presupuestosGuardar);
            Toastr("success", "Se han almacenado su progreso en las partidas");
            
        }

        /// <summary>
        /// Este metodo envia la lista de partidas de egresos para ser validadas esto con su debido presupuesto iniciar
        /// Hace un llamado a un metodo el cual al tener 1 indica que se ha aprobado y 0 que no posee recursos economicos suficientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Aprobar_Click(object sender, EventArgs e)
        {
            LinkedList<Entidades.PresupuestoEgreso> presupuestosGuardar = (LinkedList<Entidades.PresupuestoEgreso>)Session["ListaPresupuestoEgresoGuardar"];
            int respuesta = 0;
            foreach (Entidades.PresupuestoEgreso presupuestoIngresar in presupuestosGuardar)
            {
               respuesta = this.presupuestoServicios.AprobarPresupuestoEgreso(presupuestoIngresar);

                if (respuesta == 1)
                {
                    Toastr("success", "Sus partidas han sido aprobadas");

                }
                else
                {
                    Toastr("warning", "No es posible realizar la aprobación debido a que no hay suficientes ingresos para el proyecto");
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