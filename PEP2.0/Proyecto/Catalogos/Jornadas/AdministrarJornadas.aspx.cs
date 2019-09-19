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

namespace Proyecto.Catalogos.Jornadas
{
    public partial class AdministrarJornadas : System.Web.UI.Page
    {
        #region variables globales
        JornadaServicios jornadaServicios = new JornadaServicios();
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
                Session["listaJornadas"] = null;
                Session["listaJornadasFiltrada"] = null;
                Session["jornadaSeleccionada"] = null;

                List<Jornada> listaJornadas = jornadaServicios.getJornadasActivas();

                Session["listaJornadas"] = listaJornadas;
                Session["listaJornadasFiltrada"] = listaJornadas;

                mostrarDatosTabla();
            }
        }
        #endregion

        #region logica
        /// <summary>
        /// Leonardo Carrion
        /// 18/sep/2019
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTabla()
        {
            List<Jornada> listaJornadas = (List<Jornada>)Session["listaJornadas"];

            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarDesc.Text))
            {
                desc = txtBuscarDesc.Text;
            }

            List<Jornada> listaJornadasFiltrada = (List<Jornada>)listaJornadas.Where(jornada => jornada.descJornada.ToUpper().Contains(desc.ToUpper())).ToList();

            Session["listaJornadasFiltrada"] = listaJornadasFiltrada;

            var dt = listaJornadasFiltrada;
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

            rpJornadas.DataSource = pgsource;
            rpJornadas.DataBind();

            //metodo que realiza la paginacion
            Paginacion();
        }
        #endregion

        #region paginacion
        /// <summary>
        /// Leonardo Carrion
        /// 18/sep/2019
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
        /// 18/sep/2019
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
        /// 18/sep/2019
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
        /// 18/sep/2019
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
        /// 18/sep/2019
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
        /// 18/sep/2019
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
        /// 18/sep/2019
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
        /// 18/sep/2019
        /// Efecto: filtra la tabla segun los datos ingresados en los filtros
        /// Requiere: dar clic en el boton de flitrar e ingresar datos en los filtros
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            paginaActual = 0;
            mostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 18/sep/2019
        /// Efecto: levanta modal para ingresar una nueva jornada
        /// Requiere: dar clic en el boton de "Nueva jornada"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaJornada_Click(object sender, EventArgs e)
        {
            txtDescModalNuevo.Text = "";
            txtPorcentajeModalNuevo.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaJornada();", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 19/sep/2019
        /// Efecto: inserta en la base de datos la jornada nueva
        /// Requiere: llenar campos de textos
        /// Modifica: tabla de jornadas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaJornadaModal_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtDescModalNuevo.Text))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaJornada", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaJornada').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaJornada();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Se debe ingresar una descripción" + "');", true);
            }
            else
            {
                Double porcentaje = 0;
                String txtSalario = txtPorcentajeModalNuevo.Text.Replace(".", ",");
                if (Double.TryParse(txtSalario, out porcentaje))
                {
                    txtPorcentajeModalNuevo.Text = porcentaje.ToString();
                }
                Jornada jornada = new Jornada();

                jornada.descJornada = txtDescModalNuevo.Text;
                jornada.porcentajeJornada = porcentaje;
                jornada.activo = true;

                jornadaServicios.insertarJornada(jornada);

                List<Jornada> listaJornadas = jornadaServicios.getJornadasActivas();

                Session["listaJornadas"] = listaJornadas;
                Session["listaJornadasFiltrada"] = listaJornadas;

                mostrarDatosTabla();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaJornada", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaJornada').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se ingreso la nueva jornada" + "');", true);
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 19/sep/2019
        /// Efecto:
        /// Requiere:
        /// Modifica:
        /// Devuelve:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBuscarDesc_TextChanged(object sender, EventArgs e)
        {
            paginaActual = 0;
            mostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 19/sep/2019
        /// Efecto: levanta modal para editar una jornada
        /// Requiere: dar clic en el boton de "Editar jornada"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int idJornada = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Jornada> listaJornadasFiltrada = (List<Jornada>)Session["listaJornadasFiltrada"];
            Jornada jornadaEditar = new Jornada();

            foreach (Jornada jornada in listaJornadasFiltrada)
            {
                if(idJornada == jornada.idJornada)
                {
                    jornadaEditar = jornada;
                    break;
                }
            }

            Session["jornadaSeleccionada"] = jornadaEditar;

            txtDescModalEditar.Text = jornadaEditar.descJornada;
            txtPorcentajeModalEditar.Text = jornadaEditar.porcentajeJornada.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarJornada();", true);
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 19/sep/2019
        /// Efecto: actualiza de forma logica la jornada seleccionada
        /// Requiere: llenar los campos requeridos y darle clic al boton de "Editar"
        /// Modifica: la jornada seleccionada
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditarJornadaModal_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtDescModalEditar.Text))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarJornada", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarJornada').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarJornada();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Se debe ingresar una descripción" + "');", true);
            }
            else
            {
                Double porcentaje = 0;
                String txtSalario = txtPorcentajeModalEditar.Text.Replace(".", ",");
                if (Double.TryParse(txtSalario, out porcentaje))
                {
                    txtPorcentajeModalEditar.Text = porcentaje.ToString();
                }

                Jornada jornada = (Jornada)Session["jornadaSeleccionada"];

                jornada.descJornada = txtDescModalEditar.Text;
                jornada.porcentajeJornada = porcentaje;
                jornada.activo = true;

                jornadaServicios.actualizarJornadaLogica(jornada);

                List<Jornada> listaJornadas = jornadaServicios.getJornadasActivas();

                Session["listaJornadas"] = listaJornadas;
                Session["listaJornadasFiltrada"] = listaJornadas;

                mostrarDatosTabla();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarJornada", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarJornada').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se actualizo la jornada correctamente" + "');", true);
            }
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 19/sep/2019
        /// Efecto: levanta modal para eliminar una jornada
        /// Requiere: dar clic en el boton de "Eliminar jornada"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int idJornada = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Jornada> listaJornadasFiltrada = (List<Jornada>)Session["listaJornadasFiltrada"];
            Jornada jornadaEliminar = new Jornada();

            foreach (Jornada jornada in listaJornadasFiltrada)
            {
                if (idJornada == jornada.idJornada)
                {
                    jornadaEliminar = jornada;
                    break;
                }
            }

            Session["jornadaSeleccionada"] = jornadaEliminar;

            txtDescModalEliminar.Text = jornadaEliminar.descJornada;
            txtPorcentajeModalEliminar.Text = jornadaEliminar.porcentajeJornada.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarJornada();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 19/sep/2019
        /// Efecto: elimina de forma logica la jornada seleccionada
        /// Requiere: dar clic al boton de "Eliminar"
        /// Modifica: el atributo de activo de la jornada seleccionada y quita la jornada de la tabla de jornadas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarJornadaModal_Click(object sender, EventArgs e)
        {
            Jornada jornada = (Jornada)Session["jornadaSeleccionada"];

            jornadaServicios.eliminarJornadaLogica(jornada);

            List<Jornada> listaJornadas = jornadaServicios.getJornadasActivas();

            Session["listaJornadas"] = listaJornadas;
            Session["listaJornadasFiltrada"] = listaJornadas;

            mostrarDatosTabla();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarJornada", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarJornada').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se elimino la jornada correctamente" + "');", true);
        }
        #endregion

    }
}