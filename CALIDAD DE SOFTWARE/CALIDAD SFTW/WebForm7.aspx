<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm7.aspx.cs" Inherits="CALIDAD_DE_SOFTWARE.CALIDAD_SFTW.WebForm7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<asp:Panel ID="pnlPrincipal" runat="server" HorizontalAlign="Center">
    <!-- Título -->
    <asp:Label ID="lblcomprobación" runat="server" Text=""></asp:Label>
    <asp:Label ID="lblTitulo" runat="server" Text="Gestión de Productos y Compras" Font-Size="Large" Font-Bold="True"></asp:Label>
    <asp:Literal ID="litSeparador" runat="server" Text="<br /><br />"></asp:Literal>
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>

    <!-- Mostrar productos disponibles -->
    <asp:Label ID="lblProductos" runat="server" Text="Productos disponibles:" Font-Bold="True"></asp:Label>
    <asp:Literal ID="litSeparador2" runat="server" Text="<br /><br />"></asp:Literal>

   <asp:GridView
    ID="gvProductos" 
    runat="server" 
    AutoGenerateColumns="False" 
    AllowPaging="True" 
    PageSize="10" 
    CssClass="table"
    OnPageIndexChanging="gvProductos_PageIndexChanging">
    <Columns>
        <asp:BoundField DataField="id" HeaderText="ID Producto" />
        <asp:BoundField DataField="nombre" HeaderText="Nombre Producto" />
        <asp:BoundField DataField="stock" HeaderText="Stock Disponible" />
        <asp:BoundField DataField="precio_venta" HeaderText="Precio" DataFormatString="{0:C}" />
    </Columns>
</asp:GridView>
    <asp:Literal ID="litSeparador3" runat="server" Text="<br /><br />"></asp:Literal>

    <!-- Realizar compra -->
    <asp:Label ID="lblCompra" runat="server" Text="Realizar Compra:" Font-Bold="True"></asp:Label>
    <asp:Literal ID="litSeparador4" runat="server" Text="<br /><br />"></asp:Literal>

    <!-- Seleccionar producto -->
    <asp:Label ID="lblProducto" runat="server" Text="Selecciona un producto:"></asp:Label>
<asp:DropDownList
    ID="ddlProductos" 
    runat="server"
    AutoPostBack="true"
    OnSelectedIndexChanged="ddlProductos_SelectedIndexChanged">
</asp:DropDownList>
    <asp:Literal ID="litSeparador5" runat="server" Text="<br /><br />"></asp:Literal>

    <!-- Cantidad a comprar -->
    <asp:Label ID="lblCantidad" runat="server" Text="Cantidad:"></asp:Label>
    <asp:TextBox ID="txtCantidad" runat="server" Width="50"></asp:TextBox>
    <asp:RequiredFieldValidator 
        ID="rfvCantidad" 
        runat="server" 
        ControlToValidate="txtCantidad" 
        ErrorMessage="La cantidad es requerida." 
        ForeColor="Red"></asp:RequiredFieldValidator>
    <asp:RangeValidator 
        ID="rvCantidad" 
        runat="server" 
        ControlToValidate="txtCantidad" 
        MinimumValue="1" 
        MaximumValue="1000" 
        Type="Integer" 
        ErrorMessage="Ingrese una cantidad válida (entre 1 y 1000)." 
        ForeColor="Red"></asp:RangeValidator>
    <asp:Literal ID="litSeparador6" runat="server" Text="<br /><br />"></asp:Literal>

    <!-- Botón para confirmar la compra -->
    <asp:Button ID="btnComprar" runat="server" Text="Comprar" OnClick="btnComprar_Click" />

    <!-- Label para mostrar el resultado de la compra -->
    <asp:Label ID="lblMensaje" runat="server" ForeColor="Green"></asp:Label>
</asp:Panel>
</asp:Content>
