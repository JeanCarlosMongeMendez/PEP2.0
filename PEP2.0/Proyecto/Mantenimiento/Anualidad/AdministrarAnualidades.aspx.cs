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

namespace Proyecto.Mantenimiento.Anualidad
{
    public partial class AdministrarAnualidades : System.Web.UI.Page
    {
        #region variables globales
        AnualidadServicios anualidadServicios = new AnualidadServicios();
        PeriodoServicios periodoServicios = new PeriodoServicios();
        private static Entidades.Anualidad anualidadSeleccionada;
        #endregion

        #region paginacion
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

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);
            if (!IsPostBack)
            {
                Session["listaAnualidades"] = null;

                List<Entidades.Anualidad> listaAnualidades = anualidadServicios.getAnualidades();

                Session["listaAnualidades"] = listaAnualidades;

                mostrarDatosTabla();

                llenarDdlPeriodos();

                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodoModalNuevo.SelectedValue);
            }
        }
        #endregion

        #region logica
        /// <summary>
        /// Leonardo Carrion
        /// 10/jun/2019
        /// Efecto: llean el DropDownList con los periodos que se encuentran en la base de datos
        /// Requiere: - 
        /// Modifica: DropDownList
        /// Devuelve: -
        /// </summary>
        public void llenarDdlPeriodos()
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            ddlPeriodoModalNuevo.Items.Clear();
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
                    ddlPeriodoModalNuevo.Items.Add(itemPeriodo);
                }

                if (anoHabilitado != 0)
                {
                    ddlPeriodoModalNuevo.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                }
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTabla()
        {
            List<Entidades.Anualidad> listaAnualidades = (List<Entidades.Anualidad>)Session["listaAnualidades"];

            var dt = listaAnualidades;
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

            rpAnualidades.DataSource = pgsource;
            rpAnualidades.DataBind();

            //metodo que realiza la paginacion
            Paginacion();
        }
        #endregion

        #region paginacion
        /// <summary>
        /// Leonardo Carrion
        /// 11/jun/2019
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
        /// 01/nov/2019
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
        /// 01/nov/2019
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
        /// 01/nov/2019
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
        /// 01/nov/2019
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
        /// 01/nov/2019
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
        /// 01/nov/2019
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

        #region eventos

        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: levanta el modal para ingresar una nueva anualidad
        /// Requiere: dar clic en el boton de "Nueva anualidad"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaAnualidad_Click(object sender, EventArgs e)
        {
            txtPorcentajeModalNuevo.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaAnualidad();", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: Guarda en la base de datos la nueva anualidad
        /// Requiere: dar clic en el boton de "Guardar" y llenar los datos de anualidad
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaAnualidadModal_Click(object sender, EventArgs e)
        {
            String txtPorcentaje = txtPorcentajeModalNuevo.Text.Replace(".", ",");
            if (Double.TryParse(txtPorcentaje, out Double porcentaje))
            {
                txtPorcentajeModalNuevo.Text = porcentaje.ToString();
            }
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodoModalNuevo.SelectedValue);

            Entidades.Anualidad anualidad = new Entidades.Anualidad();
            anualidad.periodo = periodo;
            anualidad.porcentaje = porcentaje;

            List<Entidades.Anualidad> listaAnualidades = (List<Entidades.Anualidad>)Session["listaAnualidades"];
            List<Entidades.Anualidad> listaTemp = (List<Entidades.Anualidad>)listaAnualidades.Where(anualidadTemp => anualidadTemp.periodo.anoPeriodo==periodo.anoPeriodo).ToList();

            if (listaTemp.Count > 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaAnualidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaAnualidad').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaAnualidad();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Ya se encuentra una anualidad en el período seleccionado" + "');", true);
            }
            else
            {
                anualidadServicios.insertarAnualidad(anualidad);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaAnualidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaAnualidad').hide();", true);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se guardo correctamente la anualidad" + "');", true);
                List<Entidades.Anualidad> listaAnualidades2 = anualidadServicios.getAnualidades();

                Session["listaAnualidades"] = listaAnualidades2;
                mostrarDatosTabla();
            }
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: levanta modal para editar las anualidades
        /// Requiere: dar clic el boton de "Editar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int idAnualidad = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Entidades.Anualidad> listaAnualidades = (List<Entidades.Anualidad>)Session["listaAnualidades"];

            foreach (Entidades.Anualidad anualidad in listaAnualidades)
            {
                if (anualidad.idAnualidad == idAnualidad)
                {
                    anualidadSeleccionada = anualidad;
                    break;
                }
            }

            txtEditarModalEditar.Text = anualidadSeleccionada.periodo.anoPeriodo.ToString();
            txtPorcentajeModalEditar.Text =anualidadSeleccionada.porcentaje.ToString();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarAnualidad();", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 01/nov/2019
        /// Efecto: modifca la anulidad seleccionada
        /// Requiere: dar clic al boton de "Actualizar" y cambiar datos de la anualidad
        /// Modifica: datos de la anualidad seleccionada
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditarAnualidadModal_Click(object sender, EventArgs e)
        {
            String txtPorcentaje = txtPorcentajeModalEditar.Text.Replace(".", ",");
            if (Double.TryParse(txtPorcentaje, out Double porcentaje))
            {
                txtPorcentajeModalEditar.Text = porcentaje.ToString();
            }

            anualidadSeleccionada.porcentaje = porcentaje;

            anualidadServicios.actualizarAnualidad(anualidadSeleccionada);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarAnualidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarAnualidad').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se actualizo correctamente la anualidad" + "');", true);
            List<Entidades.Anualidad> listaAnualidades2 = anualidadServicios.getAnualidades();

            Session["listaAnualidades"] = listaAnualidades2;
            mostrarDatosTabla();
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: levanta modal para eliminar las anualidades
        /// Requiere: dar clic el boton de "Eliminar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int idAnualidad = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Entidades.Anualidad> listaAnualidades = (List<Entidades.Anualidad>)Session["listaAnualidades"];

            foreach (Entidades.Anualidad anualidad in listaAnualidades)
            {
                if (anualidad.idAnualidad == idAnualidad)
                {
                    anualidadSeleccionada = anualidad;
                    break;
                }
            }

            txtEditarModalEliminar.Text = anualidadSeleccionada.periodo.anoPeriodo.ToString();
            txtPorcentajeModalEliminar.Text = anualidadSeleccionada.porcentaje.ToString();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarAnualidad();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/nov/2019
        /// Efecto: elimina de la base de datos la anualidad
        /// Requiere: dar clic en el boton de "Eliminar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarAnualidadModal_Click(object sender, EventArgs e)
        {
            anualidadServicios.eliminarAnualidad(anualidadSeleccionada);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarAnualidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarAnualidad').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se elimino correctamente la anualidad" + "');", true);
            List<Entidades.Anualidad> listaAnualidades2 = anualidadServicios.getAnualidades();

            Session["listaAnualidades"] = listaAnualidades2;
            mostrarDatosTabla();
        }
        #endregion
    }
}