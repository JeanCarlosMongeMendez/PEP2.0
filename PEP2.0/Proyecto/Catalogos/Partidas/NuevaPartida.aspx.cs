using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Servicios;
using Entidades;

namespace Proyecto.Catalogos.Partidas
{
    /// <summary>
    /// Adrián Serrano
    /// 27/may/2019
    /// Clase para crear una nueva partida
    /// </summary>
    public partial class NuevaPartida : System.Web.UI.Page
    {
        #region variables globales
        PartidaServicios partidaServicios = new PartidaServicios();
        PeriodoServicios periodoServicios = new PeriodoServicios();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LlenarPartidasPadreDDL();
                txtNumeroPartida.Attributes.Add("oninput", "validarTexto(this)");
                txtDescripcionPartida.Attributes.Add("oninput", "validarTexto(this)");
                CargarPeriodos();
            }
        }

        #endregion

        #region logica
        private void CargarPeriodos()
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            PeriodosDDL.Items.Clear();
            periodos = this.periodoServicios.ObtenerTodos();

            if (periodos.Count > 0)
            {
                foreach (Periodo periodo in periodos)
                {
                    string nombre;

                    if (periodo.habilitado)
                    {
                        nombre = periodo.anoPeriodo.ToString() + " (Actual)";
                    }
                    else
                    {
                        nombre = periodo.anoPeriodo.ToString();
                    }

                    ListItem itemPeriodo = new ListItem(nombre, periodo.anoPeriodo.ToString());
                    PeriodosDDL.Items.Add(itemPeriodo);
                }

                if (Session["periodo"] != null)
                {
                    string anoHabilitado = Session["periodo"].ToString();
                    PeriodosDDL.Items.FindByValue(anoHabilitado).Selected = true;
                }
            }
        }

        private void LlenarPartidasPadreDDL()
        {
            if (Session["periodo"] != null)
            {
                PartidasPadreDDL.Items.Clear();

                PartidasPadreDDL.Items.Add(new ListItem("Partida Padre", "null"));

                LinkedList<Partida> partidas = new LinkedList<Partida>();
                partidas = this.partidaServicios.ObtenerPorPeriodo(Convert.ToInt32(Session["periodo"].ToString()));
                foreach (Partida partida in partidas)
                {
                    if (partida.partidaPadre == null)
                    {
                        ListItem item = new ListItem(partida.numeroPartida + ": " + partida.descripcionPartida, partida.idPartida.ToString());
                        PartidasPadreDDL.Items.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Metodo que valida los campos que debe ingresar el usuario
        /// </summary>
        /// <returns>Devuelve true si todos los campos esta con datos correctos sino devuelve false
        /// y marcar lo campos para que el usuario vea cuales son los campos que se encuentran mal</returns>
        public Boolean validarCampos()
        {
            Boolean validados = true;

            #region validacion proyecto
            if (Session["periodo"] == null)
            {
                validados = false;
            }
            #endregion

            #region validacion numero partida
            String numeroPartida = txtNumeroPartida.Text;

            if (numeroPartida.Trim() == "")
            {
                txtNumeroPartida.CssClass = "form-control alert-danger";
                divNumeroPartidaIncorrecto.Style.Add("display", "block");

                validados = false;
            }
            #endregion

            #region validacion descripcion partida
            String descripcionPartida = txtDescripcionPartida.Text;

            if (descripcionPartida.Trim() == "")
            {
                txtDescripcionPartida.CssClass = "form-control alert-danger";
                divDescripcionPartidaIncorrecto.Style.Add("display", "block");

                validados = false;
            }
            #endregion

            return validados;
        }

        #endregion

        #region eventos

        /// <summary>
        /// Metodo que se activa cuando se cambia el numero
        /// </summary>
        protected void txtxNumeroPartida_TextChanged(object sender, EventArgs e)
        {
            txtNumeroPartida.CssClass = "form-control";
            lblNumeroPartidaIncorrecto.Visible = false;
        }

        /// <summary>
        /// Metodo que se activa cuando se cambia la descripcion
        /// </summary>
        protected void txtxDescripcionPartida_TextChanged(object sender, EventArgs e)
        {
            txtDescripcionPartida.CssClass = "form-control";
            lblDescripcionPartidaIncorrecto.Visible = false;
        }

        /// <summary>
        /// Metodo que se activa cuando se da click al boton de guardar
        /// valida que todos los campos se hayan ingresado correctamente 
        /// y guarda los datos en la base de datos 
        /// redirecciona a la pantalla de administacion de presupuesto de egreso
        /// </summary>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //se validan los campos antes de guardar los datos en la base de datos
            if (validarCampos())
            {
                Partida partida = new Partida();
                partida.numeroPartida = txtNumeroPartida.Text;
                partida.descripcionPartida = txtDescripcionPartida.Text;
                partida.periodo = new Periodo();
                partida.periodo.anoPeriodo = Convert.ToInt32(PeriodosDDL.SelectedValue);

                if (PartidasPadreDDL.SelectedValue == "null")
                {
                    partida.partidaPadre = null;
                }
                else
                {
                    partida.partidaPadre = new Partida();
                    partida.partidaPadre.idPartida = Convert.ToInt32(PartidasPadreDDL.SelectedValue);
                }
                
                int idPartida = this.partidaServicios.Insertar(partida);

                String url = Page.ResolveUrl("~/Catalogos/Partidas/AdministrarPartida.aspx");
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Metodo que se activa cuando se le da click al boton cancelar
        /// redirecciona a la pantalla de adminstracion de presupuesto de egreso
        /// </summary>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/Partidas/AdministrarPartida.aspx");
            Response.Redirect(url);
        }

        #endregion
    }
}