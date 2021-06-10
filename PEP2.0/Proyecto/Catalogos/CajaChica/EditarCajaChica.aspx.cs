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

namespace Proyecto.Catalogos.CajaChica
{
    public partial class EditarCajaChica : System.Web.UI.Page
    {
        #region variables globales
        ProyectoServicios proyectoServicios = new ProyectoServicios();
        UnidadServicios unidadServicios = new UnidadServicios();
        PartidaServicios partidaServicios = new PartidaServicios();
        CajaChicaUnidadPartidaServicios cajaChicaUnidadPartidaServicios = new CajaChicaUnidadPartidaServicios();
        EstadoCajaChicaServicios estadoCajaChicaServicios = new EstadoCajaChicaServicios();
        CajaChicaServicios cajaChicaServicios = new CajaChicaServicios();
        #endregion

        #region paginacion
        readonly PagedDataSource pgsource = new PagedDataSource();
        int primerIndex, ultimoIndex, primerIndex1, ultimoIndex1, primerIndex2, ultimoIndex2, primerIndex3, ultimoIndex3, primerIndex4, ultimoIndex4, primerIndex5, ultimoIndex5;
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
        private int paginaActual1
        {
            get
            {
                if (ViewState["paginaActual1"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["paginaActual1"]);
            }
            set
            {
                ViewState["paginaActual1"] = value;
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

                Entidades.CajaChica cajaChica = (Entidades.CajaChica)Session["ejecucionEditar"];

                lblNumeroCajaChica.Text = cajaChica.numeroCajaChica.ToString();

                List<Unidad> listaUnidadesAsociadas = cajaChicaUnidadPartidaServicios.getUnidadesPorCajaChica(cajaChica);

                List<Unidad> listaUnidadesSinAsociar = unidadServicios.ObtenerPorProyecto(proyecto.idProyecto);

                ///quitar de las unidades sin asociar las unidades ya asociadas
                foreach (Unidad unidad in listaUnidadesAsociadas)
                {
                    Unidad unidadTemp = (Unidad)listaUnidadesSinAsociar.Where(unid => unid.idUnidad == unidad.idUnidad).ToList().First();

                    listaUnidadesSinAsociar.Remove(unidadTemp);
                }

                Session["listaUnidadesAsociadas"] = listaUnidadesAsociadas;
                mostrarDatosTablaUnidadesAsociadas();
                Session["listaUnidadesSinAsociadas"] = listaUnidadesSinAsociar;
                mostrarDatosTablaUnidadesSinAsociadas();

                List<Partida> listaPartidasAsociadas = cajaChicaUnidadPartidaServicios.getPartidasPorCajaChica(cajaChica);

                Session["listaPartidasAsociadas"] = listaPartidasAsociadas;
                mostrarDatosTablaPartidasAsociadas();



                txtComentario.Text = cajaChica.comentario;
                txtMonto.Text = cajaChica.monto.ToString();

                Session["listaPartidaUnidadAsociadas"] = cajaChicaUnidadPartidaServicios.getUnidadesPartidasMontoPorCajaChica(cajaChica);
                mostrarDatosTablaUnidadPartidaAsociadas();

                Session["listaPartidaUnidadSinAsociar"] = new List<PartidaUnidad>();


            }


        }
        #endregion

        #region logica


        /// <summary>
        /// Leonardo Carrion
        /// 29/sep/2019
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
        /// 25/feb/2021
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTablaUnidadesSinAsociadas()
        {
            List<Unidad> listaUnidadesSinAsociar = (List<Unidad>)Session["listaUnidadesSinAsociadas"];

            String unidad = "";

            if (!String.IsNullOrEmpty(txtBuscarUnidadSinAsociar.Text))
            {
                unidad = txtBuscarUnidadSinAsociar.Text;
            }

            List<Unidad> listaUnidadesFiltrada = (List<Unidad>)listaUnidadesSinAsociar.Where(unid => unid.nombreUnidad.ToUpper().Contains(unidad.ToUpper())).ToList();
            var dt = listaUnidadesFiltrada;
            pgsource.DataSource = dt;
            pgsource.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual1;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas1"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina1.Text = "Página " + (paginaActual1 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior1.Enabled = !pgsource.IsFirstPage;
            lbSiguiente1.Enabled = !pgsource.IsLastPage;
            lbPrimero1.Enabled = !pgsource.IsFirstPage;
            lbUltimo1.Enabled = !pgsource.IsLastPage;

            rpUnidadesSinAsociar.DataSource = pgsource;
            rpUnidadesSinAsociar.DataBind();

            //metodo que realiza la paginacion
            Paginacion1();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 26/feb/2021
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
        /// 01/mar/2021
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTablaPartidasSinAsociar()
        {
            List<Partida> listaPartidasSinAsociar = (List<Partida>)Session["listaPartidasSinAsociar"];

            String partida = "", descripcion = "";

            if (!String.IsNullOrEmpty(txtBuscarNumeroPartidaSinAsociar.Text))
            {
                partida = txtBuscarNumeroPartidaSinAsociar.Text;
            }

            if (!String.IsNullOrEmpty(txtBuscarDescripcionPartidaSinAsociar.Text))
            {
                descripcion = txtBuscarDescripcionPartidaSinAsociar.Text;
            }

            List<Partida> listaPartidasFiltrada = (List<Partida>)listaPartidasSinAsociar.Where(parti => parti.numeroPartida.ToUpper().Contains(partida.ToUpper()) &&
            parti.descripcionPartida.ToUpper().Contains(descripcion.ToUpper())).ToList();
            var dt = listaPartidasFiltrada;
            pgsource.DataSource = dt;
            pgsource.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual3;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas3"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina3.Text = "Página " + (paginaActual3 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior3.Enabled = !pgsource.IsFirstPage;
            lbSiguiente3.Enabled = !pgsource.IsLastPage;
            lbPrimero3.Enabled = !pgsource.IsFirstPage;
            lbUltimo3.Enabled = !pgsource.IsLastPage;

            rpPartidasSinAsociar.DataSource = pgsource;
            rpPartidasSinAsociar.DataBind();

            //metodo que realiza la paginacion
            Paginacion3();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/mar/2021
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

        /// <summary>
        /// Leonardo Carrion
        /// 05/mar/2021
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTablaUnidadPartidaSinAsociar()
        {
            List<PartidaUnidad> listaPartidaUnidadSinAsociar = (List<PartidaUnidad>)Session["listaPartidaUnidadSinAsociar"];

            var dt = listaPartidaUnidadSinAsociar;
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

            rpPartidaUnidadSinAsociar.DataSource = pgsource;
            rpPartidaUnidadSinAsociar.DataBind();

            //metodo que realiza la paginacion
            Paginacion5();
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
            mostrarDatosTablaUnidadesAsociadas();
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
            mostrarDatosTablaUnidadesAsociadas();
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
            mostrarDatosTablaUnidadesAsociadas();
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
            mostrarDatosTablaUnidadesAsociadas();
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
            mostrarDatosTablaUnidadesAsociadas();
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

        /// <summary>
        /// Leonardo Carrion
        /// 25/feb/2021
        /// Efecto: realiza la paginacion
        /// Requiere: -
        /// Modifica: paginacion mostrada en pantalla
        /// Devuelve: -
        /// </summary>
        private void Paginacion1()
        {
            var dt = new DataTable();
            dt.Columns.Add("IndexPagina"); //Inicia en 0
            dt.Columns.Add("PaginaText"); //Inicia en 1

            primerIndex1 = paginaActual1 - 2;
            if (paginaActual1 > 2)
                ultimoIndex1 = paginaActual1 + 2;
            else
                ultimoIndex1 = 4;

            //se revisa que la ultima pagina sea menor que el total de paginas a mostrar, sino se resta para que muestre bien la paginacion
            if (ultimoIndex1 > Convert.ToInt32(ViewState["TotalPaginas1"]))
            {
                ultimoIndex1 = Convert.ToInt32(ViewState["TotalPaginas1"]);
                primerIndex1 = ultimoIndex1 - 4;
            }

            if (primerIndex1 < 0)
                primerIndex1 = 0;

            //se crea el numero de paginas basado en la primera y ultima pagina
            for (var i = primerIndex1; i < ultimoIndex1; i++)
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
        /// 25/feb/2021
        /// Efecto: se devuelve a la primera pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Primer pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrimero1_Click(object sender, EventArgs e)
        {
            paginaActual1 = 0;
            mostrarDatosTablaUnidadesSinAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 25/feb/2021
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo1_Click(object sender, EventArgs e)
        {
            paginaActual1 = (Convert.ToInt32(ViewState["TotalPaginas1"]) - 1);
            mostrarDatosTablaUnidadesSinAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 25/feb/2021
        /// Efecto: se devuelve a la pagina anterior y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Anterior pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAnterior1_Click(object sender, EventArgs e)
        {
            paginaActual1 -= 1;
            mostrarDatosTablaUnidadesSinAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 25/feb/2021
        /// Efecto: se devuelve a la pagina siguiente y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Siguiente pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSiguiente1_Click(object sender, EventArgs e)
        {
            paginaActual1 += 1;
            mostrarDatosTablaUnidadesSinAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 25/feb/2021
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
            paginaActual1 = Convert.ToInt32(e.CommandArgument.ToString());
            mostrarDatosTablaUnidadesSinAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 25/feb/2021
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
            if (lnkPagina.CommandArgument != paginaActual1.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }

        /// <summary>
        /// Leonardo Carrion
        /// 26/feb/2021
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
        /// 26/feb/2021
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
        /// 26/feb/2021
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
        /// 26/feb/2021
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
        /// 26/feb/2021
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
        /// 26/feb/2021
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
        /// 26/feb/2021
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
        /// 01/mar/2021
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
        /// 01/mar/2021
        /// Efecto: se devuelve a la primera pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Primer pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrimero3_Click(object sender, EventArgs e)
        {
            paginaActual3 = 0;
            mostrarDatosTablaPartidasSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/mar/2021
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
            mostrarDatosTablaPartidasSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/mar/2021
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
            mostrarDatosTablaPartidasSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/mar/2021
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
            mostrarDatosTablaPartidasSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/mar/2021
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
            mostrarDatosTablaPartidasSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/mar/2021
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
        /// 03/mar/2021
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
        /// 03/mar/2021
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
        /// 03/mar/2021
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
        /// 03/mar/2021
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
        /// 03/mar/2021
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
        /// 03/mar/2021
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
        /// 03/mar/2021
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
        /// Leonardo Carrion
        /// 05/mar/2021
        /// Efecto: realiza la paginacion
        /// Requiere: -
        /// Modifica: paginacion mostrada en pantalla
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
        /// 05/mar/2021
        /// Efecto: se devuelve a la primera pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Primer pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrimero5_Click(object sender, EventArgs e)
        {
            paginaActual5 = 0;
            mostrarDatosTablaUnidadPartidaSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 05/mar/2021
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo5_Click(object sender, EventArgs e)
        {
            paginaActual5 = (Convert.ToInt32(ViewState["TotalPaginas5"]) - 1);
            mostrarDatosTablaUnidadPartidaSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 05/mar/2021
        /// Efecto: se devuelve a la pagina anterior y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Anterior pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAnterior5_Click(object sender, EventArgs e)
        {
            paginaActual5 -= 1;
            mostrarDatosTablaUnidadPartidaSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 05/mar/2021
        /// Efecto: se devuelve a la pagina siguiente y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Siguiente pagina"
        /// Modifica: elementos mostrados en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSiguiente5_Click(object sender, EventArgs e)
        {
            paginaActual5 += 1;
            mostrarDatosTablaUnidadPartidaSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 05/mar/2021
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
            mostrarDatosTablaUnidadPartidaSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 05/mar/2021
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
        #endregion

        #region eventos 
        /// <summary>
        /// Leonardo Carrion
        /// 24/feb/2021
        /// Efecto: levanta modal para elegir las unidades
        /// Requiere: dar clic en el boton "Asociar Unidades"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAsociarUnidades_Click(object sender, EventArgs e)
        {
            mostrarDatosTablaUnidadesSinAsociadas();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalUnidades();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 26/feb/2021
        /// Efecto: selecciona la unidad y la pone en la tabla de unidades seleccionadas
        /// Requiere: dar clic el en boton seleccionar
        /// Modifica: datos de la tabla de unidades seleccionadas y tabla de unidades sin seleccionar
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            int idUnidad = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            List<Unidad> listaUnidadesAsociadas = (List<Unidad>)Session["listaUnidadesAsociadas"];
            List<Unidad> listaUnidadesSinAsociadas = (List<Unidad>)Session["listaUnidadesSinAsociadas"];

            Unidad unidad = (Unidad)listaUnidadesSinAsociadas.Where(unid => unid.idUnidad == idUnidad).ToList().First();
            listaUnidadesAsociadas.Add(unidad);

            listaUnidadesSinAsociadas.Remove(unidad);

            Session["listaUnidadesAsociadas"] = listaUnidadesAsociadas;
            Session["listaUnidadesSinAsociadas"] = listaUnidadesSinAsociadas;

            mostrarDatosTablaUnidadesAsociadas();
            mostrarDatosTablaUnidadesSinAsociadas();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se  asocio " + unidad.nombreUnidad + "');", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 26/feb/2021
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
        /// 26/feb/2021
        /// Efecto: filtra la tabla de unidades sin asociar segun los datos ingresados en los filtros
        /// Requiere: ingresar datos en los filtros y dar boton de enter
        /// Modifica: datos de tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBuscarUnidadSinAsociar_TextChanged(object sender, EventArgs e)
        {
            paginaActual1 = 0;
            mostrarDatosTablaUnidadesSinAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 26/feb/2021
        /// Efecto: desasocia la unidad seleccionada
        /// Requiere: dar clic al boton de desasociar
        /// Modifica: tabla de unidades asociadas y tabla de unidades sin asociar
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDesasociarUnidad_Click(object sender, EventArgs e)
        {
            int idUnidad = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            List<Unidad> listaUnidadesAsociadas = (List<Unidad>)Session["listaUnidadesAsociadas"];
            List<Unidad> listaUnidadesSinAsociadas = (List<Unidad>)Session["listaUnidadesSinAsociadas"];

            Unidad unidad = (Unidad)listaUnidadesAsociadas.Where(unid => unid.idUnidad == idUnidad).ToList().First();
            listaUnidadesSinAsociadas.Add(unidad);

            listaUnidadesAsociadas.Remove(unidad);

            Session["listaUnidadesAsociadas"] = listaUnidadesAsociadas;
            Session["listaUnidadesSinAsociadas"] = listaUnidadesSinAsociadas;

            mostrarDatosTablaUnidadesAsociadas();
            mostrarDatosTablaUnidadesSinAsociadas();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se  quito " + unidad.nombreUnidad + " de las unidades asociadas" + "');", true);

            Session["listaPartidasAsociadas"] = new List<Partida>();
            mostrarDatosTablaPartidasAsociadas();

            Session["listaPartidaUnidadAsociadas"] = new List<PartidaUnidad>();
            mostrarDatosTablaUnidadPartidaAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/feb/2021
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
        /// 01/mar/2021
        /// Efecto: filtra la tabla de partidas sin asociar
        /// Requiere: ingresar datos en filtros
        /// Modifica: datos de la tabla de partidas sin asociar
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBuscarPartidaSinAsociar_TextChanged(object sender, EventArgs e)
        {
            paginaActual3 = 0;
            mostrarDatosTablaPartidasSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/mar/2021
        /// Efecto: levanta un modal para poder asociar las partidas deseadas
        /// Requiere: dar clic en el boton de Asociar Partidas
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAsociarPartidas_Click(object sender, EventArgs e)
        {
            Proyectos proyecto = new Proyectos();
            proyecto.idProyecto = Convert.ToInt32(Session["proyectoSeleccionado"]);
            proyecto = proyectoServicios.ObtenerPorId(proyecto.idProyecto);

            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(Session["periodoSeleccionado"]);

            List<Unidad> listaUnidadesAsociadas = (List<Unidad>)Session["listaUnidadesAsociadas"];

            List<int> listaIdUnidades = (List<int>)listaUnidadesAsociadas.Select(unidad => unidad.idUnidad).ToList();

            List<Partida> listaPartidasSinAsociar = new List<Partida>();
            List<Partida> listaPartidasSinAsociarTemp = new List<Partida>();

            listaPartidasSinAsociarTemp = partidaServicios.ObtienePartidaPorPeriodoUnidadProyecto(proyecto.idProyecto, listaIdUnidades, periodo.anoPeriodo);

            List<Partida> listaPartidasAsociadas = (List<Partida>)Session["listaPartidasAsociadas"];

            foreach (Partida partida in listaPartidasSinAsociarTemp)
            {
                List<Partida> partidas = listaPartidasAsociadas.Where(pa => pa.idPartida == partida.idPartida).ToList();
                if (partidas.Count == 0)
                {
                    listaPartidasSinAsociar.Add(partida);
                }
            }

            Session["listaPartidasSinAsociar"] = listaPartidasSinAsociar;
            mostrarDatosTablaPartidasSinAsociar();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPartidas();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 02/mar/2021
        /// Efecto: agrega en la tabla de partidas asociadas la partida seleccionada
        /// Requiere: dar clic en el boton de seleccionar
        /// Modifica: tabla de partidas asociadas y sin asociar
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSeleccionarPartida_Click(object sender, EventArgs e)
        {
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            List<Partida> listaPartidasAsociadas = (List<Partida>)Session["listaPartidasAsociadas"];
            List<Partida> listaPartidasSinAsociar = (List<Partida>)Session["listaPartidasSinAsociar"];

            Partida partida = (Partida)listaPartidasSinAsociar.Where(part => part.idPartida == idPartida).ToList().First();
            listaPartidasAsociadas.Add(partida);

            listaPartidasSinAsociar.Remove(partida);

            Session["listaPartidasAsociadas"] = listaPartidasAsociadas;
            Session["listaPartidasSinAsociar"] = listaPartidasSinAsociar;

            mostrarDatosTablaPartidasAsociadas();
            mostrarDatosTablaPartidasSinAsociar();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se  asocio " + partida.descripcionPartida + "');", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 02/mar/2021
        /// Efecto: desasocia la partida seleccionada
        /// Requiere: dar clic en el boton de desasociar
        /// Modifica: tabla de partidas asociadas y tabla de partidas sin asociar
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDesasociarPartida_Click(object sender, EventArgs e)
        {
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            List<Partida> listaPartidasAsociadas = (List<Partida>)Session["listaPartidasAsociadas"];
            List<Partida> listaPartidasSinAsociar = (List<Partida>)Session["listaPartidasSinAsociar"];

            Partida partida = (Partida)listaPartidasAsociadas.Where(part => part.idPartida == idPartida).ToList().First();
            listaPartidasSinAsociar.Add(partida);

            listaPartidasAsociadas.Remove(partida);

            Session["listaPartidasAsociadas"] = listaPartidasAsociadas;
            Session["listaPartidasSinAsociar"] = listaPartidasSinAsociar;

            mostrarDatosTablaPartidasAsociadas();
            mostrarDatosTablaPartidasSinAsociar();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se  quito " + partida.descripcionPartida + " de las partidas asociadas" + "');", true);

            Session["listaPartidaUnidadAsociadas"] = new List<PartidaUnidad>();
            mostrarDatosTablaUnidadPartidaAsociadas();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 03/mar/2021
        /// Efecto: levanta el modal para repartir los gastos entre las unidades seleccionadas
        /// Requiere: dar clic en el boton de Repartir Gastos, ingresar monto y seleccionar unidades y partidas
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRepartirGastos_Click(object sender, EventArgs e)
        {
            Double.TryParse(txtMonto.Text, out Double monto);

            if (monto <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Favor ingresar el monto" + "');", true);
            }
            else
            {

                List<PartidaUnidad> listaPartidaUnidadAsociadas = (List<PartidaUnidad>)Session["listaPartidaUnidadAsociadas"];

                Double montoResta = listaPartidaUnidadAsociadas.Sum(part => part.monto);

                lblMontoRepartir.Text = monto - montoResta + "";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalUnidadesPartidas();", true);

                List<Unidad> listaUnidades = (List<Unidad>)Session["listaUnidadesAsociadas"];

                ddlUnidades.DataSource = listaUnidades;
                ddlUnidades.DataTextField = "nombreUnidad";
                ddlUnidades.DataValueField = "idUnidad";

                ddlUnidades.DataBind();

                Unidad unidad = (Unidad)listaUnidades.Where(uni => uni.idUnidad == (Convert.ToInt32(ddlUnidades.SelectedValue))).ToList().First();

                List<Partida> listaPartidasAsociadas = (List<Partida>)Session["listaPartidasAsociadas"];

                List<PartidaUnidad> listaPartidaUnidadSinAsociar = new List<PartidaUnidad>();

                foreach (Partida partida in listaPartidasAsociadas)
                {
                    List<PartidaUnidad> listaPartidaUnidadAsociadasTemp = (List<PartidaUnidad>)listaPartidaUnidadAsociadas.Where(pu => pu.idPartida == partida.idPartida && pu.idUnidad == unidad.idUnidad).ToList();
                    if (listaPartidaUnidadAsociadasTemp.Count() == 0)
                    {
                        PartidaUnidad partidaUnidad = new PartidaUnidad();
                        partidaUnidad.idPartida = partida.idPartida;
                        partidaUnidad.idUnidad = unidad.idUnidad;
                        partidaUnidad.monto = 0;
                        partidaUnidad.montoDisponible = cajaChicaUnidadPartidaServicios.getMontoDisponible(unidad, partida);
                        partidaUnidad.nombreUnidad = unidad.nombreUnidad;
                        partidaUnidad.numeroPartida = partida.numeroPartida;
                        listaPartidaUnidadSinAsociar.Add(partidaUnidad);
                    }
                }

                Session["listaPartidaUnidadSinAsociar"] = listaPartidaUnidadSinAsociar;

                mostrarDatosTablaUnidadPartidaSinAsociar();
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 05/mar/2021
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
        /// 05/mar/2021
        /// Efecto: cambia los montos de la tabla de repartir gastos dependiendo de la unidad seleccionada
        /// Requiere: cambiar la unidad
        /// Modifica: montos disponibles de cada partida
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUnidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Unidad> listaUnidades = (List<Unidad>)Session["listaUnidadesAsociadas"];

            Unidad unidad = (Unidad)listaUnidades.Where(uni => uni.idUnidad == (Convert.ToInt32(ddlUnidades.SelectedValue))).ToList().First();

            List<Partida> listaPartidasAsociadas = (List<Partida>)Session["listaPartidasAsociadas"];

            List<PartidaUnidad> listaPartidaUnidadSinAsociar = new List<PartidaUnidad>();

            List<PartidaUnidad> listaPartidaUnidadAsociadas = (List<PartidaUnidad>)Session["listaPartidaUnidadAsociadas"];

            foreach (Partida partida in listaPartidasAsociadas)
            {
                List<PartidaUnidad> listaPartidaUnidadAsociadasTemp = (List<PartidaUnidad>)listaPartidaUnidadAsociadas.Where(pu => pu.idPartida == partida.idPartida && pu.idUnidad == unidad.idUnidad).ToList();
                if (listaPartidaUnidadAsociadasTemp.Count() == 0)
                {
                    PartidaUnidad partidaUnidad = new PartidaUnidad();
                    partidaUnidad.idPartida = partida.idPartida;
                    partidaUnidad.idUnidad = unidad.idUnidad;
                    partidaUnidad.monto = 0;
                    partidaUnidad.montoDisponible = cajaChicaUnidadPartidaServicios.getMontoDisponible(unidad, partida);
                    partidaUnidad.nombreUnidad = unidad.nombreUnidad;
                    partidaUnidad.numeroPartida = partida.numeroPartida;
                    listaPartidaUnidadSinAsociar.Add(partidaUnidad);
                }
            }


            Session["listaPartidaUnidadSinAsociar"] = listaPartidaUnidadSinAsociar;

            mostrarDatosTablaUnidadPartidaSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 08/mar/2021
        /// Efecto: agreaga en la tabla de unidades partidas monto asociadas, segun el seleccionado
        /// Requiere: dar clic en el boton de seleccionar
        /// Modifica: las tablas de unidades partidas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSeleccionarUnidadPartida_Click(object sender, EventArgs e)
        {
            string[] unidadNumeroPartida = (((LinkButton)(sender)).CommandArgument.ToString().Split(new string[] { "::" }, StringSplitOptions.None));
            Double.TryParse(unidadNumeroPartida[0], out Double idPartida);
            Double.TryParse(unidadNumeroPartida[1], out Double idUnidad);

            List<PartidaUnidad> listaPartidaUnidadAsociadas = (List<PartidaUnidad>)Session["listaPartidaUnidadAsociadas"];
            List<PartidaUnidad> listaPartidaUnidadSinAsociar = (List<PartidaUnidad>)Session["listaPartidaUnidadSinAsociar"];

            PartidaUnidad partidaUnidad = (PartidaUnidad)listaPartidaUnidadSinAsociar.Where(unid => unid.idPartida == idPartida && unid.idUnidad == idUnidad).ToList().First();

            PartidaUnidad partidaUnidadTemp = new PartidaUnidad();

            partidaUnidadTemp = partidaUnidad;

            Double monto = 0;
            foreach (RepeaterItem item in rpPartidaUnidadSinAsociar.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton btnSeleccionarUnidadPartida = (LinkButton)item.FindControl("btnSeleccionarUnidadPartida");
                    string[] unidadNumeroPartidaTemp = (btnSeleccionarUnidadPartida.CommandArgument.ToString().Split(new string[] { "::" }, StringSplitOptions.None));
                    Double.TryParse(unidadNumeroPartidaTemp[0], out Double idPartidaTemp);
                    Double.TryParse(unidadNumeroPartidaTemp[1], out Double idUnidadTemp);
                    if (idPartida == idPartidaTemp && idUnidad == idUnidadTemp)
                    {
                        TextBox txtMontoAsociar = (TextBox)item.FindControl("txtMontoAsociar");
                        Double.TryParse(txtMontoAsociar.Text, out monto);
                    } 
                }
            }

            partidaUnidadTemp.monto = monto;

            Double.TryParse(txtMonto.Text, out Double monto2);
            Double montoResta = listaPartidaUnidadAsociadas.Sum(part => part.monto);

            if ((monto2 - montoResta - monto) < 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El monto ingresado es mayor al monto a repartir" + "');", true);
            }
            else
            {
                if (monto == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El monto ingresado debe de ser mayor a 0" + "');", true);
                }
                else
                {
                    if (partidaUnidadTemp.monto > partidaUnidadTemp.montoDisponible)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El monto ingresado es mayor al monto disponible en la partida" + "');", true);
                    }
                    else
                    {
                        listaPartidaUnidadAsociadas.Add(partidaUnidadTemp);
                        listaPartidaUnidadSinAsociar.Remove(partidaUnidad);

                        Session["listaPartidaUnidadAsociadas"] = listaPartidaUnidadAsociadas;
                        Session["listaPartidaUnidadSinAsociar"] = listaPartidaUnidadSinAsociar;
                        mostrarDatosTablaUnidadPartidaAsociadas();
                        mostrarDatosTablaUnidadPartidaSinAsociar();

                        lblMontoRepartir.Text = monto2 - montoResta - monto + "";
                    }
                }
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 09/mar/2021
        /// Efecto: desasocia en la tabla de unidades partidas monto asociadas, segun el seleccionado
        /// Requiere: dar clic en el boton de deseleccionar
        /// Modifica: las tablas de unidades partidas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarUnidadPartida_Click(object sender, EventArgs e)
        {
            string[] unidadNumeroPartida = (((LinkButton)(sender)).CommandArgument.ToString().Split(new string[] { "::" }, StringSplitOptions.None));
            Double.TryParse(unidadNumeroPartida[0], out Double idPartida);
            Double.TryParse(unidadNumeroPartida[1], out Double idUnidad);

            List<PartidaUnidad> listaPartidaUnidadAsociadas = (List<PartidaUnidad>)Session["listaPartidaUnidadAsociadas"];
            List<PartidaUnidad> listaPartidaUnidadSinAsociar = (List<PartidaUnidad>)Session["listaPartidaUnidadSinAsociar"];

            PartidaUnidad partidaUnidad = (PartidaUnidad)listaPartidaUnidadAsociadas.Where(unid => unid.idPartida == idPartida && unid.idUnidad == idUnidad).ToList().First();

            PartidaUnidad partidaUnidadTemp = new PartidaUnidad();

            partidaUnidadTemp = partidaUnidad;

            listaPartidaUnidadSinAsociar.Add(partidaUnidadTemp);
            listaPartidaUnidadAsociadas.Remove(partidaUnidad);

            Session["listaPartidaUnidadAsociadas"] = listaPartidaUnidadAsociadas;
            Session["listaPartidaUnidadSinAsociar"] = listaPartidaUnidadSinAsociar;
            mostrarDatosTablaUnidadPartidaAsociadas();
            mostrarDatosTablaUnidadPartidaSinAsociar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 09/mar/2021
        /// Efecto: redirecciona a la pantalla de Administrar Ejecuciones
        /// Requiere: dar clic en cancelar
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/CajaChica/AdministrarCajaChica.aspx");
            Response.Redirect(url);
        }






        /// <summary>
        /// Leonardo Carrion
        /// 19/mar/2021
        /// Efecto: actualiza la ejecucion con los datos ingresados
        /// Requiere: dar clic en el boton de Editar e ingresar datos 
        /// Modifica: ejecucion seleccionada
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtComentario.Text))
            {
                List<PartidaUnidad> listaPartidaUnidadAsociadas = (List<PartidaUnidad>)Session["listaPartidaUnidadAsociadas"];

                if (listaPartidaUnidadAsociadas.Count > 0)
                {
                    Entidades.CajaChica cajaChica = (Entidades.CajaChica)Session["ejecucionEditar"];

                    Periodo periodo = new Periodo();
                    periodo.anoPeriodo = Convert.ToInt32(Session["periodoSeleccionado"]);

                    EstadoCajaChica estadoCajaChica = new EstadoCajaChica();
                    estadoCajaChica = estadoCajaChicaServicios.getEstadoCajaChicaSegunNombre("Guardado");

                    Proyectos proyecto = new Proyectos();
                    proyecto.idProyecto = Convert.ToInt32(Session["proyectoSeleccionado"]);

                    Double.TryParse(txtMonto.Text, out Double monto);
                    
                    cajaChica.idEstadoCajaChica = estadoCajaChica;
                    cajaChica.anoPeriodo = periodo.anoPeriodo;
                    cajaChica.idProyedto = proyecto.idProyecto;
                    cajaChica.monto = monto;
                   
                    cajaChica.comentario = txtComentario.Text;


                    cajaChicaServicios.EditarCajaChica(cajaChica);

                    cajaChicaUnidadPartidaServicios.eliminarCajaChicaUnidadPartidaPorCajaChica(cajaChica.idCajaChica);

                    foreach (PartidaUnidad partidaUnidad in listaPartidaUnidadAsociadas)
                    {
                       cajaChicaUnidadPartidaServicios.insertarCajaChicaPartidaUnidad(cajaChica.idCajaChica, partidaUnidad.idUnidad, partidaUnidad.idPartida, partidaUnidad.monto);
                    }

                    String url = Page.ResolveUrl("~/Catalogos/CajaChica/AdministrarCajaChica.aspx");
                    Response.Redirect(url);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se edito con exito" + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe de repartir los gastos entre la unidad(es)" + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe ingresar el número de referencia" + "');", true);
            }
        }
        #endregion
    }
}