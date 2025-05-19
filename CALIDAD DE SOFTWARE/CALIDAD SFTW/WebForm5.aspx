<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm5.aspx.cs" Inherits="CALIDAD_DE_SOFTWARE.CALIDAD_SFTW.WebForm5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Inicio de Sesión</h2>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    <table>
        <tr>
            <td>Correo Electrónico:</td>
            <td>
                <asp:TextBox ID="txtCorreo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Contraseña:</td>
            <td>
                <asp:TextBox ID="txtContraseña" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
    </table>
    <asp:Button ID="btnIniciarSesion" runat="server" Text="Iniciar Sesión" OnClick="btnIniciarSesion_Click" />
</asp:Content>
