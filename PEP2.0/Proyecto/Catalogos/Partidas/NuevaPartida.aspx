<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="NuevaPartida.aspx.cs" Inherits="Proyecto.Catalogos.Partidas.NuevaPartida" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divRedondo">
        <div class="row">

            <%-- titulo accion--%>
            <div class="col-md-12 col-xs-12 col-sm-12">
                <center>
                        <asp:Label ID="lblNuevaPartida" runat="server" Text="Nueva Partida" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </center>
            </div>.
            <%-- fin titulo accion --%>

            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                <hr />
            </div>

            <%-- campos a llenar --%>
            <div class="col-md-12 col-xs-12 col-sm-12">
                <div class="col-md-4 col-xs-4 col-sm-4">
                    <asp:Label ID="lblPeriodoProyecto" runat="server" Text="Periodo<span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>
                <div class="col-md-8 col-xs-8 col-sm-8">
                    <asp:DropDownList ID="PeriodosDDL" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="col-md-12 col-xs-12 col-sm-12 mt-1">
                <div class="col-md-4 col-xs-4 col-sm-4">
                    <asp:Label ID="lblPadrePartida" runat="server" Text="Tipo o Padre de la partida <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>
                <div class="col-md-8 col-xs-8 col-sm-8">
                    <asp:DropDownList ID="PartidasPadreDDL" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>

            <div class="col-md-12 col-xs-12 col-sm-12 mt-1">
                <div class="col-md-4 col-xs-4 col-sm-4">
                    <asp:Label ID="lblNumeroPartida" runat="server" Text="Número de la partida <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>
                <div class="col-md-8 col-xs-8 col-sm-8">
                    <asp:TextBox class="form-control" ID="txtNumeroPartida" runat="server"></asp:TextBox>
                </div>
                <div id="divNumeroPartidaIncorrecto" runat="server" style="display: none" class="col-md-12 col-xs-12 col-sm-12">
                    <asp:Label ID="lblNumeroPartidaIncorrecto" runat="server" Font-Size="Small" class="label alert-danger" Text="Espacio Obligatorio" ForeColor="Red"></asp:Label>
                </div>
            </div>

            <div class="col-md-12 col-xs-12 col-sm-12 mt-1">
                <div class="col-md-4 col-xs-4 col-sm-4">
                    <asp:Label ID="lblDescripcionPartida" runat="server" Text="Descripción <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
                </div>
                <div class="col-md-8 col-xs-8 col-sm-8">
                    <asp:TextBox class="form-control" TextMode="multiline" Columns="50" Rows="5" ID="txtDescripcionPartida" runat="server"></asp:TextBox>
                </div>
                <div id="divDescripcionPartidaIncorrecto" runat="server" style="display: none" class="col-md-6 col-xs-6 col-sm-6">
                    <asp:Label ID="lblDescripcionPartidaIncorrecto" runat="server" Font-Size="Small" class="label alert-danger" Text="Espacio Obligatorio" ForeColor="Red"></asp:Label>
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
            var NumeroPartidaIncorrecto = document.getElementById('<%= divNumeroPartidaIncorrecto.ClientID %>');
            var DescripcionPartidaIncorrecto = document.getElementById('<%= divDescripcionPartidaIncorrecto.ClientID %>');

            if (txtBox.value.trim() != "") {
                txtBox.className = "form-control";
                NumeroPartidaIncorrecto.style.display = 'none';
                DescripcionPartidaIncorrecto.style.display = 'none';
            } else {
                txtBox.className = "form-control alert-danger";
                NumeroPartidaIncorrecto.style.display = 'block';
                DescripcionPartidaIncorrecto.style.display = 'block';
            }
        }
    </script>
</asp:Content>
