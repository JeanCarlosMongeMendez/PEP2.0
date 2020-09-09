<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarDistribucionUcr.aspx.cs" Inherits="Proyecto.Planilla.AdministrarDistribucionUcr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>

    <asp:UpdatePanel ID="pnlUpdate" runat="server">
        <ContentTemplate>

            <%--titulo--%>
            <div class="row" style="text-align: center">
                <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                    <asp:Label ID="label" runat="server" Text="Distribución planilla UCR" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                    <br />
                </div>


                <%--periodo--%>
                <div class="col-md-3 col-xs-12 col-sm-12">
                    <asp:Label runat="server" Text="Período" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>

                <div class="col-md-9 col-xs-12 col-sm-12" style="text-align: left">
                    <asp:DropDownList ID="ddlPeriodo" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <br />
                </div>


                <%--proyecto--%>
                <div class="col-md-3 col-xs-12 col-sm-12">
                    <asp:Label runat="server" Text="Proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>

                <div class="col-md-9 col-xs-12 col-sm-12" style="text-align: left">
                    <asp:DropDownList ID="ddlProyecto" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true"></asp:DropDownList>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <br />
                </div>

                <%--tabla Funcionarios--%>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <%-- tabla--%>

                        <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                            <table class="table table-bordered">
                                <thead style="text-align: center">
                                    <tr style="text-align: center" class="btn-primary">
                                        <th></th>
                                        <th>Nombre</th>
                                    </tr>
                                </thead>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary" OnClick="btnFiltrar_Click"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton></td>
                                    <td>
                                     
                                           
                                            <asp:TextBox ID="txtBuscarNombre" runat="server" CssClass="form-control chat-input" placeholder="filtro nombre" onkeypress="enter3_click()"></asp:TextBox>
                                     
                                    </td>
                                </tr>
                                <asp:Repeater ID="rpFuncionarios" runat="server">
                                    <HeaderTemplate>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr style="text-align: center">
                                            <td>
                                                <asp:LinkButton ID="btnSelccionar" OnClick="btnSelccionarFuncionario_Click" runat="server" ToolTip="Seleccionar" CommandArgument='<%# Eval("idFuncionario") %>'><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                                            </td>
                                            <td style='<%# Convert.ToDouble(Eval("porcentajeAsignado"))==100? "background-color:#3eb13e": "background-color:white" %>'>
                                                <%# Eval("nombreFuncionario") %>
                                            </td>
                                        </tr>

                                    </ItemTemplate>

                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </table>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center; align-content: center; overflow-y: auto;">
                            <center>
                    <table class="table" style="max-width: 664px;">
                        <tr style="padding: 1px !important">
                            <td style="padding: 1px !important">
                                <asp:LinkButton ID="lbPrimero" runat="server" CssClass="btn btn-primary" OnClick="lbPrimero_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                            </td>
                            <td style="padding: 1px !important">
                                <asp:LinkButton ID="lbAnterior" runat="server" CssClass="btn btn-default" OnClick="lbAnterior_Click"><span class="glyphicon glyphicon-backward"></asp:LinkButton>
                            </td>
                            <td style="padding: 1px !important">
                                <asp:DataList ID="rptPaginacion" runat="server"
                                    OnItemCommand="rptPaginacion_ItemCommand"
                                    OnItemDataBound="rptPaginacion_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaginacion" runat="server" CssClass="btn btn-default"
                                            CommandArgument='<%# Eval("IndexPagina") %>' CommandName="nuevaPagina"
                                            Text='<%# Eval("PaginaText") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td style="padding: 1px !important">
                                <asp:LinkButton ID="lbSiguiente" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente_Click"><span class="glyphicon glyphicon-forward"></asp:LinkButton>
                            </td>
                            <td style="padding: 1px !important">
                                <asp:LinkButton ID="lbUltimo" CssClass="btn btn-primary" runat="server" OnClick="lbUltimo_Click"><span class="glyphicon glyphicon-fast-forward"></asp:LinkButton>
                            </td>
                            <td style="padding: 1px !important">
                                <asp:Label ID="lblpagina" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <%--boton regresar--%>

                <div class="col-md-4 col-xs-12 col-sm-12 col-md-offset-8">
                    <asp:Button ID="btnIngresarPresupuestoEgresos" runat="server" Text="Ingresar datos al presupuesto de egresos" CssClass="btn btn-default boton-main" OnClick="btnIngresarPresupuestoEgresos_Click" />
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CssClass="btn btn-danger boton-eliminar" OnClick="btnRegresar_Click" />
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <br />
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <%--modal distribuir jornada--%>
    <div id="modalDistribuirJornada" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg" style="min-width: 95%; margin: 2%">
            <%--modal--%>
            <div class="modal-content">

                <%--header--%>
                <div class="modal-header" style="background-color: #005da4; color: white">
                    <button type="button" class="close" data-dismiss="modal" style="color: white">&times;</button>
                    <h4 class="modal-title" runat="server">Distribuir Jornada</h4>
                </div>

                <%--body--%>
                <div class="modal-body">

                    <%--datos generales--%>
                    <div class="row">
                        <%--periodo--%>
                        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                            <asp:Label runat="server" Text="Periodo : " Font-Size="Large" ForeColor="Black"></asp:Label>
                            <asp:Label ID="lblPeriodo" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>


                        <%--proyecto--%>
                        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                            <asp:Label runat="server" Text="Proyecto : " Font-Size="Large" ForeColor="Black"></asp:Label>
                            <asp:Label ID="lblProyecto" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <%--proyecto--%>
                        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                            <asp:Label runat="server" Text="Funcionario : " Font-Size="Large" ForeColor="Black"></asp:Label>
                            <asp:Label ID="lblFuncionario" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <%--jornada--%>
                        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                            <asp:Label runat="server" Text="Jornada : " Font-Size="Large" ForeColor="Black"></asp:Label>
                            <asp:Label ID="lblJornada" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <%--progress bar--%>
                        <div id="progressBar" class="progress col-md-10 col-md-offset-1" style="padding-left: 0px; padding-right: 0px;">
                            <div id="progressBarFree" class="progress-bar progress-bar-success" role="progressbar" style="width: 0%">
                            </div>
                        </div>
                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>
                    </div>

                    <%--unidades--%>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <!-- ------------------------ tabla unidades proyecto --------------------------- -->
                            <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                <table id="tblUnidadesProyecto" class="table table-bordered">
                                    <thead>
                                        <tr style="text-align: center" class="btn-primary">
                                            <th>Nombre Unidad</th>
                                            <th>Coordinador</th>
                                            <th>Jornada en unidad</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <asp:Repeater ID="rpUnidProyecto" runat="server">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="text-align: center">
                                                <td>
                                                    <%# Eval("nombreUnidad") %>
                                                </td>
                                                <td>
                                                    <%# Eval("coordinador") %>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btnSelccionarUnidad" OnClick="btnSelccionarUnidad_Click" runat="server" ToolTip="Seleccionar" CommandArgument='<%# Eval("idUnidad") %>'><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnEliminarUnidad" OnClick="btnEliminarUnidad_Click" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idUnidad") %>'><span class="btn glyphicon glyphicon-trash"></span></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                            <%--paginacion de unidades--%>
                            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 mt-2">
                                <hr />
                            </div>
                            <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                <center>
                    <table class="table" style="max-width:664px;">
                        <tr style="padding:1px !important">
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbPrimeroUnidad" runat="server" CssClass="btn btn-primary" OnClick="lbPrimeroUnidad_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbAnteriorUnidad" runat="server" CssClass="btn btn-default" OnClick="lbAnteriorUnidad_Click"><span class="glyphicon glyphicon-backward"></span></asp:LinkButton>
                            </td>
                            <td style="padding:1px !important">
                                <asp:DataList ID="rptPaginacionUnidad" runat="server"
                                    OnItemCommand="rptPaginacionUnidad_ItemCommand"
                                    OnItemDataBound="rptPaginacionUnidad_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaginacionUnidad" runat="server" CssClass="btn btn-default"
                                            CommandArgument='<%# Eval("IndexPagina") %>' CommandName="nuevaPagina"
                                            Text='<%# Eval("PaginaText") %>' ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbSiguienteUnidad" CssClass="btn btn-default" runat="server" OnClick="lbSiguienteUnidad_Click"><span class="glyphicon glyphicon-forward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbUltimoUnidad" CssClass="btn btn-primary" runat="server" OnClick="lbUltimoUnidad_Click"><span class="glyphicon glyphicon-fast-forward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:Label ID="lblpaginaUnidad" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                            </div>
                            <!-- ---------------------- FIN tabla unidades proyecto  ------------------------- -->
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%-- fin unidades--%>
                </div>

                <%--footer--%>
                <div class="modal-footer" style="text-align: center">

                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>



    <%--modal asignar jornada--%>
    <div id="modalAsignarJornada" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg" style="min-width: 30%; margin: 20%">
            <%--modal--%>
            <div class="modal-content">

                <%--header--%>
                <div class="modal-header" style="background-color: #005da4; color: white">
                    <button type="button" class="close" data-dismiss="modal" style="color: white">&times;</button>
                    <h4 class="modal-title" runat="server">Asignar distribución</h4>
                </div>

                <%--body--%>
                <div class="modal-body">

                    <%--datos generales--%>
                    <asp:HiddenField runat="server" ID="IdUnidadSeleccionada" />
                    <div>

                        <%--Unidad--%>
                        <div class="row">
                            <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                                <asp:Label runat="server" Text="Unidad : " Font-Size="Large" ForeColor="Black"></asp:Label>
                                <asp:Label ID="lblUnidad" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>
                        </div>

                        <%--jornada--%>
                        <div class="row">
                            <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                                <asp:Label runat="server" Text="Porcentaje de tiempo en la unidad : " Font-Size="Large" ForeColor="Black"></asp:Label>
                                <asp:DropDownList ID="ddlAsignarJornada" class="btn btn-default dropdown-toggle" runat="server"></asp:DropDownList>
                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>
                        </div>

                    </div>
                </div>

                <%--footer--%>
                <div class="modal-footer" style="text-align: center">
                    <asp:LinkButton ID="btnAsignarJornada" class="btn btn-default" OnClick="btnAsignarJornada_Click" runat="server">Asignar</asp:LinkButton>
                    <asp:LinkButton class="btn btn-default" OnClick="btnCerrarModalAsignarJornada_Click" runat="server">Cerrar</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <!-- update progress-->
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlUpdate">
        <ProgressTemplate>
            <div class="alert alert-info" role="alert">
                <h6>
                    <p style="text-align: center"><b>Procesando Datos, Espere por favor...
                        <br />
                    </b></p>
                </h6>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <script>
        function activarModalDistribuirJornada() {
            $('#modalDistribuirJornada').modal({
                backdrop: true
            });
            $('#modalDistribuirJornada').modal('show');
        };

        function activarModalAsignarJornada() {
            $('#modalDistribuirJornada').modal({
                backdrop: "static"
            });
            $('#modalAsignarJornada').modal({
                focus: true
            });
            $('#modalAsignarJornada').modal('show');
        };

        function cerrarModalAsignarJornada() {
            $('#modalAsignarJornada').modal('hide');
        };
         function enter3_click() {
            if (window.event.keyCode == 13) {
              document.getElementById('<%=btnFiltrar.ClientID%>').focus();
                document.getElementById('<%=btnFiltrar.ClientID%>').click();
            }
        }

        function agregarDistribucion(josnListaUnidades) {
            var listaUnidadesAsignadas = JSON.parse(josnListaUnidades);
            console.log(listaUnidadesAsignadas);
            for (var unidad in listaUnidadesAsignadas) {
                var porcentajeDistribucion = listaUnidadesAsignadas['' + unidad + ''].jornadaAsignada;
                var div = document.createElement('div');
                div.id = 'progress' + unidad;
                div.className = 'progress-bar';
                div.role = "progressbar";
                div.innerText = listaUnidadesAsignadas['' + unidad + ''].descUnidad + " | " + porcentajeDistribucion + "%";
                div.style.width = porcentajeDistribucion + '%';
                div.style.borderRight = 'solid 5px #FFF';
                document.getElementById('progressBar').appendChild(div);
            }
            $('#modalDistribuirJornada').modal('show');
        }


    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
