<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NoAccess.aspx.cs" Inherits="HavocNet.Web.Errors.NoAccess" MasterPageFile="~/HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="mainpage" style="margin:15px 0 60px 0">
        <div style="font-size:18px; font-weight:bold; color:Red; font-family: Arial; margin-bottom:10px">
            Application Error
        </div>
        <div style="font-size:14px; font-weight:bold; font-family: Arial; margin-bottom:20px">
            Description: Access Denied (HTTP 401)
        </div>
        <div>
            There has been an error with the HavocNet application.  The user does not have access to the page requested.  <br />
            To request access to the page please email the <a href="mailto:webmaster@wreakhavoc.co.uk?subject=HavocNet Error">Webmaster</a>.<br/><br/>
            Please select a tab from the menu above to continue.
        </div>
    </div>
</asp:Content>