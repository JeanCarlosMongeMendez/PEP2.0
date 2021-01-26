using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PEP.Error
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2,13};
            Utilidades.escogerMenu(Page, rolesPermitidos);

            Exception ex = (Exception)Application["ERROR"];
            lblError.Text = ex.GetBaseException().Message;
        }
    }
}