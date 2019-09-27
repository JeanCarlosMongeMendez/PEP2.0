using Entidades;
using PEP;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.UI;

namespace Proyecto.Catalogos.Partidas
{
    public partial class AdministrarPartidas : System.Web.UI.Page
    {

        #region variables globales
        PeriodoServicios periodoServicios = new PeriodoServicios();
        PartidaServicios partidaServicios = new PartidaServicios();
        readonly PagedDataSource pgsource = new PagedDataSource();
        #endregion

        #region page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);
            if (!IsPostBack)
            {
                Session["listaPartidas"] = null;
                Session["listaPartidasFiltrada"] = null;

                Session["listaPartidaAPasar"] = null;
                Session["listaPartidaAPasarFiltrada"] = null;

                Session["listaPartidaAgregadas"] = null;
                Session["listaPartidaAgregadasFiltrada"] = null;

                llenarDdlPeriodos(ddlPeriodo);

                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);


                LinkedList<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(Int32.Parse(ddlPeriodo.SelectedValue));

                Session["listaPartidas"] = listaPartidas;
                Session["listaPartidasFiltrada"] = listaPartidas;

                mostrarDatosTabla();

            }
        }
        #endregion

        #region eventos
        /// <summary>
        /// Jesús Torres
        /// 19/sept/2019
        /// Efecto: Selecciona un periodo, de acuerdo al periodo seleccionada se llenará la tabla con los datos correspondientes
        /// Requiere: 
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            LinkedList<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(periodo.anoPeriodo);

            Session["listaPartidas"] = listaPartidas;
            Session["listaPartidasFiltrada"] = listaPartidas;

            mostrarDatosTabla();
        }

        protected void btnPartidas_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Jesús Torres
        /// 20/sept/2019
        /// Efecto: permite mostrar el modal de ingreso de partidas
        /// Requiere: dar clic en el boton de nueva partida para mostrar modal
        /// Modifica: limpia los text que se encuantrar en este modal, carga los DropDownList de periodo y partidas padre
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaPartida_Click(object sender, EventArgs e)
        {
            txtNumeroPartidas.Text = "";
            txtNumeroPartidas.CssClass = "form-control";
            txtDescripcionPartida.Text = "";
            txtDescripcionPartida.CssClass = "form-control";
            llenarDdlPeriodos(ddlPeriodoModal);

            Session["listaPartidasPorPeriodo"] = partidaServicios.ObtenerPorPeriodo(Convert.ToInt32(ddlPeriodoModal.SelectedValue));

            llenarPartidasPadreDDL(ddlPartidasPadre);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaPartida();", true);
        }
        /// <summary>
        /// Jesús Torres
        /// 19/sept/2019
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
        /// Jesús Torres
        /// 19/sept/2019
        /// Efecto: filtra la tabla segun los datos ingresados en los filtros
        /// Requiere: accede al evento con enter cuando se quiere buscar algo especifico de la tabla
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBuscarDesc_TextChanged(object sender, EventArgs e)
        {
            paginaActual = 0;
            mostrarDatosTabla();
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            String nombrePartidaPadre = "Partida Padre";
            lbPartidaPadreModalEliminar.Text = nombrePartidaPadre;
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            LinkedList<Partida> listaPartidaFiltrada = (LinkedList<Partida>)Session["listaPartidasFiltrada"];
            Partida partidaEditar = new Partida();

            foreach (Partida partida in listaPartidaFiltrada)
            {
                if (idPartida == partida.idPartida)
                {
                    partidaEditar = partida;
                    break;
                }
            }
            Session["partidaSeleccionada"] = partidaEditar;
            //if que determina si el valir del partida padre es igual a null, en caso de no serlo, buscara el nombre mas adelante
            if (partidaEditar.partidaPadre != null)
            {
                lbPartidaPadreModalModificar.Text = partidaEditar.partidaPadre.descripcionPartida;
            }
            
            lbPeriodoModalModificar.Text = partidaEditar.periodo.anoPeriodo.ToString();
            txtNumeroPartidasModalModificar.Text = partidaEditar.numeroPartida;
            txtNumeroPartidasModalModificar.CssClass = "form-control";
            txtDescripcionPartidaModalModificar.Text = partidaEditar.descripcionPartida;
            txtDescripcionPartidaModalModificar.CssClass = "form-control";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalModificarPartida();", true);
        }

        /// <summary>
        /// Jesús Torres
        /// 26/sept/2019
        /// Efecto: Muestra el modal de eliminar una partida, se llenan los datos del valor seleccionado
        /// Requiere:
        /// Modifica: 
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            String nombrePartidaPadre = "Partida Padre";
            lbPartidaPadreModalEliminar.Text = nombrePartidaPadre;
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            LinkedList<Partida> listaPartidaFiltrada = (LinkedList<Partida>)Session["listaPartidasFiltrada"];
            Partida partidaEditar = new Partida();

            foreach (Partida partida in listaPartidaFiltrada)
            {
                if (idPartida == partida.idPartida)
                {
                    partidaEditar = partida;
                    break;
                }
            }
            Session["partidaSeleccionada"] = partidaEditar;
            //if que determina si el valir del partida padre es igual a null, en caso de no serlo, buscara el nombre mas adelante
            if (partidaEditar.partidaPadre != null)
            {
                //if que determina si el valir del partida padre es igual a null, en caso de no serlo, buscara el nombre mas adelante
                if (partidaEditar.partidaPadre != null)
                {
                    lbPartidaPadreModalModificar.Text = partidaEditar.partidaPadre.descripcionPartida;
                }
            }

            lbPeriodoModalEliminar.Text = partidaEditar.periodo.anoPeriodo.ToString();
            txtNumeroPartidasModalElimina.Text = partidaEditar.numeroPartida;
            txtDescripcionPartidaModalEliminar.Text = partidaEditar.descripcionPartida;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEliminarPartida();", true);
        }

        /// <summary>
        /// Jesús Torres
        /// 19/sept/2019
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
        /// Jesús Torres
        /// 19/sept/2019
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
        /// Jesús Torres
        /// 20/sept/2019
        /// Efecto: inserta en BD, una nueva partida
        /// Requiere: dar clic al boton de Guardar
        /// Modifica: 
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaPartidaModal_Click(object sender, EventArgs e)
        {
            //se validan los campos antes de guardar los datos en la base de datos
            if (validarCampos())
            {
                Partida partida = new Partida();
                partida.numeroPartida = txtNumeroPartidas.Text;
                partida.descripcionPartida = txtDescripcionPartida.Text;
                partida.periodo = new Periodo();
                partida.periodo.anoPeriodo = Convert.ToInt32(ddlPeriodoModal.SelectedValue);

                if (ddlPartidasPadre.SelectedValue == "null")
                {
                    partida.partidaPadre = null;
                }
                else
                {
                    partida.partidaPadre = new Partida();
                    partida.partidaPadre.idPartida = Convert.ToInt32(ddlPartidasPadre.SelectedValue);
                }
                try
                {
                    int idPartida = this.partidaServicios.Insertar(partida);
                    Periodo periodo = new Periodo();
                    periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);
                    LinkedList<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(periodo.anoPeriodo);

                    Session["listaPartidas"] = listaPartidas;
                    Session["listaPartidasFiltrada"] = listaPartidas;

                    mostrarDatosTabla();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaPartida').hide();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se ingreso la partida correctamente" + "');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Error en la inserción, intentelo denuevo" + "');", true);
                }

            }
            else
            {

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaPartida').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaPartida();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Todos los espacios son requeridos" + "');", true);
            }
        
        }

        /// <summary>
        /// Jesús Torres
        /// 20/sept/2019
        /// Efecto: Actualiza los DropDownList de partidas padre
        /// Requiere: 
        /// Modifica: el contenido de el DropDownList padre
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPeriodoModal_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["listaPartidasPorPeriodo"] = partidaServicios.ObtenerPorPeriodo(Convert.ToInt32(ddlPeriodoModal.SelectedValue));
            llenarPartidasPadreDDL(ddlPartidasPadre);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaPartida').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaPartida();", true);
        }


        /// <summary>
        /// Jesús Torres
        /// 26/sept/2019
        /// Efecto: modifica una partida seleccionada, 
        /// Requiere: 
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnModificar_Click(object sender, EventArgs e)
        {
            //se validan los campos antes de guardar los datos en la base de datos
            if (validarCamposModificar())
            {
                Partida partida = (Partida)Session["partidaSeleccionada"];
                partida.numeroPartida = txtNumeroPartidasModalModificar.Text;
                partida.descripcionPartida = txtDescripcionPartidaModalModificar.Text;
            
                try
                {
                    this.partidaServicios.ActualizarPartida(partida);
                    Periodo periodo = new Periodo();
                    periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);
                    LinkedList<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(periodo.anoPeriodo);

                    Session["listaPartidas"] = listaPartidas;
                    Session["listaPartidasFiltrada"] = listaPartidas;

                    mostrarDatosTabla();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalModificarPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalModificarPartida').hide();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se modificó la partida correctamente" + "');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Error en la modificación, intentelo denuevo" + "');", true);
                }

            }
            else
            {

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalModificarPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalModificarPartida').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalModificarPartida();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Todos los espacios son requeridos" + "');", true);
            }
        }

        /// <summary>
        /// Jesús Torres
        /// 26/sept/2019
        /// Efecto: Elimina una partid seleccionada
        /// Requiere: 
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarPartidaModal_Click(object sender, EventArgs e)
        {
            Partida partida = (Partida)Session["partidaSeleccionada"];

            partidaServicios.EliminarPartida(partida.idPartida);
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            LinkedList<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(periodo.anoPeriodo);
            Session["listaPartidas"] = listaPartidas;
            Session["listaPatidasFiltrada"] = listaPartidas;

            mostrarDatosTabla();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarPartida').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se elimino la partida correctamente" + "');", true);
        }

        #endregion

        #region metodos
        /// <summary>
        /// Jesús Torres 
        /// 19/sept/2019
        /// Efecto: llena el DropDownList con los periodos que se encuentran en la base de datos
        /// Requiere: - 
        /// Modifica: DropDownList
        /// Devuelve: -
        /// </summary>
        public void llenarDdlPeriodos(DropDownList ddl)
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            ddl.Items.Clear();
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
                    ddl.Items.Add(itemPeriodo);
                }

                if (anoHabilitado != 0)
                {  
                    ddl.Items.FindByValue(ddlPeriodo.SelectedValue).Selected = true;
                }
            }
        }

        /// <summary>
        /// Jesús Torres
        /// 19/sept/2019
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        private void mostrarDatosTabla()
        {
            LinkedList<Partida> listaPartidas = (LinkedList<Partida>)Session["listaPartidas"];

            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarDesc.Text))
            {
                desc = txtBuscarDesc.Text;
            }

            List<Partida> listaPartidasFiltrada = (List<Partida>)listaPartidas.Where(partida => partida.descripcionPartida.ToUpper().Contains(desc.ToUpper())).ToList();

            Session["listaPartidaFiltrada"] = listaPartidasFiltrada;

            var dt = listaPartidasFiltrada;
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
            rpPartidas.DataSource = pgsource;
            rpPartidas.DataBind();
            //metodo que realiza la paginacion
            Paginacion();
        }

        /// <summary>
        /// Jesús Torres
        /// 20/sept/2019
        /// Efecto: carga los de padres de las partidas a el DropDownList correspondiente 
        /// Requiere: -
        /// Modifica: los datos mostrados en DropDownList de padres de partidas
        /// Devuelve: -
        /// </summary>
        protected void llenarPartidasPadreDDL(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Partida Padre", "null"));

            LinkedList<Partida> partidas = new LinkedList<Partida>();
            partidas = (LinkedList<Partida>)Session["listaPartidasPorPeriodo"];
            foreach (Partida partida in partidas)
            {
                if (partida.partidaPadre == null)
                {
                    ListItem item = new ListItem(partida.numeroPartida + ": " + partida.descripcionPartida, partida.idPartida.ToString());
                    ddl.Items.Add(item);
                }
            }
        }



        /// <summary>
        /// Jesús Torres
        /// 20/sept/2019
        /// Efecto: valida los datos ingresados para una nueva partida 
        /// Requiere: -
        /// Modifica: 
        /// Devuelve: -Boolean true si aplica, false en caso de no
        /// </summary>
        public Boolean validarCampos()
        {
            Boolean validados = true;

            
            if (ddlPeriodo.SelectedIndex.Equals(null))
            {
                validados = false;
            }
            
            String numeroPartida = txtNumeroPartidas.Text;

            if (numeroPartida.Trim() == "" || numeroPartida.Length > 255)
            {
                txtNumeroPartidas.CssClass = "form-control alert-danger";
                validados = false;
            }

            String descripcionPartida = txtDescripcionPartida.Text;

            if (descripcionPartida.Trim() == "" || descripcionPartida.Length > 255)
            {
                txtDescripcionPartida.CssClass = "form-control alert-danger";
                validados = false;
            }

            return validados;
        }

        /// <summary>
        /// Jesús Torres
        /// 26/sept/2019
        /// Efecto: valida los datos ingresados para modificar partida 
        /// Requiere: -
        /// Modifica: 
        /// Devuelve: -Boolean true si aplica, false en caso de no
        /// </summary>
        public Boolean validarCamposModificar()
        {
            Boolean validados = true;


            String numeroPartida = txtNumeroPartidasModalModificar.Text;

            if (numeroPartida.Trim() == "" || numeroPartida.Length > 255)
            {
                txtNumeroPartidasModalModificar.CssClass = "form-control alert-danger";
                validados = false;
            }

            String descripcionPartida = txtDescripcionPartidaModalModificar.Text;

            if (descripcionPartida.Trim() == "" || descripcionPartida.Length > 255)
            {
                txtDescripcionPartidaModalModificar.CssClass = "form-control alert-danger";
                validados = false;
            }

            return validados;
        }



        #endregion

        #region paginacion Actual
        //Bloque que contiene get y set de pagina actual
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

        #region paginacion
        /// <summary>
        /// Jesús Torres
        /// 19/sept/2019
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
        /// Jesús Torres
        /// 19/sept/2019
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
        /// Jesús Torres
        /// 19/sept/2019
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
        /// Jesús Torres 
        /// 19/sept/2019
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
        /// Jesús Torres
        /// 19/sep/2019
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



    }
}