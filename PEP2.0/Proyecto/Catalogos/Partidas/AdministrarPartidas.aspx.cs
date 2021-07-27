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
        int primerIndex, ultimoIndex, primerIndex2, ultimoIndex2, primerIndex3, ultimoIndex3;
        private int elmentosMostrar = 10;
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

                llenarDdlPeriodos();

                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);


                List<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(Int32.Parse(ddlPeriodo.SelectedValue));

                Session["listaPartidas"] = listaPartidas;
                Session["listaPartidasFiltrada"] = listaPartidas;

                llenarTipoBuscar();
                mostrarDatosTabla();
                llenarTipoPartidasAgregados();
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

            List<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(periodo.anoPeriodo);

            Session["listaPartidas"] = listaPartidas;
            Session["listaPartidasFiltrada"] = listaPartidas;

            mostrarDatosTabla();
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
            //llenarDdlPeriodos();
            ddlPeriodoModal.SelectedIndex = ddlPeriodo.SelectedIndex;
            Session["listaPartidasPorPeriodo"] = partidaServicios.ObtenerPorPeriodo(Convert.ToInt32(ddlPeriodo.SelectedValue));
            List<Partida> lista =( List<Partida>)Session["listaPartidasPorPeriodo"];
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
        protected void ddlBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBuscarDesc.Text = "";
            paginaActual = 0;
            int anioPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue.ToString());
            List<Partida> listaPartidas = new List<Partida>();

            if (!ddlBuscarTipo.SelectedValue.ToString().Equals("null"))
            {
                Boolean tipoPartida = Convert.ToBoolean(ddlBuscarTipo.SelectedValue.ToString());
                listaPartidas = partidaServicios.obtenerPorTipoPartidaYPeriodo(tipoPartida,anioPeriodo);
                Session["listaPartidas"] = listaPartidas;
                Session["listaPartidasFiltrada"] = listaPartidas;
                mostrarDatosTabla();

            }
            else
            {
                listaPartidas = partidaServicios.ObtenerPorPeriodo(anioPeriodo);
                Session["listaPartidas"] = listaPartidas;
                Session["listaPartidasFiltrada"] = listaPartidas;
                mostrarDatosTabla();
            }
           

        }

        /// Mariela Calvo
        /// octubre/2019
        /// Efecto: filtra la tabla segun los datos seleccionados de la tabla partidas a pasar
        /// Requiere: accede al evento seleccionado un tipo de partida en el dropdown
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBuscarApasar_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBuscarDescPartidasAPasar.Text = "";
            paginaActual = 0;
            int anioPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue.ToString());
            List<Partida> listaPartidas = new List<Partida>();
            if (!ddlBuscarApasar.SelectedValue.ToString().Equals("null"))
            {
                Boolean tipoPartida = Convert.ToBoolean(ddlBuscarApasar.SelectedValue.ToString());
                listaPartidas = partidaServicios.obtenerPorTipoPartidaYPeriodo(tipoPartida, anioPeriodo);
                Session["listaPartidasAPasar"] = listaPartidas;
                Session["listaPartidasAPasarFiltrada"] = listaPartidas;
                cargarDatosTblPartidasAPasar();

            }
            else
            {
                listaPartidas = partidaServicios.ObtenerPorPeriodo(anioPeriodo);
                Session["listaPartidas"] = listaPartidas;
                Session["listaPartidasFiltrada"] = listaPartidas;
                cargarDatosTblPartidasAPasar();
            }


        }
        /// Mariela Calvo
        /// octubre/2019
        /// Efecto: filtrar la tabla segun los datos seleccionados de la tabla de partidas agregadas
        /// Requiere: accede al evento seleccionado un tipo de partida en el dropdown
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBuscarAgregadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBuscarDescPartidasAgregadas.Text = "";
            paginaActual = 0;
            List<Partida> listaPartidas = new List<Partida>(); 
           int anioPeriodo = Convert.ToInt32(ddlPeriodoModalPasaPartidas.SelectedValue.ToString());

            if (!ddlBuscarAgregadas.SelectedValue.ToString().Equals("null"))
            {
                Boolean tipoPartida = Convert.ToBoolean(ddlBuscarAgregadas.SelectedValue.ToString());
                listaPartidas = partidaServicios.obtenerPorTipoPartidaYPeriodo(tipoPartida, anioPeriodo);
                Session["listaPartidasAgregadas"] = listaPartidas;
                Session["listaPartidasAgregadasFiltrada"] = listaPartidas;
                cargarDatosTblPartidasAgregadas();

            }
            else
            {
                listaPartidas = partidaServicios.ObtenerPorPeriodo(anioPeriodo);
                Session["listaPartidasAgregadas"] = listaPartidas;
                Session["listaPartidasAgregadasFiltrada"] = listaPartidas;
                cargarDatosTblPartidasAgregadas();
            }


        }

        /// <summary>
        /// Jesús Torres
        /// 04/oct/2019
        /// Efecto: permite mostrar el modal de editar una partida seleccionada
        /// Requiere: 
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            String nombrePartidaPadre = "Partida Padre";
            lbPartidaPadreModalModificar.Text = nombrePartidaPadre;
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            List<Partida> listaPartidaFiltrada = (List<Partida>)Session["listaPartidasFiltrada"];
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
            //txtNumeroPartidasModalModificar.CssClass = "form-control";
            txtDescripcionPartidaModalModificar.Text = partidaEditar.descripcionPartida;
            //txtDescripcionPartidaModalModificar.CssClass = "form-control";
            

            if (partidaEditar.esUCR)
            {
                lbPartidaTipoMod.Text = "UCR";

            }
            else
            {
                lbPartidaTipoMod.Text = "FundacionUCR";
            }

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
            List<Partida> listaPartidaFiltrada = (List<Partida>)Session["listaPartidasFiltrada"];
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
                lbPartidaPadreModalEliminar.Text = partidaEditar.partidaPadre.descripcionPartida;
                }      

            lbPeriodoModalEliminar.Text = partidaEditar.periodo.anoPeriodo.ToString();
            txtNumeroPartidasModalElimina.Text = partidaEditar.numeroPartida;
            txtDescripcionPartidaModalEliminar.Text = partidaEditar.descripcionPartida;

            if (partidaEditar.esUCR)
            {
                lbTipoPartidaElm.Text = "UCR";
            }
            else
            {
                lbTipoPartidaElm.Text = "FundacionUCR";
            }
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
                partida.esUCR= Convert.ToBoolean(ddlPartidasUCR.SelectedValue);

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
                    List<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(periodo.anoPeriodo);

                    Session["listaPartidas"] = listaPartidas;
                    Session["listaPartidasFiltrada"] = listaPartidas;

                    mostrarDatosTabla();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalNuevaPartida();", true);
                    
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaPartida').hide();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se ingreso la partida correctamente" + "');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Error en la inserción, intentelo denuevo" + "');", true);
                }

            }
            else
            {

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaPartida').hide();", true);
                
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
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaPartida').hide();", true);
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
                    List<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(periodo.anoPeriodo);

                    Session["listaPartidas"] = listaPartidas;
                    Session["listaPartidasFiltrada"] = listaPartidas;

                    mostrarDatosTabla();
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalModificarPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalModificarPartida').hide();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se modificó la partida correctamente" + "');", true);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalModificarPartida();", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Error en la modificación, intentelo denuevo" + "');", true);
                }

            }
            else
            {

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalModificarPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalModificarPartida').hide();", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalModificarPartida();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Todos los espacios son requeridos" + "');", true);
            }
        }
        /// <summary>
        /// Mariela Calvo
        /// 21/octubre/2019
        /// Efecto: Pide confirmar eliminar la partid seleccionada
        /// Requiere: 
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void btnConfirmarEliminarPartida(Object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalConfirmarPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalConfirmarPartida').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalConfirmarPartida()", true);
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
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            partidaServicios.EliminarPartida(partida.idPartida, periodo.anoPeriodo);
            
            List<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(periodo.anoPeriodo);
            Session["listaPartidas"] = listaPartidas;
            Session["listaPatidasFiltrada"] = listaPartidas;

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEliminarPartida", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEliminarPartida').hide();", true);
            mostrarDatosTabla();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se elimino la partida correctamente" + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalConfirmarPartida();", true);
        }

        /// <summary>
        /// Jesús Torres
        /// 04/oct/2019
        /// Efecto: Abre el modal que permite pasr partidas a el periodo seleccionado
        /// Requiere: 
        /// Modifica: 
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPasarPartidas_Click(object sender, EventArgs e)
        {
            txtBuscarDescPartidasAPasar.Text = "";
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);
            lblPeriodoSeleccionado.Text = periodo.anoPeriodo.ToString();


            // cargar periodos en dropdownlist
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            ddlPeriodoModalPasaPartidas.Items.Clear();
            periodos = this.periodoServicios.ObtenerTodos();
            int anoHabilitado = 0;

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
                        ddlPeriodoModalPasaPartidas.Items.Add(itemPeriodo);
                    }
                }

            }
            //fin de dopdownlist


            //Determina los valores a cargar de la lista de partidas a pasar
            List<Partida> listaPartidas = partidaServicios.ObtenerPorPeriodo(periodo.anoPeriodo);
            Session["listaPartidasAPasar"] = listaPartidas;
            Session["listaPartidasAPasarFiltrada"] = listaPartidas;
            cargarDatosTblPartidasAPasar();

            //Determina los valores a cargar de la lista de partidas Agregadas
            Periodo periodoAgregados = new Periodo();
            periodoAgregados.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasaPartidas.SelectedValue);

            List<Partida> listaPartidasAgregadas = partidaServicios.ObtenerPorPeriodo(periodoAgregados.anoPeriodo);

            Session["listaPartidasAgregadas"] = listaPartidasAgregadas;
            Session["listaPartidasAgregadasFiltrada"] = listaPartidasAgregadas;

            cargarDatosTblPartidasAgregadas();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarPartida();", true);
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
        /// Jessus Torres
        /// 03/oct/2019
        /// Efecto: se devuelve a la primera pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Primer pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPrimero2_Click(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            cargarDatosTblPartidasAPasar();
        }

        /// <summary>
        /// Jesus Torres
        /// 03/oct/2019
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
            cargarDatosTblPartidasAPasar();
        }

        /// <summary>
        /// Jesus Torres
        /// 03/oct/2019
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
            cargarDatosTblPartidasAPasar();
        }
        /// <summary>
        ///  Jesus Torres
        /// 03/oct/2019
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo2_Click(object sender, EventArgs e)
        {
            paginaActual2 = (Convert.ToInt32(ViewState["TotalPaginas2"]) - 1);
            cargarDatosTblPartidasAPasar();
        }

        /// <summary>
        /// Jesus Torres
        /// 03/oct/2019
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
            cargarDatosTblPartidasAPasar();
        }

        /// <summary>
        /// Jesus Torres
        /// 03/oct/2019
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
        /// Jessus Torres
        /// 03/oct/2019
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
            cargarDatosTblPartidasAgregadas();
        }
        /// <summary>
        /// Jesus Torres
        /// 03/oct/2019
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
            cargarDatosTblPartidasAgregadas();
        }
        /// <summary>
        /// Jesus Torres
        /// 03/oct/2019
        /// Efecto: filtra la tabla de partidas que estan al lado izquierdo para pasar 
        /// Requiere: dar clic al boton de "Buscar" o darle enter al campo de texto
        /// Modifica: los datos que se muestran en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFiltrarPartidasAPasar_Click(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            cargarDatosTblPartidasAPasar();

        }
        /// <summary>
        /// Jesus Torres
        /// 03/oct/2019
        /// Efecto: filtra la tabla de de acuerdo a el ddl seleccionado
        /// Requiere: seleccionar un valor del ddl
        /// Modifica: los datos que se muestran en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPeriodoModalPasaPartidas_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarTipoPartidasAgregados();
            txtBuscarDescPartidasAgregadas.Text = "";
            Periodo periodoAgregados = new Periodo();
            periodoAgregados.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasaPartidas.SelectedValue);

            List<Partida> listaPartidasAgregadas = partidaServicios.ObtenerPorPeriodo(periodoAgregados.anoPeriodo);

            Session["listaPartidasAgregadas"] = listaPartidasAgregadas;
            Session["listaPartidasAgregadasFiltrada"] = listaPartidasAgregadas;
            cargarDatosTblPartidasAgregadas();

        }

        /// <summary>
        /// Jesus Torres
        /// 03/oct/2019
        /// Efecto: filtra la tabla de partidas que estan al lado derecho a agregar
        /// Requiere: dar clic al boton de "Buscar" o darle enter al campo de texto
        /// Modifica: los datos que se muestran en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFiltrarPartidasAgregadas_Click(object sender, EventArgs e)
        {
            paginaActual3 = 0;
            cargarDatosTblPartidasAgregadas();
        }
        /// <summary>
        /// Jesus Torres
        /// 03/oct/2019
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
            cargarDatosTblPartidasAgregadas();
        }
        /// <summary>
        ///  Jesus Torres
        /// 03/oct/2019
        /// Efecto: se devuelve a la ultima pagina y muestra los datos de la misma
        /// Requiere: dar clic al boton de "Ultima pagina"
        /// Modifica: elementos mostrados en la tabla de notas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUltimo3_Click(object sender, EventArgs e)
        {
            paginaActual3 = (Convert.ToInt32(ViewState["TotalPaginas3"]) - 1);
            cargarDatosTblPartidasAgregadas();
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

        /// <summary>
        /// Jesus Torres
        /// 03/oct/2019
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
            cargarDatosTblPartidasAgregadas();
        }

        /// <summary>
        /// Jesus Torres
        /// 03/oct/2019
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
        /// Jesus Torres
        /// 04/oct/2019
        /// Efecto: pone la partida seleccionada en el perido seleccionado, en caso que la partida tenga un padre, este tambien se tiene que traladar al periodo indicado
        /// Requiere: escoger período a pasar y darle clic al boton de "seleccionar"
        /// Modifica: partidas agregadas al período
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSeleccionarPasarPartida_Click(object sender, EventArgs e)
        {
            int idPartida = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Partida> listaPartidas = (List<Partida>)Session["listaPartidasAPasarFiltrada"];
            //recorre la lista para encontrar la partida
            foreach (Partida partida in listaPartidas)
            {
                //Entra cuando existe una partida
                if (partida.idPartida == idPartida)
                {
                    int idPadre = 0;
                    int periodoAPasar = partida.periodo.anoPeriodo;
                    //Determina el periodo a la partida que ira a guardar
                    Periodo periodo = new Periodo();
                    periodo.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasaPartidas.SelectedValue);

                    Partida partidaSeleccionada = partida;
                    partidaSeleccionada.periodo = periodo;
                    //si la partida seleccionada tiene un padre, este tambien se pasará al periosdo seleccionado
                    if (partidaSeleccionada.partidaPadre != null)//la partida tiene un padre si patida.partidaPadre es diferente de null
                    {
                        //Como tiene padre, inserto primero el padre, averiguo el id con el que se guardo en BD,
                        //y se lo paso al atributo correspondinte de la partida hija
                        Partida partidaPadre = partidaSeleccionada.partidaPadre;
                        partidaPadre.periodo = periodo;
                        idPadre = partidaServicios.Insertar(partidaPadre);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "La partida seleccionada y su padre se transfirieron correctamente" + "');", true);
                        partidaSeleccionada.partidaPadre.idPartida = idPadre;
                    }
                    //si tengo un padre, se inserta primero el padre y luego la partida seleccionada
                    //si la partida seleccionada es el padre, se inserta primero, y luego inserto a mis hijas
                    idPadre = partidaServicios.Insertar(partidaSeleccionada);
                    //si la partida seleccionada es padre entra al if despues de insertarla
                    //el if busca en BD, las partidas hijas de la partida seleccionada
                    if (partidaSeleccionada.partidaPadre == null)
                    {
                        List<Partida> listaPartidasHijas = partidaServicios.obtenerPorIdPartidaPadre(partidaSeleccionada.idPartida, periodoAPasar);
                        foreach (Partida partidaHija in listaPartidasHijas)
                        {
                            partidaHija.periodo = periodo;
                            partidaHija.partidaPadre = partidaSeleccionada;
                            partidaHija.partidaPadre.idPartida = idPadre;
                            partidaServicios.Insertar(partidaHija);
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "La partida seleccionada se transfirió correctamente" + "');", true);
                    break;
                }
            }

            #region Actualiza pag Principal
            //Bloque que actualiza la tabla de la pagina principal, de manera que al pasar una partida, se actualice el contenido en la pag principal
            Periodo periodoActual = new Periodo();
            periodoActual.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

           // listaPartidas = partidaServicios.ObtenerPorPeriodo(periodoActual.anoPeriodo);

            Session["listaPartidas"] = listaPartidas;
            Session["listaPartidasFiltrada"] = listaPartidas;

            mostrarDatosTabla();
            #endregion

            #region Actualiza tabla de partidas a pasar
            Session["listaPartidasAPasar"] = listaPartidas;
            Session["listaPartidasAPasarFiltrada"] = listaPartidas;
            cargarDatosTblPartidasAPasar();
            #endregion

            #region Actualiza tabal de partidas agregadas
            Periodo periodoAgregados = new Periodo();
            periodoAgregados.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasaPartidas.SelectedValue);

            List<Partida> listaPartidasAgregadas = partidaServicios.ObtenerPorPeriodo(periodoAgregados.anoPeriodo);

            Session["listaPartidasAgregadas"] = listaPartidasAgregadas;
            Session["listaPartidasAgregadasFiltrada"] = listaPartidasAgregadas;

            cargarDatosTblPartidasAgregadas();
            #endregion
            
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
        public void llenarDdlPeriodos()
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            ddlPeriodo.Items.Clear();
            ddlPeriodoModal.Items.Clear();

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
                    ddlPeriodo.Items.Add(itemPeriodo);
                    ddlPeriodoModal.Items.Add(itemPeriodo);
                }

                if (anoHabilitado != 0)
                {
                    ddlPeriodo.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                    ddlPeriodoModal.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
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
            List<Partida> listaPartidas = (List<Partida>)Session["listaPartidas"];

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

            ddlPartidasUCR.Items.Clear();
           
            ddlPartidasUCR.Items.Add(new ListItem("UCR", "true"));
            ddlPartidasUCR.Items.Add(new ListItem("FundacionUCR", "false"));

            List<Partida> partidas = new List<Partida>();
            partidas = (List<Partida>)Session["listaPartidasPorPeriodo"];
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
        /// Efecto: carga los tipos de partidas en los DropDownList correspondiente 
        /// Requiere: -
        /// Modifica: los datos mostrados en DropDownList son los tipos de partidas
        /// Devuelve: -
        /// </summary>
        protected void llenarTipoBuscar()
        {
            ddlBuscarTipo.Items.Clear();
            ddlBuscarTipo.Items.Add(new ListItem("Tipo Partida", "null"));
            ddlBuscarTipo.Items.Add(new ListItem("UCR", "true"));
            ddlBuscarTipo.Items.Add(new ListItem("FundacionUCR", "false"));

            ddlBuscarApasar.Items.Clear();
            ddlBuscarApasar.Items.Add(new ListItem("Tipo Partida", "null"));
            ddlBuscarApasar.Items.Add(new ListItem("UCR", "true"));
            ddlBuscarApasar.Items.Add(new ListItem("FundacionUCR", "false"));
        }
        /// <summary>
        /// Jesús Torres
        /// 20/sept/2019
        /// Efecto: carga los de padres de las partidas a el DropDownList correspondiente de la tabla partidas agregadas 
        /// Requiere: -
        /// Modifica: los datos mostrados en DropDownList de padres de partidas agregadas
        /// Devuelve: -
        /// </summary>
        public void llenarTipoPartidasAgregados()
        {
            ddlBuscarAgregadas.Items.Clear();
            ddlBuscarAgregadas.Items.Add(new ListItem("Tipo Partida", "null"));
            ddlBuscarAgregadas.Items.Add(new ListItem("UCR", "true"));
            ddlBuscarAgregadas.Items.Add(new ListItem("FundacionUCR", "false"));

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

        /// <summary>
        /// Jesús Torres
        /// 24/sep/2019
        /// Efecto: Metodo para llenar la tabla de partidas en modal de pasarPartidas
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        private void cargarDatosTblPartidasAPasar()
        {

            List<Partida> listaSession = (List<Partida>)Session["listaPartidasAPasar"];
            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarDescPartidasAPasar.Text))
                desc = txtBuscarDescPartidasAPasar.Text;

            List<Partida> listaPartidas = (List<Partida>)listaSession.Where(partida => partida.descripcionPartida.ToUpper().Contains(desc.ToUpper())).ToList();
            Session["listaPartidasAPasarFiltrada"] = listaPartidas;

            //lista solicitudes
            var dt2 = listaPartidas;
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

            //rpPartidasAPasar.DataSource = listaPartidas;
            rpPartidasAPasar.DataSource = pgsource;
            rpPartidasAPasar.DataBind();

            //metodo que realiza la paginacion
            Paginacion2();

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarPartidas", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarPartidas').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarPartida();", true);
        }
        /// <summary>
        /// Jesús Torres
        /// 03/oct/2019
        /// Efecto: Metodo para llenar la tabla de partidas en modal de pasarPartidasAgregadas
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        public void cargarDatosTblPartidasAgregadas()
        {
            List<Partida> listaSession = (List<Partida>)Session["listaPartidasAgregadas"];

            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarDescPartidasAgregadas.Text))
                desc = txtBuscarDescPartidasAgregadas.Text;

            List<Partida> listaPartida = (List<Partida>)listaSession.Where(partida => partida.descripcionPartida.ToUpper().Contains(desc.ToUpper())).ToList();
            Session["listaPartidasAgregadasFiltrada"] = listaPartida;

            //lista solicitudes
            var dt3 = listaPartida;
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

            rpPartidasAgregadas.DataSource = pgsource;
            rpPartidasAgregadas.DataBind();

            //metodo que realiza la paginacion
            Paginacion3();

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarPartidas", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarPartidas').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarPartida();", true);
        }

        /// <summary>
        /// Jesús Torres
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

        /// <summary>
        /// Jesus Torres
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

        #endregion

        #region paginacion Actual
        //Bloque que contiene get y set de pagina actual
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
      
        #endregion

       

    }
}