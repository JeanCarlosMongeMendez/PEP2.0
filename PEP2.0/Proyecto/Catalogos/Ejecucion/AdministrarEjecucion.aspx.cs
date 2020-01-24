using Entidades;
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


    public partial class AdministrarEjecucion : System.Web.UI.Page
    {
        readonly PagedDataSource pgsource = new PagedDataSource();
        PeriodoServicios periodoServicios;
        ProyectoServicios proyectoServicios;
        TipoTramiteServicios tipoTramiteServicios;
        UnidadServicios unidadServicios;
        PartidaServicios partidaServicios;
        PresupuestoEgreso_PartidaServicios presupuestoEgreso_PartidaServicios;
       PresupuestoEgresosServicios presupuestoEgresosServicio;
        PartidaUnidad partidaUnidad;
        int idUnidadElegida;
        private int elmentosMostrar = 10;
        //se utiliza en el metodo  MostrarDatosTablaUnidad();se utiliza para pasar unidades seleccionadas de la tabla que aparece en el  #modalElegirUnidad
        static List<Unidad> listaUnidad = new List<Unidad>();
        //Esta llena la tabla en el metodo mostrarDatosTabla(),la uso como temporal de la linkedlist
        static List<Unidad> listUnidad = new List<Unidad>();
        //se utiliza tambien en el metodo mostrarDatosTabla(),para llenar la tabla en el inicio 
        static LinkedList<Unidad> listUnidades = new LinkedList<Unidad>();

        int primerIndex, ultimoIndex, primerIndex2, ultimoIndex2, primerIndex3, ultimoIndex3, primerIndex4, ultimoIndex4;
        //contador que se utiliza en el metodo MostrarDatosTabla(),se utiliza para que recorra solo una ves en listUnidades
        static int count = 0;
        static int contador = 0;
        static double monto=0;
        static int contadorBotonRepartir=0;

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

        private int paginaActual4
        {
            get
            {
                if (ViewState["paginaActual4"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["paginaActual4"]);
            }
            set
            {
                ViewState["paginaActual4"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            this.presupuestoEgresosServicio = new PresupuestoEgresosServicios();
            this.periodoServicios = new PeriodoServicios();
            this.proyectoServicios = new ProyectoServicios();
            this.unidadServicios = new UnidadServicios();
            this.partidaServicios = new PartidaServicios();
            this.tipoTramiteServicios = new TipoTramiteServicios();
            this.presupuestoEgreso_PartidaServicios = new PresupuestoEgreso_PartidaServicios();
            this.partidaUnidad = new PartidaUnidad();
            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                Session["partidasPorUnidadesProyectoPeriodo"] = null;
                Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
                Session["partidasAsignadasConMonto"] = null;
                PeriodosDDL.Items.Clear();
                ProyectosDDL.Items.Clear();
                DDLTipoTramite.Items.Clear();
                ddlPartida.Items.Clear();
                CargarPeriodos();
                descripcionOtroTipoTramite.Visible = false;
                UpdatePanel10.Visible = false;
                ButtonRepartir.Visible = false;
            }
            else
            {
                obtenerPartidasSeleccionadas();
            }



        }


        /// <summary>
        /// Josseline M
        /// Este método se encarga de activar el modal del total de partidas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonAsociarPartidas_Click(object sender, EventArgs e)
        {
         
            obtenerPartidasPorProyectoUnidadPeriodo();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalElegirPartidas", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalElegirPartidas').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalElegirPartidas();", true);

        }

        /// <summary>
        /// Josseline M 
        /// este método muestra las partidas selecionadas en el modal
        /// </summary>
        private void obtenerPartidasSeleccionadas()
        {
            List<Partida> partidas = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
            if (partidas != null)
            {

                List<Partida> partidasF = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
                var dt3 = partidasF;
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

                rpPartidasSeleccionada.DataSource = pgsource;
                rpPartidasSeleccionada.DataBind();

                //metodo que realiza la paginacion
                Paginacion3();

            }
            else
            {
                List<Partida> partidasF = new List<Partida>();
                rpPartidasSeleccionada.DataSource = partidasF;
                rpPartidasSeleccionada.DataBind();
            }
        }


        /// <summary>
        /// Josseline M
        /// Este método se encarga de actualizar las partidas a partir del periodo, proyecto y unidades elegidas
        /// </summary>
        private void obtenerPartidasPorProyectoUnidadPeriodo()
        {
            List<Partida> partidas = (List<Partida>)Session["partidasPorUnidadesProyectoPeriodo"];
            if (partidas == null || partidas.Count==0)
            {
                int proyectoElegido = Int32.Parse(ProyectosDDL.SelectedValue);
                int periodoElegido = Int32.Parse(PeriodosDDL.SelectedValue);
                LinkedList<int> unidades = new LinkedList<int>();
                foreach (Unidad unidad in listaUnidad)
                {
                    unidades.AddFirst(unidad.idUnidad);
                }

                if (proyectoElegido != 0 && periodoElegido != 0)
                {
                    Session["partidasPorUnidadesProyectoPeriodo"] = partidaServicios.ObtienePartidaPorPeriodoUnidadProyecto(proyectoElegido, unidades, periodoElegido);

                    List<Partida> partidasF = (List<Partida>)Session["partidasPorUnidadesProyectoPeriodo"];
                    var dt2 = partidasF;
                    pgsource.DataSource = dt2;
                    pgsource.AllowPaging = true;
                    //numero de items que se muestran en el Repeater
                    pgsource.PageSize = elmentosMostrar;
                    pgsource.CurrentPageIndex = paginaActual4;
                    //mantiene el total de paginas en View State
                    ViewState["TotalPaginas4"] = pgsource.PageCount;
                    //Ejemplo: "Página 1 al 10"
                    lblpagina4.Text = "Página " + (paginaActual4 + 1) + " de " + pgsource.PageCount + " (" + dt2.Count + " - elementos)";
                    //Habilitar los botones primero, último, anterior y siguiente
                    lbAnterior4.Enabled = !pgsource.IsFirstPage;
                    lbSiguiente4.Enabled = !pgsource.IsLastPage;
                    lbPrimero4.Enabled = !pgsource.IsFirstPage;
                    lbUltimo4.Enabled = !pgsource.IsLastPage;

                    rpElegirPartida.DataSource = pgsource;
                    rpElegirPartida.DataBind();

                    //metodo que realiza la paginacion
                    Paginacion4();

                }
                else
                {
                    partidas = null;
                    var dt = partidas;
                    pgsource.DataSource = dt;
                    rpElegirPartida.DataSource = pgsource;
                    rpElegirPartida.DataBind();
                }

            }
            else
            {
                List<Partida> partidasN = (List<Partida>)Session["partidasPorUnidadesProyectoPeriodo"];
                var dt5 = partidasN;
                pgsource.DataSource = dt5;
                pgsource.AllowPaging = true;
                //numero de items que se muestran en el Repeater
                pgsource.PageSize = elmentosMostrar;
                pgsource.CurrentPageIndex = paginaActual4;
                //mantiene el total de paginas en View State
                ViewState["TotalPaginas4"] = pgsource.PageCount;
                //Ejemplo: "Página 1 al 10"
                lblpagina4.Text = "Página " + (paginaActual4 + 1) + " de " + pgsource.PageCount + " (" + dt5.Count + " - elementos)";
                //Habilitar los botones primero, último, anterior y siguiente
                lbAnterior4.Enabled = !pgsource.IsFirstPage;
                lbSiguiente4.Enabled = !pgsource.IsLastPage;
                lbPrimero4.Enabled = !pgsource.IsFirstPage;
                lbUltimo4.Enabled = !pgsource.IsLastPage;

                rpElegirPartida.DataSource = pgsource;
                rpElegirPartida.DataBind();

                //metodo que realiza la paginacion
                Paginacion4();
                MostrarTablaRepartirGastos();



            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalElegirPartidas", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalElegirPartidas').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalElegirPartidas();", true);

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

        private void Paginacion1()
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

            rptPaginacion1.DataSource = dt;
            rptPaginacion1.DataBind();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 27/sep/2019
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
                primerIndex3 = ultimoIndex2 - 4;
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

        private void Paginacion4()
        {
            var dt = new DataTable();
            dt.Columns.Add("IndexPagina"); //Inicia en 0
            dt.Columns.Add("PaginaText"); //Inicia en 1

            primerIndex4 = paginaActual4 - 2;
            if (paginaActual4 > 2)
                ultimoIndex4 = paginaActual4 + 2;
            else
                ultimoIndex4 = 4;

            //se revisa que la ultima pagina sea menor que el total de paginas a mostrar, sino se resta para que muestre bien la paginacion
            if (ultimoIndex2 > Convert.ToInt32(ViewState["TotalPaginas4"]))
            {
                ultimoIndex4 = Convert.ToInt32(ViewState["TotalPaginas4"]);
                primerIndex4 = ultimoIndex2 - 4;
            }

            if (primerIndex4 < 0)
                primerIndex4 = 0;

            //se crea el numero de paginas basado en la primera y ultima pagina
            for (var i = primerIndex4; i < ultimoIndex4; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaginacion4.DataSource = dt;
            rptPaginacion4.DataBind();
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
        protected void lbPrimero2_Click(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            MostrarDatosTablaUnidad(listaUnidad);
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
        protected void lbUltimo2_Click(object sender, EventArgs e)
        {
            paginaActual2 = (Convert.ToInt32(ViewState["TotalPaginas"]) - 1);
            MostrarDatosTablaUnidad(listaUnidad);
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
        protected void lbAnterior2_Click(object sender, EventArgs e)
        {
            paginaActual2 -= 1;
            MostrarDatosTablaUnidad(listaUnidad);
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
        protected void lbSiguiente2_Click(object sender, EventArgs e)
        {
            paginaActual2 += 1;
            MostrarDatosTablaUnidad(listaUnidad);
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
            MostrarDatosTablaUnidad(listaUnidad);
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
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrimero1_Click(object sender, EventArgs e)
        {
            paginaActual = 0;
            //MostrarDatosTablaUnidad(listaUnidad);
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
        protected void lbUltimo1_Click(object sender, EventArgs e)
        {
            paginaActual = (Convert.ToInt32(ViewState["TotalPaginas"]) - 1);
            // MostrarDatosTablaUnidad(listaUnidad);
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
        protected void lbAnterior1_Click(object sender, EventArgs e)
        {
            paginaActual -= 1;
            // MostrarDatosTablaUnidad(listaUnidad);
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
        protected void lbSiguiente1_Click(object sender, EventArgs e)
        {
            paginaActual += 1;
            //  MostrarDatosTablaUnidad(listaUnidad);
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
        protected void rptPaginacion1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual = Convert.ToInt32(e.CommandArgument.ToString());

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
        protected void rptPaginacion1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion1");
            if (lnkPagina.CommandArgument != paginaActual.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }


        /// <summary>
        /// Kevin Picado 
        /// 09/oct/2019
        /// Efecto: cambia los datos del proyrcto segun el periodo seleccionado
        /// Requiere: seleccionar un periodo
        /// Modifica: DropDownList de proyectos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CargarPeriodos()
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            PeriodosDDL.Items.Clear();
            periodos = this.periodoServicios.ObtenerTodos();
            int anoHabilitado = 0;
            Session["partidasPorUnidadesProyectoPeriodo"] = null;
            Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
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

        /// <summary>
        /// Kevin Picado
        /// 09//2019
        /// Efecto: cambia los datos de las unidades segun el periodo seleccionado
        /// Requiere: cambiar periodo
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                    //CargarUnidades();
                }
            }

        }


        protected void lbPrimero3_Click(object sender, EventArgs e)
        {
            paginaActual3 = 0;
            obtenerPartidasSeleccionadas();
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
        protected void lbUltimo3_Click(object sender, EventArgs e)
        {
            paginaActual3 = (Convert.ToInt32(ViewState["TotalPaginas3"]) - 1);
            obtenerPartidasSeleccionadas();
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
        protected void lbAnterior3_Click(object sender, EventArgs e)
        {
            paginaActual3 -= 1;
            obtenerPartidasSeleccionadas();
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
        protected void lbSiguiente3_Click(object sender, EventArgs e)
        {
            paginaActual3 += 1;
            obtenerPartidasSeleccionadas();
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
        protected void rptPaginacion3_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual3 = Convert.ToInt32(e.CommandArgument.ToString());
            obtenerPartidasSeleccionadas();
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
        protected void rptPaginacion3_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion3");
            if (lnkPagina.CommandArgument != paginaActual.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }


        /// <summary>
        /// Kevin Picado
        /// 09/oct/2019
        /// Efecto: Muestra todas las unidades dependiendo del proyecto seleccionado
        /// Requiere: Seleccionar el periodo y el proyecto
        /// Modifica: tabla Unidades
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MostrarDatosTabla()
        {

            if ((ProyectosDDL.Items.Count > 0) && (count == 0))
            {

                listUnidades = unidadServicios.ObtenerPorProyecto(Int32.Parse(ProyectosDDL.SelectedValue));
                listUnidad = listUnidades.ToList<Unidad>();
                var dt = listUnidad;
                pgsource.DataSource = dt;
                pgsource.AllowPaging = true;
                //numero de items que se muestran en el Repeater
                pgsource.PageSize = elmentosMostrar;
                pgsource.CurrentPageIndex = paginaActual;
                //mantiene el total de paginas en View State
                ViewState["TotalPaginas"] = pgsource.PageCount;
                //Ejemplo: "Página 1 al 10"
                lblpagina1.Text = "Página " + (paginaActual + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
                //Habilitar los botones primero, último, anterior y siguiente
                lbAnterior1.Enabled = !pgsource.IsFirstPage;
                lbSiguiente1.Enabled = !pgsource.IsLastPage;
                lbPrimero1.Enabled = !pgsource.IsFirstPage;
                lbUltimo1.Enabled = !pgsource.IsLastPage;

                Repeater1.DataSource = pgsource;
                Repeater1.DataBind();

                //metodo que realiza la paginacion
                Paginacion1();
                count++;
            }
            else
            {


                var dt = listUnidad;
                pgsource.DataSource = dt;
                pgsource.AllowPaging = true;
                //numero de items que se muestran en el Repeater
                pgsource.PageSize = elmentosMostrar;
                pgsource.CurrentPageIndex = paginaActual;
                //mantiene el total de paginas en View State
                ViewState["TotalPaginas"] = pgsource.PageCount;
                //Ejemplo: "Página 1 al 10"
                lblpagina1.Text = "Página " + (paginaActual + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
                //Habilitar los botones primero, último, anterior y siguiente
                lbAnterior1.Enabled = !pgsource.IsFirstPage;
                lbSiguiente1.Enabled = !pgsource.IsLastPage;
                lbPrimero1.Enabled = !pgsource.IsFirstPage;
                lbUltimo1.Enabled = !pgsource.IsLastPage;

                Repeater1.DataSource = pgsource;
                Repeater1.DataBind();

                //metodo que realiza la paginacion
                Paginacion1();
            }
        }

        /// <summary>
        /// Josseline M
        /// Método utilizado para la accion en la selección del droplist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Periodos_OnChanged(object sender, EventArgs e)
        {
            Session["partidasPorUnidadesProyectoPeriodo"] = null;
            Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
            CargarProyectos();
            reiniciarTablaUnidad();
            DDLTipoTramite.Items.Clear();
            descripcionOtroTipoTramite.Visible = false;

        }


        protected void Proyectos_OnChanged(object sender, EventArgs e)
        {

            Session["partidasPorUnidadesProyectoPeriodo"] = null;
            Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
            reiniciarTablaUnidad();

            CargarTramites();
            DDLTipoTramite.Items.Clear();

            descripcionOtroTipoTramite.Visible = false;
        }

        private bool IsNumeric(string num)
        {
            try
            {
                double x = Convert.ToDouble(num);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    

    protected void ButtonRepartirPartidas_Click(object sender, EventArgs e)
        {
            CargarPartidasPorUnidades();
            if (IsNumeric(txtMontoIngresar.Text))
            {
                if (Convert.ToInt32((txtMontoIngresar.Text)) >= 0)
                {
                    if (contadorBotonRepartir == 0)
                    {

                        montoRepartir.Text = txtMontoIngresar.Text;
                        obtenerUnidadesPartidasAsignarMonto();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalRepartirPartidas", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalRepartirPartidas').hide();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalRepartirPartidas();", true);
                        contadorBotonRepartir++;
                    }
                    else
                    {
                        List<PartidaUnidad> partidasAsignadasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];
                        //Double sumaMontoTotalRepartir = (Double)partidasAsignadasConMonto.Sum(PartidaUnidad => PartidaUnidad.Monto);
                        //if (Convert.ToDouble(txtMontoIngresar.Text) >= sumaMontoTotalRepartir || monto == 0)
                        //{
                        if (partidasAsignadasConMonto != null)
                        {
                            Double montoDisponible = (Double)partidasAsignadasConMonto.Sum(monto => monto.Monto);


                            montoRepartir.Text = Convert.ToString(Convert.ToDouble(txtMontoIngresar.Text) - Convert.ToDouble(montoDisponible));
                        }
                        obtenerUnidadesPartidasAsignarMonto();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalRepartirPartidas", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalRepartirPartidas').hide();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalRepartirPartidas();", true);
                        contadorBotonRepartir++;
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El monto asignado es insuficiente" + "');", true);
                        //}
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El texto ingresado deben ser montos Positivos" + "');", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El texto ingresado deben ser números" + "');", true);
            }
        }
        protected void recargarTablaRepartirMontos(List<PartidaUnidad> partidaUnidad)
        {
           
                var dt = partidaUnidad;
                pgsource.DataSource = dt;
                pgsource.AllowPaging = true;
                //numero de items que se muestran en el Repeater
                //pgsource.PageSize = elmentosMostrar;
                //pgsource.CurrentPageIndex = paginaActual;
                //mantiene el total de paginas en View State
                //ViewState["TotalPaginas"] = pgsource.PageCount;
                //Ejemplo: "Página 1 al 10"
                //  lblpagina1.Text = "Página " + (paginaActual + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
                //Habilitar los botones primero, último, anterior y siguiente
                //lbAnterior1.Enabled = !pgsource.IsFirstPage;
                //lbSiguiente1.Enabled = !pgsource.IsLastPage;
                //lbPrimero1.Enabled = !pgsource.IsFirstPage;
                //lbUltimo1.Enabled = !pgsource.IsLastPage;

                rpUnidadPartida.DataSource = pgsource;
                rpUnidadPartida.DataBind();

            
        }
        protected void EliminarUnidadSeleccionada_OnChanged(object sender, EventArgs e)
        {
            int idUnidad = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            listaUnidad.RemoveAll(item => item.idUnidad == idUnidad);
            MostrarDatosTablaUnidad(listaUnidad);
            Unidad añadirUnidadEliminada = unidadServicios.ObtenerPorId(idUnidad);
            listUnidad.Add(añadirUnidadEliminada);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "La unidad fue borrada con exito" + "');", true);
            List<Partida> partidas = (List<Partida>)Session["partidasPorUnidadesProyectoPeriodo"];
            if (partidas != null)
            {
                partidas.RemoveAll(item => item.idUnidad == idUnidad);
            }
            List<Partida> partidasElegidas = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
            if (partidasElegidas!=null)
            {
                partidasElegidas.RemoveAll(item => item.idUnidad == idUnidad);
                obtenerPartidasSeleccionadas();
                //obtenerPartidasPorProyectoUnidadPeriodo();
            }
            List<PartidaUnidad> partidasElegidasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];
            if (partidasElegidasConMonto.Exists(element => element.IdUnidad.Equals(idUnidad)))
            {
                double montoEliminado = partidasElegidasConMonto.Where(item => item.IdUnidad == idUnidad).ToList().First().Monto;
                monto = monto + montoEliminado;
                partidasElegidasConMonto.RemoveAll(item => item.IdUnidad == idUnidad);
                MostrarUnidadesConMontoRepartido();
            }

        }
        private void CargarPartidasPorUnidades()
        {

            ddlPartida.Items.Clear();
           
            if (listaUnidad.Count > 0)
            {
                foreach (Unidad unidad in listaUnidad)
                {
                    
                    ListItem itemUnidad = new ListItem(unidad.nombreUnidad,unidad.idUnidad.ToString());
                    ddlPartida.Items.Add(itemUnidad);
                }

               
            }
          
        }
        /// <summary>
        /// Josseline M
        /// Añade el idUnidad, idPartida y el monto asignado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAlmacenarUnidadPartida_Click(object sender, EventArgs e)
        {

            List<PartidaUnidad> partidasAsignadas = (List<PartidaUnidad>)Session["partidasAsignadas"];

            List<PartidaUnidad> partidasElegidasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];

            if (partidasAsignadas == null)
            {
                partidasAsignadas = new List<PartidaUnidad>();
                montoRepartir.Text = txtMontoIngresar.Text;

            }

            else
            {
                if (partidasElegidasConMonto == null)
                {
                    partidasElegidasConMonto = new List<PartidaUnidad>();
                    monto = Convert.ToDouble(txtMontoIngresar.Text);
                }
                string idPartida = ((LinkButton)(sender)).CommandArgument.ToString();
                List<PresupuestoEgresoPartida> listaPartidasEgreso = new List<PresupuestoEgresoPartida>();
                listaPartidasEgreso = presupuestoEgreso_PartidaServicios.obtenerEgreso_Partida_porIdPartida(idPartida);
                // monto = Convert.ToDouble(txtMontoIngresar.Text);
                //double montoRepartir = Convert.ToDouble(UpdatePane34.TextBox1.Text);
                List<PartidaUnidad> partidasElegidas = (List<PartidaUnidad>)Session["partidasAsignadas"];
                List<PartidaUnidad> partidasElegidasTemporal = new  List<PartidaUnidad>() ;
                partidasElegidasTemporal = partidasElegidas;
                PartidaUnidad partidaUnidad = new PartidaUnidad();
                //Double montoDisponible = (Double)listaPartidasEgreso.Sum(presupuesto => presupuesto.monto);
                Double sumaMontoTotalRepartir = (Double)partidasElegidasConMonto.Sum(PartidaUnidad => PartidaUnidad.Monto);
                if (Convert.ToDouble(txtMontoIngresar.Text) >= sumaMontoTotalRepartir || monto == 0)
                {

                    foreach (PartidaUnidad p in partidasElegidas.ToList())
                    {
                        Double montoDisponible= partidasElegidas.Where(item => p.IdUnidad == item.IdUnidad && item.NumeroPartida==p.NumeroPartida).ToList().First().MontoDisponible;
                        if (monto <=montoDisponible && monto > 0)
                        {
                            if (p.IdPartida == Convert.ToInt32(idPartida))
                            {
                                partidaUnidad.IdPartida = p.IdPartida;
                                partidaUnidad.IdUnidad = p.IdUnidad;
                                partidaUnidad.NumeroPartida = p.NumeroPartida;

                                // partidasElegidas.RemoveAll(item => item.IdPartida == p.IdPartida);

                                foreach (RepeaterItem item in rpUnidadPartida.Items)
                                {
                                    HiddenField hdIdPartida = (HiddenField)item.FindControl("hdIdPartida");
                                    int idPartid = Convert.ToInt32(hdIdPartida.Value);
                                    TextBox txtMonto = (TextBox)item.FindControl("TextBox1");
                                    String txtMont = txtMonto.Text.Replace(".", ",");
                                    if (Double.TryParse(txtMont, out Double montoo) && idPartid == Convert.ToInt32(idPartida))
                                    {
                                        montoRepartir.Text = (Convert.ToString(Convert.ToDouble(montoRepartir.Text) - Convert.ToDouble(txtMont)));
                                        monto = Convert.ToDouble(montoRepartir.Text) + Convert.ToDouble(txtMont);

                                        //txtMonto.Text = monto.ToString();

                                        if (monto >= 0 && Convert.ToDouble(txtMontoIngresar.Text) >= Convert.ToDouble(txtMont))
                                        {

                                            monto = Convert.ToDouble(montoRepartir.Text);
                                            double saldo = p.MontoDisponible - Convert.ToDouble(txtMont);
                                            partidaUnidad.MontoDisponible = saldo;
                                            partidaUnidad.Monto = Convert.ToDouble(txtMont);
                                            partidasElegidasConMonto.Add(partidaUnidad);
                                        }
                                        else
                                        {
                                            montoRepartir.Text = (Convert.ToString(Convert.ToDouble(montoRepartir.Text) + Convert.ToDouble(txtMont)));
                                            monto = Convert.ToDouble(montoRepartir.Text);
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El monto a repartir es insuficiente" + "');", true);
                                        }
                                    }



                                }


                                // partidasElegidas.Add(partidaUnidad);
                            }




                            Session["partidasAsignadasConMonto"] = partidasElegidasConMonto;
                            MostrarUnidadesConMontoRepartido();
                            partidasElegidas.RemoveAll(item => item.IdPartida == Convert.ToInt32(idPartida) && item.IdUnidad == idUnidadElegida);
                            Session["partidasAsignadas"] = partidasElegidas;
                            obtenerUnidadesPartidasAsignarMonto();


                        }else if(monto!=0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "La Partida no cuenta con el dinero suficiente" + "');", true);
                        }

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalRepartirPartidas", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalRepartirPartidas').hide();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", " activarModalRepartirPartidas();", true);
                    }
                }

            }
        }
        protected void EliminarMontoRepartido_OnChanged(object sender, EventArgs e)
        {
            string [] unidadNumeroPartida= (((LinkButton)(sender)).CommandArgument.ToString().Split(new string[] { "::" }, StringSplitOptions.None));
            string numeroPartida = unidadNumeroPartida[0];
            string idUnidad = unidadNumeroPartida[1];
            List<PartidaUnidad> partidasAsignadas = (List<PartidaUnidad>)Session["partidasAsignadas"];
            List<PartidaUnidad> partidasElegidasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];
           
            partidasAsignadas.Add((PartidaUnidad)partidasElegidasConMonto.Where(item => item.NumeroPartida == numeroPartida && item.IdUnidad== Convert.ToInt32(idUnidad)).ToList().First());
            double montoEliminado = partidasElegidasConMonto.Where(item => item.NumeroPartida == numeroPartida && item.IdUnidad == Convert.ToInt32(idUnidad)).ToList().First().Monto;
            monto = monto + montoEliminado;
            partidasElegidasConMonto.RemoveAll(item => item.NumeroPartida == numeroPartida && item.IdUnidad == Convert.ToInt32(idUnidad));
            MostrarUnidadesConMontoRepartido();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "La unidad fue borrada con exito" + "');", true);
        }
        private void MostrarUnidadesConMontoRepartido()
        {
            List<PartidaUnidad> partidasAsignadasConMonto = (List<PartidaUnidad>) Session["partidasAsignadasConMonto"];
           var dt = partidasAsignadasConMonto;
            pgsource.DataSource = dt;
            pgsource.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            //pgsource.PageSize = elmentosMostrar;
            //pgsource.CurrentPageIndex = paginaActual2;
            //mantiene el total de paginas en View State
            //ViewState["TotalPaginas2"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            //lblpagina2.Text = "Página " + (paginaActual2 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            //lbAnterior2.Enabled = !pgsource.IsFirstPage;
            //lbSiguiente2.Enabled = !pgsource.IsLastPage;
            //lbPrimero2.Enabled = !pgsource.IsFirstPage;
            //lbUltimo2.Enabled = !pgsource.IsLastPage;

            Repeater5.DataSource = pgsource;
            Repeater5.DataBind();

            //metodo que realiza la paginacion
            //Paginacion2();

         
        }

        private void registroPartidasAsignadas()
        {

        }

        /// <summary>
        /// Josseline M
        /// Este método se encarga de permitir eliminar una partida añadida desde el modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EliminarPartidaSeleccionada_OnChanged(object sender, EventArgs e)
        {
            String numeroPartida = ((LinkButton)(sender)).CommandArgument.ToString();
            Partida partida = partidaServicios.ObtenerPorNumeroPartida(numeroPartida);
            List<Partida> partidasElegidas = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
            if (partidasElegidas == null)
            {
                partidasElegidas = new List<Partida>();
            }

            List<Partida> partidasFiltradas = (List<Partida>)Session["partidasPorUnidadesProyectoPeriodo"];

            bool exists = partidasElegidas.Exists(element => element.numeroPartida.Equals(numeroPartida));
            if (exists == true)
            {
              
                partidasElegidas.RemoveAll(item => item.numeroPartida.Equals(numeroPartida));

                Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = partidasElegidas;
                partidasFiltradas.Add(partida);
                Session["partidasPorUnidadesProyectoPeriodo"] = partidasFiltradas;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "La partida fue borrada con exito" + "');", true);
                List<PartidaUnidad> partidasElegidasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];
                if (partidasElegidasConMonto != null)
                {
                    if (partidasElegidasConMonto.Exists(element => element.NumeroPartida.Equals(numeroPartida)))
                    {
                        double montoEliminado = partidasElegidasConMonto.Where(item => item.NumeroPartida == numeroPartida).ToList().First().Monto;
                        monto = monto + montoEliminado;
                        partidasElegidasConMonto.RemoveAll(item => item.NumeroPartida == numeroPartida);
                        MostrarUnidadesConMontoRepartido();
                    }
                }
            }
            obtenerPartidasSeleccionadas();
        }

        protected void ButtonAsociar_Click(object sender, EventArgs e)
        {
            CargarTramites();
            MostrarDatosTabla();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalElegirUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalElegirUnidad').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalElegirUnidad();", true);

        }
        protected void ddlUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            idUnidadElegida = Int32.Parse(ddlPartida.SelectedValue);
            obtenerUnidadesPartidasAsignarMonto();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalRepartirPartidas", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalRepartirPartidas').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalRepartirPartidas();", true);
           
        }

        /// <summary>
        /// Josseline M
        /// Método encargado de permitir la distribución
        /// </summary>
        private void obtenerUnidadesPartidasAsignarMonto()
        {
           
            int proyectoElegido = Int32.Parse(ProyectosDDL.SelectedValue);
            int periodoElegido = Int32.Parse(PeriodosDDL.SelectedValue);
            idUnidadElegida = Int32.Parse(ddlPartida.SelectedValue);
            if (idUnidadElegida != 0){
                List<Partida> partidasElegidas = new List<Partida>();
                List<Partida> partidaTemp = new List<Partida>();
                partidasElegidas = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
                List<PartidaUnidad> partidaUnidad = new List<PartidaUnidad>();
                List<PresupuestoEgreso> listaPartidasEgreso = new List<PresupuestoEgreso>();
                List<PresupuestoEgresoPartida> listaPresuouestosEgreso = new List<PresupuestoEgresoPartida>();
                foreach (Partida p in partidasElegidas)
                {

                    Partida partidaTemporal = new Partida();
                    partidaTemporal = partidaServicios.ObtienePartidaPorPeriodoUnidadProyectoYNumeroUnidad(proyectoElegido, idUnidadElegida, periodoElegido, p.numeroPartida);
                    partidaTemp.Add(partidaTemporal);
                }

                foreach (Partida p in partidaTemp)
                {
                    PartidaUnidad partidaU = new PartidaUnidad();
                    partidaU.IdPartida = p.idPartida;
                    partidaU.IdUnidad = p.idUnidad;
                    partidaU.NumeroPartida = p.numeroPartida;
                    Unidad unidad = new Unidad();
                    unidad.idUnidad = p.idUnidad;


                    listaPartidasEgreso = presupuestoEgresosServicio.getPresupuestosEgresosPorUnidad(unidad);
                    PresupuestoEgreso presupuestoEgreso = new PresupuestoEgreso();
                    presupuestoEgreso.idPresupuestoEgreso= listaPartidasEgreso.Where(item => item.unidad.idUnidad == p.idUnidad).ToList().First().idPresupuestoEgreso; 
                    listaPresuouestosEgreso = presupuestoEgreso_PartidaServicios.getPresupuestoEgresoPartidas(presupuestoEgreso);
                    
                    Double montoDisponible = (Double)listaPresuouestosEgreso.Where(item => item.partida.numeroPartida == p.numeroPartida).ToList().FirstOrDefault().monto; 
                     
                    partidaU.MontoDisponible = montoDisponible;
                    partidaUnidad.Add(partidaU);

                }
                Session["partidasAsignadas"] = partidaUnidad;
                List<PartidaUnidad> partidasElegidasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];
                if (partidasElegidasConMonto != null)
                {

                    List<PartidaUnidad> TempPartida = partidaUnidad.Where(a => !partidasElegidasConMonto.Any(a1 => a1.NumeroPartida == a.NumeroPartida && a1.IdUnidad == a.IdUnidad))
                    .Union(partidasElegidasConMonto.Where(a => !partidaUnidad.Any(a1 => a1.NumeroPartida == a.NumeroPartida && a1.IdUnidad==a.IdUnidad ))).ToList();
                   
                    Session["partidasAsignadas"] = TempPartida.Where(item => item.IdUnidad == idUnidadElegida).ToList();
                   
                }
                partidaUnidad = (List<PartidaUnidad>)Session["partidasAsignadas"];

                var dt = partidaUnidad;
                pgsource.DataSource = dt;
                pgsource.AllowPaging = true;
                //numero de items que se muestran en el Repeater
                //pgsource.PageSize = elmentosMostrar;
                //pgsource.CurrentPageIndex = paginaActual;
                //mantiene el total de paginas en View State
                //ViewState["TotalPaginas"] = pgsource.PageCount;
                //Ejemplo: "Página 1 al 10"
                //  lblpagina1.Text = "Página " + (paginaActual + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
                //Habilitar los botones primero, último, anterior y siguiente
                //lbAnterior1.Enabled = !pgsource.IsFirstPage;
                //lbSiguiente1.Enabled = !pgsource.IsLastPage;
                //lbPrimero1.Enabled = !pgsource.IsFirstPage;
                //lbUltimo1.Enabled = !pgsource.IsLastPage;

                rpUnidadPartida.DataSource = pgsource;
                rpUnidadPartida.DataBind();
                contador++;
                //}
                //else
                //{
                //    partidaUnidad= (List<PartidaUnidad>)Session["partidasAsignadas"];
                //    var dt = partidaUnidad;
                //    pgsource.DataSource = dt;
                //    pgsource.AllowPaging = true;
                //    //numero de items que se muestran en el Repeater
                //    //pgsource.PageSize = elmentosMostrar;
                //    //pgsource.CurrentPageIndex = paginaActual;
                //    //mantiene el total de paginas en View State
                //    //ViewState["TotalPaginas"] = pgsource.PageCount;
                //    //Ejemplo: "Página 1 al 10"
                //    //  lblpagina1.Text = "Página " + (paginaActual + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
                //    //Habilitar los botones primero, último, anterior y siguiente
                //    //lbAnterior1.Enabled = !pgsource.IsFirstPage;
                //    //lbSiguiente1.Enabled = !pgsource.IsLastPage;
                //    //lbPrimero1.Enabled = !pgsource.IsFirstPage;
                //    //lbUltimo1.Enabled = !pgsource.IsLastPage;

                //    rpUnidadPartida.DataSource = pgsource;
                //    rpUnidadPartida.DataBind();
                //}

            }
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalElegirUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalRepartirPartidas').hide();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalRepartirPartidas();", true);

        }


    

        /// <summary>
        /// Kevin Picado
        /// 09/oct/2019
        /// Efecto: Guarda en una lista las unidades seleccionadas
        /// Requiere: Seleccionar la unidad para cargarla en la lista
        /// Modifica: tabla Unidades
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUnidadSeleccionada_Click(object sender, EventArgs e)
        {
            int idUnidad = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            Unidad unidades = unidadServicios.ObtenerPorId(idUnidad);

            bool exists = listaUnidad.Exists(element => element.idUnidad == idUnidad);
            if (exists != true)
            {
                listaUnidad.Add(unidades);
                MostrarDatosTablaUnidad(listaUnidad);
                listUnidad.RemoveAll(item => item.idUnidad == idUnidad);


                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "La unidad fue seleccionada con exito" + "');", true);
            }

            MostrarDatosTabla();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalElegirUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalElegirUnidad').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalElegirUnidad();", true);
        }

        /// <summary>
        /// Josseline M
        /// Método encargado de cargar lo trámites apartir del tipo de proyecto si es UCR o fundevi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TipoTramites_OnChanged(object sender, EventArgs e)
        {
            anadirDescripcion();
        }
        /// <summary>
        /// Josseline M
        /// Muestra un nuevo espacio para añadir una nueva descripcion de tipo de tramite 
        /// </summary>
        private void anadirDescripcion()
        {
            String valor = DDLTipoTramite.SelectedValue;
            if (valor.Equals("9") || valor.Equals("11"))
            {
                descripcionOtroTipoTramite.Visible = true;
            }
        }

        /// <summary>
        /// Josseline M
        /// Método encargado de cargar lo trámites apartir del tipo de proyecto si es UCR o fundevi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CargarTramites()
        {
            int idProyecto = Int32.Parse(ProyectosDDL.SelectedValue);
            Proyectos proyecto = new Proyectos();
            proyecto = this.proyectoServicios.ObtenerPorId(idProyecto);

            DDLTipoTramite.Items.Clear();

            if (DDLTipoTramite.SelectedValue.Equals(""))
            {
                List<TipoTramite> tramites = new List<TipoTramite>();
                tramites = this.tipoTramiteServicios.obtenerTipoTramitesPorProyecto(proyecto);

                if (tramites.Count > 0)
                {
                    foreach (TipoTramite tramite in tramites)
                    {
                        ListItem itemLB = new ListItem(tramite.nombreTramite, tramite.idTramite.ToString());
                        Session["tipoTramite"] = tramite.idTramite;
                        DDLTipoTramite.Items.Add(itemLB);
                    }


                }
            }

        }

        /// <summary>
        /// Josseline M
        /// Efecto: Guarda en una lista las partidas seleccionadas
        /// Requiere: Seleccionar una partida de la lista filtrada para cargarla en la lista
        /// Modifica: tabla partidas por unidad
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAnadirNuevaPartida_Click(object sender, EventArgs e)
        {
            string nombrePartida = (((LinkButton)(sender)).CommandArgument).ToString();
            Partida partida = partidaServicios.ObtenerPorNumeroPartida(nombrePartida);


            List<Partida> partidasElegidas = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
            if (partidasElegidas == null)
            {
                partidasElegidas = new List<Partida>();
            }


            List<Partida> partidasFiltradas = (List<Partida>)Session["partidasPorUnidadesProyectoPeriodo"];

            bool exists = partidasFiltradas.Exists(element => element.numeroPartida.Equals(nombrePartida));
            foreach (Partida p in partidasFiltradas)
            {
                if (p.numeroPartida.Equals(nombrePartida))
                {
                    partida.idUnidad = p.idUnidad;
                }
            }
            if (exists == true)
            {
                partidasElegidas.Add(partida);
                partidasFiltradas.RemoveAll(item => item.numeroPartida.Equals(nombrePartida));
                Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = partidasElegidas;
                Session["partidasPorUnidadesProyectoPeriodo"] = partidasFiltradas;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "La partida fue seleccionada con exito" + "');", true);
            }
            obtenerPartidasPorProyectoUnidadPeriodo();
            obtenerPartidasSeleccionadas();


        }


        /// <summary>
        /// Kevin Picado
        /// 09/oct/2019
        /// Efecto: Muestra las unidades seleccionadas 
        /// Requiere: Seleccionar la unidad para cargarla en la lista
        /// Modifica: tabla Unidades
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MostrarDatosTablaUnidad(List<Unidad> listaUnidad)
        {

            var dt = listaUnidad;
            pgsource.DataSource = dt;
            pgsource.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual2;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas2"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina2.Text = "Página " + (paginaActual2 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior2.Enabled = !pgsource.IsFirstPage;
            lbSiguiente2.Enabled = !pgsource.IsLastPage;
            lbPrimero2.Enabled = !pgsource.IsFirstPage;
            lbUltimo2.Enabled = !pgsource.IsLastPage;

            rpUnidadSelecionadas.DataSource = pgsource;
            rpUnidadSelecionadas.DataBind();

            //metodo que realiza la paginacion
            Paginacion2();

            MostrarTablaRepartirGastos();
        }

        /// <summary>
        /// Kevin Picado
        /// 09/oct/2019
        /// Efecto: Reinicia la tabla Unidades 
        /// Requiere: Seleccionar un periodo o proyecto
        /// Modifica: tabla Unidades
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reiniciarTablaUnidad()
        {
            Session["partidasPorUnidadesProyectoPeriodo"] = null;
            Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
            listaUnidad.RemoveAll(item => item.idUnidad > 0);
            MostrarDatosTablaUnidad(listaUnidad);
            count = 0;
            contador = 0;
            MostrarTablaRepartirGastos();
        }

        //protected void txtMontoIngresar_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtMontoIngresar.Text != "")
        //    {
        //        ButtonRepartir.Visible=false;
        //    }
        //    else
        //    {
        //        ButtonRepartir.Visible=true;
        //    }
        //}

        private void MostrarTablaRepartirGastos()
        {
            List<Partida> partidasElegidas = new List<Partida>();
            partidasElegidas = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
            if (listaUnidad.Count >= 2  && partidasElegidas!=null)
            {
                UpdatePanel10.Visible = true;
                ButtonRepartir.Visible = true;
            }
            else
            {
                UpdatePanel10.Visible = false;
                ButtonRepartir.Visible = false;
            }
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

        protected void lbPrimero4_Click(object sender, EventArgs e)
        {
            paginaActual4 = 0;
            obtenerPartidasPorProyectoUnidadPeriodo();
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
        protected void lbUltimo4_Click(object sender, EventArgs e)
        {
            paginaActual4 = (Convert.ToInt32(ViewState["TotalPaginas4"]) - 1);
            obtenerPartidasPorProyectoUnidadPeriodo();
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
        protected void lbAnterior4_Click(object sender, EventArgs e)
        {
            paginaActual4 -= 1;
            obtenerPartidasPorProyectoUnidadPeriodo();
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
        protected void lbSiguiente4_Click(object sender, EventArgs e)
        {
            paginaActual4 += 1;
            obtenerPartidasPorProyectoUnidadPeriodo();
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

        protected void rptPaginacion4_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual4 = Convert.ToInt32(e.CommandArgument.ToString());
            obtenerPartidasPorProyectoUnidadPeriodo();
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

        protected void rptPaginacion4_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion4");
            if (lnkPagina.CommandArgument != paginaActual4.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }


        //protected void textBox_TextChanged(object sender, EventArgs e)
        //{
        //    var bl = !string.IsNullOrEmpty(txtMontoIngresar.Text);

        //    ButtonRepartir.Visible = bl;
        //}
        protected void textBox1_TextChanged(object sender, EventArgs e)
        {
            //
            // This changes the main window text when you type into the TextBox.
            //
            if (IsNumeric(txtMontoIngresar.Text))
            {
                monto = monto + Convert.ToDouble(txtMontoIngresar.Text);
            }
           
        }
    }
}