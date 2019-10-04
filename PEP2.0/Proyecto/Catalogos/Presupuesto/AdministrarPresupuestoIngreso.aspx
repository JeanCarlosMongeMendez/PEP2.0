<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarPresupuestoIngreso.aspx.cs" Inherits="Proyecto.Catalogos.Presupuesto.AdministrarPresupuestoIngreso" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>
    <div class="row">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>

                <%-- titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12">
                    <center>
                        <asp:Label ID="label" runat="server" Text="Presupuesto de ingreso" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </center>
                </div>
                <%-- fin titulo pantalla --%>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>

                <div class="col-md-6 col-xs-6 col-sm-6">
                    <h4>Período</h4>
                    <asp:DropDownList ID="ddlPeriodo" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>

                <%-- tabla--%>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                            <table class="table table-bordered">
                                <thead style="text-align: center !important; align-content: center">
                                    <tr style="text-align: center" class="btn-primary">
                                        <th></th>
                                        <th>Nombre</th>
                                        <th>Código</th>
                                    </tr>
                                </thead>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary"><span aria-hidden="true" class="glyphicon glyphicon-search" onclick="btnFiltrar_Click"></span> </asp:LinkButton></td>
                                    <td>
                                        <asp:TextBox ID="txtBuscarNombre" runat="server" CssClass="form-control chat-input" placeholder="filtro nombre" AutoPostBack="true" OnTextChanged="btnFiltrar_Click"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <asp:Repeater ID="rpProyectos" runat="server">
                                    <HeaderTemplate>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr style="text-align: center">
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btnSelccionar" runat="server" ToolTip="Seleccionar" CommandArgument='<%# Eval("idProyecto") %>' OnClick="btnSelccionar_Click"><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnSelccionar" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <%# Eval("nombreProyecto") %>
                                            </td>
                                            <td><%# Eval("codigo") %>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%-- fin tabla--%>

                <asp:Panel ID="PanelIngresos" runat="server" Visible="false">

                    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                        <br />
                    </div>

                    <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center">
                        <asp:Label ID="lblIngresos" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                        </div>
                    <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center">
                        <asp:Label ID="lblMontoTotal" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                        </div>
                        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center">
                        <asp:Label ID="lblMontoAprobado" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </div>
                    <%-- fin titulo pantalla --%>

                    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                        <hr />
                    </div>

                    <%-- tabla--%>

                    <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                        <table class="table table-bordered">
                            <thead style="text-align: center !important; align-content: center">
                                <tr style="text-align: center" class="btn-primary">
                                    <th></th>
                                    <th>Monto</th>
                                    <th>Tipo</th>
                                    <th>Estado</th>
                                </tr>
                            </thead>
                            <asp:Repeater ID="rpIngresos" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <tr style="text-align: center">
                                        <td>
                                            <asp:LinkButton ID="btnAprobar" runat="server" ToolTip="Aprobar" CommandArgument='<%# Eval("idPresupuestoIngreso") %>' OnClick="btnAprobar_Click" Visible='<%# Convert.ToString(Eval("estadoPresupIngreso.descEstado"))=="Guardar"? true:false %>'><span class="glyphicon glyphicon-share"></span></asp:LinkButton>
                                            <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idPresupuestoIngreso") %>' OnClick="btnEditar_Click" Visible='<%# Convert.ToString(Eval("estadoPresupIngreso.descEstado"))=="Guardar"? true:false %>'><span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
                                            <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idPresupuestoIngreso") %>' OnClick="btnEliminar_Click" Visible='<%# Convert.ToString(Eval("estadoPresupIngreso.descEstado"))=="Guardar"? true:false %>'><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                        </td>
                                        <td>₡ <%# Eval("monto") %>
                                        </td>
                                        <td>
                                            <%# Convert.ToBoolean(Eval("esInicial"))==true?"Inicial":"Adicional" %>
                                        </td>
                                        <td>
                                           <%# Convert.ToString(Eval("estadoPresupIngreso.descEstado"))=="Guardar"? "Guardado":"Registrado" %>
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
                                <asp:LinkButton ID="lbPrimero2" runat="server" CssClass="btn btn-primary" OnClick="lbPrimero2_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbAnterior2" runat="server" CssClass="btn btn-default" OnClick="lbAnterior2_Click"><span class="glyphicon glyphicon-backward"></asp:LinkButton>
                            </td>
                            <td style="padding:1px !important">
                                <asp:DataList ID="rptPaginacion2" runat="server"
                                    OnItemCommand="rptPaginacion2_ItemCommand"
                                    OnItemDataBound="rptPaginacion2_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaginacion2" runat="server" CssClass="btn btn-default"
                                            CommandArgument='<%# Eval("IndexPagina") %>' CommandName="nuevaPagina"
                                            Text='<%# Eval("PaginaText") %>' ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbSiguiente2" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente2_Click"><span class="glyphicon glyphicon-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbUltimo2" CssClass="btn btn-primary" runat="server" OnClick="lbUltimo2_Click"><span class="glyphicon glyphicon-fast-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:Label ID="lblpagina2" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                    </div>

                    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                        <br />
                    </div>

                    <div class="row col-md-12 col-xs-12 col-sm-12">
                        <div class="col-md-12 col-xs-12 col-sm-12 mt-1 alinear-derecha">
                            <asp:Button Text="Nuevo ingreso" ID="btnNuevoIngreso" runat="server" class="btn btn-primary boton-nuevo" OnClick="btnNuevoIngreso_Click" />
                        </div>
                    </div>

                    <%-- fin tabla--%>
                </asp:Panel>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- Modal nuevo ingreso -->
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div id="modalNuevoIngreso" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Nuevo ingreso</h4>
                        </div>
                        <div class="modal-body">
                            <%-- campos a llenar --%>
                            <div class="row">

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label4" runat="server" Text="Proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-9 col-xs-12 col-sm-9" style="text-align: left">
                                        <asp:Label ID="lblProyectoNuevoModal" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label1" runat="server" Text="Tipo de ingreso" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-9 col-xs-12 col-sm-9" style="text-align: left">
                                        <asp:Label ID="lblTipoIngresoModalNuevo" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label2" runat="server" Text="Monto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox class="form-control" ID="txtMontoModalNuevo" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <%-- fin campos a llenar --%>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnNuevoIngresoModal" runat="server" Text="Guardar" CssClass="btn btn-primary boton-nuevo" OnClick="btnNuevoIngresoModal_Click" />
                            <button type="button" class="btn btn-danger boton-eliminar" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal nuevo ingreso -->

    <!-- Modal editar ingreso -->
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div id="modalEditarIngreso" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Editar ingreso</h4>
                        </div>
                        <div class="modal-body">
                            <%-- campos a llenar --%>
                            <div class="row">

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label3" runat="server" Text="Proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-9 col-xs-12 col-sm-9" style="text-align: left">
                                        <asp:Label ID="lblProyectoEditarModal" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label6" runat="server" Text="Tipo de ingreso" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-9 col-xs-12 col-sm-9" style="text-align: left">
                                        <asp:Label ID="lblTipoIngresoModalEditar" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label8" runat="server" Text="Monto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox class="form-control" ID="txtMontoModalEditar" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <%-- fin campos a llenar --%>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnActualizarIngresoModalEditar" runat="server" Text="Actualizar" CssClass="btn btn-primary boton-editar" OnClick="btnActualizarIngresoModalEditar_Click"/>
                            <button type="button" class="btn btn-danger boton-eliminar" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal editar ingreso -->

    <!-- Modal eliminar ingreso -->
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
            <div id="modalEliminarIngreso" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Eliminar ingreso</h4>
                        </div>
                        <div class="modal-body">
                            <%-- campos a llenar --%>
                            <div class="row">

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label5" runat="server" Text="Proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-9 col-xs-12 col-sm-9" style="text-align: left">
                                        <asp:Label ID="lblProyectoEliminarModal" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label9" runat="server" Text="Tipo de ingreso" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-9 col-xs-12 col-sm-9" style="text-align: left">
                                        <asp:Label ID="lblTipoIngresoModalEliminar" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label11" runat="server" Text="Monto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox class="form-control" ID="txtMontoModalEliminar" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <%-- fin campos a llenar --%>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnEliminarIngresoModalEliminar" runat="server" Text="Eliminar" CssClass="btn btn-primary boton-eliminar" OnClick="btnEliminarIngresoModalEliminar_Click"/>
                            <button type="button" class="btn btn-danger boton-eliminar" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal eliminar ingreso -->

    <!-- Modal aprobar ingreso -->
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>
            <div id="modalAprobarIngreso" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Aprobar ingreso</h4>
                        </div>
                        <div class="modal-body">
                            <%-- campos a llenar --%>
                            <div class="row">

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label7" runat="server" Text="Proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-9 col-xs-12 col-sm-9" style="text-align: left">
                                        <asp:Label ID="lblProyectoAprobarModal" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label12" runat="server" Text="Tipo de ingreso" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-9 col-xs-12 col-sm-9" style="text-align: left">
                                        <asp:Label ID="lblTipoIngresoModalAprobar" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label14" runat="server" Text="Monto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox class="form-control" ID="txtMontoModalAprobar" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <%-- fin campos a llenar --%>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnAprobarIngresoModalAprobar" runat="server" Text="Aprobar" CssClass="btn btn-primary boton-eliminar" OnClick="btnAprobarIngresoModalAprobar_Click"/>
                            <button type="button" class="btn btn-danger boton-eliminar" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal eliminar ingreso -->

    <script type="text/javascript">
        function activarModalNuevoIngreso() {
            $('#modalNuevoIngreso').modal('show');
        };

        function activarModalEditarIngreso() {
            $('#modalEditarIngreso').modal('show');
        };

        function activarModalEliminarIngreso() {
            $('#modalEliminarIngreso').modal('show');
        };

        function activarModalAprobarIngreso() {
            $('#modalAprobarIngreso').modal('show');
        };
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
