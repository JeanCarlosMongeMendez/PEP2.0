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
    public partial class ElegirEjecucion : System.Web.UI.Page
    {
        #region variables globales
        readonly PagedDataSource pgsource = new PagedDataSource();
        ProyectoServicios proyectoServicios;
        PeriodoServicios periodoServicios;
        EjecucionServicios ejecucionServicios;
      
        ArchivoEjecucionServicios archivoEjecucionServicios;
        private int elmentosMostrar = 10;
        int primerIndex4, ultimoIndex4;
        #endregion
        #region Paginacion
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

        protected void lbPrimero4_Click(object sender, EventArgs e)
        {
            paginaActual4 = 0;
            MostrarDatosTablaUnidad();
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

            DataList2.DataSource = dt;
            DataList2.DataBind();
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
            if (!e.CommandName.Equals("nuevaPagina4")) return;
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
        #endregion
        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            this.proyectoServicios = new ProyectoServicios();
            this.periodoServicios = new PeriodoServicios();
            this.ejecucionServicios = new EjecucionServicios();
            this.archivoEjecucionServicios = new ArchivoEjecucionServicios();
            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                PeriodosDDL.Items.Clear();
                ProyectosDDL.Items.Clear();
                CargarPeriodos();
                Session["listaUnidad"] = null;
                Session["listaPartida"] = null;
                Session["listaMontoPartidaDisponible"] = null;
                Session["periodo"] = null;
                Session["proyecto"] = null;
                Session["tipoTramite"] = null;
                Session["idTipoTramite"] = null;
                Session["numeroReferencia"] = null;
                Session["monto"] = null;
                Session["nuevaEjecucion"] = null;
                Session["idEjecucion"] = null;
                Session["verEjecucion"] = null;
                Session["listaArchivoEjecucion"] = null;
                Session["descripcionEjecionOtro"] = null;
                MostrarDatosTablaUnidad();
                //DDLTipoTramite.Items.Clear();
                //ddlPartida.Items.Clear();
            }
            else
            {
                paginaActual4 = 0;
                MostrarDatosTablaUnidad();
            }



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
            int idEjecucion;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton btnEditar = e.Item.FindControl("btnEditarEjecucion") as LinkButton;

                //Entidades.Ejecucion ejecucion = new Entidades.Ejecucion();
              idEjecucion= Convert.ToInt32(btnEditar.CommandArgument.ToString());
                int Estado= ejecucionServicios.EstadoEjecucion(idEjecucion);

                if (Estado == 2)
                {
                    btnEditar.Visible = false;
                }
                else
                {
                    btnEditar.Visible = true;
                }

               

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
        protected void Proyectos_OnChanged(object sender, EventArgs e)
        {

            MostrarDatosTablaUnidad();
        }
        /// <summary>
        /// kevin Picado Quesada
        /// Método para Editar una ejecucion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditarEjecucion_OnChanged(object sender, EventArgs e)
        {

            string idEjecucion = ((LinkButton)(sender)).CommandArgument.ToString();
            List<Entidades.Ejecucion> listaEjecucion = new List<Entidades.Ejecucion>();
            Session["nuevaEjecucion"] = 0;
            Session["verEjecucion"] = 1;
            Session["listaUnidad"] = ejecucionServicios.ConsultarUnidadEjecucion(Convert.ToInt32(idEjecucion));
            Session["listaPartida"] = ejecucionServicios.ConsultarPartidaEjecucion(Convert.ToInt32(idEjecucion));
            List<Entidades.ArchivoEjecucion> eje = new List<Entidades.ArchivoEjecucion>();
            Session["listaArchivoEjecucion"] = archivoEjecucionServicios.obtenerArchivoEjecucion(Convert.ToInt32(idEjecucion));
            listaEjecucion = ejecucionServicios.ConsultarEjecucion(PeriodosDDL.SelectedValue, ProyectosDDL.SelectedValue);
            int idTipoTramite = listaEjecucion.Where(item => item.idEjecucion == Convert.ToInt32(idEjecucion)).ToList().First().idTipoTramite.idTramite;
            Session["listaMontoPartidaDisponible"] = ejecucionServicios.ConsultarEjecucionMontoPartida(Convert.ToInt32(idEjecucion));
            Session["periodo"] = Convert.ToString(PeriodosDDL.SelectedValue);
            Session["proyecto"] = Convert.ToString(ProyectosDDL.SelectedValue);
            Session["idTipoTramite"] = idTipoTramite;
            Session["numeroReferencia"] = listaEjecucion.Where(item => item.idEjecucion == Convert.ToInt32(idEjecucion)).ToList().First().numeroReferencia;
            Session["monto"] = listaEjecucion.Where(item => item.idEjecucion == Convert.ToInt32(idEjecucion)).ToList().First().monto;
            Session["idEjecucion"] = Convert.ToInt32(idEjecucion);
            Session["descripcionEjecionOtro"] = listaEjecucion.Where(item => item.idEjecucion == Convert.ToInt32(idEjecucion)).ToList().First().descripcionEjecucionOtro;
            String url = Page.ResolveUrl("~/Catalogos/Ejecucion/AdministrarEjecucion.aspx");
            Response.Redirect(url);

        }
        /// <summary>
        /// kevin Picado Quesada
        /// Método para crear una nueva ejecucion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NuevaEjecucion_OnChanged(object sender, EventArgs e)
        {
            Session["nuevaEjecucion"] = 1;
            String url = Page.ResolveUrl("~/Catalogos/Ejecucion/AdministrarEjecucion.aspx");
            Response.Redirect(url);
        }
        /// <summary>
        /// kevin Picado Quesada
        /// Método para ver una ejecucion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void VerEjecucion_OnChanged(object sender, EventArgs e)
        {
            Session["verEjecucion"] = 0;
            Session["nuevaEjecucion"] = 0;
            string idEjecucion = ((LinkButton)(sender)).CommandArgument.ToString();

            List<Entidades.Ejecucion> listaEjecucion = new List<Entidades.Ejecucion>();
            Session["listaArchivoEjecucion"] = archivoEjecucionServicios.obtenerArchivoEjecucion(Convert.ToInt32(idEjecucion));
            Session["listaUnidad"] = ejecucionServicios.ConsultarUnidadEjecucion(Convert.ToInt32(idEjecucion));
            Session["listaPartida"] = ejecucionServicios.ConsultarPartidaEjecucion(Convert.ToInt32(idEjecucion));
  
            listaEjecucion = ejecucionServicios.ConsultarEjecucion(PeriodosDDL.SelectedValue, ProyectosDDL.SelectedValue);
            int idTipoTramite = listaEjecucion.Where(item => item.idEjecucion == Convert.ToInt32(idEjecucion)).ToList().First().idTipoTramite.idTramite;
            Session["listaMontoPartidaDisponible"] = ejecucionServicios.ConsultarEjecucionMontoPartida(Convert.ToInt32(idEjecucion));
            Session["periodo"] = Convert.ToString(PeriodosDDL.SelectedValue);
            Session["proyecto"] = Convert.ToString(ProyectosDDL.SelectedValue);
            Session["idTipoTramite"] = idTipoTramite;
            Session["idEstado"] = listaEjecucion.Where(item => item.idEjecucion == Convert.ToInt32(idEjecucion)).ToList().First().idestado.descripcion;
            Session["numeroReferencia"] = listaEjecucion.Where(item => item.idEjecucion == Convert.ToInt32(idEjecucion)).ToList().First().numeroReferencia;
            Session["monto"] = listaEjecucion.Where(item => item.idEjecucion == Convert.ToInt32(idEjecucion)).ToList().First().monto;
            Session["idEjecucion"] = Convert.ToInt32(idEjecucion);
            Session["descripcionEjecionOtro"] = listaEjecucion.Where(item => item.idEjecucion == Convert.ToInt32(idEjecucion)).ToList().First().descripcionEjecucionOtro;
            String url = Page.ResolveUrl("~/Catalogos/Ejecucion/AdministrarEjecucion.aspx");
            Response.Redirect(url);
        }

        #endregion

        #region logica 
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
            ViewState["TotalPaginas4"] = pgsource.PageCount;
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

            Paginacion4();

        }





        #endregion

    }
}

