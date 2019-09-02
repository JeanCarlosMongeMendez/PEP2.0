<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarFuncionarioFundevi.aspx.cs" Inherits="Proyecto.Planilla.AdministrarFuncionarioFundevi" MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12 col-xs-12 col-sm-12">
        <center>
                        <asp:Label ID="label" runat="server" Text="Planillas Fundevi" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </center>
    </div>
    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
        <hr />
    </div>
    <div class="col-md-2 col-xs-2 col-sm-2 col-md-offset-10 col-xs-offset-10 col-sm-offset-10">
        <asp:Button ID="btnAsignar1" runat="server" Text="Asignar funcionarios" CssClass="btn btn-primary boton-nuevo" OnClick="btnAsignar_Click1" />
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
                    <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary"><span aria-hidden="true" class="glyphicon glyphicon-search"></span> </asp:LinkButton></td>
                <td>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-search"></i></span>
                        <asp:TextBox ID="txtBuscarNombre" runat="server" CssClass="form-control chat-input" placeholder="filtro nombre" OnTextChanged="txtBuscarNombre_TextChanged"></asp:TextBox>
                    </div>
                </td>
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
                        <td>
                            <asp:label runat="server" Text='<%# Eval("salario") %>' ></asp:label>

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
    </div>

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
                    <asp:Label runat="server">Nombre del funcionario:</asp:Label><br />
                    <asp:Label ID="lblNombre" runat="server"></asp:Label><br />
                    <asp:Label runat="server">Actual salario:</asp:Label><br />
                    <asp:Label ID="lblSalario" runat="server"></asp:Label><br />
                    <asp:Label runat="server">Ingrese el nuevo salario:</asp:Label><br />
                    <asp:TextBox runat="server" ID="txtAsalario"></asp:TextBox>

                    <asp:Label runat="server" ID="txtInfo"></asp:Label>
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
                    <asp:Label runat="server">Nombre del funcionario:</asp:Label><br />
                    <asp:Label ID="txtNomF" runat="server"></asp:Label><br />
                    <asp:Label runat="server">Salario:</asp:Label><br />
                    <asp:Label ID="txtSa" runat="server"></asp:Label><br />


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
                    <asp:Label runat="server">Nombre del funcionario:</asp:Label><br />
                    <asp:TextBox runat="server" ID="TextBox1"></asp:TextBox><br />

                    <asp:Label runat="server">Salario:</asp:Label><br />
                    <asp:TextBox runat="server" ID="tb1"></asp:TextBox><br />
                   <%-- %><asp:RegularExpressionValidator ID="REV1" Text="<p>*Solo se admiten números" 
                       ControlToValidate="tb1" Runat="server" Display="Dynamic" 
                       EnableClientScript="True" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                       --%>

                </div>
                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-success" ID="Button2" runat="server" OnClick="Button2_Click" Text="Editar" />
                    <asp:Button CssClass="btn btn-danger" data-dismiss="modal" runat="server" Text="Cancelar" />
                    <%--<button type="button" class="btn btn-secondary" >Cerrar</button>--%>
                   

                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function activarModal() {
            $('#modalAjuste').modal('show');
        };
    </script>
    <script type="text/javascript">
        function activarModalEliminar() {
            $('#modalEliminarFuncionario').modal('show');
        };
    </script>
    <script type="text/javascript">
        function activarModalEditar() {
            $('#modalEditarFuncionario').modal('show');
        };
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>