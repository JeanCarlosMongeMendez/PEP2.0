using Entidades;
using Microsoft.Reporting.WebForms;
using PEP;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proyecto.Reportes
{
    public partial class ReporteDetalleEjecuciones : System.Web.UI.Page
    {
        #region variables globales
        PeriodoServicios periodoServicios = new PeriodoServicios();
        ProyectoServicios proyectoServicios = new ProyectoServicios();
        Reporte_DetalleEjecucionesServicios reporte_DetalleEjecucionesServicios = new Reporte_DetalleEjecucionesServicios();
        UnidadServicios unidadServicios = new UnidadServicios();
        #endregion
        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);

            if (!IsPostBack)
            {
                llenarDdlPeriodos();

                Periodo periodo = new Periodo();
                periodo.anoPeriodo = Convert.ToInt32(ddlPeriodos.SelectedValue);

                Proyectos proyecto = new Proyectos();
                proyecto.idProyecto = Convert.ToInt32(ddlProyectos.SelectedValue);

                cargarUnidades();

                Unidad unidad = new Unidad();
                unidad.idUnidad = Convert.ToInt32(ddlUnidades.SelectedValue);

                Session["listaReporteEgresos"] = reporte_DetalleEjecucionesServicios.getReporteEgresosPorUnidades(proyecto.idProyecto,periodo.anoPeriodo,unidad.idUnidad);

                cargarDatosReporte();
            }
        }
        #endregion
        #region logica
        /// <summary>
        /// Leonardo Carrion
        /// 06/may/2021
        /// Efecto: llean el DropDownList con los periodos que se encuentran en la base de datos
        /// Requiere: - 
        /// Modifica: DropDownList
        /// Devuelve: -
        /// </summary>
        public void llenarDdlPeriodos()
        {
            LinkedList<Periodo> periodos = new LinkedList<Periodo>();
            ddlPeriodos.Items.Clear();
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
                    ddlPeriodos.Items.Add(itemPeriodo);
                }

                if (anoHabilitado != 0)
                {
                    ddlPeriodos.Items.FindByValue(anoHabilitado.ToString()).Selected = true;
                }
            }

            CargarProyectos();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 06/may/2021
        /// Efecto: llean el DropDownList con los proyectos que se encuentran en la base de datos segun el periodo seleccionado
        /// Requiere: Periodo
        /// Modifica: DropDownList y datos del reporte
        /// Devuelve: -
        /// </summary>
        private void CargarProyectos()
        {
            ddlProyectos.Items.Clear();

            if (!ddlPeriodos.SelectedValue.Equals(""))
            {
                LinkedList<Proyectos> proyectos = new LinkedList<Proyectos>();
                proyectos = proyectoServicios.ObtenerPorPeriodo(Int32.Parse(ddlPeriodos.SelectedValue));

                if (proyectos.Count > 0)
                {
                    foreach (Proyectos proyectoTemp in proyectos)
                    {
                        ListItem itemLB = new ListItem(proyectoTemp.nombreProyecto, proyectoTemp.idProyecto.ToString());
                        ddlProyectos.Items.Add(itemLB);
                    }

                    cargarUnidades();
                }
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 06/may/2021
        /// Efecto: carga el DropDownList de unidades
        /// Requiere: -
        /// Modifica: DropDownList y datos del reporte
        /// Devuelve: -
        /// </summary>
        private void cargarUnidades()
        {
            ddlUnidades.Items.Clear();

            List<Unidad> listaUnidades = unidadServicios.ObtenerPorProyecto(Convert.ToInt32(ddlProyectos.SelectedValue));

            foreach (Unidad unidad in listaUnidades)
            {
                ListItem unidades2 = new ListItem(unidad.nombreUnidad, unidad.idUnidad.ToString());
                ddlUnidades.Items.Add(unidades2);
            }

            cargarDatosReporte();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 06/may/2021
        /// Efecto: cargar el reporte con los datos filtrados
        /// Requiere: -
        /// Modifica: datasource del reporte
        /// Devuelve: -
        /// </summary>
        public void cargarDatosReporte()
        {
            Periodo periodo = new Periodo();
            periodo.anoPeriodo = Convert.ToInt32(ddlPeriodos.SelectedValue);

            Proyectos proyecto = new Proyectos();
            proyecto.idProyecto = Convert.ToInt32(ddlProyectos.SelectedValue);

            Unidad unidad = new Unidad();
            unidad.idUnidad = Convert.ToInt32(ddlUnidades.SelectedValue);

            Session["listaReporteEgresos"] = reporte_DetalleEjecucionesServicios.getReporteEgresosPorUnidades(proyecto.idProyecto, periodo.anoPeriodo, unidad.idUnidad);

            ReportViewer1.Reset();
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReporteDetalleEjecuciones.rdlc");
            ReportDataSource reportDataSource = null;
            List<ReporteDetalleEjecucion> listaReporteDuracion = (List<ReporteDetalleEjecucion>)Session["listaReporteEgresos"];

            reportDataSource = new ReportDataSource("DataSet1", listaReporteDuracion);
            if (reportDataSource.Equals(null) == false)
                ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
            ReportViewer1.DataBind();
        }
        #endregion
        #region eventos

        /// <summary>
        /// Leonardo Carrion
        /// 06/may/2021
        /// Efecto: cambia los datos de la tabla segun el periodo seleccionado
        /// Requiere: cambiar periodo
        /// Modifica: datos de la tabla
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarProyectos();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 06/may/2021
        /// Efecto: cargar la tabla de plan estrategico y muestra los montos de las partidas
        /// Requiere: cambiar la unidad
        /// Modifica: plan estrategico y montos de las partidas
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarDatosReporte();
        }

        /// <summary>
        /// Leonardo Carrion
        /// 06/may/2021
        /// Efecto: actualiza los datos del reporte cuando se cambia de unidad
        /// Requiere: cambiar de unidad
        /// Modifica: datos del reporte
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUnidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarDatosReporte();
        }
        #endregion
    }
}