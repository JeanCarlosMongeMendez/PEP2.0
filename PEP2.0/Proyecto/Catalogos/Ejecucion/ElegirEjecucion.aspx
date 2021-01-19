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
            </center>
                        <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    
                    <asp:Button ID="ButtonNuevaEjecucion" runat="server" Text="Nueva Ejecución" CssClass="btn btn-primary" OnClick="NuevaEjecucion_OnChanged" />
                </div>
                    <center>
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
                                 <th>Número de Ejecución</th>
                                <th>Número de Referencia</th>
                                <th>Estado</th>
                                <th>Tipo de Tramite</th>
                                <th>Monto</th>

                            </tr>
                        </thead>

                        <asp:Repeater ID="rpUnidadSelecionadas" runat="server" OnItemDataBound="rpEjecucion_ItemDataBound">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
                                        <asp:LinkButton ID="btnVerUnidad" runat="server" ToolTip="Ver" CommandArgument='<%# Eval("idEjecucion") %>' OnClick=" VerEjecucion_OnChanged" Cssclass="btn glyphicon glyphicon-eye-open" />
                                        <asp:LinkButton ID="btnEditarEjecucion" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idEjecucion") %>' OnClick=" EditarEjecucion_OnChanged" Cssclass="btn glyphicon glyphicon-pencil" />
                                    </td>

                                    <td>
                                        <%# Eval("idEjecucion") %>
                                    
                                    </td>
                                    <td>
                                        <%# Eval("numeroReferencia") %>
                                    
                                    </td>

                                    <td>
                                        <%# Eval("idestado.descripcion") %>
                                    </td>
                                     <td>
                                        <%# Eval("idTipoTramite.nombreTramite") %>
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
                 <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                    <center>
                    <table class="table" style="max-width:664px;">
                        <tr style="padding:1px !important">
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbPrimero4" runat="server" CssClass="btn btn-primary" OnClick="lbPrimero4_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbAnterior4" runat="server" CssClass="btn btn-default" OnClick="lbAnterior4_Click"><span class="glyphicon glyphicon-backward"></asp:LinkButton>
                            </td>
                            <td style="padding:1px !important">
                                <asp:DataList ID="DataList2" runat="server"
                                    OnItemCommand="rptPaginacion4_ItemCommand"
                                    OnItemDataBound="rptPaginacion4_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaginacion4" runat="server" CssClass="btn btn-default"
                                            CommandArgument='<%# Eval("IndexPagina") %>' CommandName="nuevaPagina4"
                                            Text='<%# Eval("PaginaText") %>' ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbSiguiente4" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente4_Click"><span class="glyphicon glyphicon-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbUltimo4" CssClass="btn btn-primary" runat="server" OnClick="lbUltimo4_Click"><span class="glyphicon glyphicon-fast-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:Label ID="lblpagina4" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                                </div>
                   </ContentTemplate>
    </asp:UpdatePanel>
 </asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
 </asp:Content>
