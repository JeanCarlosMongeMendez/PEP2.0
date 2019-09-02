<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarFuncionariosPlanilla.aspx.cs" Inherits="Proyecto.Planilla.AdministrarFuncionariosPlanilla" MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>

    <div class="row">

        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
            <asp:Label ID="label" runat="server" Text="Período" Font-Size="Large" ForeColor="Black"></asp:Label>
            <asp:Label ID="lblPeriodo" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
            <br />
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
            <asp:Label ID="label1" runat="server" Text="Anualidad I semestre" Font-Size="Large" ForeColor="Black"></asp:Label>
            <asp:Label ID="lblAnualidad1" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
            <br />
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
            <asp:Label ID="label2" runat="server" Text="Anualidad II semestre" Font-Size="Large" ForeColor="Black"></asp:Label>
            <asp:Label ID="lblAnualidad2" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
            <hr />
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12">
            <div class="col-md-4 col-xs-4 col-sm-4">
                <asp:Button ID="btnNuevoFuncionario" runat="server" Text="Nuevo funcionario" CssClass="btn btn-primary boton-nuevo" OnClick="btnNuevoFuncionario_Click" />
            </div>
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12" style="align-content: center; text-align: center;">
            <hr />
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <%-- tabla--%>

                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <table class="table table-bordered">
                        <thead style="text-align: center">
                            <tr style="text-align: center" class="btn-primary">
                                <th></th>
                                <th>Nombre</th>
                            </tr>
                        </thead>
                        <tr>
                            <td>
                                <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary" OnClick="btnFiltrar_Click"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton></td>
                            <td>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-search"></i></span>
                                    <asp:TextBox ID="txtBuscarNombre" runat="server" CssClass="form-control chat-input" placeholder="filtro nombre"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <asp:Repeater ID="rpFuncionarios" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
                                        <asp:LinkButton ID="btnSelccionar" runat="server" ToolTip="Seleccionar" CommandArgument='<%# Eval("idFuncionario") %>'><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                                        <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idFuncionario") %>'><span class="btn glyphicon glyphicon-pencil"></span></asp:LinkButton>
                                        <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idFuncionario") %>'><span class="btn glyphicon glyphicon-trash"></span></asp:LinkButton>
                                    </td>
                                    <td>
                                        <%# Eval("nombreFuncionario") %>
                                    </td>
                                </tr>

                            </ItemTemplate>

                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center; align-content: center; overflow-y: auto;">
                    <center>
                    <table class="table" style="max-width: 664px;">
                        <tr style="padding: 1px !important">
                            <td style="padding: 1px !important">
                                <asp:LinkButton ID="lbPrimero" runat="server" CssClass="btn btn-primary" OnClick="lbPrimero_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                            </td>
                            <td style="padding: 1px !important">
                                <asp:LinkButton ID="lbAnterior" runat="server" CssClass="btn btn-default" OnClick="lbAnterior_Click"><span class="glyphicon glyphicon-backward"></asp:LinkButton>
                            </td>
                            <td style="padding: 1px !important">
                                <asp:DataList ID="rptPaginacion" runat="server"
                                    OnItemCommand="rptPaginacion_ItemCommand"
                                    OnItemDataBound="rptPaginacion_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaginacion" runat="server" CssClass="btn btn-default"
                                            CommandArgument='<%# Eval("IndexPagina") %>' CommandName="nuevaPagina"
                                            Text='<%# Eval("PaginaText") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td style="padding: 1px !important">
                                <asp:LinkButton ID="lbSiguiente" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente_Click"><span class="glyphicon glyphicon-forward"></asp:LinkButton>
                            </td>
                            <td style="padding: 1px !important">
                                <asp:LinkButton ID="lbUltimo" CssClass="btn btn-primary" runat="server" OnClick="lbUltimo_Click"><span class="glyphicon glyphicon-fast-forward"></asp:LinkButton>
                            </td>
                            <td style="padding: 1px !important">
                                <asp:Label ID="lblpagina" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                </div>

                <%-- fin tabla--%>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="col-md-2 col-xs-2 col-sm-2 col-md-offset-10 col-xs-offset-10 col-sm-offset-10">
            <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CssClass="btn btn-danger boton-nuevo" OnClick="btnRegresar_Click" />
        </div>

    </div>


    <!-- Modal nuevo funcionario -->
    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>--%>
    <div id="modalNuevoFuncionario" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Nuevo funcionario</h4>
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
                                <asp:Label ID="Label4" runat="server" Text="Escala salarial" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>

                            <div class="col-md-4 col-xs-4 col-sm-4">
                                <asp:DropDownList ID="ddlEscalaSalarial" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEscalaSalarial_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                            <div class="col-md-3 col-xs-3 col-sm-3">
                                <asp:Label ID="Label3" runat="server" Text="Fecha ingreso <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>

                            <div class="col-md-4 col-xs-4 col-sm-4 input-group date" id="divFecha">
                                <span class="input-group-addon">
                                    <span class="fa fa-calendar"></span>
                                </span>
                                <asp:TextBox CssClass="form-control" ID="txtFecha" runat="server" placeholder="dd/mm/yyyy"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                            <div class="col-md-3 col-xs-3 col-sm-3">
                                <asp:Label ID="Label5" runat="server" Text="Salario base I <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>

                            <div class="col-md-4 col-xs-4 col-sm-4">
                                <div class="input-group">
                                    <span class="input-group-addon">₡</span>
                                    <asp:TextBox ID="txtSalarioBase1ModalNuevo" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                            <div class="col-md-3 col-xs-3 col-sm-3">
                                <asp:Label ID="Label9" runat="server" Text="Suma a salario base I" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>

                            <div class="col-md-4 col-xs-4 col-sm-4">
                                <div class="input-group">
                                    <span class="input-group-addon">₡</span>
                                    <asp:TextBox ID="txtSumaSalarioBase1" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                            <div class="col-md-3 col-xs-3 col-sm-3">
                                <asp:Label ID="Label6" runat="server" Text="Salario base II <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>

                            <div class="col-md-4 col-xs-4 col-sm-4">
                                <div class="input-group">
                                    <span class="input-group-addon">₡</span>
                                    <asp:TextBox ID="txtSalarioBase2ModalNuevo" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                            <div class="col-md-3 col-xs-3 col-sm-3">
                                <asp:Label ID="Label10" runat="server" Text="Suma a salario base II" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>

                            <div class="col-md-4 col-xs-4 col-sm-4">
                                <div class="input-group">
                                    <span class="input-group-addon">₡</span>
                                    <asp:TextBox ID="txtSumaSalarioBase2" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                            <div class="col-md-3 col-xs-3 col-sm-3">
                                <asp:Label ID="Label7" runat="server" Text="Número escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>

                            <div class="col-md-4 col-xs-4 col-sm-4">
                                <div class="input-group">
                                    <span class="input-group-addon">#</span>
                                    <asp:TextBox class="form-control" ID="txtEscalafones" runat="server" TextMode="Number" min="0"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                            <div class="col-md-3 col-xs-3 col-sm-3">
                                <asp:Label ID="Label8" runat="server" Text="Monto escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>

                            <div class="col-md-4 col-xs-4 col-sm-4">
                                <div class="input-group">
                                    <span class="input-group-addon">₡</span>
                                    <asp:TextBox ID="txtMontoEscalafones" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <asp:LinkButton ID="btnCalcularEscalafones" runat="server" OnClick="btnCalcularEscalafones_Click">Calcular</asp:LinkButton>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                            <div class="col-md-3 col-xs-3 col-sm-3">
                                <asp:Label ID="Label11" runat="server" Text="Porcentaje anualidades <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>

                            <div class="col-md-4 col-xs-4 col-sm-4">
                                <div class="input-group">
                                    <asp:TextBox class="form-control" ID="txtPorcentajeAnualidades" runat="server"></asp:TextBox>
                                    <span class="input-group-addon">%</span>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                            <div class="col-md-3 col-xs-3 col-sm-3">
                                <asp:Label ID="Label12" runat="server" Text="Monto anualidades <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                            </div>

                            <div class="col-md-4 col-xs-4 col-sm-4">
                                <div class="input-group">
                                    <span class="input-group-addon">₡</span>
                                    <asp:TextBox ID="txtMontoAnualidades" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <asp:LinkButton ID="btnCalcularMontoAnualidades" runat="server" OnClick="btnCalcularMontoAnualidades_Click">Calcular</asp:LinkButton>
                            </div>
                        </div>

                        <div class="col-xs-12">
                            <h6 style="text-align: left">Los campos marcados con <span style='color: red'>*</span> son requeridos.</h6>
                        </div>
                    </div>

                </div>
                <div class="modal-footer" style="text-align: center">
                    <asp:Button ID="btnGuardarNuevoFuncionario" runat="server" Text="Guardar" CssClass="btn btn-primary" />
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <!-- Fin modal nuevo funcionario -->

    <script src="../Scripts/moment.js"></script>
    <script src="../Scripts/transition.js"></script>
    <script src="../Scripts/collapse.js"></script>
    <script src="../Scripts/bootstrap-datetimepicker.js"></script>
    <script src="../Scripts/bootstrap-datetimepicker.min.js"></script>

    <script type="text/javascript">
        function activarModalNuevoFuncionario() {
            $('#modalNuevoFuncionario').modal('show');
        };

        $(function () {
            // Fechas
            $('#divFecha').datetimepicker({
                format: 'DD/MM/YYYY',
                locale: moment.locale('es')
            });

        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>