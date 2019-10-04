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

namespace PEP.Catalogos.Periodos
{
    public partial class AdministrarPeriodo : System.Web.UI.Page
    {
        #region variables globales
        private PeriodoServicios periodoServicios;
        private ProyectoServicios proyectoServicios;
        private UnidadServicios unidadServicios;
        private static int periodoActualSelec;
        public static int proyectoActualSelec = 0;
        private bool botones = false;
        /*VARIABLES PAGINACIÓN */
        private static Periodo periodoSelccionado = new Periodo();
        private static Proyectos proyectoSelccionado = new Proyectos();
        private static Proyectos proyectoSelccionadoUnidades = new Proyectos();
        public static Unidad unidadSeleccionada = new Unidad();
        private static Periodo periodoActual = new Periodo();
        readonly PagedDataSource pgsourcePeriodos = new PagedDataSource();
        readonly PagedDataSource pgsourceProyectos = new PagedDataSource();
        readonly PagedDataSource pgsource = new PagedDataSource();
        int primerIndex, ultimoIndex, primerIndex2, ultimoIndex2, primerIndex3, ultimoIndex3, primerIndex4, ultimoIndex4, primerIndex5, ultimoIndex5;
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
        private int paginaActual4
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
        private int paginaActual5
        {
            get
            {
                if (ViewState["paginaActual5"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["paginaActual5"]);
            }
            set
            {
                ViewState["paginaActual5"] = value;
            }
        }
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            bool visible = false;
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);

            this.periodoServicios = new PeriodoServicios();
            this.proyectoServicios = new ProyectoServicios();
            this.unidadServicios = new UnidadServicios();

            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());

                CargarPeriodos();


            }
            else
            {
                //AgregarPeriodoBtn.Click += new EventHandler((snd, evt) => AgregarPeriodo_Click(snd, evt));
                //AgregarProyectoBtn.Click += new EventHandler((snd, evt) => AgregarProyecto_Click(snd, evt));
                //EstablecerPeriodoActualBtn.Click += new EventHandler((snd, evt) => EstablecerPeriodoActual_Click(snd, evt));


                //GuardarProyectosBtn.Click += new EventHandler((snd, evt) => GuardarProyectos_Click(snd, evt));
                //AgregarUnidadBtn.Click += new EventHandler((snd, evt) => AgregarUnidad_Click(snd, evt));
            }
            if (proyectoActualSelec == 0) divUnidades.Visible = false;
            else divUnidades.Visible = true;
            if (periodoActualSelec == 0) divPaginacionProyectos.Visible = false;
            else divPaginacionProyectos.Visible = true;
            MostrarPeriodos();


        }
        #endregion

        #region logica

        private void CargarPeriodos()
        {
            PeriodosDDL.Items.Clear();
            PeriodosDDL2.Items.Clear();
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
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
                    PeriodosDDL2.Items.Add(itemPeriodo);

                }

                if (anoHabilitado != 0)
                {
                    PeriodosDDL.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                    PeriodosDDL2.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                }

                //CargarPeriodosNuevos();
                //CargarProyectosActuales();
                MostrarPeriodos();
            }
        }


        private void CargarProyectosActuales()
        {
            //ProyectosActualesDDL.Items.Clear();


            if (!PeriodosDDL.SelectedValue.Equals(""))
            {
                AnoActual.Text = PeriodosDDL.SelectedValue;
                Session["periodo"] = PeriodosDDL.SelectedValue;

                LinkedList<Proyectos> proyectos = new LinkedList<Proyectos>();
                proyectos = this.proyectoServicios.ObtenerPorPeriodo(Int32.Parse(PeriodosDDL.SelectedValue));

                if (proyectos.Count > 0)
                {
                    foreach (Proyectos proyecto in proyectos)
                    {
                        ListItem itemLB = new ListItem(proyecto.nombreProyecto, proyecto.idProyecto.ToString());


                        //if (proyecto.esUCR)
                        //{
                        //    ListItem itemDDL = new ListItem(proyecto.nombreProyecto, proyecto.idProyecto.ToString());
                        //    ProyectosActualesDDL.Items.Add(itemDDL);
                        //}
                    }

                    //CargarUnidadesActuales2();
                }
            }
        }

        private void CargarProyectosNuevos()
        {

        }

        //private void CargarUnidadesActuales()
        //{
        //    UnidadesActualesLB.Items.Clear();

        //    if (!ProyectosActualesDDL.SelectedValue.Equals(""))
        //    {
        //        LinkedList<Unidad> unidades = new LinkedList<Unidad>();
        //        unidades = this.unidadServicios.ObtenerPorProyecto(Int32.Parse(ProyectosActualesDDL.SelectedValue));
        //        Session["proyecto"] = ProyectosActualesDDL.SelectedValue;

        //        foreach (Unidad unidad in unidades)
        //        {
        //            ListItem itemLB = new ListItem(unidad.nombreUnidad, unidad.idUnidad.ToString());
        //            UnidadesActualesLB.Items.Add(itemLB);
        //        }
        //    }
        //}


        /*private void CargarUnidadesActuales2()
        {
            UnidadesActualesLB.Items.Clear();

            int[] indices = null;
            if (indices.Length == 1)
            //if (!ProyectosActualesLB.SelectedValue.Equals(""))
            {
                LinkedList<Unidad> unidades = new LinkedList<Unidad>();
                Proyectos proyecto = null;
                unidades = this.unidadServicios.ObtenerPorProyecto(proyecto.idProyecto);

                Session["proyecto"] = proyecto.idProyecto;

                if (proyecto.esUCR)
                {
                    foreach (Unidad unidad in unidades)
                    {
                        ListItem itemLB = new ListItem(unidad.nombreUnidad, unidad.idUnidad.ToString());
                        UnidadesActualesLB.Items.Add(itemLB);
                    }
                    divUnidades.Visible = true;
                }
                else
                {
                    divUnidades.Visible = false;
                }
            }
            else
            {
                divUnidades.Visible = false;
            }
        }*/

        #endregion

        #region eventos click

        protected void AgregarPeriodo_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/Periodos/NuevoPeriodo.aspx");
            Response.Redirect(url);
        }

        protected void EliminarPeriodo_Click(object sender, EventArgs e)
        {
            if (!PeriodosDDL.SelectedValue.Equals(""))
            {
                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Int32.Parse(PeriodosDDL.SelectedValue);
                Session["periodoEliminar"] = periodo;

                String url = Page.ResolveUrl("~/Catalogos/Periodos/EliminarPeriodo.aspx");
                Response.Redirect(url);
            }
        }



        protected void EditarProyecto_Click(object sender, EventArgs e)
        {
            int[] indices = null;
            if (indices.Length == 1)
            {
                Proyectos proyecto = null;
                Session["proyectoEditar"] = proyecto;

                String url = Page.ResolveUrl("~/Catalogos/Proyecto/EditarProyecto.aspx");
                Response.Redirect(url);
            }
        }

        protected void EliminarProyecto_Click(object sender, EventArgs e)
        {
            int[] indices = null;
            if (indices.Length == 1)
            {
                Proyectos proyecto = null;
                Session["proyectoEliminar"] = proyecto;

                String url = Page.ResolveUrl("~/Catalogos/Proyecto/EliminarProyecto.aspx");
                Response.Redirect(url);
            }
        }



        /*
         * protected void EditarUnidad_Click(object sender, EventArgs e)
        {
            int[] indices = UnidadesActualesLB.GetSelectedIndices();
            if (indices.Length == 1)
            {
                Unidad unidad = this.unidadServicios.ObtenerPorId(Int32.Parse(UnidadesActualesLB.SelectedValue));
                Session["unidadEditar"] = unidad;

                String url = Page.ResolveUrl("~/Catalogos/Unidades/EditarUnidad.aspx");
                Response.Redirect(url);
            }
        }

        protected void EliminarUnidad_Click(object sender, EventArgs e)
        {
            int[] indices = UnidadesActualesLB.GetSelectedIndices();
            if (indices.Length == 1)
            {
                Unidad unidad = this.unidadServicios.ObtenerPorId(Int32.Parse(UnidadesActualesLB.SelectedValue));
                Session["unidadEliminar"] = unidad;

                String url = Page.ResolveUrl("~/Catalogos/Unidades/EliminarUnidad.aspx");
                Response.Redirect(url);
            }
        }
        */




        #endregion

        #region eventos onchanged



        protected void PeriodosNuevos_OnChanged(object sender, EventArgs e)
        {
            CargarProyectosNuevos();
        }

        protected void ProyectosActualesLB_OnChanged(object sender, EventArgs e)
        {
            //CargarUnidadesActuales2();
        }

        #endregion

        #region paginación

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
        /// 29/abr/2019
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
        /// 30/abr/2019
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
        #region paginacion tabla proyectos
        /// <summary>
        /// Leonardo Carrion
        /// 29/abr/2019
        /// Efecto: realiza la paginacion
        /// Requiere: -
        /// Modifica: paginacion mostrada en pantalla
        /// Devuelve: -
        /// </summary>
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
            if (ultimoIndex4 > Convert.ToInt32(ViewState["TotalPaginas4"]))
            {
                ultimoIndex4 = Convert.ToInt32(ViewState["TotalPaginas4"]);
                primerIndex4 = ultimoIndex4 - 4;
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
        /// </summary>
        private void Paginacion5()
        {
            var dt = new DataTable();
            dt.Columns.Add("IndexPagina"); //Inicia en 0
            dt.Columns.Add("PaginaText"); //Inicia en 1

            primerIndex5 = paginaActual5 - 2;
            if (paginaActual5 > 2)
                ultimoIndex5 = paginaActual5 + 2;
            else
                ultimoIndex5 = 4;

            //se revisa que la ultima pagina sea menor que el total de paginas a mostrar, sino se resta para que muestre bien la paginacion
            if (ultimoIndex5 > Convert.ToInt32(ViewState["TotalPaginas5"]))
            {
                ultimoIndex5 = Convert.ToInt32(ViewState["TotalPaginas5"]);
                primerIndex5 = ultimoIndex5 - 4;
            }

            if (primerIndex5 < 0)
                primerIndex5 = 0;

            //se crea el numero de paginas basado en la primera y ultima pagina
            for (var i = primerIndex5; i < ultimoIndex5; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaginacion5.DataSource = dt;
            rptPaginacion5.DataBind();
        }
        protected void lbPrimero4_Click(object sender, EventArgs e)
        {
            paginaActual4 = 0;
            MostrarTablaProyectos();
        }
        protected void lbAnterior4_Click(object sender, EventArgs e)
        {
            paginaActual4 -= 1;
            MostrarTablaProyectos();
        }
        protected void rptPaginacion4_ItemCommand(object source, DataListCommandEventArgs e)
        {

            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual4 = Convert.ToInt32(e.CommandArgument.ToString());
            MostrarTablaProyectos();
        }
        protected void rptPaginacion4_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion4");
            if (lnkPagina.CommandArgument != paginaActual4.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }

        protected void lbSiguiente4_Click(object sender, EventArgs e)
        {
            paginaActual4 += 1;

        }

        protected void lbUltimo4_Click(object sender, EventArgs e)
        {
            paginaActual4 = (Convert.ToInt32(ViewState["TotalPaginas4"]) - 1);
            MostrarTablaProyectos();
        }
        #endregion

        #region paginacion tabla periodos
        protected void lbPrimero_Click(object sender, EventArgs e)
        {
            paginaActual = 0;
            MostrarPeriodos();
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
            MostrarPeriodos();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/abr/2019
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
            MostrarPeriodos();
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
        protected void lbSiguiente_Click(object sender, EventArgs e)
        {
            paginaActual += 1;
            MostrarPeriodos();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 10/abr/2019
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
            MostrarPeriodos();
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
        protected void rptPaginacion_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion");
            if (lnkPagina.CommandArgument != paginaActual.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }
        #endregion

        #region paginacion tabla transferir y tranferidos
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
        protected void rptPaginacion3_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion3");
            if (lnkPagina.CommandArgument != paginaActual3.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
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
        protected void lbPrimero2_Click(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            cargarTablaProyectosAtransferir();
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
            cargarTablaProyectosAtransferir();
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
            cargarTablaProyectosAtransferir();
        }
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
            cargarTablaProyectosAtransferir();
        }

        protected void lbUltimo2_Click(object sender, EventArgs e)
        {
            paginaActual2 = (Convert.ToInt32(ViewState["TotalPaginas2"]) - 1);
            cargarTablaProyectosAtransferir();
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
        protected void rptPaginacion3_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual3 = Convert.ToInt32(e.CommandArgument.ToString());
            cargarTablaProyectosTransferidos();
        }
        #endregion

        #region paginacion tabla proyectos transferidos

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
        protected void lbPrimero3_Click(object sender, EventArgs e)
        {
            paginaActual3 = 0;
            cargarTablaProyectosTransferidos();
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
        protected void lbAnterior3_Click(object sender, EventArgs e)
        {
            paginaActual3 -= 1;
            cargarTablaProyectosTransferidos();
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
        protected void lbSiguiente3_Click(object sender, EventArgs e)
        {
            paginaActual3 += 1;
            cargarTablaProyectosTransferidos();
        }

        protected void lbUltimo3_Click(object sender, EventArgs e)
        {
            paginaActual3 = (Convert.ToInt32(ViewState["TotalPaginas3"]) - 1);
            cargarTablaProyectosTransferidos();
        }
        #endregion



        #region eventos nuevos
        /********************FILTRO PERIODO*******************************************/
        protected void Periodos_OnChanged(object sender, EventArgs e)
        {

            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(PeriodosDDL.SelectedValue);
            LinkedList<Periodo> listaPeriodos = new LinkedList<Periodo>();


            List<Periodo> periodoLista = new List<Periodo>();
            listaPeriodos = this.periodoServicios.ObtenerTodos();

            if (listaPeriodos.Count > 0)
            {
                foreach (Periodo periodo1 in listaPeriodos)
                {
                    if (periodo1.anoPeriodo.ToString().Equals(periodo.anoPeriodo.ToString()))
                    {
                        periodoLista.Add(periodo);

                    }
                }
            }
            Session["listaPeriodos"] = periodoLista;
            Session["listaPeriodosFiltrada"] = periodoLista;
            var dt = periodoLista;
            pgsourcePeriodos.DataSource = dt;
            pgsourcePeriodos.AllowPaging = false;

            ViewState["TotalPaginas"] = pgsourcePeriodos.PageCount;
            rpPeriodos.DataSource = pgsourcePeriodos;
            rpPeriodos.DataBind();

        }



        /*********************CARGAR DATOS PERIODOS**********************/
        private void MostrarPeriodos()
        {
            LinkedList<Periodo> listaPeriodos = new LinkedList<Periodo>();
            listaPeriodos = periodoServicios.ObtenerTodos();
            Session["listaPeriodos"] = listaPeriodos;
            var dt = listaPeriodos;
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

            rpPeriodos.DataSource = pgsource;
            rpPeriodos.DataBind();

            //metodo que realiza la paginacion
            Paginacion();


        }



        /*****************ELIMINAR PERIODO*********************************/
        protected void btnEliminar_Click1(object sender, EventArgs e)
        {
            int anoPeriodo = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Periodo> listaPeriodos = (List<Periodo>)Session["listaPeriodos"];
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = anoPeriodo;
            Session["periodoEliminar"] = periodo;

            String url = Page.ResolveUrl("~/Catalogos/Periodos/EliminarPeriodo.aspx");
            Response.Redirect(url);

        }

        /*****************EVENTO SELECCIONAR PERIODO**********************/
        protected void btnSelccionar_Click(object sender, EventArgs e)
        {
            divPaginacionProyectos.Visible = true;
            int anoPeriodo = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            LinkedList<Periodo> listaPeriodos = (LinkedList<Periodo>)Session["listaPeriodos"];

            Periodo periodoSeleccionado = new Periodo();

            foreach (Periodo periodo in listaPeriodos)
            {
                if (periodo.anoPeriodo == anoPeriodo)
                {
                    periodoSeleccionado = periodo;
                    break;
                }

            }
            periodoActualSelec = anoPeriodo;
            AnoActual.Text = "Periodo Seleccionado: " + periodoSeleccionado.anoPeriodo;
            Session["periodo"] = periodoSeleccionado.anoPeriodo;
            botones = true;
            btnTransferir.Visible = botones;
            btnNuevoProyecto.Visible = botones;
            MostrarTablaProyectos();
        }


        /*****************CARGAR PROYECTOS POR PERIODO***********************/
        private void MostrarTablaProyectos()
        {
            int anoPeriodo = Convert.ToInt32(Session["periodo"]);
            LinkedList<Entidades.Proyectos> listaProyectos = this.proyectoServicios.ObtenerPorPeriodo(anoPeriodo);
            Session["listaProyectos"] = listaProyectos;

            var dt = listaProyectos;
            pgsourceProyectos.DataSource = dt;
            pgsourceProyectos.AllowPaging = false;

            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual4;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas4"] = pgsourceProyectos.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina4.Text = "Página " + (paginaActual4 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior4.Enabled = !pgsource.IsFirstPage;
            lbSiguiente4.Enabled = !pgsource.IsLastPage;
            lbPrimero4.Enabled = !pgsource.IsFirstPage;
            lbUltimo4.Enabled = !pgsource.IsLastPage;

            rpProyectos.DataSource = pgsourceProyectos;
            rpProyectos.DataBind();


            //metodo que realiza la paginacion
            Paginacion4();
        }
        /****************EVENTOS MODAL NUEVO PERIODO**********************************/

        protected void btnNuevoPeriodoModal_Click(object sender, EventArgs e)
        {
            if (validarPeriodoNuevo())
            {
                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(txtNuevoP.Text);
                periodoServicios.Insertar(periodo);
                txtNuevoP.Text = "";

                LinkedList<Periodo> listaPeriodos = periodoServicios.ObtenerTodos();
                Session["listaPeriodos"] = listaPeriodos;
                MostrarPeriodos();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoPeriodo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoPeriodo').hide();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoPeriodo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoPeriodo').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoPeriodo();", true);
            }
        }
        public Boolean validarPeriodoNuevo()
        {
            Boolean valido = true;
            txtNuevoP.CssClass = "form-control";

            #region nombre
            if (String.IsNullOrEmpty(txtNuevoP.Text) || txtNuevoP.Text.Trim() == String.Empty || txtNuevoP.Text.Length > 255)
            {
                txtNuevoP.CssClass = "form-control alert-danger";
                valido = false;
            }
            #endregion

            return valido;
        }
        protected void btnNuevoPeriodo_Click(object sender, EventArgs e)
        {
            txtNuevoP.CssClass = "form-control";
            txtNuevoP.Text = "";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoPeriodo();", true);
        }

        /**********************EVENTOS MODAL ELIMINAR PERIODO*************************/
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            txtPeriodoEliminarModal.CssClass = "form-control";

            int anoPeriodo = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            LinkedList<Periodo> listaPeriodos = (LinkedList<Periodo>)Session["listaPeriodos"];

            foreach (Periodo periodo in listaPeriodos)
            {
                if (periodo.anoPeriodo == anoPeriodo)
                {
                    periodoSelccionado = periodo;
                    txtPeriodoEliminarModal.Text = periodo.anoPeriodo.ToString();

                    break;
                }
            }

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarPeriodo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarPeriodo').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarPeriodo();", true);
        }

        public void btnConfirmarEliminarPeriodo_Click(Object sender, EventArgs e)
        {

            lbConfPer.Text = periodoSelccionado.anoPeriodo.ToString();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmarPeriodo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmarPeriodo').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalConfirmarPeriodo()", true);
        }
        /*******************************MODAL CONFIRMAR ELIMINACIÓN DE PERIODO******************************************/
       

        protected void btnEliminarModal_Click(object sender, EventArgs e)
        {
            Periodo periodo = periodoSelccionado;

            periodoServicios.EliminarPeriodo(periodo.anoPeriodo);

            LinkedList<Periodo> listaPeriodos = periodoServicios.ObtenerTodos();

            Session["listaPeriodos"] = listaPeriodos;

            MostrarPeriodos();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmarPeriodo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmarPeriodo').hide();", true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarPeriodo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarPeriodo').hide();", true);
        }
        /*********************************EVENTOS MODAL EDITAR PROYECTO*************************************************************/
        protected void btnEditarProyecto_Click(object sender, EventArgs e)
        {
            int idProyecto = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            string anoPeriodo = proyectoServicios.ObtenerPorId(idProyecto).periodo.anoPeriodo + "";
            LinkedList<Proyectos> listaProyectos = (LinkedList<Proyectos>)Session["listaProyectos"];


            foreach (Proyectos proyecto in listaProyectos)
            {
                if (proyecto.idProyecto == idProyecto)
                {
                    proyectoSelccionado = proyecto;
                    break;
                }
            }
            txtNombreEditar.CssClass = "form-control";
            txtTipoEditar.CssClass = "form-control";
            //lbPeriodoEditar.CssClass = "form-control";
            txtNombreEditar.Text = proyectoSelccionado.nombreProyecto;
            lbPeriodoEditar.Text = anoPeriodo;

            if (proyectoSelccionado.esUCR)
            {
                txtTipoEditar.Text = "UCR";

            }
            else
            {
                txtTipoEditar.Text = "Fundevi";
            }

            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalEditarProyecto();", true);
        }

        protected void btnActualizarProyectoModal_Click(object sender, EventArgs e)
        {
            if (validarProyectoAEditar())
            {
                Proyectos proyectoEditar = proyectoServicios.ObtenerPorId(proyectoSelccionado.idProyecto);
                proyectoEditar.nombreProyecto = txtNombreEditar.Text;
                proyectoServicios.ActualizarProyecto(proyectoEditar);
                txtNombreEditar.Text = "";

                LinkedList<Proyectos> listaProyectos = proyectoServicios.ObtenerPorPeriodo(proyectoEditar.periodo.anoPeriodo);
                Session["listaProyectos"] = listaProyectos;
                MostrarTablaProyectos();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarProyecto').hide();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarProyecto').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarProyecto();", true);
            }

        }
        public Boolean validarProyectoAEditar()
        {
            Boolean valido = true;
            txtNombreEditar.CssClass = "form-control";

            #region nombre
            if (String.IsNullOrEmpty(txtNombreEditar.Text) || txtNombreEditar.Text.Trim() == String.Empty || txtNombreEditar.Text.Length > 255)
            {
                txtNombreEditar.CssClass = "form-control alert-danger";
                valido = false;
            }
            #endregion

            return valido;
        }
        /**********************ESTABLECER PERIODO COMO ACTUAL*******************************************************/
        protected void EstablecerPeriodoActual_Click(object sender, EventArgs e)
        {
            int anoPeriodo = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            if (anoPeriodo != 0)
            {
                LinkedList<Periodo> listaPeriodos = (LinkedList<Periodo>)Session["listaPeriodos"];
                periodoActual = new Periodo();

                foreach (Periodo periodo in listaPeriodos)
                {
                    if (periodo.anoPeriodo == anoPeriodo)
                    {
                        periodoActual = periodo;
                    }
                }
                bool respuesta = this.periodoServicios.HabilitarPeriodo(anoPeriodo);

                if (respuesta)
                {
                    Toastr("success", "Periodo establecido con éxito!");
                }
                else
                {
                    Toastr("error", "Error al establecer el proyecto");
                }

                CargarPeriodos();
            }
        }

        /**************************EVENTOS MODAL ELIMINAR PROYECTO**********************************************************/
        public void btnEliminarProyecto_Click(Object sender, EventArgs e)
        {
            int codigoProyecto = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            string anoPeriodo = periodoActualSelec.ToString();
            proyectoSelccionado = proyectoServicios.ObtenerPorId(codigoProyecto);

            LinkedList<Proyectos> proyectos = (LinkedList<Proyectos>)Session["listaProyectos"];

          
            if (proyectoSelccionado.esUCR)
            {
                txtTipoElim.Text = "UCR";
            }
            else
            {
                txtTipoElim.Text = "Fundevi";
            }
            lblElimPerProyModal.Text = anoPeriodo;
            txtProyEliminar.Text = proyectoSelccionado.nombreProyecto;


            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalEliminarProyecto()", true);

        }


        public void btnConfirmarEliminarProyecto_Click(Object sender, EventArgs e)
        {

            lbConfProy.Text = proyectoSelccionado.nombreProyecto;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmarProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmarProyecto').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalConfirmarProyecto()", true);
        }

        protected void btnEliminarProyectoModal_Click(object sende, EventArgs e)
        {
            int codigoP = proyectoSelccionado.idProyecto;
            proyectoServicios.EliminarProyecto(codigoP);
            LinkedList<Proyectos> listaProyectos = proyectoServicios.ObtenerPorPeriodo(periodoSelccionado.anoPeriodo);
            Session["listaProyectos"] = listaProyectos;
            MostrarTablaProyectos();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmaProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmarProyecto').hide();", true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarProyecto').hide();", true);


        }

        /*****************************************NUEVO PROYECTO *******************************************************/
        protected void AgregarProyecto_Click(object sender, EventArgs e)
        {
            txtCodigoProyecto.CssClass = "form-control";
            txtNombreProyecto.CssClass = "form-control";
            txtNombreProyecto.Text = "";
            txtCodigoProyecto.Text = "";
            CargarPeriodos();
            PeriodosDDL2.SelectedIndex = 0;
            int anoP = periodoSelccionado.anoPeriodo;


            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoProyecto();", true);
        }
        public void btnAgregarProyectoModal_Click(object sende, EventArgs e)
        {
            if (validarProyectoNuevo())
            {
                Proyectos proyecto = new Proyectos();
                proyecto.nombreProyecto = txtNombreProyecto.Text;
                proyecto.codigo = txtCodigoProyecto.Text;
                proyecto.esUCR = Convert.ToBoolean(ddlEsUCRProyecto.SelectedValue);
                proyecto.periodo = new Periodo();
                proyecto.periodo.anoPeriodo = Convert.ToInt32(PeriodosDDL2.SelectedValue.ToString());

                int respuesta = proyectoServicios.Insertar(proyecto);

                if (respuesta > 0)
                {
                    LinkedList<Proyectos> listaProyectos = proyectoServicios.ObtenerPorPeriodo(proyecto.periodo.anoPeriodo);
                    Session["listaProyectos"] = listaProyectos;
                    MostrarTablaProyectos();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoProyecto').hide();", true);
                }
                else if (respuesta == -1)
                {
                    //Ya existe un proyecto con el mismo codigo en el periodo seleccionado
                }

            }

        }

        public Boolean validarProyectoNuevo()
        {
            Boolean validados = true;

            #region validacion periodo
            if (Session["periodo"] == null)
            {
                validados = false;
            }
            #endregion

            #region validacion nombre proyecto
            String nombreProyecto = txtNombreProyecto.Text;

            if (nombreProyecto.Trim() == "")
            {
                txtNombreProyecto.CssClass = "form-control alert-danger";

                validados = false;
            }
            #endregion

            #region validacion codigo proyecto
            String codigoProyecto = txtCodigoProyecto.Text;

            if (codigoProyecto.Trim() == "")
            {
                txtCodigoProyecto.CssClass = "form-control alert-danger";
                validados = false;
            }
            #endregion

            return validados;
        }
        /*************************Transferir Proyectos*******************************************/

        public void btnTransferirProyecto_Click(object sender, EventArgs e)
        {
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = periodoActualSelec;
           
            lblPeriodoSeleccionado.Text = periodoActualSelec.ToString();

            LinkedList<Periodo> periodos = new LinkedList<Periodo>();

            ddlPeriodoTranferir.Items.Clear();
            periodos = periodoServicios.ObtenerTodos();

            int anoHabilitado = 0;
            int periodoAtransferirSelec = 0;

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
                        ddlPeriodoTranferir.Items.Add(itemPeriodo);
                    }
                }
            }

            LinkedList<Proyectos> proyectosTransferir = proyectoServicios.ObtenerPorPeriodo(periodoActualSelec);
            Session["listaProyectoTransferir"] = proyectosTransferir;
            Session["listaProyectosTransferirFiltrado"] = proyectosTransferir;

            cargarTablaProyectosAtransferir();

            periodoAtransferirSelec = Convert.ToInt32(ddlPeriodoTranferir.SelectedValue.ToString());

            LinkedList<Proyectos> proyectosTransferidos = proyectoServicios.ObtenerPorPeriodo(periodoAtransferirSelec);
            Session["listaProyectoTransferidos"] = proyectosTransferidos;
            Session["listaProyectosTransferidosFiltrado"] = proyectosTransferidos;

            cargarTablaProyectosTransferidos();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalTransferirProyecto()", true);

        }

        public void btnSeleccionarProyectoTransferir(object sender, EventArgs e)
        {
            int idProyecto = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            LinkedList<Proyectos> listaProyectos = (LinkedList<Proyectos>)Session["listaProyectosTransferir"];

        }

        protected void ddlPeriodoModalTransfeririP_SelectedIndexChanged(object sender, EventArgs e)
        {
            Periodo periodoAgregado = new Periodo();
            periodoAgregado.anoPeriodo = Convert.ToInt32(ddlPeriodoTranferir.SelectedValue);

            LinkedList<Proyectos> listaProyectosAgregados = proyectoServicios.ObtenerPorPeriodo(periodoAgregado.anoPeriodo);

            Session["listaProyectoTransferidos"] = listaProyectosAgregados;
            Session["listaProyectosTransferidosFiltrado"] = listaProyectosAgregados;

            cargarTablaProyectosTransferidos();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalTransferirProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalTransferirProyecto').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalTransferirProyecto();", true);
        }

        public void cargarTablaProyectosAtransferir()
        {
            LinkedList<Proyectos> listaProyectos = (LinkedList<Proyectos>)Session["ListaProyectoTransferir"];
            /*filtro*/
            var dt2 = listaProyectos;
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

            rpTransferirProyecto.DataSource = listaProyectos;
            rpTransferirProyecto.DataBind();

            Paginacion2();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalTransferirProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalTransferirProyecto').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalTransferirProyecto();", true);

        }

        public void cargarTablaProyectosTransferidos()
        {
            LinkedList<Proyectos> llistaProyectosT = (LinkedList<Proyectos>)Session["ListaProyectoTransferidos"];

            /*filtros*/
            var dt3 = llistaProyectosT;
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
            rpProyectoTransferidos.DataSource = llistaProyectosT;
            rpProyectoTransferidos.DataBind();

            Paginacion3();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalTransferirProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalTransferirProyecto').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalTransferirProyecto();", true);
        }

        public void btnSeleccionarProyectoT_Click(object sender, EventArgs e)
        {

            string idProyecto = ((LinkButton)(sender)).CommandArgument.ToString();
            int anioPeriodo = periodoActualSelec;
            int periodoTransferido = 0;

            LinkedList<Proyectos> listaProyectos = proyectoServicios.ObtenerPorPeriodo(anioPeriodo);
          
            foreach (Proyectos proyecto in listaProyectos)
            {
                
                if (proyecto.codigo.Equals(idProyecto))
                {
                    Toastr("success", "Periodo establecido con éxito!");
                    Proyectos proyectoInsertar = proyecto;
                    proyectoInsertar.periodo.anoPeriodo = Convert.ToInt32(ddlPeriodoTranferir.SelectedValue);
                    periodoTransferido = proyectoInsertar.periodo.anoPeriodo;
                    proyectoServicios.Insertar(proyectoInsertar);


                }
            }

            MostrarTablaProyectos();
            anioPeriodo = periodoActualSelec;
            listaProyectos = proyectoServicios.ObtenerPorPeriodo(anioPeriodo);
            Session["listaProyectoTransferir"] = listaProyectos;
            Session["listaProyectosTranferirFiltrado"] = listaProyectos;

            

            cargarTablaProyectosAtransferir();
            listaProyectos = proyectoServicios.ObtenerPorPeriodo(periodoTransferido);
            Session["listaProyectoTranferidos"] = listaProyectos;
            Session["listaProyectosTranferidosFiltrado"] = listaProyectos;

            cargarTablaProyectosTransferidos();

            Proyectos proyectoAgregado = new Proyectos();
            Periodo p = new Periodo();

            p.anoPeriodo = periodoTransferido;
            proyectoAgregado.periodo = p;
            LinkedList<Proyectos> listaProyectosAgregados = proyectoServicios.ObtenerPorPeriodo(proyectoAgregado.periodo.anoPeriodo);
            Session["listaProyectoTransferidos"] = listaProyectosAgregados;
            Session["listaProyectosTranferidosFiltrado"] = listaProyectosAgregados;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalTransferirProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalTransferirProyecto').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalTransferirProyecto();", true);
        }

        #region acciones partidas

        /*Seleccionar partida*/
        public void btnSelccionarProyecto_Click(object sender, EventArgs e)
        {
            divUnidades.Visible = true;
            int idProyecto = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            proyectoSelccionadoUnidades = proyectoServicios.ObtenerPorId(idProyecto);
            proyectoActualSelec = proyectoSelccionadoUnidades.idProyecto;
            LinkedList<Unidad> listaUnidades = new LinkedList<Unidad>();
            listaUnidades = unidadServicios.ObtenerPorProyecto(proyectoSelccionadoUnidades.idProyecto);
            Session["listaUnidades"] = listaUnidades;
            mostrarTablaUnidades();
        }

        /*Mostrar Partidas*/
        public void mostrarTablaUnidades()
        {
            LinkedList<Unidad> listaUnidades = (LinkedList<Unidad>)Session["listaUnidades"];
            /*FILTRO*/

            var dt = listaUnidades;
            pgsource.DataSource = dt;
            pgsource.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual5;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas5"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina5.Text = "Página " + (paginaActual5 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior5.Enabled = !pgsource.IsFirstPage;
            lbSiguiente5.Enabled = !pgsource.IsLastPage;
            lbPrimero5.Enabled = !pgsource.IsFirstPage;
            lbUltimo5.Enabled = !pgsource.IsLastPage;

            rpUnidProyecto.DataSource = pgsource;
            rpUnidProyecto.DataBind();

            //metodo que realiza la paginacion
            Paginacion5();
        }
        /*Cargar proyectos para nuevas partidas*/
        public void cargarProyectosUnidades() { 
            int anioP = proyectoSelccionadoUnidades.periodo.anoPeriodo;
        
            if (anioP != 0)
                ProyectosDDL.Items.Clear();
            {
                LinkedList<Proyectos> proyectos = new LinkedList<Proyectos>();
                proyectos = this.proyectoServicios.ObtenerPorPeriodo(anioP);

                if (proyectos.Count > 0)
                {
                    foreach (Proyectos proyecto in proyectos)
                    {
                        if (proyecto.esUCR)
                        {
                            ListItem itemProyecto = new ListItem(proyecto.nombreProyecto, proyecto.idProyecto.ToString());
                            ProyectosDDL.Items.Add(itemProyecto);
                        }
                    }

                    if (anioP != 0)
                    {
                        string proyectoHabilitado = proyectoSelccionadoUnidades.nombreProyecto;
                        
                    }
                }
            }
        }
        /*agregar nueva unidad*/
        protected void AgregarUnidad_Click(object sender, EventArgs e)
        {
            txtNombreUnidad.CssClass = "form-control";
            txtCoordinadorUnidad.CssClass = "form-control";
            txtNombreUnidad.Text = "";
            txtCoordinadorUnidad.Text = "";
            cargarProyectosUnidades();
            
            PeriodosDDL.SelectedIndex = 0;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaUnidad();", true);
        }
        /*agregar nueva unidad*/
        protected void btnNuevaUnidadModal_Click(object sender, EventArgs e)
        {
            //se validan los campos antes de guardar los datos en la base de datos
            if (validarUnidadNueva())
            {
                Unidad unidad = new Unidad();
                unidad.nombreUnidad = txtNombreUnidad.Text;
                unidad.coordinador = txtCoordinadorUnidad.Text;
                unidad.proyecto = new Proyectos();
                unidad.proyecto.idProyecto = Convert.ToInt32(ProyectosDDL.SelectedValue.ToString());

                unidadServicios.Insertar(unidad);
                LinkedList<Unidad> listaUnidades = unidadServicios.ObtenerPorProyecto(unidad.proyecto.idProyecto);
                Session["listaUnidades"] = listaUnidades;
                mostrarTablaUnidades();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaUnidad').hide();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaUnidad').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaUnidad();", true);
            }
        }

        /*validar nueva unidad*/
        public Boolean validarUnidadNueva()
        {
            Boolean valido = true;
            txtNombreUnidad.CssClass = "form-control";
            txtCoordinadorUnidad.CssClass = "form-control";

            #region nombre
            if (String.IsNullOrEmpty(txtNombreUnidad.Text) || txtNombreUnidad.Text.Trim() == String.Empty || txtNombreUnidad.Text.Length > 255)
            {
                txtNombreUnidad.CssClass = "form-control alert-danger";
                valido = false;
            }
            if (String.IsNullOrEmpty(txtCoordinadorUnidad.Text) || txtCoordinadorUnidad.Text.Trim() == String.Empty || txtCoordinadorUnidad.Text.Length > 255)
            {
                txtCoordinadorUnidad.CssClass = "form-control alert-danger";
                valido = false;
            }
            #endregion

            return valido;
        }

        /*Eliminar unidad*/
        public void btnEliminarUnidad_Click(Object sender, EventArgs e)
        {
            int idUnidad= Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString()); ;
            LinkedList<Unidad>listaUnidades=(LinkedList<Unidad>)Session["listaUnidades"];
            unidadSeleccionada = unidadServicios.ObtenerPorId(idUnidad);
            txtNombreUnidadEliminar.Text = unidadSeleccionada.nombreUnidad;
            txtCoordinadorEliminar.Text = unidadSeleccionada.coordinador;
            lbProyUnidadElim.Text = proyectoSelccionadoUnidades.nombreProyecto;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarUnidad').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarUnidad()", true);
        }
        public void btnConfirmarEliminarUnidad_Click(Object sender, EventArgs e)
        {
            
            lbConfUnidadEliminar.Text = unidadSeleccionada.nombreUnidad;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmar", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmar').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalConfirmar()", true);
        }

        public void btnEliminarUnidadModal_Click(Object sender, EventArgs e)
        {
            Unidad unidadEliminar = unidadSeleccionada;
            unidadServicios.EliminarUnidad(unidadEliminar.idUnidad);
            LinkedList<Unidad> listaUnidades = unidadServicios.ObtenerPorProyecto(proyectoSelccionadoUnidades.idProyecto);
            Session["listaUnidades"] = listaUnidades;

            mostrarTablaUnidades();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmar", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmar').hide();", true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarUnidad').hide();", true);

        }
        /*Editar unidad*/
        public void btnEditarUnidad_Click(object sender, EventArgs e)
        {
            int idUnidad = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            
            string nombreProyecto = proyectoSelccionadoUnidades.nombreProyecto;
            LinkedList<Unidad> listaUnidades = (LinkedList<Unidad>)Session["listaUnidades"];

            

            foreach (Unidad unidad in listaUnidades)
            {
                if (unidad.idUnidad==idUnidad)
                {
                    unidadSeleccionada = unidad;
                    break;
                }

            }
            txtNombreUnidadEditar.CssClass = "form-control";
            txtCoordinadorEditar.CssClass = "form-control";
            lbProyectoUnidad.CssClass = "form-control";
            lbProyectoUnidad.Text = nombreProyecto;
            txtNombreUnidadEditar.Text = unidadSeleccionada.nombreUnidad;
            txtCoordinadorEditar.Text = unidadSeleccionada.coordinador;
            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalEditarUnidad();", true);
        }
        protected void btnActualizarUnidadModal_Click(object sender, EventArgs e)
        {
            if (validarUnidadAEditar())
            {
               Unidad unidadEditar = unidadServicios.ObtenerPorId(unidadSeleccionada.idUnidad);
               unidadEditar.nombreUnidad = txtNombreUnidadEditar.Text;
               unidadEditar.coordinador = txtCoordinadorEditar.Text;
               unidadServicios.ActualizarUnidad(unidadEditar);
               txtNombreUnidadEditar.Text = "";
               txtCoordinadorEditar.Text = "";

                LinkedList<Unidad> listaUnidades = unidadServicios.ObtenerPorProyecto(proyectoSelccionadoUnidades.idProyecto);
                Session["listaUnidades"] = listaUnidades;
                mostrarTablaUnidades();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarUnidad').hide();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarUnidad').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarUnidad();", true);
            }

        }

        public Boolean validarUnidadAEditar()
        {
            Boolean valido = true;
            txtNombreUnidadEditar.CssClass = "form-control";
            txtCoordinadorEditar.CssClass = "form-control";
            if (String.IsNullOrEmpty(txtNombreUnidadEditar.Text) || txtNombreUnidadEditar.Text.Trim() == String.Empty || txtNombreUnidadEditar.Text.Length > 255)
            {
                txtNombreUnidadEditar.CssClass = "form-control alert-danger";
                valido = false;
            }
            if (String.IsNullOrEmpty(txtCoordinadorEditar.Text) || txtCoordinadorEditar.Text.Trim() == String.Empty || txtCoordinadorEditar.Text.Length > 255)
            {
                txtCoordinadorEditar.CssClass = "form-control alert-danger";
                valido = false;
            }
            return valido;
        }
       

        #endregion Fin acciones partidas
        #region paginacion tabla unidades

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
        protected void lbPrimero5_Click(object sender, EventArgs e)
        {
            paginaActual5 = 0;
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
        protected void lbAnterior5_Click(object sender, EventArgs e)
        {
            paginaActual2 -= 1;
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
        protected void rptPaginacion5_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual5 = Convert.ToInt32(e.CommandArgument.ToString());
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
        protected void rptPaginacion5_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion5");
            if (lnkPagina.CommandArgument != paginaActual5.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }
        protected void lbSiguiente5_Click(object sender, EventArgs e)
        {
            paginaActual2 += 1;
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
        protected void lbUltimo5_Click(object sender, EventArgs e)
        {
            paginaActual5 = (Convert.ToInt32(ViewState["TotalPaginas5"]) - 1);
            mostrarTablaUnidades();
        }
        #endregion FIN paginacion tabla unidades

        public void desactivarBotonesUnidades()
        {
            btnNuevaUnidad.Visible = false;
        }

        public void activarBotonesUnidades()
        {
            btnNuevaUnidad.Visible = true;
        }



        #endregion

        #region otros

        private void Toastr(string tipo, string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr." + tipo + "('" + mensaje + "');", true);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["CheckRefresh"] = Session["CheckRefresh"];
        }


        /**********CONFIRMAR***************/
        protected void CONFIRMAR(object sender, EventArgs e)
        {


            MostrarPeriodos();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmarEliminarPeriodo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmarEliminarPeriodo').hide();", true);


        }
        #endregion
        #endregion
    }
}
