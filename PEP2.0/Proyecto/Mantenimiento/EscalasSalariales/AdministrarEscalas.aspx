<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarEscalas.aspx.cs" Inherits="Proyecto.Mantenimiento.EscalasSalariales.AdministrarEscalas" MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="row">
                <%-- titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12">
                    <center>
                        <asp:Label ID="label" runat="server" Text="Escalas salariales" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </center>
                </div>
                <%-- fin titulo pantalla --%>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <hr />
                </div>

                <div class="col-md-6 col-xs-6 col-sm-6">
                    <h4>Período</h4>
                    <asp:DropDownList ID="ddlPeriodo" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-md-6 col-xs-6 col-sm-6">
                    <h4>Pasar escalas salariales</h4>
                    <asp:Button ID="btnPasarEscalas" runat="server" Text="Pasar escalas salariales" CssClass="btn btn-primary boton-nuevo" OnClick="btnPasarEscalas_Click" />
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
                                <th>Descripción</th>
                                <th>Salario base I</th>
                                <th>Salario base II</th>
                                <th>Tope escalafones</th>
                                <th>Porcentaje de escalafones</th>
                            </tr>
                        </thead>
                        <tr>
                            <td>
                                <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary" OnClick="btnFiltrar_Click"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton></td>
                            <td>
                                <asp:TextBox ID="txtBuscarDesc" runat="server" CssClass="form-control chat-input" placeholder="filtro descripción" onkeypress="enter_click()"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <asp:Repeater ID="rpEscalas" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
                                        <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idEscalaSalarial") %>' OnClick="btnEditar_Click"><span class="btn glyphicon glyphicon-pencil"></span></asp:LinkButton>
                                        <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idEscalaSalarial") %>' OnClick="btnEliminar_Click"><span class="btn glyphicon glyphicon-trash"></span></asp:LinkButton>
                                    </td>
                                    <td>
                                        <%# Eval("descEscalaSalarial") %>
                                    </td>
                                    <td>₡ <%# Eval("salarioBase1") %>
                                    </td>
                                    <td>₡ <%# Eval("salarioBase2") %>
                                    </td>
                                    <td>
                                        <%# Eval("topeEscalafones") %>
                                    </td>
                                    <td>
                                        <%# Eval("porentajeEscalafones") %> %
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
                    <asp:Button ID="btnNuevaEscalaSalarial" runat="server" Text="Nuevo" CssClass="btn btn-primary boton-nuevo" OnClick="btnNuevaEscalaSalarial_Click" />
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Modal nueva escala -->
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div id="modalNuevaEscala" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Nueva escala salarial</h4>
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
                                        <asp:Label ID="Label4" runat="server" Text="Período" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="lblPeriodoModal" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="lblProyecto" runat="server" Text="Descripción de clase <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:TextBox class="form-control" ID="txtDesc" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label1" runat="server" Text="Salario base I <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox class="form-control" ID="txtSalarioBase1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label5" runat="server" Text="Salario base II <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox class="form-control" ID="txtSalarioBase2" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label2" runat="server" Text="Tope escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">#</span>
                                            <asp:TextBox class="form-control" ID="txtTopeEscalafones" runat="server"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label3" runat="server" Text="Porcentaje escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <asp:TextBox class="form-control" ID="txtPorcentajeEscalafones" runat="server"></asp:TextBox>
                                            <span class="input-group-addon">%</span>
                                        </div>
                                    </div>
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
                            <asp:Button ID="btnNuevaEscalaModal" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnNuevaEscalaModal_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal nueva escala -->

    <!-- Modal editar escala -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="modalEditarEscala" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Editar escala salarial</h4>
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
                                        <asp:Label ID="Label6" runat="server" Text="Período" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="lblPeriodoModalEditar" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label8" runat="server" Text="Descripción de clase <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:TextBox class="form-control" ID="txtDescEditar" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label9" runat="server" Text="Salario base I <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox class="form-control" ID="txtSalarioBase1Editar" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label10" runat="server" Text="Salario base II <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox class="form-control" ID="txtSalarioBase2Editar" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label11" runat="server" Text="Tope escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">#</span>
                                            <asp:TextBox class="form-control" ID="txtTopeEscalafonesEditar" runat="server"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label12" runat="server" Text="Porcentaje escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <asp:TextBox class="form-control" ID="txtPorcentajeEscalafonesEditar" runat="server"></asp:TextBox>
                                            <span class="input-group-addon">%</span>
                                        </div>
                                    </div>
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
                            <asp:Button ID="btnEditarEscalaModal" runat="server" Text="Actualizar" CssClass="btn btn-primary" OnClick="btnActualizarEscalaModal_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal editar escala -->

    <!-- Modal eliminar escala -->
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div id="modalEliminarEscala" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Eliminar escala salarial</h4>
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
                                        <asp:Label ID="Label7" runat="server" Text="Período" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:Label ID="lblPeriodoModalEliminar" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label14" runat="server" Text="Descripción de clase" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <asp:TextBox class="form-control" ID="txtDescEliminar" ReadOnly="true" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label15" runat="server" Text="Salario base I" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox class="form-control" ID="txtSalarioBase1Eliminar" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label16" runat="server" Text="Salario base II" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox class="form-control" ID="txtSalarioBase2Eliminar" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label17" runat="server" Text="Tope escalafones" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">#</span>
                                            <asp:TextBox class="form-control" ID="txtTopeEscalafonesEliminar" ReadOnly="true" runat="server"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-3 col-xs-3 col-sm-3">
                                        <asp:Label ID="Label18" runat="server" Text="Porcentaje escalafones" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-4 col-xs-4 col-sm-4">
                                        <div class="input-group">
                                            <asp:TextBox class="form-control" ID="txtPorcentajeEscalafonesEliminar" ReadOnly="true" runat="server"></asp:TextBox>
                                            <span class="input-group-addon">%</span>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <asp:Button ID="btnEliminarEscalaModal" runat="server" Text="Eliminar" CssClass="btn btn-primary" OnClick="btnEliminarEscalaModal_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Fin modal eliminar escala -->

    <!-- Modal pasar escala -->
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
            <div id="modalPasarEscala" class="modal fade" role="alertdialog">
                <div class="modal-dialog modal-lg" style="width: 98% !important">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Pasar escalas salariales</h4>
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
                                        <asp:Label ID="Label13" runat="server" Text="Período seleccionado" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-6 col-xs-6 col-sm-6">
                                        <asp:Label ID="Label19" runat="server" Text="Período a pasar las escalas" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
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
                                        <asp:DropDownList ID="ddlPeriodoModalPasarEscalas" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodoModalPasarEscalas_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <hr />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <!-- ------------------------ tabla escalas salariales a pasar --------------------------- -->
                                    <div class=" table-responsive col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">

                                        <table id="tblEscalasAPasar" class="table table-bordered">
                                            <thead>
                                                <tr style="text-align: center" class="btn-primary">
                                                    <th></th>
                                                    <th>Descripción</th>
                                                    <th>Salario base I</th>
                                                    <th>Salario base II</th>
                                                    <th>Tope escalafones</th>
                                                    <th>Porcentaje de escalafones</th>
                                                </tr>
                                            </thead>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="btnFiltrarEscalasAPasar" runat="server" CssClass="btn btn-primary" OnClick="btnFiltrarEscalasAPasar_Click"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton></td>
                                                <td>
                                                    <asp:TextBox ID="txtBuscarDescEscalasAPasar" runat="server" CssClass="form-control chat-input" placeholder="filtro descripción" onkeypress="enter2_click()"></asp:TextBox>
                                                </td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>

                                            <asp:Repeater ID="rpEscalasAPasar" runat="server">
                                                <HeaderTemplate>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <tr style="text-align: center">
                                                        <td>
                                                            <asp:LinkButton ID="btnSeleccionarEscala" runat="server" ToolTip="Copiar escala salarial" CommandArgument='<%# Eval("idEscalaSalarial") %>' OnClick="btnSeleccionarEscala_Click"><span class="glyphicon glyphicon-share-alt"></span></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <%# Eval("descEscalaSalarial") %>
                                                        </td>
                                                        <td>₡ <%# Eval("salarioBase1") %>
                                                        </td>
                                                        <td>₡ <%# Eval("salarioBase2") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("topeEscalafones") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("porentajeEscalafones") %> %
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                    <!-- ---------------------- FIN tabla escalas salariales a pasar ------------------------- -->

                                    <!-- ------------------------ tabla escalas salariales agregadas pasar --------------------------- -->
                                    <div class=" table-responsive col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">

                                        <table id="tblEscalasAgregadas" class="table table-bordered">
                                            <thead>
                                                <tr style="text-align: center" class="btn-primary">
                                                    <th>Descripción</th>
                                                    <th>Salario base I</th>
                                                    <th>Salario base II</th>
                                                    <th>Tope escalafones</th>
                                                    <th>Porcentaje de escalafones</th>
                                                </tr>
                                            </thead>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="btnFiltrarEscalasAgregadas" runat="server" CssClass="btn btn-primary" OnClick="btnFiltrarEscalasAgregadas_Click"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton></td>
                                                <td>
                                                    <asp:TextBox ID="txtBuscarDescEscalasAgregadas" runat="server" CssClass="form-control chat-input" placeholder="filtro descripción" onkeypress="enter3_click()"></asp:TextBox>
                                                </td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <asp:Repeater ID="rpEscalasAgregadas" runat="server">
                                                <HeaderTemplate>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <tr style="text-align: center">
                                                        <td>
                                                            <%# Eval("descEscalaSalarial") %>
                                                        </td>
                                                        <td>₡ <%# Eval("salarioBase1") %>
                                                        </td>
                                                        <td>₡ <%# Eval("salarioBase2") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("topeEscalafones") %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("porentajeEscalafones") %> %
                                                        </td>
                                                    </tr>

                                                </ItemTemplate>

                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                    <!-- ---------------------- FIN tabla escalas salariales agregadas pasar ------------------------- -->
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <!-- ---------------------- tabla paginacion escalas salariales a pasar ------------------------- -->
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
                                    <!-- ---------------------- FIN tabla paginacion escalas salariales a pasar ------------------------- -->

                                    <!-- ---------------------- tabla paginacion escalas agregadas ------------------------- -->
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
    <!-- Fin modal pasar escala -->

    <script type="text/javascript">
        function activarModalNuevaEscala() {
            $('#modalNuevaEscala').modal('show');
        };

        function activarModalEditarEscala() {
            $('#modalEditarEscala').modal('show');
        };

        function activarModalEliminarEscala() {
            $('#modalEliminarEscala').modal('show');
        };

        function activarModalPasarEscala() {
            $('#modalPasarEscala').modal('show');
        };

        function enter_click() {
            if (window.event.keyCode == 13) {
                document.getElementById('<%=btnFiltrar.ClientID%>').focus();
                document.getElementById('<%=btnFiltrar.ClientID%>').click();
            }
        }

        function enter2_click() {
            if (window.event.keyCode == 13) {
                document.getElementById('<%=btnFiltrarEscalasAPasar.ClientID%>').focus();
                document.getElementById('<%=btnFiltrarEscalasAPasar.ClientID%>').click();
            }
        }

        function enter3_click() {
            if (window.event.keyCode == 13) {
                document.getElementById('<%=btnFiltrarEscalasAgregadas.ClientID%>').focus();
                document.getElementById('<%=btnFiltrarEscalasAgregadas.ClientID%>').click();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>