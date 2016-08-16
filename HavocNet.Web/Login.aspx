<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="HavocNet.Web.Login" MasterPageFile="HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
<div id="mainpage" style="width:100%">
    <div style="width: 100%;">
        <table style="margin: 100px auto auto auto; border-left:solid 2px black; border-right: solid 2px black;">
            <tr>
                <td style="padding:10px 18px 7px 18px; font-size:14px"><b>Login</b></td>
            </tr>
            <tr>
                <td style="padding:7px 18px 7px 18px">Currently viewing public access.  To view user specific content please login below.</td>
            </tr>
            <tr>
                <td style="padding-left:12px">
                    <asp:Panel runat="server" DefaultButton="cmdSignIn">
                        <table style="margin-left: 87px">
                            <tr>
                                <td align="right">Username <asp:TextBox runat="server" Width="150" ID="txtUsername"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right">Password <asp:TextBox runat="server" Width="150" ID="txtPassword" TextMode="Password"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right"><asp:LinkButton runat="server" ID="cmdSignIn" CssClass="MyButton" Text="Sign In" Width="146"></asp:LinkButton></td>
                            </tr>
                        </table>                    
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="padding:7px 18px 0px 18px">Forgotten your password? Click <a href="RetrievePassword.aspx" style="text-decoration: none; color:lightblue">here</a>.</td>
            </tr>
            <tr>
                <td style="padding:10px 18px 10px 18px"><asp:Label runat="server" ID="lblStatus" ForeColor="Red"></asp:Label></td>
            </tr>
        </table>
    </div>
</div>
<script type="text/javascript">
    document.getElementById('ctl00_cphMain_txtUsername').focus();
</script>
</asp:Content>