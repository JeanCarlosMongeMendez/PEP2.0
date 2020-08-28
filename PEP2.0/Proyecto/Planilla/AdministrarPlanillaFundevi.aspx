<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarPlanillaFundevi.aspx.cs" Inherits="Proyecto.Planilla.AdministrarPlanillaFundevi" MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <%-- titulo pantalla --%>
        <div class="col-md-12 col-xs-12 col-sm-12">
            <center>
                        <asp:Label ID="label" runat="server" Text="Planillas Fundevi" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </center>
        </div>
        <%-- fin titulo pantalla --%>

        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
            <hr />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-md-offset-10 col-xs-offset-10 col-sm-offset-10">
            <asp:Button ID="btnNuevaPlanilla" runat="server" Text="Nueva" CssClass="btn btn-primary boton-nuevo" OnClick="btnNuevaPlanilla_Click" />
        </div>
        <%-- tabla--%>

        <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
            <table class="table table-bordered">
                <thead style="text-align: center">
                    <tr style="text-align: center" class="btn-primary">
                        <th></th>
                        <th>Periodo</th>
                    </tr>
                </thead>
                <tr>
                    <td>
                        <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary" OnClick="btnFiltrar_Click"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton></td>
                    <td>
                        <div class="input-group">
                            
                            <asp:TextBox ID="txtBuscarPeriodo" runat="server" CssClass="form-control chat-input" placeholder="filtro período" OnTextChanged="btnFiltrar_Click"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <asp:Repeater ID="rpPlanillas" runat="server">
                    <HeaderTemplate>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <tr style="text-align: center">
                            <td>
                                <asp:LinkButton ID="btnSelccionar" runat="server" ToolTip="Seleccionar"  OnClick="btnSelccionar_Click" CommandArgument='<%# Eval("idPlanilla") %>'><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                                
                                <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar" OnClick="btnEliminar_Click" CommandArgument='<%# Eval("anoPeriodo") %>'><span class="btn glyphicon glyphicon-trash"></span></asp:LinkButton>
                            </td>
                            <td>
                                <%# Eval("anoPeriodo") %> 
                            </td>
                        </tr>

                    </ItemTemplate>

                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
            <center>
                    <table class="table" style="max-width:664px;">
                        <tr style="padding:1px !important">
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbPrimero" runat="server" CssClass="btn btn-primary" OnClick="lbPrimero_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbAnterior" runat="server" CssClass="btn btn-default" OnClick="lbAnterior_Click"><span class="glyphicon glyphicon-backward"></asp:LinkButton>
                            </td>
                            <td style="padding:1px !important">
                                <asp:DataList ID="rptPaginacion" runat="server"
                                    OnItemCommand="rptPaginacion_ItemCommand"
                                    OnItemDataBound="rptPaginacion_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaginacion" runat="server" CssClass="btn btn-default"
                                            CommandArgument='<%# Eval("IndexPagina") %>' CommandName="nuevaPagina"
                                            Text='<%# Eval("PaginaText") %>' ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbSiguiente" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente_Click"><span class="glyphicon glyphicon-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbUltimo" CssClass="btn btn-primary" runat="server" OnClick="lbUltimo_Click"><span class="glyphicon glyphicon-fast-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:Label ID="lblpagina" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
        </div>

        <%-- fin tabla--%>

        

        <!-- Modal nueva planilla -->
        <div id="modalNuevaPlanilla" class="modal fade" role="alertdialog">
            <div class="modal-dialog modal-lg">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Nueva planilla Fundevi</h4>
                    </div>
                    <div class="modal-body" style="align-content:center;text-align:center">
                        <%-- campos a llenar --%>
                        <div class="row">

                            <%-- fin campos a llenar --%>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>
                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <asp:Label ID="label44" runat="server" Text="Seleccione un periodo para agregar la planilla" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>
                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>
                            
                        
                            <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                <div class="col-md-3 col-xs-3 col-sm-3">
                                    <asp:Label ID="label4" runat="server" Text="Período" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>
                                <div class="col-md-4 col-xs-4 col-sm-4">
                                    <asp:DropDownList ID="ddlPeriodo" class="btn btn-default dropdown-toggle" runat="server"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>
              

                        </div>
                    </div>
                    <div class="modal-footer" style="text-align: center">
                        <asp:Button ID="btnNuevaPlanillaModal" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick="btnNuevaPlanillaModal_Click" />
                        <button type="button" class="btn btn-danger" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>

            </div>
        </div>


        <!-- -->
        <div id="modalEliminarPlanilla" class="modal fade" role="alertdialog">
            <div class="modal-dialog modal-lg">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Eliminar planilla: </h4>
                    </div>
                    <div class="modal-body">
                        <%-- campos a llenar --%>
                        <div class="row">

                            <%-- fin campos a llenar --%>
                            <asp:Label ID="txtPeriodo" runat="server" Text="Planilla del periodo: "></asp:Label>
                            <asp:Label Id="txtAno" runat="server"></asp:Label>
                    </div>
                      </div>
                    <div class="modal-footer" style="text-align: center">
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-success" OnClick="btnEliminar_Click1" />
                        <button type="button" class="btn btn-danger" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>

            </div>
        </div>

    </div>
    <script type="text/javascript">
        function activarModalNuevaPlanilla() {
            $('#modalNuevaPlanilla').modal('show');
        };

        function activarModalEliminarPlanilla() {
            $('#modalEliminarPlanilla').modal('show');
        };
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>