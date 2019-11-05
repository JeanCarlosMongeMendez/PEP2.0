<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarProyeccion.aspx.cs" Inherits="Proyecto.Planilla.AdministrarProyeccion" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>

    <%-- titulo pantalla --%>
    <asp:UpdatePanel ID="pnlUpdate" runat="server">
        <ContentTemplate>

            <div class="row" style="text-align: center;">

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <asp:Label ID="label" runat="server" Text="Proyección Salarial" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>


                <%--periodo--%>

                <div class="col-md-3 col-xs-12 col-sm-12">
                    <asp:Label runat="server" Text="Planilla" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>

                <div class="col-md-9 col-xs-12 col-sm-12" style="text-align: left">
                    <asp:DropDownList ID="ddlPeriodo" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <br />
                </div>

                <%--boton calcular proyeccion--%>

                <div class="col-md-3 col-xs-12 col-sm-12">
                    <asp:Button ID="btnCalcularProyeccion" runat="server" Text="Calcular Proyección" CssClass="btn btn-primary boton-nuevo" OnClick="btnCalcularProyeccion_Click" />
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
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-search"></i></span>
                                            <asp:TextBox ID="txtBuscarNombre" runat="server" CssClass="form-control chat-input" placeholder="filtro nombre" AutoPostBack="true" OnTextChanged="btnFiltrar_Click"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <asp:Repeater ID="rpFuncionarios" runat="server">
                                    <HeaderTemplate>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr style="text-align: center">
                                            <td>
                                                <asp:LinkButton ID="btnSelccionar" OnClick="btnSelccionarFuncionario_Click" runat="server" ToolTip="Ver" CommandArgument='<%# Eval("idFuncionario") %>'><span class="glyphicon glyphicon-eye-open"></span></asp:LinkButton>
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

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



    <%--modal ver distribucion funcionario--%>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
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

                                <div class="col-md-3 col-xs-12 col-sm-3">
                                    <asp:Label runat="server" Text="Nombre Completo" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:TextBox CssClass="form-control" ID="txtNombreCompleto" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <%--periodo--%>

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Periodo" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:TextBox CssClass="form-control" ID="txtPeriodo" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>
                                <%-- Proyeccion --%>

                                <%--tabla Funcionarios--%>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <%-- tabla--%>

                                        <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                            <table class="table table-bordered">
                                                <thead style="text-align: center">
                                                    <tr style="text-align: center" class="btn-primary">
                                                        <th>Mes</th>
                                                        <th>Monto Salario</th>
                                                        <th>Monto Cargas Sociales</th>
                                                    </tr>
                                                </thead>
                                                <asp:Repeater ID="rpProyeccion" runat="server">
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>

                                                    <ItemTemplate>
                                                        <tr style="text-align: center">
                                                            <td>
                                                                <%# Eval("mes.nombreMes") %>
                                                            </td>
                                                            <td>
                                                                ₡ <%# Eval("montoSalario") %>
                                                            </td>
                                                            <td>
                                                                ₡ <%# Eval("montoCargasTotal") %>
                                                            </td>
                                                        </tr>

                                                    </ItemTemplate>

                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>

                                        <div class="col-md-12 col-xs-12 col-sm-12">
                                            <br />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

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
        </ContentTemplate>
    </asp:UpdatePanel>

     <!-- update progress-->
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlUpdate">
        <ProgressTemplate>
            <div class="alert alert-info" role="alert">
                <h6> <p style="text-align:center"> <b>Procesando Datos, Espere por favor... <br /></b> </p> </h6>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <script type="text/javascript">
        function activarModalVerDistribucionDeFuncionario() {
            $('#modalVerProyeccionDeFuncionario').modal('show');
        };

    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
