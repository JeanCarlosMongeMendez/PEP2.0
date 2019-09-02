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

namespace PEP.Catalogos.Periodos
{
    public partial class AdministrarPeriodo : System.Web.UI.Page
    {
        #region variables globales
        private PeriodoServicios periodoServicios;
        private ProyectoServicios proyectoServicios;
        private UnidadServicios unidadServicios;
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);

            this.periodoServicios = new PeriodoServicios();
            this.proyectoServicios = new ProyectoServicios();
            this.unidadServicios = new UnidadServicios();

            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());

                CargarPeriodos();
                divUnidades.Visible = false;
            }
            else
            {
                //AgregarPeriodoBtn.Click += new EventHandler((snd, evt) => AgregarPeriodo_Click(snd, evt));
                //AgregarProyectoBtn.Click += new EventHandler((snd, evt) => AgregarProyecto_Click(snd, evt));
                EstablecerPeriodoActualBtn.Click += new EventHandler((snd, evt) => EstablecerPeriodoActual_Click(snd, evt));
                PasarProyectosBtn.Click += new EventHandler((snd, evt) => PasarProyectosBtn_Click(snd, evt));
                DevolverProyectosBtn.Click += new EventHandler((snd, evt) => DevolverProyectosBtn_Click(snd, evt));
                GuardarProyectosBtn.Click += new EventHandler((snd, evt) => GuardarProyectos_Click(snd, evt));
                //AgregarUnidadBtn.Click += new EventHandler((snd, evt) => AgregarUnidad_Click(snd, evt));
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
                CargarProyectosActuales();
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

        private void CargarProyectosActuales()
        {
            //ProyectosActualesDDL.Items.Clear();
            ProyectosActualesLB.Items.Clear();
            ProyectosNuevosLB.Items.Clear();

            if (!PeriodosDDL.SelectedValue.Equals(""))
            {
                AnoActual.Text = PeriodosDDL.SelectedValue;
                Session["periodo"] = PeriodosDDL.SelectedValue;

                LinkedList<Proyectos> proyectos = new LinkedList<Proyectos>();
                proyectos = this.proyectoServicios.ObtenerPorPeriodo(Int32.Parse(PeriodosDDL.SelectedValue));

                if (proyectos.Count > 0)
                {
                    foreach (Proyectos proyecto in proyectos)
                    {
                        ListItem itemLB = new ListItem(proyecto.nombreProyecto, proyecto.idProyecto.ToString());
                        ProyectosActualesLB.Items.Add(itemLB);

                        //if (proyecto.esUCR)
                        //{
                        //    ListItem itemDDL = new ListItem(proyecto.nombreProyecto, proyecto.idProyecto.ToString());
                        //    ProyectosActualesDDL.Items.Add(itemDDL);
                        //}
                    }

                    CargarUnidadesActuales2();
                }
            }
        }

        private void CargarProyectosNuevos()
        {
            ProyectosNuevosLB.Items.Clear();

            if (!PeriodosNuevosDDL.SelectedValue.Equals(""))
            {
                LinkedList<Proyectos> proyectos = new LinkedList<Proyectos>();
                proyectos = this.proyectoServicios.ObtenerPorPeriodo(Int32.Parse(PeriodosNuevosDDL.SelectedValue));
                
                if (proyectos.Count > 0)
                {
                    foreach (Proyectos proyecto in proyectos)
                    {
                        ListItem itemLB = new ListItem(proyecto.nombreProyecto, proyecto.idProyecto.ToString());
                        ProyectosNuevosLB.Items.Add(itemLB);

                        ListItem itemDDL = new ListItem(proyecto.nombreProyecto, proyecto.idProyecto.ToString());
                    }
                }
            }
        }

        //private void CargarUnidadesActuales()
        //{
        //    UnidadesActualesLB.Items.Clear();

        //    if (!ProyectosActualesDDL.SelectedValue.Equals(""))
        //    {
        //        LinkedList<Unidad> unidades = new LinkedList<Unidad>();
        //        unidades = this.unidadServicios.ObtenerPorProyecto(Int32.Parse(ProyectosActualesDDL.SelectedValue));
        //        Session["proyecto"] = ProyectosActualesDDL.SelectedValue;

        //        foreach (Unidad unidad in unidades)
        //        {
        //            ListItem itemLB = new ListItem(unidad.nombreUnidad, unidad.idUnidad.ToString());
        //            UnidadesActualesLB.Items.Add(itemLB);
        //        }
        //    }
        //}


        private void CargarUnidadesActuales2()
        {
            UnidadesActualesLB.Items.Clear();

            int[] indices = ProyectosActualesLB.GetSelectedIndices();
            if (indices.Length == 1)
            //if (!ProyectosActualesLB.SelectedValue.Equals(""))
            {
                LinkedList<Unidad> unidades = new LinkedList<Unidad>();
                Proyectos proyecto = this.proyectoServicios.ObtenerPorId(Int32.Parse(ProyectosActualesLB.SelectedValue));
                unidades = this.unidadServicios.ObtenerPorProyecto(proyecto.idProyecto);

                Session["proyecto"] = proyecto.idProyecto;

                if (proyecto.esUCR)
                {
                    foreach (Unidad unidad in unidades)
                    {
                        ListItem itemLB = new ListItem(unidad.nombreUnidad, unidad.idUnidad.ToString());
                        UnidadesActualesLB.Items.Add(itemLB);
                    }
                    divUnidades.Visible = true;
                }
                else
                {
                    divUnidades.Visible = false;
                }
            }
            else
            {
                divUnidades.Visible = false;
            }
        }

        #endregion

        #region eventos click

        protected void AgregarPeriodo_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/Periodos/NuevoPeriodo.aspx");
            Response.Redirect(url);
        }

        protected void EliminarPeriodo_Click(object sender, EventArgs e)
        {
            if (!PeriodosDDL.SelectedValue.Equals(""))
            {
                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Int32.Parse(PeriodosDDL.SelectedValue);
                Session["periodoEliminar"] = periodo;

                String url = Page.ResolveUrl("~/Catalogos/Periodos/EliminarPeriodo.aspx");
                Response.Redirect(url);
            }
        }

        protected void AgregarProyecto_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/Proyecto/NuevoProyecto.aspx");
            Response.Redirect(url);
        }

        protected void EditarProyecto_Click(object sender, EventArgs e)
        {
            int[] indices = ProyectosActualesLB.GetSelectedIndices();
            if (indices.Length == 1)
            {
                Proyectos proyecto = this.proyectoServicios.ObtenerPorId(Int32.Parse(ProyectosActualesLB.SelectedValue));
                Session["proyectoEditar"] = proyecto;

                String url = Page.ResolveUrl("~/Catalogos/Proyecto/EditarProyecto.aspx");
                Response.Redirect(url);
            }
        }

        protected void EliminarProyecto_Click(object sender, EventArgs e)
        {
            int[] indices = ProyectosActualesLB.GetSelectedIndices();
            if (indices.Length == 1)
            {
                Proyectos proyecto = this.proyectoServicios.ObtenerPorId(Int32.Parse(ProyectosActualesLB.SelectedValue));
                Session["proyectoEliminar"] = proyecto;

                String url = Page.ResolveUrl("~/Catalogos/Proyecto/EliminarProyecto.aspx");
                Response.Redirect(url);
            }
        }

        protected void AgregarUnidad_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/Unidades/NuevaUnidad.aspx");
            Response.Redirect(url);
        }

        protected void EditarUnidad_Click(object sender, EventArgs e)
        {
            int[] indices = UnidadesActualesLB.GetSelectedIndices();
            if (indices.Length == 1)
            {
                Unidad unidad = this.unidadServicios.ObtenerPorId(Int32.Parse(UnidadesActualesLB.SelectedValue));
                Session["unidadEditar"] = unidad;

                String url = Page.ResolveUrl("~/Catalogos/Unidades/EditarUnidad.aspx");
                Response.Redirect(url);
            }
        }

        protected void EliminarUnidad_Click(object sender, EventArgs e)
        {
            int[] indices = UnidadesActualesLB.GetSelectedIndices();
            if (indices.Length == 1)
            {
                Unidad unidad = this.unidadServicios.ObtenerPorId(Int32.Parse(UnidadesActualesLB.SelectedValue));
                Session["unidadEliminar"] = unidad;

                String url = Page.ResolveUrl("~/Catalogos/Unidades/EliminarUnidad.aspx");
                Response.Redirect(url);
            }
        }

        protected void EstablecerPeriodoActual_Click(object sender, EventArgs e)
        {
            if (!PeriodosDDL.SelectedValue.Trim().Equals(""))
            {
                bool respuesta = this.periodoServicios.HabilitarPeriodo(Int32.Parse(PeriodosDDL.SelectedValue));

                if (respuesta)
                {
                    Toastr("success", "Periodo establecido con éxito!");
                }
                else
                {
                    Toastr("error", "Error al establecer el proyecto");
                }

                CargarPeriodos();
            }
        }

        protected void PasarProyectosBtn_Click(object sender, EventArgs e)
        {
            if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
            {
                if (!PeriodosNuevosDDL.SelectedValue.Trim().Equals(""))
                {
                    foreach (ListItem proyecto in ProyectosActualesLB.Items)
                    {
                        if (proyecto.Selected)
                        {
                            ProyectosNuevosLB.Items.Add(proyecto);
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

        protected void GuardarProyectos_Click(object sender, EventArgs e)
        {
            if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
            {
                if (ProyectosNuevosLB.Items.Count > 0)
                {
                    LinkedList<int> proyectosId = new LinkedList<int>();

                    foreach (ListItem idProyecto in ProyectosNuevosLB.Items)
                    {
                        proyectosId.AddLast(Int32.Parse(idProyecto.Value));
                    }

                    int respuesta = this.proyectoServicios.Guardar(proyectosId, Int32.Parse(PeriodosNuevosDDL.SelectedValue));

                    if (respuesta > 0)
                    {
                        Toastr("success", "Se han guardado los cambios con éxito!");
                    }
                    else if(respuesta == -1)
                    {
                        Toastr("error", "Uno de los proyectos que desea guardar ya existe en el periodo");
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
            CargarProyectosActuales();
            CargarProyectosNuevos();
            CargarUnidadesActuales2();
        }

        protected void PeriodosNuevos_OnChanged(object sender, EventArgs e)
        {
            CargarProyectosNuevos();
        }

        protected void ProyectosActualesLB_OnChanged(object sender, EventArgs e)
        {
            CargarUnidadesActuales2();
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