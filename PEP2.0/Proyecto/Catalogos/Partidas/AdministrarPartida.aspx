<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarPartida.aspx.cs" Inherits="Proyecto.Catalogos.Partidas.AdministrarPartida" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" EnableCdn="true"/>
        <asp:UpdatePanel ID="pnlUpdate" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <center>
                            <asp:Label runat="server" Text="Partidas" Font-Size="Large" ForeColor="Black"></asp:Label>
                            <p class="mt-1">Seleccione un periodo</p>
                        </center>
                    </div>

                    <div class="form-group col-md-12 col-xs-12 col-sm-12">
                        <asp:DropDownList AutoPostBack="true" ID="PeriodosDDL" runat="server" CssClass="form-control" OnSelectedIndexChanged="Periodos_OnChanged">
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <center>
                            <p class="mt-1">Seleccione las partidas del actual periodo, y elija un periodo para transferirlas</p>
                        </center>
                    </div>
        
                    <div class="row col-md-12 col-xs-12 col-sm-12">
                        <div class="col-md-5 col-xs-5 col-sm-5">
                            <div class="center form-group">
                                <center>
                                    <asp:Label ID="AnoActual" Text="Año" runat="server" CssClass="font-weight-bold"/>
                                </center>
                            </div>
                        </div>
        
                        <div class="col-md-2 col-xs-2 col-sm-2"></div>
        
                        <div class="col-md-5 col-xs-5 col-sm-5">
                            <div class="row form-group">
                                <asp:DropDownList AutoPostBack="true" ID="PeriodosNuevosDDL" runat="server" CssClass="form-control" OnSelectedIndexChanged="PeriodosNuevos_OnChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
        
                    <div class="row col-md-12 col-xs-12 col-sm-12">
                        <div class="col-md-5 col-xs-5 col-sm-5">
                            <asp:ListBox runat="server" ID="PartidasActualesLB" CssClass="form-control" SelectionMode="Multiple">
                            </asp:ListBox>
                        </div>
        
                        <div class="col-md-2 col-xs-2 col-sm-2">
                            <div class="center">
                                <center>
                                    <asp:Button Text=">" ID="PasarPartidasBtn" CssClass="btn btn-secondary" runat="server" />
                                </center>
                            </div>
            
                            <div class="center">
                                <center>
                                    <asp:Button Text="<" ID="DevolverPartidasBtn" CssClass="btn btn-secondary" runat="server" />
                                </center>
                            </div>
                        </div>
        
                        <div class="col-md-5 col-xs-5 col-sm-5">
                            <div class="row">
                                <asp:ListBox runat="server" ID="PartidasNuevasLB" CssClass="form-control" SelectionMode="Multiple">
                                </asp:ListBox>
                            </div>
                        </div>
                    </div>

                    <div class="row col-md-12 col-xs-12 col-sm-12">
                        <div class="col-md-6 col-xs-6 col-sm-6 mt-1">
                            <asp:Button ID="btnNuevaPartida" runat="server" Text="Nueva partida" CssClass="btn btn-primary" OnClick="AgregarPartida_Click" />
                            <asp:Button ID="btnEditarPartida" runat="server" Text="Editar proyecto" CssClass="btn btn-success" OnClick="EditarPartida_Click" />
                            <asp:Button ID="btnEliminarPartida" runat="server" Text="Eliminar proyecto" CssClass="btn btn-danger" OnClick="EliminarPartida_Click" />
                        </div>
        
                        <div class="col-md-6 col-xs-6 col-sm-6 mt-1 alinear-derecha">
                            <asp:Button Text="Guardar cambios" ID="GuardarPartidasBtn" runat="server" class="btn btn-primary"/>
                        </div>
                    </div>            
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
