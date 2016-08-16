<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuDokuPoly.aspx.cs" Inherits="HavocNet.Web.Puzzles.SuDokuPolyPage" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="/Scripts/SuDokuKeys.js"></script>

    <script type="text/javascript">
        function OnSolve() {
            var aTable = document.getElementById('ctl00_cphMain_aPuzzle');
            var hid = document.getElementById('ctl00_cphMain_hidPolyConditions');

            document.getElementById('ctl00_cphMain_imgProcessing').style.display = 'inline';
            document.getElementById('ctl00_cphMain_cmdSolve').style.display = 'none';
            document.getElementById('ctl00_cphMain_cmdDemo').style.display = 'none';
            document.getElementById('ctl00_cphMain_cmdClear').style.display = 'none';
            document.getElementById('ctl00_cphMain_hidPolyConditions').value = '';

            for (var i = 0; i <= 8; i++) {
                for (var j = 0; j <= 8; j++) {
                    hid.value += GetValFromColour(aTable.rows[j].cells[i].style.backgroundColor) + ';';
                }
                hid.value += '|';
            }
            return true;
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
            if (sender.style.backgroundColor != document.getElementById('ctl00_cphMain_hidColour').value) {
                sender.style.backgroundColor = document.getElementById('ctl00_cphMain_hidColour').value;
                document.getElementById(sender.id.toString().replace("txt", "cell")).style.backgroundColor = document.getElementById('ctl00_cphMain_hidColour').value;
            }
            return false;
        }

        function GetValFromColour(aColour) {
            switch (aColour) {
                case 'lawngreen', 'rgb(124, 252, 0)':
                    return 1;
                case 'crimson', 'rgb(220, 20, 60)':
                    return 2;
                case 'steelblue', 'rgb(70, 130, 180)':
                    return 3;
                case 'yellow':
                    return 4;
                case 'violet', 'rgb(238, 130, 238)':
                    return 5;
                case 'orangered', 'rgb(255, 69, 0)':
                    return 6;
                case 'cyan', 'rgb(0, 255, 255)':
                    return 7;
                case 'pink', 'rgb(255, 192, 203)':
                    return 8;
                case 'goldenrod', 'rgb(218, 165, 32)':
                    return 9;
                default:
                    return 0;
            }
        }

        var aSudokuKeys = new SuDokuKeys(9, 9);
        aSudokuKeys.OnKeyPress = OnKeyPressWithEnter;
    </script>
    <div class="pageTitle">
        <table>
        <tr>
            <td>Polgon Su-Doku Solver</td>
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
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:PlaceHolder runat="server" ID="phPuzzle"></asp:PlaceHolder>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="margin: 10px 0 0 1px; width:100%">
                                <tr style="border:solid 2px black;">
                                <td class="sudokuCell" style="background-color: lawngreen"><asp:RadioButton ID="rdoColours1" runat="server" GroupName="rdoColours" CssClass="polyRadio" /></td>
                                <td class="sudokuCell" style="background-color: crimson"><asp:RadioButton ID="rdoColours2" runat="server" GroupName="rdoColours" CssClass="polyRadio" /></td>
                                <td class="sudokuCell" style="background-color: steelblue"><asp:RadioButton ID="rdoColours3" runat="server" GroupName="rdoColours" CssClass="polyRadio" /></td>
                                <td class="sudokuCell" style="background-color: yellow"><asp:RadioButton ID="rdoColours4" runat="server" GroupName="rdoColours" CssClass="polyRadio" /></td>
                                <td class="sudokuCell" style="background-color: violet"><asp:RadioButton ID="rdoColours5" runat="server" GroupName="rdoColours" CssClass="polyRadio" /></td>
                                <td class="sudokuCell" style="background-color: orangered"><asp:RadioButton ID="rdoColours6" runat="server" GroupName="rdoColours" CssClass="polyRadio" /></td>
                                <td class="sudokuCell" style="background-color: cyan"><asp:RadioButton ID="rdoColours7" runat="server" GroupName="rdoColours" CssClass="polyRadio" /></td>
                                <td class="sudokuCell" style="background-color: pink"><asp:RadioButton ID="rdoColours8" runat="server" GroupName="rdoColours" CssClass="polyRadio" /></td>
                                <td class="sudokuCell" style="background-color: goldenrod"><asp:RadioButton ID="rdoColours9" runat="server" GroupName="rdoColours" CssClass="polyRadio" /></td>
                                </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    
                </td>
                <td style="padding: 3px 0 0 10px" valign="top">
                    <span style='font-weight:bold'>Instructions</span>
                    <p>
                        To demarkate the separate boxes for the polygon Su-Doku, select a colour, place the cursor in each of the cells that correspond to that colour polygon and press the enter key.  
                    </p>
                    <p>
                        Once all of the grid is coloured, enter the initial conditions and press solve.
                    </p>
                    <asp:Label runat="server" ID="lblStatus"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hidColour" />
    <asp:HiddenField runat="server" ID="hidPolyConditions" />
</asp:Content>