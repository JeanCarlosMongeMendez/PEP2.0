<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarFuncionariosPlanilla.aspx.cs" Inherits="Proyecto.Planilla.AdministrarFuncionariosPlanilla" MaintainScrollPositionOnPostback="true" %>

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
                                        <asp:LinkButton ID="btnSelccionar" OnClick="btnSelccionar_Click" runat="server" ToolTip="Seleccionar" CommandArgument='<%# Eval("idFuncionario") %>'><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                                        <asp:LinkButton ID="btnEditar" OnClick="btnEditar_Click" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idFuncionario") %>'><span class="btn glyphicon glyphicon-pencil"></span></asp:LinkButton>
                                        <asp:LinkButton ID="btnEliminar" OnClick="btnEliminar_Click" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idFuncionario") %>'><span class="btn glyphicon glyphicon-trash"></span></asp:LinkButton>
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


    <!-- Modal funcionario -->
    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>--%>
    <div id="modalNuevoFuncionario" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg" style="min-width: 95%; margin: 2%">
            <asp:HiddenField ID="hdIdFuncionario" runat="server" />
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header" style="background-color: #005da4; color: white">
                    <button type="button" class="close" data-dismiss="modal" style="color: white">&times;</button>
                    <h4 class="modal-title" id="tituloModalFuncionario" runat="server">Nuevo funcionario</h4>
                </div>
                <div class="modal-body">
                    <%-- campos a llenar --%>
                    <div class="row">


                        <%-- Datos generales --%>
                        <asp:Panel ID="panelDatosGenerales" runat="server" CssClass="col-md-12 col-xs-12 col-sm-12">

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-3">
                                    <asp:Label runat="server" Text="Nombre Completo" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-6 col-xs-12 col-sm-12">
                                    <asp:TextBox CssClass="form-control" ID="txtNombreCompleto" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Escala salarial" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-9 col-xs-12 col-sm-12" style="text-align: left">
                                    <asp:DropDownList ID="ddlEscalaSalarial" class="btn btn-default dropdown-toggle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEscalaSalarial_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Fecha ingreso <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-3 col-xs-12 col-sm-12 input-group date" id="divFecha">
                                    <span class="input-group-addon">
                                        <span class="fa fa-calendar"></span>
                                    </span>
                                    <asp:TextBox CssClass="form-control" ID="txtFecha" runat="server" placeholder="dd/mm/yyyy"></asp:TextBox>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Jornada laboral <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-9 col-xs-12 col-sm-12" style="text-align: left">
                                    <asp:DropDownList ID="ddlJornadaLaboral" class="btn btn-default dropdown-toggle" runat="server"></asp:DropDownList>
                                </div>

                            </div>

                        </asp:Panel>

                        <%-- Division --%>
                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                            <hr />
                            <br />
                        </div>

                        <%-- Primer semestre --%>
                        <asp:Panel ID="panelPrimerSemestre" runat="server" CssClass="col-md-6 col-xs-12 col-sm-12" Style="text-align: center">

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <asp:Label runat="server" Text="Datos para el primer semestre" Font-Size="Medium" ForeColor="Black" CssClass="label col-xs-12"></asp:Label>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Salario base --%>
                            <div class="row">
                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Salario base I <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-4 col-xs-6 col-sm-6" style="text-align: center">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtSalarioBase1" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>


                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-4 col-xs-6 col-sm-6" style="text-align: center">
                                            <div class="input-group">
                                                <span class="input-group-addon">+</span>
                                                <asp:TextBox ID="txtSumaSalarioBase1" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                                <span class="input-group-addon">%</span>
                                            </div>
                                        </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <%-- Total Salario base --%>
                                <div class="row">

                                    <div class="col-md-4 col-xs-12 col-sm-12">
                                        <asp:Label runat="server" Text="Total salario base I" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-8 col-xs-12 col-sm-12">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox ID="txtSumaTotalSalarioBase1" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <asp:LinkButton ID="btnCalcularTotalSalarioBaseI" runat="server" OnClick="btnCalcularTotalSalarioBase_Click">Calcular</asp:LinkButton>
                                    </div>

                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <%-- Escalafones --%>
                                <div class="row">

                                    <div class="col-md-4 col-xs-12 col-sm-12">
                                        <asp:Label runat="server" Text="Número escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-8 col-xs-12 col-sm-12">
                                        <div class="input-group">
                                            <span class="input-group-addon">#</span>
                                            <asp:TextBox class="form-control" ID="txtEscalafonesI" runat="server" TextMode="Number" min="0"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <%-- Monto Escalafones --%>
                                <div class="row">
                                    <div class="col-md-4 col-xs-12 col-sm-12">
                                        <asp:Label runat="server" Text="Monto escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-8 col-xs-12 col-sm-12">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox ID="txtMontoEscalafonesI" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <asp:LinkButton ID="btnCalcularEscalafonesI" runat="server" OnClick="btnCalcularEscalafones_Click">Calcular</asp:LinkButton>
                                    </div>
                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <%-- Porcentaje anualidades --%>
                                <div class="row">

                                    <div class="col-md-4 col-xs-12 col-sm-12">
                                        <asp:Label runat="server" Text="Porcentaje anualidades <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-8 col-xs-12 col-sm-12">
                                        <div class="input-group">
                                            <asp:TextBox class="form-control" ID="txtPorcentajeAnualidadesI" runat="server"></asp:TextBox>
                                            <span class="input-group-addon">%</span>
                                        </div>
                                    </div>

                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <%-- Monto anualidades --%>
                                <div class="row">

                                    <div class="col-md-4 col-xs-12 col-sm-12">
                                        <asp:Label runat="server" Text="Monto anualidades <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-8 col-xs-12 col-sm-12">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox ID="txtMontoAnualidadesI" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <asp:LinkButton ID="btnCalcularMontoAnualidadesI" runat="server" OnClick="btnCalcularMontoAnualidades_Click">Calcular</asp:LinkButton>
                                    </div>

                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <%-- Salario contatacion --%>
                                <div class="row">

                                    <div class="col-md-4 col-xs-12 col-sm-12">
                                        <asp:Label runat="server" Text="Salario Contratación <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-8 col-xs-12 col-sm-12">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox ID="txtSalContratacionI" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <asp:LinkButton ID="btnCalcularSalContratacionI" runat="server" OnClick="btnCalcularSalContratacion_Click">Calcular</asp:LinkButton>
                                    </div>

                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <%-- Ley 8114 --%>
                                <div class="row">

                                    <div class="col-md-4 col-xs-12 col-sm-12">
                                        <asp:Label runat="server" Text="Pago de Ley 8114" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                    </div>

                                    <div class="col-md-8 col-xs-12 col-sm-12">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox ID="txtPagoLey8114" runat="server" class="form-control" TextMode="Number"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>

                                <%-- Salario mensual Ene-Jun --%>
                                <div class="row">

                                    <div class="col-md-4 col-xs-12 col-sm-12">
                                        <asp:Label runat="server" Text="Salario Enero - Junio <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label" AutoSize="true"></asp:Label>
                                    </div>

                                    <div class="col-md-8 col-xs-12 col-sm-12">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox ID="txtSalarioMensualEneroJunio" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <asp:LinkButton ID="btnCalcularSalarioMensualI" runat="server" OnClick="btnCalcularSalarioMensualPorSemestre_Click">Calcular</asp:LinkButton>
                                    </div>

                                </div>

                                <div class="col-md-12 col-xs-12 col-sm-12">
                                    <br />
                                </div>
                        </asp:Panel>

                        <%-- Segundo semestre --%>
                        <asp:Panel ID="panelSegundoSemestre" runat="server" CssClass="col-md-6 col-xs-12 col-sm-12" Style="text-align: center">

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Titulo --%>
                            <asp:Label runat="server" Text="Datos para el segundo semestre" Font-Size="Medium" ForeColor="Black" CssClass="label col-xs-12"></asp:Label>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Salario base --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Salario base II <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtSalarioBase2" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <%--                           <div class="col-md-4 col-xs-6 col-sm-6">
                                            <div class="input-group">
                                                <span class="input-group-addon">+</span>
                                                <asp:TextBox ID="txtSumaSalarioBase2" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>--%>
                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Total Salario base --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Total salario base II" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtSumaTotalSalarioBaseII" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <asp:LinkButton ID="btnCalcularTotalSalarioBaseII" runat="server" OnClick="btnCalcularTotalSalarioBase_Click">Calcular</asp:LinkButton>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Escalafones --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Número escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">#</span>
                                        <asp:TextBox class="form-control" ID="txtEscalafonesII" runat="server" TextMode="Number" min="0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <asp:LinkButton ID="btnCalcularNumeroEscalafones2" runat="server" OnClick="btnCalcularNumeroEscalafones2_Click">Calcular</asp:LinkButton>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Monto escalafones --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Monto escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtMontoEscalafonesII" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <asp:LinkButton ID="btnCalcularEscalafonesII" runat="server" OnClick="btnCalcularEscalafones_Click">Calcular</asp:LinkButton>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Porcentaje anualidades --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Porcentaje anualidades <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <asp:TextBox class="form-control" ID="txtPorcentajeAnualidadesII" runat="server"></asp:TextBox>
                                        <span class="input-group-addon">%</span>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Monto anualidades --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Monto anualidades <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtMontoAnualidadesII" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <asp:LinkButton ID="btnCalcularMontoAnualidadesII" runat="server" OnClick="btnCalcularMontoAnualidades_Click">Calcular</asp:LinkButton>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Salario contratacion --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Salario Contratación <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtSalContratacionII" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <asp:LinkButton ID="btnCalcularSalContratacionII" runat="server" OnClick="btnCalcularSalContratacion_Click">Calcular</asp:LinkButton>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Salario mensual Jun-Dic --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Salario Junio - Dic. <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label" AutoSize="true"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtSalarioMensualJunioDiciembre" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <asp:LinkButton ID="btnCalcularSalarioMensualII" runat="server" OnClick="btnCalcularSalarioMensualPorSemestre_Click">Calcular</asp:LinkButton>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>
                        </asp:Panel>

                        <%-- Division --%>
                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                            <hr />
                            <br />
                        </div>

                        <%-- Datos generales --%>
                        <asp:Panel ID="panelDatosGeneralesFinales" runat="server" CssClass="col-md-12 col-xs-12 col-sm-12">

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Promedio de semestres" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-6 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtPromedioSemestres" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <asp:LinkButton ID="btnCalcularPromedioSemestres" runat="server" OnClick="btnCalcularPromedioSemestres_Click">Calcular</asp:LinkButton>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Salario propuesto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-6 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtSalarioPropuesto" runat="server" class="form-control" TextMode="Number"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Observaciones" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-6 col-xs-12 col-sm-12">
                                    <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" TextMode="MultiLine"></asp:TextBox>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                        </asp:Panel>

                        <div class="col-xs-12">
                            <h6 style="text-align: left">Los campos marcados con <span style='color: red'>*</span> son requeridos.</h6>
                        </div>
                    </div>

                    <%-- Fin campos a llenar --%>
                </div>
                <div class="modal-footer" style="text-align: center">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <!-- Fin modal nuevo funcionario -->



    <!-- Modal Ver funcionario -->
    <div id="modalVerFuncionario" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg" style="min-width: 95%; margin: 2%">
            <asp:HiddenField ID="hdIdEliminarFuncionario" runat="server" />
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header" style="background-color: #005da4; color: white">
                    <button type="button" class="close" data-dismiss="modal" style="color: white">&times;</button>
                    <h4 class="modal-title" id="tituloVerFuncionario" runat="server">Ver funcionario</h4>
                </div>
                <div class="modal-body">
                    <%-- campos a llenar --%>
                    <div class="row">

                        <%-- Datos generales --%>
                        <asp:Panel ID="panel1" runat="server" CssClass="col-md-12 col-xs-12 col-sm-12">

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-3">
                                    <asp:Label runat="server" Text="Nombre Completo" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-6 col-xs-12 col-sm-12">
                                    <asp:TextBox CssClass="form-control" ID="txtVerNombreCompleto" runat="server" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-3">
                                    <asp:Label runat="server" Text="Escala salarial" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-6 col-xs-12 col-sm-12">
                                    <asp:TextBox CssClass="form-control" ID="txtVerEscalaSalarial" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Fecha ingreso <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-6 col-xs-12 col-sm-12">
                                    <asp:TextBox CssClass="form-control" ID="txtVerFecha" runat="server" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>

                        </asp:Panel>

                        <%-- Division --%>
                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                            <hr />
                            <br />
                        </div>

                        <%-- Primer semestre --%>
                        <asp:Panel ID="panel2" runat="server" CssClass="col-md-6 col-xs-12 col-sm-12" Style="text-align: center">

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <asp:Label runat="server" Text="Datos del primer semestre" Font-Size="Medium" ForeColor="Black" CssClass="label col-xs-12"></asp:Label>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Salario base --%>
                            <div class="row">
                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Salario base I <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-4 col-xs-6 col-sm-6" style="text-align: center">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerSalarioBaseI" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-4 col-xs-6 col-sm-6" style="text-align: center">
                                    <div class="input-group">
                                        <span class="input-group-addon">+</span>
                                        <asp:TextBox ID="txtVerSumaSalarioBase1" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Total Salario base --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Total salario base I" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerSumaTotalSalarioBase1" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Escalafones --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Número escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">#</span>
                                        <asp:TextBox class="form-control" ID="txtVerEscalafonesI" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Monto Escalafones --%>
                            <div class="row">
                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Monto escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerMontoEscalafonesI" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Porcentaje anualidades --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Porcentaje anualidades <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <asp:TextBox class="form-control" ID="txtVerPorcentajeAnualidadesI" runat="server" ReadOnly="true"></asp:TextBox>
                                        <span class="input-group-addon">%</span>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Monto anualidades --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Monto anualidades <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerMontoAnualidadesI" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Salario contatacion --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Salario Contratación <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerSalContratacionI" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Ley 8114 --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Pago de Ley 8114" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerPagoLey8114" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Salario mensual Ene-Jun --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Salario Enero - Junio <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label" AutoSize="true"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerSalarioMensualEneroJunio" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>
                        </asp:Panel>

                        <%-- Segundo semestre --%>
                        <asp:Panel ID="panel3" runat="server" CssClass="col-md-6 col-xs-12 col-sm-12" Style="text-align: center">

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Titulo --%>
                            <asp:Label runat="server" Text="Datos del segundo semestre" Font-Size="Medium" ForeColor="Black" CssClass="label col-xs-12"></asp:Label>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Salario base --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Salario base II <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-4 col-xs-6 col-sm-6">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerSalarioBase2" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-4 col-xs-6 col-sm-6">
                                    <div class="input-group">
                                        <span class="input-group-addon">+</span>
                                        <asp:TextBox ID="txtVerSumaSalarioBase2" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Total Salario base --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Total salario base II" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerSumaTotalSalarioBaseII" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Escalafones --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Número escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">#</span>
                                        <asp:TextBox class="form-control" ID="txtVerEscalafonesII" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Monto escalafones --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Monto escalafones <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerMontoEscalafonesII" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Porcentaje anualidades --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Porcentaje anualidades <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <asp:TextBox class="form-control" ID="txtVerPorcentajeAnualidadesII" runat="server" ReadOnly="true"></asp:TextBox>
                                        <span class="input-group-addon">%</span>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Monto anualidades --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Monto anualidades <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerMontoAnualidadesII" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Salario contratacion --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Salario Contratación <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerSalContratacionII" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <%-- Salario mensual Jun-Dic --%>
                            <div class="row">

                                <div class="col-md-4 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Salario Junio - Dic. <span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label" AutoSize="true"></asp:Label>
                                </div>

                                <div class="col-md-8 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerSalarioMensualJunioDiciembre" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>
                        </asp:Panel>

                        <%-- Division --%>
                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <br />
                            <hr />
                            <br />
                        </div>

                        <%-- Datos generales --%>
                        <asp:Panel ID="panel4" runat="server" CssClass="col-md-12 col-xs-12 col-sm-12">

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Promedio de semestres" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-6 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerPromedioSemestres" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Salario propuesto" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-6 col-xs-12 col-sm-12">
                                    <div class="input-group">
                                        <span class="input-group-addon">₡</span>
                                        <asp:TextBox ID="txtVerSalarioPropuesto" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                            <div class="row" style="text-align: center">

                                <div class="col-md-3 col-xs-12 col-sm-12">
                                    <asp:Label runat="server" Text="Observaciones" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                                </div>

                                <div class="col-md-6 col-xs-12 col-sm-12">
                                    <asp:TextBox ID="txtVerObservaciones" runat="server" class="form-control" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>

                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <br />
                            </div>

                        </asp:Panel>
                    </div>

                    <%-- Fin campos a llenar --%>
                </div>
                <div class="modal-footer" style="text-align: center">
                    <asp:Button ID="btnConfirmarEliminar" runat="server" Text="Eliminar" CssClass="btn btn-primary" OnClick="btnConfirmarEliminar_Click" />
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

        function activarModalVerFuncionario() {
            $('#modalVerFuncionario').modal('show');
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
