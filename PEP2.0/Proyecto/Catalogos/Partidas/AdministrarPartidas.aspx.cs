using Entidades;
using PEP;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;


namespace Proyecto.Catalogos.Partidas
{
    public partial class AdministrarPartidas : System.Web.UI.Page
    {

        #region variables globales
        PeriodoServicios periodoServicios = new PeriodoServicios();
        PartidaServicios partidaservicios = new PartidaServicios();
        readonly PagedDataSource pgsource = new PagedDataSource();
        #endregion

        #region page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2, 9, 17 };
            Utilidades.escogerMenu(Page, rolesPermitidos);
            if (!IsPostBack)
            {
                Session["listaPartidas"] = null;
                Session["listaPartidasFiltrada"] = null;

                Session["listaPartidaAPasar"] = null;
                Session["listaPartidaAPasarFiltrada"] = null;

                Session["listaPartidaAgregadas"] = null;
                Session["listaPartidaAgregadasFiltrada"] = null;

                llenarDdlPeriodos();

                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);


                LinkedList<Partida> listaPartidas = partidaservicios.ObtenerPorPeriodo(Int32.Parse(ddlPeriodo.SelectedValue));

                Session["listaPartidas"] = listaPartidas;
                Session["listaPartidasFiltrada"] = listaPartidas;

                mostrarDatosTabla();

            }
        }
        #endregion

        #region eventos
        /// <summary>
        /// Jesús Torres
        /// 19/sept/2019
        /// Efecto: Selecciona un periodo, de acuerdo al periodo seleccionada se llenará la tabla con los datos correspondientes
        /// Requiere: 
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            LinkedList<Partida> listaPartidas = partidaservicios.ObtenerPorPeriodo(periodo.anoPeriodo);

            Session["listaPartidas"] = listaPartidas;
            Session["listaPartidasFiltrada"] = listaPartidas;

            mostrarDatosTabla();
        }

        protected void btnPartidas_Click(object sender, EventArgs e)
        {

        }

        protected void btnNuevaPartida_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Jesús Torres
        /// 19/sept/2019
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
        /// Jesús Torres
        /// 19/sept/2019
        /// Efecto: filtra la tabla segun los datos ingresados en los filtros
        /// Requiere: accede al evento con enter cuando se quiere buscar algo especifico de la tabla
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

        protected void btnEditar_Click(object sender, EventArgs e)
        {

        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region metodos
        /// <summary>
        /// Jesús Torres 
        /// 19/sept/2019
        /// Efecto: llena el DropDownList con los periodos que se encuentran en la base de datos
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
        /// Jesús Torres
        /// 19/sept/2019
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        private void mostrarDatosTabla()
        {
            LinkedList<Partida> listaPartidas = (LinkedList<Partida>)Session["listaPartidas"];

            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarDesc.Text))
            {
                desc = txtBuscarDesc.Text;
            }

            List<Partida> listaPartidasFiltrada = (List<Partida>)listaPartidas.Where(partida => partida.descripcionPartida.ToUpper().Contains(desc.ToUpper())).ToList();

            Session["listaPartidaFiltrada"] = listaPartidasFiltrada;

            var dt = listaPartidasFiltrada;
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
            rpPartidas.DataSource = pgsource;
            rpPartidas.DataBind();
            //metodo que realiza la paginacion
            Paginacion();
        }


        #endregion

        #region paginacion Actual
        //Bloque que contiene get y set de pagina actual
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

        #region paginacion
        /// <summary>
        /// Jesús Torres
        /// 19/sept/2019
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
        /// Jesús Torres
        /// 19/sept/2019
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
        /// Jesús Torres
        /// 19/sept/2019
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
        /// Jesús Torres
        /// 19/sept/2019
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
        /// Jesús Torres
        /// 19/sept/2019
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
        /// Jesús Torres 
        /// 19/sept/2019
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
        /// Jesús Torres
        /// 19/sep/2019
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