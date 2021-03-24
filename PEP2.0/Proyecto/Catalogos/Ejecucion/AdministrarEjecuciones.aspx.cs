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

namespace Proyecto.Catalogos.Ejecucion
{
    public partial class AdministrarEjecuciones : System.Web.UI.Page
    {
        #region variables globales
        PeriodoServicios periodoServicios = new PeriodoServicios();
        ProyectoServicios proyectoServicios = new ProyectoServicios();
        EjecucionServicios ejecucionServicios = new EjecucionServicios();
        #endregion

        #region paginacion
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
                Session["periodoSeleccionado"] = null;
                Session["proyectoSeleccionado"] = null;
                llenarDdlPeriodos();
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

                CargarProyectos();
            }

        }

        /// <summary>
        /// Leonardo Carrion
        /// 23/feb/2021
        /// Efecto: llena el DropDownList con los proyectos que se encuentran en la base de datos segun el periodo seleccionado
        /// Requiere: seleccionar periodo
        /// Modifica: DropDownList
        /// Devuelve: -
        /// </summary>
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
                    mostrarDatosTabla();
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
            List<Entidades.Ejecucion> listaEjecuciones = new List<Entidades.Ejecucion>();

            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodos.SelectedValue);

            Proyectos proyecto = new Proyectos();
            proyecto.idProyecto = Convert.ToInt32(ddlProyectos.SelectedValue);

            listaEjecuciones = ejecucionServicios.getEjecucionesPorPeriodoYProyecto(periodo, proyecto);

            String numeroEjecucion = "", numeroReferencia = "", estado = "", tipoTramite = "", monto = "", realizadoPor = "", ingresado = "";

            if (!String.IsNullOrEmpty(txtBuscarNumeroEjecucion.Text))
            {
                numeroEjecucion = txtBuscarNumeroEjecucion.Text;
            }
            if (!String.IsNullOrEmpty(txtBuscarNumeroReferencia.Text))
            {
                numeroReferencia = txtBuscarNumeroReferencia.Text;
            }
            if (!String.IsNullOrEmpty(txtBuscarEstado.Text))
            {
                estado = txtBuscarEstado.Text;
            }
            if (!String.IsNullOrEmpty(txtBuscarTipoTramite.Text))
            {
                tipoTramite = txtBuscarTipoTramite.Text;
            }
            if (!String.IsNullOrEmpty(txtBuscarMonto.Text))
            {
                monto = txtBuscarMonto.Text;
            }
            if (!String.IsNullOrEmpty(txtBuscarRealizadoPor.Text))
            {
                realizadoPor = txtBuscarRealizadoPor.Text;
            }
            if (!String.IsNullOrEmpty(txtBuscarIngresado.Text))
            {
                ingresado = txtBuscarIngresado.Text;
            }

            List<Entidades.Ejecucion> listaEjecucionesFiltrada = (List<Entidades.Ejecucion>)listaEjecuciones.Where(eje => eje.idEjecucion.ToString().ToUpper().Contains(numeroEjecucion.ToUpper()) &&
           eje.numeroReferencia.ToUpper().Contains(numeroReferencia.ToUpper()) && eje.estadoEjecucion.descripcion.ToUpper().Contains(estado.ToUpper()) &&
           eje.tipoTramite.nombreTramite.ToUpper().Contains(tipoTramite.ToUpper()) && eje.monto.ToString().ToUpper().Contains(monto.ToUpper()) &&
           eje.realizadoPor.ToUpper().Contains(realizadoPor.ToUpper()) && eje.fecha.ToShortDateString().ToUpper().Contains(ingresado.ToUpper())).ToList();

            var dt = listaEjecucionesFiltrada;
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

            rpEjecuciones.DataSource = pgsource;
            rpEjecuciones.DataBind();

            //metodo que realiza la paginacion
            Paginacion();
        }
        #endregion

        #region paginacion
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

        #endregion

        #region eventos
        ///// <summary>
        ///// Kevin Picado
        ///// 22/oct/2020
        ///// Efecto: deshabilita botones
        ///// Requiere: -
        ///// Modifica: -
        ///// Devuelve: -
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        protected void rpEjecucion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //LinkButton btnEditar = e.Item.FindControl("btnEditarEjecucion") as LinkButton;

                //int idEjecucion = Convert.ToInt32(btnEditar.CommandArgument.ToString());
                //int Estado = ejecucionServicios.EstadoEjecucion(idEjecucion);

                //if (Estado == 2)
                //{
                //    btnEditar.Visible = false;
                //}
                //else
                //{
                //    btnEditar.Visible = true;
                //}
            }
        }

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
        protected void ddlPeriodos_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarProyectos();
        }
        /// <summary>
        /// Leonardo Carrion
        /// 02/oct/2019
        /// Efecto: cambia los datos de la tabla segun el proyecto seleccionado
        /// Requiere: cambiar proyecto
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            mostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 23/feb/2021
        /// Efecto: redirecciona a la pantalla de nueva ejecucion
        /// Requiere: dar clic al boton de "Nueva"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaEjecucion_Click(object sender, EventArgs e)
        {
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodos.SelectedValue);

            Proyectos proyecto = new Proyectos();
            proyecto.idProyecto = Convert.ToInt32(ddlProyectos.SelectedValue);

            Session["periodoSeleccionado"] = periodo.anoPeriodo;
            Session["proyectoSeleccionado"] = proyecto.idProyecto;
            String url = Page.ResolveUrl("~/Catalogos/Ejecucion/NuevaEjecucion.aspx");
            Response.Redirect(url);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 23/feb/2021
        /// Efecto: filtra la tabla de buscar
        /// Requiere: inresar datos en los filtros y dar clic en boton de enter
        /// Modifica: datos en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            paginaActual = 0;
            mostrarDatosTabla();
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 11/mar/2021
        /// Efecto: redirecciona a la pantalla de editar ejecucion
        /// Requiere: dar clic en el boton de Editar
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int idEjecucion = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Entidades.Ejecucion> listaEjecuciones = new List<Entidades.Ejecucion>();

            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodos.SelectedValue);

            Proyectos proyecto = new Proyectos();
            proyecto.idProyecto = Convert.ToInt32(ddlProyectos.SelectedValue);

            listaEjecuciones = ejecucionServicios.getEjecucionesPorPeriodoYProyecto(periodo, proyecto);

            Entidades.Ejecucion ejecucion = (Entidades.Ejecucion)listaEjecuciones.Where(eje => eje.idEjecucion == idEjecucion).ToList().First();

            Session["ejecucionEditar"] = ejecucion;

            Session["periodoSeleccionado"] = periodo.anoPeriodo;
            Session["proyectoSeleccionado"] = proyecto.idProyecto;
            String url = Page.ResolveUrl("~/Catalogos/Ejecucion/EditarEjecucion.aspx");
            Response.Redirect(url);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: redirecciona a la pantalla de Eliminar Ejecucion
        /// Requiere: dar clic en el boton de Eliminar
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int idEjecucion = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Entidades.Ejecucion> listaEjecuciones = new List<Entidades.Ejecucion>();

            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodos.SelectedValue);

            Proyectos proyecto = new Proyectos();
            proyecto.idProyecto = Convert.ToInt32(ddlProyectos.SelectedValue);

            listaEjecuciones = ejecucionServicios.getEjecucionesPorPeriodoYProyecto(periodo, proyecto);

            Entidades.Ejecucion ejecucion = (Entidades.Ejecucion)listaEjecuciones.Where(eje => eje.idEjecucion == idEjecucion).ToList().First();

            Session["ejecucionEliminar"] = ejecucion;

            Session["periodoSeleccionado"] = periodo.anoPeriodo;
            Session["proyectoSeleccionado"] = proyecto.idProyecto;
            String url = Page.ResolveUrl("~/Catalogos/Ejecucion/EliminarEjecucion.aspx");
            Response.Redirect(url);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: redirecciona a la pantalla de Ver Ejecucion
        /// Requiere: dar clic en el boton de Ver
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVer_Click(object sender, EventArgs e)
        {
            int idEjecucion = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Entidades.Ejecucion> listaEjecuciones = new List<Entidades.Ejecucion>();

            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodos.SelectedValue);

            Proyectos proyecto = new Proyectos();
            proyecto.idProyecto = Convert.ToInt32(ddlProyectos.SelectedValue);

            listaEjecuciones = ejecucionServicios.getEjecucionesPorPeriodoYProyecto(periodo, proyecto);

            Entidades.Ejecucion ejecucion = (Entidades.Ejecucion)listaEjecuciones.Where(eje => eje.idEjecucion == idEjecucion).ToList().First();

            Session["ejecucionVer"] = ejecucion;

            Session["periodoSeleccionado"] = periodo.anoPeriodo;
            Session["proyectoSeleccionado"] = proyecto.idProyecto;
            String url = Page.ResolveUrl("~/Catalogos/Ejecucion/VerEjecucion.aspx");
            Response.Redirect(url);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 23/mar/2021
        /// Efecto: levanta modal para confirmar si se desa comprometer la ejecucion
        /// Requiere: dar clic en el boton de comprometer
        /// Modifica: -
        /// Deuvelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnComprometer_Click(object sender, EventArgs e)
        {
            int idEjecucion = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Entidades.Ejecucion> listaEjecuciones = new List<Entidades.Ejecucion>();

            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodos.SelectedValue);

            Proyectos proyecto = new Proyectos();
            proyecto.idProyecto = Convert.ToInt32(ddlProyectos.SelectedValue);

            listaEjecuciones = ejecucionServicios.getEjecucionesPorPeriodoYProyecto(periodo, proyecto);

            Entidades.Ejecucion ejecucion = (Entidades.Ejecucion)listaEjecuciones.Where(eje => eje.idEjecucion == idEjecucion).ToList().First();

            Session["ejecucionVer"] = ejecucion;
            lblConfirmarComprometer.Text = "¿Seguro o segura que desea comprometer la ejecución número "+ejecucion.idEjecucion+"?";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalComprometer();", true);
        }
        #endregion
    }
}