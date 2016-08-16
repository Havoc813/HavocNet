<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainMountains.aspx.cs" Inherits="HavocNet.Web.Summits.MainMountains" MasterPageFile="~/Summits/Summits.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <script type="text/javascript" src="Scripts/Trip.js"></script>
    <script type="text/javascript" src="Scripts/Map.js"></script>
    <script type="text/javascript" src="Scripts/MountainMenu.js"></script>
    <script type="text/javascript">
        var map;
        var user = 'havoc';

        window.onload = function InitMap() {
            var myOptions = {
                center: new google.maps.LatLng(25, 0),
                zoom: 3,
                mapTypeId: google.maps.MapTypeId.TERRAIN,
                mapTypeControlOptions: {
                    style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
                    position: google.maps.ControlPosition.TOP_RIGHT
                }
            };
            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

            map.controls[google.maps.ControlPosition.TOP_CENTER].push(InitMountainMenu());
        };
    </script> 
    <div id="map_canvas" style="height: 100%; overflow: hidden;"></div> 
    <script type="text/javascript"src="http://maps.googleapis.com/maps/api/js?key=AIzaSyC5kMNefmqbuopZrZ3XJuoEvSevMUhIjfw">
    </script> 
    <div id="data" runat="server" style="display:none"></div>
</asp:Content>