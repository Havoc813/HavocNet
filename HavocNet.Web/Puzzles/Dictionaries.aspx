<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dictionaries.aspx.cs" Inherits="HavocNet.Web.Puzzles.Dictionaries" MasterPageFile="..\HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="/Scripts/Validate.js"></script>
    <script type="text/javascript" src="/Scripts/TableManage.js"></script>
   
    <script type="text/javascript">
        function OnBeforeUpdateWithValidate() {
            if (!ValidateString(document.getElementById("ctl00_cphMain_txtEditDictionaryName").value, "ctl00_cphMain_txtEditDictionaryName", "Name")) { return false; }
            if (!ValidateString(document.getElementById("ctl00_cphMain_txtEditDictionaryDescription").value, "ctl00_cphMain_txtEditDictionaryDescription", "Description")) { return false; }

            return confirm("Update this dictionary?");
        }

        function OnBeforeSaveWithValidate() {
            if (!ValidateString(document.getElementById("ctl00_cphMain_txtNewDictionaryName").value, "ctl00_cphMain_txtNewDictionaryName", "Name")) { return false; }
            if (!ValidateString(document.getElementById("ctl00_cphMain_txtNewDictionaryDescription").value, "ctl00_cphMain_txtNewDictionaryDescription", "Description")) { return false; }
            
            return confirm("Save this dictionary?");
        }
        
        function ChangeButtons(id) {
            var loaded = document.getElementById('ctl00_cphMain_hid' + id).value;

            if (loaded == 'True') {
                document.getElementById('ctl00_cphMain_upWords').style.display = "none";
                document.getElementById('ctl00_cphMain_cmdUpload').style.display = "none";
                document.getElementById('ctl00_cphMain_cmdClear').style.display = "inline";
            }
            else {
                document.getElementById('ctl00_cphMain_upWords').style.display = "inline";
                document.getElementById('ctl00_cphMain_cmdUpload').style.display = "inline";
                document.getElementById('ctl00_cphMain_cmdClear').style.display = "none";
            }
        }
        
        function OnWordsUpload() {
            document.getElementById('ctl00_cphMain_imgProcessing').style.display = 'inline';
            document.getElementById('ctl00_cphMain_cmdClear').style.display = 'none';
            document.getElementById('ctl00_cphMain_cmdUpload').style.display = 'none';
            document.getElementById('ctl00_cphMain_upWords').style.display = 'none';

            return true;
        }
        
        var aTableManage = new TableManage('Dictionary');
        aTableManage.OnBeforeSave = OnBeforeSaveWithValidate;
        aTableManage.OnBeforeUpdate = OnBeforeUpdateWithValidate;
    </script>
    <div class="pageTitle">
        <table>
        <tr>
            <td>Manage Dictionaries</td>
            <td>
                <asp:ImageButton runat="server" ID="cmdAdd" ImageUrl="~\App_Themes\HavocNet\Images\new.gif" ToolTip="Add New" CssClass="TaskButton"></asp:ImageButton>
                <asp:ImageButton runat="server" ID="cmdSave" ImageUrl="~\App_Themes\HavocNet\Images\save.gif" ToolTip="Save" CssClass="TaskButton" OnClientClick="javascript:return aTableManage.OnBeforeSave();"></asp:ImageButton>
            </td>
            <td>
                <asp:ImageButton runat="server" ID="cmdEdit" ImageUrl="~\App_Themes\HavocNet\Images\edit.gif" ToolTip="Edit" CssClass="TaskButton" OnClientClick="javascript:return aTableManage.OnBeforeClick('edit');"></asp:ImageButton>
                <asp:ImageButton runat="server" ID="cmdUpdate" ImageUrl="~\App_Themes\HavocNet\Images\update.gif" OnClientClick="javascript:return aTableManage.OnBeforeUpdate();" ToolTip="Update" CssClass="TaskButton"></asp:ImageButton>
            </td>
            <td>
                <asp:ImageButton runat="server" ID="cmdDelete" ImageUrl="~\App_Themes\HavocNet\Images\delete.gif" ToolTip="Delete Dictionary" CssClass="TaskButton" OnClientClick="javascript:return aTableManage.OnBeforeDelete();"></asp:ImageButton>
                <asp:ImageButton runat="server" ID="cmdCancel" ImageUrl="~\App_Themes\HavocNet\Images\cancel.gif" ToolTip="Cancel" CssClass="TaskButton"></asp:ImageButton>
            </td>
            <td>
                <asp:ImageButton runat="server" ID="cmdUp" ImageUrl="~\App_Themes\HavocNet\Images\up.gif" CssClass="TaskButton" ToolTip="Move Up" />
            </td>
            <td>
                <asp:ImageButton runat="server" ID="cmdDown" ImageUrl="~\App_Themes\HavocNet\Images\down.gif" CssClass="TaskButton" ToolTip="Move Down" />
            </td>
            <td>
                <asp:ImageButton runat="server" ID="cmdExportToExcel" ImageUrl="~\App_Themes\HavocNet\Images\excel.gif" CssClass="TaskButton" ToolTip="Export To Excel" />
            </td>
            <td>and Words </td>
            <td><asp:Image runat="server" ID="imgProcessing" ImageUrl="~\App_Themes\HavocNet\Images\busy.gif" style="display:none" CssClass="TaskButton" /></td>
            <td>
                <asp:ImageButton runat="server" ID="cmdClear" ImageUrl="~\App_Themes\HavocNet\Images\cancel.gif" ToolTip="Delete Words" CssClass="TaskButton" OnClientClick="return confirm('Delete all words from this dictionary?');"></asp:ImageButton>
                <asp:ImageButton runat="server" ID="cmdUpload" ImageUrl="~\App_Themes\HavocNet\Images\save.gif" ToolTip="Save" CssClass="TaskButton" OnClientClick="return OnWordsUpload();"></asp:ImageButton>
            </td>
            <td>
                <asp:FileUpload runat="server" Width="200" CssClass="TaskButton" ID="upWords" />
            </td>
        </tr>
        </table>
    </div>
    <div>
        <asp:PlaceHolder runat="server" ID="phTable"></asp:PlaceHolder>
    </div>
    <asp:HiddenField runat="server" ID="hidSelectedRow" />
    <asp:HiddenField runat="server" ID="hidEditingRow" />
</asp:Content>