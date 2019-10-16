<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarPeriodo.aspx.cs" Inherits="PEP.Catalogos.Periodos.AdministrarPeriodo" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:scriptmanager id="MainScriptManager" runat="server" enablecdn="true"></asp:scriptmanager>

    <asp:updatepanel id="pnlUpdate" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12 col-xs-12 col-sm-12">
                    <center>
                            <asp:Label runat="server" Text="Apertura de Periodo" Font-Size="Large" ForeColor="Black"></asp:Label>
                            <p class="mt-1">Seleccione un periodo, o ingrese uno nuevo</p>
                        </center>
                </div>


                <div class="col-md-12 col-xs-6 col-sm-6">

                    <div class="col-md-2 col-xs-2 col-sm-2 col-md-offset-10 col-xs-offset-10 col-sm-offset-10" style="text-align: right">
                        <asp:Button ID="btnNuevoPeriodo" runat="server" Text="Nuevo periodo" CssClass="btn btn-primary boton-nuevo" OnClick="btnNuevoPeriodo_Click" />
                    </div>
                </div>



                <%-- tabla periodo--%>


                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table class="table table-bordered">
                        <thead style="text-align: center !important; align-content: center">
                            <tr style="text-align: center" class="btn-primary">

                                <th>Seleccionar Período</th>
                                <th>Habilitar Período</th>
                                <th>Año Período</th>

                                <th></th>
                            </tr>
                        </thead>

                        <asp:Repeater ID="rpPeriodos" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">

                                    <td>
                                        <div class="btn-group">
                                            <asp:HiddenField runat="server" ID="HFIdProyecto" Value='<%# Eval("anoPeriodo") %>' />
                                            <%--<asp:CheckBox ID="cbProyecto" runat="server" Text="" />--%>
                                            <asp:LinkButton ID="btnSelccionar" runat="server" ToolTip="Seleccionar" OnClick="btnSelccionar_Click" CommandArgument='<%# Eval("anoPeriodo") %>'><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="btn-group">
                                            <asp:HiddenField runat="server" ID="HiddenField1" Value='<%# Eval("anoPeriodo") %>' />
                                            <%--<asp:CheckBox ID="cbProyecto" runat="server" Text="" />--%>
                                            <asp:LinkButton ID="btnHabilitarActual" runat="server" ToolTip="Seleccionar" OnClick="EstablecerPeriodoActual_Click" CommandArgument='<%# Eval("anoPeriodo") %>'><span class="glyphicon glyphicon-thumbs-up"></span></asp:LinkButton>
                                        </div>
                                    </td>
                                    <td>
                                        <%# Eval("AnoPeriodo") %> <%# (Eval("habilitado").ToString() == "True")? "(Actual)" : "" %>
                                    </td>

                                    <td>
                                        <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("anoPeriodo") %>' OnClick="btnEliminar_Click"><span class="btn glyphicon glyphicon-trash"></span></asp:LinkButton>
                                    </td>
                                </tr>

                            </ItemTemplate>

                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                </div>
                <%-- fin tabla--%>

                <%--Paginación--%>
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
                <%--Fin Paginación--%>

                <%--Botones--%>

                <div class="form-group col-md-6 col-xs-6 col-sm-6 alinear-derecha">
                    <asp:Button ID="EstablecerPeriodoActualBtn" runat="server" Text="Establecer como periodo actual" CssClass="btn btn-primary" Visible="false" />
                </div>

                <%--Modales--%>


                <!-- Modal nuevo periodo -->
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                      
                        <div id="modalNuevoPeriodo" class="modal fade" role="alertdialog">
                            <div class="modal-dialog modal-lg">

                                <!-- Modal content-->
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        <h4 class="modal-title">Nuevo Período</h4>
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
                                                    <asp:Label ID="label4" runat="server" Text="Período <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                </div>
                                                <div class="col-md-4 col-xs-4 col-sm-4">
                                                    <div class="input-group">
                                                        <asp:TextBox class="form-control" ID="txtNuevoP" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-12 col-xs-12 col-sm-12">
                                                <br />
                                            </div>



                                            <div class="col-md-12 col-xs-12 col-sm-12">
                                                <br />
                                            </div>

                                        </div>
                                    </div>
                                    <div class="modal-footer" style="text-align: center">
                                        <asp:Button ID="btnNuevoPeriodoModal" runat="server" Text="Guardar" CssClass="btn btn-primary"  OnClick="btnNuevoPeriodoModal_Click" />
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel> 
                <!-- Fin modal nuevo periodo -->

                <!-- Modal eliminar periodo -->
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <div id="modalEliminarPeriodo" class="modal fade" role="alertdialog">
                            <div class="modal-dialog modal-lg">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                                        <h4 class="modal-title">Eliminar Periodo</h4>
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
                                                    <asp:Label ID="lblProyecto" runat="server" Text="Perído" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                </div>
                                                <div class="col-md-3 col-xs-3 col-sm-3">
                                                    <asp:Label ID="txtPeriodoEliminarModal" runat="server" Text="Período" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                </div>
                                            </div>


                                            <div class="col-md-12 col-xs-12 col-sm-12">
                                                <br />
                                            </div>


                                        </div>
                                        <div class="modal-footer" style="text-align: center">
                                            <asp:Button ID="btnEliminarModal" runat="server" Text="Eliminar" CssClass="btn btn-primary" OnClick="btnConfirmarEliminarPeriodo_Click" />
                                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <!-- Fin modal eliminar proyecto -->
                    </ContentTemplate>
                </asp:UpdatePanel>

               
                <script type="text/javascript">
                    function activarModalNuevoPeriodo() {
                        $('#modalNuevoPeriodo').modal('show');
                    };
                    function activarModalEliminarPeriodo() {
                        $('#modalEliminarPeriodo').modal('show');
                    };
                    function activarModalEditarProyecto() {
                        $('#modalEditarProyecto').modal('show');
                    };
                    function activarModalEliminarProyecto() {
                        $('#modalEliminarProyecto').modal('show');
                    };
                    function activarModalNuevoProyecto() {
                        $('#modalNuevoProyecto').modal('show');
                    };
                    function activarModalTransferirProyecto() {
                        $('#modalTransferirProyecto').modal('show');
                    };
                    function activarModalNuevaUnidad() {
                        $('#modalNuevaUnidad').modal('show');
                    };
                    function activarModalEliminarUnidad() {
                        $('#modalEliminarUnidad').modal('show');
                    };
                    function activarModalConfirmar() {
                        $('#modalConfirmar').modal('show');
                    };
                    function activarModalEditarUnidad() {
                        $('#modalEditarUnidad').modal('show');
                    };
                    function activarModalConfirmarProyecto() {
                        $('#modalConfirmarProyecto').modal('show');
                    };
                    function activarModalConfirmarPeriodo() {
                        $('#modalConfirmarPeriodo').modal('show');
                    };
                </script>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 mt-2">
                    <hr />
                </div>

                <div id="divProyectosPeriodos" runat="server" Visible="false"> 
                    
                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <center>
                            <asp:Label runat="server" Text="Proyectos" Font-Size="Large" ForeColor="Black"></asp:Label>
                            <p class="mt-1">Seleccione los proyectos del periodo seleccionado, y elija un periodo para transferirlos; o ingrese uno nuevo</p>
                        </center>
                    </div>
                    <%--ACOMODAR BOTONES--%>
                    <div class="row col-md-11 col-xs-12 col-sm-12">
                        <div class="form-group col-md-6 col-xs-6 col-sm-6 mt-1">
                            <asp:Button ID="btnNuevoProyecto" runat="server" Text="Nuevo proyecto" CssClass="btn btn-primary boton-nuevo" OnClick="AgregarProyecto_Click" />
                            <asp:Button ID="btnTransferir" runat="server" Text="Transferir Proyecto" CssClass="btn btn-primary boton-otro" OnClick="btnTransferirProyecto_Click" />

                        </div>
                        <br />
                        <br />
                        <asp:Label ID="AnoActual" runat="server" CssClass="font-weight-bold" />

                    </div>

                    <%--TABLA PROYECTOS--%>
                    <div class="row col-md-12 col-xs-12 col-sm-12">

                            <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                                <asp:Repeater ID="rpProyectos" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-bordered">
                                            <thead style="text-align: center">
                                                <tr style="text-align: center" class="btn-primary">
                                                    <th>Seleccionar Proyecto</th>
                                                    <th>Nombre</th>
                                                    <th>Tipo</th>
                                                    <th></th>

                                                </tr>
                                            </thead>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr style="text-align: center">
                                            <td>
                                                <div class="btn-group">
                                                    <asp:LinkButton ID="btnSelccionarProyecto" runat="server" ToolTip="Seleccionar proyecto" OnClick="btnSelccionarProyecto_Click" CommandArgument='<%# Eval("idProyecto") %>'><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                                                </div>
                                            </td>
                                            <td>
                                                <%# Eval("nombreProyecto") %>
                                            </td>

                                            <td>
                                                <%# (Eval("esUCR").ToString() == "True")? "UCR" : "Fundevi" %>
                                            
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnEditarProyecto" runat="server" ToolTip="Editar proyecto" CommandArgument='<%# Eval("idProyecto") %>' OnClick="btnEditarProyecto_Click"><span class="btn glyphicon glyphicon-pencil"></span></asp:LinkButton>
                                                <asp:LinkButton ID="btnEliminarProyecto" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idProyecto") %>' OnClick="btnEliminarProyecto_Click"><span class="btn glyphicon glyphicon-trash"></span></asp:LinkButton>
                                            </td>

                                        </tr>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                           
                            <%--FIN TABLA PROYECTOS--%>
                            <%--Paginación--%>

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
                                <asp:Label ID="lblpagina4" runat="server" Text="">p</asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                            </div>
                        </div>
                       
                    </div>

                        <%--Fin Paginación Tabla Proyecto--%>

                        <!-- Modal editar proyecto -->
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="modalEditarProyecto" class="modal fade" role="alertdialog">
                                    <div class="modal-dialog modal-lg">

                                        <!-- Modal content-->
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                <h4 class="modal-title">Editar Proyecto</h4>
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
                                                            <asp:Label ID="lbPeriodo" runat="server" Text="Período" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                        </div>

                                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                                            <asp:Label ID="lbPeriodoEditar" runat="server" Font-Size="Medium" ForeColor="Black" CssClass="label" Text=""></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                                            <asp:Label ID="lbNombreEditar" runat="server" Text="Nombre Proyecto<span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                        </div>

                                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                                            <asp:TextBox class="form-control" ID="txtNombreEditar" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                                            <asp:Label ID="lbTipoEditar" runat="server" Text="Tipo Proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                        </div>

                                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                                            <div class="input-group">

                                                                <asp:TextBox class="form-control" ID="txtTipoEditar" runat="server" ReadOnly="true"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                                        <br />
                                                    </div>

                                                    <div class="col-xs-12">
                                                        <br />
                                                        <div class="col-xs-12">
                                                            <h6 style="text-align: left">Los campos marcados con <span style='color: red'>*</span> son requeridos.</h6>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="modal-footer" style="text-align: center">
                                                <asp:Button ID="btnEditarProyectoModal" runat="server" Text="Actualizar" CssClass="btn btn-primary" OnClick="btnActualizarProyectoModal_Click" />
                                                <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                               
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <!-- Fin modal editar proyecto -->

                        <!-- Modal eliminar proyecto -->
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div id="modalEliminarProyecto" class="modal fade" role="alertdialog">
                                    <div class="modal-dialog modal-lg">

                                        <!-- Modal content-->
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                <h4 class="modal-title">Eliminar Proyecto</h4>
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
                                                            <asp:Label ID="lbElimPerProy" runat="server" Text="Período" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                        </div>

                                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                                            <asp:Label ID="lblElimPerProyModal" runat="server" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                                            <asp:Label ID="lbNombreElim" runat="server" Text="Nombre del Proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                        </div>

                                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                                            <asp:TextBox class="form-control" ID="txtProyEliminar" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                                        <br />
                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                                            <asp:Label ID="lbTipoElim" runat="server" Text="Tipo del Proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                        </div>

                                                        <div class="col-md-4 col-xs-4 col-sm-4">
                                                            <asp:TextBox class="form-control" ID="txtTipoElim" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                                        <br />
                                                    </div>
                                                </div>
                                                <div class="modal-footer" style="text-align: center">
                                                    <asp:Button ID="btnEliminarProyectoaModal" runat="server" Text="Eliminar" CssClass="btn btn-primary" OnClick="btnConfirmarEliminarProyecto_Click" />
                                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </div>
                        <!-- Fin modal eliminar proyecto-->

                        <!-- Modal nuevo proyecto -->
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <div id="modalNuevoProyecto" class="modal fade" role="alertdialog">
                                    <div class="modal-dialog modal-lg">

                                        <!-- Modal content-->
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                <h4 class="modal-title">Eliminar Proyecto</h4>
                                            </div>
                                            <div class="modal-body">

                                                <div class="row">
                                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                                        <br />
                                                    </div>
                                                    <%-- campos a llenar --%>
                                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                                        <div class="col-md-2 col-xs-2 col-sm-2">
                                                            <asp:Label ID="lblPeriodoProyecto" runat="server" Text="Periodo" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                        </div>
                                                        
                                                        <div class="col-md-2 col-xs-2 col-sm-2">
                                                            <asp:Label ID="lbPeriodoDDLNuevo" runat="server" Text="f" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12 mt-1">
                                                        <div class="col-md-2 col-xs-2 col-sm-2">
                                                            <asp:Label ID="lblNombreProyecto" runat="server" Text="Nombre <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8 col-xs-8 col-sm-8">
                                                            <asp:TextBox class="form-control" ID="txtNombreProyecto" runat="server"></asp:TextBox>
                                                        </div>

                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12 mt-1">
                                                        <div class="col-md-2 col-xs-2 col-sm-2">
                                                            <asp:Label ID="lblCodigoProyecto" runat="server" Text="Código <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8 col-xs-8 col-sm-8">
                                                            <asp:TextBox class="form-control" ID="txtCodigoProyecto" runat="server"></asp:TextBox>
                                                        </div>

                                                    </div>

                                                    <div class="col-md-12 col-xs-12 col-sm-12 mt-1">
                                                        <div class="col-md-2 col-xs-2 col-sm-2">
                                                            <asp:Label ID="lblEsUCRProyecto" runat="server" Text="Tipo <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8 col-xs-8 col-sm-8">
                                                            <asp:DropDownList ID="ddlEsUCRProyecto" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="UCR" Value="true" />
                                                                <asp:ListItem Text="Fundevi" Value="false" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-xs-12">
                                                        <br />
                                                        <div class="col-xs-12">
                                                            <h6 style="text-align: left">Los campos marcados con <span style='color: red'>*</span> son requeridos.</h6>
                                                        </div>
                                                    </div>

                                                    <%-- fin campos a llenar --%>

                                                    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                                                        <hr />
                                                    </div>

                                                    <%-- botones --%>
                                                    <div class="modal-footer" style="text-align: center">
                                                        <asp:Button ID="btnNuevoProy" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick=" btnAgregarProyectoModal_Click" />
                                                        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                                    </div>
                                                    <%-- fin botones --%>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <!-- Fin modal nuevo proyecto-->

                            <!-- Modal pasar proyecto -->
    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
        <ContentTemplate>
            <div id="modalTransferirProyecto" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg" style="width: 98% !important">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Transferir Proyectos</h4>
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
                                        <asp:Label ID="lbPeriodoSeleccionado" runat="server" Text="Período seleccionado" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-6 col-xs-6 col-sm-6">
                                        <asp:Label ID="lbPeriodoATransferir" runat="server" Text="Período a transferir proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
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
                                        <asp:DropDownList ID="ddlPeriodoTranferir" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodoModalTransfeririP_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <hr />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <!-- ------------------------ tabla proyectos  a pasar --------------------------- -->
                                    <div class=" table-responsive col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">

                                        <table id="tbProyectosPasar" class="table table-bordered">
                                            <thead>
                                                <tr style="text-align: center" class="btn-primary">
                                                    <th></th>
                                                    <th>Nombre Proyecto</th>
                                                    <th>Tipo</th>
                                                </tr>
                                            </thead>


                                            <asp:Repeater ID="rpTransferirProyecto" runat="server">
                                                <HeaderTemplate>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <tr style="text-align: center">
                                                        <td>
                                                            <asp:LinkButton ID="btnSeleccionarProyectoT" runat="server" ToolTip="Transferir proyecto" CommandArgument='<%# Eval("idProyecto") %>' OnClick="btnSeleccionarProyectoT_Click"><span class="glyphicon glyphicon-share-alt"></span></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <%# Eval("nombreProyecto") %>
                                                        </td>
                                                        <td><%# (Eval("esUCR").ToString() == "True")? "UCR" : "Fundevi" %>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                    <!-- ---------------------- FIN tabla proyectos a tranferir ------------------------- -->

                                    <!-- ------------------------ tabla proyectos transferidos --------------------------- -->
                                    <div class=" table-responsive col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">

                                        <table id="tblProyectosTransferidos" class="table table-bordered">
                                            <thead>
                                                <tr style="text-align: center" class="btn-primary">
                                                    <th>Nombre</th>
                                                    <th>Tipo Proyecto</th>
                                                </tr>
                                            </thead>

                                            <asp:Repeater ID="rpProyectoTransferidos" runat="server">
                                                <HeaderTemplate>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <tr style="text-align: center">
                                                        <td>
                                                            <%# Eval("nombreProyecto") %>
                                                        </td>
                                                        <td><%# (Eval("esUCR").ToString() == "True")? "UCR" : "Fundevi" %>
                                                        </td>
                                                    </tr>

                                                </ItemTemplate>

                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                    <!-- ---------------------- FIN tabla proyectos transferidos ------------------------- -->
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <!-- ---------------------- tabla paginacion tranferir ------------------------- -->
                                    <div class="col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">
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
                                    <!-- ---------------------- FIN tabla paginacion proyectos a transferir ------------------------- -->

                                    <!-- ---------------------- tabla paginacion proyectos transferir ------------------------- -->
                                    <div class="col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">
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
                                    <!-- ---------------------- FIN tabla paginacion escalas agregadas ------------------------- -->
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
    <!-- FIn modal transferir proyecto -->


                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 mt-2">
                    <hr />
                </div>
               <!-- Div Unidades -->
    <div id="divUnidades" runat="server" visible="false">

        <div class="col-md-12 col-xs-12 col-sm-12">
            <center>
                  <asp:Label runat="server" Text="Unidades" Font-Size="Large" ForeColor="Black"></asp:Label>
                   <p class="mt-1">Seleccione una unidad del proyecto seleccionado, o ingrese una nueva</p>
             </center>
            <br />
            <center>
                 <asp:Label ID="proyectoActual" runat="server" CssClass="font-weight-bold" />
            </center>
        </div>
        
        <div class="row col-md-11 col-xs-12 col-sm-12">
            <div class="form-group col-md-6 col-xs-6 col-sm-6 mt-1">
                <asp:Button ID="btnNuevaUnidad" runat="server" Text="Nueva unidad" CssClass="btn btn-primary boton-nuevo" OnClick="AgregarUnidad_Click" Visible ="false" />
            </div>
            <br />
            <br />
        </div>

        <!-- ------------------------ Tabla unidades proyecto --------------------------- -->
        <div class=" table-responsive col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">

            <table id="tblUnidadesProyecto" class="table table-bordered">
                <thead>
                    <tr style="text-align: center" class="btn-primary">
                        <th>Nombre Unidad</th>
                        <th>Coordinador</th>
                        <th></th>
                    </tr>
                </thead>

                <asp:Repeater ID="rpUnidProyecto" runat="server">
                    <HeaderTemplate>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <tr style="text-align: center">

                            <td>
                                <%# Eval("nombreUnidad") %>
                            </td>
                            <td>
                                <%# Eval("coordinador") %>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnEditarUnidades" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idUnidad") %>' OnClick="btnEditarUnidad_Click"><span class="btn glyphicon glyphicon-pencil"></span></asp:LinkButton>
                                <asp:LinkButton ID="btnEliminarUnidad" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idUnidad") %>' OnClick="btnEliminarUnidad_Click"><span class="btn glyphicon glyphicon-trash"></span></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </div>
        <!-- ---------------------- FIN tabla unidades proyecto  ------------------------- -->
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 mt-2">
            <hr />
        </div>
        <!-- ---------------------- Paginación tabla unidades proyecto  ------------------------- -->
        <div class="col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">
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
        <!-- ---------------------- FIN paginación tabla unidades proyecto  ------------------------- -->
    </div>
    <!-- Fin Div Unidades-->

  <!-- Modal Nueva Unidad-->
    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
        <ContentTemplate>
            <div id="modalNuevaUnidad" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Nueva Unidad</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">


                                <%-- fin titulo accion --%>

                                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                                    <hr />
                                </div>

                                <%-- campos a llenar --%>
                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="lblProyectoUnidad" runat="server" Text="Proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                   
                                    <div class="col-md-5 col-xs-3 col-sm-3">
                                        <asp:Label ID="lbNuevaUnidadProy" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12 mt-1">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="lblNombreUnidad" runat="server" Text="Nombre unidad <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-xs-8 col-sm-8">
                                        <asp:TextBox class="form-control" ID="txtNombreUnidad" runat="server"></asp:TextBox>
                                    </div>

                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12 mt-1">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="lblCoordinadorUnidad" runat="server" Text="Coordinador <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-xs-8 col-sm-8">
                                        <asp:TextBox class="form-control" ID="txtCoordinadorUnidad" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-xs-12">
                                    <br />
                                    <div class="col-xs-12">
                                        <h6 style="text-align: left">Los campos marcados con <span style='color: red'>*</span> son requeridos.</h6>
                                    </div>
                                </div>

                                <%-- fin campos a llenar --%>

                                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                                    <hr />
                                </div>

                                <%-- botones --%>
                                <div class="col-md-3 col-xs-3 col-sm-3 col-md-offset-9 col-xs-offset-9 col-sm-offset-9">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnNuevaUnidadModal_Click" />
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                </div>
                                <%-- fin botones --%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIN Modal nueva unidad -->

            <!-- Modal Eliminar -->
     <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>
            <div id="modalEliminarUnidad" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Eliminar Unidad</h4>
                        </div> 
                        <div class="modal-body">
                            <div class="row">

                                <%-- campos a llenar --%>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="lbUnidadElim" runat="server" Text="Proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="lbProyUnidadElim" runat="server" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                     </div>
                                </div>
                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="lbNombreUniElim" runat="server" Text="Nombre Unidad" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:TextBox class="form-control" ID="txtNombreUnidadEliminar" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>


                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label5" runat="server" Text="Coordinador:" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:TextBox class="form-control" ID="txtCoordinadorEliminar" ReadOnly="true" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <%-- fin campos a llenar --%>

                                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                                    <hr />
                                </div>

                                <%-- botones --%>
                                <div class="modal-footer" style="text-align: center">
                                    <asp:Button ID="btnEliminarUnidadP" runat="server" Text="Eliminar" CssClass="btn btn-primary" OnClick="btnConfirmarEliminarUnidad_Click" />
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                </div>
                                <%-- fin botones --%>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIN modal eliminar unidad -->

                <!-- Modal editar unidad -->
    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
        <ContentTemplate>
            <div id="modalEditarUnidad" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Editar Unidad</h4>
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
                                        <asp:Label ID="Label7" runat="server" Text="Proyecto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                       <asp:Label ID="lbProyectoUnidad" runat="server" Font-Size="Medium" ForeColor="Black" CssClass="label" Text=""></asp:Label>
                                    </div>
                                </div>


                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="lbUnidadEdi" runat="server" Text="Nombre Unidad<span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:TextBox class="form-control" ID="txtNombreUnidadEditar" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="lbCoordEditar" runat="server" Text="Coordinador<span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">

                                            <asp:TextBox class="form-control" ID="txtCoordinadorEditar" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-xs-12">
                                    <br />
                                    <div class="col-xs-12">
                                        <h6 style="text-align: left">Los campos marcados con <span style='color: red'>*</span> son requeridos.</h6>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnEditarUnidadA" runat="server" Text="Actualizar" CssClass="btn btn-primary" OnClick="btnActualizarUnidadModal_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIN modal editar unidad -->
           
               <!-- Modal Confirmar Eliminar Unidad -->
    <asp:UpdatePanel ID="UPEliminar" runat="server">
        <ContentTemplate>
            <div id="modalConfirmar" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Confirmar elimimar Unidad</h4>
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
                                             <p>¿Está seguro que desea eliminar la Unidad?</p> 
                                              <asp:Label ID="lbConfUnidadEliminar" runat="server" Text="" Font-Size="Large" ForeColor="Black" CssClass="label"></asp:Label>             
                                            </center>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" CssClass="btn btn-primary boton-eliminar" OnClick="btnEliminarUnidadModal_Click" />
                            <button type="button" class="btn btn-default boton-otro" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIN Modal Confirmar Eliminar Unidad -->

                <!-- Modal Confirmar Eliminar Periodo-->
    <asp:UpdatePanel ID="UPconfirmarPeriodo" runat="server">
        <ContentTemplate>
            <div id="modalConfirmarPeriodo" class="modal" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Confirmar elimimar Periodo</h4>
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
                                             <p>¿Está seguro que desea eliminar el Período?</p> 
                                              <asp:Label ID="lbConfPer" runat="server" Text="" Font-Size="Large" ForeColor="Black" CssClass="label"></asp:Label>             
                                            </center>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>
                            </div><div class="col-xs-12">
                                            <br />
                                            <div class="col-xs-12">
                                                <h6 style="text-align: left">Los campos marcados con <span style='color: red'>*</span> son requeridos.</h6>
                                            </div>
                                        </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnConfPeriodo" runat="server" Text="Confirmar" CssClass="btn btn-primary boton-eliminar" OnClick="btnEliminarModal_Click" />
                            <button type="button" class="btn btn-default boton-otro" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIN Modal Confirmar Eliminar periodo -->

                 <!-- Modal Confirmar Eliminar Proyecto-->
    <asp:UpdatePanel ID="UPconfirmarProyecto" runat="server">
        <ContentTemplate>
            <div id="modalConfirmarProyecto" class="modal" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Confirmar elimimar Proyecto</h4>
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
                                             <p>¿Está seguro que desea eliminar el Proyecto?</p> 
                                              <asp:Label ID="lbConfProy" runat="server" Text="" Font-Size="Large" ForeColor="Black" CssClass="label"></asp:Label>             
                                            </center>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="Button1" runat="server" Text="Confirmar" CssClass="btn btn-primary boton-eliminar" OnClick="btnEliminarProyectoModal_Click" />
                            <button type="button" class="btn btn-default boton-otro" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIN Modal Confirmar Eliminar PECTO -->


            </div>

        </ContentTemplate>
    </asp:updatepanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
