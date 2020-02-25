using Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proyecto.Catalogos.Ejecucion
{
    public partial class ElegirEjecucion : System.Web.UI.Page
    {
        readonly PagedDataSource pgsource = new PagedDataSource();
        ProyectoServicios proyectoServicios;
        PeriodoServicios periodoServicios;
        EjecucionServicios ejecucionServicios;
        private int elmentosMostrar = 10;


        private int paginaActual4
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

        protected void lbPrimero4_Click(object sender, EventArgs e)
        {
            paginaActual4 = 0;
            MostrarDatosTablaUnidad();
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
            MostrarDatosTablaUnidad();
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
            MostrarDatosTablaUnidad();
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
            MostrarDatosTablaUnidad();
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
            MostrarDatosTablaUnidad();
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
        protected void Page_Load(object sender, EventArgs e)
        {
            this.proyectoServicios = new ProyectoServicios();
            this.periodoServicios = new PeriodoServicios();
            this.ejecucionServicios = new EjecucionServicios();
            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                PeriodosDDL.Items.Clear();
                ProyectosDDL.Items.Clear();
                CargarPeriodos();
                Session["listaUnidad"] = null;
                Session["listaPartida"] = null;
                Session["listaMontoPartidaDisponible"] = null;

                //DDLTipoTramite.Items.Clear();
                //ddlPartida.Items.Clear();
            }

            

        }
        /// <summary>
        /// kevin Picado Quesada
        /// Método utilizado para la accion en la selección del droplist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Periodos_OnChanged(object sender, EventArgs e)
        {
            //Session["partidasPorUnidadesProyectoPeriodo"] = null;
            //Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
             CargarProyectos();
             MostrarDatosTablaUnidad();
            //reiniciarTablaUnidad();
            //DDLTipoTramite.Items.Clear();
            //descripcionOtroTipoTramite.Visible = false;
        }
        /// <summary>
        /// Kevin Picado
        /// 21/2/2020
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
                //MostrarDatosTablaUnidad();
            }

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
            //Session["partidasPorUnidadesProyectoPeriodo"] = null;
            //Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
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
        protected void Proyectos_OnChanged(object sender, EventArgs e)
        {
            
            MostrarDatosTablaUnidad();
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
        private void MostrarDatosTablaUnidad()
        {

            string a = ProyectosDDL.SelectedValue;
            var dt = ejecucionServicios.ConsultarEjecucion(PeriodosDDL.SelectedValue, a);
                pgsource.DataSource = dt;
                pgsource.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual4;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas2"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina4.Text = "Página " + (paginaActual4 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior4.Enabled = !pgsource.IsFirstPage;
            lbSiguiente4.Enabled = !pgsource.IsLastPage;
            lbPrimero4.Enabled = !pgsource.IsFirstPage;
            lbUltimo4.Enabled = !pgsource.IsLastPage;

            rpUnidadSelecionadas.DataSource = pgsource;
            rpUnidadSelecionadas.DataBind();

                //metodo que realiza la paginacion
                //Paginacion2();

           
        }
        protected void EditarEjecucion_OnChanged(object sender, EventArgs e)
        {
           
            string idEjecucion = ((LinkButton)(sender)).CommandArgument.ToString();

            Session["listaUnidad"]= ejecucionServicios.ConsultarUnidadEjecucion(Convert.ToInt32(idEjecucion));
            Session["listaPartida"] = ejecucionServicios.ConsultarPartidaEjecucion(Convert.ToInt32(idEjecucion));
            Session["listaMontoPartidaDisponible"] = ejecucionServicios.ConsultarEjecucionMontoPartida(Convert.ToInt32(idEjecucion));
            String url = Page.ResolveUrl("~/Catalogos/Ejecucion/AdministrarEjecucion.aspx");
            Response.Redirect(url);
            //string[] unidadNumeroPartida = (((LinkButton)(sender)).CommandArgument.ToString().Split(new string[] { "::" }, StringSplitOptions.None));
            //string numeroPartida = unidadNumeroPartida[0];
            //string idUnidad = unidadNumeroPartida[1];

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "La unidad fue borrada con exito" + "');", true);
        }

    }
}