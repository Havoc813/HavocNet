<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuDokuKiller.aspx.cs" Inherits="HavocNet.Web.Puzzles.SuDokuKillerPage" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="/Scripts/SuDokuKeys.js"></script>

    <script type="text/javascript">
        function OnSolve() {
            if (document.getElementById('ctl00_cphMain_hidConditionCount').value == '81') {
                document.getElementById('ctl00_cphMain_imgProcessing').style.display = 'inline';
                document.getElementById('ctl00_cphMain_cmdSolve').style.display = 'none';
                document.getElementById('ctl00_cphMain_cmdDemo').style.display = 'none';
                document.getElementById('ctl00_cphMain_cmdClear').style.display = 'none';
                document.getElementById('ctl00_cphMain_txtSumAmount').style.display = 'none';
                document.getElementById('ctl00_cphMain_cmdSave').style.display = 'none';

                return true;
            }
            else {
                alert('Not all cells are included in the conditions.  Please complete the definition.');

                return false;
            }
        }

        function SelectCell(sender) {
            if (sender.style.backgroundColor == '') {
                sender.style.backgroundColor = 'lightblue';
            }
            else {
                sender.style.backgroundColor = '';
            }
        }

        function OnSave() {
            var selected = '';
            var i;

            for (i = 0; i <= 8; i++) {
                for (var j = 0; j <= 8; j++) {
                    var aCell = document.getElementById("ctl00_cphMain_cell0" + String(i) + "0" + String(j));

                    if (aCell.style.backgroundColor == 'lightblue' || aCell.style.backgroundColor == 'rgb(173, 216, 230)') {
                        selected += i + ';' + j + '|';

                        aCell.style.backgroundColor = '';

                        aCell.innerText = document.getElementById('ctl00_cphMain_txtSumAmount').value;

                        aCell.onclick = '';

                        aCell.style.border = 'solid 2px black';
                    }
                }
            }

            var splitSelected = selected.split("|");

            for (i = 0; i < splitSelected.length; i++) {
                var aStr = splitSelected[i];
                if (aStr != '') {
                    document.getElementById('ctl00_cphMain_hidConditionCount').value = String(Number(document.getElementById('ctl00_cphMain_hidConditionCount').value) + 1);

                    if (selected.indexOf(String(Number(aStr.split(";")[0]) + 1) + ";" + aStr.split(";")[1] + "|") > -1) {
                        document.getElementById('ctl00_cphMain_cell0' + aStr.split(";")[0] + "0" + aStr.split(";")[1]).style.borderRight = "solid 0px white";
                        document.getElementById('ctl00_cphMain_cell0' + String(Number(aStr.split(";")[0]) + 1) + "0" + aStr.split(";")[1]).style.borderLeft = "solid 0px white";
                    }
                    if (selected.indexOf(aStr.split(";")[0] + ";" + String(Number(aStr.split(";")[1]) + 1) + "|") > -1) {
                        document.getElementById('ctl00_cphMain_cell0' + aStr.split(";")[0] + "0" + aStr.split(";")[1]).style.borderBottom = "solid 0px white";
                        document.getElementById('ctl00_cphMain_cell0' + aStr.split(";")[0] + "0" + String(Number(aStr.split(";")[1]) + 1)).style.borderTop = "solid 0px white";
                    }
                }
            }

            document.getElementById('ctl00_cphMain_hidInitialConditions').value += selected + ',' + document.getElementById('ctl00_cphMain_txtSumAmount').value + '&';

            alert(document.getElementById('ctl00_cphMain_hidInitialConditions').value);

            return false;
        }
    </script>
    <div class="pageTitle">
        <table>
        <tr>
            <td>Killer Su-Doku Solver</td>
            <td><asp:ImageButton runat="server" ID="cmdSolve" ImageUrl="~\App_Themes\HavocNet\Images\submit.gif" ToolTip="Solve The Puzzle" CssClass="TaskButton" OnClientClick="javascript:return OnSolve();"></asp:ImageButton></td>
            <td><asp:ImageButton runat="server" ID="cmdDemo" ImageUrl="~\App_Themes\HavocNet\Images\help.gif" ToolTip="Display The Demo" CssClass="TaskButton"></asp:ImageButton></td>
            <td><asp:ImageButton runat="server" ID="cmdClear" ImageUrl="~\App_Themes\HavocNet\Images\new.gif" ToolTip="Clear The Puzzle" CssClass="TaskButton" OnClientClick="return confirm('Are you sure you wish to clear?');"></asp:ImageButton></td>
            <td><asp:TextBox runat="server" width="50px" ID="txtSumAmount" style="text-align:center;"></asp:TextBox></td>
            <td><asp:imagebutton runat="server" ID="cmdSave" ImageUrl="~\App_Themes\HavocNet\Images\edit.gif" ToolTip="Save" CssClass="TaskButton" OnClientClick="javascript:return OnSave();" /></td>
            <td><asp:Image runat="server" ID="imgProcessing" ImageUrl="~\App_Themes\HavocNet\Images\busy.gif" style="display:none" CssClass="TaskButton" /></td>
        </tr>
        </table>
    </div>
    <div style="margin-top: 10px;">
        <table>
            <tr>
                <td width="310px"><asp:PlaceHolder runat="server" ID="phPuzzle"></asp:PlaceHolder></td>
                <td width="20px"><asp:Image runat="server" ImageUrl="~\App_Themes\HavocNet\Images\Puzzles\HorizGreater.gif" Visible="false" ID="imgNext" style="padding-left:10px; padding-right:10px" /></td>
                <td width="310px"><asp:PlaceHolder runat="server" ID="phPuzzle2"></asp:PlaceHolder></td>
                <td>&nbsp;</td>
            </tr>
            <tr><td colspan="4">&nbsp;</td></tr>
            <tr>
                <td colspan="4">
                    <span style='font-weight:bold'>Instructions</span>
                    <p>
                        To enter the initial sum conditions, click on the cells as required, enter the value of their sum in the textbox above and press the arrow button to commit.  
                        This will highlight the condition in the puzzle.  
                    </p>
                    <p>
                        Once all conditions have been added, pressing the solve button will return a solution.
                    </p>                        
                    <asp:Label runat="server" ID="lblStatus"></asp:Label>                    
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hidInitialConditions" />
        <asp:HiddenField runat="server" ID="hidConditionCount" />
    </div>
</asp:Content>