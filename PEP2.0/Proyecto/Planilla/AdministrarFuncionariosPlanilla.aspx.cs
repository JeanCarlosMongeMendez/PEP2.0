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
        private Double calcularMontoAnualidad(Boolean boton)
        {
            Double montoAnualiadad = 0;
            Double montoEscalafon = 0;
            Double salarioBase = 0;
            try
            {
                if (boton)
                {
                    salarioBase = escalaSeleccionada.salarioBase1;
                    String montoTxt = txtMontoEscalafonesI.Text.Replace(".", ",");
                    Double monto = Convert.ToDouble(montoTxt);
                    montoEscalafon = monto;
                }
                else
                {

                    salarioBase = escalaSeleccionada.salarioBase2;
                    String montoTxt = txtMontoEscalafonesII.Text.Replace(".", ",");
                    Double monto = Convert.ToDouble(montoTxt);
                    montoEscalafon = monto;
                }
            }
            catch
            {
                txtMontoAnualidades.Text = "0";
            }

            Double porcentajeAnualidad = 0;
            try
            {

                if (boton)
                {

                    String montoTxt = txtPorcentajeAnualidadesI.Text.Replace(".", ",");
                    porcentajeAnualidad = Convert.ToDouble(montoTxt);
                }
                else
                {
                    String montoTxt = txtPorcentajeAnualidadesII.Text.Replace(".", ",");
                    porcentajeAnualidad = Convert.ToDouble(montoTxt);
                }
            }
            catch
            {
                txtPorcentajeAnualidades.Text = "0";
            }



            montoAnualiadad = (salarioBase + montoEscalafon) * (porcentajeAnualidad / 100);

            return montoAnualiadad;
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 04/09/2019
        /// Efecto: Calcula el salario de contratacion
        /// Requiere: -
        /// Modifica : -
        /// Devuelve : Salario de contratacion
        /// </summary>
        /// <returns></returns>
        private Double calcularSalarioContratacion()
        {
            Double montoEscalafon = 0;
            try
            {
                String montoTxt = txtMontoEscalafonesI.Text.Replace(".", ",");
                Double monto = Convert.ToDouble(montoTxt);

                montoEscalafon = monto;

            }
            catch
            {
                txtMontoAnualidadesI.Text = "0";
            }

            Double porcentajeAnualidad = 0;
            try
            {
                String montoTxt = txtPorcentajeAnualidadesI.Text.Replace(".", ",");
                porcentajeAnualidad = Convert.ToDouble(montoTxt);
            }
            catch
            {
                txtPorcentajeAnualidadesI.Text = "0";
            }
            





            Double salarioBaseI = escalaSeleccionada.salarioBase1;
            Double escalafon = calcularMontoEscalafon();
            Double anualidad = calcularMontoAnualidad(montoEscalafon, porcentajeAnualidad);
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
        private Double calcularMontoEscalafon(Boolean boton)
        {
            int numeroEscalafones = 0;
            Double salarioBase = 0;
            try
            {
<<<<<<< HEAD
                if (boton)
                {
                    numeroEscalafones = Convert.ToInt32(txtEscalafonesI.Text);
                }
                else
                {
                    numeroEscalafones = Convert.ToInt32(txtEscalafonesII.Text);
                }
=======
                numeroEscalafones = Convert.ToInt32(txtEscalafonesI.Text);
>>>>>>> 5236de5898702114d95497a0bbd955c6a835dbc0
            }
            catch
            {
                txtEscalafonesI.Text = "0";
            }

            Double sumaSalario1 = 0, sumaSalario2 = 0;

            try
            {
                if (boton)
                {
                    String salarioTxt = txtSumaSalarioBase1.Text.Replace(".", ",");
                    Double salario = Convert.ToDouble(salarioTxt);

                    sumaSalario1 = salario;
                }
                else
                {
                    String salarioTxt = txtSumaSalarioBase2.Text.Replace(".", ",");
                    Double salario = Convert.ToDouble(salarioTxt);

                    sumaSalario1 = salario;
                }
            }
            catch
            {
                txtSumaSalarioBase1.Text = "0";
            }

            try
            {
                if (boton)
                {
                    salarioBase = escalaSeleccionada.salarioBase1;
                    String salarioTxt = txtSumaSalarioBase2.Text.Replace(".", ",");
                    Double salario = Convert.ToDouble(salarioTxt);

                    sumaSalario2 = salario;
                }
                else
                {
                    salarioBase = escalaSeleccionada.salarioBase2;
                    String salarioTxt = txtSumaSalarioBase1.Text.Replace(".", ",");
                    Double salario = Convert.ToDouble(salarioTxt);

                    sumaSalario2 = salario;
                }
            }
            catch
            {
                txtSumaSalarioBase2.Text = "0";
            }




            Double montoEscalafon = ((salarioBase + sumaSalario1) * numeroEscalafones) * (escalaSeleccionada.porentajeEscalafones / 100);

            return montoEscalafon;
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 04/09/2019
        /// Efecto : Calcula el salario mensual Enero - Junio 
        /// Requiere : -
        /// Modifica : -
        /// Devuelve : Salario mensual Enero - Junio
        /// </summary>
        /// <returns></returns>
        private Double calcularSalarioMensualEneroJunio()
        {
            Double salarioMensual = 0;
            int pagoLey8114 = 0;
            try
            {
                pagoLey8114 = Convert.ToInt32(txtPagoLey8114.Text);
            }
            catch
            {
                txtEscalafonesI.Text = "0";
            }
            salarioMensual = pagoLey8114 + calcularSalarioContratacion();
            return salarioMensual;
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
                    txtSalarioBase1.Text = escalaSalarial.salarioBase1.ToString();
                    txtSalarioBase2.Text = escalaSalarial.salarioBase2.ToString();
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
                    txtSalarioBase1.Text = escalaSalarial.salarioBase1.ToString();
                    txtSalarioBase2.Text = escalaSalarial.salarioBase2.ToString();
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
<<<<<<< HEAD
            LinkButton button = (LinkButton)sender;
            bool boton = button.ID.Equals("btnCalcularMontoAnualidadesI");
            txtMontoEscalafonesI.Text = calcularMontoEscalafon(boton).ToString();
=======
            txtMontoEscalafonesI.Text = calcularMontoEscalafon().ToString();
>>>>>>> 5236de5898702114d95497a0bbd955c6a835dbc0
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 02/09/2019
        /// Efecto : Calcula el salario de contratacion y lo muestra en un campo de texto
        /// Requiere : Salario base 1, monto de escalafones, monto de anualidad
        /// Modifica : El campo de texto con el salario de contratacion
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularSalContratacion_Click(object sender, EventArgs e)
        {
            txtSalContratacionI.Text = calcularSalarioContratacion().ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 04/09/2019
        /// Efecto: Calcula el salario mensual a proponer real Ene-Jun
        /// Requiere : Salario de contratacion, Concepto de pago Ley 8114
        /// Modifica : Campo de texto con el salario mensual enero - junio
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularSalarioMensualEneroJunio_Click(object sender, EventArgs e)
        {
            txtSalarioMensualEneroJunio.Text = calcularSalarioMensualEneroJunio().ToString();
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
<<<<<<< HEAD
            #region anualidades
            
            LinkButton button = (LinkButton)sender;
            bool boton = button.ID.Equals("btnCalcularMontoAnualidadesI");
            
            txtMontoAnualidades.Text = calcularMontoAnualidad(boton).ToString();
            #endregion
=======
<<<<<<< HEAD
            txtMontoEscalafonesI.Text = calcularMontoEscalafon().ToString();
            txtMontoAnualidadesI.Text = calcularMontoAnualidad().ToString();
=======
            Double montoEscalafon = 0;
            try
            {
                String montoTxt = txtMontoEscalafones.Text.Replace(".", ",");
                Double monto = Convert.ToDouble(montoTxt);
>>>>>>> 5236de5898702114d95497a0bbd955c6a835dbc0





            #region escalafon

           

            txtMontoEscalafonesI.Text = calcularMontoEscalafon(boton).ToString();
            #endregion

<<<<<<< HEAD
=======
            txtMontoEscalafones.Text = calcularMontoEscalafon().ToString();
>>>>>>> be10332ac470dea9f416f57176a3d0d4d1c884d8
>>>>>>> 5236de5898702114d95497a0bbd955c6a835dbc0
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        protected void btnCalcularNumeroEscalafones2_Click(object sender, EventArgs e)
        {

        }

        protected void btnCalcularPromedioSemestres_Click(object sender, EventArgs e)
        {

        }

        protected void btnCalcularSalarioPropuesto_Click(object sender, EventArgs e)
        {

        }

        protected void btnCalcularTotalSalarioBase_Click(object sender, EventArgs e)
        {
        }

        #endregion
    }
}