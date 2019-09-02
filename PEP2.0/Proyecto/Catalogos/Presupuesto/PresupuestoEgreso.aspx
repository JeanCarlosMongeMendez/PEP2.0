<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="PresupuestoEgreso.aspx.cs" Inherits="Proyecto.Catalogos.Presupuesto.PresupuestoEgreso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" EnableCdn="true"/>

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
                                        <th>Partida</th>
                                        <th>Total</th>
                                        <th>Monto</th>
                                        <th>Descripción</th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>

                        <ItemTemplate>
                            <tr style="text-align: center">
                                <td>
                                    <asp:HiddenField runat="server" ID="HFIdPartida" Value='<%# Eval("idPartida") %>' />
                                    <%# Eval("numeroPartida") %>
                                    
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="LBTotal" Text=<%# String.Format("{0:N}", "0") %>></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox class="form-control" ID="TbMonto" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox class="form-control" ID="TbDescripcion" runat="server"></asp:TextBox>
                                </td>
                            </tr>

                        </ItemTemplate>

                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                    <%-- fin tabla--%>
            </center>
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12">
            <div class="row">
                <div class="col-md-6 col-xs-6 col-sm-6">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="Guardar_Click"/>
                </div>

                <div class="col-md-6 col-xs-6 col-sm-6 alinear-derecha">
                    <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" CssClass="btn btn-primary" OnClick="Aprobar_Click"/>
                </div>
            </div>
        </div> 
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
