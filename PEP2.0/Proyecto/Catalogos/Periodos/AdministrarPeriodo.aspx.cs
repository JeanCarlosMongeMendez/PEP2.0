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
        private SubUnidadServicios subUnidadServicios = new SubUnidadServicios();
        private static int periodoActualSelec;
        public static int proyectoActualSelec = 0;
        private bool botones = false;
        private static Periodo periodoSelccionado = new Periodo();
        private static Proyectos proyectoSelccionado = new Proyectos();
        private static Proyectos proyectoSelccionadoUnidades = new Proyectos();
        public static Unidad unidadSeleccionada = new Unidad();
        private static Periodo periodoActual = new Periodo();
        readonly PagedDataSource pgsourcePeriodos = new PagedDataSource();
        readonly PagedDataSource pgsourceProyectos = new PagedDataSource();
        readonly PagedDataSource pgsource = new PagedDataSource();
        int primerIndex, ultimoIndex, primerIndex2, ultimoIndex2, primerIndex3, ultimoIndex3, primerIndex4, ultimoIndex4, primerIndex5, ultimoIndex5, primerIndex6, ultimoIndex6;
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

        private int paginaActual6
        {
            get
            {
                if (ViewState["paginaActual6"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["paginaActual6"]);
            }
            set
            {
                ViewState["paginaActual6"] = value;
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



            MostrarPeriodos();
        }
        #endregion

        #region logica

        /// <summary>
        /// Mariela Calvo
        /// septiembre/2019
        /// Efecto: llena los DropDownList con los periodos que se encuentran en la base de datos 
        /// Requiere: - 
        /// Modifica: DropDownList
        /// Devuelve: -
        /// </summary>
        private void CargarPeriodos()
        {
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
                }

                if (anoHabilitado != 0)
                {
                    //PeriodosDDL.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                    //PeriodosDDL2.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                }
                MostrarPeriodos();
            }
        }


        #endregion


        #region paginación

        /// <summary>
        /// Mariela Calvo
        /// septiembre/2019
        /// Efecto: Paginación Tabla Periodos
        /// Requiere: - 
        /// Modifica: Tabla Periodos
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
        /// Mariela Calvo
        /// Septiembre/2019
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
        /// Mariela Calvo
        /// Septiembre/2019
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
        /// Mariela Calvo   
        /// Septiembre/2019
        /// Efecto: se devuelve a la págian pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla de contactos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrimero_Click(object sender, EventArgs e)
        {
            paginaActual = 0;
            MostrarPeriodos();
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
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
        /// Mariela Calvo
        /// Septiembre/2019
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
        /// Mariela Calvo
        /// Septiembre/2019
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
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: realiza la paginacion de la tabla proyectos
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

        protected void lbPrimero2_Click(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            cargarTablaProyectosAtransferir();
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
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
        /// Mariela Calvo
        /// Septiembre/2019
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: actualiza la la pagina actual y muestra los datos de la misma
        /// Requiere: -
        /// Modifica: elementos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="source"></param>
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
        /// Mariela Calvo
        /// Septiembre/2019
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla de contactos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo2_Click(object sender, EventArgs e)
        {
            paginaActual2 = (Convert.ToInt32(ViewState["TotalPaginas2"]) - 1);
            cargarTablaProyectosAtransferir();
        }

        /// <summary>
        /// Mariela Calvo
        /// septiembre/2019
        /// Efecto: realiza la paginacion de la tabla proyectos a transferir
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
        /// Mariela Calvo
        /// Septiembre/2019
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
        /// Mariela Calvo
        /// Septiembre/2019
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


        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
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
        /// Mariela Calvo
        /// Septiembre/2019
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
        /// Mariela Calvo
        /// Septiembre/2019
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla de contactos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo3_Click(object sender, EventArgs e)
        {
            paginaActual3 = (Convert.ToInt32(ViewState["TotalPaginas3"]) - 1);
            cargarTablaProyectosTransferidos();
        }

        /// <summary>
        /// Mariela Calvo
        /// septiembre/2019
        /// Efecto: realiza la paginacion de la tabla proyectos transferidos
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
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
            MostrarTablaProyectos();
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: se devuelve a la pagina anterior y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Anterior pagina"
        /// Modifica: elementos mostrados en la tabla de contactos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAnterior4_Click(object sender, EventArgs e)
        {
            paginaActual4 -= 1;
            MostrarTablaProyectos();
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
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
            MostrarTablaProyectos();
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: se devuelve a la pagina siguiente y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Siguiente pagina"
        /// Modifica: elementos mostrados en la tabla de contactos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        protected void lbSiguiente4_Click(object sender, EventArgs e)
        {
            paginaActual4 += 1;
            MostrarTablaProyectos();
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla de contactos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo4_Click(object sender, EventArgs e)
        {
            paginaActual4 = (Convert.ToInt32(ViewState["TotalPaginas4"]) - 1);
            MostrarTablaProyectos();
        }


        /// <summary>
        /// Mariela Calvo
        /// septiembre/2019
        /// Efecto: realiza la paginacion de la tabla proyectos unidades
        /// Requiere: - 
        /// Modifica: DropDownList
        /// Devuelve: -
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
        ///Mariela Calvo
        /// septiembre/2019
        /// Efecto: se devuelve a la pagina anterior y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Anterior pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAnterior5_Click(object sender, EventArgs e)
        {
            paginaActual5 -= 1;
            mostrarTablaUnidades();
        }

        /// <summary>
        /// Mariela Calvo
        /// septiembre/2019
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
        /// Mariela Calvo
        /// septiembre/2019
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: se devuelve a la pagina siguiente y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Siguiente pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSiguiente5_Click(object sender, EventArgs e)
        {
            paginaActual5 += 1;
            mostrarTablaUnidades();
        }

        /// <summary>
        /// Mariela Calvo
        /// septiembre/2019
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

        /// <summary>
        /// Mariela Calvo
        /// septiembre/2019
        /// Efecto: realiza la paginacion de la tabla proyectos unidades
        /// Requiere: - 
        /// Modifica: DropDownList
        /// Devuelve: -
        /// </summary>
        private void Paginacion6()
        {
            var dt = new DataTable();
            dt.Columns.Add("IndexPagina"); //Inicia en 0
            dt.Columns.Add("PaginaText"); //Inicia en 1

            primerIndex6 = paginaActual6 - 2;
            if (paginaActual6 > 2)
                ultimoIndex6 = paginaActual6 + 2;
            else
                ultimoIndex6 = 4;

            //se revisa que la ultima pagina sea menor que el total de paginas a mostrar, sino se resta para que muestre bien la paginacion
            if (ultimoIndex6 > Convert.ToInt32(ViewState["TotalPaginas6"]))
            {
                ultimoIndex6 = Convert.ToInt32(ViewState["TotalPaginas6"]);
                primerIndex6 = ultimoIndex5 - 4;
            }

            if (primerIndex6 < 0)
                primerIndex6 = 0;

            //se crea el numero de paginas basado en la primera y ultima pagina
            for (var i = primerIndex6; i < ultimoIndex6; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaginacion6.DataSource = dt;
            rptPaginacion6.DataBind();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 28/oct/2020
        /// Efecto: se devuelve a la primera pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Primer pagina"
        /// Modifica: elementos mostrados
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrimero6_Click(object sender, EventArgs e)
        {
            paginaActual6 = 0;
            mostrarTablaSubUnidades();
        }

        /// <summary>
        ///Mariela Calvo
        /// 28/oct/2020
        /// Efecto: se devuelve a la pagina anterior y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Anterior pagina"
        /// Modifica: elementos mostrados
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAnterior6_Click(object sender, EventArgs e)
        {
            paginaActual6 -= 1;
            mostrarTablaSubUnidades();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 28/oct/2020
        /// Efecto: actualiza la la pagina actual y muestra los datos de la misma
        /// Requiere: -
        /// Modifica: elementos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptPaginacion6_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual6 = Convert.ToInt32(e.CommandArgument.ToString());
            mostrarTablaSubUnidades();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 28/oct/2020
        /// Efecto: marca el boton de la pagina seleccionada
        /// Requiere: dar clic al boton de paginacion
        /// Modifica: color del boton seleccionado
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptPaginacion6_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion6");
            if (lnkPagina.CommandArgument != paginaActual5.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }

        /// <summary>
        /// Leonardo Carrion
        /// 28/oct/2020
        /// Efecto: se devuelve a la pagina siguiente y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Siguiente pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSiguiente6_Click(object sender, EventArgs e)
        {
            paginaActual6 += 1;
            mostrarTablaSubUnidades();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 28/oct/2020
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -lbPrimero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo6_Click(object sender, EventArgs e)
        {
            paginaActual6 = (Convert.ToInt32(ViewState["TotalPaginas6"]) - 1);
            mostrarTablaSubUnidades();
        }
        #endregion



        #region eventos nuevos

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Muestra el filtro del periodo seleccionado
        /// Requiere: - 
        /// Modifica: Tabla Periodos
        /// Devuelve: -
        /// </summary>
        protected void Periodos_OnChanged(object sender, EventArgs e)
        {
            // divPaginacionProyectos.Visible = false;
            divUnidades.Visible = false;
            Periodo periodo = new Periodo();
            //periodo.anoPeriodo = Convert.ToInt32(PeriodosDDL.SelectedValue);
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
            pgsource.DataSource = dt;
            pgsource.AllowPaging = false;

            ViewState["TotalPaginas"] = pgsource.PageCount;


            rpPeriodos.DataSource = pgsource;
            rpPeriodos.DataBind();

        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Muestra la tabla con todos los periodos de la base de datos
        /// Requiere: - 
        /// Modifica: Tabla Periodos
        /// Devuelve: -
        /// </summary>
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Seleccionar un periodo para ver sus proyectos
        /// Requiere: Seleccionar el check del Periodo
        /// Modifica: Tabla Proyectos
        /// Devuelve: -
        /// </summary>
        protected void btnSelccionar_Click(object sender, EventArgs e)
        {
            divProyectosPeriodos.Visible = true;
            divUnidades.Visible = false;
            int anoPeriodo = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            LinkedList<Periodo> listaPeriodos = (LinkedList<Periodo>)Session["listaPeriodos"];
            periodoSelccionado = new Periodo();

            foreach (Periodo periodo in listaPeriodos)
            {
                if (periodo.anoPeriodo == anoPeriodo)
                {
                    periodoSelccionado = periodo;
                    break;
                }

            }
            periodoActualSelec = anoPeriodo;
            AnoActual.Text = "Periodo Seleccionado: " + periodoSelccionado.anoPeriodo;
            Session["periodo"] = periodoSelccionado.anoPeriodo;
            botones = true;
            btnTransferir.Visible = botones;
            btnNuevoProyecto.Visible = botones;

            Toastr("success", "Periodo " + anoPeriodo + " seleccionado con éxito!");
            MostrarTablaProyectos();
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Mostrar los datos de los proyectos del periodo seleccionado
        /// Requiere: Seleccionar el check del Periodo deseado
        /// Modifica: Tabla Proyectos
        /// Devuelve: -
        /// </summary>
        private void MostrarTablaProyectos()
        {
            int anoPeriodo = Convert.ToInt32(Session["periodo"]);
            LinkedList<Entidades.Proyectos> listaProyectos = this.proyectoServicios.ObtenerPorPeriodo(anoPeriodo);
            Session["listaProyectos"] = listaProyectos;

            var dt = listaProyectos;
            pgsource.DataSource = dt;
            pgsource.AllowPaging = false;

            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual4;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas4"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina4.Text = "Página " + (paginaActual4 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior4.Enabled = !pgsource.IsFirstPage;
            lbSiguiente4.Enabled = !pgsource.IsLastPage;
            lbPrimero4.Enabled = !pgsource.IsFirstPage;
            lbUltimo4.Enabled = !pgsource.IsLastPage;

            rpProyectos.DataSource = pgsource;
            rpProyectos.DataBind();

            //metodo que realiza la paginacion
            Paginacion4();
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Activar modal nuevo periodo
        /// Requiere: Presionar boton nuevo periodo
        /// Modifica: Tabla Periodos
        /// Devuelve: -
        /// </summary>
        protected void btnNuevoPeriodo_Click(object sender, EventArgs e)
        {
            txtNuevoP.CssClass = "form-control";
            txtNuevoP.Text = "";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoPeriodo();", true);
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Guardar un nuevo periodo
        /// Requiere: Introducir datos del nuevo periodo, presionar boton guardar del modal nuevo periodo
        /// Modifica: Tabla Periodos
        /// Devuelve: -
        /// </summary>
        protected void btnNuevoPeriodoModal_Click(object sender, EventArgs e)
        {
            int respuesta = 0;
            if (validarPeriodoNuevo())
            {
                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(txtNuevoP.Text);
                respuesta = periodoServicios.Insertar(periodo);
                txtNuevoP.Text = "";
                if (respuesta == periodo.anoPeriodo)
                {
                    Toastr("success", "Período registrado con éxito!");
                }
                else
                {
                    Toastr("error", "Error, el período " + periodo.anoPeriodo + " ya se encuentra registrado");
                }
                LinkedList<Periodo> listaPeriodos = periodoServicios.ObtenerTodos();
                Session["listaPeriodos"] = listaPeriodos;
                MostrarPeriodos();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalNuevoPeriodo();", true);
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoPeriodo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoPeriodo').hide();", true);
            }
            else
            {
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoPeriodo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoPeriodo').hide();", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoPeriodo();", true);
            }
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Validar que los datos del nuevo periodo sean ingresados
        /// Requiere: -
        /// Modifica: Tabla Periodos
        /// Devuelve: -
        /// </summary>
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Activar modal eliminar periodo para proceder a eliminar un periodo
        /// Requiere: Presionar boto nuevo periodo
        /// Modifica: Tabla Periodos
        /// Devuelve: -
        /// </summary>
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

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarPeriodo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarPeriodo').hide();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarPeriodo();", true);
            lbConfPer.Text = periodoSelccionado.anoPeriodo.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalConfirmarPeriodo();", true);
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Mensaje de confirmacion para la eliminacion de un periodo
        /// Requiere: Presionar boto eliminar del modal eliminar periodo
        /// Modifica: Tabla Periodos
        /// Devuelve: -
        /// </summary>
        public void btnConfirmarEliminarPeriodo_Click(Object sender, EventArgs e)
        {
            lbConfPer.Text = periodoSelccionado.anoPeriodo.ToString();

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalEliminarPeriodo()", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalConfirmarPeriodo();", true);
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Encargado de eliminar el periodo luego de la confirmacion
        /// Requiere: Presionar boton confirmar del modal confirmar eliminar periodo
        /// Modifica: Tabla Periodos
        /// Devuelve: -
        /// </summary>
        /// 
        protected void btnEliminarModal_Click(object sender, EventArgs e)
        {
            
            Periodo periodo = periodoSelccionado;

            periodoServicios.EliminarPeriodo(periodo.anoPeriodo);

            LinkedList<Periodo> listaPeriodos = periodoServicios.ObtenerTodos();

            if (listaPeriodos.Contains(periodo))
            {
                Toastr("error", "Error al eliminar el período");
            }
            else
            {
                Toastr("success", "Período eliminado con éxito!");
            }
            Session["listaPeriodos"] = listaPeriodos;

            MostrarPeriodos();

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmarPeriodo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmarPeriodo').hide();", true);
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarPeriodo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarPeriodo').hide();", true);
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Mostrar modal de editar proyecto
        /// Requiere: Presionar boton con icono editar en tabla proyectos del proyecto deseado
        /// Modifica: Tabla Proyectos
        /// Devuelve: -
        /// </summary>
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: MEncargado de modificar el proyecto que se selcciono
        /// Requiere: Presionar boton con icono actualizar del proyecto deseado
        /// Modifica: Tabla Proyectos
        /// Devuelve: -
        /// </summary>
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

                Toastr("success", "Proyecto actualizado con éxito!");

                MostrarTablaProyectos();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarProyecto').hide();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarProyecto').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarProyecto();", true);
            }

        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Valida que los campos del proyecto a editar estén llenos
        /// Requiere: Presionar boton actualizar
        /// Modifica: Tabla Proyectos
        /// Devuelve: -
        /// </summary>
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Establecer un periodo como actual
        /// Requiere: Presionar boton con icono de manita arriba en tabla periodo de algun periodo
        /// Modifica: Tabla Periodos
        /// Devuelve: -
        /// </summary>
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Modal para eliminar proyecto
        /// Requiere: Presionar boton con icono de basurero en tabla proyectos de algun proyecto
        /// Modifica: Tabla Proyectos
        /// Devuelve: -
        /// </summary>
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Mensaje de confirmacion para la eliminacion de un proyecto
        /// Requiere: Presionar boton eliminar del modal eliminar proyecto
        /// Modifica: Tabla Periodos
        /// Devuelve: -
        /// </summary>
        public void btnConfirmarEliminarProyecto_Click(Object sender, EventArgs e)
        {

            lbConfProy.Text = proyectoSelccionado.nombreProyecto;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmarProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmarProyecto').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalConfirmarProyecto()", true);
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Encargado de eliminar el proyecto luego de la confirmacion
        /// Requiere: Presionar boton confirmar del modal confirmar eliminar proyecto
        /// Modifica: Tabla Periodos
        /// Devuelve: -
        /// </summary>
        /// 
        protected void btnEliminarProyectoModal_Click(object sende, EventArgs e)
        {
            int codigoP = proyectoSelccionado.idProyecto;
            Proyectos proyectoEliminar = proyectoServicios.ObtenerPorId(codigoP);
            proyectoServicios.EliminarProyecto(codigoP);
            LinkedList<Proyectos> listaProyectos = proyectoServicios.ObtenerPorPeriodo(periodoSelccionado.anoPeriodo);

            if (listaProyectos.Contains(proyectoEliminar))
            {
                Toastr("success", "Erro, no se pudo eliminar el proyecto " + proyectoEliminar.nombreProyecto);
            }
            else
            {
                Toastr("success", "Se elimino el proyecto " + proyectoEliminar.nombreProyecto + " con éxito!");
            }
            Session["listaProyectos"] = listaProyectos;
            MostrarTablaProyectos();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmaProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmarProyecto').hide();", true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarProyecto').hide();", true);


        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Modal de agregar proyecto
        /// Requiere: Presionar boton Nuevo Proyecto 
        /// Modifica: Tabla Proyectos
        /// Devuelve: -
        /// </summary>
        /// 
        protected void AgregarProyecto_Click(object sender, EventArgs e)
        {
            txtCodigoProyecto.CssClass = "form-control";
            txtNombreProyecto.CssClass = "form-control";
            txtNombreProyecto.Text = "";
            txtCodigoProyecto.Text = "";
            lbPeriodoDDLNuevo.Text = periodoSelccionado.anoPeriodo.ToString();
            CargarPeriodos();
            //PeriodosDDL2.SelectedIndex = 0;
            int anoP = periodoSelccionado.anoPeriodo;


            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoProyecto();", true);
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Encargado de insertar un nuevo proyecto en un periodo especifico 
        /// Requiere: Presionar boton guardar del modal nuevo proyecto
        /// Modifica: Tabla Proyectos
        /// Devuelve: -
        /// </summary>
        /// 
        public void btnAgregarProyectoModal_Click(object sende, EventArgs e)
        {
            if (validarProyectoNuevo())
            {
                Proyectos proyecto = new Proyectos();
                proyecto.nombreProyecto = txtNombreProyecto.Text;
                proyecto.codigo = txtCodigoProyecto.Text;
                proyecto.esUCR = Convert.ToBoolean(ddlEsUCRProyecto.SelectedValue);
                proyecto.periodo = new Periodo();
                proyecto.periodo.anoPeriodo = periodoSelccionado.anoPeriodo;

                int respuesta = proyectoServicios.Insertar(proyecto);

                if (respuesta > 0)
                {
                    Toastr("success", "Se registró el proyecto " + proyecto.nombreProyecto + " al período " + periodoSelccionado.anoPeriodo + " con éxito!");
                    LinkedList<Proyectos> listaProyectos = proyectoServicios.ObtenerPorPeriodo(proyecto.periodo.anoPeriodo);
                    Session["listaProyectos"] = listaProyectos;
                    MostrarTablaProyectos();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoProyecto').hide();", true);
                }
                else if (respuesta == -1)
                {
                    Toastr("error", "Error, el proyecto con código " + proyecto.codigo + " ya fue registrado para el período " + periodoSelccionado.anoPeriodo);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoProyecto').hide();", true);

                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoProyecto').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoProyecto();", true);
            }

        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Encargado de validar que todos los campos del nuevo proyecto estén llenos
        /// Requiere: Presionar boton guardar del modal nuevo proyecto
        /// Modifica: Tabla Proyectos
        /// Devuelve: -
        /// </summary>
        /// 
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Encargado de mostrar el modal para transferir proyecto de un periodo a otro
        /// Requiere: Presionar Tranferir proyectos
        /// Modifica: Tabla Proyectos
        /// Devuelve: -
        /// </summary>
        /// 
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

            LinkedList<Proyectos> proyTransferir = proyectoServicios.ObtenerPorPeriodo(periodoSelccionado.anoPeriodo);
            LinkedList<Proyectos> proyTransferidos = proyectoServicios.ObtenerPorPeriodo(Convert.ToInt32(ddlPeriodoTranferir.SelectedValue));



            Session["listaProyectoTransferir"] = proyTransferir;



            cargarTablaProyectosAtransferir();

            periodoAtransferirSelec = Convert.ToInt32(ddlPeriodoTranferir.SelectedValue.ToString());

            LinkedList<Proyectos> proyectosTransferidos = proyectoServicios.ObtenerPorPeriodo(periodoAtransferirSelec);
            Session["listaProyectoTransferidos"] = proyectosTransferidos;
            Session["listaProyectosTransferidosFiltrado"] = proyectosTransferidos;

            cargarTablaProyectosTransferidos();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalTransferirProyecto()", true);

        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Encargado de mostrar la tabla del periodo al cual se van a transferir proyectos
        /// Requiere: Presionar boton con icono de flecha del pryecto que se desea transferir 
        /// Modifica: DropDownList y Tabla Proyectos Tranferidos
        /// Devuelve: -
        /// </summary>
        ///
        protected void ddlPeriodoModalTransfeririP_SelectedIndexChanged(object sender, EventArgs e)
        {
            Periodo periodoAgregado = new Periodo();
            periodoAgregado.anoPeriodo = Convert.ToInt32(ddlPeriodoTranferir.SelectedValue);

            LinkedList<Proyectos> listaProyectosAgregados = proyectoServicios.ObtenerPorPeriodo(periodoAgregado.anoPeriodo);

            LinkedList<Proyectos> proyTransferir = proyectoServicios.ObtenerPorPeriodo(periodoActualSelec);

            Session["listaProyectoTransferir"] = proyTransferir;
            Session["listaProyectosTranferirFiltrado"] = proyTransferir;

            cargarTablaProyectosAtransferir();

            Session["listaProyectoTransferidos"] = listaProyectosAgregados;
            Session["listaProyectosTransferidosFiltrado"] = listaProyectosAgregados;

            cargarTablaProyectosTransferidos();


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalTransferirProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalTransferirProyecto').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalTransferirProyecto();", true);
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Carga los datos de la tabla de proyectos de acuerdo al periodo anteriormente elegido para pasar transferir proyectos
        /// Requiere:- 
        /// Modifica: Tabla de Proyectos a tranferir
        /// Devuelve: -
        /// </summary>
        ///
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Carga los datos de la tabla de proyectos transferidos de acuerdo al periodo que se selecciono para pasar periodos
        /// Requiere:- 
        /// Modifica: Tabla de Proyectos a tranferir
        /// Devuelve: -
        /// </summary>
        ///
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Se encarga de transferir del proyecto actualmente seleccionado al periodo al que se selcciono transferir
        /// Requiere:Seleccionar icono de flecha en alguno de los proyectos en el modal de tranferir proyectos 
        /// Modifica: Tabla Proyectos Transferidos
        /// Devuelve: -
        /// </summary>
        ///
        public void btnSeleccionarProyectoT_Click(object sender, EventArgs e)
        {
            int idProyecto = Convert.ToInt32(((LinkButton)(sender)).CommandArgument.ToString());
            int anioPeriodo = periodoActualSelec;
            int periodoTransferido = 0;
            int proyectoTransferido = 0;

            Proyectos proyectoInsertar = new Proyectos();
            LinkedList<Proyectos> listaProyectos = proyectoServicios.ObtenerPorPeriodo(anioPeriodo);
            List<Unidad> listaUnidades = new List<Unidad>();

            foreach (Proyectos proyecto in listaProyectos)
            {

                if (proyecto.idProyecto == idProyecto)
                {
                    proyectoInsertar = proyecto;
                    Periodo periodoInsertar = new Periodo();
                    periodoInsertar.anoPeriodo = Convert.ToInt32(ddlPeriodoTranferir.SelectedValue);
                    proyectoInsertar.periodo = periodoInsertar;
                    periodoTransferido = proyectoInsertar.periodo.anoPeriodo;
                    proyectoTransferido = proyectoServicios.Insertar(proyectoInsertar);
                }

            }

            listaUnidades = unidadServicios.ObtenerPorProyecto(idProyecto);
            foreach (Unidad unidad in listaUnidades)
            {
                Proyectos proyectoUnidad = new Proyectos();
                proyectoUnidad.idProyecto = proyectoTransferido;
                unidad.proyecto = proyectoUnidad;
                unidadServicios.Insertar(unidad);
            }

            MostrarTablaProyectos();

            LinkedList<Proyectos> proyTransferir = proyectoServicios.ObtenerPorPeriodo(periodoSelccionado.anoPeriodo);
            LinkedList<Proyectos> proyTransferidos = proyectoServicios.ObtenerPorPeriodo(Convert.ToInt32(ddlPeriodoTranferir.SelectedValue));

            Session["listaProyectoTransferir"] = proyTransferir;
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



            cargarTablaProyectosTransferidos();


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalTransferirProyecto", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalTransferirProyecto').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalTransferirProyecto();", true);

            if (proyectoTransferido != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "El proyecto " + proyectoInsertar.nombreProyecto + " fue transferido con éxito!');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Error, el proyecto  no fue transferido, intente nuevamente');", true);
            }

        }



        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Mostrar las unidaddes de un proyecto al seleccionar el mismo en la tabla de proyectos
        /// Requiere: Presionar el boton con el icono check de laguno de los proyectos de la tabala de proyectos
        /// Modifica: Tabla de PrUnidades
        /// Devuelve: -
        /// </summary>
        ///
        public void btnSelccionarProyecto_Click(object sender, EventArgs e)
        {

            divUnidades.Visible = true;
            int idProyecto = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            proyectoSelccionadoUnidades = proyectoServicios.ObtenerPorId(idProyecto);
            proyectoActualSelec = proyectoSelccionadoUnidades.idProyecto;
            proyectoActual.Text = "Proyecto Seleccionado: " + proyectoSelccionadoUnidades.nombreProyecto + "";

            if (proyectoSelccionadoUnidades.idProyecto != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "El proyecto " + proyectoSelccionadoUnidades.nombreProyecto + " fue seleccionado con éxito!');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "El proyecto no fue seleccionado, intente nuevamente');", true);
            }


            if (proyectoSelccionadoUnidades.esUCR)
            {
                btnNuevaUnidad.Visible = true;
            }
            else
            {
                btnNuevaUnidad.Visible = false;
            }


            List<Unidad> listaUnidades = new List<Unidad>();
            listaUnidades = unidadServicios.ObtenerPorProyecto(proyectoSelccionadoUnidades.idProyecto);
            Session["listaUnidades"] = listaUnidades;
            mostrarTablaUnidades();
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Carga los datos de la tabla de unidades de acuerdo al proyectos seleccionado de la BD
        /// Requiere: Seleccionar un proyecto de la tabla proyectos
        /// Modifica: Tabla Unidades
        /// Devuelve: -
        /// </summary>
        ///
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

        /// <summary>
        /// Leonardo Carrion
        /// 12/nov/2020
        /// Efecto: Carga los datos de la tabla de sub unidades de acuerdo a la unidad seleccionada de la BD
        /// Requiere: Seleccionar una unidad de la tabla unidades
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        public void mostrarTablaSubUnidades()
        {
            List<SubUnidad> listaSubUnidades = (List<SubUnidad>)Session["listaSubUnidades"];
            /*FILTRO*/

            var dt = listaSubUnidades;
            pgsource.DataSource = dt;
            pgsource.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual5;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas6"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina6.Text = "Página " + (paginaActual6 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior6.Enabled = !pgsource.IsFirstPage;
            lbSiguiente6.Enabled = !pgsource.IsLastPage;
            lbPrimero6.Enabled = !pgsource.IsFirstPage;
            lbUltimo6.Enabled = !pgsource.IsLastPage;

            rpSubUnidades.DataSource = pgsource;
            rpSubUnidades.DataBind();

            //metodo que realiza la paginacion
            Paginacion6();
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Cargar todos los proyectos del periodo anteriormente seleccionado en un dropDown para usarlos en la inserción de unidades
        /// Requiere: 
        /// Modifica: DropDown Periodos
        /// Devuelve: -
        /// </summary>
        ///
        public void cargarProyectosUnidades()
        {
            int anioP = proyectoSelccionadoUnidades.periodo.anoPeriodo;

            if (anioP != 0)
            //ProyectosDDL.Items.Clear();
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
                            //royectosDDL.Items.Add(itemProyecto);
                        }
                    }

                    if (anioP != 0)
                    {
                        string proyectoHabilitado = proyectoSelccionadoUnidades.nombreProyecto;

                    }
                }
            }
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Modal de nueva unidad
        /// Requiere: Presionar boton Nuevo Unidad
        /// Modifica: Tabla PUnidades
        /// Devuelve: -
        /// </summary>
        /// 
        protected void AgregarUnidad_Click(object sender, EventArgs e)
        {
            txtNombreUnidad.CssClass = "form-control";
            txtCoordinadorUnidad.CssClass = "form-control";
            txtNombreUnidad.Text = "";
            txtCoordinadorUnidad.Text = "";
            lbNuevaUnidadProy.Text = proyectoSelccionadoUnidades.nombreProyecto;
            cargarProyectosUnidades();

            //PeriodosDDL.SelectedIndex = 0;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaUnidad();", true);
        }
        
        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Encargado de insertar una unidad nueva en la BD
        /// Requiere: Presionar el boton con el icono check de laguno de los proyectos de la tabala de proyectos
        /// Modifica: Tabla de PrUnidades
        /// Devuelve: -
        /// </summary>
        ///
        protected void btnNuevaUnidadModal_Click(object sender, EventArgs e)
        {
            //se validan los campos antes de guardar los datos en la base de datos
            if (validarUnidadNueva())
            {
                Unidad unidad = new Unidad();
                unidad.nombreUnidad = txtNombreUnidad.Text;
                unidad.coordinador = txtCoordinadorUnidad.Text;
                unidad.proyecto = new Proyectos();
                unidad.proyecto.idProyecto = Convert.ToInt32(proyectoSelccionadoUnidades.idProyecto);

                int respuesta = unidadServicios.Insertar(unidad);
                if (respuesta != 0)
                {
                    Toastr("sucess", "La unidad " + unidad.nombreUnidad + " fue registrada con éxito en el proyecto " + proyectoSelccionadoUnidades.nombreProyecto);
                }
                else
                {
                    Toastr("error", "Error, la unidad " + unidad.nombreUnidad + " ya se encuentra registrada en el proyecto " + proyectoSelccionadoUnidades.nombreProyecto);
                }
                List<Unidad> listaUnidades = unidadServicios.ObtenerPorProyecto(proyectoActualSelec);
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
        
        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Encargado de validar que todos los campos de la nueva estén llenos
        /// Requiere: Presionar boton guardar del modal nueva unidad
        /// Modifica: Tabla Unidad
        /// Devuelve: -
        /// </summary>
        /// 
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

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Modal de eliminar unidad
        /// Requiere: Presionar boton con icono de basurero de alguna unidad de la tabla unidades
        /// Modifica: Tabla Unidades
        /// Devuelve: -
        /// </summary>
        /// 
        public void btnEliminarUnidad_Click(Object sender, EventArgs e)
        {
            int idUnidad = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString()); ;
            LinkedList<Unidad> listaUnidades = (LinkedList<Unidad>)Session["listaUnidades"];
            unidadSeleccionada = unidadServicios.ObtenerPorId(idUnidad);
            txtNombreUnidadEliminar.Text = unidadSeleccionada.nombreUnidad;
            txtCoordinadorEliminar.Text = unidadSeleccionada.coordinador;
            lbProyUnidadElim.Text = proyectoSelccionadoUnidades.nombreProyecto;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarUnidad').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarUnidad()", true);
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Mensaje de confirmacion para la eliminacion de una unidad
        /// Requiere: Presionar boton eliminar del modal eliminar unidad
        /// Modifica: Tabla Periodos
        /// Devuelve: -
        /// </summary>
        public void btnConfirmarEliminarUnidad_Click(Object sender, EventArgs e)
        {

            lbConfUnidadEliminar.Text = unidadSeleccionada.nombreUnidad;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmar", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmar').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalConfirmar()", true);
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Encargado de eliminar la unidad seleccionada
        /// Requiere: Presionar el boton con el icono check de laguno de los proyectos de la tabala de proyectos
        /// Modifica: Tabla de Unidades
        /// Devuelve: -
        /// </summary>
        ///
        public void btnEliminarUnidadModal_Click(Object sender, EventArgs e)
        {
            Unidad unidadEliminar = unidadSeleccionada;
            unidadServicios.EliminarUnidad(unidadEliminar.idUnidad);
            List<Unidad> listaUnidades = unidadServicios.ObtenerPorProyecto(proyectoSelccionadoUnidades.idProyecto);
            if (listaUnidades.Contains(unidadEliminar))
            {
                Toastr("error", "Error, la unidad " + unidadEliminar.nombreUnidad + " no pudo ser eliminada.");
            }
            else
            {
                Toastr("sucess", "La unidad " + unidadEliminar.nombreUnidad + " fue eliminada con éxito!");
            }
            Session["listaUnidades"] = listaUnidades;

            mostrarTablaUnidades();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmar", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmar').hide();", true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarUnidad').hide();", true);

        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Modal de editar una unidad
        /// Requiere: Presionar con el icono editar de una de las unidades en la tabla de unidades
        /// Modifica: Tabla Unidades
        /// Devuelve: -
        /// </summary>
        /// 
        public void btnEditarUnidad_Click(object sender, EventArgs e)
        {
            int idUnidad = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            string nombreProyecto = proyectoSelccionadoUnidades.nombreProyecto;
            LinkedList<Unidad> listaUnidades = (LinkedList<Unidad>)Session["listaUnidades"];



            foreach (Unidad unidad in listaUnidades)
            {
                if (unidad.idUnidad == idUnidad)
                {
                    unidadSeleccionada = unidad;
                    break;
                }

            }
            txtNombreUnidadEditar.CssClass = "form-control";
            txtCoordinadorEditar.CssClass = "form-control";

            lbProyectoUnidad.Text = nombreProyecto;
            txtNombreUnidadEditar.Text = unidadSeleccionada.nombreUnidad;
            txtCoordinadorEditar.Text = unidadSeleccionada.coordinador;
            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalEditarUnidad();", true);
        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Encargado de moddifcar la unidad selccionada
        /// Requiere: Presionar el boton guardar del modal editar unidad
        /// Modifica: Tabla de PrUnidades
        /// Devuelve: -
        /// </summary>
        ///
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

                List<Unidad> listaUnidades = unidadServicios.ObtenerPorProyecto(proyectoSelccionadoUnidades.idProyecto);

                Session["listaUnidades"] = listaUnidades;
                Toastr("success", "La unidad fue modificada con éxito!");
                mostrarTablaUnidades();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarUnidad').hide();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarUnidad').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarUnidad();", true);
            }

        }

        /// <summary>
        /// Mariela Calvo
        /// Septiembre/2019
        /// Efecto: Encargado de validar que todos los campos de la unidad a editar estén llenos
        /// Requiere: Presionar boton guardar del modal editar unidad
        /// Modifica: Tabla Unidad
        /// Devuelve: -
        /// </summary>
        /// 
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

        /// <summary>
        /// Leonardo Carrion
        /// 26/oct/2020
        /// Efecto:
        /// Requiere:
        /// Modifica:
        /// Devuelve:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubUnidades_Click(object sender, EventArgs e)
        {
            int idUnidad = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            string nombreProyecto = proyectoSelccionadoUnidades.nombreProyecto;
            LinkedList<Unidad> listaUnidades = (LinkedList<Unidad>)Session["listaUnidades"];
            
            foreach (Unidad unidad in listaUnidades)
            {
                if (unidad.idUnidad == idUnidad)
                {
                    unidadSeleccionada = unidad;
                    break;
                }

            }

            Session["listaSubUnidades"] = subUnidadServicios.getSubUnidadesPorUnidad(unidadSeleccionada);
            mostrarTablaSubUnidades();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalSubUnidades();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/nov/2020
        /// Efecto: Guarda la nueva sub unidad en la base de datos
        /// Requiere: ingresar nombre de sub unidad
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaSubUnidad_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtNombreSubUnidad.Text))
            {
                Toastr("error", "Debe ingresar el nombre de la sub unidad");
            }
            else
            {

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalSubUnidades();", true);
        }

        #region otros

        private void Toastr(string tipo, string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr." + tipo + "('" + mensaje + "');", true);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["CheckRefresh"] = Session["CheckRefresh"];
        }
        #endregion
        #endregion
    }
}
