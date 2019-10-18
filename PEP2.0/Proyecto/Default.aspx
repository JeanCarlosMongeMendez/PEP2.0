<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PEP.Default" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" EnableCdn="true"/>
        <asp:UpdatePanel ID="pnlUpdate" runat="server">
            <ContentTemplate>

    <div class="row">
    </div>
                <div class="jumbotron col-md-12 col-xs-12 col-sm-12 center">
                    <h2>Bienvenido a la Plataforma de Ejecución Presupuestaria</h2>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    <!-- script tabla jquery -->
    <script type="text/javascript">
</script>
    <!-- fin script tabla jquery -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>