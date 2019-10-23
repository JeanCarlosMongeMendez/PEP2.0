﻿using Entidades;
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

namespace Proyecto.Planilla
{
    public partial class AdministrarProyeccion : System.Web.UI.Page
    {
        #region variables globales
        private FuncionarioServicios funcionarioServicios = new FuncionarioServicios();
        private PlanillaServicios planillaServicios = new PlanillaServicios();
        #endregion

        #region variables globales paginacion Funcionarios
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
                
                //llenar drop down list
                List<Entidades.Planilla> periodos = planillaServicios.getPlanillas();
                ddlPeriodo.DataValueField = "idPlanilla";
                ddlPeriodo.DataTextField = "periodo";
                ddlPeriodo.DataSource = from a in periodos select new { a.idPlanilla, periodo = a.periodo.anoPeriodo};
                ddlPeriodo.SelectedValue = periodos.First().idPlanilla.ToString();
                ddlPeriodo.DataBind();

                List<Funcionario> listaFuncionarios = funcionarioServicios.getFuncionarios(periodos.First().idPlanilla);
                Session["listaFuncionarios"] = listaFuncionarios;
                Session["listaFuncionariosFiltrada"] = listaFuncionarios;
                mostrarDatosTabla();
            }
        }
        #endregion

        #region logica

        /// <summary>
        /// Leonardo Carrion
        /// 14/jun/2019
        /// Efecto: carga los datos filtrados en la tabla y realiza la paginacion correspondiente
        /// Requiere: -
        /// Modifica: los datos mostrados en pantalla
        /// Devuelve: -
        /// </summary>
        public void mostrarDatosTabla()
        {
            List<Funcionario> listaFuncionarios = (List<Funcionario>)Session["listaFuncionarios"];

            String nombre = "";

            if (!String.IsNullOrEmpty(txtBuscarNombre.Text))
            {
                nombre = txtBuscarNombre.Text;
            }

            List<Funcionario> listaFuncionarioFiltrada = (List<Funcionario>)listaFuncionarios.Where(funcionario => funcionario.nombreFuncionario.ToString().Contains(nombre)).ToList();

            Session["listaFuncionariosFiltrada"] = listaFuncionarioFiltrada;

            var dt = listaFuncionarioFiltrada;
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

            rpFuncionarios.DataSource = pgsource;
            rpFuncionarios.DataBind();

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
        /// 14/jun/2019
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
        /// 14/jun/2019
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
        /// 14/jun/2019
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
        /// 14/jun/2019
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
        /// 14/jun/2019
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
        /// 14/jun/2019
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
        /// 16/jul/2019
        /// Efecto: devuelve a la pantalla de administrar planillas
        /// Requiere: dar clic al boton de "regresar"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Default.aspx");
            Response.Redirect(url);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 18/10/2019
        /// Efecto : Calcula la proyeccion de todos los funcionarios
        /// Requiere : Clickear el boton "Calcular Proyeccion" del formulario
        /// Modifica : -
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularProyeccion_Click(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        protected void btnAprobar_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 23/10/2019
        /// Efecto : Actualiza la lista de funcionarios dependiendo del periodo seleccionado
        /// Requiere : Cambiar la seleccion del ddlPeriodos
        /// Modifica : Lista de funcionarios que se muestran
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Funcionario> listaFuncionarios = funcionarioServicios.getFuncionarios(Convert.ToInt32(ddlPeriodo.SelectedValue));
            Session["listaFuncionarios"] = listaFuncionarios;
            Session["listaFuncionariosFiltrada"] = listaFuncionarios;
            mostrarDatosTabla();
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 20/09/2019
        /// Efecto : Muestra la proyeccion del funcionario seleccionado
        /// Requiere : Clickear el boton "Seleccionar"
        /// Modifica : -
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelccionarFuncionario_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            Funcionario funcionarioDistribucion = null;
            List<Funcionario> funcionarios = (List<Funcionario>)Session["listaFuncionariosFiltrada"];
            foreach (Funcionario funcionario in funcionarios)
            {
                if (funcionario.idFuncionario == id)
                {
                    funcionarioDistribucion = funcionario;
                    break;
                }
            }
            txtNombreCompleto.Text = funcionarioDistribucion.nombreFuncionario;
            txtPeriodo.Text = ddlPeriodo.SelectedItem.Text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalVerDistribucionDeFuncionario();", true);
        }
        #endregion

    }
}