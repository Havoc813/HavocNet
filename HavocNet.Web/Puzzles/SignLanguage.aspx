<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SignLanguage.aspx.cs" Inherits="HavocNet.Web.Puzzles.SignLanguage" MasterPageFile="~/HavocNet.master" %>

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
            <td>Sign Language Convertor</td>
        </tr>
        </table>
    </div>
    <div>
        <div style="margin-top: 5px">
            Provides the facility to convert from text to sign language.  A sign language reference is provided for information  
        </div>
        <div class="subHeader">
            <table><tr>
                <td>Text >> Morse</td>
                <td><asp:ImageButton runat="server" ID="cmdMorseToText" ImageUrl="~/App_Themes/HavocNet/Images/submit.gif" ToolTip="Convert Text To Morse" CssClass="SubTaskButton" /></td>
            </tr></table>
        </div>
        <div>
            Enter the text you wish to convert into sign language into the textbox below.  Press the convert button to view the signs.
        </div>
        <div style="margin-top: 5px">
            <asp:TextBox runat="server" BorderColor="Gray" BorderWidth="1" ID="txtPlainText" Rows="5" Columns="95" TextMode="MultiLine"></asp:TextBox>
        </div>
        <div style="margin-top: 5px">
            <asp:PlaceHolder runat="server" ID="phSignAnswer"></asp:PlaceHolder>
        </div>
        <div class="subHeaderText">
            <table><tr>
                <td>Sign Reference</td>
            </tr></table>
        </div>
        <div style="margin-top: 5px">
            <asp:PlaceHolder runat="server" ID="phSigns"></asp:PlaceHolder>
        </div>
    </div>
</asp:Content>