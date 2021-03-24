using Entidades;
using PEP;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proyecto.Catalogos.Ejecucion
{
    public partial class EliminarEjecucion : System.Web.UI.Page
    {
        #region variables globales
        ProyectoServicios proyectoServicios = new ProyectoServicios();
        TipoTramiteServicios tipoTramiteServicios = new TipoTramiteServicios();
        EjecucionUnidadParitdaServicios ejecucionUnidadParitdaServicios = new EjecucionUnidadParitdaServicios();
        EjecucionServicios ejecucionServicios = new EjecucionServicios();
        ArchivoEjecucionServicios archivoEjecucionServicios = new ArchivoEjecucionServicios();
        #endregion

        #region paginacion
        readonly PagedDataSource pgsource = new PagedDataSource();
        int primerIndex, ultimoIndex, primerIndex2, ultimoIndex2, primerIndex4, ultimoIndex4;
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
                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(Session["periodoSeleccionado"]);

                Proyectos proyecto = new Proyectos();
                proyecto.idProyecto = Convert.ToInt32(Session["proyectoSeleccionado"]);
                proyecto = proyectoServicios.ObtenerPorId(proyecto.idProyecto);

                lblPeriodo.Text = periodo.anoPeriodo.ToString();
                lblProyecto.Text = proyecto.nombreProyecto;

                Entidades.Ejecucion ejecucion = (Entidades.Ejecucion)Session["ejecucionEliminar"];

                lblNumeroEjecucion.Text = ejecucion.idEjecucion.ToString();

                List<Unidad> listaUnidadesAsociadas = ejecucionUnidadParitdaServicios.getUnidadesPorEjecucion(ejecucion);

                Session["listaUnidadesAsociadas"] = listaUnidadesAsociadas;
                mostrarDatosTablaUnidadesAsociadas();

                List<Partida> listaPartidasAsociadas = ejecucionUnidadParitdaServicios.getPartidasPorEjecucion(ejecucion);

                Session["listaPartidasAsociadas"] = listaPartidasAsociadas;
                mostrarDatosTablaPartidasAsociadas();

                llenarDdlTipoTramites();

                int contIndex = 0;

                foreach (ListItem item in ddlTipoTramite.Items)
                {
                    if (Convert.ToInt32(item.Value) == ejecucion.tipoTramite.idTramite)
                    {
                        ddlTipoTramite.SelectedIndex = contIndex;
                        break;
                    }
                    contIndex++;
                }

                txtNumeroReferencia.Text = ejecucion.numeroReferencia;
                txtMonto.Text = ejecucion.monto.ToString();

                Session["listaPartidaUnidadAsociadas"] = ejecucionUnidadParitdaServicios.getUnidadesPartidasMontoPorEjecucion(ejecucion);
                mostrarDatosTablaUnidadPartidaAsociadas();

                Session["archivo"] = null;
                Session["listaArchivosAsociados"] = archivoEjecucionServicios.obtenerArchivoEjecucion(ejecucion.idEjecucion);
                cargarArchivos();
            }
        }
        #endregion

        #region logica

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: descarga el archivo para que el usuario lo pueda ver
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        private void descargar(string fileName, Byte[] file)
        {
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.BinaryWrite(file);
            Response.Flush();
            Response.End();

        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: Crea una tabla donde se muestran los archivos asociados
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        public void cargarArchivos()
        {
            List<ArchivoEjecucion> listaArchivos = (List<ArchivoEjecucion>)Session["listaArchivosAsociados"];

            if (listaArchivos.Count == 0)
            {
                txtArchivos.Text = "No hay archivos asociados a esta muestra";
                txtArchivos.Visible = true;
                rpArchivos.Visible = false;
            }
            else
            {
                txtArchivos.Visible = false;
                rpArchivos.Visible = true;
            }

            rpArchivos.DataSource = listaArchivos;
            rpArchivos.DataBind();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: llena el DropDownList de tramites
        /// Requiere: proyecto 
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        public void llenarDdlTipoTramites()
        {
            Proyectos proyecto = new Proyectos();
            proyecto.idProyecto = Convert.ToInt32(Session["proyectoSeleccionado"]);
            proyecto = proyectoServicios.ObtenerPorId(proyecto.idProyecto);

            List<TipoTramite> listaTipoTramites = tipoTramiteServicios.obtenerTipoTramitesPorProyecto(proyecto);

            ddlTipoTramite.DataSource = listaTipoTramites;
            ddlTipoTramite.DataTextField = "nombreTramite";
            ddlTipoTramite.DataValueField = "idTramite";

            ddlTipoTramite.DataBind();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTablaUnidadesAsociadas()
        {
            List<Unidad> listaUnidades = (List<Unidad>)Session["listaUnidadesAsociadas"];

            String unidad = "";

            if (!String.IsNullOrEmpty(txtBuscarUnidad.Text))
            {
                unidad = txtBuscarUnidad.Text;
            }

            List<Unidad> listaUnidadesFiltrada = (List<Unidad>)listaUnidades.Where(unid => unid.nombreUnidad.ToUpper().Contains(unidad.ToUpper())).ToList();
            var dt = listaUnidadesFiltrada;
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

            rpUnidadesAsociadas.DataSource = pgsource;
            rpUnidadesAsociadas.DataBind();

            //metodo que realiza la paginacion
            Paginacion();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTablaPartidasAsociadas()
        {
            List<Partida> listaPartidasAsociadas = (List<Partida>)Session["listaPartidasAsociadas"];

            String partida = "", descripcion = "";

            if (!String.IsNullOrEmpty(txtBuscarNumeroPartida.Text))
            {
                partida = txtBuscarNumeroPartida.Text;
            }

            if (!String.IsNullOrEmpty(txtBuscarDescripcionPartida.Text))
            {
                descripcion = txtBuscarDescripcionPartida.Text;
            }

            List<Partida> listaPartidasFiltrada = (List<Partida>)listaPartidasAsociadas.Where(parti => parti.numeroPartida.ToUpper().Contains(partida.ToUpper()) &&
            parti.descripcionPartida.ToUpper().Contains(descripcion.ToUpper())).ToList();
            var dt = listaPartidasFiltrada;
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

            rpPartidasAsociadas.DataSource = pgsource;
            rpPartidasAsociadas.DataBind();

            //metodo que realiza la paginacion
            Paginacion2();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTablaUnidadPartidaAsociadas()
        {
            List<PartidaUnidad> listaPartidaUnidadAsociadas = (List<PartidaUnidad>)Session["listaPartidaUnidadAsociadas"];

            String partida = "", unidad = "";

            if (!String.IsNullOrEmpty(txtBuscarUnidadPartidaUnidadAsociada.Text))
            {
                unidad = txtBuscarUnidadPartidaUnidadAsociada.Text;
            }

            if (!String.IsNullOrEmpty(txtBuscarPartidaPartidaUnidadAsociada.Text))
            {
                partida = txtBuscarPartidaPartidaUnidadAsociada.Text;
            }

            List<PartidaUnidad> listaPartidaUnidadSinAsociarFiltrada = (List<PartidaUnidad>)listaPartidaUnidadAsociadas.Where(parti => parti.numeroPartida.ToUpper().Contains(partida.ToUpper()) &&
            parti.nombreUnidad.ToUpper().Contains(unidad.ToUpper())).ToList();
            var dt = listaPartidaUnidadSinAsociarFiltrada;
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

            rpPartidaUnidadAsociadas.DataSource = pgsource;
            rpPartidaUnidadAsociadas.DataBind();

            //metodo que realiza la paginacion
            Paginacion4();
        }
        #endregion

        #region paginacion
        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
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
        /// 22/mar/2021
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
            mostrarDatosTablaUnidadesAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
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
            mostrarDatosTablaUnidadesAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
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
            mostrarDatosTablaUnidadesAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
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
            mostrarDatosTablaUnidadesAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
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
            mostrarDatosTablaUnidadesAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
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
        /// 22/mar/2021
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
        /// 22/mar/2021
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
            mostrarDatosTablaPartidasAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo2_Click(object sender, EventArgs e)
        {
            paginaActual2 = (Convert.ToInt32(ViewState["TotalPaginas2"]) - 1);
            mostrarDatosTablaPartidasAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
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
            mostrarDatosTablaPartidasAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
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
            mostrarDatosTablaPartidasAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
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
            mostrarDatosTablaPartidasAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
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
        /// 22/mar/2021
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

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: se devuelve a la primera pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Primer pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrimero4_Click(object sender, EventArgs e)
        {
            paginaActual4 = 0;
            mostrarDatosTablaUnidadPartidaAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo4_Click(object sender, EventArgs e)
        {
            paginaActual4 = (Convert.ToInt32(ViewState["TotalPaginas4"]) - 1);
            mostrarDatosTablaUnidadPartidaAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: se devuelve a la pagina anterior y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Anterior pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAnterior4_Click(object sender, EventArgs e)
        {
            paginaActual4 -= 1;
            mostrarDatosTablaUnidadPartidaAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: se devuelve a la pagina siguiente y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Siguiente pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSiguiente4_Click(object sender, EventArgs e)
        {
            paginaActual4 += 1;
            mostrarDatosTablaUnidadPartidaAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
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
            mostrarDatosTablaUnidadPartidaAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
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

        #region eventos 

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: filtra la tabla de unidades asociadas segun los datos ingresados en los filtros
        /// Requiere: ingresar datos en los filtros y dar boton de enter
        /// Modifica: datos de tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBuscarUnidad_TextChanged(object sender, EventArgs e)
        {
            paginaActual = 0;
            mostrarDatosTablaUnidadesAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: filtra la tabla de partidas asociadas 
        /// Requiere: ingresar datos en filtros
        /// Modifica: datos de la tabla de partidas asocidas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBuscarPartidasAsocidas_TextChanged(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            mostrarDatosTablaPartidasAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: filtra la tabla de unidad partidas asociadas
        /// Requiere: ingresar datos en filtros
        /// Modifica: datos de la tabla de unidades  partidas asociadas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBuscarPartidaUnidadAsociada_TextChanged(object sender, EventArgs e)
        {
            paginaActual4 = 0;
            mostrarDatosTablaUnidadPartidaAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: redirecciona a la pantalla de Administrar Ejecuciones
        /// Requiere: dar clic en cancelar
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/Ejecucion/AdministrarEjecuciones.aspx");
            Response.Redirect(url);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: descarga el archivo para que el usuario lo pueda ver
        /// Requiere: clic en el archivo
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVerArchivo_Click(object sender, EventArgs e)
        {
            String[] infoArchivo = (((LinkButton)(sender)).CommandArgument).ToString().Split(',');
            String nombreArchivo = infoArchivo[1];
            String rutaArchivo = infoArchivo[2];

            FileStream fileStream = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            Byte[] blobValue = binaryReader.ReadBytes(Convert.ToInt32(fileStream.Length));

            fileStream.Close();
            binaryReader.Close();

            descargar(nombreArchivo, blobValue);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/mar/2021
        /// Efecto: elimina la ejecucion seleccionada
        /// Requiere: dar clic en el boton de Eliminar e ingresar datos 
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {

            Entidades.Ejecucion ejecucion = (Entidades.Ejecucion)Session["ejecucionEliminar"];
            
            ejecucionUnidadParitdaServicios.eliminarEjecucionUnidadPartidaPorEjecucion(ejecucion.idEjecucion);

            List<ArchivoEjecucion> listaArchivos = (List<ArchivoEjecucion>)Session["listaArchivosAsociados"];

            foreach (ArchivoEjecucion archivoEjecucion in listaArchivos)
            {
                String ruta = archivoEjecucion.rutaArchivo;
                if (System.IO.File.Exists(@ruta))
                {
                    try
                    {
                        System.IO.File.Delete(@ruta);

                    }
                    catch (Exception ex)
                    {
                        //(this.Master as SiteMaster).Mensaje("No se pudo eliminar el archivo", "¡Alerta!");
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "No se pudo eliminar el archivo" + "');", true);
                    }
                }
                archivoEjecucionServicios.eliminarArchivoEjecucionPorId(archivoEjecucion.idArchivoEjecucion);
            }

            ejecucionServicios.EliminarEjecucion(ejecucion.idEjecucion);

            String url = Page.ResolveUrl("~/Catalogos/Ejecucion/AdministrarEjecuciones.aspx");
            Response.Redirect(url);

        }
        #endregion

    }
}