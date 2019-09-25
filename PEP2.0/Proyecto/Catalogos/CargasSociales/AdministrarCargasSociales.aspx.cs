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

namespace Proyecto.Catalogos.CargasSociales
{
    public partial class AdministrarCargasSociales : System.Web.UI.Page
    {
        #region variables globales
        CargaSocialServicios cargaSocialServicios = new CargaSocialServicios();
        PeriodoServicios periodoServicios = new PeriodoServicios();
        PartidaServicios partidaServicios = new PartidaServicios();
        #endregion

        #region paginacion
        readonly PagedDataSource pgsource = new PagedDataSource();
        int primerIndex, ultimoIndex, primerIndex2, ultimoIndex2, primerIndex3, ultimoIndex3;
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

        private int paginaActual3
        {
            get
            {
                if (ViewState["paginaActual3"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["paginaActual3"]);
            }
            set
            {
                ViewState["paginaActual3"] = value;
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
                Session["listaCargasSociales"] = null;
                Session["listaCargasSocialesFiltrada"] = null;
                Session["cargaSocialSeleccionada"] = null;

                Session["listaCargasSocialesAPasar"] = null;
                Session["listaCargasSocialesAPasarFiltrada"] = null;

                Session["listaCargasSocialesAgregadas"] = null;
                Session["listaCargasSocialesAgregadasFiltrada"] = null;

                llenarDdlPeriodos();

                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

                List<CargaSocial> listaCargasSociales = cargaSocialServicios.getCargasSocialesActivasPorPeriodo(periodo);

                Session["listaCargasSociales"] = listaCargasSociales;
                Session["listaCargasSocialesFiltrada"] = listaCargasSociales;

                mostrarDatosTabla();
            }
        }
        #endregion

        #region logica

        /// <summary>
        /// Leonardo Carrion
        /// 23/jun/2019
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
        /// 20/sep/2019
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTabla()
        {
            List<CargaSocial> listaCargasSociales = (List<CargaSocial>)Session["listaCargasSociales"];

            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarDesc.Text))
            {
                desc = txtBuscarDesc.Text;
            }

            List<CargaSocial> listaCargasSocialesFiltrada = (List<CargaSocial>)listaCargasSociales.Where(jornada => jornada.descCargaSocial.ToUpper().Contains(desc.ToUpper())).ToList();

            Session["listaCargasSocialesFiltrada"] = listaCargasSocialesFiltrada;

            var dt = listaCargasSocialesFiltrada;
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

            rpCargasSociales.DataSource = pgsource;
            rpCargasSociales.DataBind();

            //metodo que realiza la paginacion
            Paginacion();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: Metodo para llenar la tabla con los datos de las cargas sociales que se encuentran en la base de datos en el modal de pasar cargas sociales en la tabla de periodo seleccionado
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        private void cargarDatosTblCargasSocialesAPasar()
        {
            List<CargaSocial> listaSession = (List<CargaSocial>)Session["listaCargasSocialesAPasar"];

            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarDescCargasSocialesAPasar.Text))
                desc = txtBuscarDescCargasSocialesAPasar.Text;

            List<CargaSocial> listaCargasSociales = (List<CargaSocial>)listaSession.Where(cargaSocial => cargaSocial.descCargaSocial.ToUpper().Contains(desc.ToUpper())).ToList();
            Session["listaCargasSocialesAPasarFiltrada"] = listaCargasSociales;

            //lista solicitudes
            var dt2 = listaCargasSociales;
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

            rpCargasSocialesAPasar.DataSource = listaCargasSociales;
            rpCargasSocialesAPasar.DataBind();

            //metodo que realiza la paginacion
            Paginacion2();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarCargaSocial').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarCargaSocial();", true);

        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: Metodo para llenar la tabla con los datos de las cargas sociales que se encuentran en la base de datos en el modal de pasar cargas sociales en la tabla de periodo seleccionado
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        private void cargarDatosTblCargasSocialesAgregadas()
        {
            List<CargaSocial> listaSession = (List<CargaSocial>)Session["listaCargasSocialesAgregadas"];

            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarDescCargasSocialesAgregadas.Text))
                desc = txtBuscarDescCargasSocialesAgregadas.Text;

            List<CargaSocial> listaCargasSociales = (List<CargaSocial>)listaSession.Where(cargaSocial => cargaSocial.descCargaSocial.ToUpper().Contains(desc.ToUpper())).ToList();
            Session["listaCargasSocialesAgregadasFiltrada"] = listaCargasSociales;

            //lista solicitudes
            var dt3 = listaCargasSociales;
            pgsource.DataSource = dt3;
            pgsource.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual3;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas3"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina3.Text = "Página " + (paginaActual3 + 1) + " de " + pgsource.PageCount + " (" + dt3.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior3.Enabled = !pgsource.IsFirstPage;
            lbSiguiente3.Enabled = !pgsource.IsLastPage;
            lbPrimero3.Enabled = !pgsource.IsFirstPage;
            lbUltimo3.Enabled = !pgsource.IsLastPage;

            rpCargasSocialesAgregadas.DataSource = listaCargasSociales;
            rpCargasSocialesAgregadas.DataBind();

            //metodo que realiza la paginacion
            Paginacion3();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarCargaSocial').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarCargaSocial();", true);
        }

        #region paginacion
        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
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
        /// 24/sep/2019
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

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: realiza la paginacion
        /// Requiere: -
        /// Modifica: paginacion mostrada en pantalla
        /// Devuelve: -
        /// </summary>
        private void Paginacion3()
        {
            var dt = new DataTable();
            dt.Columns.Add("IndexPagina"); //Inicia en 0
            dt.Columns.Add("PaginaText"); //Inicia en 1

            primerIndex3 = paginaActual3 - 2;
            if (paginaActual3 > 2)
                ultimoIndex3 = paginaActual3 + 2;
            else
                ultimoIndex3 = 4;

            //se revisa que la ultima pagina sea menor que el total de paginas a mostrar, sino se resta para que muestre bien la paginacion
            if (ultimoIndex3 > Convert.ToInt32(ViewState["TotalPaginas3"]))
            {
                ultimoIndex3 = Convert.ToInt32(ViewState["TotalPaginas3"]);
                primerIndex3 = ultimoIndex3 - 4;
            }

            if (primerIndex3 < 0)
                primerIndex3 = 0;

            //se crea el numero de paginas basado en la primera y ultima pagina
            for (var i = primerIndex3; i < ultimoIndex3; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaginacion3.DataSource = dt;
            rptPaginacion3.DataBind();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
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
        /// 20/sep/2019
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
        /// 20/sep/2019
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
        /// 20/sep/2019
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
        /// 20/sep/2019
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
        /// 20/sep/2019
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
        /// 24/sep/2019
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
            cargarDatosTblCargasSocialesAPasar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
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
            cargarDatosTblCargasSocialesAPasar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
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
            cargarDatosTblCargasSocialesAPasar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
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
            cargarDatosTblCargasSocialesAPasar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
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
            cargarDatosTblCargasSocialesAPasar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
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

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: se devuelve a la primera pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Primer pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrimero3_Click(object sender, EventArgs e)
        {
            paginaActual3 = 0;
            cargarDatosTblCargasSocialesAgregadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo3_Click(object sender, EventArgs e)
        {
            paginaActual3 = (Convert.ToInt32(ViewState["TotalPaginas3"]) - 1);
            cargarDatosTblCargasSocialesAgregadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: se devuelve a la pagina anterior y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Anterior pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAnterior3_Click(object sender, EventArgs e)
        {
            paginaActual3 -= 1;
            cargarDatosTblCargasSocialesAgregadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: se devuelve a la pagina siguiente y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Siguiente pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSiguiente3_Click(object sender, EventArgs e)
        {
            paginaActual3 += 1;
            cargarDatosTblCargasSocialesAgregadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/jul/2019
        /// Efecto: actualiza la la pagina actual y muestra los datos de la misma
        /// Requiere: -
        /// Modifica: elementos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptPaginacion3_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual3 = Convert.ToInt32(e.CommandArgument.ToString());
            cargarDatosTblCargasSocialesAgregadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: marca el boton de la pagina seleccionada
        /// Requiere: dar clic al boton de paginacion
        /// Modifica: color del boton seleccionado
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptPaginacion3_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion3");
            if (lnkPagina.CommandArgument != paginaActual3.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }

        #endregion
        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
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

        #endregion

        #region eventos
        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: filtra la tabla segun los datos ingresados en el textbox
        /// Requiere: ingresar datos en textbox
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBuscarDesc_TextChanged(object sender, EventArgs e)
        {
            paginaActual = 0;
            mostrarDatosTabla();
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: levanta modal para ingresar una nueva carga social
        /// Requiere: dar clic al boton de "Nueva carga social"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaCargaSocial_Click(object sender, EventArgs e)
        {
            txtDescModalNuevo.Text = "";
            txtPorcentajeModalNuevo.Text = "";
            lblPeriodoModalNuevo.Text= Convert.ToInt32(ddlPeriodo.SelectedValue).ToString();

            LinkedList<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(Convert.ToInt32(ddlPeriodo.SelectedValue));

            ddlPartidasModalNuevo.DataSource = listaPartidas;
            ddlPartidasModalNuevo.DataTextField = "numeroPartida";
            ddlPartidasModalNuevo.DataValueField = "idPartida";
            ddlPartidasModalNuevo.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaCargaSocial();", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 20/sep/2019
        /// Efecto: 
        /// Requiere:
        /// Modifica:
        /// Devuelve:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaCargaSocialModal_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtDescModalNuevo.Text))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaCargaSocial').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaCargaSocial();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Se debe ingresar una descripción" + "');", true);
            }
            else
            {
                Double porcentaje = 0;
                String txtPorcentaje = txtPorcentajeModalNuevo.Text.Replace(".",",");
                if (Double.TryParse(txtPorcentaje,out porcentaje))
                {
                    txtPorcentajeModalNuevo.Text = porcentaje.ToString();
                }

                if (String.IsNullOrEmpty(ddlPartidasModalNuevo.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaCargaSocial').hide();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaCargaSocial();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Se debe seleccionar una escala salarial" + "');", true);
                }
                else
                {
                    Partida partida = new Partida();
                    partida.idPartida = Convert.ToInt32(ddlPartidasModalNuevo.SelectedValue);

                    Periodo periodo = new Periodo();
                    periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

                    CargaSocial cargaSocial = new CargaSocial();

                    cargaSocial.descCargaSocial = txtDescModalNuevo.Text;
                    cargaSocial.porcentajeCargaSocial = porcentaje;
                    cargaSocial.partida = partida;
                    cargaSocial.periodo = periodo;
                    cargaSocial.activo = true;

                    cargaSocialServicios.insertarCargaSocial(cargaSocial);

                    List<CargaSocial> listaCargasSociales = cargaSocialServicios.getCargasSocialesActivasPorPeriodo(periodo);

                    Session["listaCargasSociales"] = listaCargasSociales;
                    Session["listaCargasSocialesFiltrada"] = listaCargasSociales;

                    mostrarDatosTabla();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaCargaSocial').hide();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se ingreso la carga social correctamente" + "');", true);
                }
                
            }
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 23/sep/2019
        /// Efecto: levanta modal para editar una carga social
        /// Requiere: dar clic en el boton de "Editar carga social"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int idCargaSocial = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<CargaSocial> listaJornadasFiltrada = (List<CargaSocial>)Session["listaCargasSocialesFiltrada"];
            CargaSocial cargaSocialEditar = new CargaSocial();

            foreach (CargaSocial cargaSocial in listaJornadasFiltrada)
            {
                if (idCargaSocial == cargaSocial.idCargaSocial)
                {
                    cargaSocialEditar = cargaSocial;
                    break;
                }
            }

            Session["cargaSocialSeleccionada"] = cargaSocialEditar;

            txtDescModalEditar.Text = cargaSocialEditar.descCargaSocial;
            txtPorcentajeModalEditar.Text = cargaSocialEditar.porcentajeCargaSocial.ToString();

            lblPeriodoModalEditar.Text = cargaSocialEditar.periodo.anoPeriodo.ToString();

            LinkedList<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(Convert.ToInt32(cargaSocialEditar.periodo.anoPeriodo));

            ddlPartidasModalEditar.DataSource = listaPartidas;
            ddlPartidasModalEditar.DataTextField = "numeroPartida";
            ddlPartidasModalEditar.DataValueField = "idPartida";
            ddlPartidasModalEditar.DataBind();

            int contIndex = 0;

            foreach (ListItem item in ddlPartidasModalEditar.Items)
            {
                if (Convert.ToInt32(item.Value) == cargaSocialEditar.partida.idPartida)
                {
                    ddlPartidasModalEditar.SelectedIndex = contIndex;
                    break;
                }
                contIndex++;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarCargaSocial();", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 23/sep/2019
        /// Efecto: actualiza de forma logica la carga social seleccionada
        /// Requiere: llenar los campos requeridos y darle clic al boton de "Editar"
        /// Modifica: la carga social seleccionada
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditarCargaSocialModal_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtDescModalEditar.Text))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarCargaSocial').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarCargaSocial();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Se debe ingresar una descripción" + "');", true);
            }
            else
            {
                Double porcentaje = 0;
                String txtSalario = txtPorcentajeModalEditar.Text.Replace(".", ",");
                if (Double.TryParse(txtSalario, out porcentaje))
                {
                    txtPorcentajeModalEditar.Text = porcentaje.ToString();
                }

                if (String.IsNullOrEmpty(ddlPartidasModalEditar.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaCargaSocial').hide();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaCargaSocial();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Se debe seleccionar una escala salarial" + "');", true);
                }
                else
                {
                    Partida partida = new Partida();
                    partida.idPartida = Convert.ToInt32(ddlPartidasModalEditar.SelectedValue);

                    Periodo periodo = new Periodo();
                    periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

                    CargaSocial cargaSocial = (CargaSocial)Session["cargaSocialSeleccionada"];

                    cargaSocial.descCargaSocial = txtDescModalEditar.Text;
                    cargaSocial.porcentajeCargaSocial = porcentaje;
                    cargaSocial.partida = partida;
                    cargaSocial.periodo = periodo;
                    cargaSocial.activo = true;

                    cargaSocialServicios.actualizarCargaSocialLogica(cargaSocial);

                    List<CargaSocial> listaCargasSociales = cargaSocialServicios.getCargasSocialesActivasPorPeriodo(periodo);

                    Session["listaCargasSociales"] = listaCargasSociales;
                    Session["listaCargasSocialesFiltrada"] = listaCargasSociales;

                    mostrarDatosTabla();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarCargaSocial').hide();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se actualizo la carga social correctamente" + "');", true);
                }
            }
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 23/sep/2019
        /// Efecto: levanta modal para eliminar una carga social
        /// Requiere: dar clic en el boton de "Eliminar carga social"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int idCargaSocial = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<CargaSocial> listaCargaSocialFiltrada = (List<CargaSocial>)Session["listaCargasSocialesFiltrada"];
            CargaSocial cargaSocialEliminar = new CargaSocial();

            foreach (CargaSocial cargaSocial in listaCargaSocialFiltrada)
            {
                if (idCargaSocial == cargaSocial.idCargaSocial)
                {
                    cargaSocialEliminar = cargaSocial;
                    break;
                }
            }

            Session["cargaSocialSeleccionada"] = cargaSocialEliminar;

            txtDescModalEliminar.Text = cargaSocialEliminar.descCargaSocial;
            txtPorcentajeModalEliminar.Text = cargaSocialEliminar.porcentajeCargaSocial.ToString();
            txtPeriodoModalEliminar.Text = cargaSocialEliminar.periodo.anoPeriodo.ToString();
            txtPartidaModalEliminar.Text = cargaSocialEliminar.partida.numeroPartida;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarCargaSocial();", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 23/sep/2019
        /// Efecto: elimina de forma logica la carga social seleccionada
        /// Requiere: dar clic al boton de "Eliminar"
        /// Modifica: el atributo de activo de la carga social seleccionada y quita la carga social de la tabla de cargas sociales
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarCargaSocialModal_Click(object sender, EventArgs e)
        {
            CargaSocial cargaSocial = (CargaSocial)Session["cargaSocialSeleccionada"];

            cargaSocialServicios.eliminarCargaSocialLogica(cargaSocial);

            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            List<CargaSocial> listaCargasSociales = cargaSocialServicios.getCargasSocialesActivasPorPeriodo(periodo);

            Session["listaCargasSociales"] = listaCargasSociales;
            Session["listaCargasSocialesFiltrada"] = listaCargasSociales;

            mostrarDatosTabla();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarCargaSocial').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se elimino la carga social correctamente" + "');", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 23/sep/2019
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

            List<CargaSocial> listaCargasSociales = cargaSocialServicios.getCargasSocialesActivasPorPeriodo(periodo);

            Session["listaCargasSociales"] = listaCargasSociales;
            Session["listaCargasSocialesFiltrada"] = listaCargasSociales;

            mostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: levanta el modal para pasar las cargas sociales entre los períodos
        /// Requiere: dar clic en el boton de "Pasar cargas sociales"
        /// Modifca: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPasarCargasSociales_Click(object sender, EventArgs e)
        {
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            lblPeriodoSeleccionado.Text = periodo.anoPeriodo.ToString();

            // cargar periodos en dropdownlist
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            ddlPeriodoModalPasarCargasSociales.Items.Clear();
            periodos = this.periodoServicios.ObtenerTodos();
            int anoHabilitado = 0;

            if (periodos.Count > 0)
            {
                foreach (Periodo periodoTemp in periodos)
                {
                    string nombre;

                    if (periodoTemp.habilitado)
                    {
                        nombre = periodoTemp.anoPeriodo.ToString() + " (Actual)";
                        anoHabilitado = periodoTemp.anoPeriodo;
                    }
                    else
                    {
                        nombre = periodoTemp.anoPeriodo.ToString();
                    }

                    if (periodo.anoPeriodo != periodoTemp.anoPeriodo)
                    {
                        ListItem itemPeriodo = new ListItem(nombre, periodoTemp.anoPeriodo.ToString());
                        ddlPeriodoModalPasarCargasSociales.Items.Add(itemPeriodo);
                    }
                }

            }
            //fin de dopdownlist

            List<CargaSocial> listaCargasSociales = cargaSocialServicios.getCargasSocialesActivasPorPeriodo(periodo);

            Session["listaCargasSocialesAPasar"] = listaCargasSociales;
            Session["listaCargasSocialesAPasarFiltrada"] = listaCargasSociales;

            cargarDatosTblCargasSocialesAPasar();

            Periodo periodoAgregados = new Periodo();
            periodoAgregados.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasarCargasSociales.SelectedValue);

            List<CargaSocial> listaCargasSocialesAgregadas = cargaSocialServicios.getCargasSocialesActivasPorPeriodo(periodoAgregados);

            Session["listaCargasSocialesAgregadas"] = listaCargasSocialesAgregadas;
            Session["listaCargasSocialesAgregadasFiltrada"] = listaCargasSocialesAgregadas;

            cargarDatosTblCargasSocialesAgregadas();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarCargaSocial();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: carga las cargas sociales que contiene el periodo seleccionado
        /// Requiere: cambiar el periodo
        /// Modifica: la lista de cargas sociales que se muestran en la tabla de cargas sociales asosciadas al periodo seleccionado 
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPeriodoModalPasarCargasSociales_SelectedIndexChanged(object sender, EventArgs e)
        {
            Periodo periodoAgregados = new Periodo();
            periodoAgregados.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasarCargasSociales.SelectedValue);

            List<CargaSocial> listaCargasSocialesAgregadas = cargaSocialServicios.getCargasSocialesActivasPorPeriodo(periodoAgregados);

            Session["listaCargasSocialesAgregadas"] = listaCargasSocialesAgregadas;
            Session["listaCargasSocialesAgregadasFiltrada"] = listaCargasSocialesAgregadas;

            cargarDatosTblCargasSocialesAgregadas();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarCargaSocial').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarCargaSocial();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: filtra la tabla de cargas sociales que estan al lado izquierdo para pasar 
        /// Requiere: dar clic al boton de "Buscar" o darle enter al campo de texto
        /// Modifica: los datos que se muestran en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFiltrarCargasSocialesAPasar_Click(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            cargarDatosTblCargasSocialesAPasar();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarCargaSocial').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarCargaSocial();", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: filtra la tabla de cargas sociales que estan al lado derecho para pasar 
        /// Requiere: dar clic al boton de "Buscar" o darle enter al campo de texto
        /// Modifica: los datos que se muestran en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFiltrarEscalasAgregadas_Click(object sender, EventArgs e)
        {
            paginaActual3 = 0;

            Periodo periodoAgregados = new Periodo();
            periodoAgregados.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasarCargasSociales.SelectedValue);

            List<CargaSocial> listaCargasSocialesAgregadas = cargaSocialServicios.getCargasSocialesActivasPorPeriodo(periodoAgregados);

            Session["listaCargasSocialesAgregadas"] = listaCargasSocialesAgregadas;
            Session["listaCargasSocialesAgregadasFiltrada"] = listaCargasSocialesAgregadas;

            cargarDatosTblCargasSocialesAgregadas();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarCargaSocial').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarCargaSocial();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/sep/2019
        /// Efecto: pone la carga social seleccionada en el perido seleccionado
        /// Requiere: escoger período a pasar y darle clic al boton de "seleccionar"
        /// Modifica: cargas sociales agregadas al período
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSeleccionarCargaSocial_Click(object sender, EventArgs e)
        {
            int idCargaSocial = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<CargaSocial> listaCargasSociales = (List<CargaSocial>)Session["listaCargasSocialesAPasarFiltrada"];

            foreach (CargaSocial cargaSocial in listaCargasSociales)
            {
                if (cargaSocial.idCargaSocial == idCargaSocial)
                {
                    Periodo periodo = new Periodo();
                    periodo.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasarCargasSociales.SelectedValue);

                    CargaSocial carga = cargaSocial;
                    carga.periodo = periodo;

                    Partida partida = partidaServicios.getPartidaPorNumeroYPeriodo(cargaSocial.partida, periodo);

                    if(partida.idPartida == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "En el período "+periodo.anoPeriodo+" no se encuentra la partida "+ cargaSocial.partida.numeroPartida+ "');", true);
                    }
                    else {
                        carga.partida = partida;

                        cargaSocialServicios.insertarCargaSocial(carga);
                    }
                    break;
                }
            }

            Periodo periodoE = new Periodo();
            periodoE.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            listaCargasSociales = cargaSocialServicios.getCargasSocialesActivasPorPeriodo(periodoE);

            Session["listaCargasSociales"] = listaCargasSociales;
            Session["listaCargasSocialesFiltrada"] = listaCargasSociales;

            mostrarDatosTabla();

            listaCargasSociales = cargaSocialServicios.getCargasSocialesActivasPorPeriodo(periodoE);

            Session["listaCargasSocialesAPasar"] = listaCargasSociales;
            Session["listaCargasSocialesAPasarFiltrada"] = listaCargasSociales;

            cargarDatosTblCargasSocialesAPasar();

            Periodo periodoAgregados = new Periodo();
            periodoAgregados.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasarCargasSociales.SelectedValue);

            List<CargaSocial> listaCargasSocialesAgregadas = cargaSocialServicios.getCargasSocialesActivasPorPeriodo(periodoAgregados);

            Session["listaCargasSocialesAgregadas"] = listaCargasSocialesAgregadas;
            Session["listaCargasSocialesAgregadasFiltrada"] = listaCargasSocialesAgregadas;

            cargarDatosTblCargasSocialesAgregadas();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarCargaSocial", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarCargaSocial').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarCargaSocial();", true);
        }
        #endregion
    }
}