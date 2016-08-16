<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuSecurity.aspx.cs" Inherits="HavocNet.Web.MenuSecurity" MasterPageFile="HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="/Scripts/Validate.js"></script>
    <script type="text/javascript" src="/Scripts/TableManage.js"></script>
   
    <script type="text/javascript">
        function OnBeforeUpdateWithValidate() {
            if (!ValidateString(document.getElementById("ctl00_cphMain_txtEditUsername").value, "ctl00_cphMain_txtEditUsername", "Username")) { return false; }
            if (!ValidateString(document.getElementById("ctl00_cphMain_txtEditFirstName").value, "ctl00_cphMain_txtEditFirstName", "First Name")) { return false; }
            if (!ValidateString(document.getElementById("ctl00_cphMain_txtEditSurname").value, "ctl00_cphMain_txtEditSurname", "Surname")) { return false; }
            if (!ValidateString(document.getElementById("ctl00_cphMain_txtEditEmailAddress").value, "ctl00_cphMain_txtEditEmailAddress", "Email Address")) { return false; }
            if (!ValidateEmail(document.getElementById("ctl00_cphMain_txtEditEmailAddress").value, "ctl00_cphMain_txtEditEmailAddress", "Email Address")) { return false; }

            if (document.getElementById("ctl00_cphMain_txtEditPassword").value != "") {
                if (document.getElementById("ctl00_cphMain_txtEditPassword").value != document.getElementById("ctl00_cphMain_txtEditPasswordConfirm").value) {
                    alert('Passwords do not match');
                    document.getElementById("ctl00_cphMain_txtEditPassword").focus();
                    return false;
                }
                if (!ValidatePassword(document.getElementById("ctl00_cphMain_txtEditPassword").value, "ctl00_cphMain_txtEditPassword", "Password")) { return false; }
            }

            return true;
        }

        function OnBeforeSaveWithValidate() {
            //Check username does not exist

            if (!ValidateString(document.getElementById("ctl00_cphMain_txtNewUsername").value, "ctl00_cphMain_txtNewUsername", "Username")) { return false; }
            if (!ValidateString(document.getElementById("ctl00_cphMain_txtNewFirstName").value, "ctl00_cphMain_txtNewFirstName", "First Name")) { return false; }
            if (!ValidateString(document.getElementById("ctl00_cphMain_txtNewSurname").value, "ctl00_cphMain_txtNewSurname", "Surname")) { return false; }
            if (!ValidateString(document.getElementById("ctl00_cphMain_txtNewPassword").value, "ctl00_cphMain_txtNewPassword", "Password")) { return false; }
            if (!ValidateString(document.getElementById("ctl00_cphMain_txtNewEmailAddress").value, "ctl00_cphMain_txtNewEmailAddress", "Email Address")) { return false; }
            if (!ValidateEmail(document.getElementById("ctl00_cphMain_txtNewEmailAddress").value, "ctl00_cphMain_txtNewEmailAddress", "Email Address")) { return false; }
            
            if (document.getElementById("ctl00_cphMain_txtNewPassword").value != document.getElementById("ctl00_cphMain_txtNewPasswordConfirm").value) {
                alert('Passwords must match');
                document.getElementById("ctl00_cphMain_txtNewPassword").focus();
                return false;
            }
            if (!ValidatePassword(document.getElementById("ctl00_cphMain_txtNewPassword").value, "ctl00_cphMain_txtNewPassword", "Password")) { return false; }

            return true;
        }
        
        var aTableManage = new TableManage('User');
        aTableManage.OnBeforeSave = OnBeforeSaveWithValidate;
        aTableManage.OnBeforeUpdate = OnBeforeUpdateWithValidate;
    </script>
    <div class="pageTitle">
        <table>
        <tr>
            <td><asp:Label runat="server" id="lblTitle"></asp:Label></td>
            <td>
                <asp:ImageButton runat="server" ID="cmdAdd" ImageUrl="~\App_Themes\HavocNet\Images\new.gif" ToolTip="Add New" CssClass="TaskButton"></asp:ImageButton>
                <asp:ImageButton runat="server" ID="cmdSave" ImageUrl="~\App_Themes\HavocNet\Images\save.gif" ToolTip="Save" CssClass="TaskButton" OnClientClick="javascript:return aTableManage.OnBeforeSave();"></asp:ImageButton>
            </td>
            <td>
                <asp:ImageButton runat="server" ID="cmdEdit" ImageUrl="~\App_Themes\HavocNet\Images\edit.gif" ToolTip="Edit" CssClass="TaskButton" OnClientClick="javascript:return aTableManage.OnBeforeClick('edit');"></asp:ImageButton>
                <asp:ImageButton runat="server" ID="cmdUpdate" ImageUrl="~\App_Themes\HavocNet\Images\update.gif" OnClientClick="javascript:return aTableManage.OnBeforeUpdate();" ToolTip="Update" CssClass="TaskButton"></asp:ImageButton>
            </td>
            <td>
                <asp:ImageButton runat="server" ID="cmdDelete" ImageUrl="~\App_Themes\HavocNet\Images\delete.gif" ToolTip="Delete" CssClass="TaskButton" OnClientClick="javascript:return aTableManage.OnBeforeDelete();"></asp:ImageButton>
                <asp:ImageButton runat="server" ID="cmdCancel" ImageUrl="~\App_Themes\HavocNet\Images\cancel.gif" ToolTip="Cancel" CssClass="TaskButton"></asp:ImageButton>
            </td>
            <td>
                <asp:ImageButton runat="server" ID="cmdExportToExcel" ImageUrl="~\App_Themes\HavocNet\Images\excel.gif" CssClass="TaskButton" ToolTip="Export To Excel" />
            </td>
        </tr>
        </table>
    </div>
    <div style="margin-top: 10px">
        <asp:PlaceHolder runat="server" ID="phTable"></asp:PlaceHolder>
    </div>
    <asp:HiddenField runat="server" ID="hidSelectedRow" />
    <asp:HiddenField runat="server" ID="hidEditingRow" />
</asp:Content>