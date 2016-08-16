<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuLexicon.aspx.cs" Inherits="HavocNet.Web.MenuLexicon" MasterPageFile="HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
<script type="text/javascript" src="~\Scripts\Validate.js"></script>

<script type="text/javascript">
    function ValidateLetters(lettersToValidate, lettersControlId) {
        var i;
        for (i = 0; i < lettersToValidate.length; i++) {
            var charCode = lettersToValidate.toString().charCodeAt(i);

            if (!((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || charCode == 63)) {
                alert('Letter options must only contain letters or ?');
                document.getElementById(lettersControlId).focus();
                return false;
            }
        }
        return true;
    }

    function ValidateInputMask(inputMaskToValidate, inputMaskControlId) {
        var i;
        for (i = 0; i < inputMaskToValidate.length; i++) {
            var charCode = inputMaskToValidate.toString().charCodeAt(i);

            if (!((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122))) {
                if (!(charCode == 91 || charCode == 93 || charCode == 95 || charCode == 37)) {
                    alert('Input mask must only contain letters, _, %, [, ]');
                    document.getElementById(inputMaskControlId).focus();
                    return false;
                }
            }
        }
        return true;
    }

    function OnBeforeAnalyse() {
        //Check letter options ? letters
        if (!ValidateLetters(document.getElementById('ctl00_cphMain_txtLetterOptions').value, 'ctl00_cphMain_txtLetterOptions')) { return false; }

        //Check input mask _?%[] letters
        if (!ValidateInputMask(document.getElementById('ctl00_cphMain_txtInputMask').value, 'ctl00_cphMain_txtInputMask')) { return false; }

        return true;
    }
</script>
<div class="pageTitle">
    <table>
    <tr>
        <td>Language Analysis</td>
        <td><asp:ImageButton runat="server" ID="cmdRun" Tooltip="Analyse" ImageUrl="App_Themes/HavocNet/Images/submit.gif" OnClientClick="javascript:return OnBeforeAnalyse();" CssClass="TaskButton"></asp:ImageButton></td>
    </tr>
    </table>
</div>
<div>
    <table>
        <tr>
            <td>Dictionary</td>
            <td><asp:DropDownList runat="server" ID="cboDictionaries" CssClass="MyDropdown"></asp:DropDownList></td>
        </tr>
        <tr>
            <td style="padding-top:4px">Input Mask</td>
            <td style="padding-left:4px; padding-top:6px"><asp:TextBox runat="server" ID="txtInputMask" Width="200"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="padding-top:4px">Letter Options</td>
            <td style="padding-left:4px; padding-top:6px"><asp:TextBox runat="server" ID="txtLetterOptions" Width="200"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="padding-top:4px">Set Type</td>
            <td style="padding-top:6px">
                <asp:RadioButtonList runat="server" ID="rdoSetType" RepeatColumns="2" RepeatLayout="Flow">
                    <asp:ListItem Value="Sub" Text="Sub" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="Super" Text="Super"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="padding-top:4px">Ordering</td>
            <td style="padding-top:6px">
                <asp:RadioButtonList runat="server" ID="rdoOrderBy" RepeatColumns="2" RepeatLayout="Flow">
                    <asp:ListItem Value="Length" Text="Length"></asp:ListItem>
                    <asp:ListItem Value="Score" Text="Score" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
</div>
<div runat="server" id="results">
    <div class="subHeaderText">
        <table>
            <tr>
                <td>Results</td>
            </tr>
        </table>
    </div>
    <div>
        <asp:PlaceHolder runat="server" ID="phStats"></asp:PlaceHolder>
    </div>
    <div>
        <asp:Label runat="server" ID="lblStatus" ForeColor="Red"></asp:Label>
    </div>
    <div style="padding-top:8px">
        <asp:PlaceHolder runat="server" ID="phResults"></asp:PlaceHolder>
    </div>
</div>
</asp:Content>