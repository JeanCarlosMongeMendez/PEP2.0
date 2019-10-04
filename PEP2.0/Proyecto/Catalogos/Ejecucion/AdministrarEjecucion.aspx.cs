using Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proyecto.Catalogos.Ejecucion
{


  
    public partial class AdministrarEjecucion : System.Web.UI.Page
    {
        readonly PagedDataSource pgsource = new PagedDataSource();
        PeriodoServicios periodoServicios;
        ProyectoServicios proyectoServicios;
        UnidadServicios unidadServicios;
        private int elmentosMostrar = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.periodoServicios = new PeriodoServicios();
            this.proyectoServicios = new ProyectoServicios();
            this.unidadServicios = new UnidadServicios();
            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());

               
                PeriodosDDL.Items.Clear();
                ProyectosDDL.Items.Clear();
                CargarPeriodos();


            }
           
           
            
        }

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

                CargarProyectos();



            }

        }
      
        private void CargarProyectos()
        {
            ProyectosDDL.Items.Clear();

            if (!PeriodosDDL.SelectedValue.Equals(""))
            {
                LinkedList<Proyectos> proyectos = new LinkedList<Proyectos>();
                proyectos = this.proyectoServicios.ObtenerPorPeriodo(Int32.Parse(PeriodosDDL.SelectedValue));

                if (proyectos.Count > 0)
                {
                    foreach (Proyectos proyecto in proyectos)
                    {
                        ListItem itemLB = new ListItem(proyecto.nombreProyecto, proyecto.idProyecto.ToString());
                        Session["proyecto"] = proyecto.idProyecto;
                        ProyectosDDL.Items.Add(itemLB);
                    }

                    //CargarUnidades();
                }
            }
        }
        private void MostrarDatosTabla()
        {

            if (ProyectosDDL.Items.Count > 0)
            {
               
                LinkedList<Entidades.Unidad> listUnidades = unidadServicios.ObtenerPorProyecto(Int32.Parse(ProyectosDDL.SelectedValue));
              
                var dt = listUnidades;
                pgsource.DataSource = dt;
                pgsource.AllowPaging = true;
                //numero de items que se muestran en el Repeater
                pgsource.PageSize = elmentosMostrar;
                //pgsource.CurrentPageIndex = paginaActual;
                //mantiene el total de paginas en View State
                //ViewState["TotalPaginas"] = pgsource.PageCount;
                //Ejemplo: "Página 1 al 10"
                //lblpagina.Text = "Página " + (paginaActual + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
                //Habilitar los botones primero, último, anterior y siguiente
                //lbAnterior.Enabled = !pgsource.IsFirstPage;
                //lbSiguiente.Enabled = !pgsource.IsLastPage;
                //lbPrimero.Enabled = !pgsource.IsFirstPage;
                //lbUltimo.Enabled = !pgsource.IsLastPage;

                rpEjecucion.DataSource = pgsource;
                rpEjecucion.DataBind();

                //metodo que realiza la paginacion
                //Paginacion();

            }
            else
            {
                //LinkedList<Entidades.PresupuestoEgreso> listaPresupuestosEgresos = new LinkedList<Entidades.PresupuestoEgreso>();
                //rpPartida.DataSource = listaPresupuestosEgresos;
                //rpPartida.DataBind();
            }
        }
        protected void Periodos_OnChanged(object sender, EventArgs e)
        {

            CargarProyectos();
            //CargarUnidades();
            //MostrarDatosTabla();
        }
        protected void Proyectos_OnChanged(object sender, EventArgs e)
        {
            //CargarUnidades();
        }

        protected void ButtonAsociar_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "#modalElegirUnidad", "$('body').removeClass('modal-open');$('.modal-backdrop').remove();$('#modalElegirUnidad').hide();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "activarModalElegirUnidad();", true);

        }
      
    }
}