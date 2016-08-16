<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainTrips.aspx.cs" Inherits="HavocNet.Web.MainMountains" MasterPageFile="~/Summits/Summits.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="Scripts/Trip.js"></script>
    <script type="text/javascript" src="Scripts/TripMenu.js"></script>
    <script type="text/javascript">
        var user = 'havoc';

        window.onload = function Init() {
            document.getElementById("divHolder").appendChild(InitTripMenu());
        };
    </script>
    <div id="divHolder" class="content">
        
    </div>
</asp:Content>