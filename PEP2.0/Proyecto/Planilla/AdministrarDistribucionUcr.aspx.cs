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
    public partial class AdministrarDistribucionUcr : System.Web.UI.Page
    {

        #region variables globales
        private FuncionarioServicios funcionarioServicios = new FuncionarioServicios();
        private PeriodoServicios periodoServicios = new PeriodoServicios();
        private ProyectoServicios proyectoServicios = new ProyectoServicios();
        private UnidadServicios unidadServicios = new UnidadServicios();
        private JornadaServicios jornadaServicios = new JornadaServicios();
        #endregion

        #region variables globales paginacion Funcionarios
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

        #region variables globales unidades
        readonly PagedDataSource pgsourceUnidad = new PagedDataSource();
        int primerIndexUnidad, ultimoIndexUnidad;
        private int paginaActualUnidad
        {
            get
            {
                if (ViewState["paginaActualUnidad"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["paginaActualUnidad"]);
            }
            set
            {
                ViewState["paginaActualUnidad"] = value;
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
                List<Funcionario> listaFuncionarios = funcionarioServicios.getFuncionarios();
                Session["listaFuncionarios"] = listaFuncionarios;
                Session["listaFuncionariosFiltrada"] = listaFuncionarios;
                mostrarDatosTabla();
                //llenar drop down list
                LinkedList<Periodo> periodos = (LinkedList<Periodo>)periodoServicios.ObtenerTodos();
                ddlPeriodo.DataSource = periodos;
                ddlPeriodo.DataTextField = "anoPeriodo";
                ddlPeriodo.DataValueField = "anoPeriodo";
                ddlPeriodo.DataBind();
                LinkedList<Proyectos> proyectos = proyectoServicios.ObtenerPorPeriodo(periodos.First().anoPeriodo);
                ddlProyecto.DataSource = proyectos;
                ddlProyecto.DataTextField = "nombreProyecto";
                ddlProyecto.DataValueField = "idProyecto";
                ddlProyecto.SelectedValue = proyectos.First.Value.idProyecto.ToString();
                ddlProyecto.DataBind();
                Session["listaUnidades"] = unidadServicios.ObtenerPorProyecto(Convert.ToInt32(ddlProyecto.SelectedValue));
                Session["listaUnidadesConJornadaAsignada"] = new List<UnidadFuncionario>();
                mostrarTablaUnidades();
            }
        }
        #endregion

        #region logica

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
            List<Funcionario> listaFuncionarios = (List<Funcionario>)Session["listaFuncionarios"];

            String nombre = "";

            if (!String.IsNullOrEmpty(txtBuscarNombre.Text))
            {
                nombre = txtBuscarNombre.Text;
            }

            List<Funcionario> listaFuncionarioFiltrada = (List<Funcionario>)listaFuncionarios.Where(funcionario => funcionario.nombreFuncionario.ToString().Contains(nombre)).ToList();

            Session["listaFuncionariosFiltrada"] = listaFuncionarioFiltrada;

            var dt = listaFuncionarioFiltrada;
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

            rpFuncionarios.DataSource = pgsource;
            rpFuncionarios.DataBind();

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

        #region paginacion
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
        #endregion

        #region paginacion tabla unidades

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 07/10/2019
        /// Efecto : Llena la tabla con las unidades de un proyecto
        /// Requiere : -
        /// Modifica : Tabla de unidades
        /// Devuelve : -
        /// </summary>
        public void mostrarTablaUnidades()
        {
            LinkedList<Unidad> listaUnidades = unidadServicios.ObtenerPorProyecto(Convert.ToInt32(ddlProyecto.SelectedValue));
            /*FILTRO*/

            var dt = listaUnidades;
            pgsourceUnidad.DataSource = dt;
            pgsourceUnidad.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            pgsourceUnidad.PageSize = elmentosMostrar;
            pgsourceUnidad.CurrentPageIndex = paginaActualUnidad;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginasUnidad"] = pgsourceUnidad.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpaginaUnidad.Text = "Página " + (paginaActualUnidad + 1) + " de " + pgsourceUnidad.PageCount + " (" + dt.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnteriorUnidad.Enabled = !pgsourceUnidad.IsFirstPage;
            lbSiguienteUnidad.Enabled = !pgsourceUnidad.IsLastPage;
            lbPrimeroUnidad.Enabled = !pgsourceUnidad.IsFirstPage;
            lbUltimoUnidad.Enabled = !pgsourceUnidad.IsLastPage;

            rpUnidProyecto.DataSource = pgsourceUnidad;
            rpUnidProyecto.DataBind();
            
            //metodo que realiza la paginacion
            PaginacionUnidad();
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 07/10/2019
        /// Efecto : Realiza la paginacion en la tabla de unidades
        /// Requiere : -
        /// Modifica : Tabla de unidades
        /// Devuelve : -
        /// </summary>
        private void PaginacionUnidad()
        {
            var dt = new DataTable();
            dt.Columns.Add("IndexPagina"); //Inicia en 0
            dt.Columns.Add("PaginaText"); //Inicia en 1

            primerIndexUnidad = paginaActualUnidad - 2;
            if (paginaActualUnidad > 2)
                ultimoIndexUnidad = paginaActualUnidad + 2;
            else
                ultimoIndexUnidad = 4;

            //se revisa que la ultima pagina sea menor que el total de paginas a mostrar, sino se resta para que muestre bien la paginacion
            if (ultimoIndexUnidad > Convert.ToInt32(ViewState["TotalPaginasUnidad"]))
            {
                ultimoIndexUnidad = Convert.ToInt32(ViewState["TotalPaginasUnidad"]);
                primerIndexUnidad = ultimoIndexUnidad - 4;
            }

            if (primerIndexUnidad < 0)
                primerIndexUnidad = 0;

            //se crea el numero de paginas basado en la primera y ultima pagina
            for (var i = primerIndexUnidad; i < ultimoIndexUnidad; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaginacionUnidad.DataSource = dt;
            rptPaginacionUnidad.DataBind();
        }

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
        protected void lbPrimeroUnidad_Click(object sender, EventArgs e)
        {
            paginaActualUnidad = 0;
            mostrarTablaUnidades();
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
        protected void lbAnteriorUnidad_Click(object sender, EventArgs e)
        {
            paginaActualUnidad -= 1;
            mostrarTablaUnidades();
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
        protected void rptPaginacionUnidad_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActualUnidad = Convert.ToInt32(e.CommandArgument.ToString());
            mostrarTablaUnidades();
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
        protected void rptPaginacionUnidad_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacionUnidad");
            if (lnkPagina.CommandArgument != paginaActualUnidad.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#00Unidadda4");
            lnkPagina.ForeColor = Color.FromName("#000000");
        }

        /// <summary>
        /// Leonardo Carrion
        /// 14/jun/2019
        /// Efecto: se devuelve a la pagina siguiente y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Siguiente pagina"
        /// Modifica: elementos mostrados en la tabla 
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSiguienteUnidad_Click(object sender, EventArgs e)
        {
            paginaActualUnidad += 1;
            mostrarTablaUnidades();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -lbPrimero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimoUnidad_Click(object sender, EventArgs e)
        {
            paginaActualUnidad = (Convert.ToInt32(ViewState["TotalPaginasUnidad"]) - 1);
            mostrarTablaUnidades();
        }
        #endregion FIN paginacion tabla unidades

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 20/09/2019
        /// Efecto : Muestra los datos del funcionario seleccionado
        /// Requiere : Clickear el boton "Seleccionar"
        /// Modifica : -
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelccionar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            Funcionario funcionarioVer = null;
            List<Funcionario> funcionarios = (List<Funcionario>)Session["listaFuncionariosFiltrada"];
            foreach (Funcionario funcionario in funcionarios)
            {
                if (funcionario.idFuncionario == id)
                {
                    funcionarioVer = funcionario;
                    break;
                }
            }
            lblPeriodo.Text = ddlPeriodo.SelectedValue;
            lblProyecto.Text = ddlProyecto.SelectedItem.Text;
            lblJornada.Text = funcionarioVer.JornadaLaboral.descJornada + " , " + funcionarioVer.JornadaLaboral.porcentajeJornada+"%";
            lblFuncionario.Text = funcionarioVer.nombreFuncionario;
            Session["idFuncionarioSeleccionado"] = funcionarioVer.idFuncionario;
            mostrarTablaUnidades();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalDistribuirJornada();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
        /// Efecto: devuelve a la pantalla de administrar planillas
        /// Requiere: dar clic al boton de "regresar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Default.aspx");
            Response.Redirect(url);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 07/10/2019
        /// Efecto : Completa el formulario para asinar una jornada laboral a una unidad
        /// Requiere : Seleccionar una unidad
        /// Modifica : Formulario de asignar jornada 
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelccionarUnidad_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            Unidad unidadSeleccionada = null;
            LinkedList<Unidad> unidades = unidadServicios.ObtenerPorProyecto(Convert.ToInt32(ddlProyecto.SelectedValue));
            foreach (Unidad unidad in unidades)
            {
                if (unidad.idUnidad == id)
                {
                    unidadSeleccionada = unidad;
                    break;
                }
            }
            IdUnidadSeleccionada.Value = unidadSeleccionada.idUnidad.ToString();
            lblUnidad.Text = unidadSeleccionada.nombreUnidad;
            List<Jornada> jornadas = jornadaServicios.getJornadasActivas();
            ddlAsignarJornada.DataSource = jornadas;
            ddlAsignarJornada.DataTextField = "porcentajeJornada";
            ddlAsignarJornada.DataValueField = "porcentajeJornada";
            ddlAsignarJornada.DataBind();
            mostrarTablaUnidades();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalAsignarJornada();", true);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 07/10/2019
        /// Efecto : Asigna una jornada laboral a la unidad seleccionada 
        /// Requiere : Seleccionar la jornada laboral y clickear el boton "Asignar" del formulario
        /// Modifica : Jornada laboral del funcionario
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAsignarJornada_Click(object sender, EventArgs e)
        {
            int idUnidad = Convert.ToInt32(IdUnidadSeleccionada.Value);
            string unidad = lblUnidad.Text;
            double porcentaje = Convert.ToDouble(ddlAsignarJornada.SelectedValue);
            UnidadFuncionario unidadAsignada = new UnidadFuncionario();
            unidadAsignada.idUnidad = idUnidad;
            unidadAsignada.jornadaAsignada = porcentaje;
            unidadAsignada.idFuncionario = Convert.ToInt32(Session["idFuncionarioSeleccionado"]);
            List<UnidadFuncionario> unidadesFuncionario = (List<UnidadFuncionario>)Session["listaUnidadesConJornadaAsignada"];
            unidadesFuncionario.Add(unidadAsignada);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "agregarDistribucion();", true);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 27/09/2019
        /// Efecto : Actualiza la lista de proyectos de un periodo
        /// Requiere : Cambiar el periodo
        /// Modifica : Lista de proyectos 
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(ddlPeriodo.SelectedValue);
            LinkedList<Proyectos> proyectos = proyectoServicios.ObtenerPorPeriodo(id);
            ddlProyecto.DataSource = proyectos;
            ddlProyecto.DataBind();
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
        }

        #endregion
    }
}