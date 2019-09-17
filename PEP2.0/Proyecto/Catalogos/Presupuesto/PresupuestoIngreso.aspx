<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="PresupuestoIngreso.aspx.cs" Inherits="Proyecto.Catalogos.Presupuesto.PresupuestoIngreso" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" EnableCdn="true" />
    <asp:UpdatePanel ID="pnlUpdate" runat="server">
        <ContentTemplate>

            <div class="row">

                <%-- titulo pantalla --%>
                <div class="col-md-12 col-xs-12 col-sm-12">
                    <center>
                        <asp:Label ID="label" runat="server" Text="Presupuesto de Ingreso" Font-Size="Large" ForeColor="Black"></asp:Label>
                            <p class="mt-1">Seleccione un periodo</p>
                    </center>
                </div>
                <%-- fin titulo pantalla --%>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 mt-1 form-group">
                    <asp:DropDownList AutoPostBack="true" ID="PeriodosDDL" runat="server" OnSelectedIndexChanged="PeriodosDDL_OnChanged" CssClass="form-control">
                    </asp:DropDownList>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                    <p class="mt-1">Ingrese el monto del presupuesto para cada proyecto</p>
                </div>

                <%-- tabla--%>
                <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <asp:Repeater ID="rpProyectos" runat="server">
                        <HeaderTemplate>
                            <table class="table table-bordered">
                                <thead style="text-align: center">
                                    <tr style="text-align: center" class="btn-primary">
                                        <th></th>
                                        <th>Nombre</th>
                                        <th>Código</th>
                                        <th>Tipo</th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr style="text-align: center">
                                <td>
                                    <div class="btn-group">
                                        <asp:HiddenField runat="server" ID="HFIdProyecto" Value='<%# Eval("idProyecto") %>' />
                                        <%--<asp:CheckBox ID="cbProyecto" runat="server" Text="" />--%>
                                        <asp:LinkButton ID="btnSelccionar" runat="server" ToolTip="Seleccionar" OnClick="btnSelccionar_Click" CommandArgument='<%# Eval("idProyecto") %>'><span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                                    </div>
                                </td>
                                <td>
                                    <%# Eval("nombreProyecto") %>
                                </td>
                                <td>
                                    <%# Eval("codigo") %>
                                </td>
                                <td>
                                    <%# (Eval("esUCR").ToString() == "True")? "UCR" : "FUNDEVI" %>
                                </td>
                            </tr>

                        </ItemTemplate>

                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <%-- fin tabla--%>

                <%--<div class="col-md-12 col-xs-12 col-sm-12">
                        <asp:Button ID="btnMostrar" runat="server" Text="Mostrar" CssClass="btn btn-primary" OnClick="Mostrar_Click"/>
                    </div>--%>
                
                <div id="divMontoPresupuesto" runat="server">

                    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 mt-2">
                        <hr />
                    </div>

                    <div class="col-md-12 col-xs-12 col-sm-12" style="align-content:center;text-align:center">
                        <asp:Label ID="Label1" runat="server" Text="Proyecto seleccionado:" Font-Size="Large" ForeColor="Black"></asp:Label>

                        <asp:Label ID="lblProyectoSeleccionado" runat="server" Text="" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </div>

                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <center>
                                <p class="mt-1">Ingrese el monto del presupuesto o edite un monto</p>
                            </center>
                    </div>

                    <div class="row mt-2 center">
                        <center>
                                <div class="col-md-4 col-xs-4 col-sm-4 form-group">
                                    <asp:Label runat="server" ID="TipoPresupuesto" Text="Tipo"></asp:Label>
                                </div>
                                <div class="col-md-4 col-xs-4 col-sm-4 form-group">
                                    <asp:TextBox runat="server" ID="montoPresupuesto" CssClass="form-control"></asp:TextBox>
                                    
                                    <div id="divMontoPresupuestoIncorrecto" runat="server" style="display: none" class="col-md-12 col-xs-12 col-sm-12">
                                        <asp:Label ID="lblMontoPresupuestoFormatoIncorrecto" runat="server" Font-Size="Small" class="label alert-danger" Text="Debe ingresar un número" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-4 col-xs-4 col-sm-4 form-group">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="Guardar_Click"/>
                                </div>
                            </center>
                    </div>


                    <div class="table-responsive col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                        <asp:Repeater ID="rpPresupuestos" runat="server">
                            <HeaderTemplate>
                                <table class="table table-bordered">
                                    <thead style="text-align: center">
                                        <tr style="text-align: center" class="btn-primary">
                                            <th>Tipo</th>
                                            <th>Monto</th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr style="text-align: center">
                                    <td>
                                        <%# (Eval("esInicial").ToString() == "True")? "Inicial" : "Adicional" %>
                                        <asp:HiddenField ID="HDIdPresupuestoIngreso" Value='<%# Eval("idPresupuestoIngreso") %>' runat="server" />
                                    </td>
                                    <td>
                                        <%# String.Format("{0:N}", Eval("monto") ) %>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" CssClass="btn btn-primary" OnClick="Aprobar_OnClick" CommandArgument='<%# Eval("idPresupuestoIngreso") %>'
                                            Enabled='<%# (Eval("estado").ToString() == "True")? Convert.ToBoolean("false") : Convert.ToBoolean("true") %>' />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-primary" OnClick="Eliminar_OnClick" CommandArgument='<%# Eval("idPresupuestoIngreso") %>'
                                            Enabled='<%# (Eval("estado").ToString() == "True")? Convert.ToBoolean("false") : Convert.ToBoolean("true") %>' />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        /*
        Evalúa de manera inmediata los campos de texto que va ingresando el usuario.
        */
        function validarTexto(txtBox) {
            var id = txtBox.id.substring(12);

            var MontoPresupuestoIncorrecto = document.getElementById('<%= divMontoPresupuestoIncorrecto.ClientID %>');

            if (txtBox.value.trim() != "") {
                txtBox.className = "form-control";
                MontoPresupuestoIncorrecto.style.display = 'none';
            } else if (isNaN(txtBox.value)) {
                txtBox.className = "form-control alert-danger";
                MontoPresupuestoIncorrecto.style.display = 'block';
            } else {
                txtBox.className = "form-control alert-danger";
                MontoPresupuestoIncorrecto.style.display = 'block';
            }
        }
    </script>
</asp:Content>
