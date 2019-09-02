<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PEP.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <body class="loginBody">
    <div class="container-fluid">
        <div class="row">

            <asp:LinkButton ID="btnInformacion" runat="server" ToolTip="Información" ForeColor="#005da4" data-toggle="modal" data-target="#modalInformacion" Style="text-align: left;">
               <span class="glyphicon glyphicon-info-sign" id="infoIcon"></span>                
                </asp:LinkButton>

                    <div class="col-md-4 col-sm-4 col-xs-12 col-md-offset-4 col-sm-4 col-sm-offset-4" id="loginPanel">
                        <div class="loginOutline col-xs-12">   
                            <div class="panel-heading">
                                <div class="panel-title text-center">
                                    <h3><strong>Bienvenido</strong></h3>
                                </div>

                            </div>
                            <div class="panel-body">
                                <div class="errorMessages">
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Usuario o Contraseña Incorrecta" Visible="False"></asp:Label>
                                    <asp:Label ID="lblNoUsuario" runat="server" ForeColor="Red" Text="El usuario no tiene permisos para entrar a esta aplicación" Visible="False"></asp:Label>
                                </div>
                                <label for="exampleInputEmail1">Usuario</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control chat-input"></asp:TextBox>

                                </div>

                                <br />
                                <label for="exampleInputPassword1">Contraseña </label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-lock"></i></span>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass=" form-control chat-input" TextMode="Password"></asp:TextBox>

                                </div>
                                <br />
                                <div class="loginButton">
                                    <asp:LinkButton ID="btIngresar" runat="server" CssClass="btn btn-default boton-main btn-lg" OnClick="btIngresar_Click" CausesValidation="False" UseSubmitBehavior="False">
                                        Iniciar Sesión
                                        <span class="fa fa-lock fa-hover-hidden"></span>
                                        <span class="fa fa-unlock fa-hover-show"></span>
                                    </asp:LinkButton>
                                </div>

                            </div>

                        </div>
                        <br />
                        <br />

                    </div>
    </div>
        </div>

        </body>
    <script>//Script para detectar el "Enter" en los txtbox
        function button_click(objTextBox, objBtnID) {
            if (window.event.keyCode == 13) {
                document.getElementById(objBtnID).focus();
                document.getElementById(objBtnID).click();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>