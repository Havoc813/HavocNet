<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Fail.aspx.cs" Inherits="HavocNet.Web.Errors.Fail" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="mainpage" style="margin:15px 0 60px 0">
        <div style="font-size:18px; font-weight:bold; color:Red; font-family: Arial; margin-bottom:10px">
            Application Error
        </div>
        <div style="font-size:14px; font-weight:bold; font-family: Arial; margin-bottom:20px">
            Description: Internal Server Error
        </div>
        <div>
            There has been an error with the HavocNet application.  This error has been logged and the administrators have been notified.  <br />
            If you wish to escalate this error please contact the <a href="mailto:webmaster@wreakhavoc.co.uk?subject=FSM Error">webmaster</a>.<br /><br />
            Please select a tab from the menu above to continue.
        </div>
    </div>
</asp:Content>