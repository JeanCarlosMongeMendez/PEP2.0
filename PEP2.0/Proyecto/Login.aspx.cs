using Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PEP
{
    public partial class Login : System.Web.UI.Page
    {
        #region variables globales
        ConexionServicios conexionServicios = new ConexionServicios();
        int rol;
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Master.FindControl("menu").Visible = false;

            Utilidades.SetLogDirectory();
            Session["rol"] = null;
            Session["nombreCompleto"] = null;

            this.txtPassword.Attributes.Add("onkeypress", "button_click(this,'" + this.btIngresar.ClientID + "')");
            this.txtUsuario.Attributes.Add("onkeypress", "button_click(this,'" + this.btIngresar.ClientID + "')");

        }
        #endregion

        #region logica
        public bool AuthenticateUser(string domain, string username, string password, string LdapPath, out string Errmsg)
        {
            Errmsg = "";
            string domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(LdapPath, domainAndUsername, password);
            try
            {
                // Bind to the native AdsObject to force authentication.
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                // Update the new path to the user in the directory
                LdapPath = result.Path;
                string _filterAttribute = (String)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                Errmsg = ex.Message;
                return false;
                throw new Exception("Error authenticating user." + ex.Message);

            }
            return true;
        }
        #endregion

        #region eventos
        protected void btIngresar_Click(object sender, EventArgs e)
        {
            string dominName = string.Empty;
            string adPath = string.Empty;
            string nombreCompleto = string.Empty;
            string userName = txtUsuario.Text.Trim().ToUpper();
            string strError = string.Empty;

            //estos son los datos que deben de llenar para realizar el login, el rol 2 es de administrador
            //string nombreCompleto = "nombre usuario";
            //string userName = "nusuario";
            //rol = 2;

            foreach (string key in System.Configuration.ConfigurationManager.AppSettings.Keys)
            {

                dominName = key.Contains("DirectoryDomain") ? System.Configuration.ConfigurationManager.AppSettings[key] : dominName;
                adPath = key.Contains("DirectoryPath") ? System.Configuration.ConfigurationManager.AppSettings[key] : adPath;
                if (!String.IsNullOrEmpty(dominName) && !String.IsNullOrEmpty(adPath))
                {
                    if (true == AuthenticateUser(dominName, userName, txtPassword.Text, adPath, out strError))
                    {

                        // Session["login"] = userName;

                        object[] datos = conexionServicios.loguearse(userName);
                        if (datos[0] != null)
                        {
                            rol = Convert.ToInt32(datos[0].ToString());
                            nombreCompleto = datos[1].ToString();

                            Session["rol"] = rol;
                            Session["nombreCompleto"] = nombreCompleto;
                            String url = Page.ResolveUrl("~/Default.aspx");
                            Response.Redirect(url);
                        }
                        else
                        {
                            lblNoUsuario.Visible = true;
                            lblError.Visible = false;
                        }
                    }
                    dominName = string.Empty;
                    adPath = string.Empty;
                    if (String.IsNullOrEmpty(strError)) break;
                }
            }
            if (!string.IsNullOrEmpty(strError))
            {
                lblError.Visible = true;
                lblNoUsuario.Visible = false;
            }
        }

        protected void btCancelar_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }
        #endregion
    }
}