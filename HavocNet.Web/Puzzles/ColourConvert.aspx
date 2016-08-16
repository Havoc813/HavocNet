<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ColourConvert.aspx.cs" Inherits="HavocNet.Web.Puzzles.ColourConvert" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="..\Scripts\Validate.js"></script>

    <script type="text/javascript">
        function OnConvertRGB() {
            var R = document.getElementById('ctl00_cphMain_txtColourR').value;
            var G = document.getElementById('ctl00_cphMain_txtColourG').value;
            var B = document.getElementById('ctl00_cphMain_txtColourB').value;

            if (!ValidateString(R, "ctl00_cphMain_txtColourR", "Red Value")) { return false; }
            if (!ValidateString(G, "ctl00_cphMain_txtColourG", "Green Value")) { return false; }
            if (!ValidateString(B, "ctl00_cphMain_txtColourB", "Blue Value")) { return false; }

            if (!ValidateNumber(R, "ctl00_cphMain_txtColourR", "Red Value")) { return false; }
            if (!ValidateNumber(G, "ctl00_cphMain_txtColourG", "Green Value")) { return false; }
            if (!ValidateNumber(B, "ctl00_cphMain_txtColourB", "Blue Value")) { return false; }

            if (!CheckNumberSizeMax(R, "ctl00_cphMain_txtColourR", "Red Value", 255)) { return false; }
            if (!CheckNumberSizeMax(G, "ctl00_cphMain_txtColourG", "Green Value", 255)) { return false; }
            if (!CheckNumberSizeMax(B, "ctl00_cphMain_txtColourB", "Blue Value", 255)) { return false; }

            if (!CheckNumberSizeMin(R, "ctl00_cphMain_txtColourR", "Red Value", 0)) { return false; }
            if (!CheckNumberSizeMin(G, "ctl00_cphMain_txtColourG", "Green Value", 0)) { return false; }
            if (!CheckNumberSizeMin(B, "ctl00_cphMain_txtColourB", "Blue Value", 0)) { return false; }

            return true;
        }

        function OnConvertHex() {
            var hex = document.getElementById('txtHexColour').value;

            if (!ValidateString(hex, "txtHexColour", "Hex Colour")) { return false; }

            if (!CheckFirstCharacter(hex, "txtHexColour", "Hex Colour", "#")) { return false; }

            if (!CheckLength(hex, "txtHexColour", "Hex Colour", 7)) { return false; }

            if (!ValidateNumber("0x" + hex.substring(1), "txtHexColour", "Hex Colour")) { return false; }

            return true;
        }
    </script>
    <div class="pageTitleText">
        <table>
        <tr>
            <td>Colour Convert</td>
        </tr>
        </table>
    </div>
    <div>
        <div style="margin-top:5px; margin-bottom:10px">
            Enter a colour into any of the sections below to convert it into the other colour formats.  Named colours are presented in a dropdown box for convenience.  Hex colours must be entered in the format #HHHHHH.  The individual components of the RGB colour must be entered independently.
        </div>
        <div class="subHeader">
            <table><tr>
                <td>Named Colour</td>
                <td><asp:ImageButton runat="server" ID="cmdConvertNamedColour" ImageUrl="~/App_Themes/HavocNet/Images/submit.gif" ToolTip="Convert To Hex & RGB" CssClass="SubTaskButton" /></td>
            </tr></table>
        </div>
        <div>
            Select the named colour to be converted from the dropdown list below.  
            <table>
            <tr>
            <td>
            <table style="margin-top:5px">
                <tr>
                    <td width="150px"><asp:DropDownList runat="server" ID="cboNamedColours"></asp:DropDownList></td>
                </tr>
            </table>
            </td>
            <td>
                <asp:PlaceHolder runat="server" ID="phNamedColourConvert"></asp:PlaceHolder>
            </td>
            </tr>
            </table>
        </div>
        <div class="subHeader">
            <table><tr>
                <td>Hex Colour</td>
                <td><asp:ImageButton runat="server" ID="cmdConvertHexColour" ImageUrl="~/App_Themes/HavocNet/Images/submit.gif" ToolTip="Convert To Named & RGB" OnClientClick="javascript:return OnConvertHex();" CssClass="SubTaskButton" /></td>
            </tr></table>
        </div>
        <div>
            Enter the Hex colour to be converted into the textbox below.
            <table>
            <tr>
            <td>
            <table style="margin-top:5px">
                <tr>
                    <td width="150px"><asp:TextBox runat="server" ID="txtHexColour" Width="120"></asp:TextBox></td>                    
                </tr>
            </table>
            </td>
            <td>
                <asp:PlaceHolder runat="server" ID="phHexColourConvert"></asp:PlaceHolder>
            </td>
            </tr>
            </table>
        </div>
        <div class="subHeader">
            <table><tr>
                <td>RGB Colour</td>
                <td><asp:ImageButton runat="server" ID="cmdConvertRGBColour" ImageUrl="~/App_Themes/HavocNet/Images/submit.gif" ToolTip="Convert To Named & Hex" OnClientClick="javascript:return OnConvertRGB();" CssClass="SubTaskButton" /></td>
            </tr></table>
        </div>
        <div>
            Enter the Red, Green and Blue components of the colour to be converted into the textboxes below.
            <table>
            <tr>
            <td>
            <table style="margin-top:5px">
                <tr>
                    <td width="150px">
                    R: <asp:TextBox runat="server" ID="txtColourR" Width="20" style="text-align:center"></asp:TextBox>
                    G: <asp:TextBox runat="server" ID="txtColourG" Width="20" style="text-align:center"></asp:TextBox>
                    B: <asp:TextBox runat="server" ID="txtColourB" Width="20" style="text-align:center"></asp:TextBox>
                    </td>
                </tr>
            </table>
            </td>
            <td>
                <asp:PlaceHolder runat="server" ID="phRGBColourConvert"></asp:PlaceHolder>
            </td>
            </tr>
            </table>
        </div>
    </div>
</asp:Content>