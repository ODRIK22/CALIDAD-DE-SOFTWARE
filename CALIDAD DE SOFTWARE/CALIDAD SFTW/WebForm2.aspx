<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="CALIDAD_DE_SOFTWARE.CALIDAD_SFTW.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <h2>Gestión de Productos</h2>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>

    <table>
        <tr>
            <td>Nombre del Producto:</td>
            <td>
                <asp:DropDownList 
                    ID="ddlNombreProducto" 
                    runat="server" 
                    AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlNombreProducto_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Código de barras:</td>
            <td>
                <asp:TextBox 
                    ID="txtIdProducto" 
                    runat="server" 
                    ReadOnly="true">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Nombre Detallado:</td>
            <td>
                <asp:TextBox 
                    ID="txtNombreDetallado" 
                    runat="server" 
                    AutoCompleteType="Disabled">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Precio Compra:</td>
            <td>
                <asp:TextBox ID="txtPrecioCompra" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Precio Venta:</td>
            <td>
                <asp:TextBox ID="txtPrecioVenta" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Stock:</td>
            <td>
                <asp:TextBox ID="txtStock" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Proveedor:</td>
            <td>
                <asp:DropDownList ID="ddlProveedor" runat="server"></asp:DropDownList>
            </td>
        </tr>
    </table>

    <asp:Button ID="btnInsertar" runat="server" Text="Insertar" OnClick="btnInsertar_Click" />
    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" OnClick="btnActualizar_Click" />
    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click" />
<asp:GridView
    ID="gvProductos"
    runat="server"
    AutoGenerateColumns="False"
    AllowPaging="True"
    PageSize="10"
    CssClass="table"
    OnPageIndexChanging="gvProductos_PageIndexChanging"
    OnSelectedIndexChanged="gvProductos_SelectedIndexChanged">
    <Columns>
        <asp:BoundField DataField="id" HeaderText="ID Producto" />
        <asp:BoundField DataField="nombre" HeaderText="Nombre Producto" />
        <asp:BoundField DataField="precio_compra" HeaderText="Precio Compra" DataFormatString="{0:C}" />
        <asp:BoundField DataField="precio_venta" HeaderText="Precio Venta" DataFormatString="{0:C}" />
        <asp:BoundField DataField="stock" HeaderText="Stock Disponible" />
        <asp:BoundField DataField="codigo_barras" HeaderText="Código de Barras" />
        <asp:BoundField DataField="id_proveedor" HeaderText="ID Proveedor" />
        <asp:BoundField DataField="nombre2" HeaderText="Nombre Detallado" />
        <asp:CommandField ShowSelectButton="True" SelectText="Seleccionar" />
    </Columns>
</asp:GridView>
</asp:Content>
