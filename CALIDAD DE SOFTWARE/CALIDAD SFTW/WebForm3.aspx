<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm3.aspx.cs" Inherits="CALIDAD_DE_SOFTWARE.CALIDAD_SFTW.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    <h2>Gestión de Proveedores</h2>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
    <table border="1" cellpadding="5">
        <tr>
            <td>Nombre:</td>
            <td><asp:TextBox ID="txtNombre" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Teléfono:</td>
            <td><asp:TextBox ID="txtTelefono" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Empresa:</td>
            <td><asp:TextBox ID="txtEmpresa" runat="server"></asp:TextBox></td>
        </tr>
    </table>
    <br />
    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" CssClass="btn" />
    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" OnClick="btnActualizar_Click" CssClass="btn" />
    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click" CssClass="btn" />
    <br /><br />
    <asp:Label ID="lblMensaje" runat="server" ForeColor="Green"></asp:Label>
       <asp:GridView
            ID="gvProveedores"
            runat="server"
            AutoGenerateColumns="False"
            AllowPaging="True"
            PageSize="10"
            CssClass="table"
            OnPageIndexChanging="gvProveedores_PageIndexChanging"
            OnSelectedIndexChanged="gvProveedores_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID" />
                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="telefono" HeaderText="Teléfono" />
                <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                <asp:CommandField ShowSelectButton="True" SelectText="Seleccionar" />
            </Columns>
        </asp:GridView>
</asp:Content>
