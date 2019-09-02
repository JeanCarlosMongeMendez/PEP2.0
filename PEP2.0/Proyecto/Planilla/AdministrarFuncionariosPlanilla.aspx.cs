using PEP;
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

namespace Proyecto.Planilla
{
    public partial class AdministrarFuncionariosPlanilla : System.Web.UI.Page
    {
        #region variables globales
        FuncionarioServicios funcionarioServicios = new FuncionarioServicios();
        EscalaSalarialServicios escalaSalarialServicios = new EscalaSalarialServicios();
        static Funcionario funcionarioSeleccionado = new Funcionario();
        static EscalaSalarial escalaSeleccionada = new EscalaSalarial();

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
                Entidades.Planilla planilla = (Entidades.Planilla)Session["planillaSeleccionada"];
                lblPeriodo.Text = planilla.periodo.anoPeriodo.ToString();
                lblAnualidad1.Text = planilla.anualidad1 + " %";
                lblAnualidad2.Text = planilla.anualidad2 + " %";

                List<Funcionario> listaFuncionarios = funcionarioServicios.getFuncionarios();

                Session["listaFuncionarios"] = listaFuncionarios;
                Session["listaFuncionariosFiltrada"] = listaFuncionarios;

                mostrarDatosTabla();

                List<EscalaSalarial> listaEscalas = escalaSalarialServicios.getEscalasSalarialesPorPeriodo(planilla.periodo);
                Session["listaEscalas"] = listaEscalas;
                ddlEscalaSalarial.DataSource = listaEscalas;
                ddlEscalaSalarial.DataTextField = "descEscalaSalarial";
                ddlEscalaSalarial.DataValueField = "idEscalaSalarial";

                ddlEscalaSalarial.DataBind();
            }
        }
        #endregion

        #region logica

        /// <summary>
        /// Leonardo Carrion
        /// 17/jul/2019
        /// Efecto: calcula el monto de la anualidad
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: monto de anualidad
        /// </summary>
        /// <returns></returns>
        private Double calcularMontoAnualidad()
        {
            Double montoAnualiadad = 0;

            Double montoEscalafon = 0;

            try
            {
                String montoTxt = txtMontoEscalafones.Text.Replace(".", ",");
                Double monto = Convert.ToDouble(montoTxt);

                montoEscalafon = monto;
            }
            catch
            {
                txtMontoAnualidades.Text = "0";
            }

            Double porcentajeAnualidad = 0;
            try
            {
                String montoTxt = txtPorcentajeAnualidades.Text.Replace(".", ",");
                porcentajeAnualidad = Convert.ToDouble(montoTxt);
            }
            catch
            {
                txtPorcentajeAnualidades.Text = "0";
            }

            montoAnualiadad = (escalaSeleccionada.salarioBase1 + montoEscalafon) * (porcentajeAnualidad / 100);

            return montoAnualiadad;
        }

        private Double calcularSalarioContratacion()
        {
            Double salarioBaseI = escalaSeleccionada.salarioBase1;
            Double escalafon = calcularMontoEscalafon();
            Double anualidad = calcularMontoAnualidad();
            return (salarioBaseI + escalafon + anualidad);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 17/jul/2019
        /// Efecto: calcula el monto de escalafon
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: monto de escalafones
        /// </summary>
        /// <returns></returns>
        private Double calcularMontoEscalafon()
        {
            Double montoEscalafon = 0;
            int numeroEscalafones = 0;
            try
            {
                numeroEscalafones = Convert.ToInt32(txtEscalafones.Text);
            }
            catch
            {
                txtEscalafones.Text = "0";
            }

            Double sumaSalario1 = 0, sumaSalario2 = 0;

            try
            {
                String salarioTxt = txtSumaSalarioBase1.Text.Replace(".", ",");
                Double salario = Convert.ToDouble(salarioTxt);

                sumaSalario1 = salario;
            }
            catch
            {
                txtSumaSalarioBase1.Text = "0";
            }

            try
            {
                String salarioTxt = txtSumaSalarioBase2.Text.Replace(".", ",");
                Double salario = Convert.ToDouble(salarioTxt);

                sumaSalario2 = salario;
            }
            catch
            {
                txtSumaSalarioBase2.Text = "0";
            }

            montoEscalafon = ((escalaSeleccionada.salarioBase1 + sumaSalario1) * numeroEscalafones) * (escalaSeleccionada.porentajeEscalafones / 100);

            return montoEscalafon;
        }

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

            Session["listaPlanillasFiltrada"] = listaFuncionarioFiltrada;

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

        /// <summary>
        /// Leonardo Carrion
        /// 14/jun/2019
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

        #region eventos
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
        /// 16/jul/2019
        /// Efecto: levanta el modal para crear un funcionario y agregarlo a la planilla seleccionada
        /// Requiere: dar clic en el boton de "Nuevo funcionario"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoFuncionario_Click(object sender, EventArgs e)
        {
            List<EscalaSalarial> listaEscalas = (List<EscalaSalarial>)Session["listaEscalas"];

            EscalaSalarial escalaSeleccionadaModal = new EscalaSalarial();
            escalaSeleccionadaModal.idEscalaSalarial = Convert.ToInt32(ddlEscalaSalarial.SelectedValue);

            foreach (EscalaSalarial escalaSalarial in listaEscalas)
            {
                if (escalaSalarial.idEscalaSalarial == escalaSeleccionadaModal.idEscalaSalarial)
                {
                    escalaSeleccionada = escalaSalarial;
                    txtSalarioBase1ModalNuevo.Text = escalaSalarial.salarioBase1.ToString();
                    txtSalarioBase2ModalNuevo.Text = escalaSalarial.salarioBase2.ToString();
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
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
            String url = Page.ResolveUrl("~/Planilla/AdministrarPlanilla.aspx");
            Response.Redirect(url);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 17/jul/2019
        /// Efecto: cambia los salarios bases del modal de nuevo funcionario
        /// Requiere: cambiar escala salarial
        /// Modifica: salarios base 1 y 2
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlEscalaSalarial_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<EscalaSalarial> listaEscalas = (List<EscalaSalarial>)Session["listaEscalas"];

            EscalaSalarial escalaSeleccionadaModal = new EscalaSalarial();
            escalaSeleccionadaModal.idEscalaSalarial = Convert.ToInt32(ddlEscalaSalarial.SelectedValue);

            foreach (EscalaSalarial escalaSalarial in listaEscalas)
            {
                if (escalaSalarial.idEscalaSalarial == escalaSeleccionadaModal.idEscalaSalarial)
                {
                    escalaSeleccionada = escalaSalarial;
                    txtSalarioBase1ModalNuevo.Text = escalaSalarial.salarioBase1.ToString();
                    txtSalarioBase2ModalNuevo.Text = escalaSalarial.salarioBase2.ToString();
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 17/jul/2019
        /// Efecto: calcula el monto de escalafones segun los datos ingresados
        /// Requiere: ingresar numero de escalafones, escoger escala salarial y darle clic al boton de calcular en monto escalafones
        /// Modifica: monto de escalafones
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularEscalafones_Click(object sender, EventArgs e)
        {
            txtMontoEscalafones.Text = calcularMontoEscalafon().ToString();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 02/09/2019
        /// Efecto : Calcula el salario de contratacion y lo muestra en un campo de texto
        /// Requiere : Clickear el boton calcular
        /// Modifica : -
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularSalContratacion_Click(object sender, EventArgs e)
        {
            txtSalContratacion.Text = calcularSalarioContratacion().ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 17/jul/2019
        /// Efecto: calcula el monto de la anualidad segun los datos ingresados
        /// Requiere: monto salario base 1, monto escalafon y porcentaje anualidad
        /// Modifica: el campo con el monto de anualidad
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularMontoAnualidades_Click(object sender, EventArgs e)
        {
            txtMontoEscalafones.Text = calcularMontoEscalafon().ToString();
            txtMontoAnualidades.Text = calcularMontoAnualidad().ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        #endregion
    }
}