<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuDokuOddEven.aspx.cs" Inherits="HavocNet.Web.Puzzles.SuDokuOddEvenPage" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="/Scripts/SuDokuKeys.js"></script>

    <script type="text/javascript">
        function OnSolve() {
            var count = 0;

            for (var i = 0; i <= 8; i++) {
                for (var j = 0; j <= 8; j++) {
                    if (document.getElementById('ctl00_cphMain_txt0' + String(i) + '0' + String(j)).style.backgroundColor != '') {
                        count++;
                    }
                }
            }

            if (count != 45) {
                alert('There must be 45 odd squares marked.  Only ' + String(count) + ' so far.');

                return false;
            }
            else {
                document.getElementById('ctl00_cphMain_imgProcessing').style.display = 'inline';
                document.getElementById('ctl00_cphMain_cmdSolve').style.display = 'none';
                document.getElementById('ctl00_cphMain_cmdDemo').style.display = 'none';
                document.getElementById('ctl00_cphMain_cmdClear').style.display = 'none';

                return true;
            }
        }
        
        function OnKeyPressWithEnter(sender) {
            switch (window.event.keyCode) {
                case 13:
                    return OnCarriageReturn(sender);
                default:
                    return aSudokuKeys.OnKeyPressStub(sender);
            }
        }
        
        function OnCarriageReturn(sender) {
            if (sender.style.backgroundColor != '#cccccc') {
                sender.style.backgroundColor = '#cccccc';
                document.getElementById(sender.id.toString().replace("txt", "cell")).style.backgroundColor = '#cccccc';
                document.getElementById("ctl00_cphMain_hidOddCells").value += sender.id.toString().replace("ctl00_cphMain_txt", "") + ";";
            }
            else {
                sender.style.backgroundColor = '';
                document.getElementById(sender.id.toString().replace("txt", "cell")).style.backgroundColor = '';
                document.getElementById("ctl00_cphMain_hidOddCells").value = document.getElementById("ctl00_cphMain_hidOddCells").value.replace(sender.id.toString().replace("ctl00_cphMain_txt", "") + ";", "");
            }
            return false;
        }

        var aSudokuKeys = new SuDokuKeys(9, 9);
        aSudokuKeys.OnKeyPress = OnKeyPressWithEnter;
    </script>
    <div class="pageTitle">
        <table>
        <tr>
            <td>Odd/Even Su-Doku Solver</td>
            <td><asp:ImageButton runat="server" ID="cmdSolve" ImageUrl="~\App_Themes\HavocNet\Images\submit.gif" ToolTip="Solve The Puzzle" CssClass="TaskButton" OnClientClick="javascript:return OnSolve();"></asp:ImageButton></td>
            <td><asp:ImageButton runat="server" ID="cmdDemo" ImageUrl="~\App_Themes\HavocNet\Images\help.gif" ToolTip="Display The Demo" CssClass="TaskButton"></asp:ImageButton></td>
            <td><asp:ImageButton runat="server" ID="cmdClear" ImageUrl="~\App_Themes\HavocNet\Images\new.gif" ToolTip="Clear The Puzzle" CssClass="TaskButton" OnClientClick="return confirm('Are you sure you wish to clear?');"></asp:ImageButton></td>
            <td><asp:Image runat="server" ID="imgProcessing" ImageUrl="~\App_Themes\HavocNet\Images\busy.gif" style="display:none" CssClass="TaskButton" /></td>
        </tr>
        </table>
    </div>
    <div style="margin-top: 10px;">
        <table>
            <tr>
                <td><asp:PlaceHolder runat="server" ID="phPuzzle"></asp:PlaceHolder></td>
                <td style="padding: 3px 0 0 10px" valign="top">
                    <span style='font-weight:bold'>Instructions</span>
                    <p>
                        To mark a cell as containing an odd number, select that cell and press the enter key.  Continue until all the odd cells are coloured grey.  
                    </p>
                    <p>
                        Enter the numerical initial conditions and press the solve button above.
                    </p>
                    <asp:Label runat="server" ID="lblStatus"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hidOddCells"/>
</asp:Content>