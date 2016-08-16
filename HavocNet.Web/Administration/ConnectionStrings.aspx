<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConnectionStrings.aspx.cs" Inherits="HavocNet.Web.Administration.ConnectionStrings"  MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="pageTitleText">
        <table>
        <tr>
            <td>Encrypt/ Decrypt Connection Strings</td>
        </tr>
        </table>
    </div>
    <div style="margin-top:10px">
        <div>
            <asp:TextBox ID="txtConnectionString" runat="server" Width="500px"></asp:TextBox>
        </div>
        <div style="margin-top:10px">
            <asp:LinkButton runat="server" ID="cmdEncrypt" Text="Encrypt" CssClass="MyButton"></asp:LinkButton>
            <asp:LinkButton runat="server" ID="cmdDecrypt" Text="Decrypt" CssClass="MyButton"></asp:LinkButton>            
        </div>
        <div style="margin-top:10px">
            <asp:TextBox ID="txtResponse" runat="server" Width="500px"></asp:TextBox>
        </div>
    </div>
</asp:Content>