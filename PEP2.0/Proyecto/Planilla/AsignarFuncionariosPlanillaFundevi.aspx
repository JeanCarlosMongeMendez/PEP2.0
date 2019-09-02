<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AsignarFuncionariosPlanillaFundevi.aspx.cs" Inherits="Proyecto.Planilla.AsignarFuncionariosPlanillaFundevi" MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" EnableCdn="true"/>
        <asp:UpdatePanel ID="pnlUpdate" runat="server">
            <ContentTemplate>

                <div class="row">
                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <center>
                            <asp:Label runat="server" Text="Asignar funcionarios a planilla" Font-Size="Large" ForeColor="Black"></asp:Label>
                        </center>
                    </div>
                    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 mt-2">
                        <hr />
                    </div>
                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <center>
                            <asp:Label runat="server" Text="Planilla" Font-Size="Large" ForeColor="Black"></asp:Label>
                        </center>
                    </div>

                    <div class="form-group col-md-12 col-xs-12 col-sm-12">
                        <asp:DropDownList AutoPostBack="true" ID="PeriodosDDL" runat="server" CssClass="form-control"  OnSelectedIndexChanged="FuncionariosPeriodos" >
                        </asp:DropDownList>
                    </div>

                
                    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 mt-2">
                        <hr />
                    </div>

                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <center>
                            <asp:Label runat="server" Text="Funcionarios" Font-Size="Large" ForeColor="Black"></asp:Label>
                            <p class="mt-1">Seleccione los funcionarios de una planilla, y elija la planilla para transferirlos</p>
                        </center>
                    </div>
        
                    <div class="row col-md-12 col-xs-12 col-sm-12">
                        <div class="col-md-5 col-xs-5 col-sm-5">
                            <div class="center form-group">
                                <asp:Label ID="AnoActual" Text="Funcionarios" runat="server" CssClass="font-weight-bold"/>
                            </div>
                        </div>
        
                        <div class="col-md-2 col-xs-2 col-sm-2"></div>
        
                        <div class="col-md-5 col-xs-5 col-sm-5">
                            <div class="row form-group">
                                <asp:DropDownList AutoPostBack="true" ID="PeriodosNuevosDDL" runat="server" CssClass="form-control" >
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
        
                    <div class="row col-md-12 col-xs-12 col-sm-12">
                        <div class="col-md-5 col-xs-5 col-sm-5">
                            <asp:ListBox runat="server" ID="ProyectosActualesLB" CssClass="form-control" SelectionMode="Multiple">
                            </asp:ListBox>
                        </div>
        
                        <div class="col-md-2 col-xs-2 col-sm-2">
                            <div class="center">
                                <asp:Button Text=">" ID="PasarProyectosBtn" CssClass="btn btn-secondary" runat="server" />
                            </div>
            
                            <div class="center">
                                <asp:Button Text="<" ID="DevolverProyectosBtn" CssClass="btn btn-secondary" runat="server" />
                            </div>
                        </div>
        
                        <div class="col-md-5 col-xs-5 col-sm-5">
                            <div class="row">
                                <asp:ListBox runat="server" ID="ProyectosNuevosLB" CssClass="form-control" SelectionMode="Multiple">
                                </asp:ListBox>
                            </div>
                        </div>
                    </div>

                    <div class="row col-md-12 col-xs-12 col-sm-12">
                        <div class="col-md-6 col-xs-6 col-sm-6 mt-1">
                            <asp:Button ID="btnNuevoFuncionario" runat="server" Text="Nuevo funcionario" CssClass="btn btn-primary" OnClick="btnNuevoFuncionario_Click"/>
                        </div>
        
                        <div class="col-md-6 col-xs-6 col-sm-6 mt-1 alinear-derecha">
                            <asp:Button Text="Guardar cambios" ID="GuardarFuncionarios" runat="server" class="btn btn-primary" OnClick="GuardarFuncionariosBtn_Click"/>
                        </div>
                    </div>            

                    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 mt-2">
                        <hr />
                    </div>

                         
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>