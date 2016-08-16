<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CharacterRef.aspx.cs" Inherits="HavocNet.Web.Puzzles.CharacterRef" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="..\Scripts\Validate.js"></script>

    <script type="text/javascript">
        function OnBeforeSolve() {
            return true;
        }
    </script>
    <div class="pageTitle">
        <table>
        <tr>
            <td>Character Reference</td>
            <td><asp:DropDownList runat="server" ID="cboCharacterType" AutoPostBack="true" /></td>
            <td><asp:ImageButton runat="server" ID="cmdExportToExcel" ImageUrl="~/App_Themes/HavocNet/Images/excel.gif" CssClass="TaskButton" ToolTip="Export To Excel" /></td>
        </tr>
        </table>
    </div>
    <div>
        <asp:PlaceHolder runat="server" ID="phColourReferences"></asp:PlaceHolder>
    </div>
</asp:Content>