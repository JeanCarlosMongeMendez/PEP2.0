using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Servicios;

namespace Proyecto.Planilla
{
    public partial class AdministrarFuncionarioFundevi : System.Web.UI.Page
    {
        FuncionariosFundeviServicios serviciosFuncionario = new FuncionariosFundeviServicios();
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
        protected void Page_Load(object sender, EventArgs e)
        {
            PlanillaFundevi planillaFundevi = (PlanillaFundevi)Session["planillaSeleccionada"];

            if (!IsPostBack)
            {
                Session["funcionarios"] = null;
                Session["funcionariosFiltrados"] = null;
                List<FuncionarioFundevi> funcionarios = serviciosFuncionario.GetFuncionario(planillaFundevi);
                Session["funcionarios"] = funcionarios;
                Session["funcionariosFiltrados"] = funcionarios;
                PlanillaFundevi plan = (PlanillaFundevi)Session["planillaSeleccionada"];
                label.Text = "Funcionarios de la planilla: " + plan.anoPeriodo;
                mostrarDatosTabla();
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
            List<FuncionarioFundevi> listaPlanillasFiltrada = (List<FuncionarioFundevi>)funcionarios.Where(funcionario => funcionario.nombre.ToString().Contains(nombre)).ToList();

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
            FuncionarioFundevi funcionarioFundevi = serviciosFuncionario.GetFuncionario(idFuncionario);
            lblNombre.Text = funcionarioFundevi.nombre;
            lblSalario.Text = "" + funcionarioFundevi.salario;
            Session["funcionarioAjuste"] = funcionarioFundevi;
            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModal();", true);
        }

        protected void btnGuardarAjuste_Click(object sender, EventArgs e)
        {
            FuncionarioFundevi funcionarioFundevi = (FuncionarioFundevi)Session["funcionarioAjuste"];
            int salarioAjuste = Convert.ToInt32(txtAsalario.Text.ToString());
            if (serviciosFuncionario.actualizarSalario(funcionarioFundevi, salarioAjuste))
            {
                txtInfo.Text = "El salario se actualizó correctamente";
                String url = Page.ResolveUrl("~/Planilla/AdministrarFuncionarioFundevi.aspx");
                Response.Redirect(url);
            }
            else
            {
                txtInfo.Text = "No se pudo actualizar el salario";
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
            FuncionarioFundevi funcionarioFundevi = serviciosFuncionario.GetFuncionario(idFuncionario);
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
            if (serviciosFuncionario.EliminarUnFuncionario(funcionario, plan.idPlanilla))
            {
                String url = Page.ResolveUrl("~/Planilla/AdministrarFuncionarioFundevi.aspx");
                Response.Redirect(url);
            }
            else
            {
                txtInfo.Text = "No se pudo actualizar el salario";
            }

        }

        protected void btnEditar_Click1(object sender, EventArgs e)
        {
            int idFuncionario = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());
            FuncionarioFundevi funcionarioFundevi = serviciosFuncionario.GetFuncionario(idFuncionario);
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
                funcionario.salario = float.Parse(tb1.Text);
                if (serviciosFuncionario.EditarFuncionario(funcionario))
                {
                    String url = Page.ResolveUrl("~/Planilla/AdministrarFuncionarioFundevi.aspx");
                    Response.Redirect(url);
                }
                else
                {
                    txtInfo.Text = "No se pudo actualizar la información del funcionario";
                }
            }
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
    }
}