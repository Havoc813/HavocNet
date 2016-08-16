<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Futoshiki.aspx.cs" Inherits="HavocNet.Web.Puzzles.FutoshikiPage" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="/Scripts/SuDokuKeys.js"></script>

    <script type="text/javascript">
        function ChangeImage(sender, direction) {
            var flag;

            if (sender.src.toString().indexOf('None.gif') > 0) {
                sender.src = "../App_Themes/HavocNet/Images/Puzzles/" + direction + "Greater.gif";
                flag = 2;
            }
            else if (sender.src.toString().indexOf('Greater.gif') > 0) {
                sender.src = "../App_Themes/HavocNet/Images/Puzzles/" + direction + "Lesser.gif";
                flag = 1;
            }
            else {
                sender.src = "../App_Themes/HavocNet/Images/Puzzles/None.gif";
                flag = 0;
            }

            var cellID = sender.id.toString().substring(14, 15) + '|' + sender.id.toString().substring(15, 16) + ';' + sender.id.toString().substring(16, 17);

            if (document.getElementById('ctl00_cphMain_hidInitialConditions').value.toString().indexOf(cellID) > -1) {
                var initial = document.getElementById('ctl00_cphMain_hidInitialConditions').value.toString();
                var oldCondition = initial.substring(initial.indexOf(cellID), initial.indexOf(cellID) + 8);

                if (flag != 0) {
                    document.getElementById('ctl00_cphMain_hidInitialConditions').value = initial.replace(oldCondition, cellID + '|' + flag + ',');
                }
                else {
                    document.getElementById('ctl00_cphMain_hidInitialConditions').value = initial.replace(oldCondition, '');
                }
            }
            else {
                document.getElementById('ctl00_cphMain_hidInitialConditions').value += cellID + '|' + flag + ',';
            }

            return false;
        }
        
        function OnSolve() {
            document.getElementById('ctl00_cphMain_imgProcessing').style.display = 'inline';
            document.getElementById('ctl00_cphMain_cmdSolve').style.display = 'none';
            document.getElementById('ctl00_cphMain_cmdDemo').style.display = 'none';
            document.getElementById('ctl00_cphMain_cmdClear').style.display = 'none';
            document.getElementById('ctl00_cphMain_cboOrder').style.display = 'none';

            return true;
        }

        var aSudokuKeys = new SuDokuKeys(9, 9);
    </script>
    <div class="pageTitle">
        <table>
        <tr>
            <td>Futoshiki Solver</td>
            <td><asp:DropDownList runat="server" ID="cboOrder" CssClass="TaskButton" AutoPostBack="true"></asp:DropDownList></td>
            <td><asp:Checkbox runat="server" ID="chkUseSuDokuConditions" Text="SuDoku?" Visible="false"></asp:Checkbox></td>
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
                        Select the order of the puzzle from the dropdown box.
                    </p>
                    <p>
                        Enter the numerical conditions in the cells.  
                    </p>
                    <p>
                        Click the images between the cells to set the greater/less than conditions that the cells must fullfill.
                    </p>
                    <asp:Label runat="server" ID="lblStatus"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField runat="server" ID="hidInitialConditions" />
</asp:Content>