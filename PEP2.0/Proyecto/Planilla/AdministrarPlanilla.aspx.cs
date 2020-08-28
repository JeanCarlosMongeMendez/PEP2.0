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

namespace Proyecto.Planilla
{
    public partial class AdministrarPlanilla : System.Web.UI.Page
    {
        #region variables globales
        PeriodoServicios periodoServicios = new PeriodoServicios();
        PlanillaServicios planillaServicios = new PlanillaServicios();
        static Entidades.Planilla planillaSeleccionada = new Entidades.Planilla();
        Boolean mostrar;
        readonly PagedDataSource pgsource = new PagedDataSource();
        int primerIndex, ultimoIndex;
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
                Session["listaPlanillas"] = null;
                Session["listaPlanillasFiltrada"] = null;

                llenarDdlPeriodos();

                List<Entidades.Planilla> listaPlanillas = planillaServicios.getPlanillas();

                Session["listaPlanillas"] = listaPlanillas;
                Session["listaPlanillasFiltrada"] = listaPlanillas;

                mostrarDatosTabla();
            }
        }
        #endregion

        #region logica

        /// <summary>
        /// Leonardo Carrion
        /// 09/jul/2019
        /// Efecto: valida los campos ingresados en nueva planilla y marca los que estan mal
        /// Requiere: -
        /// Modifica: los txt que esten mal
        /// Devuelve: true si esta bien o false de lo contrario
        /// </summary>
        /// <returns></returns>
        public Boolean validarNuevaPlanilla()
        {
            Boolean valido = true;

            txtAnualidad1.CssClass = "form-control";
            txtAnualidad2.CssClass = "form-control";

            #region anualidad 1
            if (String.IsNullOrEmpty(txtAnualidad1.Text))
            {
                txtAnualidad1.CssClass = "form-control alert-danger";
                valido = false;
            }
            else
            {
                try
                {
                    String anualidadTxt = txtAnualidad1.Text.Replace(",", ".");
                    Double anualidad = Convert.ToDouble(anualidadTxt);
                }
                catch
                {
                    txtAnualidad1.CssClass = "form-control alert-danger";
                    valido = false;
                }
            }
            #endregion

            #region anualidad 2
            if (String.IsNullOrEmpty(txtAnualidad2.Text))
            {
                txtAnualidad2.CssClass = "form-control alert-danger";
                valido = false;
            }
            else
            {
                try
                {
                    String anualidadTxt = txtAnualidad2.Text.Replace(",", ".");
                    Double anualidad = Convert.ToDouble(anualidadTxt);
                }
                catch
                {
                    txtAnualidad2.CssClass = "form-control alert-danger";
                    valido = false;
                }
            }
            #endregion

            return valido;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 09/jul/2019
        /// Efecto: valida los campos ingresados en editar planilla y marca los que estan mal
        /// Requiere: -
        /// Modifica: los txt que esten mal
        /// Devuelve: true si esta bien o false de lo contrario
        /// </summary>
        /// <returns></returns>
        public Boolean validarEditarPlanilla()
        {
            Boolean valido = true;

            txtAnualidad1EditarModal.CssClass = "form-control";
            txtAnualidad2EditarModal.CssClass = "form-control";

            #region anualidad 1
            if (String.IsNullOrEmpty(txtAnualidad1EditarModal.Text))
            {
                txtAnualidad1EditarModal.CssClass = "form-control alert-danger";
                valido = false;
            }
            else
            {
                try
                {
                    String anualidadTxt = txtAnualidad1EditarModal.Text.Replace(",", ".");
                    Double anualidad = Convert.ToDouble(anualidadTxt);
                }
                catch
                {
                    txtAnualidad1EditarModal.CssClass = "form-control alert-danger";
                    valido = false;
                }
            }
            #endregion

            #region anualidad 2
            if (String.IsNullOrEmpty(txtAnualidad2EditarModal.Text))
            {
                txtAnualidad2EditarModal.CssClass = "form-control alert-danger";
                valido = false;
            }
            else
            {
                try
                {
                    String anualidadTxt = txtAnualidad2EditarModal.Text.Replace(",", ".");
                    Double anualidad = Convert.ToDouble(anualidadTxt);
                }
                catch
                {
                    txtAnualidad2EditarModal.CssClass = "form-control alert-danger";
                    valido = false;
                }
            }
            #endregion

            return valido;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 14/jun/2019
        /// Efecto: llean el DropDownList con los periodos que se encuentran en la base de datos
        /// Requiere: - 
        /// Modifica: DropDownList
        /// Devuelve: -
        /// </summary>
        public void llenarDdlPeriodos()
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            ddlPeriodo.Items.Clear();
            ddlPeriodoEditarModal.Items.Clear();
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
                    ddlPeriodoEditarModal.Items.Add(itemPeriodo);
                }

                if (anoHabilitado != 0)
                {
                    ddlPeriodo.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                }
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 14/jun/2019
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTabla()
        {
            List<Entidades.Planilla> listaPlanillas = (List<Entidades.Planilla>)Session["listaPlanillas"];

            String anno = "";

            if (!String.IsNullOrEmpty(txtBuscarPeriodo.Text))
            {
                anno = txtBuscarPeriodo.Text;
            }

            List<Entidades.Planilla> listaPlanillasFiltrada = (List<Entidades.Planilla>)listaPlanillas.Where(planilla => planilla.periodo.anoPeriodo.ToString().Contains(anno)).ToList();

            Session["listaPlanillasFiltrada"] = listaPlanillasFiltrada;

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
        /// Leonardo Carrion
        /// 14/jun/2019
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

        #endregion

        #region eventos

        /// <summary>
        /// Leonardo Carrion
        /// 14/jun/2019
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
        /// Leonardo Carrion
        /// 14/jun/2019
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
        /// Leonardo Carrion
        /// 14/jun/2019
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
        /// Leonardo Carrion
        /// 14/jun/2019
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
        /// Leonardo Carrion
        /// 14/jun/2019
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
        /// 14/jun/2019
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
        /// 14/jun/2019
        /// Efecto: guarda en la base de datos la nueva planilla, y recarga la tabla de planillas
        /// Requiere: dar clic en el boton de "Guardar"
        /// Modifica: tabla de base de datos y pantalla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaPlanillaModal_Click(object sender, EventArgs e)
        {
            if (validarNuevaPlanilla())
            {
                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

                Entidades.Planilla planilla = new Entidades.Planilla();

                planilla.anualidad1 = Convert.ToDouble(txtAnualidad1.Text.Replace(".", ","));
                planilla.anualidad2 = Convert.ToDouble(txtAnualidad2.Text.Replace(".", ","));
                planilla.periodo = periodo;

                planillaServicios.insertarPlanilla(planilla);

                txtAnualidad1.Text = "";
                txtAnualidad2.Text = "";

                List<Entidades.Planilla> listaPlanillas = planillaServicios.getPlanillas();

                Session["listaPlanillas"] = listaPlanillas;
                Session["listaPlanillasFiltrada"] = listaPlanillas;

                mostrarDatosTabla();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaPlanilla", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaPlanilla').hide();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaPlanilla", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaPlanilla').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaPlanilla();", true);
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 13/jun/2019
        /// Efecto: levanta el modal para ingresar una nueva escala salarial y pone el año escogido en el modal
        /// Requiere: dar clic al boton de "Nuevo"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaPlanilla_Click(object sender, EventArgs e)
        {
            txtAnualidad1.CssClass = "form-control";
            txtAnualidad2.CssClass = "form-control";
            if (mostrar == false)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaPlanilla", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaPlanilla').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaPlanilla();", true);
            }
            else
            {
                mostrar = false;
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/jun/2019
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
            mostrar = true;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 08/jul/2019
        /// Efecto:  levanta modal con la informacion de la planilla seleccionada
        /// Requiere: dar clic en boton de "Editar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            txtAnualidad1EditarModal.CssClass = "form-control";
            txtAnualidad2EditarModal.CssClass = "form-control";

            int idPlanilla = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            List<Entidades.Planilla> listaEntidades = (List<Entidades.Planilla>)Session["listaPlanillasFiltrada"];

            foreach (Entidades.Planilla planilla in listaEntidades)
            {
                if (planilla.idPlanilla == idPlanilla)
                {
                    planillaSeleccionada = planilla;
                    txtAnualidad1EditarModal.Text = planilla.anualidad1.ToString();
                    txtAnualidad2EditarModal.Text = planilla.anualidad2.ToString();
                    break;
                }
            }

            //variable para contar index del ddlPaises
            int contIndex = 0;

            foreach (ListItem item in ddlPeriodoEditarModal.Items)
            {
                if (Convert.ToInt32(item.Value) == planillaSeleccionada.periodo.anoPeriodo)
                {
                    ddlPeriodoEditarModal.SelectedIndex = contIndex;
                    break;
                }
                contIndex++;
            }

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarPlanilla", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarPlanilla').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarPlanilla();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 08/jul/2019
        /// Efecto:  edita la plantilla seleccionada con los datos ingresados
        /// Requiere: cambiar datos de la plantilla seleccionada
        /// Modifica: la plantilla 
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            if (validarEditarPlanilla())
            {
                Entidades.Planilla planilla = planillaSeleccionada;
                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodoEditarModal.SelectedValue);

                planilla.anualidad1 = Convert.ToDouble(txtAnualidad1EditarModal.Text.Replace(".", ","));
                planilla.anualidad2 = Convert.ToDouble(txtAnualidad2EditarModal.Text.Replace(".", ","));
                planilla.periodo = periodo;

                planillaServicios.actualizarPlanilla(planilla);

                txtAnualidad1EditarModal.Text = "";
                txtAnualidad2EditarModal.Text = "";

                List<Entidades.Planilla> listaPlanillas = planillaServicios.getPlanillas();

                Session["listaPlanillas"] = listaPlanillas;
                Session["listaPlanillasFiltrada"] = listaPlanillas;

                mostrarDatosTabla();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarPlanilla", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarPlanilla').hide();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarPlanilla", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarPlanilla').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarPlanilla();", true);
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/jul/2019
        /// Efecto:  levanta modal con la informacion de la planilla seleccionada
        /// Requiere: dar clic en boton de "Eliminar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            txtAnualidad1EliminarModal.CssClass = "form-control";
            txtAnualidad2EliminarModal.CssClass = "form-control";

            int idPlanilla = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            List<Entidades.Planilla> listaEntidades = (List<Entidades.Planilla>)Session["listaPlanillasFiltrada"];

            foreach (Entidades.Planilla planilla in listaEntidades)
            {
                if (planilla.idPlanilla == idPlanilla)
                {
                    planillaSeleccionada = planilla;
                    txtPeriodoEliminarModal.Text = planilla.periodo.anoPeriodo.ToString();
                    txtAnualidad1EliminarModal.Text = planilla.anualidad1.ToString();
                    txtAnualidad2EliminarModal.Text = planilla.anualidad2.ToString();
                    break;
                }
            }

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarPlanilla", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarPlanilla').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarPlanilla();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/jul/2019
        /// Efecto: eliminar la planilla seleccionada
        /// Requiere: dar clic en el boton de "Eliminar" del modal
        /// Modifica: la tabla de planillas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarModal_Click(object sender, EventArgs e)
        {
            Entidades.Planilla planilla = planillaSeleccionada;

            planillaServicios.eliminarPlanilla(planilla);

            List<Entidades.Planilla> listaPlanillas = planillaServicios.getPlanillas();

            Session["listaPlanillas"] = listaPlanillas;
            Session["listaPlanillasFiltrada"] = listaPlanillas;

            mostrarDatosTabla();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarPlanilla", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarPlanilla').hide();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/jul/2019
        /// Efecto: redirige a la pantalla para agregar funcionarios a la planilla seleccionada
        /// Requiere: dar clic al boton de "Seleccionar"
        /// Modifica: - 
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelccionar_Click(object sender, EventArgs e)
        {
            int idPlanilla = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            List<Entidades.Planilla> listaEntidades = (List<Entidades.Planilla>)Session["listaPlanillasFiltrada"];

            foreach (Entidades.Planilla planilla in listaEntidades)
            {
                if (planilla.idPlanilla == idPlanilla)
                {
                    planillaSeleccionada = planilla;
                    break;
                }
            }
            Session["planillaSeleccionada"] = planillaSeleccionada;

            String url = Page.ResolveUrl("~/Planilla/AdministrarFuncionariosPlanilla.aspx");
            Response.Redirect(url);
        }
        #endregion

    }
}