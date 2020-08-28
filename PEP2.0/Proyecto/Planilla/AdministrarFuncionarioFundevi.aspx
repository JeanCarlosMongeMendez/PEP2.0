<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarFuncionarioFundevi.aspx.cs" Inherits="Proyecto.Planilla.AdministrarFuncionarioFundevi" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel20" runat="server">
        <ContentTemplate>

        <div class="row" style="text-align: center">

            <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
                <asp:Label ID="label" runat="server" Text="Planillas Fundevi" Font-Size="Large" ForeColor="Black"></asp:Label>
            </div>


     
    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
        <hr />
    </div>
    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
        <asp:Button ID="btnNuevoFuncionario" runat="server" Text="Nuevo funcionario" CssClass="btn btn-primary boton-nuevo" OnClick="btnNuevoFuncionario_Click" />
        <asp:Button ID="btnAsignarFuncionarios" runat="server" Text="Copiar funcionarios" CssClass="btn btn-primary boton-nuevo" OnClick="btnAsignarFuncionarios_Click" />
    </div>
    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
        <hr />
    </div>
    <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
        <table class="table table-bordered">
            <thead style="text-align: center">
                <tr style="text-align: center" class="btn-primary">
                    <th></th>
                    <th>Nombre</th>
                    <th>Salario</th>
                    <th>Ajustar Salario</th>
                </tr>
            </thead>
            <tr>
                <td>
                    <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton>
                </td>
                <td>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-search"></i></span>
                        <asp:TextBox ID="txtBuscarNombre" runat="server" CssClass="form-control chat-input" placeholder="filtro nombre" AutoPostBack="true" OnTextChanged="txtBuscarNombre_TextChanged"></asp:TextBox>
                    </div>
                </td>
                <td></td>
                <td></td>
            </tr>
            <asp:Repeater ID="rpPlanillas" runat="server">
                <HeaderTemplate>
                </HeaderTemplate>

                <ItemTemplate>
                    <tr style="text-align: center">
                        <td>

                            <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" OnClick="btnEditar_Click1" CommandArgument='<%# Eval("idFuncionario") %>'><span class="btn glyphicon glyphicon-pencil"></span></asp:LinkButton>
                            <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar" OnClick="btnEliminar_Click" CommandArgument='<%# Eval("idFuncionario") %>'><span class="btn glyphicon glyphicon-trash"></span></asp:LinkButton>
                        </td>

                        <td>
                            <%# Eval("nombre") %> 
                        </td>
                        <td>₡
                            <asp:Label runat="server" Text='<%# Eval("salario") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnAjuste" runat="server" Text="Ajustar" CssClass="btn btn-primary boton-nuevo" data-toggle="modal" data-target="#modalAjuste" OnClick="btnAjuste_Click" CommandArgument='<%# Eval("idFuncionario") %>' />

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

        <div class="row col-md-12 col-xs-12 col-sm-12">
            <div class="col-md-12 col-xs-12 col-sm-12 mt-1 alinear-derecha">
                <asp:Button Text="Regresar" ID="btnRegresar" runat="server" class="btn btn-danger" OnClick="btnRegresar_Click" />
            </div>
        </div>
    </div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- fin tabla--%>



    <!-- Modal -->
    <div class="modal fade" id="modalAjuste" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Ajustar salario
                    </h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body" style="align-content: center">

                    <div class="row">
                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <div class="col-md-5 col-xs-12 col-sm-5">
                                <asp:Label ID="Label11" runat="server" Text="Nombre del funcionario" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>
                            <div class="col-md-5 col-xs-12 col-sm-5">
                                <asp:Label ID="lblNombre" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <div class="col-md-5 col-xs-12 col-sm-5">
                                <asp:Label ID="Label12" runat="server" Text="Salario actual" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>
                            <div class="col-md-5 col-xs-12 col-sm-5">
                                <div class="input-group">
                                    <span class="input-group-addon">₡</span>
                                    <asp:TextBox class="form-control" ID="lblSalario" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <div class="col-md-5 col-xs-12 col-sm-5">
                                <asp:Label ID="Label14" runat="server" Text="Ingrese el nuevo salario" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>
                            <div class="col-md-5 col-xs-12 col-sm-5">
                                <div class="input-group">
                                    <span class="input-group-addon">₡</span>
                                    <asp:TextBox class="form-control" ID="txtAsalario" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-success" ID="btnGuardar" runat="server" OnClick="btnGuardarAjuste_Click" Text="Guardar" />
                    <asp:Button CssClass="btn btn-danger" data-dismiss="modal" runat="server" Text="Cancelar" />
                    <%--<button type="button" class="btn btn-secondary" >Cerrar</button>--%>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modalEliminarFuncionario" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5>
                        <asp:Label ID="txtEli" runat="server" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body" style="align-content: center">
                    <div class="row">
                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <div class="col-md-3 col-xs-12 col-sm-3">
                                <asp:Label ID="Label9" runat="server" Text="Nombre del funcionario" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>
                            <div class="col-md-5 col-xs-12 col-sm-5">
                                <asp:TextBox class="form-control" ID="txtNomF" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <div class="col-md-3 col-xs-12 col-sm-3">
                                <asp:Label ID="Label10" runat="server" Text="Salario" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>
                            <div class="col-md-5 col-xs-12 col-sm-5">
                                <div class="input-group">
                                    <span class="input-group-addon">₡</span>
                                    <asp:TextBox class="form-control" ID="txtSa" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-success" ID="Button1" runat="server" OnClick="Button1_Click" Text="Eliminar" />
                    <asp:Button CssClass="btn btn-danger" data-dismiss="modal" runat="server" Text="Cancelar" />
                    <%--<button type="button" class="btn btn-secondary" >Cerrar</button>--%>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modalEditarFuncionario" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5>
                        <asp:Label ID="Label1" runat="server" Text="Editar funcionario" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body" style="align-content: center">
                    <div class="row">
                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <div class="col-md-3 col-xs-12 col-sm-3">
                                <asp:Label ID="Label6" runat="server" Text="Nombre del funcionario" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>
                            <div class="col-md-5 col-xs-12 col-sm-5">
                                <asp:TextBox class="form-control" ID="TextBox1" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <div class="col-md-3 col-xs-12 col-sm-3">
                                <asp:Label ID="Label8" runat="server" Text="Salario" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>
                            <div class="col-md-5 col-xs-12 col-sm-5">
                                <div class="input-group">
                                    <span class="input-group-addon">₡</span>
                                    <asp:TextBox class="form-control" ID="tb1" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-success" ID="Button2" runat="server" OnClick="Button2_Click" Text="Editar" />
                    <asp:Button CssClass="btn btn-danger" data-dismiss="modal" runat="server" Text="Cancelar" />
                    <%--<button type="button" class="btn btn-secondary" >Cerrar</button>--%>
                </div>
            </div>
        </div>
    </div>


    <!-- modal pasar funcionarios -->

    <%--<div class="modal fade" id="modalPasarFuncionario" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">--%>
         <div id="modalPasarFuncionario" class="modal fade" role="alertdialog">
        <div class="modal-dialog" role="document" style="width: 98% !important">
            <div class="modal-content">
                <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                <ContentTemplate>
                <div class="modal-header">
                    <h5>
                        <asp:Label ID="Label2" runat="server" Text="Copiar funcionario" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body" style="align-content: center">
                    <div class="row">
                        <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>--%>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <div class="col-md-6 col-xs-6 col-sm-6">
                                        <asp:Label ID="Label13" runat="server" Text="Período seleccionado" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-6 col-xs-6 col-sm-6">
                                        <asp:Label ID="Label19" runat="server" Text="Período a pasar los funcionarios" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
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
                                        <asp:DropDownList ID="ddlPeriodo" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <!-- ------------------------ tabla funcionarios a pasar --------------------------- -->
                                    <div class=" table-responsive col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">

                                        <table id="tblFuncionariosAPasar" class="table table-bordered">
                                            <thead>
                                                <tr style="text-align: center" class="btn-primary">
                                                    <th></th>
                                                    <th>Nombre</th>
                                                    <th>Salario</th>
                                                </tr>
                                            </thead>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="btnFiltrarEscalasAPasar" runat="server" CssClass="btn btn-primary" OnClick="txtBuscarNombreFuncionarioAPasar_TextChanged"><span aria-hidden="true" class="glyphicon glyphicon-search" ></span> </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>--%>
                                                            <asp:TextBox ID="txtBuscarNombreFuncionarioAPasar" runat="server" CssClass="form-control chat-input" placeholder="filtro nombre"  onkeypress="enter3_click()"></asp:TextBox>
                                                        <%--</ContentTemplate>--%>
                                                       <%-- <Triggers>
                                                            <asp:PostBackTrigger ControlID="txtBuscarNombreFuncionarioAPasar" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>--%>
                                                </td>
                                                <td></td>
                                            </tr>

                                            <asp:Repeater ID="rpFuncionariosAPasar" runat="server">
                                                <HeaderTemplate>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <tr style="text-align: center">
                                                        <td>
                                                            <asp:LinkButton ID="btnSeleccionarFuncionario" runat="server" ToolTip="Copiar funcionario" CommandArgument='<%# Eval("idFuncionario") %>' OnClick="btnSeleccionarFuncionario_Click"><span class="glyphicon glyphicon-share-alt"></span></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <%# Eval("nombre") %>
                                                        </td>
                                                        <td>₡ <%# Eval("salario") %>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                    <!-- ---------------------- FIN tabla funcionarios a pasar ------------------------- -->
                                    <!-- ------------------------ tabla funcionarios agregadas pasar --------------------------- -->
                                    <div class=" table-responsive col-md-6 col-xs-6 col-sm-6" style="text-align: center; overflow-y: auto;">

                                        <table id="tblFuncionariosAgregados" class="table table-bordered">
                                            <thead>
                                                <tr style="text-align: center" class="btn-primary">
                                                    <th></th>
                                                    <th>Nombre</th>
                                                    <th>Salario</th>
                                                </tr>
                                            </thead>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="btnFiltrarEscalasAgregadas" runat="server" CssClass="btn btn-primary" OnClick="txtBuscarNombreFuncionarioAgregadas_TextChanged"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton></td>
                                                <td>
                                                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>--%>
                                                            <asp:TextBox ID="txtBuscarNombreFuncionarioAgregadas" runat="server" CssClass="form-control chat-input" placeholder="filtro descripción"  onkeypress="enter2_click()" ></asp:TextBox>
                                                       <%-- </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="txtBuscarNombreFuncionarioAgregadas" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>--%>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <asp:Repeater ID="rpFuncionariosAgregados" runat="server">
                                                <HeaderTemplate>
                                                </HeaderTemplate>

                                                <ItemTemplate>
                                                    <tr style="text-align: center">
                                                        <td></td>
                                                        <td>
                                                            <%# Eval("nombre") %>
                                                        </td>
                                                        <td>₡ <%# Eval("salario") %>
                                                        </td>
                                                    </tr>

                                                </ItemTemplate>

                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                    <!-- ---------------------- FIN tabla funcionarios agregadas pasar ------------------------- -->
                                </div>
                                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                    <!-- ---------------------- tabla paginacion funcionarios a pasar ------------------------- -->
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
                                    <!-- ---------------------- FIN tabla paginacion funcionarios a pasar ------------------------- -->

                                    <!-- ---------------------- tabla paginacion funcionarios agregados ------------------------- -->
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
                                    <!-- ---------------------- FIN tabla paginacion funcionarios agregados ------------------------- -->
                                </div>

                                </div>
                          <%--  </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    
                    <div class="modal-footer" style="align-content: center; text-align: center">
                        <asp:Button CssClass="btn btn-danger" data-dismiss="modal" runat="server" Text="Cerrar" />
                    </div>
                            </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- fin modal pasar funcionarios-->

        <!-- modal nuevo funcionario -->
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div class="modal fade" id="modalNuevoFuncionario" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5>
                                    <asp:Label ID="Label3" runat="server" Text="Nuevo funcionario" Font-Size="Large" ForeColor="Black"></asp:Label>
                                </h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body" style="align-content: center">
                                <div class="row">
                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                            <asp:Label ID="Label4" runat="server" Text="Planilla" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>

                                        <div class="col-md-5 col-xs-5 col-sm-5">
                                            <asp:Label ID="lblPlanillaModalNuevo" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                        <div class="col-md-3 col-xs-12 col-sm-3">
                                            <asp:Label ID="Label5" runat="server" Text="Nombre completo" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>
                                        <div class="col-md-5 col-xs-12 col-sm-5">
                                            <asp:TextBox class="form-control" ID="txtNombreFuncionarioModalNuevo" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12">
                                        <br />
                                    </div>

                                    <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                                        <div class="col-md-3 col-xs-3 col-sm-3">
                                            <asp:Label ID="Label7" runat="server" Text="Salario" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                        </div>

                                        <div class="col-md-5 col-xs-12 col-sm-5">
                                            <div class="input-group">
                                                <span class="input-group-addon">₡</span>
                                                <asp:TextBox class="form-control" ID="txtSalarioModalNuevo" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                            </div>
                            <div class="modal-footer">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnNuevoFuncionarioModal" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnNuevoFuncionarioModal_Click" />
                                        <asp:Button CssClass="btn btn-danger" data-dismiss="modal" runat="server" Text="Cerrar" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnNuevoFuncionarioModal" />
                                    </Triggers>
                                </asp:UpdatePanel>

                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- fin modal nuevo funcionario-->

        <script type="text/javascript">
            function activarModal() {
                $('#modalAjuste').modal('show');
            };

            function activarModalEliminar() {
                $('#modalEliminarFuncionario').modal('show');
            };

            function activarModalEditar() {
                $('#modalEditarFuncionario').modal('show');
            };

            function activarModalPasarFuncionario() {
                $('#modalPasarFuncionario').modal('show');
            };

            function activarModalNuevoFuncionario() {
                $('#modalNuevoFuncionario').modal('show');
            };
             function enter3_click() {
                 if (window.event.keyCode == 13) {
                     document.getElementById('<%=btnFiltrarEscalasAPasar.ClientID%>').focus();
                document.getElementById('<%=btnFiltrarEscalasAPasar.ClientID%>').click();
                 }
            }

            function enter2_click() {
                 if (window.event.keyCode == 13) {
                     document.getElementById('<%=btnFiltrarEscalasAgregadas.ClientID%>').focus();
                document.getElementById('<%=btnFiltrarEscalasAgregadas.ClientID%>').click();
                 }
             }
        </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
