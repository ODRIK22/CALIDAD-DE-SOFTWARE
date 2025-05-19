<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm8.aspx.cs" Inherits="CALIDAD_DE_SOFTWARE.CALIDAD_SFTW.WebForm8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>VENTAS</h1>
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
<asp:GridView 
    ID="gvVentas" 
    runat="server" 
    AutoGenerateColumns="False" 
    AllowPaging="True" 
    PageSize="10" 
    CssClass="table" 
    OnPageIndexChanging="gvVentas_PageIndexChanging">
    <Columns>
        <asp:BoundField DataField="VentaID" HeaderText="ID Venta" />
        <asp:BoundField DataField="FechaVenta" HeaderText="Fecha de Venta" DataFormatString="{0:dd/MM/yyyy}" />
        <asp:BoundField DataField="TotalVenta" HeaderText="Total Venta" DataFormatString="{0:C}" />
        <asp:BoundField DataField="ProductoID" HeaderText="ID Producto" />
        <asp:BoundField DataField="NombreProducto" HeaderText="Producto" />
        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
        <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unitario" DataFormatString="{0:C}" />
        <asp:BoundField DataField="PrecioCompra" HeaderText="Precio Compra" DataFormatString="{0:C}" />
        <asp:BoundField DataField="Ganancia" HeaderText="Ganancia" DataFormatString="{0:C}" />
    </Columns>
</asp:GridView>
    <asp:Button 
    ID="btnGenerarInforme" 
    runat="server" 
    Text="Generar Informe en PDF" 
    OnClick="btnGenerarInforme_Click" 
    CssClass="btn btn-primary" />
<asp:Label ID="lblGananciaTotal" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
</asp:Content>
