<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="HavocNet.Web.ChangePassword" MasterPageFile="HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="Scripts/Validate.js"></script>
    <script type="text/javascript">
        function OnBeforeClick() {
            if (document.getElementById("ctl00_cphMain_txtNewPassword").value != document.getElementById("ctl00_cphMain_txtNewPasswordConfirm").value) {
                alert('Passwords must match');
                document.getElementById("ctl00_cphMain_txtNewPassword").focus();
                return false;
            }
            if (!ValidatePassword(document.getElementById('ctl00_cphMain_txtNewPassword').value, 'ctl00_cphMain_txtNewPassword', 'Password')) { return false; }

            return true;
        }
    </script>
<div id="mainpage" style="width:100%">
    <div style="width: 100%;">
        <table style="margin: 100px auto auto auto; border-left:solid 2px black; border-right: solid 2px black;">
            <tr>
                <td style="padding:10px 18px 7px 18px; font-size:14px"><b>Change Password</b></td>
            </tr>
            <tr>
                <td style="padding:7px 18px 7px 18px">Please enter your existing password and your new password, twice.</td>
            </tr>
            <tr>
                <td style="padding-left:12px">
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="cmdChange">
                        <table style="margin-left: 30px">
                            <tr>
                                <td align="right">Current Password <asp:TextBox runat="server" Width="150" TextMode="Password" ID="txtOldPassword"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right">New Password <asp:TextBox runat="server" Width="150" TextMode="Password" ID="txtNewPassword"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right">Confirmation <asp:TextBox runat="server" Width="150" TextMode="Password" ID="txtNewPasswordConfirm"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right"><asp:LinkButton runat="server" ID="cmdChange" CssClass="MyButton" Text="Change" Width="146" OnClientClick="return OnBeforeClick();"></asp:LinkButton></td>
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
    document.getElementById('ctl00_cphMain_txtOldPassword').focus();
</script>
</asp:Content>