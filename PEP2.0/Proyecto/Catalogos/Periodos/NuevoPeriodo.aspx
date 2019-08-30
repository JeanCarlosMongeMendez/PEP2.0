<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NuevoPeriodo.aspx.cs" Inherits="Proyecto.Catalogos.Periodos.NuevoPeriodo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divRedondo">
        <div class="row">

            <%-- titulo accion--%>
            <div class="col-md-12 col-xs-12 col-sm-12">
                <center>
                        <asp:Label ID="lblNuevoPeriodo" runat="server" Text="Nuevo Periodo" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </center>
            </div>
            <%-- fin titulo accion --%>

            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                <hr />
            </div>

            <%-- campos a llenar --%>
            <div class="col-md-12 col-xs-12 col-sm-12">

                <div class="col-md-2 col-xs-2 col-sm-2">
                    <asp:Label ID="lblAnoPeriodo" runat="server" Text="Año <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>
                <div class="col-md-8 col-xs-8 col-sm-8">
                    <asp:TextBox class="form-control" ID="txtAnoPeriodo" runat="server"></asp:TextBox>
                </div>
                <div id="divAnoPeriodoIncorrecto" runat="server" style="display: none" class="col-md-12 col-xs-12 col-sm-12">
                    <asp:Label ID="lblAnoPeriodoIncorrecto" runat="server" Font-Size="Small" class="label alert-danger" Text="Espacio Obligatorio" ForeColor="Red"></asp:Label>
                </div>
                <div id="divAnoPeriodoFormatoIncorrecto" runat="server" style="display: none" class="col-md-12 col-xs-12 col-sm-12">
                    <asp:Label ID="lblAnoPeriodoFormatoIncorrecto" runat="server" Font-Size="Small" class="label alert-danger" Text="Debe ingresar un número" ForeColor="Red"></asp:Label>
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

            var AnoPeriodoIncorrecto = document.getElementById('<%= divAnoPeriodoIncorrecto.ClientID %>');
            var AnoPeriodoFormatoIncorrecto = document.getElementById('<%= divAnoPeriodoFormatoIncorrecto.ClientID %>');

            if (txtBox.value.trim() != "") {
                txtBox.className = "form-control";
                AnoPeriodoIncorrecto.style.display = 'none';
            }else if (isNaN(txtBox.value)) {
                txtBox.className = "form-control alert-danger";
                AnoPeriodoFormatoIncorrecto.style.display = 'block';
            } else {
                txtBox.className = "form-control alert-danger";
                AnoPeriodoIncorrecto.style.display = 'block';
            }
        }
    </script>
</asp:Content>
