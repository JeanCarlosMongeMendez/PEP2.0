<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AgregarFuncionarioFundevi.aspx.cs" Inherits="Proyecto.Planilla.AgregarFuncionarioFundevi" MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <%-- fin titulo accion --%>
        <h3>Registrar nuevo funcionario</h3>
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
            <hr />
        </div>

        <%-- campos a llenar --%>
        <%-- campos a llenar --%>

        <div class="col-md-12 col-xs-12 col-sm-12">

            <div class="col-md-3 col-xs-3 col-sm-3">
                <asp:Label ID="lblNombre" runat="server" Text="Nombre <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
            </div>
            <div class="col-md-4 col-xs-4 col-sm-4">
                <asp:TextBox class="form-control" ID="txtNombre" runat="server"></asp:TextBox>
            </div>
            <div id="divNombreIncorrecto" runat="server" style="display: none" class="col-md-5 col-xs-5 col-sm-5">
                <asp:Label ID="lblNombreIncorrecto" runat="server" Font-Size="Small" class="label alert-danger" Text="Espacio Obligatorio" ForeColor="Red"></asp:Label>
            </div>

        </div>

        <div class="col-md-12 col-xs-12 col-sm-12">
            <br />
        </div>

        <div class="col-md-12 col-xs-12 col-sm-12">

            <div class="col-md-3 col-xs-3 col-sm-3">
                <asp:Label ID="lblApellidos" runat="server" Text="Salario <span style='color:red'>*</span> " Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
            </div>
            <div class="col-md-4 col-xs-4 col-sm-4">
                <asp:TextBox class="form-control" ID="txtApellido" runat="server"></asp:TextBox>
            </div>
            <div id="divApellidoIncorrecto" runat="server" style="display: none" class="col-md-5 col-xs-5 col-sm-5">
                <asp:Label ID="lblApellidosIncorrecto" runat="server" Font-Size="Small" class="label alert-danger" Text="Espacio Obligatorio" ForeColor="Red"></asp:Label>
            </div>

        </div>
         <div class="col-md-12 col-xs-12 col-sm-12">
            <br />
        </div>
        <div class="col-md-12 col-xs-12 col-sm-12" >
            <div class="col-md-3 col-xs-3 col-sm-3">
                <asp:Label ID="label4" runat="server" Text="Planilla<span style='color:red'>*</span>" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
            </div>
            <div class="col-md-4 col-xs-4 col-sm-4">
                <asp:DropDownList ID="ddlPeriodo" class="btn btn-default dropdown-toggle" runat="server"></asp:DropDownList>
            </div>
        </div>




        <div class="col-md-12 col-xs-12 col-sm-12">
            <br />
        </div>


        <div class="col-md-12 col-xs-12 col-sm-12">
            <br />
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
        <div>
            <asp:Label ID="txtInfo" runat="server" Text="" Font-Size="Medium" ForeColor="Black" CssClass="label"></asp:Label>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>