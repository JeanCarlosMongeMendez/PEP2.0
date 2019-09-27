<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarPartidas.aspx.cs" Inherits="Proyecto.Catalogos.Partidas.AdministrarPartidas" MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
     <ContentTemplate>
            <div class="row">
                <%-- titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12">
                    <center>
                        <asp:Label ID="label" runat="server" Text="Administrar partidas" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </center>
                </div>
                <%-- fin titulo pantalla --%>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>


                <div class="col-md-6 col-xs-6 col-sm-6">
                    <h4>Periodo</h4>
                    <asp:DropDownList ID="ddlPeriodo" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged"></asp:DropDownList>
                </div>

                 <div class="col-md-6 col-xs-6 col-sm-6">
                    <h4>Pasar Partidas</h4>
                    <asp:Button ID="btnPartidas" runat="server" Text="Pasar Partidas" CssClass="btn btn-primary boton-nuevo" OnClick="btnPartidas_Click" />
                </div>
                
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <br />
                </div>


                 <%-- tabla--%>

                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table class="table table-bordered">
                        <thead style="text-align: center !important; align-content: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th></th>
                                <th>Descripción de Partida</th>
                                <th>Número de Partida</th>
                            </tr>
                        </thead>
                        <tr>
                            <td>
                                <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary" OnClick="btnFiltrar_Click"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton></td>
                            <td> 
                                <asp:TextBox ID="txtBuscarDesc" runat="server" CssClass="form-control chat-input" placeholder="filtro descripción" AutoPostBack="true" OnTextChanged="txtBuscarDesc_TextChanged"></asp:TextBox>
                            </td>
                            <td></td>
                           
                        </tr>
                        <asp:Repeater ID="rpPartidas" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
                                        <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idPartida") %>' OnClick="btnEditar_Click"><span class="btn glyphicon glyphicon-pencil"></span></asp:LinkButton>
                                        <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idPartida") %>' OnClick="btnEliminar_Click"><span class="btn glyphicon glyphicon-trash"></span></asp:LinkButton>
                                    </td>
                                    <td>
                                        <%# Eval("descripcionPartida") %>
                                    </td>
                                    <td> 
                                        <%# Eval("numeroPartida") %>
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

              

                <%-- fin tabla--%>

                <div class="col-md-2 col-xs-2 col-sm-2 col-md-offset-10 col-xs-offset-10 col-sm-offset-10">
                    <asp:Button ID="btnNuevaPartida" runat="server" Text="Nueva" CssClass="btn btn-primary boton-nuevo" OnClick="btnNuevaPartida_Click" />
                </div>

            </div>
      

    <!-- Modal nueva partida -->
            <div id="modalNuevaPartida" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Nueva Partida</h4>
                        </div>
                        <div class="modal-body">

                            <div class="row">
                                <%-- campos a llenar --%>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="Label16" runat="server" Text="Periodo" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:DropDownList ID="ddlPeriodoModal" class="btn btn-default dropdown-toggle" runat="server" OnSelectedIndexChanged="ddlPeriodoModal_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="Label1" runat="server" Text="Tipo o Padre" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:DropDownList ID="ddlPartidasPadre" class="btn btn-default dropdown-toggle" runat="server" ></asp:DropDownList>
                                    </div>
                                </div>

                                
                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="Label3" runat="server" Text="Número de Partida" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                            <asp:TextBox class="form-control" ID="txtNumeroPartidas" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                
                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                 <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="Label2" runat="server" Text="Descripción de Partida" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                     <div class="col-md-4 col-xs-4 col-sm-4">
                                         <asp:TextBox class="form-control" TextMode="multiline" Rows="5" ID="txtDescripcionPartida" runat="server"></asp:TextBox>
                                     </div>
                                </div>

                                <%-- fin campos a llenar --%>
                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnNuevaPartidaModal" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnNuevaPartidaModal_Click"/>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
    <!-- Fin modal nueva partida -->








     <!-- Modal modificar partida -->
            <div id="modalModificarPartida" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Modificar Partida</h4>
                        </div>
                        <div class="modal-body">

                            <div class="row">
                                <%-- campos a llenar --%>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="Label4" runat="server" Text="Periodo" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="lbPeriodoModalModificar" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="Label5" runat="server" Text="Tipo o Padre" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="lbPartidaPadreModalModificar" runat="server" Text="Partida Padre" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                
                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="Label6" runat="server" Text="Número de Partida" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                            <asp:TextBox class="form-control" ID="txtNumeroPartidasModalModificar" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                
                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                 <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="Label7" runat="server" Text="Descripción de Partida" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                     <div class="col-md-4 col-xs-4 col-sm-4">
                                         <asp:TextBox class="form-control" TextMode="multiline" Rows="5" ID="txtDescripcionPartidaModalModificar" runat="server"></asp:TextBox>
                                     </div>
                                </div>

                                <%-- fin campos a llenar --%>
                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-primary" OnClick="btnModificar_Click"/>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
    <!-- Fin modal modificar partida -->


     <!-- Modal eliminar escala -->
            <div id="modalEliminarPartida" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Eliminar partida</h4>
                        </div>
                        <div class="modal-body">

                            <div class="row">
                                <%-- campos a llenar --%>

                                 <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="labelP" runat="server" Text="Periodo" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="lbPeriodoModalEliminar" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="Label10" runat="server" Text="Tipo o Padre" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="lbPartidaPadreModalEliminar" runat="server" Text="Partida Padre" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                
                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="Label12" runat="server" Text="Número de Partida" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                            <asp:TextBox class="form-control" ID="txtNumeroPartidasModalElimina" runat="server" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>

                                
                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                 <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="Label13" runat="server" Text="Descripción de Partida" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                     <div class="col-md-4 col-xs-4 col-sm-4">
                                         <asp:TextBox class="form-control" TextMode="multiline" Rows="5" ID="txtDescripcionPartidaModalEliminar" runat="server" ReadOnly="true"></asp:TextBox>
                                     </div>
                                </div>

                                <%-- fin campos a llenar --%>
                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnEliminarPartidaModal" runat="server" Text="Eliminar" CssClass="btn btn-primary" OnClick="btnEliminarPartidaModal_Click"/>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal eliminar escala -->



     <!-- Script inicio -->
    <script type="text/javascript">
        function activarModalNuevaPartida() {
            $('#modalNuevaPartida').modal('show');
        };

        function activarModalModificarPartida() {
            $('#modalModificarPartida').modal('show');
        };

          function activarModalEliminarPartida() {
            $('#modalEliminarPartida').modal('show');
        };
    </script>
    <!-- Script fin -->

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
