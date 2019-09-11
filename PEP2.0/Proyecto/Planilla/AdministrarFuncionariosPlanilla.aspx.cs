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
        private FuncionarioServicios funcionarioServicios = new FuncionarioServicios();
        private EscalaSalarialServicios escalaSalarialServicios = new EscalaSalarialServicios();
        private static Funcionario funcionarioSeleccionado = new Funcionario();
        private static EscalaSalarial escalaSeleccionada = new EscalaSalarial();
        #endregion

        #region variables globales paginacion
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
        /// Requiere: porcentaje de anualidad, total salario base, monto de escalafones
        /// Modifica: -
        /// Devuelve: monto de anualidad
        /// </summary>
        /// <returns></returns>
        /// <param name="montoEscalafones"></param>
        /// <param name="porcentajeAnualidad"></param>
        /// <param name="sumaSalarioBase"></param>
        private Double calcularMontoAnualidad(double sumaSalarioBase, double montoEscalafones, double porcentajeAnualidad)
        {
            double montoAnualiadad = (sumaSalarioBase + montoEscalafones) * (porcentajeAnualidad / 100);
            return montoAnualiadad;
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 04/09/2019
        /// Efecto: Calcula el salario de contratacion
        /// Requiere: total salario base, monto escalafones, monto anualidad
        /// Modifica : -
        /// Devuelve : Salario de contratacion
        /// </summary>
        /// <returns></returns>
        /// <param name="sumaSalarioBase"></param>
        /// <param name="montoEscalafones"></param>
        /// <param name="montoAnualidad"></param>
        private Double calcularSalarioContratacion(double sumaSalarioBase, double montoEscalafones, double montoAnualidad)
        {
            double salarioContratacion = sumaSalarioBase + montoEscalafones + montoAnualidad;
            return salarioContratacion;
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 06/29/2019
        /// Efecto : Calcula el salario total de un funcionario
        /// Requiere : Salario base, suma al salario base
        /// Modifica : -
        /// Devuelve : Salario total
        /// </summary>
        /// <param name="salarioBase"></param>
        /// <param name="sumaSalarioBase"></param>
        /// <returns></returns>
        private Double calcularTotalSalarioBase(Double salarioBase, Double sumaSalarioBase)
        {
            Double total = 0;
            if (sumaSalarioBase == 0)
            {
                total = salarioBase;
            }
            else
            {
                total = salarioBase * sumaSalarioBase;
            }
            return total;
        }

        /// <summary>
        /// Leonardo Carrion
        /// 17/jul/2019
        /// Efecto: calcula el monto de escalafon
        /// Requiere: total salario base, numero de escalafones
        /// Modifica: -
        /// Devuelve: monto de escalafones
        /// </summary>
        /// <param name="numeroEscalafones"></param>
        /// <param name="totalSalarioBase"></param>
        /// <returns></returns>
        private Double calcularMontoEscalafon(double totalSalarioBase, int numeroEscalafones)
        {
            double montoEscalafon = (totalSalarioBase * numeroEscalafones) * (escalaSeleccionada.porentajeEscalafones / 100);
            return montoEscalafon;
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 04/09/2019
        /// Efecto : Calcula el salario mensual por semestre 
        /// Requiere : Salario contratacion, pago ley 8114
        /// Modifica : -
        /// Devuelve : Salario mensual del semestre
        /// </summary>
        /// <returns></returns>
        /// <param name="salarioContratacion"></param>
        /// <param name="pagoLey8114"></param>
        private Double calcularSalarioMensual(double salarioContratacion, int pagoLey8114)
        {
            double salarioMensual = salarioContratacion + pagoLey8114;
            return salarioMensual;
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 11/09/2019
        /// Efecto : Calcula el promedio del salario mensual de los dos semestres
        /// Requiere : Salario mensual primer semestre, salario mensual segundo semestre
        /// Modifica : -
        /// Devuelve : Promedio de los dos semetres
        /// </summary>
        /// <param name="salarioPrimerSemestre"></param>
        /// <param name="salarioSegundoSemestre"></param>
        /// <returns></returns>
        private Double calcularPromedioSemestres (double salarioPrimerSemestre, double salarioSegundoSemestre)
        {
            double promedio = (salarioPrimerSemestre + salarioSegundoSemestre) / 2;
            return promedio;
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

        #region paginacion
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
            LinkButton btnEscalafones = (LinkButton)sender;
            double sumaSalarioBase = 0;
            int cantidadEscalafones = 0;
            if (btnEscalafones.ID.Equals("btnCalcularEscalafonesI"))
            {
                Double.TryParse(txtSumaTotalSalarioBase1.Text, out sumaSalarioBase);
                int.TryParse(txtEscalafonesI.Text, out cantidadEscalafones);
                txtSumaTotalSalarioBase1.Text = sumaSalarioBase.ToString();
                txtEscalafonesI.Text = cantidadEscalafones.ToString();
                txtMontoEscalafonesI.Text = calcularMontoEscalafon(sumaSalarioBase, cantidadEscalafones).ToString();
            }
            else if (btnEscalafones.ID.Equals("btnCalcularEscalafonesII"))
            {
                Double.TryParse(txtSumaTotalSalarioBaseII.Text, out sumaSalarioBase);
                int.TryParse(txtEscalafonesII.Text, out cantidadEscalafones);
                txtSumaTotalSalarioBaseII.Text = sumaSalarioBase.ToString();
                txtEscalafonesII.Text = cantidadEscalafones.ToString();
                txtMontoEscalafonesII.Text = calcularMontoEscalafon(sumaSalarioBase, cantidadEscalafones).ToString();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 02/09/2019
        /// Efecto : Calcula el salario de contratacion y lo muestra en un campo de texto
        /// Requiere : Salario base, monto de escalafones, monto de anualidad, clickear el boton calcular salario contratacion
        /// Modifica : El campo de texto con el salario de contratacion
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularSalContratacion_Click(object sender, EventArgs e)
        {
            LinkButton btnSalarioContratacion = (LinkButton)sender;
            double sumaSalarioBase = 0;
            double montoEscalafones = 0;
            double montoAnualidad = 0;
            if (btnSalarioContratacion.ID.Equals("btnCalcularSalContratacionI"))
            {
                Double.TryParse(txtSumaTotalSalarioBase1.Text.Replace(".", ","), out sumaSalarioBase);
                Double.TryParse(txtMontoEscalafonesI.Text.Replace(".", ","), out montoEscalafones);
                Double.TryParse(txtMontoAnualidadesI.Text.Replace(".", ","), out montoAnualidad);
                txtSalContratacionI.Text = calcularSalarioContratacion(sumaSalarioBase, montoEscalafones, montoAnualidad).ToString();
                txtSumaTotalSalarioBase1.Text = sumaSalarioBase.ToString();
                txtMontoEscalafonesI.Text = montoEscalafones.ToString();
                txtMontoAnualidadesI.Text = montoAnualidad.ToString();
            }
            else if (btnSalarioContratacion.ID.Equals("btnCalcularSalContratacionII"))
            {
                Double.TryParse(txtSumaTotalSalarioBaseII.Text.Replace(".", ","), out sumaSalarioBase);
                Double.TryParse(txtMontoEscalafonesII.Text.Replace(".", ","), out montoEscalafones);
                Double.TryParse(txtMontoAnualidadesII.Text.Replace(".", ","), out montoAnualidad);
                txtSalContratacionII.Text = calcularSalarioContratacion(sumaSalarioBase, montoEscalafones, montoAnualidad).ToString();
                txtSumaTotalSalarioBaseII.Text = sumaSalarioBase.ToString();
                txtMontoEscalafonesII.Text = montoEscalafones.ToString();
                txtMontoAnualidadesII.Text = montoAnualidad.ToString();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 04/09/2019
        /// Efecto: Calcula el salario mensual a proponer real Ene-Jun
        /// Requiere : Salario de contratacion, Concepto de pago Ley 8114, clickear el boton calcular salario mensual
        /// Modifica : Campo de texto con el salario mensual 
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularSalarioMensualPorSemestre_Click(object sender, EventArgs e)
        {
            LinkButton btnCalcularSalario = (LinkButton)sender;
            double salarioContratacion = 0;
            int pagoLey8114 = 0;
            if (btnCalcularSalario.ID.Equals("btnCalcularSalarioMensualI"))
            {
                Double.TryParse(txtSalContratacionI.Text.Replace(".", ","), out salarioContratacion);
                int.TryParse(txtPagoLey8114.Text, out pagoLey8114);
                txtSalarioMensualEneroJunio.Text = calcularSalarioMensual(salarioContratacion, pagoLey8114).ToString();
                txtSalContratacionI.Text = salarioContratacion.ToString();
                txtPagoLey8114.Text = pagoLey8114.ToString();
            }
            else if (btnCalcularSalario.ID.Equals("btnCalcularSalarioMensualII"))
            {
                Double.TryParse(txtSalContratacionII.Text.Replace(".", ","), out salarioContratacion);
                int.TryParse(txtPagoLey8114.Text, out pagoLey8114);
                txtSalarioMensualJunioDiciembre.Text = calcularSalarioMensual(salarioContratacion, pagoLey8114).ToString();
                txtSalContratacionII.Text = salarioContratacion.ToString();
                txtPagoLey8114.Text = pagoLey8114.ToString();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        /// <summary>
        /// Leonardo Carrion
        /// 17/jul/2019
        /// Efecto: calcula el monto de la anualidad segun los datos ingresados
        /// Requiere: monto salario base, monto escalafon y porcentaje anualidad, clickear el boton calcular
        /// Modifica: el campo con el monto de anualidad
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularMontoAnualidades_Click(object sender, EventArgs e)
        {
            LinkButton bntMontoAnualidades = (LinkButton)sender;
            double sumaSalarioBase = 1.220;
            double montoEscalafones = 0;
            double porcentajeAnualidad = 0;

            if (bntMontoAnualidades.ID.Equals("btnCalcularMontoAnualidadesI"))
            {
                Double.TryParse(txtSumaTotalSalarioBase1.Text.Replace(".", ","), out sumaSalarioBase);
                Double.TryParse(txtMontoEscalafonesI.Text.Replace(".", ","), out montoEscalafones);
                Double.TryParse(txtPorcentajeAnualidadesI.Text.Replace(".", ","), out porcentajeAnualidad);
                txtMontoAnualidadesI.Text = calcularMontoAnualidad(sumaSalarioBase, montoEscalafones, porcentajeAnualidad).ToString();
                txtSumaTotalSalarioBase1.Text = sumaSalarioBase.ToString();
                txtMontoEscalafonesI.Text = montoEscalafones.ToString();
                txtPorcentajeAnualidadesI.Text = porcentajeAnualidad.ToString();
            }
            else if (bntMontoAnualidades.ID.Equals("btnCalcularMontoAnualidadesII"))
            {
                Double.TryParse(txtSumaTotalSalarioBaseII.Text.Replace(".", ","), out sumaSalarioBase);
                Double.TryParse(txtMontoEscalafonesII.Text.Replace(".", ","), out montoEscalafones);
                Double.TryParse(txtPorcentajeAnualidadesII.Text.Replace(".", ","), out porcentajeAnualidad);
                txtMontoAnualidadesII.Text = calcularMontoAnualidad(sumaSalarioBase, montoEscalafones, porcentajeAnualidad).ToString();
                txtSumaTotalSalarioBaseII.Text = sumaSalarioBase.ToString();
                txtMontoEscalafonesII.Text = montoEscalafones.ToString();
                txtPorcentajeAnualidadesII.Text = porcentajeAnualidad.ToString();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularNumeroEscalafones2_Click(object sender, EventArgs e)
        {
            int cantidadEscalafonesIsemestre = Convert.ToInt32(txtEscalafonesI.Text);
            DateTime fechaIngreso = Convert.ToDateTime(txtFecha.Text);
            DateTime fechaDivisionSemestres = Convert.ToDateTime("6/01/2020");
            if (cantidadEscalafonesIsemestre < escalaSeleccionada.topeEscalafones && DateTime.Compare(fechaDivisionSemestres, fechaIngreso) < 0)
            {
                txtEscalafonesII.Text = (cantidadEscalafonesIsemestre + 1).ToString();
            }
            else
            {
                txtEscalafonesII.Text = cantidadEscalafonesIsemestre.ToString();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 11/09/2019
        /// Efecto : Calcular el promedio de los salarios semestrales y lo muestra en un campo de texto
        /// Requiere: salario mensual primer semestre, salario mensual segundo semestre, clickear el boton calcular promedio
        /// Mofidica: campo de texto promedio dos semetres
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularPromedioSemestres_Click(object sender, EventArgs e)
        {
            double primerSemestre = 0;
            double segundoSemestre = 0;
            Double.TryParse(txtSalarioMensualEneroJunio.Text.Replace(".", ","), out primerSemestre);
            Double.TryParse(txtSalarioMensualJunioDiciembre.Text.Replace(".", ","), out segundoSemestre);
            txtPromedioSemestres.Text = calcularPromedioSemestres(primerSemestre, segundoSemestre).ToString();
            txtSalarioMensualEneroJunio.Text = primerSemestre.ToString();
            txtSalarioMensualJunioDiciembre.Text = segundoSemestre.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        /// <summary>
        /// Jean Carlos Monge Mendez
        /// 06/09/2019
        /// Efecto : Calcula el salario total del funcionario
        /// Requiere : Salario base, Suma al salario base, clickear el boton de calcular total salario base 
        /// Modifica : Campo de texto con el valor del salario total
        /// Devuelve : -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularTotalSalarioBase_Click(object sender, EventArgs e)
        {
            LinkButton btnTotalSalarioBase = (LinkButton)sender;
            double salarioBase = 0;
            double sumaSalarioBase = 0;
            if (btnTotalSalarioBase.ID.Equals("btnCalcularTotalSalarioBaseI"))
            {
                salarioBase = escalaSeleccionada.salarioBase1;
                Double.TryParse(txtSumaSalarioBase1.Text.Replace(".", ",").ToString(), out sumaSalarioBase);
                txtSumaTotalSalarioBase1.Text = calcularTotalSalarioBase(salarioBase, sumaSalarioBase).ToString();
                txtSumaSalarioBase1.Text = sumaSalarioBase.ToString();
            }
            else if (btnTotalSalarioBase.ID.Equals("btnCalcularTotalSalarioBaseII"))
            {
                salarioBase = escalaSeleccionada.salarioBase2;
                Double.TryParse(txtSumaSalarioBase2.Text.Replace(".", ",").ToString(), out sumaSalarioBase);
                txtSumaTotalSalarioBaseII.Text = calcularTotalSalarioBase(salarioBase, sumaSalarioBase).ToString();
                txtSumaSalarioBase2.Text = sumaSalarioBase.ToString();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Daniel Solano
        /// 06/Set/2019
        /// Efecto: Cambia el  punto por coma en el valor decimal
        /// Requiere: -
        /// Modifica: campo de texto de suma salario
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtFormatoComas_TextChanged(object sender, EventArgs e)
        {

            LinkButton txtSumasalario = (LinkButton)sender;
            //falta validar cual es el campo en que se va a hacer el cambio
            if (!String.IsNullOrEmpty(txtSumaSalarioBase1.Text.Trim()))
            {
                Double sumaSalarioBase = Convert.ToDouble(txtSumaSalarioBase1.Text.Replace(".", ","));
                txtSalarioBase1.Text = Convert.ToString(sumaSalarioBase);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalNuevoFuncionario();", true);
            }
        }
        #endregion
    }
}