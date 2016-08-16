<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logout.aspx.cs" Inherits="HavocNet.Web.Logout" MasterPageFile="HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
<script type="text/javascript">
    window.setTimeout("window.location.href = '/Login.aspx'", 3000);
</script>
<div id="mainpage" style="width:100%">
    <div style="width: 100%;">
        You have successfully logged out.  Page will redirect to the Login screen.
    </div>
</div>
</asp:Content>