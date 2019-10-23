<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarProyeccion.aspx.cs" Inherits="Proyecto.Planilla.AdministrarProyeccion" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>

    <%-- titulo pantalla --%>
    <div class="row" style="text-align: center;">

        <div class="col-md-12 col-xs-12 col-sm-12">
            <asp:Label ID="label" runat="server" Text="Proyección Salarial" Font-Size="Large" ForeColor="Black"></asp:Label>
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
            <hr />
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

    <%--boton calcular proyeccion--%>
    <div class="row" style="text-align: center">
        <div class="col-md-3 col-xs-12 col-sm-12">
            <asp:Button ID="btnCalcularProyeccion" runat="server" Text="Calcular Proyección" CssClass="btn btn-primary boton-nuevo" OnClick="btnCalcularProyeccion_Click" />
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
                                    <asp:LinkButton ID="btnSelccionar" OnClick="btnSelccionarFuncionario_Click" runat="server" ToolTip="Seleccionar" CommandArgument='<%# Eval("idFuncionario") %>'><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
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

            <%--paginacion--%>
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

    <%--botones --%>
    <div class="row">
        <div class="col-md-2 col-xs-2 col-sm-2 col-md-offset-10 col-xs-offset-8 col-sm-offset-10">
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary boton-nuevo" OnClick="btnGuardar_Click" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-md-offset-10 col-xs-offset-10 col-sm-offset-10">
            <asp:Button ID="btnAprobar" runat="server" Text="Aprovar" CssClass="btn btn-primary boton-nuevo" OnClick="btnAprobar_Click" />
        </div>
    </div>

    <%--modal ver distribucion funcionario--%>

    <div id="modalVerProyeccionDeFuncionario" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg" style="min-width: 95%; margin: 2%">

            <!-- Modal content-->
            <div class="modal-content">

                <%--header--%>
                <div class="modal-header" style="background-color: #005da4; color: white">
                    <button type="button" class="close" data-dismiss="modal" style="color: white">&times;</button>
                    <h4 class="modal-title">Proyección del Funcionario</h4>
                </div>

                <%--body--%>
                <div class="modal-body">

                    <%-- datos --%>
                    <div class="row">

                        <%-- Datos generales --%>
                        <asp:Panel ID="panelDatosGenerales" runat="server" CssClass="col-md-12 col-xs-12 col-sm-12">

                            <%--nombre--%>
                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-3">
                                    <asp:Label runat="server" Text="Nombre Completo" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-6 col-xs-12 col-sm-12">
                                    <asp:TextBox CssClass="form-control" ID="txtNombreCompleto" runat="server" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                            </div>

                            <%--periodo--%>
                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Periodo" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:TextBox CssClass="form-control" ID="txtPeriodo" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>
                            </div>

                            <%-- Division --%>
                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                                <hr />
                                <br />
                            </div>

                        </asp:Panel>


                        <%-- Proyeccion --%>
                        <asp:Panel ID="panelProyeccion" runat="server" CssClass="col-md-12 col-xs-12 col-sm-12" Style="text-align: center">

                            <%--tabla--%>



                            <%-- Division --%>
                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                                <hr />
                                <br />
                            </div>

                        </asp:Panel>
                    </div>

                    <%-- Fin campos a llenar --%>
                </div>

                <%--footer--%>
                <div class="modal-footer" style="text-align: center">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>

            </div>
        </div>
    </div>

    <script type="text/javascript">
        function activarModalVerDistribucionDeFuncionario() {
            $('#modalVerProyeccionDeFuncionario').modal('show');
        };
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
