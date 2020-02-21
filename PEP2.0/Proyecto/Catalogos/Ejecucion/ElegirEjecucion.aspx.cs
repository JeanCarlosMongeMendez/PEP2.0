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
    public partial class ElegirEjecucion : System.Web.UI.Page
    {
        readonly PagedDataSource pgsource = new PagedDataSource();
        ProyectoServicios proyectoServicios;
        PeriodoServicios periodoServicios;
        EjecucionServicios ejecucionServicios;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.proyectoServicios = new ProyectoServicios();
            this.periodoServicios = new PeriodoServicios();
            this.ejecucionServicios = new EjecucionServicios();
            if (!IsPostBack)
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                PeriodosDDL.Items.Clear();
                ProyectosDDL.Items.Clear();
                CargarPeriodos();
                //DDLTipoTramite.Items.Clear();
                //ddlPartida.Items.Clear();
            }

            

        }
        /// <summary>
        /// kevin Picado Quesada
        /// Método utilizado para la accion en la selección del droplist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Periodos_OnChanged(object sender, EventArgs e)
        {
            //Session["partidasPorUnidadesProyectoPeriodo"] = null;
            //Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
             CargarProyectos();
            
            //reiniciarTablaUnidad();
            //DDLTipoTramite.Items.Clear();
            //descripcionOtroTipoTramite.Visible = false;
        }
        /// <summary>
        /// Kevin Picado
        /// 21/2/2020
        /// Efecto: cambia los datos de las unidades segun el periodo seleccionado
        /// Requiere: cambiar periodo
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                MostrarDatosTablaUnidad();
            }

        }


        /// <summary>
        /// Kevin Picado 
        /// 09/oct/2019
        /// Efecto: cambia los datos del proyrcto segun el periodo seleccionado
        /// Requiere: seleccionar un periodo
        /// Modifica: DropDownList de proyectos
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CargarPeriodos()
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            PeriodosDDL.Items.Clear();
            periodos = this.periodoServicios.ObtenerTodos();
            int anoHabilitado = 0;
            //Session["partidasPorUnidadesProyectoPeriodo"] = null;
            //Session["partidasSeleccionadasPorUnidadesProyectoPeriodo"] = null;
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
        protected void Proyectos_OnChanged(object sender, EventArgs e)
        {
            
            MostrarDatosTablaUnidad();
        }

        /// <summary>
        /// Kevin Picado
        /// 09/oct/2019
        /// Efecto: Muestra las unidades seleccionadas 
        /// Requiere: Seleccionar la unidad para cargarla en la lista
        /// Modifica: tabla Unidades
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MostrarDatosTablaUnidad()
        {

            string a = ProyectosDDL.SelectedValue;
            var dt = ejecucionServicios.ConsultarEjecucion(PeriodosDDL.SelectedValue, ProyectosDDL.SelectedValue);
                pgsource.DataSource = dt;
                pgsource.AllowPaging = true;
                //numero de items que se muestran en el Repeater
                //pgsource.PageSize = elmentosMostrar;
                //pgsource.CurrentPageIndex = paginaActual2;
                //mantiene el total de paginas en View State
                //ViewState["TotalPaginas2"] = pgsource.PageCount;
                //Ejemplo: "Página 1 al 10"
                //lblpagina2.Text = "Página " + (paginaActual2 + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
                //Habilitar los botones primero, último, anterior y siguiente
                //lbAnterior2.Enabled = !pgsource.IsFirstPage;
                //lbSiguiente2.Enabled = !pgsource.IsLastPage;
                //lbPrimero2.Enabled = !pgsource.IsFirstPage;
                //lbUltimo2.Enabled = !pgsource.IsLastPage;

                rpUnidadSelecionadas.DataSource = pgsource;
                rpUnidadSelecionadas.DataBind();

                //metodo que realiza la paginacion
                //Paginacion2();

           
        }

    }
}