<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RandomNumbers.aspx.cs" Inherits="HavocNet.Web.Puzzles.RandomNumbers" MasterPageFile="~/HavocNet.master" %>

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
            <td>Random Number Generator</td>
            <td><asp:ImageButton runat="server" ID="cmdGenerate" CssClass="TaskButton" ImageUrl="~\App_Themes\HavocNet\Images\submit.gif"></asp:ImageButton></td>
        </tr>
        </table>
    </div>
    <div style="margin-top:5px; margin-bottom:10px">
        This is a random number generator.  Click the "Generate" button to create a new random number between 1 and 100.
    </div>
    <div>
        <asp:PlaceHolder runat="server" ID="phRandom"></asp:PlaceHolder>
    </div>
    <div style="margin-top:5px">
        <asp:Label runat="server" ID="lblHistory"></asp:Label>
    </div>
</asp:Content>