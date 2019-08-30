<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.Master" AutoEventWireup="true" CodeBehind="AdministrarPeriodo.aspx.cs" Inherits="PEP.Catalogos.Periodos.AdministrarPeriodo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="MainScriptManager" runat="server" EnableCdn="true"/>
        <asp:UpdatePanel ID="pnlUpdate" runat="server">
            <ContentTemplate>

                <div class="row">
                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <center>
                            <asp:Label runat="server" Text="Apertura de Periodo" Font-Size="Large" ForeColor="Black"></asp:Label>
                            <p class="mt-1">Seleccione un periodo, o ingrese uno nuevo</p>
                        </center>
                    </div>

                    <div class="form-group col-md-12 col-xs-12 col-sm-12">
                        <asp:DropDownList AutoPostBack="true" ID="PeriodosDDL" runat="server" CssClass="form-control" OnSelectedIndexChanged="Periodos_OnChanged">
                        </asp:DropDownList>
                    </div>
                    
                    <div class="form-group col-md-6 col-xs-6 col-sm-6">
                        <asp:Button ID="btnNuevoPeriodo" runat="server" Text="Nuevo periodo" CssClass="btn btn-primary" OnClick="AgregarPeriodo_Click" />
                        <%--<asp:Button ID="btnEditarPeriodo" runat="server" Text="Editar periodo" CssClass="btn btn-success" OnClick="EditarPeriodo_Click" />--%>
                        <asp:Button ID="btnEliminarPeriodo" runat="server" Text="Eliminar periodo" CssClass="btn btn-danger" OnClick="EliminarPeriodo_Click" />
                    </div>
                    
                    <div class="form-group col-md-6 col-xs-6 col-sm-6 alinear-derecha">
                        <asp:Button ID="EstablecerPeriodoActualBtn" runat="server" Text="Establecer como periodo actual" CssClass="btn btn-primary"/>
                    </div>
                    
                    <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 mt-2">
                        <hr />
                    </div>

                    <div class="col-md-12 col-xs-12 col-sm-12">
                        <center>
                            <asp:Label runat="server" Text="Proyectos" Font-Size="Large" ForeColor="Black"></asp:Label>
                            <p class="mt-1">Seleccione los proyectos del periodo seleccionado, y elija un periodo para transferirlos; o ingrese uno nuevo</p>
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
                        <div class="form-group col-md-5 col-xs-5 col-sm-5">
                            <asp:ListBox runat="server" AutoPostBack="true" ID="ProyectosActualesLB" CssClass="form-control" SelectionMode="Multiple" OnSelectedIndexChanged="ProyectosActualesLB_OnChanged">
                            </asp:ListBox>
                        </div>
        
                        <div class="form-group col-md-2 col-xs-2 col-sm-2">
                            <div class="center">
                                <center>
                                    <asp:Button Text=">" ID="PasarProyectosBtn" CssClass="btn btn-secondary" runat="server" />
                                </center>
                            </div>
            
                            <div class="center">
                                <center>
                                    <asp:Button Text="<" ID="DevolverProyectosBtn" CssClass="btn btn-secondary" runat="server" />
                                </center>
                            </div>
                        </div>
        
                        <div class="form-group col-md-5 col-xs-5 col-sm-5">
                            <asp:ListBox runat="server" ID="ProyectosNuevosLB" CssClass="form-control" SelectionMode="Multiple">
                            </asp:ListBox>
                        </div>
                    </div>

                    <div class="row col-md-12 col-xs-12 col-sm-12">
                        <div class="form-group col-md-6 col-xs-6 col-sm-6 mt-1">
                            <asp:Button ID="btnNuevoProyecto" runat="server" Text="Nuevo proyecto" CssClass="btn btn-primary" OnClick="AgregarProyecto_Click" />
                            <asp:Button ID="btnEditarProyecto" runat="server" Text="Editar proyecto" CssClass="btn btn-success" OnClick="EditarProyecto_Click" />
                            <asp:Button ID="btnEliminarProyecto" runat="server" Text="Eliminar proyecto" CssClass="btn btn-danger" OnClick="EliminarProyecto_Click" />
                        </div>
        
                        <div class="col-md-6 col-xs-6 col-sm-6 mt-1 alinear-derecha">
                            <asp:Button Text="Guardar cambios" ID="GuardarProyectosBtn" runat="server" class="btn btn-primary"/>
                        </div>
                    </div>            

                    <div id="divUnidades" runat="server">
                        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 mt-2">
                            <hr />
                        </div>

                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <center>
                                <asp:Label runat="server" Text="Unidades" Font-Size="Large" ForeColor="Black"></asp:Label>
                                <p class="mt-1">Seleccione una unidad del proyecto seleccionado, o ingrese una nueva</p>
                            </center>
                        </div>
        
                        <div class="form-group col-md-12 col-xs-12 col-sm-12">
                            <asp:ListBox runat="server" ID="UnidadesActualesLB" CssClass="form-control" SelectionMode="Single">
                            </asp:ListBox>
                        </div>
        
                        <div class="col-md-12 col-xs-12 col-sm-12">
                            <asp:Button ID="btnNuevaUnidad" runat="server" Text="Nueva unidad" CssClass="btn btn-primary" OnClick="AgregarUnidad_Click" />
                            <asp:Button ID="btnEditarUnidad" runat="server" Text="Editar unidad" CssClass="btn btn-success" OnClick="EditarUnidad_Click" />
                            <asp:Button ID="btnEliminarUnidad" runat="server" Text="Eliminar unidad" CssClass="btn btn-danger" OnClick="EliminarUnidad_Click" />
                        </div> 
                    </div>    
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
