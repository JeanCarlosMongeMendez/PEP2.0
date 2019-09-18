using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using PEP;
using Servicios;

namespace Proyecto.Planilla
{
    public partial class AdministrarFuncionarioFundevi : System.Web.UI.Page
    {
        FuncionariosFundeviServicios funcionariosFundeviServicios = new FuncionariosFundeviServicios();
        PeriodoServicios periodoServicios = new PeriodoServicios();
        PlanillaFundeviServicios planillaFundeviServicios = new PlanillaFundeviServicios();

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
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);

            PlanillaFundevi planillaFundevi = (PlanillaFundevi)Session["planillaSeleccionada"];

            if (!IsPostBack)
            {
                Session["funcionarios"] = null;
                Session["funcionariosFiltrados"] = null;
                List<FuncionarioFundevi> funcionarios = funcionariosFundeviServicios.GetFuncionariosPorPlanilla(planillaFundevi);
                Session["funcionariosAPasar"] = null;
                Session["funcionariosAPasarFiltrados"] = null;
                Session["funcionariosAgregados"] = null;
                Session["funcionariosAgregadosFiltrados"] = null;

                Session["funcionarios"] = funcionarios;
                Session["funcionariosFiltrados"] = funcionarios;
                PlanillaFundevi plan = (PlanillaFundevi)Session["planillaSeleccionada"];
                label.Text = "Funcionarios de la planilla: " + plan.anoPeriodo;
                lblPeriodoSeleccionado.Text = plan.anoPeriodo.ToString();
                mostrarDatosTabla();

                llenarDdlPeriodos();

                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);
            }
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
            List<Periodo> periodos = new List<Periodo>();
            ddlPeriodo.Items.Clear();
            periodos = this.periodoServicios.getPeriodosPlanillasFundevi();
            
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
        /// Juan Solano Brenes
        /// 20/06/2019
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
        /// Juan Solano Brenes
        /// 20/06/2019
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTabla()
        {
            List<FuncionarioFundevi> funcionarios = (List<FuncionarioFundevi>)Session["funcionarios"];

            String nombre = txtBuscarNombre.Text;
            List<FuncionarioFundevi> listaPlanillasFiltrada = (List<FuncionarioFundevi>)funcionarios.Where(funcionario => funcionario.nombre.ToUpper().Contains(nombre.ToUpper())).ToList();

            Session["funcionariosFiltrados"] = listaPlanillasFiltrada;

            var dt = listaPlanillasFiltrada;
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

            rpPlanillas.DataSource = pgsource;
            rpPlanillas.DataBind();

            //metodo que realiza la paginacion
            Paginacion();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/sep/2019
        /// Efecto: Metodo para llenar la tabla con los datos de los funcionarios que se encuentran en la base de datos en el modal de pasar funcionarios en la tabla de periodo seleccionado
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        private void cargarDatosTblFuncionariosAPasar()
        {
            List<FuncionarioFundevi> listaSession = (List<FuncionarioFundevi>)Session["funcionariosAPasar"];

            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarNombreFuncionarioAPasar.Text))
                desc = txtBuscarNombreFuncionarioAPasar.Text;

            List<FuncionarioFundevi> listaFuncionarios = (List<FuncionarioFundevi>)listaSession.Where(funcionario => funcionario.nombre.ToUpper().Contains(desc.ToUpper())).ToList();
            Session["funcionariosAPasarFiltrados"] = listaFuncionarios;

            //lista solicitudes
            var dt2 = listaFuncionarios;
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

            rpFuncionariosAPasar.DataSource = listaFuncionarios;
            rpFuncionariosAPasar.DataBind();

            //metodo que realiza la paginacion
            Paginacion2();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarFuncionario", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarFuncionario').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarFuncionario();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/sep/2019
        /// Efecto: Metodo para llenar la tabla con los datos de las escalas salariales que se encuentran en la base de datos en el modal de pasar escalas en la tabla de periodo seleccionado
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        private void cargarDatosTblFuncionariosAgregados()
        {
            List<FuncionarioFundevi> listaSession = (List<FuncionarioFundevi>)Session["funcionariosAgregados"];

            String desc = "";

            if (!String.IsNullOrEmpty(txtBuscarNombreFuncionarioAgregadas.Text))
                desc = txtBuscarNombreFuncionarioAgregadas.Text;

            List<FuncionarioFundevi> listafuncionarios = (List<FuncionarioFundevi>)listaSession.Where(funcionario => funcionario.nombre.ToUpper().Contains(desc.ToUpper())).ToList();
            Session["funcionariosAgregadosFiltrados"] = listafuncionarios;

            //lista solicitudes
            var dt3 = listafuncionarios;
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

            rpFuncionariosAgregados.DataSource = listafuncionarios;
            rpFuncionariosAgregados.DataBind();

            //metodo que realiza la paginacion
            Paginacion3();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarFuncionario", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarFuncionario').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarFuncionario();", true);
        }

        /// <summary>
        /// Juan Solano Brenes
        /// 20/06/2019
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
        /// 11/sep/2019
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
        /// 11/sep/2019
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
        /// Juan Solano Brenes
        /// 20/06/2019
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
        /// Juan Solano Brenes
        /// 20/06/2019
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
        /// Juan Solano Brenes
        /// 20/06/2019
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
        /// Juan Solano Brenes
        /// 20/06/2019
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
        /// Juan Solano Brenes
        /// 20/06/2019
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
        /// 11/sep/2019
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
            cargarDatosTblFuncionariosAPasar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/sep/2019
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
            cargarDatosTblFuncionariosAPasar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/sep/2019
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
            cargarDatosTblFuncionariosAPasar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/sep/2019
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
            cargarDatosTblFuncionariosAPasar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/sep/2019
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
            cargarDatosTblFuncionariosAPasar();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/sep/2019
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
        /// 11/sep/2019
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
            cargarDatosTblFuncionariosAgregados();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/sep/2019
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
            cargarDatosTblFuncionariosAgregados();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/sep/2019
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
            cargarDatosTblFuncionariosAgregados();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/sep/2019
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
            cargarDatosTblFuncionariosAgregados();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/sep/2019
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
            cargarDatosTblFuncionariosAgregados();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 11/sep/2019
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

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Planilla/AsignarFuncionariosPlanillaFundevi.aspx");
            Response.Redirect(url);
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            Page.FindControl(id.ToString()).EnableViewState = true;
        }

        protected void btnAjuste_Click(object sender, EventArgs e)
        {
            int idFuncionario = Convert.ToInt32((((Button)(sender)).CommandArgument).ToString());
            Session["funcionarioAjuste"] = null;
            FuncionarioFundevi funcionarioFundevi = funcionariosFundeviServicios.GetFuncionario(idFuncionario);
            lblNombre.Text = funcionarioFundevi.nombre;
            lblSalario.Text = "" + funcionarioFundevi.salario;
            Session["funcionarioAjuste"] = funcionarioFundevi;
            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModal();", true);
        }

        protected void btnGuardarAjuste_Click(object sender, EventArgs e)
        {
            FuncionarioFundevi funcionarioFundevi = (FuncionarioFundevi)Session["funcionarioAjuste"];

            Double salario = 0;
            String txtSalario = txtAsalario.Text.Replace(".", ",");
            if (Double.TryParse(txtSalario, out salario))
            {
                txtAsalario.Text = salario.ToString();
            }

            if (funcionariosFundeviServicios.actualizarSalario(funcionarioFundevi, salario))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "El salario se actualizó correctamente" + "');", true);
                String url = Page.ResolveUrl("~/Planilla/AdministrarFuncionarioFundevi.aspx");
                Response.Redirect(url);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "No se pudo actualizar el salario" + "');", true);
            }
        }

        protected void btnAsignar_Click1(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Planilla/AsignarFuncionariosPlanillaFundevi.aspx");
            Response.Redirect(url);
        }

        protected void txtBuscarNombre_TextChanged(object sender, EventArgs e)
        {
            paginaActual = 0;
            mostrarDatosTabla();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int idFuncionario = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            Session["funcionarioEliminar"] = null;
            FuncionarioFundevi funcionarioFundevi = funcionariosFundeviServicios.GetFuncionario(idFuncionario);
            txtNomF.Text = funcionarioFundevi.nombre;
            txtSa.Text = "" + funcionarioFundevi.salario;
            Session["funcionarioEliminar"] = funcionarioFundevi;
            PlanillaFundevi plan = (PlanillaFundevi)Session["planillaSeleccionada"];
            txtEli.Text = "¿Desea eliminar al funcionario de la planilla " + plan.anoPeriodo + "?";
            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalEliminar();", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            FuncionarioFundevi funcionario = new FuncionarioFundevi();
            funcionario = (FuncionarioFundevi)Session["funcionarioEliminar"];
            PlanillaFundevi plan = (PlanillaFundevi)Session["planillaSeleccionada"];
            if (funcionariosFundeviServicios.EliminarUnFuncionario(funcionario, plan.idPlanilla))
            {
                String url = Page.ResolveUrl("~/Planilla/AdministrarFuncionarioFundevi.aspx");
                Response.Redirect(url);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "No se pudo actualizar el salario" + "');", true);
            }

        }

        protected void btnEditar_Click1(object sender, EventArgs e)
        {
            int idFuncionario = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            FuncionarioFundevi funcionarioFundevi = funcionariosFundeviServicios.GetFuncionario(idFuncionario);
            Session["funcionarioEditar"] = null;
            Session["funcionarioEditar"] = funcionarioFundevi;
            TextBox1.Text = funcionarioFundevi.nombre;
            tb1.Text = funcionarioFundevi.salario + "";
            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalEditar();", true);
        }
        
        protected void Button2_Click(object sender, EventArgs e)
        {
            FuncionarioFundevi funcionario = new FuncionarioFundevi();
            funcionario = (FuncionarioFundevi)Session["funcionarioEditar"];
            if (!TextBox1.Equals("") && !tb1.Equals(""))
            {
                funcionario.nombre = TextBox1.Text;

                Double salario = 0;
                String txtSalario = tb1.Text.Replace(".", ",");
                if (Double.TryParse(txtSalario, out salario))
                {
                    tb1.Text = salario.ToString();
                }

                funcionario.salario = salario;
                if (funcionariosFundeviServicios.EditarFuncionario(funcionario))
                {
                    String url = Page.ResolveUrl("~/Planilla/AdministrarFuncionarioFundevi.aspx");
                    Response.Redirect(url);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "No se pudo actualizar la información del funcionario" + "');", true);
                }
            }
        }
        
        /// <summary>
        /// Leonardo Carrion
        /// 04/sep/2019
        /// Efecto: levanta el modal para pasar los funcionarios entre periodos
        /// Requiere: dar clic en el boton de pasar funcionarios
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAsignarFuncionarios_Click(object sender, EventArgs e)
        {
            Session["funcionariosAPasar"] = Session["funcionariosFiltrados"];
            cargarDatosTblFuncionariosAPasar();

            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            PlanillaFundevi planillaFundevi = planillaFundeviServicios.getPlanilla(periodo.anoPeriodo);

            List<FuncionarioFundevi> listaFuncionarios = funcionariosFundeviServicios.GetFuncionariosPorPlanilla(planillaFundevi);

            Session["funcionariosAgregados"] = listaFuncionarios;
            
            cargarDatosTblFuncionariosAgregados();
            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalPasarFuncionario();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/sep/2019
        /// Efecto: carga la tabla de funcionarios segun el periodo seleccionado
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

            PlanillaFundevi planillaFundevi = planillaFundeviServicios.getPlanilla(periodo.anoPeriodo);

            List<FuncionarioFundevi> listaFuncionarios = funcionariosFundeviServicios.GetFuncionariosPorPlanilla(planillaFundevi);

            Session["funcionariosAgregados"] = listaFuncionarios;
            cargarDatosTblFuncionariosAgregados();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarFuncionario", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarFuncionario').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarFuncionario();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 04/sep/2019
        /// Efecto: regresa a la pantalla de administrar planillas fundevi
        /// Requiere: dar clic al boton de regresar
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Planilla/AdministrarPlanillaFundevi.aspx");
            Response.Redirect(url);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/sep/2019
        /// Efecto: copia el funcionario en el periodo seleccionado
        /// Requiere: dar clic en el boton de copiar funcionario y seleccionar planilla a pasar
        /// Modifica: agrega un nuevo funcionario en la planilla seleccionada
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSeleccionarFuncionario_Click(object sender, EventArgs e)
        {
            int idFuncionario = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<FuncionarioFundevi> listaFuncionario = (List<FuncionarioFundevi>)Session["funcionariosFiltrados"];

            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            PlanillaFundevi planillaFundevi = planillaFundeviServicios.getPlanilla(periodo.anoPeriodo);

            foreach (FuncionarioFundevi funcionario in listaFuncionario)
            {
                if (funcionario.idFuncionario == idFuncionario)
                {
                    
                    funcionario.idPlanilla = planillaFundevi.idPlanilla;

                    if (funcionariosFundeviServicios.InsertFuncionario(funcionario))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "El funcionario ha sido copiado correctamente" + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "No se pudo registrar al funcionario" + "');", true);
                    }
                    break;
                }
            }

            List<FuncionarioFundevi> listaFuncionarios = funcionariosFundeviServicios.GetFuncionariosPorPlanilla(planillaFundevi);

            Session["funcionariosAgregados"] = listaFuncionarios;
            cargarDatosTblFuncionariosAgregados();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarFuncionario", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarFuncionario').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarFuncionario();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 12/sep/2019
        /// Efecto: filtra la tabla de funcionarios a pasar
        /// Requiere: ingresar texto a filtrar y dar clic al boton de filtrar
        /// Modifica: datos de la tabla de funcionarios a pasar
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBuscarNombreFuncionarioAPasar_TextChanged(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            cargarDatosTblFuncionariosAPasar();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarFuncionario", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarFuncionario').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarFuncionario();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 13/sep/2019
        /// Efecto: filtra la tabla de funcionarios agregados 
        /// Requiere: ingresar texto a filtrar y dar clic al boton de filtrar
        /// Modifica: datos de la tabla de funcionarios agregados
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtBuscarNombreFuncionarioAgregadas_TextChanged(object sender, EventArgs e)
        {
            paginaActual3 = 0;
            cargarDatosTblFuncionariosAgregados();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalPasarFuncionario", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalPasarFuncionario').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalPasarFuncionario();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 13/sep/2019
        /// Efecto:
        /// Requiere:
        /// Modifica:
        /// Devuelve:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoFuncionario_Click(object sender, EventArgs e)
        {
            PlanillaFundevi planillaFundevi = (PlanillaFundevi)Session["planillaSeleccionada"];
            lblPlanillaModalNuevo.Text = planillaFundevi.anoPeriodo.ToString();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoFuncionario", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoFuncionario').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 13/sep/2019
        /// Efecto:
        /// Requiere:
        /// Modifica:
        /// Devuelve:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoFuncionarioModal_Click(object sender, EventArgs e)
        {
            Double salario = 0;
            String txtSalario = txtSalarioModalNuevo.Text.Replace(".", ",");
            if (Double.TryParse(txtSalario, out salario))
            {
                txtSalarioModalNuevo.Text = salario.ToString();

                if (String.IsNullOrEmpty(txtNombreFuncionarioModalNuevo.Text))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoFuncionario", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoFuncionario').hide();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe de ingresar el nombre completo de funcionario" + "');", true);
                }
                else
                {
                    FuncionarioFundevi funcionarioFundevi = new FuncionarioFundevi();
                    funcionarioFundevi.nombre = txtNombreFuncionarioModalNuevo.Text;
                    funcionarioFundevi.salario = salario;
                    PlanillaFundevi planillaFundevi = (PlanillaFundevi)Session["planillaSeleccionada"];
                    funcionarioFundevi.idPlanilla = planillaFundevi.idPlanilla;
                    funcionariosFundeviServicios.InsertFuncionario(funcionarioFundevi);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoFuncionario", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoFuncionario').hide();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se creo correctamente el nuevo funcionario" + "');", true);
                    List<FuncionarioFundevi> funcionarios = funcionariosFundeviServicios.GetFuncionariosPorPlanilla(planillaFundevi);
                    Session["funcionarios"] = funcionarios;
                    mostrarDatosTabla();

                    txtSalarioModalNuevo.Text = "";
                    txtNombreFuncionarioModalNuevo.Text = "";
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalNuevoFuncionario", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalNuevoFuncionario').hide();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El monto de salario es incorrecto" + "');", true);
            }
        }

    }
}