<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarCajaChica.aspx.cs" Inherits="Proyecto.Catalogos.CajaChica.AdministrarCajaChica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:scriptmanager id="ScriptManager1" runat="server" enablecdn="true"></asp:scriptmanager>
    <div class="row">
        <asp:updatepanel id="UpdatePanel1" runat="server">
            <ContentTemplate>
                 <%-- titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                    <asp:Label ID="label" runat="server" Text="Administrar Caja Chica" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>
                <%-- fin titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>
                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Button ID="btnNuevaCajaChica" runat="server" Text="Nueva Caja Chica" CssClass="btn btn-primary" OnClick="btnNuevaCajaChica_Click" />
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                    <asp:Label ID="label1" runat="server" Text="Seleccione un periodo" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <asp:DropDownList AutoPostBack="true" ID="ddlPeriodos" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPeriodos_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                    <asp:Label ID="label2" runat="server" Text="Seleccione un proyecto" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <asp:DropDownList AutoPostBack="true" ID="ddlProyectos" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlProyectos_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>
                 <%-- tabla--%>
                 <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table id="tblUnidad" class="table table-bordered">
                        <thead style="text-align: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th></th>
                                <th></th>
                                <th style="display:none">Número de Caja Chica</th> 
                                <th>Número Solicitud de Caja Chica</th> 
                                <th>Estado</th>
                                <th>Monto</th>
                                <th>Realizado por</th>
                                <th>Ingresado</th>
                                
                            </tr>
                        </thead>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txtBuscarNumeroCajaChica" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" placeholder="Filtro número Caja Chica"></asp:TextBox></td>
                             <td>
                                <asp:TextBox ID="txtBuscarEstado" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" placeholder="Filtro estado"></asp:TextBox></td>
                            
                            <td>
                                <asp:TextBox ID="txtBuscarMonto" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" placeholder="Filtro monto"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtBuscarRealizadoPor" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" placeholder="Filtro realizado por"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtBuscarIngresado" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" placeholder="Filtro ingresado"></asp:TextBox></td>
                        </tr>

                        <asp:Repeater ID="rpCajaChica" runat="server" OnItemDataBound="rpCajaChica_ItemDataBound">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr >
                                        
                                    <td>
                                        <asp:LinkButton ID="btnVer" runat="server" ToolTip="Ver" CommandArgument='<%# Eval("idCajaChica") %>' CssClass="btn glyphicon glyphicon-eye-open" OnClick="btnVer_Click" />
                                        <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idCajaChica") %>' CssClass="btn glyphicon glyphicon-trash" OnClick="btnEliminar_Click" Visible='<%# Convert.ToString(Eval("idEstadoCajaChica.descripcion"))=="Aprobado"?false:true  %>'/>
                                        <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idCajaChica") %>' CssClass="btn glyphicon glyphicon-pencil" OnClick="btnEditar_Click" Visible='<%# Convert.ToString(Eval("idEstadoCajaChica.descripcion"))=="Aprobado"?false:true  %>'/>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnAprobar" runat="server" ToolTip="Aprobar" Text="Aprobar" CssClass="btn btn-default" CommandArgument='<%# Eval("idCajaChica") %>' OnClick="btnAprobar_Click" Visible='<%# Convert.ToString(Eval("idEstadoCajaChica.descripcion"))=="Guardado"?true: (Convert.ToString(Eval("estadoEjecucion.descripcion"))!="Rechazado"?true:false)  %>'/>
                                        <%--<asp:LinkButton ID="btnRechazar" runat="server" ToolTip="Rechazar" Text="Rechazar" CssClass="btn btn-default" CommandArgument='<%# Eval("idEjecucion") %>' OnClick="btnRechazar_Click" Visible='<%# Convert.ToString(Eval("estadoEjecucion.descripcion"))=="Guardado"?true:false  %>'/>--%>
                                    </td>
                                    <td >
                                        <%# Eval("numeroCajaChica") %>
                                    
                                    </td>
                                    <td style ="display:none">
                                        <%# Eval("idCajaChica") %>
                                    
                                    </td>
                                   

                                    <td style='<%# Convert.ToString(Eval("idEstadoCajaChica.descripcion"))=="Guardado"?"background-color:#E74C3C; color: #fff;":(Convert.ToString(Eval("idEstadoCajaChica.descripcion"))=="Rechazado")?"background-color:#D4AC0D":"background-color:white" %>'>
                                        <%# Eval("idEstadoCajaChica.descripcion") %>
                                    </td>
                                  
                                    <td>₡ <%# Eval("monto") %>
                                    </td>
                                    <td>
                                        <%# Eval("realizadoPor") %>
                                    </td>
                                    <td>
                                        <%# Convert.ToDateTime(Eval("fecha")).ToShortDateString() %>
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
        </asp:updatepanel>

    </div>
     <!-- Modal Aprobar -->
    <div id="modalAprobar" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg">

            <!-- Modal content-->
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Repartir gastos</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <%-- campos a llenar --%>
                                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                                    <asp:Label ID="lblConfirmarAprobar" runat="server" Text="¿Seguro o segura que desea aprobar la ejecución?" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <%-- fin campos a llenar --%>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnSiAprobar" runat="server" Text="Si" CssClass="btn btn-primary" OnClick="btnSiAprobar_Click"/>
                            <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- fin Modal Aprobar-->

    <script type="text/javascript">
       
        function activarModalAprobar() {
            $('#modalAprobar').modal('show');
        };

        function cerrarModalAprobar() {
            $('#modalAprobar').modal('hide');
        };
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
