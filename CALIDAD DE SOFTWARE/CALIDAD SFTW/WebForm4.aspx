<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm4.aspx.cs" Inherits="CALIDAD_DE_SOFTWARE.CALIDAD_SFTW.WebForm4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <h2>Búsqueda de Productos</h2>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>

    <!-- Formulario para buscar un producto -->
    <div>
        <h3>Búsqueda de Producto</h3>
        <table>
            <tr>
                <td>Criterio de Búsqueda:</td>
                <td>
                    <asp:TextBox ID="txtBusqueda" runat="server" Width="300"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Buscar por:</td>
                <td>
                    <asp:DropDownList ID="ddlCriterio" runat="server">
                        <asp:ListItem Text="Nombre Detallado" Value="nombre2"></asp:ListItem>
                        <asp:ListItem Text="Código de Barras" Value="codigo_barras"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
    </div>

    <!-- Tabla para mostrar resultados de la búsqueda -->
    <div>
        <h3>Resultados de la Búsqueda</h3>
        <asp:GridView ID="gvResultados" runat="server" AutoGenerateColumns="true" EmptyDataText="No se encontraron productos."></asp:GridView>
    </div>
</asp:Content>
