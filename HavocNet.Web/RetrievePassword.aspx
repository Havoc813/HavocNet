<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RetrievePassword.aspx.cs" Inherits="HavocNet.Web.RetrievePassword" MasterPageFile="HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="Scripts/Validate.js"></script>
    <script type="text/javascript">
        function OnBeforeClick() {
            if (!ValidateEmail(document.getElementById('ctl00_cphMain_txtEmailAddress').value, 'ctl00_cphMain_txtEmailAddress', 'Email Address')) { return false; }

            return true;
        }
    </script>
<div id="mainpage" style="width:100%">
    <div style="width: 100%;">
        <table style="margin: 100px auto auto auto; border-left:solid 2px black; border-right: solid 2px black;">
            <tr>
                <td style="padding:10px 18px 7px 18px; font-size:14px"><b>Forgotten Password</b></td>
            </tr>
            <tr>
                <td style="padding:7px 18px 7px 18px">Enter your username and the email address with which you registered below.  A new password will be sent to you.</td>
            </tr>
            <tr>
                <td style="padding-left:12px">
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="cmdReset">
                        <table style="margin-left: 150px">
                            <tr>
                                <td align="right">Username <asp:TextBox runat="server" Width="150" ID="txtUsername"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right">Email Address <asp:TextBox runat="server" Width="150" ID="txtEmailAddress"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right"><asp:LinkButton runat="server" ID="cmdReset" CssClass="MyButton" Text="Reset" Width="146" OnClientClick="return OnBeforeClick();"></asp:LinkButton></td>
                            </tr>
                        </table>                    
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="padding:10px 18px 10px 18px"><asp:Label runat="server" ID="lblStatus" ForeColor="Red"></asp:Label></td>
            </tr>
        </table>
    </div>
</div>
<asp:HiddenField runat="server" ID="hidReset"/>
<script type="text/javascript">
    document.getElementById('ctl00_cphMain_txtUsername').focus();
    
    if (document.getElementById('ctl00_cphMain_hidReset').value == 'login') {
        window.setTimeout("window.location.href = '/Login.aspx'", 5000);
    }
</script>
</asp:Content>