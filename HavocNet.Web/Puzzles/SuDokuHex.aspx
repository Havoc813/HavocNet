﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuDokuHex.aspx.cs" Inherits="HavocNet.Web.Puzzles.SuDokuHexPage" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="/Scripts/SuDokuKeys.js"></script>

    <script type="text/javascript">
        function OnSolve() {
            document.getElementById('ctl00_cphMain_imgProcessing').style.display = 'inline';
            document.getElementById('ctl00_cphMain_cmdSolve').style.display = 'none';
            document.getElementById('ctl00_cphMain_cmdDemo').style.display = 'none';
            document.getElementById('ctl00_cphMain_cmdClear').style.display = 'none';

            return true;
        }
        var aSudokuKeys = new SuDokuKeys(16,16);
    </script>
    <div class="pageTitle">
        <table>
        <tr>
            <td>Hexadecimal Su-Doku Solver</td>
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
                <td style="padding: 3px 0 0 10px" valign="top"><asp:Label runat="server" ID="lblStatus"></asp:Label></td>
            </tr>
        </table>
    </div>
</asp:Content>