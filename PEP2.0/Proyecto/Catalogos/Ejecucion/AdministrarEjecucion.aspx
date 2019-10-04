<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarEjecucion.aspx.cs" Inherits="Proyecto.Catalogos.Ejecucion.AdministrarEjecucion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="form-group col-md-12 col-xs-12 col-sm-12">
                    <center>
                <asp:Label runat="server" Text="Ejecución" Font-Size="Large" ForeColor="Black"></asp:Label>
            
                <p class="mt-1">Seleccione un periodo</p>
                <asp:DropDownList AutoPostBack="true" ID="PeriodosDDL" runat="server" CssClass="form-control" OnSelectedIndexChanged="Periodos_OnChanged"></asp:DropDownList>
                
                <p class="mt-1">Seleccione un proyecto</p>
                <asp:DropDownList AutoPostBack="true" ID="ProyectosDDL" runat="server" CssClass="form-control" OnSelectedIndexChanged="Proyectos_OnChanged"></asp:DropDownList>
                </div>
                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Label ID="Unidades" runat="server" Text="Unidades" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                    <asp:Button ID="ButtonAsociar" runat="server" Text="Asociar" CssClass="btn btn-primary" OnClick="ButtonAsociar_Click" />
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 mt-2">
                    <hr />
                </div>
                <%-- tabla--%>
                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table id="tblPresupuesto" class="table table-bordered">
                        <thead style="text-align: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th></th>
                                <th>Nombre</th>
                                <th>Unidad </th>

                            </tr>
                        </thead>

                        <asp:Repeater ID="rpEjecucion" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
                                        <%-- <asp:LinkButton ID="btnAnadirPartida" runat="server" ToolTip="Anadir" CommandArgument='<%# Eval("idPresupuestoEgreso") %>' OnClick="btnNuevoPresupuesto_Click"><span class="btn glyphicon glyphicon-plus"></span></asp:LinkButton>
                                        <asp:LinkButton ID="btnVerPartida" runat="server" ToolTip="Ver" CommandArgument='<%# Eval("idPresupuestoEgreso") %>' OnClick="btnVerPartidasEgreso_Click"><span class="btn glyphicon glyphicon-eye-open"></span></asp:LinkButton>--%>

                                    </td>
                                    <td>
                                        <%# Eval("idUnidad") %>
                                    
                                    </td>
                                    <%--<td>₡  <%# Eval("montoTotal") %>--%>

                                    <%--</td>--%>

                                    </td>
                                </tr>

                            </ItemTemplate>

                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Modal Elegir unidad -->
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div id="modalElegirUnidad" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Elegir Unidad</h4>
                        </div>
                        <div class="modal-body">
                            <%-- campos a llenar --%>
                            <div class="row">

                                <%-- fin campos a llenar --%>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="label4" runat="server" Text="Seleccione una Unidad" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                </div>
                                <%-- tabla--%>
                                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                    <table id="tblPresupuesto" class="table table-bordered">
                                        <thead style="text-align: center">
                                            <tr style="text-align: center" class="btn-primary">
                                                <th></th>
                                                <th>Nombre</th>
                                                <th>Unidad </th>

                                            </tr>
                                        </thead>

                                        <asp:Repeater ID="Repeater1" runat="server">
                                            <HeaderTemplate>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr style="text-align: center">
                                                    <td>
                                                        <%-- <asp:LinkButton ID="btnAnadirPartida" runat="server" ToolTip="Anadir" CommandArgument='<%# Eval("idPresupuestoEgreso") %>' OnClick="btnNuevoPresupuesto_Click"><span class="btn glyphicon glyphicon-plus"></span></asp:LinkButton>
                                                                    <asp:LinkButton ID="btnVerPartida" runat="server" ToolTip="Ver" CommandArgument='<%# Eval("idPresupuestoEgreso") %>' OnClick="btnVerPartidasEgreso_Click"><span class="btn glyphicon glyphicon-eye-open"></span></asp:LinkButton>--%>

                                                    </td>
                                                    <td>
                                                        <%--<%# Eval("idPresupuestoEgreso") %>--%>
                                    
                                                    </td>
                                                    <%--<td>₡  <%# Eval("montoTotal") %>--%>

                                                    <%--</td>--%>

                                                    <td>
                                                        <%--<%# Eval("descripcion") %>--%>
                                                    </td>
                                                </tr>

                                            </ItemTemplate>

                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>

                                </div>
                            </div>
                            <div class="modal-footer" style="text-align: center">

                                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>

                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal elegir unidad -->

    <script type="text/javascript">
        function activarModalElegirUnidad() {
            $('#modalElegirUnidad').modal('show');
        };

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
