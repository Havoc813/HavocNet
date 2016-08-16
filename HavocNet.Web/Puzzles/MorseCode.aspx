<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MorseCode.aspx.cs" Inherits="HavocNet.Web.Puzzles.MorseCode" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="..\Scripts\Validate.js"></script>

    <script type="text/javascript">
        function OnBeforeSolve() {
            return true;
        }
    </script>
    <div class="pageTitleText">
        <table>
        <tr>
            <td>Morse Code Convertor</td>
        </tr>
        </table>
    </div>
    <div>
        <div style="margin-top: 5px">
            Provides the facility to convert from text to morse code and morse code to text.  A morse code reference is provided for information  
        </div>
        <div class="subHeader">
            <table><tr>
                <td>Text >> Morse</td>
                <td><asp:ImageButton runat="server" ID="cmdMorseToText" ImageUrl="~/App_Themes/HavocNet/Images/submit.gif" ToolTip="Convert Text To Morse" CssClass="SubTaskButton" /></td>
            </tr></table>
        </div>
        <div>
            Enter the text you wish to convert into morse code into the textbox below.  Press the convert button to view the morse code.
        </div>
        <div style="margin-top: 5px">
            <asp:TextBox runat="server" BorderColor="Gray" BorderWidth="1" ID="txtPlainText" Rows="5" Columns="95" TextMode="MultiLine"></asp:TextBox>
        </div>
        <div style="margin-top: 5px">
            <asp:PlaceHolder runat="server" ID="phMorseCode"></asp:PlaceHolder>
        </div>
        <div class="subHeader">
            <table><tr>
                <td>Morse >> Text</td>
                <td><asp:ImageButton runat="server" ID="cmdTextToMorse" ImageUrl="~/App_Themes/HavocNet/Images/submit.gif" ToolTip="Convert Text To Morse" CssClass="SubTaskButton" /></td>
            </tr></table>
        </div>
        <div>
            Enter the morse code you wish to convert into text into the textbox below.  Separate words with forward slashes (/) and separate characters with spaces.  Press the convert button to view the morse code.
        </div>
        <div style="margin-top: 5px">
            <asp:TextBox runat="server" BorderColor="Gray" BorderWidth="1" ID="txtMorseCode" Rows="5" Columns="95" TextMode="MultiLine"></asp:TextBox>
        </div>
        <div style="margin-top: 5px">
            <asp:PlaceHolder runat="server" ID="phPlainText"></asp:PlaceHolder>
        </div>
    </div>
</asp:Content>