<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarPresupuestoEgresos.aspx.cs" Inherits="Proyecto.Catalogos.Presupuesto.AdministrarPresupuestoEgresos" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>
    <div class="row">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <%-- titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                    <asp:Label ID="label" runat="server" Text="Presupuesto de egresos" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>
                <%-- fin titulo pantalla --%>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <h4>Período</h4>
                    <asp:DropDownList ID="ddlPeriodos" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <h4>Proyecto</h4>
                    <asp:DropDownList ID="ddlProyectos" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <h4>Unidades</h4>
                    <asp:DropDownList ID="ddlUnidades" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUnidades_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">

                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <asp:Button Text="Nuevo plan estrategico operacional" ID="btnNuevoPlan" runat="server" class="btn btn-primary boton-nuevo" OnClick="btnNuevoPlan_Click" />
                    </div>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                <table class="table table-bordered">
                                    <thead style="text-align: center !important; align-content: center">
                                        <tr style="text-align: center" class="btn-primary">
                                            <th></th>
                                            <th>Plan estrategico</th>
                                        </tr>
                                    </thead>
                                    <asp:Repeater ID="rpPlanEstrategico" runat="server">
                                        <HeaderTemplate>
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <tr style="text-align: center">
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idPresupuestoEgreso") %>' OnClick="btnEditar_Click"><span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
                                                            <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idPresupuestoEgreso") %>' OnClick="btnEliminar_Click"><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnEditar" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <%# Eval("planEstrategicoOperacional") %>
                                                </td>
                                            </tr>

                                        </ItemTemplate>

                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <asp:Label ID="lblMontoIngresos" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <asp:Label ID="lblMontoAprobado" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <asp:Label ID="lblMontoGuardado" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>

                <!-- tabla partidas -->
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                            <table class="table table-bordered">
                                <thead style="text-align: center !important; align-content: center">
                                    <tr style="text-align: center" class="btn-primary">
                                        <th></th>
                                        <th>Número partida</th>
                                        <th>Descripción partida</th>
                                        <th>Monto</th>
                                        <th>Descripción</th>
                                    </tr>
                                </thead>
                                <asp:Repeater ID="rpPartidas" runat="server">
                                    <HeaderTemplate>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <div runat="server" visible='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("descripcion"))) || Convert.ToDouble(Eval("monto")) > 0? true:false %>'>
                                                    <asp:LinkButton ID="btnAgregarPartida" runat="server" ToolTip="Agregar" CommandArgument='<%# Eval("partida.idPartida") %>' OnClick="btnAgregarPartida_Click"><span class="glyphicon glyphicon-plus"></span></asp:LinkButton>
                                                    <asp:LinkButton ID="btnEditarPartida" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("partida.idPartida") %>' OnClick="btnEditarPartida_Click"><span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
                                                </div>
                                            </td>
                                            <td style='<%# Convert.ToString(Eval("estadoPresupuesto.descripcionEstado")).Equals("Guardar")? "background-color:#fd8e03": (Convert.ToString(Eval("estadoPresupuesto.descripcionEstado")).Equals("Aprobar")? "background-color:#3eb13e": "background-color:white") %>'>
                                                <%# Eval("partida.numeroPartida") %>
                                            </td>
                                            <td>
                                                <%# Eval("partida.descripcionPartida") %>
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="hdIdPartida" Value='<%# Eval("partida.idPartida") %>' runat="server" />
                                                <div class="input-group">
                                                    <span class="input-group-addon">₡</span>
                                                    <asp:TextBox class="form-control" ID="txtMonto" runat="server" Text='<%# Eval("monto") %>' ReadOnly='<%# Convert.ToDouble(Eval("monto")) > 0? true:false || !String.IsNullOrEmpty(Convert.ToString(Eval("descripcion")))%>'></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnVerDescripcion" runat="server" ToolTip="Ver descripción" CommandArgument='<%# Eval("partida.idPartida") %>' Visible='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("descripcion"))) || Convert.ToDouble(Eval("monto")) > 0? true:false %>' OnClick="btnVerDescripcion_Click"><span class="glyphicon glyphicon-eye-open"></span></asp:LinkButton>
                                                <asp:TextBox class="form-control" ID="txtDescripcion" runat="server" Text='<%# Eval("descripcion") %>' TextMode="MultiLine" Visible='<%# String.IsNullOrEmpty(Convert.ToString(Eval("descripcion"))) && Convert.ToDouble(Eval("monto")) == 0? true:false %>'></asp:TextBox>
                                            </td>
                                        </tr>

                                    </ItemTemplate>

                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- fin tabla partidas -->

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary boton-nuevo" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" CssClass="btn btn-primary boton-nuevo" OnClick="btnAprobar_Click"/>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Modal nuevo plan -->
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <div id="modalNuevoPlan" class="modal fade" role="alertdialog">
                    <div class="modal-dialog modal-lg">

                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Nuevo plan estrategico operacional</h4>
                            </div>
                            <div class="modal-body">
                                <%-- campos a llenar --%>
                                <div class="row">

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                            <asp:Label ID="Label14" runat="server" Text="Descripción" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>

                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                            <asp:TextBox class="form-control" ID="txtDescNuevoPlanModal" runat="server" TextMode="MultiLine" Style="width: 500px; height: 300px"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                                <%-- fin campos a llenar --%>
                            </div>
                            <div class="modal-footer" style="text-align: center">
                                <asp:Button ID="btnNuevoPlanModal" runat="server" Text="Guardar" CssClass="btn btn-primary boton-nuevo" OnClick="btnNuevoPlanModal_Click" />
                                <button type="button" class="btn btn-danger boton-eliminar" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Fin modal nuevo plan -->

        <!-- Modal editar plan -->
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="modalEditarPlan" class="modal fade" role="alertdialog">
                    <div class="modal-dialog modal-lg">

                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Editar plan estrategico operacional</h4>
                            </div>
                            <div class="modal-body">
                                <%-- campos a llenar --%>
                                <div class="row">

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                            <asp:Label ID="Label1" runat="server" Text="Descripción" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>

                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                            <asp:TextBox class="form-control" ID="txtDescEditarPlanModal" runat="server" TextMode="MultiLine" Style="width: 500px; height: 300px"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                                <%-- fin campos a llenar --%>
                            </div>
                            <div class="modal-footer" style="text-align: center">
                                <asp:Button ID="btnEditarPlanModal" runat="server" Text="Actualizar" CssClass="btn btn-primary boton-editar" OnClick="btnEditarPlanModal_Click" />
                                <button type="button" class="btn btn-danger boton-eliminar" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Fin modal editar plan -->

        <!-- Modal eliminar plan -->
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div id="modalEliminarPlan" class="modal fade" role="alertdialog">
                    <div class="modal-dialog modal-lg">

                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Eliminar plan estrategico operacional</h4>
                            </div>
                            <div class="modal-body">
                                <%-- campos a llenar --%>
                                <div class="row">

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                            <asp:Label ID="Label2" runat="server" Text="Descripción" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>

                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                            <asp:TextBox class="form-control" ID="txtDescEliminarPlanModal" runat="server" ReadOnly="true" TextMode="MultiLine" Style="width: 500px; height: 300px"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                                <%-- fin campos a llenar --%>
                            </div>
                            <div class="modal-footer" style="text-align: center">
                                <asp:Button ID="btnEliminarPlanModal" runat="server" Text="Eliminar" CssClass="btn btn-primary boton-eliminar" OnClick="btnEliminarPlanModal_Click" />
                                <button type="button" class="btn btn-danger boton-eliminar" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Fin modal editar plan -->

        <!-- Modal agregar partida -->
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <div id="modalAgregarPartida" class="modal fade" role="alertdialog">
                    <div class="modal-dialog modal-lg">

                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Agregar</h4>
                            </div>
                            <div class="modal-body">
                                <%-- campos a llenar --%>
                                <div class="row">

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                            <asp:Label ID="Label5" runat="server" Text="Partida seleccionada" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                            <asp:Label ID="lblPartidaSeleccionadaModalAgregar" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                            <asp:Label ID="Label4" runat="server" Text="Monto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>

                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">₡</span>
                                                <asp:TextBox class="form-control" ID="txtMontoModalAgregarPartida" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                            <asp:Label ID="Label3" runat="server" Text="Descripción" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>

                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                            <asp:TextBox class="form-control" ID="txtDescripcionModalAgregarPartida" runat="server" TextMode="MultiLine" Style="width: 500px; height: 300px"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                                <%-- fin campos a llenar --%>
                            </div>
                            <div class="modal-footer" style="text-align: center">
                                <asp:Button ID="btnGuardarModalPartida" runat="server" Text="Guardar" CssClass="btn btn-primary boton-nuevo" OnClick="btnGuardarModalPartida_Click" />
                                <button type="button" class="btn btn-danger boton-eliminar" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Fin modal agregar partida -->

        <!-- Modal Ver descripcion partida -->
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <div id="modalVerDescripcionPartida" class="modal fade" role="alertdialog">
                    <div class="modal-dialog modal-lg">

                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Ver descripción</h4>
                            </div>
                            <div class="modal-body">
                                <%-- campos a llenar --%>
                                <div class="row">

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                            <asp:Label ID="Label6" runat="server" Text="Partida seleccionada" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>
                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                            <asp:Label ID="lblPartidaSeleccionadaModalVerDescripcion" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                            <asp:Label ID="Label9" runat="server" Text="Descripción" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>

                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                            <asp:TextBox class="form-control" ID="txtDescripcionModalVerDescripcionPartida" ReadOnly="true" runat="server" TextMode="MultiLine" Style="width: 500px; height: 300px"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                                <%-- fin campos a llenar --%>
                            </div>
                            <div class="modal-footer" style="text-align: center">
                                <button type="button" class="btn btn-danger boton-eliminar" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Fin modal ver descripcion partida -->

        <!-- Modal editar -->
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <div id="modalEditar" class="modal fade" role="alertdialog">
                    <div class="modal-dialog modal-lg" style="width: 98% !important">

                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">Editar</h4>
                            </div>
                            <div class="modal-body">
                                <%-- campos a llenar --%>
                                <div class="row">

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <!-- tabla partidas -->
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                                <table class="table table-bordered">
                                                    <thead style="text-align: center !important; align-content: center">
                                                        <tr style="text-align: center" class="btn-primary">
                                                            <th></th>
                                                            <th>Número partida</th>
                                                            <th>Descripción partida</th>
                                                            <th>Monto</th>
                                                            <th>Descripción</th>
                                                        </tr>
                                                    </thead>
                                                    <asp:Repeater ID="rpPartidasEditar" runat="server">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>

                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="btnAprobarModalEditar" runat="server" ToolTip="Aprobar" CommandArgument='<%# Eval("partida.idPartida") +"~"+ Eval("idPresupuestoEgreso")+"~"+Eval("idLinea")+"~"+Eval("monto") %>' OnClick="btnAprobarModalEditar_Click" Visible='<%# Convert.ToString(Eval("estadoPresupuesto.descripcionEstado")).Equals("Aprobar")? false: true %>'><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                                                                    <asp:LinkButton ID="btnEliminarModalEditar" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("partida.idPartida") +"~"+ Eval("idPresupuestoEgreso")+"~"+Eval("idLinea")+"~"+Eval("monto") %>' OnClick="btnEliminarModalEditar_Click" Visible='<%# Convert.ToString(Eval("estadoPresupuesto.descripcionEstado")).Equals("Aprobar")? false: true %>'><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                                                </td>
                                                                <td style='<%# Convert.ToString(Eval("estadoPresupuesto.descripcionEstado")).Equals("Guardar")? "background-color:#fd8e03": (Convert.ToString(Eval("estadoPresupuesto.descripcionEstado")).Equals("Aprobar")? "background-color:#3eb13e": "background-color:white") %>'>
                                                                    <%# Eval("partida.numeroPartida") %>
                                                                </td>
                                                                <td>
                                                                    <%# Eval("partida.descripcionPartida") %>
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="hdIdPartida" Value='<%# Eval("partida.idPartida") %>' runat="server" />
                                                                    <asp:HiddenField ID="hdIdPresupuesto" Value='<%# Eval("idPresupuestoEgreso") %>' runat="server" />
                                                                    <asp:HiddenField ID="hdIdLinea" Value='<%# Eval("idLinea") %>' runat="server" />
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">₡</span>
                                                                        <asp:TextBox class="form-control" ID="txtMonto" runat="server" Text='<%# Eval("montoo") %>' ReadOnly='<%# Convert.ToString(Eval("estadoPresupuesto.descripcionEstado")).Equals("Aprobar")? true: false %>'></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox class="form-control" ID="txtDescripcion" runat="server" Text='<%# Eval("descripcion") %>' TextMode="MultiLine" ReadOnly='<%# Convert.ToString(Eval("estadoPresupuesto.descripcionEstado")).Equals("Aprobar")? true: false %>'></asp:TextBox>
                                                                </td>
                                                            </tr>

                                                        </ItemTemplate>

                                                        <FooterTemplate>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <!-- fin tabla partidas -->

                                </div>
                                <%-- fin campos a llenar --%>
                            </div>
                            <div class="modal-footer" style="text-align: center">
                                <asp:Button ID="btnActualizarModalEditar" runat="server" Text="Actualizar" CssClass="btn btn-primary boton-editar" OnClick="btnActualizarModalEditar_Click" />
                                <button type="button" class="btn btn-danger boton-eliminar" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>

                    </div>
                </div>
            </ContentTemplate> 
        </asp:UpdatePanel>
        <!-- Fin modal editar -->

        <!-- Modal eliminar editar -->
        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
            <ContentTemplate>
                <div id="modalEliminarEditar" class="modal fade" data-keyboard="false" data-backdrop="static" role="alertdialog">
                    <div class="modal-dialog modal-lg">

                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title">Eliminar</h4>
                            </div>
                            <div class="modal-body">
                                <%-- campos a llenar --%>
                                <div class="row">

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                            <asp:Label ID="lblEliminarModalEliminarEditar" runat="server" Text="¿Desea eliminar la información seleccionada?" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                </div>
                                <%-- fin campos a llenar --%>
                            </div>
                            <div class="modal-footer" style="text-align: center">
                                <asp:Button ID="btnSiEliminarEditar" runat="server" Text="Si" CssClass="btn btn-primary boton-nuevo" OnClick="btnSiEliminarEditar_Click" />
                                <asp:Button ID="btnNoEliminarEditar" runat="server" Text="No" CssClass="btn btn-primary boton-eliminar" OnClick="btnNoEliminarEditar_Click" />
                            </div>
                        </div>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Fin modal eliminar editar -->

        <!-- Modal aprobar editar -->
        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
            <ContentTemplate>
                <div id="modalAprobarEditar" class="modal fade" data-keyboard="false" data-backdrop="static" role="alertdialog">
                    <div class="modal-dialog modal-lg">

                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title">Eliminar</h4>
                            </div>
                            <div class="modal-body">
                                <%-- campos a llenar --%>
                                <div class="row">

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                            <asp:Label ID="Label7" runat="server" Text="¿Desea aprobar la información seleccionada?" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                </div>
                                <%-- fin campos a llenar --%>
                            </div>
                            <div class="modal-footer" style="text-align: center">
                                <asp:Button ID="btnSiAprobarEditar" runat="server" Text="Si" CssClass="btn btn-primary boton-nuevo" OnClick="btnSiAprobarEditar_Click" />
                                <asp:Button ID="btnNoAprobarEditar" runat="server" Text="No" CssClass="btn btn-primary boton-eliminar" OnClick="btnNoAprobarEditar_Click" />
                            </div>
                        </div>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Fin modal aprobar editar -->


        <!-- Modal mensaje aprobar -->
        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
            <ContentTemplate>
                <div id="modalMensajeAprobar" class="modal fade" data-keyboard="false" data-backdrop="static" role="alertdialog">
                    <div class="modal-dialog modal-lg">

                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title">Información</h4>
                            </div>
                            <div class="modal-body">
                                <%-- campos a llenar --%>
                                <div class="row">

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                            <asp:Label ID="Label8" runat="server" Text="¿Esta seguro o segura de aprobar los montos GUARDADOS en la unidad seleccionada?" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                </div>
                                <%-- fin campos a llenar --%>
                            </div>
                            <div class="modal-footer" style="text-align: center">
                                <asp:Button ID="btnSiMensajeAprobar" runat="server" Text="Si" CssClass="btn btn-primary boton-nuevo" OnClick="btnSiMensajeAprobar_Click" />
                                <button type="button" class="btn btn-danger boton-eliminar" data-dismiss="modal">No</button>
                            </div>
                        </div>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Fin modal mensaje aprobar -->

    </div>

    <script type="text/javascript">
        function activarModalNuevoPlan() {
            $('#modalNuevoPlan').modal('show');
        };

        function activarModalEditarPlan() {
            $('#modalEditarPlan').modal('show');
        };

        function activarModalEliminarPlan() {
            $('#modalEliminarPlan').modal('show');
        };

        function activarModalAgregarPartida() {
            $('#modalAgregarPartida').modal('show');
        };

        function activarModalVerDescripcionPartida() {
            $('#modalVerDescripcionPartida').modal('show');
        };

        function activarModalEditar() {
            $('#modalEditar').modal('show');
        };

        function activarModalEliminarEditar() {
            $('#modalEliminarEditar').modal('show');
        };

        function activarModalAprobarEditar() {
            $('#modalAprobarEditar').modal('show');
        };

        function activarModalMensajeAprobar() {
            $('#modalMensajeAprobar').modal('show');
        };
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
