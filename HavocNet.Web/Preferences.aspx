<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Preferences.aspx.cs" Inherits="HavocNet.Web.Preferences" MasterPageFile="HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
<div class="pageTitle">
    <table>
        <tr>
            <td>Personal Information</td>
            <td><asp:ImageButton runat="server" ID="cmdUpdate" CssClass="TaskButton" ImageUrl="App_Themes/HavocNet/Images/update.gif" /></td>
        </tr>
    </table>
</div>
<div style="padding-left:10px">
    <table>
        <tr>
            <td>Name</td><td><asp:TextBox runat="server" ID="txtFirstName" Width="200"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Surname</td><td><asp:TextBox runat="server" ID="txtSurname" Width="200"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="padding-right:30px">Email Address</td><td><asp:TextBox runat="server" ID="txtEmailAddress" Width="200"></asp:TextBox></td>
        </tr>
    </table>
</div>
<div class="subHeaderText">
    <table>
        <tr>
            <td>Choose your homepage</td>
        </tr>
    </table>
</div>
<div style="padding-left:10px">
    <asp:DropDownList runat="server" ID="cboHomePage"/>
</div>
<div class="subHeaderText">
    <table>
        <tr>
            <td>Options</td>
        </tr>
    </table>
</div>
<div style="padding-left:10px">
    <asp:CheckBox runat="server" ID="chkNewsLetter" /> Email News Letter
</div>
<div style="padding: 10px 0 0 5px">
    <asp:Label runat="server" ID="lblStatus" ForeColor="Green"></asp:Label>
</div>
</asp:Content>