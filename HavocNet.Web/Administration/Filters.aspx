<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Filters.aspx.cs" Inherits="HavocNet.Web.Administration.Filters" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script>
        function DDLChange(sender) {
            var aHid = document.getElementById(sender.id.replace('cbo', 'hid'));

            document.getElementById('ctl00_cphMain_hidUserAccess').value += sender.selectedIndex + '|' + aHid.value + ";";

            document.getElementById('ctl00_cphMain_lblStatus').innerHTML = "* Press update to save changes *";
            document.getElementById('ctl00_cphMain_lblStatus').style["color"] = "red";
        }
    </script>

    <div class="pageTitle">
        <table>
        <tr>
            <td><asp:Label runat="server" id="lblTitle"></asp:Label></td>
            <td><asp:DropDownList runat="server" id="cboUsers" AutoPostBack="True"></asp:DropDownList></td>
            <td><asp:ImageButton runat="server" id="cmdUpdate" ImageUrl="..\App_Themes\HavocNet\Images\Update.gif" CssClass="TaskButton"></asp:ImageButton></td>
        </tr>
        </table>
    </div>
    <div style="margin-top: 10px;">
        <asp:Literal runat="server" ID="FilterTree"></asp:Literal>
    </div>
    <div style="margin-top:10px; color:red">
        <asp:Label runat="server" ID="lblStatus"></asp:Label>
    </div>
    <asp:HiddenField runat="server" ID="hidUserAccess"/>
</asp:Content>