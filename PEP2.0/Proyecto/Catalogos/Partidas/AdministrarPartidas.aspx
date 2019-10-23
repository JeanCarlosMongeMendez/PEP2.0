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
                    <asp:Button ID="btnPasarPartidas" runat="server" Text="Pasar Partidas" CssClass="btn btn-primary boton-otro" OnClick="btnPasarPartidas_Click" />
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
                                <th>Tipo de Partida</th>
                            </tr>
                        </thead>
                        <tr>
                            <td>
                                <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary" OnClick="btnFiltrar_Click"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton></td>
                            <td> 
                                <asp:TextBox ID="txtBuscarDesc" runat="server" CssClass="form-control chat-input" placeholder="filtro descripción" AutoPostBack="true" OnTextChanged="txtBuscarDesc_TextChanged"></asp:TextBox>
                            </td>
                            <td></td>
                            <td>
                              <div class="col-md-3 col-xs-4 col-sm-4">
                                </div>
                                <div class="col-md-5 col-xs-4 col-sm-4">
                                    <asp:DropDownList ID="ddlBuscarTipo" class="btn btn-default dropdown-toggle" runat="server" OnSelectedIndexChanged="ddlBuscar_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>                          
                                </div>

                               
                            </td>
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
                                    <td>
                                        <%# (Eval("esUCR").ToString() == "True")? "UCR" : "Fundevi" %>     
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
                                        <asp:Label ID="Label1" runat="server" Text="Partida Padre" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
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
                                        <asp:Label ID="Label9" runat="server" Text="Tipo Partida" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:DropDownList ID="ddlPartidasUCR" class="btn btn-default dropdown-toggle" runat="server">
                                        </asp:DropDownList>
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
                                        <asp:Label ID="Label5" runat="server" Text="Partida Padre" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
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
                                        <asp:Label ID="lbTipoP" runat="server" Text="Tipo Partida" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="lbPartidaTipoMod" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
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


     <!-- Modal eliminar partida -->
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
                                        <asp:Label ID="Label10" runat="server" Text="Partida Padre" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
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
                                        <asp:Label ID="Label11" runat="server" Text="Tipo Partida" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="lbTipoPartidaElm" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
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
                            <asp:Button ID="btnEliminarPartidaModal" runat="server" Text="Eliminar" CssClass="btn btn-primary" OnClick="btnConfirmarEliminarPartida"/>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
 <!-- Fin modal eliminar partida -->



         <!-- Modal pasar partidas -->
         <div id="modalPasarPartida" class="modal fade" role="alertdialog">
             <div class="modal-dialog modal-lg" style="width: 98% !important">

                 <!-- Modal content-->
                 <div class="modal-content">
                     <div class="modal-header">
                         <button type="button" class="close" data-dismiss="modal">&times;</button>
                         <h4 class="modal-title">Pasar Partidas</h4>
                     </div>
                     <div class="modal-body">
                         <%-- campos a llenar --%>
                         <div class="row">

                             <%-- fin campos a llenar --%>

                             <div class="col-md-12 col-xs-12 col-sm-12">
                                 <br />
                             </div>

                             <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                 <div class="col-md-6 col-xs-6 col-sm-6">
                                     <asp:Label ID="Label8" runat="server" Text="Período seleccionado" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                 </div>

                                 <div class="col-md-6 col-xs-6 col-sm-6">
                                     <asp:Label ID="Label19" runat="server" Text="Período a pasar partidas" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                 </div>
                             </div>

                             <div class="col-md-12 col-xs-12 col-sm-12">
                                 <br />
                             </div>

                             <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                 <div class="col-md-6 col-xs-6 col-sm-6">
                                     <asp:Label ID="lblPeriodoSeleccionado" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                 </div>

                                 <div class="col-md-6 col-xs-6 col-sm-6">
                                     <asp:DropDownList ID="ddlPeriodoModalPasaPartidas" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodoModalPasaPartidas_SelectedIndexChanged"></asp:DropDownList>
                                 </div>

                             </div>

                             <div class="col-md-12 col-xs-12 col-sm-12">
                                 <hr />
                             </div>


                             <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                 <!-- ------------------------ tabla partidas a pasar --------------------------- -->
                                 <div class=" table-responsive col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">

                                     <table id="tblPartidasAPasar" class="table table-bordered">
                                         <thead>
                                             <tr style="text-align: center" class="btn-primary">
                                                 <th></th>
                                                 <th>Descripción Partida</th>
                                                 <th>Número de Partida</th>
                                                 <th>Tipo de Partida</th>

                                             </tr>
                                         </thead>
                                         <tr>
                                             <td>
                                                 <asp:LinkButton ID="btnFiltrarPartidasAPasar" runat="server" CssClass="btn btn-primary" OnClick="btnFiltrarPartidasAPasar_Click"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton></td>
                                             <td>
                                                 <asp:TextBox ID="txtBuscarDescPartidasAPasar" runat="server" CssClass="form-control chat-input" placeholder="filtro descripción"></asp:TextBox>
                                             </td>
                                             <td></td>
                                       
                                             <td>
                                              
                                                 <div class="col-md-5 col-xs-4 col-sm-4">
                                                     <asp:DropDownList ID="ddlBuscarApasar" class="btn btn-default dropdown-toggle" runat="server" OnSelectedIndexChanged="ddlBuscarApasar_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                 </div>
                                             </td>
                                         </tr>

                                         <asp:Repeater ID="rpPartidasAPasar" runat="server">
                                             <HeaderTemplate>
                                             </HeaderTemplate>

                                             <ItemTemplate>
                                                 <tr style="text-align: center">
                                                     <td>
                                                         <asp:LinkButton ID="btnSeleccionarPasarPartida" runat="server" ToolTip="Copiar partida" CommandArgument='<%# Eval("idPartida") %>' OnClick="btnSeleccionarPasarPartida_Click"><span class="glyphicon glyphicon-share-alt"></span></asp:LinkButton>
                                                     </td>
                                                     <td>
                                                         <%# Eval("descripcionPartida") %>
                                                     </td>
                                                     <td>
                                                         <%# Eval("numeroPartida") %>
                                                     </td>
                                                     <td>
                                                        <%# (Eval("esUCR").ToString() == "True")? "UCR" : "Fundevi" %> 
                                                     </td>
                                                 </tr>
                                             </ItemTemplate>
                                             <FooterTemplate>
                                             </FooterTemplate>
                                         </asp:Repeater>
                                     </table>
                                 </div>
                                 <!-- ---------------------- FIN tabla partidas a pasar ------------------------- -->



                                 <!-- ------------------------ tabla partidas agregadas pasar --------------------------- -->
                                 <div class=" table-responsive col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">

                                     <table id="tblPartidasAgregadas" class="table table-bordered">
                                         <thead>
                                             <tr style="text-align: center" class="btn-primary">
                                                 <th></th>
                                                 <th>Descripción Partida</th>
                                                 <th>Número de Partida</th>
                                                 <th>Tipo de Partida</th>
                                             </tr>
                                         </thead>
                                         <tr>
                                             <td>
                                                 <asp:LinkButton ID="btnFiltrarPartidasAgregadas" runat="server" CssClass="btn btn-primary" OnClick="btnFiltrarPartidasAgregadas_Click"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton></td>
                                             <td>
                                                 <asp:TextBox ID="txtBuscarDescPartidasAgregadas" runat="server" CssClass="form-control chat-input" placeholder="filtro descripción"></asp:TextBox>
                                             </td>
                                             <td></td>
                                             <td>
                                              
                                                 <div class="col-md-5 col-xs-4 col-sm-4">
                                                     <asp:DropDownList ID="ddlBuscarAgregadas" class="btn btn-default dropdown-toggle" runat="server" OnSelectedIndexChanged="ddlBuscarAgregadas_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                 </div>
                                             </td>
                                         </tr>
                                         <asp:Repeater ID="rpPartidasAgregadas" runat="server">
                                             <HeaderTemplate>
                                             </HeaderTemplate>

                                             <ItemTemplate>
                                                 <tr style="text-align: center">
                                                     <td></td>
                                                     <td>
                                                         <%# Eval("descripcionPartida") %>
                                                     </td>
                                                     <td>
                                                         <%# Eval("numeroPartida") %>
                                                     </td>
                                                    <td>
                                                        <%# (Eval("esUCR").ToString() == "True")? "UCR" : "Fundevi" %> 
                                                     </td>
                                                     
                                                 </tr>

                                             </ItemTemplate>

                                             <FooterTemplate>
                                             </FooterTemplate>
                                         </asp:Repeater>
                                     </table>
                                 </div>
                                 <!-- ---------------------- FIN tabla partidas agregadas pasar ------------------------- -->
                             </div>




                             <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                 <!-- ---------------------- tabla paginacion partidas a pasar ------------------------- -->
                                 <div class="col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">
                                     <center>
                    <table class="table" style="max-width:664px;">
                        <tr style="padding:1px !important">
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbPrimero2" runat="server" CssClass="btn btn-primary" OnClick="lbPrimero2_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbAnterior2" runat="server" CssClass="btn btn-default" OnClick="lbAnterior2_Click" ><span class="glyphicon glyphicon-backward"></asp:LinkButton>
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
                                 <!-- ---------------------- FIN tabla paginacion partidas a pasar ------------------------- -->

                                 <!-- ---------------------- tabla paginacion partidas agregadas ------------------------- -->
                                 <div class="col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">
                                     <center>
                    <table class="table" style="max-width:664px;">
                        <tr style="padding:1px !important">
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbPrimero3" runat="server" CssClass="btn btn-primary" OnClick="lbPrimero3_Click" ><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
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
                                 <!-- ---------------------- FIN tabla paginacion cargas sociales agregadas ------------------------- -->
                             </div>





                         </div>
                     </div>
                 </div>
             </div>
         </div>

             <!-- Modal Confirmar Eliminar Unidad -->
    <asp:UpdatePanel ID="UPEliminar" runat="server">
        <ContentTemplate>
            <div id="modalConfirmarPartida" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Confirmar Eliminar Partida</h4>
                        </div>
                        <div class="modal-body">
                            <%-- campos a llenar --%>
                            <div class="row">

                                <%-- fin campos a llenar --%>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>
                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <center>
                                             <asp:Label runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                                             <p>¿Está seguro que desea eliminar la Partida?</p> 
                                                         
                                            </center>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" CssClass="btn btn-primary boton-eliminar" OnClick="btnEliminarPartidaModal_Click" />
                            <button type="button" class="btn btn-default boton-otro" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIN Modal Confirmar Eliminar Unidad -->








     </ContentTemplate>
    </asp:UpdatePanel>
    

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

         function activarModalPasarPartida() {
            $('#modalPasarPartida').modal('show');
        };
        function activarModalConfirmarPartida() {
             $('#modalConfirmarPartida').modal('show');
        }
    </script>
    <!-- Script fin -->

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
