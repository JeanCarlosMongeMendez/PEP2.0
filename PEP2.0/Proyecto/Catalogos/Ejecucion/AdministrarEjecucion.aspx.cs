using Entidades;
using PEP;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proyecto.Catalogos.Ejecucion
{


    public partial class AdministrarEjecucion : System.Web.UI.Page
    {

        #region variables globales
        readonly PagedDataSource pgsource = new PagedDataSource();
        PeriodoServicios periodoServicios;
        ProyectoServicios proyectoServicios;
        TipoTramiteServicios tipoTramiteServicios;
        UnidadServicios unidadServicios;
        PartidaServicios partidaServicios;
        PresupuestoEgreso_PartidaServicios presupuestoEgreso_PartidaServicios;
        PresupuestoEgresosServicios presupuestoEgresosServicio;
        PartidaUnidad partidaUnidad;
        EjecucionServicios ejecucionServicios;
        ArchivoEjecucionServicios archivoEjecucionServicios;
        int idUnidadElegida;
        private int elmentosMostrar = 10;
        string periodooo = "";
        string proyectoo = "";
        string tipoTramite = "";
        int nuevaEjecucion;
        int verEjecucion;
        string nombre;
        string descripcionEjecucionOtro;
        static string idEjecucioon = "";
        //se utiliza en el metodo  MostrarDatosTablaUnidad();se utiliza para pasar unidades seleccionadas de la tabla que aparece en el  #modalElegirUnidad
        static List<Unidad> listaUnidad = new List<Unidad>();
        //Esta llena la tabla en el metodo mostrarDatosTabla(),la uso como temporal de la linkedlist
        static List<Unidad> listUnidad = new List<Unidad>();
        //se utiliza tambien en el metodo mostrarDatosTabla(),para llenar la tabla en el inicio 
        static List<Unidad> listUnidades = new List<Unidad>();

        int primerIndex, ultimoIndex, primerIndex2, ultimoIndex2, primerIndex3, ultimoIndex3, primerIndex4, ultimoIndex4, primerIndex7, ultimoIndex7, primerIndex6, ultimoIndex6;
        //contador que se utiliza en el metodo MostrarDatosTabla(),se utiliza para que recorra solo una ves en listUnidades
        static int count = 0;
        static int contador = 0;
        static double monto = 0;
        static int contadorBotonRepartir = 0;
        #endregion
        #region Paginacion
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
        private int paginaActual7
        {
            get
            {
                if (ViewState["paginaActual7"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["paginaActual7"]);
            }
            set
            {
                ViewState["paginaActual7"] = value;
            }
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
        private void Paginacion5()
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

            DataList1.DataSource = dt;
            DataList1.DataBind();
        }

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
                primerIndex6 = ultimoIndex6 - 4;
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

            DataList2.DataSource = dt;
            DataList2.DataBind();
        }
        private void Paginacion7()
        {
            var dt = new DataTable();
            dt.Columns.Add("IndexPagina"); //Inicia en 0
            dt.Columns.Add("PaginaText"); //Inicia en 1

            primerIndex7 = paginaActual - 2;
            if (paginaActual > 2)
                ultimoIndex7 = paginaActual + 2;
            else
                ultimoIndex7 = 4;

            //se revisa que la ultima pagina sea menor que el total de paginas a mostrar, sino se resta para que muestre bien la paginacion
            if (ultimoIndex7 > Convert.ToInt32(ViewState["TotalPaginas7"]))
            {
                ultimoIndex7 = Convert.ToInt32(ViewState["TotalPaginas7"]);
                primerIndex7 = ultimoIndex - 4;
            }

            if (primerIndex7 < 0)
                primerIndex7 = 0;

            //se crea el numero de paginas basado en la primera y ultima pagina
            for (var i = primerIndex7; i < ultimoIndex7; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            DataList7.DataSource = dt;
            DataList7.DataBind();
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
            if (!e.CommandName.Equals("nuevaPagina3")) return;
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
        protected void lbPrimero5_Click(object sender, EventArgs e)
        {
            paginaActual5 = 0;
            obtenerUnidadesPartidasAsignarMonto();
            
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
        protected void lbUltimo5_Click(object sender, EventArgs e)
        {
            paginaActual5 = (Convert.ToInt32(ViewState["TotalPaginas"]) - 1);
            obtenerUnidadesPartidasAsignarMonto();
           
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
        protected void lbAnterior5_Click(object sender, EventArgs e)
        {
            paginaActual5 -= 1;
            obtenerUnidadesPartidasAsignarMonto();
            
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
        protected void lbSiguiente5_Click(object sender, EventArgs e)
        {
            paginaActual5 += 1;
            obtenerUnidadesPartidasAsignarMonto();
           
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
        protected void rptPaginacion5_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual5 = Convert.ToInt32(e.CommandArgument.ToString());
            obtenerUnidadesPartidasAsignarMonto();
            
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
        protected void rptPaginacion5_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion");
            if (lnkPagina.CommandArgument != paginaActual.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }
        //----------

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
        protected void lbPrimero6_Click(object sender, EventArgs e)
        {
            paginaActual6 = 0;
            MostrarUnidadesConMontoRepartido();
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
        protected void lbUltimo6_Click(object sender, EventArgs e)
        {
            paginaActual6 = (Convert.ToInt32(ViewState["TotalPaginas6"]) - 1);
            MostrarUnidadesConMontoRepartido();
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
        protected void lbAnterior6_Click(object sender, EventArgs e)
        {
            paginaActual6 -= 1;
            MostrarUnidadesConMontoRepartido();
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
        protected void lbSiguiente6_Click(object sender, EventArgs e)
        {
            paginaActual6 += 1;
            MostrarUnidadesConMontoRepartido();
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
        protected void rptPaginacion6_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina6")) return;
            paginaActual6 = Convert.ToInt32(e.CommandArgument.ToString());
            MostrarUnidadesConMontoRepartido();
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
        protected void rptPaginacion6_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion6");
            if (lnkPagina.CommandArgument != paginaActual6.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }
       

            //----------

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
        protected void lbPrimero7_Click(object sender, EventArgs e)
        {
            paginaActual7 = 0;
            VerEjecucionArchivos();
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
        protected void lbUltimo7_Click(object sender, EventArgs e)
        {
            paginaActual7 = (Convert.ToInt32(ViewState["TotalPaginas"]) - 1);
            VerEjecucionArchivos();
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
        protected void lbAnterior7_Click(object sender, EventArgs e)
        {
            paginaActual7 -= 1;
            VerEjecucionArchivos();
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
        protected void lbSiguiente7_Click(object sender, EventArgs e)
        {
            paginaActual7 += 1;
            VerEjecucionArchivos();
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
        protected void rptPaginacion7_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual7 = Convert.ToInt32(e.CommandArgument.ToString());
            VerEjecucionArchivos();
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
        protected void rptPaginacion7_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion");
            if (lnkPagina.CommandArgument != paginaActual7.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }
        #endregion
        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {

            this.ejecucionServicios = new EjecucionServicios();
            this.presupuestoEgresosServicio = new PresupuestoEgresosServicios();
            this.periodoServicios = new PeriodoServicios();
            this.proyectoServicios = new ProyectoServicios();
            this.unidadServicios = new UnidadServicios();
            this.partidaServicios = new PartidaServicios();
            this.tipoTramiteServicios = new TipoTramiteServicios();
            this.presupuestoEgreso_PartidaServicios = new PresupuestoEgreso_PartidaServicios();
            this.partidaUnidad = new PartidaUnidad();
            this.archivoEjecucionServicios = new ArchivoEjecucionServicios();
            if (!IsPostBack)
            {
                descripcionEjecucionOtro = Convert.ToString(Session["descripcionEjecionOtro"]);
                nuevaEjecucion = Convert.ToInt32(Session["nuevaEjecucion"]);
                verEjecucion = Convert.ToInt32(Session["verEjecucion"]);
                nombre = Convert.ToString(Session["nombreCompleto"]);
                if (nuevaEjecucion == 0)
                {
                    List<Entidades.Unidad> comparaListaUnidades = new List<Entidades.Unidad>();
                    LinkedList<int> unidades = new LinkedList<int>();
                    Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());

                    //Session["partidasPorUnidadesProyectoPeriodo"] = null;
                    //Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
                    Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = (List<Partida>)Session["listaPartida"];
                    // Session["partidasAsignadasConMonto"] = null;
                    periodooo = Convert.ToString(Session["periodo"]);
                    proyectoo = Convert.ToString(Session["proyecto"]);
                    Session["partidasAsignadasConMonto"] = (List<PartidaUnidad>)Session["listaMontoPartidaDisponible"];
                    
                    Session["listaArchivoEjecucion"] = (List<ArchivoEjecucion>)Session["listaArchivoEjecucion"];
                   
                    listaUnidad = (List<Unidad>)Session["listaUnidad"];
                    MostrarDatosTablaUnidad(listaUnidad);


                    comparaListaUnidades = unidadServicios.ObtenerPorProyecto(Convert.ToInt32(proyectoo)).ToList<Unidad>();
                    List<Unidad> tempUnidad = listaUnidad.Where(a => !comparaListaUnidades.Any(a1 => a1.idUnidad == a.idUnidad))
                         .Union(comparaListaUnidades.Where(a => !listaUnidad.Any(a1 => a1.idUnidad == a.idUnidad))).ToList();

                    listUnidad = tempUnidad;
                    count = count + 1;


                    foreach (Unidad unidad in listaUnidad)
                    {
                        unidades.AddFirst(unidad.idUnidad);
                    }
                    List<Partida> tempPartida = new List<Partida>();
                    List<Partida> Partidas = partidaServicios.ObtienePartidaPorPeriodoUnidadProyecto(Convert.ToInt32(proyectoo), unidades, Convert.ToInt32(periodooo));
                    List<Partida> temppartidasSeleccionadasPorUnidadesProyectoPeriodo = (List<Partida>)Session["listaPartida"];
                    tempPartida = Partidas.Where(a => !temppartidasSeleccionadasPorUnidadesProyectoPeriodo.Any(a1 => a1.numeroPartida == a.numeroPartida && a1.idPartida == a.idPartida))
                         .Union(temppartidasSeleccionadasPorUnidadesProyectoPeriodo.Where(a => !Partidas.Any(a1 => a1.numeroPartida != a.numeroPartida && a1.idPartida != a.idPartida))).ToList();
                    //MostrarDatosTabla();
                    Session["partidasPorUnidadesProyectoPeriodo"] = tempPartida;
                    tipoTramite = Convert.ToString(Session["idTipoTramite"]);
                    txtMontoIngresar.Text = Convert.ToString(Session["monto"]);
                    numeroReferencia.Text = Convert.ToString(Session["numeroReferencia"]);
                    idEjecucioon = Convert.ToString(Session["idEjecucion"]);

                    PeriodosDDL.Items.Clear();
                    ProyectosDDL.Items.Clear();
                    DDLTipoTramite.Items.Clear();
                    ddlPartida.Items.Clear();

                    CargarPeriodos();
                    //obtenerUnidadesPartidasAsignarMonto();
                    obtenerPartidasSeleccionadas();
                    CargarTramites();
                    MostrarUnidadesConMontoRepartido();



                    descripcionOtroTipoTramite.Visible = false;
                    UpdatePanel10.Visible = false;
                    ButtonRepartir.Visible = false;
                    BtnElimarEjecucion.Visible = false;
                    BtnCerrar.Visible = true;
                    MostrarTablaRepartirGastos();
                    if (verEjecucion == 0)
                    {

                        MostrarEjecucionBotonesLink();

                    }
                    if (!descripcionEjecucionOtro.Equals(""))
                    {
                        descripcionOtroTipoTramite.Text = descripcionEjecucionOtro;
                        descripcionOtroTipoTramite.Visible = true;
                    }
                }
                else
                {
                    descripcionOtroTipoTramite.Visible = false;
                    UpdatePanel10.Visible = false;
                    ButtonRepartir.Visible = false;
                    BtnElimarEjecucion.Visible = false;
                    BtnCerrar.Visible = true;
                    Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                    Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
                    Session["partidasPorUnidadesProyectoPeriodo"] = null;
                    Session["partidasAsignadasConMonto"] = null;
                    idEjecucioon = "";
                    CargarPeriodos();
                    CargarTramites();
                    MostrarTablaRepartirGastos();
                }
            }
            else
            {

                obtenerPartidasSeleccionadas();
            }



        }
        #endregion
        #region eventos
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

            obtenerPartidasSeleccionadas();
            DDLTipoTramite.Items.Clear();
            descripcionOtroTipoTramite.Visible = false;
            CargarTramites();
        }
        protected void Proyectos_OnChanged(object sender, EventArgs e)
        {

            Session["partidasPorUnidadesProyectoPeriodo"] = null;
            Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
            reiniciarTablaUnidad();

            obtenerPartidasSeleccionadas();
            DDLTipoTramite.Items.Clear();
            descripcionOtroTipoTramite.Visible = false;
            CargarTramites();

        }
        protected void ButtonRepartirPartidas_Click(object sender, EventArgs e)
        {
            //
            CargarPartidasPorUnidades();
            List<PartidaUnidad> partidasAsignadasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];
            Double montoDisponible = 0;
            if (partidasAsignadasConMonto != null)
            {

                montoDisponible = (Double)partidasAsignadasConMonto.Sum(monto => monto.Monto);
            }
            else
            {
                montoRepartir.Text = txtMontoIngresar.Text;
            }

            if (partidasAsignadasConMonto == null)
            {


            }

            if (!IsNumeric(txtMontoIngresar.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El texto ingresado deben ser números" + "');", true);
            }
            else if (montoDisponible <= Convert.ToDouble((txtMontoIngresar.Text)))
            {



                if (IsNumeric(txtMontoIngresar.Text))
                {
                    if (Convert.ToDouble((txtMontoIngresar.Text)) >= 0)
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
                            //List<PartidaUnidad> partidasAsignadasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];
                            //Double sumaMontoTotalRepartir = (Double)partidasAsignadasConMonto.Sum(PartidaUnidad => PartidaUnidad.Monto);
                            //if (Convert.ToDouble(txtMontoIngresar.Text) >= sumaMontoTotalRepartir || monto == 0)
                            //{
                            if (partidasAsignadasConMonto != null)
                            {
                                montoDisponible = (Double)partidasAsignadasConMonto.Sum(monto => monto.Monto);


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
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El texto ingresado deben ser mayor para cubrir los gastos" + "');", true);
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
                List<PartidaUnidad> partidasElegidasTemporal = new List<PartidaUnidad>();
                partidasElegidasTemporal = partidasElegidas;
                PartidaUnidad partidaUnidad = new PartidaUnidad();
                //Double montoDisponible = (Double)listaPartidasEgreso.Sum(presupuesto => presupuesto.monto);
                Double sumaMontoTotalRepartir = (Double)partidasElegidasConMonto.Sum(PartidaUnidad => PartidaUnidad.Monto);
                if (Convert.ToDouble(txtMontoIngresar.Text) >= sumaMontoTotalRepartir || monto == 0)
                {

                    foreach (PartidaUnidad p in partidasElegidas.ToList())
                    {
                        Double montoDisponible = partidasElegidasTemporal.Where(item => p.IdUnidad == item.IdUnidad && item.NumeroPartida == p.NumeroPartida).ToList().First().MontoDisponible;
                        if (monto <= montoDisponible || monto >= 0)
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
                                    if (Double.TryParse(txtMont, out Double montoo) && idPartid == Convert.ToDouble(idPartida) && Convert.ToDouble(txtMont) > 0)
                                    {
                                        montoRepartir.Text = (Convert.ToString(Convert.ToDouble(montoRepartir.Text) - Convert.ToDouble(txtMont)));
                                        monto = Convert.ToDouble(montoRepartir.Text) + Convert.ToDouble(txtMont);

                                        //txtMonto.Text = monto.ToString();

                                        if (monto >= 0 && monto >= Convert.ToDouble(txtMont) && montoDisponible >= Convert.ToDouble(txtMont))
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
                                    else if (!IsNumeric(txtMont))
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Por favor ingrese numeros" + "');", true);

                                    }
                                    else if (Convert.ToDouble(txtMont) < 0 && IsNumeric(txtMont))

                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Por favor ingrese montos positivos" + "');", true);
                                    }
                                    else if (montoDisponible < monto)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "La Partida no cuenta con el dinero suficiente" + "');", true);
                                    }
                                    else if (Convert.ToDouble(txtMont) == 0 && idPartid == Convert.ToDouble(idPartida))
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Por favor ingrese montos mayores que 0" + "');", true);
                                    }




                                }
                                if (montoDisponible >= monto)
                                {
                                    Session["partidasAsignadasConMonto"] = partidasElegidasConMonto;
                                    MostrarUnidadesConMontoRepartido();
                                    partidasElegidas.RemoveAll(item => item.IdPartida == Convert.ToInt32(idPartida) && item.IdUnidad == idUnidadElegida);
                                    Session["partidasAsignadas"] = partidasElegidas;
                                    obtenerUnidadesPartidasAsignarMonto();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "La Partida fue añadida con exito" + "');", true);
                                }


                                // partidasElegidas.Add(partidaUnidad);
                            }

                        }/*else if(monto!=0)*/
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "La Partida no cuenta con el dinero suficiente" + "');", true);
                        //}

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalRepartirPartidas", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalRepartirPartidas').hide();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", " activarModalRepartirPartidas();", true);
                    }
                }

            }
        }
        protected void EliminarMontoRepartido_OnChanged(object sender, EventArgs e)
        {
            string[] unidadNumeroPartida = (((LinkButton)(sender)).CommandArgument.ToString().Split(new string[] { "::" }, StringSplitOptions.None));
            string numeroPartida = unidadNumeroPartida[0];
            string idUnidad = unidadNumeroPartida[1];
            List<PartidaUnidad> partidasAsignadas = (List<PartidaUnidad>)Session["partidasAsignadas"];
            List<PartidaUnidad> partidasElegidasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];

            partidasAsignadas.Add((PartidaUnidad)partidasElegidasConMonto.Where(item => item.NumeroPartida == numeroPartida && item.IdUnidad == Convert.ToInt32(idUnidad)).ToList().First());
            double montoEliminado = partidasElegidasConMonto.Where(item => item.NumeroPartida == numeroPartida && item.IdUnidad == Convert.ToInt32(idUnidad)).ToList().First().Monto;
            monto = monto + montoEliminado;
            partidasElegidasConMonto.RemoveAll(item => item.NumeroPartida == numeroPartida && item.IdUnidad == Convert.ToInt32(idUnidad));
            MostrarUnidadesConMontoRepartido();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "La unidad fue borrada con exito" + "');", true);
        }
        private void MostrarUnidadesConMontoRepartido()
        {
            List<PartidaUnidad> partidasAsignadasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];
            if (partidasAsignadasConMonto != null)
            {
                var dt = partidasAsignadasConMonto;
                pgsource.DataSource = dt;
                pgsource.AllowPaging = true;
                //numero de items que se muestran en el Repeater
                pgsource.PageSize = elmentosMostrar;
                pgsource.CurrentPageIndex = paginaActual6;
                //mantiene el total de paginas en View State
                ViewState["TotalPaginas6"] = pgsource.PageCount;
                //Ejemplo: "Página 1 al 10"
                lblpagina6.Text = "Página " + (paginaActual6 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
                //Habilitar los botones primero, último, anterior y siguiente
                lbAnterior6.Enabled = !pgsource.IsFirstPage;
                lbSiguiente6.Enabled = !pgsource.IsLastPage;
                lbPrimero6.Enabled = !pgsource.IsFirstPage;
                lbUltimo6.Enabled = !pgsource.IsLastPage;

                Repeater5.DataSource = pgsource;
                Repeater5.DataBind();

                //metodo que realiza la paginacion
                Paginacion6();
            }
            else
            {
                List<PartidaUnidad> partidasAsignadasConMontoo = new List<PartidaUnidad>();
                var dt = partidasAsignadasConMontoo;
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

                Repeater5.DataSource = pgsource;
                Repeater5.DataBind();

                //metodo que realiza la paginacion
                Paginacion6();
            }


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

            List<Partida> partidasElegidas = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
            Partida partida = partidasElegidas.Where(item => numeroPartida == item.numeroPartida).ToList().First();   /* partidaServicios.ObtenerPorNumeroPartida(numeroPartida);*/

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
            if (partidasElegidas != null)
            {
                partidasElegidas.RemoveAll(item => item.idUnidad == idUnidad);
                obtenerPartidasSeleccionadas();
                //obtenerPartidasPorProyectoUnidadPeriodo();
            }
            List<PartidaUnidad> partidasElegidasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];
            if (partidasElegidasConMonto != null)
            {
                if (partidasElegidasConMonto.Exists(element => element.IdUnidad.Equals(idUnidad)))
                {
                    double montoEliminado = partidasElegidasConMonto.Where(item => item.IdUnidad == idUnidad).ToList().First().Monto;
                    monto = monto + montoEliminado;
                    partidasElegidasConMonto.RemoveAll(item => item.IdUnidad == idUnidad);
                    MostrarUnidadesConMontoRepartido();
                }
            }
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
            else
            {
                descripcionOtroTipoTramite.Visible = false;
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
        protected void textBox1_TextChanged(object sender, EventArgs e)
        {

            if (IsNumeric(txtMontoIngresar.Text))
            {
                monto = monto + Convert.ToDouble(txtMontoIngresar.Text);
            }


        }
        /// <summary>
        /// Kevin Picado Quesada
        /// 15/2/2020
        /// Efecto:  Guarda las unidades elegidas,partidas,partidas selecionadas con montos asignados ,ademas se matiene con un estado de ejecucion en 1 que significa guardado
        /// Requiere: dar clic en el boton de "Guardar"
        /// Modifica: ---
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int respuesta = 0;
            //Entidades.Ejecucion ejecucionGuardar = new Entidades.Ejecucion();
            List<Partida> partidasAsignadas = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
            List<PartidaUnidad> partidasElegidasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];
            Unidad unidad = new Unidad();
            EstadoEjecucion estadoEjecucion = new EstadoEjecucion();
            TipoTramite tipoTramite = new TipoTramite();
            String valor = DDLTipoTramite.SelectedItem.Text;
            if (!txtMontoIngresar.Text.Equals(""))
            {

                if ((valor.Equals("Otros") && !descripcionOtroTipoTramite.Text.Equals("")) || !valor.Equals("Otros"))
                {


                    if (partidasAsignadas == null)
                    {
                        partidasAsignadas = new List<Partida>();
                    }
                    if (partidasElegidasConMonto == null)
                    {
                        partidasElegidasConMonto = new List<PartidaUnidad>();
                    }
                    if (listaUnidad == null)
                    {

                        listaUnidad = new List<Unidad>();
                    }

                    if (idEjecucioon.Equals(""))

                    {
                        Entidades.Ejecucion ejecucionGuardar = new Entidades.Ejecucion();
                        estadoEjecucion.idEstado = 1;
                        ejecucionGuardar.idestado = estadoEjecucion;
                        ejecucionGuardar.anoPeriodo = Convert.ToInt32(PeriodosDDL.SelectedValue);
                        ejecucionGuardar.idProyecto = Int32.Parse(ProyectosDDL.SelectedValue);
                        ejecucionGuardar.monto = Convert.ToInt32(txtMontoIngresar.Text);
                        tipoTramite.idTramite = Int32.Parse(DDLTipoTramite.SelectedValue);
                        ejecucionGuardar.idTipoTramite = tipoTramite;
                        ejecucionGuardar.descripcionEjecucionOtro = descripcionOtroTipoTramite.Text;
                        ejecucionGuardar.numeroReferencia = numeroReferencia.Text;
                        ejecucionGuardar.numeroReferencia = numeroReferencia.Text;
                        respuesta = ejecucionServicios.InsertarEjecucion(ejecucionGuardar);
                        // Inserción de los archivos en el servidor y en la BD
                        if (fuArchivos.HasFiles)
                        {
                            ejecucionGuardar.idEjecucion = respuesta;
                            List<ArchivoEjecucion> listaArchivos = guardarArchivos(ejecucionGuardar, fuArchivos);

                            foreach (ArchivoEjecucion archivo in listaArchivos)
                            {
                                //archivo.tipoArchivo = tipoArchivoServicios.getTipoArchivoPorDatos("Archivo");
                                archivoEjecucionServicios.insertarArchivoEjecucion(archivo);
                            }
                        }

                    }
                    else
                    {
                        Entidades.Ejecucion ejecucionGuardar = new Entidades.Ejecucion();

                        respuesta = Convert.ToInt32(idEjecucioon);
                        ejecucionGuardar.idEjecucion = respuesta;
                        estadoEjecucion.idEstado = 1;
                        ejecucionGuardar.idestado = estadoEjecucion;
                        ejecucionGuardar.anoPeriodo = Convert.ToInt32(PeriodosDDL.SelectedValue);
                        ejecucionGuardar.idProyecto = Int32.Parse(ProyectosDDL.SelectedValue);
                        ejecucionGuardar.monto = Convert.ToInt32(txtMontoIngresar.Text);
                        tipoTramite.idTramite = Int32.Parse(DDLTipoTramite.SelectedValue);
                        ejecucionGuardar.idTipoTramite = tipoTramite;
                        ejecucionGuardar.numeroReferencia = numeroReferencia.Text;
                        ejecucionGuardar.descripcionEjecucionOtro = descripcionOtroTipoTramite.Text;
                        ejecucionServicios.EliminarEjecucionUnidad(respuesta);
                        ejecucionServicios.EliminarEjecucionPartidas(respuesta);
                        ejecucionServicios.EliminarEjecucionPartidaMontoElelegido(respuesta);
                        ejecucionServicios.EditarEjecucion(ejecucionGuardar);
                        if (fuArchivos.HasFiles)
                        {
                            List<ArchivoEjecucion> listaArchivos = guardarArchivos(ejecucionGuardar, fuArchivos);

                            foreach (ArchivoEjecucion archivo in listaArchivos)
                            {
                                //archivo.tipoArchivo = tipoArchivoServicios.getTipoArchivoPorDatos("Archivo");
                                archivoEjecucionServicios.insertarArchivoEjecucion(archivo);
                            }
                        }
                    }
                    foreach (Unidad u in listaUnidad)
                    {
                        unidad.idUnidad = u.idUnidad;
                        unidad.nombreUnidad = u.nombreUnidad;
                        ejecucionServicios.InsertarEjecucionUnidad(unidad, numeroReferencia.Text, respuesta);
                    }
                    Partida partida = new Partida();
                    foreach (Partida p in partidasAsignadas)
                    {

                        partida.numeroPartida = p.numeroPartida;
                        partida.idPartida = p.idPartida;
                        partida.descripcionPartida = p.descripcionPartida;
                        ejecucionServicios.InsertarEjecucionPartidas(partida, numeroReferencia.Text, respuesta);
                    }
                    PartidaUnidad partidaUnidad = new PartidaUnidad();
                    foreach (PartidaUnidad pu in partidasElegidasConMonto)
                    {

                        partidaUnidad.IdPartida = pu.IdPartida;
                        partidaUnidad.IdUnidad = pu.IdUnidad;
                        partidaUnidad.Monto = pu.Monto;
                        partidaUnidad.MontoDisponible = pu.MontoDisponible;
                        partidaUnidad.NumeroPartida = pu.NumeroPartida;
                        ejecucionServicios.InsertarEjecucionPartidaMontoElelegido(partidaUnidad, numeroReferencia.Text, respuesta);
                    }
                    String url = Page.ResolveUrl("~/Catalogos/Ejecucion/ElegirEjecucion.aspx");
                    Response.Redirect(url);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Por favor ingrese una descripción del tipo de trámite Otros" + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Por favor ingrese un monto" + "');", true);
            }


        }
        /// <summary>
        /// Kevin Picado Quesada
        /// 15/2/2020
        /// Efecto: Aprueba las partidad elegidas con su monto correspondiente 
        /// Requiere: dar clic en el boton de "Aprobar"
        /// Modifica: Modifica el estado de ejecucion y le asigana un 2 de aprobado,ademas si hay algun cambio en las unidades elegidas o en la partidas elegidas o en los montos
        /// elimina los datos relacionados con la tabla ejecucion por ejemplo Ejecucion_Unidades y las vuelve a rescribir .
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApobar_Click(object sender, EventArgs e)
        {
            Entidades.Ejecucion ejecucionGuardar = new Entidades.Ejecucion();
            List<Partida> partidasAsignadas = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
            List<PartidaUnidad> partidasElegidasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];

            EstadoEjecucion estadoEjecucion = new EstadoEjecucion();
            TipoTramite tipoTramite = new TipoTramite();
            Unidad unidad = new Unidad();
            String valor = DDLTipoTramite.SelectedItem.Text;
            int respuesta = 0;
            if (!txtMontoIngresar.Text.Equals("") && partidasElegidasConMonto != null && partidasElegidasConMonto.Count() != 0)
            {
                if ((valor.Equals("Otros") && !descripcionOtroTipoTramite.Text.Equals("")) || !valor.Equals("Otros"))
                {
                    if (partidasAsignadas == null)
                    {
                        partidasAsignadas = new List<Partida>();
                    }
                    if (partidasElegidasConMonto == null)
                    {
                        partidasElegidasConMonto = new List<PartidaUnidad>();
                    }
                    if (listaUnidad == null)
                    {

                        listaUnidad = new List<Unidad>();
                    }


                    if (idEjecucioon.Equals(""))
                    {
                        estadoEjecucion.idEstado = 2;
                        ejecucionGuardar.idestado = estadoEjecucion;
                        ejecucionGuardar.anoPeriodo = Convert.ToInt32(PeriodosDDL.SelectedValue);
                        ejecucionGuardar.idProyecto = Int32.Parse(ProyectosDDL.SelectedValue);
                        ejecucionGuardar.monto = Convert.ToInt32(txtMontoIngresar.Text);
                        tipoTramite.idTramite = Int32.Parse(DDLTipoTramite.SelectedValue);
                        ejecucionGuardar.idTipoTramite = tipoTramite;
                        ejecucionGuardar.numeroReferencia = numeroReferencia.Text;
                        ejecucionGuardar.descripcionEjecucionOtro = descripcionOtroTipoTramite.Text;
                        respuesta = ejecucionServicios.InsertarEjecucion(ejecucionGuardar);
                        // Inserción de los archivos en el servidor y en la BD
                        if (fuArchivos.HasFiles)
                        {
                            ejecucionGuardar.idEjecucion = respuesta;
                            List<ArchivoEjecucion> listaArchivos = guardarArchivos(ejecucionGuardar, fuArchivos);

                            foreach (ArchivoEjecucion archivo in listaArchivos)
                            {
                                //archivo.tipoArchivo = tipoArchivoServicios.getTipoArchivoPorDatos("Archivo");
                                archivoEjecucionServicios.insertarArchivoEjecucion(archivo);
                            }
                        }
                    }
                    else
                    {
                        respuesta = Convert.ToInt32(idEjecucioon);
                        ejecucionGuardar.idEjecucion = respuesta;
                        estadoEjecucion.idEstado = 2;
                        ejecucionGuardar.idestado = estadoEjecucion;
                        ejecucionGuardar.anoPeriodo = Convert.ToInt32(PeriodosDDL.SelectedValue);
                        ejecucionGuardar.anoPeriodo = Convert.ToInt32(PeriodosDDL.SelectedValue);
                        ejecucionGuardar.idProyecto = Int32.Parse(ProyectosDDL.SelectedValue);
                        ejecucionGuardar.monto = Convert.ToInt32(txtMontoIngresar.Text);
                        tipoTramite.idTramite = Int32.Parse(DDLTipoTramite.SelectedValue);
                        ejecucionGuardar.idTipoTramite = tipoTramite;
                        ejecucionGuardar.numeroReferencia = numeroReferencia.Text;
                        ejecucionGuardar.descripcionEjecucionOtro = descripcionOtroTipoTramite.Text;
                        ejecucionServicios.EliminarEjecucionUnidad(ejecucionGuardar.idEjecucion);
                        ejecucionServicios.EliminarEjecucionPartidas(ejecucionGuardar.idEjecucion);
                        ejecucionServicios.EliminarEjecucionPartidaMontoElelegido(ejecucionGuardar.idEjecucion);
                        ejecucionServicios.EditarEjecucion(ejecucionGuardar);
                        if (fuArchivos.HasFiles)
                        {
                            List<ArchivoEjecucion> listaArchivos = guardarArchivos(ejecucionGuardar, fuArchivos);

                            foreach (ArchivoEjecucion archivo in listaArchivos)
                            {
                                //archivo.tipoArchivo = tipoArchivoServicios.getTipoArchivoPorDatos("Archivo");
                                archivoEjecucionServicios.insertarArchivoEjecucion(archivo);
                            }
                        }

                    }

                    foreach (Unidad u in listaUnidad)
                    {
                        unidad.idUnidad = u.idUnidad;
                        unidad.nombreUnidad = u.nombreUnidad;
                        ejecucionServicios.InsertarEjecucionUnidad(unidad, numeroReferencia.Text, respuesta);
                    }
                    Partida partida = new Partida();
                    foreach (Partida p in partidasAsignadas)
                    {

                        partida.numeroPartida = p.numeroPartida;
                        partida.idPartida = p.idPartida;
                        partida.descripcionPartida = p.descripcionPartida;
                        ejecucionServicios.InsertarEjecucionPartidas(partida, numeroReferencia.Text, respuesta);
                    }
                    PartidaUnidad partidaUnidad = new PartidaUnidad();
                    foreach (PartidaUnidad pu in partidasElegidasConMonto)
                    {

                        partidaUnidad.IdPartida = pu.IdPartida;
                        partidaUnidad.IdUnidad = pu.IdUnidad;
                        partidaUnidad.Monto = pu.Monto;
                        partidaUnidad.MontoDisponible = pu.MontoDisponible;
                        partidaUnidad.NumeroPartida = pu.NumeroPartida;
                        ejecucionServicios.InsertarEjecucionPartidaMontoElelegido(partidaUnidad, numeroReferencia.Text, respuesta);
                    }
                    String url = Page.ResolveUrl("~/Catalogos/Ejecucion/ElegirEjecucion.aspx");
                    Response.Redirect(url);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Por favor ingrese una descripción del tipo de trámite Otros" + "');", true);
                }
            }
            else
            {
                if (partidasElegidasConMonto == null || partidasElegidasConMonto.Count() == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Por favor debe seleccionar partidas con montos repartidos " + "');", true);
                }
                if (txtMontoIngresar.Text.Equals(""))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Por favor ingrese un monto" + "');", true);
                }
            }


        }
        /// <summary>
        /// Kevin Picado Quesada
        /// 15/2/2020
        /// Efecto: Aprueba las partidad elegidas con su monto correspondiente 
        /// Requiere: dar clic en el boton de "Aprobar"
        /// Modifica: Modifica el estado de ejecucion y le asigana un 2 de aprobado,ademas si hay algun cambio en las unidades elegidas o en la partidas elegidas o en los montos
        /// elimina los datos relacionados con la tabla ejecucion por ejemplo Ejecucion_Unidades y las vuelve a rescribir .
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarEjecucion_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmarEliminar", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalElegirUnidad').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalConfirmarEjecucion();", true);
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/Ejecucion/ElegirEjecucion.aspx");
            Response.Redirect(url);
        }
        protected void btnConfirmarEjecucion_Click(object sender, EventArgs e)
        {
            EliminarArchivo();
            ejecucionServicios.EliminarEjecucionUnidad(Convert.ToInt32(idEjecucioon));
            ejecucionServicios.EliminarEjecucionPartidas(Convert.ToInt32(idEjecucioon));
            ejecucionServicios.EliminarEjecucionPartidaMontoElelegido(Convert.ToInt32(idEjecucioon));
            ejecucionServicios.EliminarEjecucion(Convert.ToInt32(idEjecucioon));
            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Ejecucion fue borrada con exito" + "');", true);
            String url = Page.ResolveUrl("~/Catalogos/Ejecucion/ElegirEjecucion.aspx");
            Response.Redirect(url);
        }
        protected void btnVerEjecucionArchivo_Click(object sender, EventArgs e)
        {
            VerEjecucionArchivos();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalVerEjecucionArchivo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalVerEjecucionArchivo').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalVerEjecucionArchivo();", true);
        }

            #endregion
            #region logica 

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
                var dt3 = partidasF;
                rpPartidasSeleccionada.DataSource = dt3;
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

                rpPartidasSeleccionada.DataBind();
            }
        }


        /// <summary>
        /// Josseline M
        /// Este método se encarga de actualizar las partidas a partir del periodo, proyecto y unidades elegidas
        /// </summary>
        private void obtenerPartidasPorProyectoUnidadPeriodo()
        {
            List<Partida> TempPartida = new List<Partida>();
            if (listaUnidad == null || listaUnidad.Count == 0)
            {
                Session["partidasPorUnidadesProyectoPeriodo"] = null;
            }

            List<Partida> partidas = (List<Partida>)Session["partidasPorUnidadesProyectoPeriodo"];
            if (partidas == null || partidas.Count == 0)
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
                    if (periodooo.Equals(""))
                    {

                        Session["partidasPorUnidadesProyectoPeriodo"] = partidaServicios.ObtienePartidaPorPeriodoUnidadProyecto(proyectoElegido, unidades, periodoElegido);
                    }

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
                LinkedList<int> unidades = new LinkedList<int>();
                foreach (Unidad unidad in listaUnidad)
                {
                    unidades.AddFirst(unidad.idUnidad);
                }
            
                List<Partida> TempPartidasN = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
                    //(List<Partida>)Session["partidasPorUnidadesProyectoPeriodo"];
                int proyectoElegido = Int32.Parse(ProyectosDDL.SelectedValue);
                int periodoElegido = Int32.Parse(PeriodosDDL.SelectedValue);
                List<Partida> partidasN = partidaServicios.ObtienePartidaPorPeriodoUnidadProyecto(proyectoElegido, unidades, periodoElegido);
                if (TempPartidasN != null || TempPartidasN.Count !=0)
                {
                    TempPartida = TempPartidasN.Where(a => !partidasN.Any(a1 => a1.numeroPartida == a.numeroPartida))
                   .Union(partidasN.Where(a => !TempPartidasN.Any(a1 => a1.numeroPartida == a.numeroPartida))).ToList();
                }
                else
                {
                    TempPartida = partidasN;
                }
                

                var dt5 = TempPartida;
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


            if (periodooo.Equals(""))
            {
                Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
                Session["partidasPorUnidadesProyectoPeriodo"] = null;
            }
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
                    if (Convert.ToString(periodo.anoPeriodo).Equals(periodooo))
                    {
                        anoHabilitado = periodo.anoPeriodo;
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
                    if (proyectoo != "")
                        ProyectosDDL.Items.FindByValue(proyectoo.ToString()).Selected = true;
                    //CargarUnidades();
                }
            }

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

                listUnidades = unidadServicios.ObtenerPorProyecto(Int32.Parse(ProyectosDDL.SelectedValue)).ToList<Unidad>();

                listUnidad = listUnidades.ToList<Unidad>();
                if (listaUnidad == null || listaUnidad.Count == 0)
                {
                    Session["partidasPorUnidadesProyectoPeriodo"] = null;
                }

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

        private void CargarPartidasPorUnidades()
        {

            ddlPartida.Items.Clear();

            if (listaUnidad.Count > 0)
            {
                foreach (Unidad unidad in listaUnidad)
                {

                    ListItem itemUnidad = new ListItem(unidad.nombreUnidad, unidad.idUnidad.ToString());
                    ddlPartida.Items.Add(itemUnidad);
                }


            }



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

            if (idUnidadElegida != 0)
            {
                List<Partida> partidasElegidas = new List<Partida>();
                List<Partida> partidaTemp = new List<Partida>();
                partidasElegidas = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
                List<PartidaUnidad> partidaUnidad = new List<PartidaUnidad>();
                List<PresupuestoEgreso> listaPartidasEgreso = new List<PresupuestoEgreso>();
                List<PresupuestoEgresoPartida> listaPresuouestosEgreso = new List<PresupuestoEgresoPartida>();
                List<PresupuestoEgresoPartida> listaPresupuestosEgreso = new List<PresupuestoEgresoPartida>();
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
                    int idPresupuestoEgreso = listaPartidasEgreso.Where(item => item.unidad.idUnidad == p.idUnidad).ToList().First().idPresupuestoEgreso;
                    presupuestoEgreso.idPresupuestoEgreso = idPresupuestoEgreso;
                    listaPresuouestosEgreso = presupuestoEgreso_PartidaServicios.getPresupuestoEgresoPartidas(presupuestoEgreso);

                    //Double montoDisponible = (Double)listaPresuouestosEgreso.Where(item => item.partida.numeroPartida == p.numeroPartida).ToList().FirstOrDefault().monto;
                    string idPartida = Convert.ToString(listaPresuouestosEgreso.Where(item => item.partida.numeroPartida == p.numeroPartida).ToList().FirstOrDefault().partida.idPartida);

                    Double montoDisponible = ejecucionServicios.ConsultaMontoDisponiblePartida(idPartida, Convert.ToString(idPresupuestoEgreso));
                    partidaU.MontoDisponible = montoDisponible;
                    partidaUnidad.Add(partidaU);

                }
                Session["partidasAsignadas"] = partidaUnidad;
                List<PartidaUnidad> partidasElegidasConMonto = (List<PartidaUnidad>)Session["partidasAsignadasConMonto"];
                if (partidasElegidasConMonto != null)
                {

                    List<PartidaUnidad> TempPartida = partidaUnidad.Where(a => !partidasElegidasConMonto.Any(a1 => a1.NumeroPartida == a.NumeroPartida && a1.IdUnidad == a.IdUnidad))
                    .Union(partidasElegidasConMonto.Where(a => !partidaUnidad.Any(a1 => a1.NumeroPartida == a.NumeroPartida && a1.IdUnidad == a.IdUnidad))).ToList();

                    Session["partidasAsignadas"] = TempPartida.Where(item => item.IdUnidad == idUnidadElegida).ToList();

                }
                partidaUnidad = (List<PartidaUnidad>)Session["partidasAsignadas"];

                var dt = partidaUnidad;
                pgsource.DataSource = dt;
                pgsource.AllowPaging = true;
                //numero de items que se muestran en el Repeater
                pgsource.PageSize = elmentosMostrar;
                pgsource.CurrentPageIndex = paginaActual;
                //mantiene el total de paginas en View State
                ViewState["TotalPaginas"] = pgsource.PageCount;
                //Ejemplo: "Página 1 al 10"
                lblpagina5.Text = "Página " + (paginaActual5 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
                //Habilitar los botones primero, último, anterior y siguiente
                lbAnterior5.Enabled = !pgsource.IsFirstPage;
                lbSiguiente5.Enabled = !pgsource.IsLastPage;
                lbPrimero5.Enabled = !pgsource.IsFirstPage;
                lbUltimo5.Enabled = !pgsource.IsLastPage;

                rpUnidadPartida.DataSource = pgsource;
                rpUnidadPartida.DataBind();
                contador++;


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
                if (tipoTramite != "")
                {
                    DDLTipoTramite.Items.FindByValue(tipoTramite.ToString()).Selected = true;
                }
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
            Session["partidasAsignadasConMonto"] = null;
            listaUnidad.RemoveAll(item => item.idUnidad > 0);
            MostrarDatosTablaUnidad(listaUnidad);
            MostrarUnidadesConMontoRepartido();
            count = 0;
            contador = 0;
            MostrarTablaRepartirGastos();
            CargarTramites();
            txtMontoIngresar.Text = "";
            numeroReferencia.Text = "";
            idEjecucioon = "";
        }

        /// <summary>
        ///Kevin Picado Quesada
        /// 04/febrero/20
        /// Efecto: muestra la Tabla en donde se visualiza las partidas elegidas con los montos,ademas esconde el boton ButtonRepartir.
        /// Modifica: datos de la tabla de plan estrategico y visibilidad de botones
        /// Devuelve: -
        /// </summary>
        private void MostrarTablaRepartirGastos()
        {
            List<Partida> partidasElegidas = new List<Partida>();
            partidasElegidas = (List<Partida>)Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"];
            if (listaUnidad.Count >= 1 && partidasElegidas != null)
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

        /*
      * Kevin Picado
      * 20/03/20
      * Efecto: elimina el archivo seleccionado en el modal activarModalVerEjecucionArchivo()
      * Requiere: -
      * Modifica: -
      * Devuelve: -
      */
        private void MostrarEjecucionBotonesLink()
        {

            LinkButton lnkViewUnidad = new LinkButton();
            LinkButton lnkViewPartida = new LinkButton();
            LinkButton lnkViewMontoPartida = new LinkButton();

            for (int i = 0; i < rpUnidadSelecionadas.Items.Count; i++)
            {


                lnkViewUnidad = (

                LinkButton)rpUnidadSelecionadas.Items[i].FindControl("btnEliminarUnidad");
                if (lnkViewUnidad != null)
                {


                    lnkViewUnidad.Visible = false;

                }
            }
            for (int i = 0; i < rpPartidasSeleccionada.Items.Count; i++)
            {


                lnkViewPartida = (

                LinkButton)rpPartidasSeleccionada.Items[i].FindControl("btnEliminarPartida");
                if (lnkViewPartida != null)
                {


                    lnkViewPartida.Visible = false;

                }
            }
            for (int i = 0; i < Repeater5.Items.Count; i++)
            {


                lnkViewMontoPartida = (

                LinkButton)Repeater5.Items[i].FindControl("btnEliminarPartida");
                if (lnkViewMontoPartida != null)
                {


                    lnkViewMontoPartida.Visible = false;

                }

            }
            if (!descripcionEjecucionOtro.Equals(""))
            {
                descripcionOtroTipoTramite.Text = descripcionEjecucionOtro;
                descripcionOtroTipoTramite.Visible = true;
                descripcionOtroTipoTramite.Enabled = false;
            }
            fuArchivos.Enabled = false;
            ButtonAsociar.Visible = false;
            ButtonAsociarPartida.Visible = false;
            ButtonRepartir.Visible = false;
            Button1.Visible = false;
            Button2.Visible = false;

            DDLTipoTramite.Enabled = false;
            txtMontoIngresar.Enabled = false;
            numeroReferencia.Enabled = false;
            ButtonRepartir.Enabled = false;
            PeriodosDDL.Enabled = false;
            ProyectosDDL.Enabled = false;
            BtnElimarEjecucion.Visible = true;
            BtnCerrar.Visible = true;
        }
        /*
      * Kevin Picado
      * 20/03/20
      * Efecto: Guardar el archivo selecionado
      * Requiere: -
      * Modifica: -
      * Devuelve: -
      */

        public List<ArchivoEjecucion> guardarArchivos(Entidades.Ejecucion ejecucion, FileUpload fuArchivos)
        {
            List<ArchivoEjecucion> listaArchivos = new List<ArchivoEjecucion>();

            String archivosRepetidos = "";

            foreach (HttpPostedFile file in fuArchivos.PostedFiles)
            {
                String nombreArchivo = Path.GetFileName(file.FileName);
                nombreArchivo = nombreArchivo.Replace(' ', '_');
                DateTime fechaHoy = DateTime.Now;
                String carpeta = Convert.ToString(ejecucion.idEjecucion + "-" + ProyectosDDL.SelectedItem.Text);

                int guardado = Utilidades.SaveFile(file, fechaHoy.Year, nombreArchivo, carpeta);

                if (guardado == 0)
                {

                    ArchivoEjecucion archivoEjecucion = new ArchivoEjecucion();
                    archivoEjecucion.idEjecucion = ejecucion.idEjecucion;
                    archivoEjecucion.nombreArchivo = nombreArchivo;
                    archivoEjecucion.rutaArchivo = Utilidades.path + fechaHoy.Year + "\\" + carpeta + "\\" + nombreArchivo;
                    archivoEjecucion.fechaCreacion = fechaHoy;
                    // archivoEjecucion.muestra = muestra;
                    archivoEjecucion.creadoPor = "Kevin";

                    listaArchivos.Add(archivoEjecucion);
                }
                else
                {
                    archivosRepetidos += "* " + nombreArchivo + ", \n";
                }
            }

            if (archivosRepetidos.Trim() != "")
            {
                archivosRepetidos = archivosRepetidos.Remove(archivosRepetidos.Length - 3);
                //(this.Master as SiteMaster).Mensaje("Los archivos " + archivosRepetidos + " no se pudieron guardar porque ya había archivos con ese nombre", "¡Alerta!");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Los archivos " + archivosRepetidos + " no se pudieron guardar porque ya había archivos con ese nombre" + "');", true);
            }

            return listaArchivos;
        }
        /*
      * Kevin Picado
      * 20/03/20
      * Efecto: cuando se elimina el archivo cuando se elimina una ejecucion
      * Requiere: -
      * Modifica: -
      * Devuelve: -
      */
        public void EliminarArchivo()
        {
            List<ArchivoEjecucion> listaArchivoEjecucion = (List<ArchivoEjecucion>)Session["listaArchivoEjecucion"];
            if (listaArchivoEjecucion != null || listaArchivoEjecucion.Count() != 0)
            {
                foreach (ArchivoEjecucion archivoEjecucion in listaArchivoEjecucion)
                {
                    string ruta = archivoEjecucion.rutaArchivo;
                    if (System.IO.File.Exists(@ruta))
                    {
                        try
                        {
                            System.IO.File.Delete(@ruta);

                        }
                        catch (Exception ex)
                        {
                            //(this.Master as SiteMaster).Mensaje("No se pudo eliminar el archivo", "¡Alerta!");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "No se pudo eliminar el archivo" + "');", true);
                        }
                    }

                    //archivoMuestraServicios.eliminarArchivoMuestra(archivoMuestra, (String)Session["nombreCompleto"]);
                }
                archivoEjecucionServicios.eliminarArchivoEjecucion(Convert.ToInt32(idEjecucioon));
            }
        }

        /*
       * Kevin Picado
       * 20/03/20
       * Efecto: elimina el archivo seleccionado en el modal activarModalVerEjecucionArchivo()
       * Requiere: -
       * Modifica: -
       * Devuelve: -
       */
        public void EliminarArchivoSeleccionado_Click(object sender, EventArgs e)
        {
            List<ArchivoEjecucion> listaArchivoEjecucion = (List<ArchivoEjecucion>)Session["listaArchivoEjecucion"];
            String nombreArchivo = (((LinkButton)(sender)).CommandArgument).ToString();
            if (listaArchivoEjecucion != null || listaArchivoEjecucion.Count() != 0)
            {
                foreach (ArchivoEjecucion archivoEjecucion in listaArchivoEjecucion)
                {
                    if (archivoEjecucion.nombreArchivo.Equals(nombreArchivo))
                    {
                        string ruta = archivoEjecucion.rutaArchivo;
                        if (System.IO.File.Exists(@ruta))
                        {
                            try
                            {
                                System.IO.File.Delete(@ruta);

                            }
                            catch (Exception ex)
                            {
                                //(this.Master as SiteMaster).Mensaje("No se pudo eliminar el archivo", "¡Alerta!");
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "No se pudo eliminar el archivo" + "');", true);
                            }
                        }
                    }
                    //archivoMuestraServicios.eliminarArchivoMuestra(archivoMuestra, (String)Session["nombreCompleto"]);
                }
                archivoEjecucionServicios.eliminarArchivoEjecucionPorNombreYId(Convert.ToInt32(idEjecucioon),nombreArchivo);
                Session["listaArchivoEjecucion"] = archivoEjecucionServicios.obtenerArchivoEjecucion(Convert.ToInt32(idEjecucioon));
                VerEjecucionArchivos();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalVerEjecucionArchivo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalVerEjecucionArchivo').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalVerEjecucionArchivo();", true);
            }
        }


        /*
       * Kevin Picado
       * 20/03/20
       * Efecto: Carga la tabla para ver los archivos que corresponde a una ejecucion
       * Requiere: -
       * Modifica: -
       * Devuelve: -
       */
        private void VerEjecucionArchivos()
        {
            List<ArchivoEjecucion> archivoEjecucion = (List<ArchivoEjecucion>)Session["listaArchivoEjecucion"];
            if (archivoEjecucion != null)
            {
                var dt = archivoEjecucion;
                pgsource.DataSource = dt;
                pgsource.AllowPaging = true;
                //numero de items que se muestran en el Repeater
                pgsource.PageSize = elmentosMostrar;
                pgsource.CurrentPageIndex = paginaActual7;
                //mantiene el total de paginas en View State
                ViewState["TotalPaginas2"] = pgsource.PageCount;
                //Ejemplo: "Página 1 al 10"
                lblpagina7.Text = "Página " + (paginaActual7 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
                //Habilitar los botones primero, último, anterior y siguiente
                lbAnterior7.Enabled = !pgsource.IsFirstPage;
                lbSiguiente7.Enabled = !pgsource.IsLastPage;
                lbPrimero7.Enabled = !pgsource.IsFirstPage;
                lbUltimo7.Enabled = !pgsource.IsLastPage;

                RepeaterArchivo.DataSource = pgsource;
                RepeaterArchivo.DataBind();

                //metodo que realiza la paginacion
                Paginacion7();
            }
            else
            {
                List<ArchivoEjecucion> archivoEjecucionn = new List<ArchivoEjecucion>();
                var dt = archivoEjecucionn;
                pgsource.DataSource = dt;
                pgsource.AllowPaging = true;
                //numero de items que se muestran en el Repeater
                pgsource.PageSize = elmentosMostrar;
                pgsource.CurrentPageIndex = paginaActual7;
                //mantiene el total de paginas en View State
                ViewState["TotalPaginas7"] = pgsource.PageCount;
                //Ejemplo: "Página 1 al 10"
                lblpagina7.Text = "Página " + (paginaActual7 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
                //Habilitar los botones primero, último, anterior y siguiente
                lbAnterior7.Enabled = !pgsource.IsFirstPage;
                lbSiguiente7.Enabled = !pgsource.IsLastPage;
                lbPrimero7.Enabled = !pgsource.IsFirstPage;
                lbUltimo7.Enabled = !pgsource.IsLastPage;

                RepeaterArchivo.DataSource = pgsource;
                RepeaterArchivo.DataBind();

                //metodo que realiza la paginacion
                Paginacion7();
            }


        }
        /*Kevin Picado
        * 20/03/20
        * Efecto: descarga el archivo para que el usuario lo pueda ver
        * Requiere: clic en el archivo
        * Modifica: -
        * Devuelve: -
        */
        protected void btnVerArchivo_Click(object sender, EventArgs e)
        {
            List<ArchivoEjecucion> archivoEjecucion = (List<ArchivoEjecucion>)Session["listaArchivoEjecucion"];
            //String[] infoArchivo = (((LinkButton)(sender)).CommandArgument).ToString().Split(',');
            //String nombreArchivo = infoArchivo[1];
            String rutaArchivo = (((LinkButton)(sender)).CommandArgument).ToString();
            string ruta = archivoEjecucion.Where(item => item.nombreArchivo.Equals(rutaArchivo)).ToList().First().rutaArchivo; 
            FileStream fileStream = new FileStream(ruta, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            Byte[] blobValue = binaryReader.ReadBytes(Convert.ToInt32(fileStream.Length));

            fileStream.Close();
            binaryReader.Close();

            descargar(rutaArchivo, ruta);
            VerEjecucionArchivos();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalVerEjecucionArchivo", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalVerEjecucionArchivo').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalVerEjecucionArchivo();", true);
        }
        /*
        * Kevin Picado
        * 20/03/20
        * Efecto: descarga el archivo para que el usuario lo pueda ver
        * Requiere: -
        * Modifica: -
        * Devuelve: -
        */
        private void descargar(string fileName,string ruta)
        {
            
            Process proceso = new Process();
            proceso.StartInfo.FileName = ruta;
            proceso.Start();


        }

        #endregion

    }
}
   
       