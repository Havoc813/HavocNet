<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AuditLog.aspx.cs" Inherits="HavocNet.Web.Administration.AuditLog"  MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript">
        function OnBeforeClick() {
            if (!ValidateDate("ctl00_cphMain_txtDateFrom", "Date From")) { return false; };
            if (!ValidateDate("ctl00_cphMain_txtDateTo", "Date To")) { return false; };

            return true;
        } 
    </script>
    <div class="pageTitle">
        <table>
        <tr>
            <td>Audit Log For</td>
            <td><asp:DropDownList runat="server" ID="cboUsers" CssClass="HeaderBarDropdownStandalone"></asp:DropDownList></td>
            <td> doing </td>
            <td><asp:DropDownList runat="server" ID="cboAction" CssClass="HeaderBarDropdownStandalone"></asp:DropDownList></td>
            <td> from </td>
            <td><asp:TextBox runat="server" ID="txtDateFrom" CssClass="HeaderBarDateTextStandalone" /></td>
            <td> to </td>
            <td><asp:TextBox runat="server" ID="txtDateTo" CssClass="HeaderBarDateText" /></td>
            <td><asp:ImageButton runat="server" ImageUrl="~\App_Themes\HavocNet\Images\Submit.gif" CssClass="TaskButton" ToolTip="Submit" OnClientClick="javascript:return OnBeforeClick();" /></td>
            <td><asp:ImageButton runat="server" ID="cmdExportToExcel" ImageUrl="~\App_Themes\HavocNet\Images\excel.gif" CssClass="TaskButton" ToolTip="Export To Excel" /></td>
        </tr>
        </table>
    </div>
    <div>
        <asp:PlaceHolder runat="server" ID="phAudits" />
    </div>
</asp:Content>