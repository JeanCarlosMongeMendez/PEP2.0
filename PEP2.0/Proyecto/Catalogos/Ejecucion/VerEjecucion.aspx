<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="VerEjecucion.aspx.cs" Inherits="Proyecto.Catalogos.Ejecucion.VerEjecucion" MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>
    <div class="row">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <%-- titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                    <asp:Label ID="label" runat="server" Text="Eliminar Ejecución" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>
                <%-- fin titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12">
                    <asp:Label ID="label10" runat="server" Text="Número ejecución:" Font-Size="Large" ForeColor="Black"></asp:Label>
                    <asp:Label ID="lblNumeroEjecucion" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
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
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>
                <%-- tabla--%>
                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table class="table table-bordered">
                        <thead style="text-align: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th>Unidad</th>
                            </tr>
                        </thead>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtBuscarUnidad" runat="server" CssClass="form-control chat-input" AutoPostBack="true" placeholder="Filtro unidad" OnTextChanged="txtBuscarUnidad_TextChanged"></asp:TextBox></td>
                        </tr>

                        <asp:Repeater ID="rpUnidadesAsociadas" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
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
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>
                <%-- tabla--%>
                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table class="table table-bordered">
                        <thead style="text-align: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th>Número</th>
                                <th>Descripción</th>
                            </tr>
                        </thead>
                        <tr>
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

                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: left">
                    <asp:Label ID="label4" runat="server" Text="Tipo de tramite" Font-Size="Large" ForeColor="Black"></asp:Label>
                </div>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <asp:DropDownList AutoPostBack="true" ID="ddlTipoTramite" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                </div>
                <div class="form-group col-md-12 col-xs-12 col-sm-12 col-lg-12" id="divTipoTramiteOtro" runat="server" visible="false">
                    <asp:Label ID="label5" runat="server" Text="Ingrese tipo de tramite" Font-Size="Large" ForeColor="Black"></asp:Label>
                    <asp:TextBox ID="txtTipoTramiteOtro" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>

                <div class="form-group col-md-12 col-xs-12 col-sm-12 col-lg-12" id="div1" runat="server">
                    <asp:Label ID="label6" runat="server" Text="Número de referencia" Font-Size="Large" ForeColor="Black"></asp:Label>
                    <asp:TextBox ID="txtNumeroReferencia" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
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
                        <asp:TextBox class="form-control" ID="txtMonto" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 mt-2">
                    <br />
                </div>

                <%-- tabla--%>
                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table class="table table-bordered">
                        <thead style="text-align: center">
                            <tr style="text-align: center" class="btn-primary">
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

                <!-- Archivos -->
                <div class="col-md-12 col-sm-12 col-xs-12 ">
                    <div class="col-md-3 col-sm-3 col-xs-3">
                        <asp:Label ID="lblArchivosAsociados" runat="server" Text="Archivos asociados " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <asp:Repeater ID="rpArchivos" runat="server">
                            <HeaderTemplate>
                                <table id="tblArchivos" class="table table-hover table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Nombre del archivo</th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:LinkButton ID="btnVerArchivo" runat="server" Text='<%# Eval("nombreArchivo") %>' OnClick="btnVerArchivo_Click" CommandArgument='<%# Eval("idArchivoEjecucion")+","+Eval("nombreArchivo")+","+Eval("rutaArchivo") %>'></asp:LinkButton>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnVerArchivo" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </ItemTemplate>

                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:TextBox class="form-control" ID="txtArchivos" runat="server" TextMode="MultiLine" ReadOnly="true" Rows="6" Visible="false"></asp:TextBox>
                    </div>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 mt-2">
                    <br />
                </div>

                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" OnClick="btnCancelar_Click" />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>