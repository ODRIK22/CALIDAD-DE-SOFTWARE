<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="CALIDAD_DE_SOFTWARE.CALIDAD_SFTW.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-container">
        <h2>Crear Usuario</h2>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red"></asp:Label>
        
        <div class="form-group">
            <label for="txtNombre">Nombre:</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
        </div>
        
        <div class="form-group">
            <label for="txtApellido">Apellido:</label>
            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
        </div>
        
        <div class="form-group">
            <label for="txtCorreo">Correo Electrónico:</label>
            <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" />
        </div>
        
        <div class="form-group">
            <label for="txtContraseña">Contraseña:</label>
            <asp:TextBox ID="txtContraseña" runat="server" CssClass="form-control" TextMode="Password" />
        </div>
        
        <div class="form-group">
            <label for="ddlRol">Rol:</label>
            <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        
        <div class="form-group">
            <label for="ddlCodigoPostal">Código Postal:</label>
            <asp:DropDownList ID="ddlCodigoPostal" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCodigoPostal_SelectedIndexChanged"></asp:DropDownList>
        </div>
        
        <div class="form-group">
            <label for="txtEstado">Estado:</label>
            <asp:TextBox ID="txtEstado" runat="server" CssClass="form-control" ReadOnly="true" />
        </div>
        
        <div class="form-group">
            <asp:Button ID="btnCrearUsuario" runat="server" Text="Crear Usuario" OnClick="btnCrearUsuario_Click" CssClass="btn btn-primary" />
        </div>
    </div>
</asp:Content>
