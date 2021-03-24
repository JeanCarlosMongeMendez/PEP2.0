<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarEjecuciones.aspx.cs" Inherits="Proyecto.Catalogos.Ejecucion.AdministrarEjecuciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>
    <div class="row">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <%-- titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                    <asp:Label ID="label" runat="server" Text="Ejecución" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>
                <%-- fin titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>
                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Button ID="btnNuevaEjecucion" runat="server" Text="Nueva Ejecución" CssClass="btn btn-primary" OnClick="btnNuevaEjecucion_Click" />
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
                                <th>Número de Ejecución</th>
                                <th>Número de Referencia</th>
                                <th>Estado</th>
                                <th>Tipo de Tramite</th>
                                <th>Monto</th>
                                <th>Realizado por</th>
                                <th>Ingresado</th>
                            </tr>
                        </thead>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txtBuscarNumeroEjecucion" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" placeholder="Filtro número ejecución"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtBuscarNumeroReferencia" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" placeholder="Filtro número referencia"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtBuscarEstado" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" placeholder="Filtro estado"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtBuscarTipoTramite" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" placeholder="Filtro tipo de tramite"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtBuscarMonto" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" placeholder="Filtro monto"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtBuscarRealizadoPor" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" placeholder="Filtro realizado por"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtBuscarIngresado" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" placeholder="Filtro ingresado"></asp:TextBox></td>
                        </tr>

                        <asp:Repeater ID="rpEjecuciones" runat="server" OnItemDataBound="rpEjecucion_ItemDataBound">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr >
                                        
                                    <td>
                                        <asp:LinkButton ID="btnVer" runat="server" ToolTip="Ver" CommandArgument='<%# Eval("idEjecucion") %>' CssClass="btn glyphicon glyphicon-eye-open" OnClick="btnVer_Click" />
                                        <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idEjecucion") %>' CssClass="btn glyphicon glyphicon-trash" OnClick="btnEliminar_Click" Visible='<%# Convert.ToString(Eval("estadoEjecucion.descripcion"))=="Aprobado"?false:true  %>'/>
                                        <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idEjecucion") %>' CssClass="btn glyphicon glyphicon-pencil" OnClick="btnEditar_Click" Visible='<%# Convert.ToString(Eval("estadoEjecucion.descripcion"))=="Aprobado"?false:true  %>'/>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnAprobar" runat="server" ToolTip="Aprobar" Text="Aprobar" CssClass="btn btn-default" CommandArgument='<%# Eval("idEjecucion") %>' OnClick="btnAprobar_Click" Visible='<%# Convert.ToString(Eval("estadoEjecucion.descripcion"))=="Guardado"?true: (Convert.ToString(Eval("estadoEjecucion.descripcion"))=="Comprometer"?true:false)  %>'/>
                                        <asp:LinkButton ID="btnComprometer" runat="server" ToolTip="Comprometer" Text="Comprometer" CssClass="btn btn-default" CommandArgument='<%# Eval("idEjecucion") %>' OnClick="btnComprometer_Click" Visible='<%# Convert.ToString(Eval("estadoEjecucion.descripcion"))=="Guardado"?true:false  %>'/>
                                    </td>
                                    <td>
                                        <%# Eval("idEjecucion") %>
                                    
                                    </td>
                                    <td>
                                        <%# Eval("numeroReferencia") %>
                                    
                                    </td>

                                    <td style='<%# Convert.ToString(Eval("estadoEjecucion.descripcion"))=="Guardado"?"background-color:#E74C3C; color: #fff;":(Convert.ToString(Eval("estadoEjecucion.descripcion"))=="Comprometer")?"background-color:#D4AC0D":"background-color:white" %>'>
                                        <%# Eval("estadoEjecucion.descripcion") %>
                                    </td>
                                    <td>
                                        <%# Eval("tipoTramite.nombreTramite") %>
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
        </asp:UpdatePanel>
    </div>


    <!-- Modal comproter -->
    <div id="modalComprometer" class="modal fade" role="alertdialog">
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
                                    <asp:Label ID="lblConfirmarComprometer" runat="server" Text="¿Seguro o segura que desea comprometer la ejecución?" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <%-- fin campos a llenar --%>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnSiComprometer" runat="server" Text="Si" CssClass="btn btn-primary" OnClick="btnSiComprometer_Click"/>
                            <button type="button" class="btn btn-default" data-dismiss="modal">No</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- fin Modal comproter-->

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
        function activarModalComprometer() {
            $('#modalComprometer').modal('show');
        };

        function cerrarModalComprometer() {
            $('#modalComprometer').modal('hide');
        };
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
