<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ColourRef.aspx.cs" Inherits="HavocNet.Web.Puzzles.ColourRef" MasterPageFile="~/HavocNet.master" %>

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
            <td>Colour Reference</td>
            <td><asp:CheckBox runat="server" ID="chkInclude" Text="Include System Colours?" AutoPostBack="true" CssClass="TaskButton" /></td>
            <td><asp:ImageButton runat="server" ID="cmdExportToExcel" ImageUrl="~/App_Themes/HavocNet/Images/excel.gif" CssClass="TaskButton" ToolTip="Export To Excel" /></td>
        </tr>
        </table>
    </div>
    <div>
        <asp:PlaceHolder runat="server" ID="phColourReferences"></asp:PlaceHolder>
    </div>
</asp:Content>