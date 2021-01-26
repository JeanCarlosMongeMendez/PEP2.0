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

namespace PEP
{
    public partial class Default : System.Web.UI.Page
    {
        #region variables globales
        ContactoServicios contactoServicios = new ContactoServicios();
        #endregion

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

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2,13};
            Utilidades.escogerMenu(Page, rolesPermitidos);
        }
        #endregion

        #region logica

        #region prueba

        /// <summary>
        /// Leonardo Carrion
        /// 10/abr/2019
        /// Efecto: carga los datos filtrados en la tabla de contactos y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        //private void mostrarDatosTabla()
        //{

        //    List<Contacto> listaSession = (List<Contacto>)Session["listaContactos"];
        //    String id = "";
        //    String nombre = "";
        //    if (ViewState["id"] != null)
        //        id = (String)ViewState["id"];

        //    if (ViewState["nombreContacto"] != null)
        //        nombre = (String)ViewState["nombreContacto"];

        //    List<Contacto> listaContactos = (List<Contacto>)listaSession.Where(x => x.nombreContacto.ToUpper().Contains(nombre.ToUpper()) && x.idContacto.ToString().Contains(id)).ToList();
        //    Session["listaContactosFiltrada"] = listaContactos;

        //    var dt = listaContactos;
        //    pgsource.DataSource = dt;
        //    pgsource.AllowPaging = true;
        //    //numero de items que se muestran en el Repeater
        //    pgsource.PageSize = elmentosMostrar;
        //    pgsource.CurrentPageIndex = paginaActual;
        //    //mantiene el total de paginas en View State
        //    ViewState["TotalPaginas"] = pgsource.PageCount;
        //    //Ejemplo: "Página 1 al 10"
        //    lblpagina.Text = "Página " + (paginaActual + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
        //    //Habilitar los botones primero, último, anterior y siguiente
        //    lbAnterior.Enabled = !pgsource.IsFirstPage;
        //    lbSiguiente.Enabled = !pgsource.IsLastPage;
        //    lbPrimero.Enabled = !pgsource.IsFirstPage;
        //    lbUltimo.Enabled = !pgsource.IsLastPage;

        //    rpContacto.DataSource = pgsource;
        //    rpContacto.DataBind();

        //    //metodo que realiza la paginacion
        //    Paginacion();
        //}

        /// <summary>
        /// Leonardo Carrion
        /// 10/abr/2019
        /// Efecto: realiza la paginacion
        /// Requiere: -
        /// Modifica: paginacion mostrada en pantalla
        /// Devuelve: -
        /// </summary>
        //private void Paginacion()
        //{
        //    var dt = new DataTable();
        //    dt.Columns.Add("IndexPagina"); //Inicia en 0
        //    dt.Columns.Add("PaginaText"); //Inicia en 1

        //    primerIndex = paginaActual - 2;
        //    if (paginaActual > 2)
        //        ultimoIndex = paginaActual + 2;
        //    else
        //        ultimoIndex = 4;

        //    //se revisa que la ultima pagina sea menor que el total de paginas a mostrar, sino se resta para que muestre bien la paginacion
        //    if (ultimoIndex > Convert.ToInt32(ViewState["TotalPaginas"]))
        //    {
        //        ultimoIndex = Convert.ToInt32(ViewState["TotalPaginas"]);
        //        primerIndex = ultimoIndex - 4;
        //    }

        //    if (primerIndex < 0)
        //        primerIndex = 0;

        //    //se crea el numero de paginas basado en la primera y ultima pagina
        //    for (var i = primerIndex; i < ultimoIndex; i++)
        //    {
        //        var dr = dt.NewRow();
        //        dr[0] = i;
        //        dr[1] = i + 1;
        //        dt.Rows.Add(dr);
        //    }

        //    rptPaginacion.DataSource = dt;
        //    rptPaginacion.DataBind();
        //}

        /// <summary>
        /// Leonardo Carrion
        /// 10/abr/2019
        /// Efecto: se devuelve a la primera pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Primer pagina"
        /// Modifica: elementos mostrados en la tabla de contactos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbPrimero_Click(object sender, EventArgs e)
        //{
        //    paginaActual = 0;
        //    mostrarDatosTabla();
        //}

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
        //protected void lbUltimo_Click(object sender, EventArgs e)
        //{
        //    paginaActual = (Convert.ToInt32(ViewState["TotalPaginas"]) - 1);
        //    mostrarDatosTabla();
        //}

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
        //protected void lbAnterior_Click(object sender, EventArgs e)
        //{
        //    paginaActual -= 1;
        //    mostrarDatosTabla();
        //}

        /// <summary>
        /// Leonardo Carrion
        /// 10/abr/2019
        /// Efecto: se devuelve a la pagina siguiente y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Siguiente pagina"
        /// Modifica: elementos mostrados en la tabla de contactos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbSiguiente_Click(object sender, EventArgs e)
        //{
        //    paginaActual += 1;
        //    mostrarDatosTabla();
        //}

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
        //protected void rptPaginacion_ItemCommand(object source, DataListCommandEventArgs e)
        //{
        //    if (!e.CommandName.Equals("nuevaPagina")) return;
        //    paginaActual = Convert.ToInt32(e.CommandArgument.ToString());
        //    mostrarDatosTabla();
        //}

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

        #endregion

        #region eventos
        #endregion


    }
}