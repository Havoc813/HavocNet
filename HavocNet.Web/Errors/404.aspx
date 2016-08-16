<%@ Page Language="C#" AutoEventWireup="true" CodeFile="404.aspx.cs" Inherits="HavocNet.Web.Errors.FourOhFour" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="mainpage" style="margin:15px 0 60px 0">
        <div style="font-size:18px; font-weight:bold; color:Red; font-family: Arial; margin-bottom:10px">
            Application Error
        </div>
        <div style="font-size:14px; font-weight:bold; font-family: Arial; margin-bottom:20px">
            Description: Page Not Found (HTTP 404)
        </div>
        <div>
            There has been an error with the FSM application.  This error has been logged and the administrators have been notified.  <br />
            It looks as though the page you tried to access does not exist.  If you followed a system link please escalate this issue to the <a href="mailto:webmaster@wreakhavoc.co.uk?subject=FSM Error">webmaster</a>.<br/><br/>
            Please select a tab from the menu above to continue.
        </div>
    </div>
</asp:Content>