<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuSummits.aspx.cs" Inherits="HavocNet.Web.MenuSummits" MasterPageFile="HavocNet.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="pageTitle">
        
    </div>
    <div style="margin-top: 10px">
        <asp:PlaceHolder runat="server" ID="phTable"></asp:PlaceHolder>
    </div>
    <asp:HiddenField runat="server" ID="hidSelectedRow" />
    <asp:HiddenField runat="server" ID="hidEditingRow" />
</asp:Content>