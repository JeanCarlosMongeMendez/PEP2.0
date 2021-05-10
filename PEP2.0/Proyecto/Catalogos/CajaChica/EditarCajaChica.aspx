<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="EditarCajaChica.aspx.cs" Inherits="Proyecto.Catalogos.CajaChica.EditarCajaChica" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>
    <div class="row">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <%-- titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                    <asp:Label ID="label" runat="server" Text="Editar Caja Chica" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>
                <%-- fin titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12">
                    <asp:Label ID="label10" runat="server" Text="Número Caja Chica:" Font-Size="Large" ForeColor="Black"></asp:Label>
                    <asp:Label ID="lblNumeroCajaChica" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12">
                    <asp:Label ID="label1" runat="server" Text="Período seleccionado:" Font-Size="Large" ForeColor="Black"></asp:Label>
                    <asp:Label ID="lblPeriodo" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12">
                    <asp:Label ID="label2" runat="server" Text="Proyecto seleccionado:" Font-Size="Large" ForeColor="Black"></asp:Label>
                    <asp:Label ID="lblProyecto" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 mt-2">
                    <hr />
                </div>
                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Label ID="Unidades" runat="server" Text="Unidades" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                    <asp:Button ID="btnAsociarUnidades" runat="server" Text="Asociar" CssClass="btn btn-primary" OnClick="btnAsociarUnidades_Click" />
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>
                <%-- tabla--%>
                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table class="table table-bordered">
                        <thead style="text-align: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th></th>
                                <th>Unidad</th>
                            </tr>
                        </thead>
                        <tr>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txtBuscarUnidad" runat="server" CssClass="form-control chat-input" AutoPostBack="true" placeholder="Filtro unidad" OnTextChanged="txtBuscarUnidad_TextChanged"></asp:TextBox></td>
                        </tr>

                        <asp:Repeater ID="rpUnidadesAsociadas" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
                                        <asp:LinkButton ID="btnDesasociarUnidad" runat="server" ToolTip="Desasociar" CommandArgument='<%# Eval("idUnidad") %>' CssClass="btn glyphicon glyphicon-remove-circle" OnClick="btnDesasociarUnidad_Click" />
                                    </td>
                                    <td>
                                        <%# Eval("nombreUnidad") %>
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
                                <asp:LinkButton ID="lbAnterior" runat="server" CssClass="btn btn-default" OnClick="lbAnterior_Click"><span class="glyphicon glyphicon-backward"></span></asp:LinkButton>
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
                                <asp:LinkButton ID="lbSiguiente" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente_Click"><span class="glyphicon glyphicon-forward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbUltimo" CssClass="btn btn-primary" runat="server" OnClick="lbUltimo_Click"><span class="glyphicon glyphicon-fast-forward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:Label ID="lblpagina" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                </div>
                <%-- fin de tabla--%>

                <div class="col-md-12 col-xs-12 col-sm-12 mt-2">
                    <hr />
                </div>
                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Label ID="Label3" runat="server" Text="Partidas" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                    <asp:Button ID="btnAsociarPartidas" runat="server" Text="Asociar" CssClass="btn btn-primary" OnClick="btnAsociarPartidas_Click" />
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>
                <%-- tabla--%>
                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table class="table table-bordered">
                        <thead style="text-align: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th></th>
                                <th>Número</th>
                                <th>Descripción</th>
                            </tr>
                        </thead>
                        <tr>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txtBuscarNumeroPartida" runat="server" CssClass="form-control chat-input" AutoPostBack="true" placeholder="Filtro número" OnTextChanged="txtBuscarPartidasAsocidas_TextChanged"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtBuscarDescripcionPartida" runat="server" CssClass="form-control chat-input" AutoPostBack="true" placeholder="Filtro descripción" OnTextChanged="txtBuscarPartidasAsocidas_TextChanged"></asp:TextBox></td>
                        </tr>

                        <asp:Repeater ID="rpPartidasAsociadas" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
                                        <asp:LinkButton ID="btnDesasociarPartida" runat="server" ToolTip="Desasociar" CommandArgument='<%# Eval("idPartida") %>' CssClass="btn glyphicon glyphicon-remove-circle" OnClick="btnDesasociarPartida_Click" />
                                    </td>
                                    <td>
                                        <%# Eval("numeroPartida") %>
                                    </td>
                                    <td>
                                        <%# Eval("descripcionPartida") %>
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
                                <asp:LinkButton ID="lbAnterior2" runat="server" CssClass="btn btn-default" OnClick="lbAnterior2_Click"><span class="glyphicon glyphicon-backward"></span></asp:LinkButton>
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
                                <asp:LinkButton ID="lbSiguiente2" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente2_Click"><span class="glyphicon glyphicon-forward"></span></asp:LinkButton>
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
                <%-- fin de tabla--%>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>

             

                <div class="form-group col-md-12 col-xs-12 col-sm-12 col-lg-12" id="div1" runat="server">
                    <asp:Label ID="label6" runat="server" Text="Descripción" Font-Size="Large" ForeColor="Black"></asp:Label>
                    <asp:TextBox ID="txtComentario" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12" style="text-align: left">
                    <asp:Label ID="label7" runat="server" Text="Monto" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>
                <div class="col-md-4 col-xs-4 col-sm-4">
                    <div class="input-group">
                        <span class="input-group-addon">₡</span>
                        <asp:TextBox class="form-control" ID="txtMonto" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 mt-2">
                    <br />
                </div>
                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Button ID="btnRepartirGastos" runat="server" Text="Repartir gastos" CssClass="btn btn-primary" OnClick="btnRepartirGastos_Click" />
                </div>

                <%-- tabla--%>
                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table class="table table-bordered">
                        <thead style="text-align: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th></th>
                                <th>Unidad</th>
                                <th>Número Partida</th>
                                <th>Monto</th>
                            </tr>
                        </thead>
                        <tr>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txtBuscarUnidadPartidaUnidadAsociada" runat="server" CssClass="form-control chat-input" AutoPostBack="true" placeholder="Filtro unidad" OnTextChanged="txtBuscarPartidaUnidadAsociada_TextChanged"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBuscarPartidaPartidaUnidadAsociada" runat="server" CssClass="form-control chat-input" AutoPostBack="true" placeholder="Filtro partida" OnTextChanged="txtBuscarPartidaUnidadAsociada_TextChanged"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>

                        <asp:Repeater ID="rpPartidaUnidadAsociadas" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
                                        <asp:LinkButton ID="btnEliminarUnidadPartida" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idPartida") +"::"+Eval("idUnidad") %>' CssClass="btn glyphicon glyphicon-remove" OnClick="btnEliminarUnidadPartida_Click" />
                                    </td>
                                    <td>
                                        <%# Eval("nombreUnidad") %>
                                    </td>
                                    <td>
                                        <%# Eval("numeroPartida") %>
                                    </td>
                                    <td>
                                        <%# Eval("Monto") %>
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
                                <asp:LinkButton ID="lbAnterior4" runat="server" CssClass="btn btn-default" OnClick="lbAnterior4_Click"><span class="glyphicon glyphicon-backward"></span></asp:LinkButton>
                            </td>
                            <td style="padding:1px !important">
                                <asp:DataList ID="rptPaginacion4" runat="server"
                                    OnItemCommand="rptPaginacion4_ItemCommand"
                                    OnItemDataBound="rptPaginacion4_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaginacion4" runat="server" CssClass="btn btn-default"
                                            CommandArgument='<%# Eval("IndexPagina") %>' CommandName="nuevaPagina"
                                            Text='<%# Eval("PaginaText") %>' ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbSiguiente4" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente4_Click"><span class="glyphicon glyphicon-forward"></span></asp:LinkButton>
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
                <%-- fin de tabla--%>

                <div class="col-md-12 col-xs-12 col-sm-12 mt-2">
                    <br />
                </div>

                

                <div class="col-md-12 col-xs-12 col-sm-12 mt-2">
                    <br />
                </div>

                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-primary" OnClick="btnEditar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" OnClick="btnCancelar_Click" />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- Modal Unidades -->
    <div id="modalUnidades" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg">

            <!-- Modal content-->
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Seleccionar Unidades</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <%-- campos a llenar --%>
                                <%-- tabla--%>
                                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                    <table class="table table-bordered">
                                        <thead style="text-align: center">
                                            <tr style="text-align: center" class="btn-primary">
                                                <th></th>
                                                <th>Unidad</th>
                                            </tr>
                                        </thead>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:TextBox ID="txtBuscarUnidadSinAsociar" runat="server" CssClass="form-control chat-input" AutoPostBack="true" placeholder="Filtro unidad" OnTextChanged="txtBuscarUnidadSinAsociar_TextChanged"></asp:TextBox></td>
                                        </tr>

                                        <asp:Repeater ID="rpUnidadesSinAsociar" runat="server">
                                            <HeaderTemplate>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr style="text-align: center">
                                                    <td>
                                                        <asp:LinkButton ID="btnSeleccionar" runat="server" ToolTip="Seleccionar" CommandArgument='<%# Eval("idUnidad") %>' CssClass="btn glyphicon glyphicon-ok" OnClick="btnSeleccionar_Click" />
                                                    </td>
                                                    <td>
                                                        <%# Eval("nombreUnidad") %>
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
                                <asp:LinkButton ID="lbPrimero1" runat="server" CssClass="btn btn-primary" OnClick="lbPrimero1_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbAnterior1" runat="server" CssClass="btn btn-default" OnClick="lbAnterior1_Click"><span class="glyphicon glyphicon-backward"></span></asp:LinkButton>
                            </td>
                            <td style="padding:1px !important">
                                <asp:DataList ID="rptPaginacion1" runat="server"
                                    OnItemCommand="rptPaginacion1_ItemCommand"
                                    OnItemDataBound="rptPaginacion1_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaginacion1" runat="server" CssClass="btn btn-default"
                                            CommandArgument='<%# Eval("IndexPagina") %>' CommandName="nuevaPagina"
                                            Text='<%# Eval("PaginaText") %>' ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbSiguiente1" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente1_Click"><span class="glyphicon glyphicon-forward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbUltimo1" CssClass="btn btn-primary" runat="server" OnClick="lbUltimo1_Click"><span class="glyphicon glyphicon-fast-forward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:Label ID="lblpagina1" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                                </div>
                                <%-- fin de tabla--%>
                                <%-- fin campos a llenar --%>
                            </div>
                            <div class="modal-footer" style="text-align: center">
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- fin Modal Unidades -->

    <!-- Modal Partidas -->
    <div id="modalPartidas" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg">

            <!-- Modal content-->
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Seleccionar Partidas</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <%-- campos a llenar --%>
                                <%-- tabla--%>
                                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                    <table class="table table-bordered">
                                        <thead style="text-align: center">
                                            <tr style="text-align: center" class="btn-primary">
                                                <th></th>
                                                <th>Número</th>
                                                <th>Descripción</th>
                                            </tr>
                                        </thead>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:TextBox ID="txtBuscarNumeroPartidaSinAsociar" runat="server" CssClass="form-control chat-input" AutoPostBack="true" placeholder="Filtro número" OnTextChanged="txtBuscarPartidaSinAsociar_TextChanged"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtBuscarDescripcionPartidaSinAsociar" runat="server" CssClass="form-control chat-input" AutoPostBack="true" placeholder="Filtro descripción" OnTextChanged="txtBuscarPartidaSinAsociar_TextChanged"></asp:TextBox></td>
                                        </tr>

                                        <asp:Repeater ID="rpPartidasSinAsociar" runat="server">
                                            <HeaderTemplate>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr style="text-align: center">
                                                    <td>
                                                        <asp:LinkButton ID="btnSeleccionarPartida" runat="server" ToolTip="Seleccionar" CommandArgument='<%# Eval("idPartida") %>' CssClass="btn glyphicon glyphicon-ok" OnClick="btnSeleccionarPartida_Click" />
                                                    </td>
                                                    <td>
                                                        <%# Eval("numeroPartida") %>
                                                    </td>
                                                    <td>
                                                        <%# Eval("descripcionPartida") %>
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
                                <asp:LinkButton ID="lbPrimero3" runat="server" CssClass="btn btn-primary" OnClick="lbPrimero3_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbAnterior3" runat="server" CssClass="btn btn-default" OnClick="lbAnterior3_Click"><span class="glyphicon glyphicon-backward"></span></asp:LinkButton>
                            </td>
                            <td style="padding:1px !important">
                                <asp:DataList ID="rptPaginacion3" runat="server"
                                    OnItemCommand="rptPaginacion3_ItemCommand"
                                    OnItemDataBound="rptPaginacion3_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaginacion3" runat="server" CssClass="btn btn-default"
                                            CommandArgument='<%# Eval("IndexPagina") %>' CommandName="nuevaPagina"
                                            Text='<%# Eval("PaginaText") %>' ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbSiguiente3" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente3_Click"><span class="glyphicon glyphicon-forward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbUltimo3" CssClass="btn btn-primary" runat="server" OnClick="lbUltimo3_Click"><span class="glyphicon glyphicon-fast-forward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:Label ID="lblpagina3" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                                </div>
                                <%-- fin de tabla--%>
                                <%-- fin campos a llenar --%>
                            </div>
                            <div class="modal-footer" style="text-align: center">
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- fin Modal partidas -->

    <!-- Modal Partidas unidades -->
    <div id="modalUnidadesPartidas" class="modal fade" role="alertdialog">
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
                                    <asp:Label ID="Label8" runat="server" Text="Monto pendiente de repartir" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    ₡ 
                                    <asp:Label ID="lblMontoRepartir" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>
                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: left">
                                    <asp:Label ID="label9" runat="server" Text="Unidad(es)" Font-Size="Large" ForeColor="Black"></asp:Label>
                                </div>
                                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                                    <asp:DropDownList AutoPostBack="true" ID="ddlUnidades" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlUnidades_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <hr />
                                </div>

                                <%-- tabla--%>
                                <table id="tblPartidaUnidad" class="table table-bordered">
                                    <thead style="text-align: center">
                                        <tr style="text-align: center" class="btn-primary">
                                            <th></th>
                                            <th>Número de partida </th>
                                            <th>Saldo </th>
                                            <th>Monto </th>
                                        </tr>
                                    </thead>

                                    <asp:Repeater ID="rpPartidaUnidadSinAsociar" runat="server">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="text-align: center">
                                                <td>
                                                    <asp:LinkButton ID="btnSeleccionarUnidadPartida" runat="server" ToolTip="Seleccionar" CommandArgument='<%# Eval("idPartida") +"::"+Eval("idUnidad") %>' CssClass="btn glyphicon glyphicon-ok" OnClick="btnSeleccionarUnidadPartida_Click" />
                                                </td>
                                                <td>
                                                    <%# Eval("numeroPartida") %>
                                                </td>
                                                <td>
                                                    <%# Eval("MontoDisponible") %>
                                                </td>
                                                <td>
                                                    <div class="input-group">
                                                        <span class="input-group-addon">₡</span>
                                                        <asp:TextBox class="form-control" ID="txtMontoAsociar" runat="server" Text='<%# Eval("monto") %>'></asp:TextBox>
                                                    </div>
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
                                <asp:LinkButton ID="lbPrimero5" runat="server" CssClass="btn btn-primary" OnClick="lbPrimero5_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbAnterior5" runat="server" CssClass="btn btn-default" OnClick="lbAnterior5_Click"><span class="glyphicon glyphicon-backward"></asp:LinkButton>
                            </td>
                            <td style="padding:1px !important">
                                <asp:DataList ID="rptPaginacion5" runat="server"
                                    OnItemCommand="rptPaginacion5_ItemCommand"
                                    OnItemDataBound="rptPaginacion5_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaginacion5" runat="server" CssClass="btn btn-default"
                                            CommandArgument='<%# Eval("IndexPagina") %>' CommandName="nuevaPagina"
                                            Text='<%# Eval("PaginaText") %>' ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbSiguiente5" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente5_Click"><span class="glyphicon glyphicon-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbUltimo5" CssClass="btn btn-primary" runat="server" OnClick="lbUltimo5_Click"><span class="glyphicon glyphicon-fast-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:Label ID="lblpagina5" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                            </div>
                            <%-- fin tabla --%>
                        </div>
                        <%-- fin campos a llenar --%>
                        <div class="modal-footer" style="text-align: center">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- fin Modal partidas  unidades-->

    <script type="text/javascript">
        function activarModalUnidades() {
            $('#modalUnidades').modal('show');
        };

        function activarModalPartidas() {
            $('#modalPartidas').modal('show');
        };

        function activarModalUnidadesPartidas() {
            $('#modalUnidadesPartidas').modal('show');
        };
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
