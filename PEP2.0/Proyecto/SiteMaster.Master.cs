using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PEP
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["nombreCompleto"] != null)
            {
                username.InnerText = "Bienvenid@ " + Session["nombreCompleto"].ToString();
            }   
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Session.Abandon();
            Session.Clear();
            String url = Page.ResolveUrl("~/login.aspx");
            Response.Redirect(url);
        }
        public void Mensaje(string mensajes, string tipo)
        {
            mensaje.Text = mensajes;
            alert.Text = tipo;
            ScriptManager.RegisterStartupScript(this, GetType(), "oso", "Mensaje();", true);
        }
    }
}