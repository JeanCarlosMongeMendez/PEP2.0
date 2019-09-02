using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Servicios;
using Entidades;

namespace Proyecto.Planilla
{
    public partial class AgregarFuncionarioFundevi : System.Web.UI.Page
    {
        PeriodoServicios periodoServicios = new PeriodoServicios();
        FuncionariosFundeviServicios funcionarioServicios = new FuncionariosFundeviServicios();
        PlanillaFundeviServicios fundeviServicios = new PlanillaFundeviServicios();
        protected void Page_Load(object sender, EventArgs e)
        {
            LlenarPeriodosDDL();
        }
        private void LlenarPeriodosDDL()
        {
            PlanillaFundeviServicios fundeviServicios = new PlanillaFundeviServicios();
            List<PlanillaFundevi> planillas = new List<PlanillaFundevi>();
            planillas = fundeviServicios.GetPlanillaFundevi();
            foreach (PlanillaFundevi planilla in planillas)
            {
                ListItem item = new ListItem("" + planilla.anoPeriodo);
                ddlPeriodo.Items.Add(item);
            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!txtNombre.Equals("") && !txtApellido.Equals(""))
            {
                FuncionarioFundevi funcionario = new FuncionarioFundevi();
                funcionario.nombre = txtNombre.Text;
                PlanillaFundevi planillaFundevi = new PlanillaFundevi();
                planillaFundevi = fundeviServicios.GetPlanilla(Convert.ToInt32(ddlPeriodo.SelectedValue.ToString()));
                funcionario.idPlanilla = planillaFundevi.idPlanilla;
                funcionario.salario = Convert.ToInt32(txtApellido.Text);


                if (funcionarioServicios.Insertar(funcionario))
                {
                    txtInfo.CssClass = "alert alert-success";
                    txtInfo.Text = "El funcionario ha sido registrado correctamente.";
                }
                else
                {
                    txtInfo.CssClass = "alert alert-danger";
                    txtInfo.Text = "No se pudo registrar al funcionario";
                }
            }
            txtNombre.Text = "";
            txtApellido.Text = "";

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Planilla/AdministrarPlanillaFundevi.aspx");
            Response.Redirect(url);
        }

    }
}