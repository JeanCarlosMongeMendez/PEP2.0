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

namespace Proyecto.Mantenimiento.EscalasSalariales
{
    public partial class AdministrarEscalas : System.Web.UI.Page
    {
        #region variables globales
        PeriodoServicios periodoServicios = new PeriodoServicios();
        EscalaSalarialServicios escalaSalarialServicios = new EscalaSalarialServicios();
        static EscalaSalarial escalaSalarialSeleccionada = new EscalaSalarial();

        readonly PagedDataSource pgsource = new PagedDataSource();
        int primerIndex, ultimoIndex, primerIndex2, ultimoIndex2, primerIndex3, ultimoIndex3;
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

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);
            if (!IsPostBack)
            {
                Session["listaEscalas"] = null;
                Session["listaEscalasFiltrada"] = null;

                Session["listaEscalasAPasar"] = null;
                Session["listaEscalasAPasarFiltrada"] = null;

                Session["listaEscalasAgregadas"] = null;
                Session["listaEscalasAgregadasFiltrada"] = null;

                llenarDdlPeriodos();

                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

                List<EscalaSalarial> listaEscalas = escalaSalarialServicios.getEscalasSalarialesPorPeriodo(periodo);

                Session["listaEscalas"] = listaEscalas;
                Session["listaEscalasFiltrada"] = listaEscalas;

                mostrarDatosTabla();
            }
          
        }
        #endregion

        #region logica

        /// <summary>
        /// Leonardo Carrion
        /// 25/jun/2019
        /// Efecto: valida los campos ingresados en nueva escala y marca los que estan mal
        /// Requiere: -
        /// Modifica: los txt que esten mal
        /// Devuelve: true si esta bien o false de lo contrario
        ///        
        /// Modificado por Jesús Torres
        /// 13/09/2019
        /// Se modifica validaciones de descripcion, salariobaseI, Salario base II, tope de Escalofones, y porcentaje, 
        /// con el fin de no poder ingresar valores nulos, o en caso correspondientes valores inferiores a 1
        /// </summary>
        /// <returns></returns>
        public Boolean validarNuevaEscala()
        {
            Boolean valido = true;

            txtDesc.CssClass = "form-control";
            txtSalarioBase1.CssClass = "form-control";
            txtSalarioBase2.CssClass = "form-control";
            txtTopeEscalafones.CssClass = "form-control";
            txtPorcentajeEscalafones.CssClass = "form-control";

            #region descripcion
            if (String.IsNullOrEmpty(txtDesc.Text) || txtDesc.Text.Trim() == String.Empty || txtDesc.Text.Length > 255)
            {
                txtDesc.CssClass = "form-control alert-danger";
                valido = false;
            }
            #endregion

            #region salario base 1
            if (String.IsNullOrEmpty(txtSalarioBase1.Text))
            {
                txtSalarioBase1.CssClass = "form-control alert-danger";
                valido = false;
            }
            else
            {
                try
                {
                    //String salarioTxt = ;
                    Double salario = Convert.ToDouble(txtSalarioBase1.Text.Replace(".", ","));
                    if (salario < 0)
                    {
                        txtSalarioBase1.CssClass = "form-control alert-danger";
                        valido = false;
                    }

                }
                catch
                {
                    txtSalarioBase1.CssClass = "form-control alert-danger";
                    valido = false;
                }
            }
            #endregion

            #region salario base 2
            if (String.IsNullOrEmpty(txtSalarioBase2.Text))
            {
                txtSalarioBase2.CssClass = "form-control alert-danger";
                valido = false;
            }
            else
            {
                try
                {
                    String salarioTxt = txtSalarioBase2.Text.Replace(".", ",");
                    Double salario = Convert.ToDouble(salarioTxt);
                    if (salario < 0 )
                    {
                        txtSalarioBase2.CssClass = "form-control alert-danger";
                        valido = false;
                    }
                }
                catch
                {
                    txtSalarioBase2.CssClass = "form-control alert-danger";
                    valido = false;
                }
            }
            #endregion

            #region tope escalafones
            if (String.IsNullOrEmpty(txtTopeEscalafones.Text))
            {
                txtTopeEscalafones.CssClass = "form-control alert-danger";
                valido = false;
            }
            else
            {
                try
                {
                    int topeEscalafones = Convert.ToInt32(txtTopeEscalafones.Text);
                    if (topeEscalafones < 0 )
                    {
                        txtTopeEscalafones.CssClass = "form-control alert-danger";
                        valido = false;
                    }
                }
                catch
                {
                    txtTopeEscalafones.CssClass = "form-control alert-danger";
                    valido = false;
                }
            }
            #endregion

            #region porcentaje escalafones
            if (String.IsNullOrEmpty(txtPorcentajeEscalafones.Text))
            {
                txtPorcentajeEscalafones.CssClass = "form-control alert-danger";
                valido = false;
            }
            else
            {
                try
                {
                    String porcentajeEscalafonesTxt = txtPorcentajeEscalafones.Text.Replace(".", ",");
                    Double porcentajeEscalafones = Convert.ToDouble(porcentajeEscalafonesTxt);
                    if (porcentajeEscalafones < 0 )
                    {
                        txtPorcentajeEscalafones.CssClass = "form-control alert-danger";
                        valido = false;
                    }
                }
                catch
                {
                    txtPorcentajeEscalafones.CssClass = "form-control alert-danger";
                    valido = false;
                }
            }
            #endregion

            return valido;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 25/jun/2019
        /// Efecto: valida los campos ingresados en editar escala y marca los que estan mal
        /// Requiere: -
        /// Modifica: los txt que esten mal
        /// Devuelve: true si esta bien o false de lo contrario
        /// 
        /// Modificado por Jesús Torres 
        /// 13/09/2019
        /// Valida los datos del modal de editar  con el fin de evitar ingresar valores no pertinentes como vacios, negativos o fuera de rango etc
        /// </summary>
        /// <returns></returns>
        public Boolean validarEditarEscala()
        {
            Boolean valido = true;

            txtDescEditar.CssClass = "form-control";
            txtSalarioBase1Editar.CssClass = "form-control";
            txtSalarioBase2Editar.CssClass = "form-control";
            txtTopeEscalafonesEditar.CssClass = "form-control";
            txtPorcentajeEscalafonesEditar.CssClass = "form-control";

            #region descripcion
            if (String.IsNullOrEmpty(txtDescEditar.Text) || txtDescEditar.Text.Trim() == String.Empty || txtDescEditar.Text.Length > 255)
            {
                txtDescEditar.CssClass = "form-control alert-danger";
                valido = false;
            }
            #endregion

            #region salario base 1
            if (String.IsNullOrEmpty(txtSalarioBase1Editar.Text))
            {
                txtSalarioBase1Editar.CssClass = "form-control alert-danger";
                valido = false;
            }
            else
            {
                try
                {
                    String salarioTxt = txtSalarioBase1Editar.Text.Replace(".", ",");
                    Double salario = Convert.ToDouble(salarioTxt);
                    if (salario < 1 )
                    {
                        txtSalarioBase1Editar.CssClass = "form-control alert-danger";
                        valido = false;
                    }
                }
                catch
                {
                    txtSalarioBase1Editar.CssClass = "form-control alert-danger";
                    valido = false;
                }
            }
            #endregion

            #region salario base 2
            if (String.IsNullOrEmpty(txtSalarioBase2Editar.Text))
            {
                txtSalarioBase2Editar.CssClass = "form-control alert-danger";
                valido = false;
            }
            else
            {
                try
                {
                    String salarioTxt = txtSalarioBase2Editar.Text.Replace(".", ",");
                    Double salario = Convert.ToDouble(salarioTxt);
                    if (salario < 1 )
                    {
                        txtSalarioBase2Editar.CssClass = "form-control alert-danger";
                        valido = false;
                    }
                }
                catch
                {
                    txtSalarioBase2Editar.CssClass = "form-control alert-danger";
                    valido = false;
                }
            }
            #endregion

            #region tope escalafones
            if (String.IsNullOrEmpty(txtTopeEscalafonesEditar.Text))
            {
                txtTopeEscalafonesEditar.CssClass = "form-control alert-danger";
                valido = false;
            }
            else
            {
                try
                {
                    int topeEscalafones = Convert.ToInt32(txtTopeEscalafonesEditar.Text);
                    if (topeEscalafones < 1)
                    {
                        txtTopeEscalafonesEditar.CssClass = "form-control alert-danger";
                        valido = false;
                    }
                }
                catch
                {
                    txtTopeEscalafonesEditar.CssClass = "form-control alert-danger";
                    valido = false;
                }
            }
            #endregion

            #region porcentaje escalafones
            if (String.IsNullOrEmpty(txtPorcentajeEscalafonesEditar.Text))
            {
                txtPorcentajeEscalafonesEditar.CssClass = "form-control alert-danger";
                valido = false;
            }
            else
            {
                try
                {
                    String porcentajeEscalafonesTxt = txtPorcentajeEscalafonesEditar.Text.Replace(".", ",");
                    Double porcentajeEscalafones = Convert.ToDouble(porcentajeEscalafonesTxt);
                    if (porcentajeEscalafones < 1)
                    {
                        txtPorcentajeEscalafonesEditar.CssClass = "form-control alert-danger";
                        valido = false;
                    }
                }
                catch
                {
                    txtPorcentajeEscalafonesEditar.CssClass = "form-control alert-danger";
                    valido = false;
                }
            }
            #endregion

            return valido;
        }

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
            ddlPeriodo.Items.Clear();
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
                }

                if (anoHabilitado != 0)
                {
                    ddlPeriodo.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                }
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/jun/2019
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTabla()
        {
            List<EscalaSalarial> listaEscalas = (List<EscalaSalarial>)Session["listaEscalas"];

            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarDesc.Text))
            {
                desc = txtBuscarDesc.Text;
            }

            List<EscalaSalarial> listaEscalasFiltrada = (List<EscalaSalarial>)listaEscalas.Where(escala => escala.descEscalaSalarial.ToUpper().Contains(desc.ToUpper())).ToList();

            Session["listaEscalasFiltrada"] = listaEscalasFiltrada;

            var dt = listaEscalasFiltrada;
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

            rpEscalas.DataSource = pgsource;
            rpEscalas.DataBind();

            //metodo que realiza la paginacion
            Paginacion();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
        /// Efecto: Metodo para llenar la tabla con los datos de las escalas salariales que se encuentran en la base de datos en el modal de pasar escalas en la tabla de periodo seleccionado
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        private void cargarDatosTblEscalasAPasar()
        {
            List<EscalaSalarial> listaSession = (List<EscalaSalarial>)Session["listaEscalasAPasar"];

            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarDescEscalasAPasar.Text))
                desc = txtBuscarDescEscalasAPasar.Text;

            List<EscalaSalarial> listaEscalas = (List<EscalaSalarial>)listaSession.Where(escala => escala.descEscalaSalarial.ToUpper().Contains(desc.ToUpper())).ToList();
            Session["listaEscalasAPasarFiltrada"] = listaEscalas;

            //lista solicitudes
            var dt2 = listaEscalas;
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

            rpEscalasAPasar.DataSource = listaEscalas;
            rpEscalasAPasar.DataBind();

            //metodo que realiza la paginacion
            Paginacion2();
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarEscala", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarEscala').hide();", true);


            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarEscala();", true);

        }

        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
        /// Efecto: Metodo para llenar la tabla con los datos de las escalas salariales que se encuentran en la base de datos en el modal de pasar escalas en la tabla de periodo seleccionado
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        private void cargarDatosTblEscalasAgregadas()
        {
            List<EscalaSalarial> listaSession = (List<EscalaSalarial>)Session["listaEscalasAgregadas"];

            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarDescEscalasAgregadas.Text))
                desc = txtBuscarDescEscalasAgregadas.Text;

            List<EscalaSalarial> listaEscalas = (List<EscalaSalarial>)listaSession.Where(escala => escala.descEscalaSalarial.ToUpper().Contains(desc.ToUpper())).ToList();
            Session["listaEscalasAgregadasFiltrada"] = listaEscalas;

            //lista solicitudes
            var dt3 = listaEscalas;
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

            rpEscalasAgregadas.DataSource = listaEscalas;
            rpEscalasAgregadas.DataBind();

            //metodo que realiza la paginacion
            Paginacion3();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarEscala();", true);
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarEscala", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarEscala').hide();", true);
           
        }

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
        /// 29/abr/2019
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
        /// 30/abr/2019
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

        #region eventos

        /// <summary>
        /// Leonardo Carrion
        /// 10/jun/2019
        /// Efecto: cambia los datos de la tabla segun el periodo seleccionado
        /// Requiere: cambiar periodo
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            List<EscalaSalarial> listaEscalas = escalaSalarialServicios.getEscalasSalarialesPorPeriodo(periodo);

            Session["listaEscalas"] = listaEscalas;
            Session["listaEscalasFiltrada"] = listaEscalas;

            mostrarDatosTabla();
        }

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
        protected void lbPrimero_Click(object sender, EventArgs e)
        {
            paginaActual = 0;
            mostrarDatosTabla();
        }

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
        protected void lbUltimo_Click(object sender, EventArgs e)
        {
            paginaActual = (Convert.ToInt32(ViewState["TotalPaginas"]) - 1);
            mostrarDatosTabla();
        }

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
        protected void lbAnterior_Click(object sender, EventArgs e)
        {
            paginaActual -= 1;
            mostrarDatosTabla();
        }

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
        protected void lbSiguiente_Click(object sender, EventArgs e)
        {
            paginaActual += 1;
            mostrarDatosTabla();
        }

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
        protected void rptPaginacion_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual = Convert.ToInt32(e.CommandArgument.ToString());
            mostrarDatosTabla();
        }

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
        protected void lbPrimero2_Click(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            cargarDatosTblEscalasAPasar();
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
        protected void lbUltimo2_Click(object sender, EventArgs e)
        {
            paginaActual2 = (Convert.ToInt32(ViewState["TotalPaginas2"]) - 1);
            cargarDatosTblEscalasAPasar();
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
        protected void lbAnterior2_Click(object sender, EventArgs e)
        {
            paginaActual2 -= 1;
            cargarDatosTblEscalasAPasar();
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
        protected void lbSiguiente2_Click(object sender, EventArgs e)
        {
            paginaActual2 += 1;
            cargarDatosTblEscalasAPasar();
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
        protected void rptPaginacion2_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual2 = Convert.ToInt32(e.CommandArgument.ToString());
            cargarDatosTblEscalasAPasar();
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
        /// 16/jul/2019
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
            cargarDatosTblEscalasAgregadas();
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
        protected void lbUltimo3_Click(object sender, EventArgs e)
        {
            paginaActual3 = (Convert.ToInt32(ViewState["TotalPaginas3"]) - 1);
            cargarDatosTblEscalasAgregadas();
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
        protected void lbAnterior3_Click(object sender, EventArgs e)
        {
            paginaActual3 -= 1;
            cargarDatosTblEscalasAgregadas();
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
        protected void lbSiguiente3_Click(object sender, EventArgs e)
        {
            paginaActual3 += 1;
            cargarDatosTblEscalasAgregadas();
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
        protected void rptPaginacion3_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual3 = Convert.ToInt32(e.CommandArgument.ToString());
            cargarDatosTblEscalasAgregadas();
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
        /// 13/jun/2019
        /// Efecto: guarda en la base de datos la nueva escala salarial, y recarga la tabla de escalas
        /// Requiere: dar clic en el boton de "Guardar"
        /// Modifica: tabla de base de datos y pantalla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaEscalaModal_Click(object sender, EventArgs e)
        {
            if (validarNuevaEscala())
            {
                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

                EscalaSalarial escalaSalarial = new EscalaSalarial();
                escalaSalarial.descEscalaSalarial = txtDesc.Text;
                escalaSalarial.salarioBase1 = Convert.ToDouble(txtSalarioBase1.Text.Replace(".", ","));
                escalaSalarial.salarioBase2 = Convert.ToDouble(txtSalarioBase2.Text.Replace(".", ","));
                escalaSalarial.topeEscalafones = Convert.ToInt32(txtTopeEscalafones.Text);
                escalaSalarial.porentajeEscalafones = Convert.ToDouble(txtPorcentajeEscalafones.Text.Replace(".", ","));
                escalaSalarial.periodo = periodo;

                escalaSalarialServicios.insertarEscalaSalarial(escalaSalarial);

                txtDesc.Text = "";
                txtSalarioBase1.Text = "";
                txtSalarioBase2.Text = "";
                txtTopeEscalafones.Text = "";
                txtPorcentajeEscalafones.Text = "";

                List<EscalaSalarial> listaEscalas = escalaSalarialServicios.getEscalasSalarialesPorPeriodo(periodo);

                Session["listaEscalas"] = listaEscalas;
                Session["listaEscalasFiltrada"] = listaEscalas;

                mostrarDatosTabla();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaEscala", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaEscala').hide();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevaEscala", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevaEscala').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaEscala();", true);
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 13/jun/2019
        /// Efecto: levanta el modal para ingresar una nueva escala salarial y pone el año escogido en el modal
        /// Requiere: dar clic al boton de "Nuevo"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaEscalaSalarial_Click(object sender, EventArgs e)
        {
            txtDesc.CssClass = "form-control";
            txtSalarioBase1.CssClass = "form-control";
            txtSalarioBase2.CssClass = "form-control";
            txtTopeEscalafones.CssClass = "form-control";
            txtPorcentajeEscalafones.CssClass = "form-control";

            txtDesc.Text = "";
            txtSalarioBase1.Text = "";
            txtSalarioBase2.Text = "";
            txtTopeEscalafones.Text = "";
            txtPorcentajeEscalafones.Text = "";

            lblPeriodoModal.Text = ddlPeriodo.SelectedValue;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevaEscala();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/jun/2019
        /// Efecto: actualiza la escala salarial seleccionada
        /// Requiere:  dar clic en "Actualizar"
        /// Modifica: la escala salarial seleccionada
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizarEscalaModal_Click(object sender, EventArgs e)
        {
            if (validarEditarEscala())
            {
                EscalaSalarial escalaSalarial = escalaSalarialSeleccionada;
                escalaSalarial.descEscalaSalarial = txtDescEditar.Text;
                escalaSalarial.salarioBase1 = Convert.ToDouble(txtSalarioBase1Editar.Text.Replace(".", ","));
                escalaSalarial.salarioBase2 = Convert.ToDouble(txtSalarioBase2Editar.Text.Replace(".", ","));
                escalaSalarial.topeEscalafones = Convert.ToInt32(txtTopeEscalafonesEditar.Text);
                escalaSalarial.porentajeEscalafones = Convert.ToDouble(txtPorcentajeEscalafonesEditar.Text.Replace(".", ","));

                escalaSalarialServicios.actualizarEscalaSalarial(escalaSalarial);

                mostrarDatosTabla();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarEscala", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarEscala').hide();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarEscala", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarEscala').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalEditarEscala();", true);
            }

        }

        /// <summary>
        /// Leonardo Carrion
        /// 24/jun/2019
        /// Efecto: levanta el modal con los datos de la escala salarial seleccionada para poder editarla
        /// Requiere: dar clic al boton de "Editar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int idEscala = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<EscalaSalarial> listaEscalas = (List<EscalaSalarial>)Session["listaEscalasFiltrada"];

            foreach (EscalaSalarial escala in listaEscalas)
            {
                if (escala.idEscalaSalarial == idEscala)
                {
                    escalaSalarialSeleccionada = escala;
                    break;
                }
            }

            txtDescEditar.CssClass = "form-control";
            txtSalarioBase1Editar.CssClass = "form-control";
            txtSalarioBase2Editar.CssClass = "form-control";
            txtTopeEscalafonesEditar.CssClass = "form-control";
            txtPorcentajeEscalafonesEditar.CssClass = "form-control";

            lblPeriodoModalEditar.Text = escalaSalarialSeleccionada.periodo.anoPeriodo.ToString();
            txtDescEditar.Text = escalaSalarialSeleccionada.descEscalaSalarial;
            txtSalarioBase1Editar.Text = escalaSalarialSeleccionada.salarioBase1.ToString();
            txtSalarioBase2Editar.Text = escalaSalarialSeleccionada.salarioBase2.ToString();
            txtTopeEscalafonesEditar.Text = escalaSalarialSeleccionada.topeEscalafones.ToString();
            txtPorcentajeEscalafonesEditar.Text = escalaSalarialSeleccionada.porentajeEscalafones.ToString();
            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalEditarEscala();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 25/jun/2019
        /// Efecto: levanta el modal para eliminar la escala salarial, muestra los datos de la escala salarial seleccionada
        /// Requiere: dar clic en el boton de "Eliminar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int idEscala = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<EscalaSalarial> listaEscalas = (List<EscalaSalarial>)Session["listaEscalasFiltrada"];

            foreach (EscalaSalarial escala in listaEscalas)
            {
                if (escala.idEscalaSalarial == idEscala)
                {
                    escalaSalarialSeleccionada = escala;
                    break;
                }
            }

            lblPeriodoModalEliminar.Text = escalaSalarialSeleccionada.periodo.anoPeriodo.ToString();
            txtDescEliminar.Text = escalaSalarialSeleccionada.descEscalaSalarial;
            txtSalarioBase1Eliminar.Text = escalaSalarialSeleccionada.salarioBase1.ToString();
            txtSalarioBase2Eliminar.Text = escalaSalarialSeleccionada.salarioBase2.ToString();
            txtTopeEscalafonesEliminar.Text = escalaSalarialSeleccionada.topeEscalafones.ToString();
            txtPorcentajeEscalafonesEliminar.Text = escalaSalarialSeleccionada.porentajeEscalafones.ToString();
            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalEliminarEscala();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/jun/2019
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
        /// 15/jul/2019
        /// Efecto: elimina la escala salarial seleccionada 
        /// Requiere: dar clic en el boton de eliminar
        /// Modifica: las escalas que se encuentran en la base de datos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarEscalaModal_Click(object sender, EventArgs e)
        {
            escalaSalarialServicios.eliminarEscalaSalarial(escalaSalarialSeleccionada);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalEditarEscala", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalEditarEscala').hide();", true);

            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            List<EscalaSalarial> listaEscalas = escalaSalarialServicios.getEscalasSalarialesPorPeriodo(periodo);

            Session["listaEscalas"] = listaEscalas;
            Session["listaEscalasFiltrada"] = listaEscalas;

            mostrarDatosTabla();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 15/jul/2019
        /// Efecto: levanta el modal para pasar las escalas salariales entre los períodos
        /// Requiere: dar clic en el boton de "Pasar escalas salariales"
        /// Modifca: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPasarEscalas_Click(object sender, EventArgs e)
        {
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            lblPeriodoSeleccionado.Text = periodo.anoPeriodo.ToString();

            // cargar periodos en dropdownlist
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            ddlPeriodoModalPasarEscalas.Items.Clear();
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
                        ddlPeriodoModalPasarEscalas.Items.Add(itemPeriodo);
                    }
                }

            }
            //fin de dopdownlist

            List<EscalaSalarial> listaEscalas = escalaSalarialServicios.getEscalasSalarialesPorPeriodo(periodo);

            Session["listaEscalasAPasar"] = listaEscalas;
            Session["listaEscalasAPasarFiltrada"] = listaEscalas;

            cargarDatosTblEscalasAPasar();

            Periodo periodoAgregados = new Periodo();
            periodoAgregados.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasarEscalas.SelectedValue);

            List<EscalaSalarial> listaEscalasAgregadas = escalaSalarialServicios.getEscalasSalarialesPorPeriodo(periodoAgregados);

            Session["listaEscalasAgregadas"] = listaEscalasAgregadas;
            Session["listaEscalasAgregadasFiltrada"] = listaEscalasAgregadas;

            cargarDatosTblEscalasAgregadas();
           
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarEscala();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 15/jul/2019
        /// Efecto: carga las escalas salariales que contiene el periodo seleccionado
        /// Requiere: cambiar el periodo
        /// Modifica: la lista de escalas salariales que se muestran en la tabla de escalas asosciadas al periodo seleccionado 
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPeriodoModalPasarEscalas_SelectedIndexChanged(object sender, EventArgs e)
        {

            Periodo periodoAgregados = new Periodo();
            periodoAgregados.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasarEscalas.SelectedValue);

            List<EscalaSalarial> listaEscalasAgregadas = escalaSalarialServicios.getEscalasSalarialesPorPeriodo(periodoAgregados);

            Session["listaEscalasAgregadas"] = listaEscalasAgregadas;
            Session["listaEscalasAgregadasFiltrada"] = listaEscalasAgregadas;

            cargarDatosTblEscalasAgregadas();

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarEscala", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarEscala').hide();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarEscala();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
        /// Efecto: filtra la tabla de escalasa salariales que estan al lado izquierdo para pasar 
        /// Requiere: dar clic al boton de "Buscar" o darle enter al campo de texto
        /// Modifica: los datos que se muestran en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFiltrarEscalasAPasar_Click(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            cargarDatosTblEscalasAPasar();
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarEscala", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarEscala').hide();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarEscala();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
        /// Efecto: filtra la tabla de escalasa salariales que estan al lado derecho para pasar 
        /// Requiere: dar clic al boton de "Buscar" o darle enter al campo de texto
        /// Modifica: los datos que se muestran en la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFiltrarEscalasAgregadas_Click(object sender, EventArgs e)
        {
            paginaActual3 = 0;

            Periodo periodoAgregados = new Periodo();
            periodoAgregados.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasarEscalas.SelectedValue);

            List<EscalaSalarial> listaEscalasAgregadas = escalaSalarialServicios.getEscalasSalarialesPorPeriodo(periodoAgregados);

            Session["listaEscalasAgregadas"] = listaEscalasAgregadas;
            Session["listaEscalasAgregadasFiltrada"] = listaEscalasAgregadas;

            cargarDatosTblEscalasAgregadas();
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarEscala", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarEscala').hide();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarEscala();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 16/jul/2019
        /// Efecto: pone la escala salarial seleccionada en el perido seleccionado
        /// Requiere: escoger período a pasar y darle clic al boton de "seleccionar"
        /// Modifica: escalas agregadas al período
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSeleccionarEscala_Click(object sender, EventArgs e)
        {
            int idEscala = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<EscalaSalarial> listaEscalas = (List<EscalaSalarial>)Session["listaEscalasAPasarFiltrada"];

            foreach (EscalaSalarial escala in listaEscalas)
            {
                if (escala.idEscalaSalarial == idEscala)
                {
                    Periodo periodo = new Periodo();
                    periodo.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasarEscalas.SelectedValue);

                    EscalaSalarial escalaSalarial = escala;
                    escalaSalarial.periodo = periodo;

                    escalaSalarialServicios.insertarEscalaSalarial(escalaSalarial);
                    break;
                }
            }

            Periodo periodoE = new Periodo();
            periodoE.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            listaEscalas = escalaSalarialServicios.getEscalasSalarialesPorPeriodo(periodoE);

            Session["listaEscalas"] = listaEscalas;
            Session["listaEscalasFiltrada"] = listaEscalas;

            mostrarDatosTabla();

            listaEscalas = escalaSalarialServicios.getEscalasSalarialesPorPeriodo(periodoE);

            Session["listaEscalasAPasar"] = listaEscalas;
            Session["listaEscalasAPasarFiltrada"] = listaEscalas;

            cargarDatosTblEscalasAPasar();

            Periodo periodoAgregados = new Periodo();
            periodoAgregados.anoPeriodo = Convert.ToInt32(ddlPeriodoModalPasarEscalas.SelectedValue);

            List<EscalaSalarial> listaEscalasAgregadas = escalaSalarialServicios.getEscalasSalarialesPorPeriodo(periodoAgregados);

            Session["listaEscalasAgregadas"] = listaEscalasAgregadas;
            Session["listaEscalasAgregadasFiltrada"] = listaEscalasAgregadas;

            cargarDatosTblEscalasAgregadas();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarEscala", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarEscala').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarEscala();", true);

        }
        #endregion
    }
}