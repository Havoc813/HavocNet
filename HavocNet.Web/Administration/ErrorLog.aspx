<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorLog.aspx.cs" Inherits="HavocNet.Web.Administration.ErrorLog" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript">
        function OnBeforeClick() {
            if (!ValidateDate("ctl00_cphMain_txtDateFrom", "Date From")) { return false; }
            if (!ValidateDate("ctl00_cphMain_txtDateTo", "Date To")) { return false; }

            return true;
        }
        
        function UnhideDetail(sender, rowID) {
            var row = document.getElementById("ctl00_cphMain_row" + String(rowID));

            if (row.style.display == 'none') {
                row.style.display = 'table-row';
                sender.innerHTML = '[-]';
                sender.style.color = 'red';
            }
            else {
                row.style.display = 'none';
                sender.innerHTML = '[+]';
                sender.style.color = 'green';
            }
            return false;
        }
    </script>
    <div class="pageTitle">
        <table>
        <tr>
            <td>Error Log for </td>
            <td><asp:DropDownList runat="server" ID="cboUsers" CssClass="HeaderBarDropdownStandalone"></asp:DropDownList></td>
            <td>from</td>
            <td>
                <asp:TextBox runat="server" ID="txtDateFrom" CssClass="HeaderBarDateTextStandalone" />
            </td>
            <td>to</td>
            <td>
                <asp:TextBox runat="server" ID="txtDateTo" CssClass="HeaderBarDateText" />
            </td>
            <td><asp:ImageButton runat="server" ImageUrl="~\App_Themes\HavocNet\Images\Submit.gif" CssClass="TaskButton" ToolTip="Submit" OnClientClick="javascript:return OnBeforeClick();" /></td>
            <td><asp:ImageButton runat="server" ID="cmdExportToExcel" ImageUrl="~\App_Themes\HavocNet\Images\excel.gif" CssClass="TaskButton" ToolTip="Export To Excel" /></td>
        </tr>
        </table>
    </div>
    <div>
        <asp:PlaceHolder runat="server" ID="phErrors" />
    </div>
</asp:Content>