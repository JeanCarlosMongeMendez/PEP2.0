<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarEjecucion.aspx.cs" Inherits="Proyecto.Catalogos.Ejecucion.AdministrarEjecucion" MaintainScrollPositionOnPostback="true" %>

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
                    <table id="tblUnidad" class="table table-bordered">
                        <thead style="text-align: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th></th>
                                <th>Unidad</th>
                                <th>Nombre</th>

                            </tr>
                        </thead>

                        <asp:Repeater ID="rpUnidadSelecionadas" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
                                        <asp:LinkButton ID="btnEliminarUnidad" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idUnidad") %>' OnClick="EliminarUnidadSeleccionada_OnChanged"><span class="btn glyphicon glyphicon-remove"></span></asp:LinkButton>

                                    </td>
                                    <td>
                                        <%# Eval("idUnidad") %>
                                    
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


                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Label ID="Label2" runat="server" Text="Partidas" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                    <asp:Button ID="ButtonAsociarPartida" runat="server" Text="Asociar" CssClass="btn btn-primary" OnClick="ButtonAsociarPartidas_Click" />
                </div>

                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table id="tblPartida" class="table table-bordered">
                        <thead style="text-align: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th></th>
                                <th>Número de partida</th>
                                <th>Descripción</th>

                            </tr>
                        </thead>

                        <asp:Repeater ID="rpPartidasSeleccionada" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
                                        <asp:LinkButton ID="btnEliminarPartida" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("numeroPartida") %>' OnClick="EliminarPartidaSeleccionada_OnChanged"><span class="btn glyphicon glyphicon-remove"></span></asp:LinkButton>

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
                                <asp:LinkButton ID="lbAnterior3" runat="server" CssClass="btn btn-default" OnClick="lbAnterior3_Click"><span class="glyphicon glyphicon-backward"></asp:LinkButton>
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
                                <asp:LinkButton ID="lbSiguiente3" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente3_Click"><span class="glyphicon glyphicon-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbUltimo3" CssClass="btn btn-primary" runat="server" OnClick="lbUltimo3_Click"><span class="glyphicon glyphicon-fast-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:Label ID="lblpagina3" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                </div>

                <br />
                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Label ID="Label3" runat="server" Text="Tipo de tramite" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                    <asp:DropDownList AutoPostBack="true" ID="DDLTipoTramite" runat="server" CssClass="form-control" OnSelectedIndexChanged="TipoTramites_OnChanged"></asp:DropDownList>
                    <br />
                    <br />
                    <asp:TextBox class="form-control" ID="descripcionOtroTipoTramite" runat="server"  ></asp:TextBox>

                </div>
                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Label ID="Label6" runat="server" Text="Número de referencia" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                    <asp:TextBox class="form-control" ID="numeroReferencia" runat="server" Enabled="true"></asp:TextBox>

                </div>
                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Label ID="Label5" runat="server" Text="Monto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                    ₡
                    <asp:TextBox class="form-control" ID="txtMontoIngresar" runat="server" Enabled="true"></asp:TextBox>

                </div>
                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Button ID="ButtonRepartir" runat="server" Text="Repartir Gastos" CssClass="btn btn-primary" OnClick="ButtonRepartirPartidas_Click" />
                </div>
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                            <table id="tblRepartirGastos" class="table table-bordered">
                                <thead style="text-align: center">
                                    <tr style="text-align: center" class="btn-primary">
                                        <th></th>
                                        <th>Unidad</th>
                                        <th>Número Partida</th>
                                        <th>monto</th>
                                    </tr>
                                </thead>

                                <asp:Repeater ID="Repeater5" runat="server">
                                    <HeaderTemplate>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr style="text-align: center">
                                            <td>
                                                <%--                                       <asp:LinkButton ID="btnEliminarPartida" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("numeroPartida") %>' OnClick="EliminarPartidaSeleccionada_OnChanged"><span class="btn glyphicon glyphicon-remove"></span></asp:LinkButton>--%>

                                            </td>
                                            <td>
                                                <%--                                        <%# Eval("numeroPartida") %>--%>
                                    
                                            </td>

                                            <td>
                                                <%--                                       <%# Eval("descripcionPartida") %>--%>
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

        </ContentTemplate>
    </asp:UpdatePanel>



    <!-- Modal elegir partida -->
    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
        <ContentTemplate>
            <div id="modalElegirPartidas" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Elegir partidas</h4>
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
                                        <asp:Label ID="label1" runat="server" Text="Seleccione las partidas que desea" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                </div>
                                <%-- tabla--%>
                                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                    <table id="tblPartidas" class="table table-bordered">
                                        <thead style="text-align: center">
                                            <tr style="text-align: center" class="btn-primary">
                                                <th></th>
                                                <th>Número de partida</th>
                                                <th>Descripción </th>

                                            </tr>
                                        </thead>

                                        <asp:Repeater ID="rpElegirPartida" runat="server">
                                            <HeaderTemplate>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr style="text-align: center">

                                                    <td>
                                                        <asp:LinkButton ID="btnAnadirNuevaPartida" runat="server" ToolTip="Seleccionar" CommandArgument='<%# Eval("numeroPartida") %>' OnClick="btnAnadirNuevaPartida_Click"><span class="btn  glyphicon glyphicon-ok-sign"></span></asp:LinkButton>

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
                                <asp:LinkButton ID="lbPrimero4" runat="server" CssClass="btn btn-primary" OnClick="lbPrimero4_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbAnterior4" runat="server" CssClass="btn btn-default" OnClick="lbAnterior4_Click"><span class="glyphicon glyphicon-backward"></asp:LinkButton>
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


                            </div>
                            <div class="modal-footer" style="text-align: center">

                                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

        <!-- Modal repartir partida -->
    <asp:UpdatePanel ID="UpdatePane34" runat="server">
        <ContentTemplate>
            <div id="modalRepartirPartidas" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            
                             <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Label ID="Label7" runat="server" Text="Monto pendiente de repartir" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                   ₡  <asp:Label ID="montoRepartir" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    
                </div>
                        </div>
                        <div class="modal-body">
                            <%-- campos a llenar --%>
                            <div class="row">
                                  <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                    <table id="tblPartidaUnidad" class="table table-bordered">
                                        <thead style="text-align: center">
                                            <tr style="text-align: center" class="btn-primary">
                                                <th></th>
                                                <th>Unidad</th>
                                                <th>Número de partida </th>
                                                <th>Saldo </th>
                                                <th>Monto </th>
                                            </tr>
                                        </thead>

                                        <asp:Repeater ID="rpUnidadPartida" runat="server">
                                            <HeaderTemplate>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr style="text-align: center">

                                                    <td>
                                                       <asp:LinkButton ID="btnAlmacenarUnidadPartida" runat="server" ToolTip="Almacenar" CommandArgument='<%# Eval("idPartida") %>'  OnClick="btnAlmacenarUnidadPartida_Click"><span class="btn  glyphicon glyphicon-ok-sign"></span></asp:LinkButton>

                                                    </td>
                                                    <td>
                                                      
                                                        <%# Eval("idUnidad") %>
                                    
                                                    </td>
                                    
                                                   
                                                    <td>
                                                    
                                                        <%# Eval("numeroPartida") %>
                                    
                                                    </td>
                                                     <td>

                                                         <%# Eval("MontoDisponible") %>
                                    
                                                     </td>
                                                   
                                                    <td>
                                                         ₡ <asp:TextBox class="form-control" ID="txtMonto" runat="server" ReadOnly="True"></asp:TextBox>
                                   
                                                     </td>

                                                </tr>

                                            </ItemTemplate>

                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>

                                </div>
                            <div class="modal-footer" style="text-align: center">

                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                            </div>

                    </div>
                </div>
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
                                            <th>Unidad</th>
                                            <th>Nombre</th>

                                        </tr>
                                    </thead>

                                    <asp:Repeater ID="Repeater1" runat="server">
                                        <HeaderTemplate>
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <tr style="text-align: center">
                                                <td>
                                                    <asp:LinkButton ID="btnAnadirPartida" runat="server" ToolTip="Seleccionar" CommandArgument='<%# Eval("idUnidad") %>' OnClick="btnUnidadSeleccionada_Click"><span class="btn  glyphicon glyphicon-ok"></span></asp:LinkButton>

                                                </td>
                                                <td>
                                                    <%# Eval("idUnidad") %>
                                    
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
                        </div>
                        <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                    <center>
                    <table class="table" style="max-width:664px;">
                        <tr style="padding:1px !important">
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbPrimero1" runat="server" CssClass="btn btn-primary" OnClick="lbPrimero4_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbAnterior1" runat="server" CssClass="btn btn-default" OnClick="lbAnterior4_Click"><span class="glyphicon glyphicon-backward"></asp:LinkButton>
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
                                <asp:LinkButton ID="lbSiguiente1" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente4_Click"><span class="glyphicon glyphicon-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbUltimo1" CssClass="btn btn-primary" runat="server" OnClick="lbUltimo4_Click"><span class="glyphicon glyphicon-fast-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:Label ID="lblpagina1" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
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
        function activarModalElegirPartidas() {
            $('#modalElegirPartidas').modal('show');
        };

         function activarModalRepartirPartidas() {
            $('#modalRepartirPartidas').modal('show');
        };


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
