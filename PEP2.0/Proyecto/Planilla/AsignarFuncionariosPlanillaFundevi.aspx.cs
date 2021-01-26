using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Servicios;
using Entidades;
using PEP;

namespace Proyecto.Planilla
{
    public partial class AsignarFuncionariosPlanillaFundevi : System.Web.UI.Page
    {
        PeriodoServicios periodoServicios = new PeriodoServicios();
        FuncionariosFundeviServicios funcionarioServicios = new FuncionariosFundeviServicios();
        PlanillaFundeviServicios fundeviServicios = new PlanillaFundeviServicios();
        protected void Page_Load(object sender, EventArgs e)
        {
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);
            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                LlenarPeriodosDDL();
            }
            else
            {
                PasarProyectosBtn.Click += new EventHandler((snd, evt) => PasarProyectosBtn_Click(snd, evt));
                DevolverProyectosBtn.Click += new EventHandler((snd, evt) => DevolverProyectosBtn_Click(snd, evt));
            }
        }
        private void LlenarPeriodosDDL()
        {
            PlanillaFundeviServicios fundeviServicios = new PlanillaFundeviServicios();
            List<PlanillaFundevi> planillas = new List<PlanillaFundevi>();
            planillas = fundeviServicios.GetPlanillasFundevi();
            foreach (PlanillaFundevi planilla in planillas)
            {
                ListItem item = new ListItem("" + planilla.anoPeriodo);
                PeriodosDDL.Items.Add(item);
                PeriodosNuevosDDL.Items.Add(item);
                CargarFuncionariosActuales();
            }

        }

        protected void btnNuevoFuncionario_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Planilla/AgregarFuncionarioFundevi.aspx");
            Response.Redirect(url);
        }
        private void CargarFuncionariosActuales()
        {
            // ProyectosActualesDDL.Items.Clear();
            ProyectosActualesLB.Items.Clear();
            ProyectosNuevosLB.Items.Clear();

            if (!PeriodosDDL.SelectedValue.Equals(""))
            {
                AnoActual.Text = PeriodosDDL.SelectedValue;
                Session["periodo"] = PeriodosDDL.SelectedValue;

                List<FuncionarioFundevi> funcionarios = new List<FuncionarioFundevi>();
                PlanillaFundevi planilla = fundeviServicios.GetPlanilla(Convert.ToInt32(PeriodosDDL.SelectedValue));

                funcionarios = funcionarioServicios.GetFuncionariosPorPlanilla(planilla);
                if (funcionarios.Count > 0)
                {
                    foreach (FuncionarioFundevi funcionario in funcionarios)
                    {
                        String item = funcionario.nombre;
                        ListItem itemLB = new ListItem(item, funcionario.idFuncionario.ToString());

                        ProyectosActualesLB.Items.Add(itemLB);
                    }
                }
            }
        }
        protected void FuncionariosPeriodos(object sender, EventArgs e)
        {
            CargarFuncionariosActuales();
        }

        protected void PasarProyectosBtn_Click(object sender, EventArgs e)
        {
            if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
            {
                if (!PeriodosNuevosDDL.SelectedValue.Trim().Equals(""))
                {
                    foreach (ListItem funcionarios in ProyectosActualesLB.Items)
                    {
                        if (funcionarios.Selected)
                        {
                            ProyectosNuevosLB.Items.Add(funcionarios);
                        }
                    }
                }

                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
            }
        }
        protected void DevolverProyectosBtn_Click(object sender, EventArgs e)
        {
            if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
            {
                ListBox proyectosNuevos = new ListBox();
                foreach (ListItem proyecto in ProyectosNuevosLB.Items)
                {
                    proyectosNuevos.Items.Add(proyecto);
                }

                foreach (ListItem proyecto in proyectosNuevos.Items)
                {
                    if (proyecto.Selected)
                    {
                        ProyectosNuevosLB.Items.Remove(proyecto);
                    }
                }

                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
            }
        }

        #region otros

        private void Toastr(string tipo, string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr." + tipo + "('" + mensaje + "');", true);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["CheckRefresh"] = Session["CheckRefresh"];
        }

        #endregion

        protected void GuardarFuncionariosBtn_Click(object sender, EventArgs e)
        {
            if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
            {
                if (ProyectosNuevosLB.Items.Count > 0)
                {
                    int anoPeriodo = Convert.ToInt32(PeriodosNuevosDDL.SelectedValue);
                    PlanillaFundevi planilla = fundeviServicios.GetPlanilla(anoPeriodo);
                    if (ProyectosNuevosLB.Items.Count > 0)
                    {
                        LinkedList<int> proyectosId = new LinkedList<int>();
                        LinkedList<FuncionarioFundevi> funcionarios = new LinkedList<FuncionarioFundevi>();
                        foreach (ListItem idFuncionario in ProyectosNuevosLB.Items)
                        {
                            FuncionarioFundevi funcionario = new FuncionarioFundevi();
                            funcionario = funcionarioServicios.GetFuncionario(Convert.ToInt32(idFuncionario.Value));
                            funcionarios.AddLast(funcionario);
                        }

                        // int respuesta = this.proyectoServicios.Guardar(proyectosId, Int32.Parse(PeriodosNuevosDDL.SelectedValue));

                        if (funcionarioServicios.InsertarFuncionarios(funcionarios, planilla.idPlanilla))
                        {
                            Toastr("success", "Se han guardado los cambios con éxito!");
                            String url = Page.ResolveUrl("~/Planilla/AdministrarPlanillaFundevi.aspx");
                            Response.Redirect(url);
                        }
                        else
                        {
                            Toastr("error", "Error al guardar los funcionarios en la planilla");
                        }
                    }

                    Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                }
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 02/sep/2019
        /// Efecto: regresa a la pantalla de administrar funcionarios fundevi
        /// Requiere: dar clic al boton de regresar
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Planilla/AdministrarFuncionarioFundevi.aspx");
            Response.Redirect(url);
        }
    }
}