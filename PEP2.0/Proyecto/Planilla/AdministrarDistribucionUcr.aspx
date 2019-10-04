<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarDistribucionUcr.aspx.cs" Inherits="Proyecto.Planilla.AdministrarDistribucionUcr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>

    <%--titulo--%>
    <div class="row" style="text-align: center">
        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
            <asp:Label ID="label" runat="server" Text="Distribución planilla UCR" Font-Size="Large" ForeColor="Black"></asp:Label>
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
            <br />
        </div>
    </div>

    <%--periodo--%>
    <div class="row" style="text-align: center">
        <div class="col-md-3 col-xs-12 col-sm-12">
            <asp:Label runat="server" Text="Periodo" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
        </div>

        <div class="col-md-9 col-xs-12 col-sm-12" style="text-align: left">
            <asp:DropDownList ID="ddlPeriodo" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged"></asp:DropDownList>
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12">
            <br />
        </div>
    </div>

    <%--proyecto--%>
    <div class="row" style="text-align: center">
        <div class="col-md-3 col-xs-12 col-sm-12">
            <asp:Label runat="server" Text="Proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
        </div>

        <div class="col-md-9 col-xs-12 col-sm-12" style="text-align: left">
            <asp:DropDownList ID="ddlProyecto" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true"></asp:DropDownList>
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12">
            <br />
        </div>
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
                            <div class="input-group">
                                <span class="input-group-addon"><i class="fa fa-search"></i></span>
                                <asp:TextBox ID="txtBuscarNombre" runat="server" CssClass="form-control chat-input" placeholder="filtro nombre"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <asp:Repeater ID="rpFuncionarios" runat="server">
                        <HeaderTemplate>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr style="text-align: center">
                                <td>
                                    <asp:LinkButton ID="btnSelccionar" OnClick="btnSelccionar_Click" runat="server" ToolTip="Seleccionar" CommandArgument='<%# Eval("idFuncionario") %>'><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                                </td>
                                <td>
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
    <div class="col-md-2 col-xs-2 col-sm-2 col-md-offset-10 col-xs-offset-10 col-sm-offset-10">
        <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CssClass="btn btn-danger boton-nuevo" OnClick="btnRegresar_Click" />
    </div>

    <div class="col-md-12 col-xs-12 col-sm-12">
        <br />
    </div>

    <%--modal distribuir jornada--%>
    <div id="modalDistribuirJornada" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg" style="min-width: 95%; margin: 2%">
            <asp:HiddenField ID="hdIdFuncionario" runat="server" />
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
                    <div>
                        <%--periodo--%>
                        <div class="row">
                            <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                                <asp:Label ID="label1" runat="server" Text="Periodo : " Font-Size="Large" ForeColor="Black"></asp:Label>
                                <asp:Label ID="lblPeriodo" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>
                        </div>

                        <%--proyecto--%>
                        <div class="row">
                            <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                                <asp:Label runat="server" Text="Proyecto : " Font-Size="Large" ForeColor="Black"></asp:Label>
                                <asp:Label ID="lblProyecto" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>
                        </div>

                        <%--proyecto--%>
                        <div class="row">
                            <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                                <asp:Label runat="server" Text="Funcionario : " Font-Size="Large" ForeColor="Black"></asp:Label>
                                <asp:Label ID="lblFuncionario" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>
                        </div>

                        <%--jornada--%>
                        <div class="row">
                            <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                                <asp:Label runat="server" Text="Jornada : " Font-Size="Large" ForeColor="Black"></asp:Label>
                                <asp:Label ID="lblJornada" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>
                        </div>

                        <%--progress bar--%>
                        <div class="row">
                            <div id="progressBar" class="progress col-md-10 col-md-offset-1" style="padding-left: 0px;">
                                <div id="progressBarFree" class="progress-bar progress-bar-success" role="progressbar" style="width: 0%">
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>
                    </div>

                    <%--unidades--%>
                    <!-- ------------------------ tabla unidades proyecto --------------------------- -->
                    <div class="row">
                        <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                            <table id="tblUnidadesProyecto" class="table table-bordered">
                                <thead>
                                    <tr style="text-align: center" class="btn-primary">
                                        <th>Nombre Unidad</th>
                                        <th>Coordinador</th>
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
                                                <asp:DropDownList ID="ddlDistribucionJornada" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                    <%--paginacion de unidades--%>
                    <div class="row">
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
                                <asp:LinkButton ID="lbAnteriorUnidad" runat="server" CssClass="btn btn-default" OnClick="lbAnteriorUnidad_Click"><span class="glyphicon glyphicon-backward"></asp:LinkButton>
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
                                <asp:LinkButton ID="lbSiguienteUnidad" CssClass="btn btn-default" runat="server" OnClick="lbSiguienteUnidad_Click"><span class="glyphicon glyphicon-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbUltimoUnidad" CssClass="btn btn-primary" runat="server" OnClick="lbUltimoUnidad_Click"><span class="glyphicon glyphicon-fast-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:Label ID="lblpaginaUnidad" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                        </div>
                    </div>
                    <!-- ---------------------- FIN tabla unidades proyecto  ------------------------- -->
                    <%-- fin unidades--%>
                </div>

                <%--footer--%>
                <div class="modal-footer" style="text-align: center">
                    <button type="button" class="btn btn-default" onclick="agregarDistribucion()">Agregar</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        function activarModalDistribuirJornada() {
            $('#modalDistribuirJornada').modal('show');
        };

        function agregarDistribucion() {
            var porcentajeDistribucion = 50;
            var div = document.createElement('div');
            div.className = 'progress-bar';
            div.role = "progressbar";
            var progreso = 0;
            var interval = setInterval(function () {
                progreso += 5;
                div.style.width = progreso + '%';
                if (progreso >= porcentajeDistribucion) {
                    clearInterval(interval);
                }
            }, 200);
            document.getElementById('progressBar').appendChild(div);
            $('#modalDistribuirJornada').modal('show');
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
