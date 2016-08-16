<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Polynomial.aspx.cs" Inherits="HavocNet.Web.Puzzles.Polynomial" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="..\Scripts\Validate.js"></script>

    <script type="text/javascript">
        function OnBeforeSolve() {

            if (!ValidateNumber(document.getElementById('ctl00_cphMain_txt1').value, 'ctl00_cphMain_txt1', 'First Coefficient')) { return false; }
            if (!ValidateNumber(document.getElementById('ctl00_cphMain_txt2').value, 'ctl00_cphMain_txt2', 'Second Coefficient')) { return false; }
            if (!ValidateNumber(document.getElementById('ctl00_cphMain_txt3').value, 'ctl00_cphMain_txt3', 'Third Coefficient')) { return false; }

            if (document.getElementById('ctl00_cphMain_txt4') != null) {
                if (!ValidateNumber(document.getElementById('ctl00_cphMain_txt4').value, 'ctl00_cphMain_txt4', 'Fourth Coefficient')) { return false; }
            }
            if (document.getElementById('ctl00_cphMain_txt5') != null) {
                if (!ValidateNumber(document.getElementById('ctl00_cphMain_txt5').value, 'ctl00_cphMain_txt5', 'Fifth Coefficient')) { return false; }
            }
            return true;
        }
    </script>
    <div class="pageTitle">
        <table>
        <tr>
            <td>Polynomial Equation Solver</td>
            <td><asp:dropdownList runat="server" ID="cboDegree" AutoPostBack="true" ToolTip="Degree Of The Polynomial" /></td>
            <td><asp:ImageButton runat="server" ID="cmdSolve" ImageUrl="~\App_Themes\HavocNet\Images\submit.gif" ToolTip="Solve The Puzzle" CssClass="TaskButton" OnClientClick="return OnBeforeSolve();"></asp:ImageButton></td>
            <td><asp:ImageButton runat="server" ID="cmdDemo" ImageUrl="~\App_Themes\HavocNet\Images\help.gif" ToolTip="Display The Demo" CssClass="TaskButton"></asp:ImageButton></td>
            <td><asp:Image runat="server" ID="imgProcessing" ImageUrl="~\App_Themes\HavocNet\Images\busy.gif" style="display:none" CssClass="TaskButton" /></td>
        </tr>
        </table>
    </div>
    <div>
        <div style="margin-top:5px">
            <asp:Label runat="server" ID="lblDescription"></asp:Label>
        </div>
        <div style="padding:5px">
            <asp:Image runat="server" ID="imgForm" />
        </div>
        <asp:PlaceHolder runat="server" ID="phSolutionMethod"></asp:PlaceHolder>
        <div style="margin-top:5px">
            <asp:Label runat="server" ID="lblInstructions"></asp:Label>
        </div>
        <div style="margin-top:10px">
            <asp:PlaceHolder runat="server" ID="phPuzzleEntry"></asp:PlaceHolder>
        </div>
        <div style="margin-top:10px">
            <asp:Label runat="server" ID="lblDescription2"></asp:Label>
        </div>
        <div style="margin-top:10px">
            <asp:PlaceHolder runat="server" ID="phPuzzleSolution"></asp:PlaceHolder>
        </div>
        <div>
            <asp:Label runat="server" ID="lblStatus"></asp:Label>
        </div>
    </div>
</asp:Content>