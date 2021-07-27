<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NuevoProyecto.aspx.cs" Inherits="Proyecto.Catalogos.Proyecto.NuevoProyecto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divRedondo">
        <div class="row">

            <%-- titulo accion--%>
            <div class="col-md-12 col-xs-12 col-sm-12">
                <center>
                        <asp:Label ID="lblNuevoProyecto" runat="server" Text="Nuevo Proyecto" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </center>
            </div>
            <%-- fin titulo accion --%>

            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                <hr />
            </div>

            <%-- campos a llenar --%>
            <div class="col-md-12 col-xs-12 col-sm-12">
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <asp:Label ID="lblPeriodoProyecto" runat="server" Text="Periodo<span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>
                <div class="col-md-8 col-xs-8 col-sm-8">
                    <asp:DropDownList ID="PeriodosDDL" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            
            <div class="col-md-12 col-xs-12 col-sm-12 mt-1">
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <asp:Label ID="lblNombreProyecto" runat="server" Text="Nombre del proyecto <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>
                <div class="col-md-8 col-xs-8 col-sm-8">
                    <asp:TextBox class="form-control" ID="txtNombreProyecto" runat="server"></asp:TextBox>
                </div>
                <div id="divNombreProyectoIncorrecto" runat="server" style="display: none" class="col-md-6 col-xs-6 col-sm-6">
                    <asp:Label ID="lblNombreProyectoIncorrecto" runat="server" Font-Size="Small" class="label alert-danger" Text="Espacio Obligatorio" ForeColor="Red"></asp:Label>
                </div>
            </div>

            <div class="col-md-12 col-xs-12 col-sm-12 mt-1">
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <asp:Label ID="lblCodigoProyecto" runat="server" Text="Código <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>
                <div class="col-md-8 col-xs-8 col-sm-8">
                    <asp:TextBox class="form-control" ID="txtCodigoProyecto" runat="server"></asp:TextBox>
                </div>
                <div id="divCodigoProyectoIncorrecto" runat="server" style="display: none" class="col-md-6 col-xs-6 col-sm-6">
                    <asp:Label ID="lblCodigoProyectoIncorrecto" runat="server" Font-Size="Small" class="label alert-danger" Text="Espacio Obligatorio" ForeColor="Red"></asp:Label>
                </div>
            </div>

            <div class="col-md-12 col-xs-12 col-sm-12 mt-1">
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <asp:Label ID="lblEsUCRProyecto" runat="server" Text="Tipo <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>
                <div class="col-md-8 col-xs-8 col-sm-8">
                    <asp:DropDownList ID="ddlEsUCRProyecto" runat="server" CssClass="form-control">
                        <asp:ListItem Text="UCR" Value="true" />
                        <asp:ListItem Text="FundacionUCR" Value="false" />
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
            <div class="col-md-3 col-xs-3 col-sm-3 col-md-offset-9 col-xs-offset-9 col-sm-offset-9">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" OnClick="btnCancelar_Click" />
            </div>
            <%-- fin botones --%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        /*
        Evalúa de manera inmediata los campos de texto que va ingresando el usuario.
        */
        function validarTexto(txtBox) {
            var id = txtBox.id.substring(12);
            var NombreProyectoIncorrecto = document.getElementById('<%= divNombreProyectoIncorrecto.ClientID %>');
            var CodigoProyectoIncorrecto = document.getElementById('<%= divCodigoProyectoIncorrecto.ClientID %>');

            if (txtBox.value.trim() != "") {
                txtBox.className = "form-control";
                NombreProyectoIncorrecto.style.display = 'none';
                CodigoProyectoIncorrecto.style.display = 'none';
            } else {
                txtBox.className = "form-control alert-danger";
                NombreProyectoIncorrecto.style.display = 'block';
                CodigoProyectoIncorrecto.style.display = 'block';
            }
        }
    </script>
</asp:Content>
