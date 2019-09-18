<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="PresupuestoEgreso.aspx.cs" Inherits="Proyecto.Catalogos.Presupuesto.PresupuestoEgreso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <div class="row">
        <div class="form-group col-md-12 col-xs-12 col-sm-12">
            <center>
                <asp:Label runat="server" Text="Presupuesto de Egreso" Font-Size="Large" ForeColor="Black"></asp:Label>
            
                <p class="mt-1">Seleccione un periodo</p>
                <asp:DropDownList AutoPostBack="true" ID="PeriodosDDL" runat="server" CssClass="form-control" OnSelectedIndexChanged="Periodos_OnChanged"></asp:DropDownList>
                
                <p class="mt-1">Seleccione un proyecto</p>
                <asp:DropDownList AutoPostBack="true" ID="ProyectosDDL" runat="server" CssClass="form-control" OnSelectedIndexChanged="Proyectos_OnChanged"></asp:DropDownList>

                <p class="mt-1">Seleccione una unidad</p>
                <asp:DropDownList AutoPostBack="true" ID="UnidadesDDL" runat="server" CssClass="form-control" OnSelectedIndexChanged="Unidades_OnChanged"></asp:DropDownList>
                      </center>
        </div>

                <div class="col-md-12 col-xs-12 col-sm-12 mt-2">
                    <hr />
                </div>

                <div class="form-group col-md-12 col-xs-12 col-sm-12 mt-1">
                    <asp:Label ID="LblPresupuestoIngreso" runat="server" Text="El monto disponible para el proyecto seleccionado es" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <p class="mt-2">Plan estrategico operacional <span style='color:red'>*</span></p>
                    </div>
                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <asp:TextBox class="form-control" TextMode="multiline" Columns="50" Rows="5" ID="txtPAO" runat="server"></asp:TextBox>
                    </div>
                    <div id="divPAOIncorrecto" runat="server" style="display: none" class="col-md-6 col-xs-6 col-sm-6">
                        <asp:Label ID="lblPAOIncorrecto" runat="server" Font-Size="Small" class="label alert-danger" Text="Espacio Obligatorio" ForeColor="Red"></asp:Label>
                    </div>
                </div>

                <%--<asp:FileUpload ID="FileUpload1" runat="server"></asp:FileUpload>--%>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <p class="mt-2">Ingrese los montos para cada partida</p>
                    </div>
                </div>

                <%-- tabla--%>
                    <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <asp:Repeater ID="rpPartida" runat="server" >
                        <HeaderTemplate>
                            <table class="table table-bordered">
                                <thead style="text-align: center">
                                    <tr style="text-align: center" class="btn-primary">
                                     <th></th>
                                        <th>Partida</th>
                                        <th>Monto total </th>
                                         <th>Descripción</th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr style="text-align: center">
                               <td>
                                    <asp:LinkButton ID="btnAnadirPartida" runat="server" ToolTip="Anadir" CommandArgument='<%# Eval("idPresupuestoEgreso") %>' OnClick="btnNuevoPresupuesto_Click"><span class="btn glyphicon glyphicon-plus"></span></asp:LinkButton>
                                  
                                    <asp:LinkButton ID="btnVerPartida" runat="server" ToolTip="Ver" CommandArgument='<%# Eval("idPresupuestoEgreso") %>' OnClick="btnVerPartidas_Click"><span class="btn glyphicon glyphicon-eye-open"></span></asp:LinkButton>
                                       
                                </td>
                                <td>
                                  <%# Eval("idPresupuestoEgreso") %>
                                    
                                </td>
                                <td>
                                     ₡  <%# Eval("montoTotal") %> 
                                    
                                </td>
                               
                                 <td>
                                        <%# Eval("descripcion") %>
                                 </td>
                            </tr>

                        </ItemTemplate>

                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
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
      

        <div class="col-md-12 col-xs-12 col-sm-12">
                <div class="col-md-6 col-xs-6 col-sm-6">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="Guardar_Click"/>
                </div>

                <div class="col-md-6 col-xs-6 col-sm-6 alinear-derecha">
                    <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" CssClass="btn btn-primary" OnClick="Aprobar_Click"/>
                </div>
        </div> 
    </div>
     </ContentTemplate>
 </asp:UpdatePanel>

    
    <!-- Modal nueva partida -->
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div id="modalIngresarPartida" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Agregar una nueva partida</h4>
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
                                        <asp:Label ID="label4" runat="server" Text="Partida" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="txtIdPartida" runat="server"  Font-Size="Medium" ForeColor="Black" CssClass="label" Enabled="false"></asp:Label>
                                 </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                      <asp:Label ID="Label10" runat="server" Text="Monto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                   
                                       <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox class="form-control" ID="txtMontoIngresarModal"   runat="server" Enabled="true" ></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                         <asp:Label ID="Label2" runat="server" Text="Descripción" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                       
                                             <asp:TextBox class="form-control" ID="txtdescripcionNuevaPartida" type="text" runat="server"></asp:TextBox>
                                           
                                        </div>
                                    </div>
                                </div>
                                                       
                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                             <asp:Button ID="btnNuevoIngresoPartidaModal" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnNuevoIngresoPartidaModal_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal nueva escala -->
      <!-- Modal pasar escala -->
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
            <div id="modalMostrarPresupuestoEgresos" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg" style="width: 98% !important">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Partidas de egresos</h4>
                        </div>
                        <div class="modal-body">
                            <%-- campos a llenar --%>
                            <div class="row">

                                <%-- fin campos a llenar --%>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <asp:Repeater ID="rpPartidaEgresoPartida" runat="server" >
                        <HeaderTemplate>
                            <table class="table table-bordered">
                                <thead style="text-align: center">
                                    <tr style="text-align: center" class="btn-primary">
                                     
                                        <th>Partida</th>
                                        <th>Presupuesto  </th>
                                        <th>Monto </th>
                                         <th>Descripción</th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr style="text-align: center">
                          
                                <td>
                                  <%# Eval("idPresupuestoEgreso") %>
                                    
                                </td>
                                <td>
                                  <%# Eval("idPartida") %> 
                                    
                                </td>
                               <td>
                                     ₡  <%# Eval("monto") %> 
                                    
                                </td>
                                 <td>
                                    <%# Eval("descripcion") %> 
                                  </td>
                            </tr>

                        </ItemTemplate>

                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

                               

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
    <!-- Fin modal pasar escala -->

      <script type="text/javascript">
        function activarModalIngresarPartida() {
            $('#modalIngresarPartida').modal('show');
        };
        function activarModalMostrarPresupuestoEgresos() {
            $('#modalMostrarPresupuestoEgresos').modal('show');
        };
        
       

    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
