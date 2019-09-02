using Servicios;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proyecto.Catalogos.Partidas
{
    public partial class AdministrarPartida : System.Web.UI.Page
    {
        #region variables globales
        private PeriodoServicios periodoServicios;
        private PartidaServicios partidaServicios;
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            PEP.Utilidades.escogerMenu(Page, rolesPermitidos);

            this.periodoServicios = new PeriodoServicios();
            this.partidaServicios = new PartidaServicios();

            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());

                CargarPeriodos();
            }
            else
            {
                PasarPartidasBtn.Click += new EventHandler((snd, evt) => PasarPartidasBtn_Click(snd, evt));
                DevolverPartidasBtn.Click += new EventHandler((snd, evt) => DevolverPartidasBtn_Click(snd, evt));
                GuardarPartidasBtn.Click += new EventHandler((snd, evt) => GuardarPartidas_Click(snd, evt));
            }
        }
        #endregion

        #region logica

        private void CargarPeriodos()
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            PeriodosDDL.Items.Clear();
            periodos = this.periodoServicios.ObtenerTodos();
            int anoHabilitado = 0;

            if (periodos.Count > 0)
            {
                foreach (Periodo periodo in periodos)
                {
                    string nombre;

                    if (periodo.habilitado)
                    {
                        nombre = periodo.anoPeriodo.ToString() + " (Actual)";
                        anoHabilitado = periodo.anoPeriodo;
                    }
                    else
                    {
                        nombre = periodo.anoPeriodo.ToString();
                    }

                    ListItem itemPeriodo = new ListItem(nombre, periodo.anoPeriodo.ToString());
                    PeriodosDDL.Items.Add(itemPeriodo);
                }

                if (anoHabilitado != 0)
                {
                    PeriodosDDL.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                }

                CargarPeriodosNuevos();
                CargarPartidasActuales();
            }
        }

        private void CargarPeriodosNuevos()
        {
            PeriodosNuevosDDL.Items.Clear();
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            periodos = this.periodoServicios.ObtenerTodos();

            if (periodos.Count > 0)
            {
                ListItem itemVacio = new ListItem("");
                PeriodosNuevosDDL.Items.Add(itemVacio);

                foreach (Periodo periodo in periodos)
                {
                    if (!periodo.anoPeriodo.ToString().Equals(PeriodosDDL.SelectedValue))
                    {
                        ListItem itemPeriodoNuevo = new ListItem(periodo.anoPeriodo.ToString(), periodo.anoPeriodo.ToString());
                        PeriodosNuevosDDL.Items.Add(itemPeriodoNuevo);
                    }
                }
            }
        }

        private void CargarPartidasActuales()
        {
            PartidasActualesLB.Items.Clear();
            PartidasNuevasLB.Items.Clear();

            if (!PeriodosDDL.SelectedValue.Equals(""))
            {
                AnoActual.Text = PeriodosDDL.SelectedValue;
                Session["periodo"] = PeriodosDDL.SelectedValue;

                LinkedList<Partida> partidas = new LinkedList<Partida>();
                partidas = this.partidaServicios.ObtenerPorPeriodo(Int32.Parse(PeriodosDDL.SelectedValue));

                if (partidas.Count > 0)
                {
                    foreach (Partida partida in partidas)
                    {
                        if (partida.partidaPadre != null)
                        {
                            ListItem itemLB = new ListItem(partida.numeroPartida + " : " + partida.descripcionPartida, partida.idPartida.ToString());
                            PartidasActualesLB.Items.Add(itemLB);
                        }
                    }
                }
            }
        }

        private void CargarPartidasNuevas()
        {
            PartidasNuevasLB.Items.Clear();

            if (!PeriodosNuevosDDL.SelectedValue.Equals(""))
            {
                LinkedList<Partida> partidas = new LinkedList<Partida>();
                partidas = this.partidaServicios.ObtenerPorPeriodo(Int32.Parse(PeriodosNuevosDDL.SelectedValue));

                if (partidas.Count > 0)
                {
                    foreach (Partida partida in partidas)
                    {
                        if (partida.partidaPadre != null)
                        {
                            ListItem itemLB = new ListItem(partida.numeroPartida + " : " + partida.descripcionPartida, partida.idPartida.ToString());
                            PartidasNuevasLB.Items.Add(itemLB);
                        }
                    }
                }
            }
        }

        #endregion

        #region eventos click

        protected void AgregarPartida_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/Partidas/NuevaPartida.aspx");
            Response.Redirect(url);
        }

        protected void EditarPartida_Click(object sender, EventArgs e)
        {
            int[] indices = PartidasActualesLB.GetSelectedIndices();
            if (indices.Length == 1)
            {
                Partida partida = this.partidaServicios.ObtenerPorId(Int32.Parse(PartidasActualesLB.SelectedValue));
                Session["partidaEditar"] = partida;

                String url = Page.ResolveUrl("~/Catalogos/Partidas/EditarPartida.aspx");
                Response.Redirect(url);
            }
        }

        protected void EliminarPartida_Click(object sender, EventArgs e)
        {
            int[] indices = PartidasActualesLB.GetSelectedIndices();
            if (indices.Length == 1)
            {
                Partida partida = this.partidaServicios.ObtenerPorId(Int32.Parse(PartidasActualesLB.SelectedValue));
                Session["partidaEliminar"] = partida;

                String url = Page.ResolveUrl("~/Catalogos/Partidas/EliminarPartida.aspx");
                Response.Redirect(url);
            }
        }

        protected void PasarPartidasBtn_Click(object sender, EventArgs e)
        {
            if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
            {
                if (!PeriodosNuevosDDL.SelectedValue.Trim().Equals(""))
                {
                    foreach (ListItem partida in PartidasActualesLB.Items)
                    {
                        if (partida.Selected)
                        {
                            PartidasNuevasLB.Items.Add(partida);
                        }
                    }
                }
                else
                {
                    Toastr("error", "Debe seleccionar el periodo al que desea pasar las partidas");
                }

                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
            }
        }

        protected void DevolverPartidasBtn_Click(object sender, EventArgs e)
        {
            if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
            {
                ListBox partidasNuevas = new ListBox();
                foreach (ListItem partida in PartidasNuevasLB.Items)
                {
                    partidasNuevas.Items.Add(partida);
                }

                foreach (ListItem partida in partidasNuevas.Items)
                {
                    if (partida.Selected)
                    {
                        PartidasNuevasLB.Items.Remove(partida);
                    }
                }

                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
            }
        }

        protected void GuardarPartidas_Click(object sender, EventArgs e)
        {
            if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
            {
                if (PartidasNuevasLB.Items.Count > 0)
                {
                    LinkedList<int> partidasId = new LinkedList<int>();

                    foreach (ListItem idPartida in PartidasNuevasLB.Items)
                    {
                        partidasId.AddLast(Int32.Parse(idPartida.Value));
                    }

                    bool guardado = this.partidaServicios.Guardar(partidasId, Int32.Parse(PeriodosNuevosDDL.SelectedValue));

                    if (guardado)
                    {
                        Toastr("success", "Se han guardado los cambios con éxito!");
                    }
                    else
                    {
                        Toastr("error", "Error al guardar los proyectos");
                    }
                }

                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
            }
        }

        #endregion

        #region eventos onchanged

        protected void Periodos_OnChanged(object sender, EventArgs e)
        {
            CargarPeriodosNuevos();
            CargarPartidasActuales();
            CargarPartidasNuevas();
        }

        protected void PeriodosNuevos_OnChanged(object sender, EventArgs e)
        {
            CargarPartidasNuevas();
        }

        #endregion

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
    }
}