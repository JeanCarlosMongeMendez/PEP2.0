<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarAnualidades.aspx.cs" Inherits="Proyecto.Mantenimiento.Anualidad.AdministrarAnualidades" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="row">
                <%-- titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12">
                    <center>
                        <asp:Label ID="label" runat="server" Text="Anualidades" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </center>
                </div>
                <%-- fin titulo pantalla --%>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>

                <%-- tabla--%>

                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table class="table table-bordered" >
                        <thead style="text-align: center !important; align-content: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th></th>
                                <th>Porcentaje</th>
                                <th>Período</th>
                            </tr>
                        </thead>
                        <asp:Repeater ID="rpAnualidades" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
                                        <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idAnualidad") %>' OnClick="btnEditar_Click"><span class="btn glyphicon glyphicon-pencil"></span></asp:LinkButton>
                                        <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idAnualidad") %>' OnClick="btnEliminar_Click"><span class="btn glyphicon glyphicon-trash"></span></asp:LinkButton>
                                    </td>
                                    <td>
                                        <%# Eval("porcentaje") %> %
                                    </td>
                                    <td><%# Eval("periodo.anoPeriodo") %>
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

                <div class="col-md-2 col-xs-2 col-sm-2 col-md-offset-10 col-xs-offset-10 col-sm-offset-10">
                    <asp:Button ID="btnNuevaAnualidad" runat="server" Text="Nueva anualidad" CssClass="btn btn-primary boton-nuevo" OnClick="btnNuevaAnualidad_Click"/>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Modal nueva anualidad -->
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div id="modalNuevaAnualidad" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Nueva anualidad</h4>
                        </div>
                        <div class="modal-body">
                            <%-- campos a llenar --%>
                            <div class="row">

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label4" runat="server" Text="Período" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:DropDownList ID="ddlPeriodoModalNuevo" class="btn btn-default dropdown-toggle" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label3" runat="server" Text="Porcentaje <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <asp:TextBox class="form-control" ID="txtPorcentajeModalNuevo" runat="server"></asp:TextBox>
                                            <span class="input-group-addon">%</span>
                                        </div>
                                    </div>
                                </div>

                                <%-- fin campos a llenar --%>
                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnNuevaAnualidadModal" runat="server" Text="Guardar" CssClass="btn btn-primary boton-nuevo" OnClick="btnNuevaAnualidadModal_Click"/>
                            <button type="button" class="btn btn-default boton-eliminar" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal nueva anualidad -->

    <!-- Modal editar anualidad -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="modalEditarAnualidad" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Editar anualidad</h4>
                        </div>
                        <div class="modal-body">
                            <%-- campos a llenar --%>
                            <div class="row">

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label1" runat="server" Text="Período" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:TextBox class="form-control" ID="txtEditarModalEditar" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label2" runat="server" Text="Porcentaje <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <asp:TextBox class="form-control" ID="txtPorcentajeModalEditar" runat="server"></asp:TextBox>
                                            <span class="input-group-addon">%</span>
                                        </div>
                                    </div>
                                </div>

                                <%-- fin campos a llenar --%>
                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnEditarAnualidadModal" runat="server" Text="Actualizar" CssClass="btn btn-primary boton-editar" OnClick="btnEditarAnualidadModal_Click"/>
                            <button type="button" class="btn btn-default boton-eliminar" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal editar escala -->

    <!-- Modal editar anualidad -->
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div id="modalEliminarAnualidad" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Eliminar anualidad</h4>
                        </div>
                        <div class="modal-body">
                            <%-- campos a llenar --%>
                            <div class="row">

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label5" runat="server" Text="Período" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:TextBox class="form-control" ID="txtEditarModalEliminar" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label6" runat="server" Text="Porcentaje" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <asp:TextBox class="form-control" ID="txtPorcentajeModalEliminar" runat="server" ReadOnly="true"></asp:TextBox>
                                            <span class="input-group-addon">%</span>
                                        </div>
                                    </div>
                                </div>

                                <%-- fin campos a llenar --%>
                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnEliminarAnualidadModal" runat="server" Text="Eliminar" CssClass="btn btn-primary boton-eliminar" OnClick="btnEliminarAnualidadModal_Click"/>
                            <button type="button" class="btn btn-default boton-eliminar" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal eliminar escala -->

    <script type="text/javascript">
        
        function activarModalNuevaAnualidad() {
            $('#modalNuevaAnualidad').modal('show');
        };

        function activarModalEditarAnualidad() {
            $('#modalEditarAnualidad').modal('show');
        };

        function activarModalEliminarAnualidad() {
            $('#modalEliminarAnualidad').modal('show');
        };
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
