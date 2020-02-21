<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="ElegirEjecucion.aspx.cs" Inherits="Proyecto.Catalogos.Ejecucion.ElegirEjecucion"  MaintainScrollPositionOnPostback="true" %>
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
                 <%-- tabla--%>
                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table id="tblUnidad" class="table table-bordered">
                        <thead style="text-align: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th></th>
                                <th>Número de Referencia</th>
                                <th>Estado</th>
                                <th>Tipo de Tramite</th>
                                <th>Monto</th>

                            </tr>
                        </thead>

                        <asp:Repeater ID="rpUnidadSelecionadas" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
<%--                                        <asp:LinkButton ID="btnEliminarUnidad" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idUnidad") %>' OnClick="EliminarUnidadSeleccionada_OnChanged" Cssclass="btn glyphicon glyphicon-remove" />--%>

                                    </td>
                                    <td>
                                        <%# Eval("numeroReferencia") %>
                                    
                                    </td>

                                    <td>
                                        <%# Eval("descripcion") %>
                                    </td>
                                     <td>
                                        <%# Eval("nombreTramite") %>
                                    </td>
                                     <td>
                                        <%# Eval("monto") %>
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
 </asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
 </asp:Content>
