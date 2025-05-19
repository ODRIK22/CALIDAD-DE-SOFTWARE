<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm6.aspx.cs" Inherits="CALIDAD_DE_SOFTWARE.CALIDAD_SFTW.WebForm6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h2>Cerrar Sesión</h2>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
    <br /><br />
    <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar Sesión" OnClick="btnCerrarSesion_Click" />
</asp:Content>
