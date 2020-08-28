using PEP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Servicios;
using System.Data;

namespace Proyecto.Planilla
{
    public partial class AdministrarPlanillaFundevi : System.Web.UI.Page
    {
        PlanillaFundeviServicios fundeviServicios = new PlanillaFundeviServicios();
        PeriodoServicios periodoServicios = new PeriodoServicios();
        readonly PagedDataSource pgsource = new PagedDataSource();
        int primerIndex, ultimoIndex;
        private int elmentosMostrar = 10;
        Boolean mostrar;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);
            if (!IsPostBack)
            {
                Session["planillas"] = null;
                Session["planillasFiltradas"] = null;
                llenarDdlPeriodos();
                List<PlanillaFundevi> planillaFundevi = fundeviServicios.GetPlanillasFundevi();
                Session["planillas"] = planillaFundevi;
                Session["planillasFiltradas"] = planillaFundevi;
                mostrarDatosTabla();


            }
        }

        /// <summary>
        /// Juan Solano Brenes
        /// 20/06/2019
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
        /// Juan Solano Brenes
        /// 20/06/2019
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTabla()
        {
            List<PlanillaFundevi> listaPlanillas = (List<PlanillaFundevi>)Session["planillas"];
            String anno = "";
            if (!String.IsNullOrEmpty(txtBuscarPeriodo.Text))
            {
                anno = txtBuscarPeriodo.Text;
            }
            List<PlanillaFundevi> listaPlanillasFiltrada = (List<PlanillaFundevi>)listaPlanillas.Where(planilla => planilla.anoPeriodo.ToString().Contains(anno)).ToList();

            Session["planillasFiltrada"] = listaPlanillasFiltrada;

            var dt = listaPlanillasFiltrada;
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

            rpPlanillas.DataSource = pgsource;
            rpPlanillas.DataBind();

            //metodo que realiza la paginacion
            Paginacion();
        }
        /// <summary>
        /// Juan Solano Brenes
        /// 20/06/2019
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
        #region eventos

        /// <summary>
        /// Juan Solano Brenes
        /// 20/06/2019
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
            mostrarDatosTabla();
        }

        /// <summary>
        /// Juan Solano Brenes
        /// 20/06/2019
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
            mostrarDatosTabla();
        }

        /// <summary>
        /// Juan Solano Brenes
        /// 20/06/2019
        /// Efecto: se devuelve a la pagina anterior y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Anterior pagina"
        /// Modifica: elementos mostrados en la tabla de contactos
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
        /// Juan Solano Brenes
        /// 20/06/2019
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
            mostrarDatosTabla();
        }

        /// <summary>
        /// Juan Solano Brenes
        /// 20/06/2019
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

        protected void btnNuevaPlanillaModal_Click(object sender, EventArgs e)
        {
            int ano = Convert.ToInt32(ddlPeriodo.SelectedValue);

            if (fundeviServicios.existePlanilla(ano))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.warning('" + "Ya existe una planilla en el período "+ano+ "');", true);
            }
            else
            {
                fundeviServicios.Insertar(ano);
                //String url = Page.ResolveUrl("~/Planilla/AdministrarPlanillaFundevi.aspx");
                //Response.Redirect(url);}
                List<PlanillaFundevi> planillaFundevi = fundeviServicios.GetPlanillasFundevi();
                Session["planillas"] = planillaFundevi;
                Session["planillasFiltradas"] = planillaFundevi;
                mostrarDatosTabla();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se guardo correctamente la planilla"+"');", true);
            }
            
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaPlanilla", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaPlanilla').hide();", true);
        }

        protected void btnNuevaPlanilla_Click(object sender, EventArgs e)
        {
            if (mostrar = false)
            {
                ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalNuevaPlanilla();", true);
            }
            else
            {
                mostrar = false;
            }
           
        }

        protected void btnSelccionar_Click(object sender, EventArgs e)
        {
            int idPlanilla = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<PlanillaFundevi> listaPlanillas = (List<PlanillaFundevi>)Session["planillas"];

            PlanillaFundevi planillaSeleccionada = new PlanillaFundevi();

            foreach (PlanillaFundevi planilla in listaPlanillas)
            {
                if (planilla.idPlanilla == idPlanilla)
                {
                    planillaSeleccionada = planilla;
                    break;
                }
            }

            Session["planillaSeleccionada"] = planillaSeleccionada;

            String url = Page.ResolveUrl("~/Planilla/AdministrarFuncionarioFundevi.aspx");
            Response.Redirect(url);
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            paginaActual = 0;
            mostrarDatosTabla();
            mostrar = true;
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int ano = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            PlanillaFundevi planilla = fundeviServicios.GetPlanilla(ano);
            Session["planillaEliminar"] = planilla;
            txtAno.Text = "" + planilla.anoPeriodo;
            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalEliminarPlanilla();", true);
        }

        protected void btnEliminar_Click1(object sender, EventArgs e)
        {
            PlanillaFundevi planilla = (PlanillaFundevi)Session["planillaEliminar"];


            if (fundeviServicios.EliminarPlanilla(planilla.idPlanilla))
            {
                String url = Page.ResolveUrl("~/Planilla/AdministrarPlanillaFundevi.aspx");
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Juan Solano Brenes
        /// 20/06/2019
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
        #endregion
    }
}