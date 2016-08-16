$(function () {
    $("#ctl00_cphMain_txtDate").datepicker({ dateFormat: "dd/mm/yy", firstDay: 1 });
});

function SetRoute() {
    var cbo = document.getElementById('ctl00_cphMain_cboRoute');

    if (cbo.selectedIndex == 0) {
        document.getElementById('ctl00_cphMain_txtDistance').disabled = false;
        document.getElementById('ctl00_cphMain_cboLocation').disabled = false;
    } else {
        document.getElementById('ctl00_cphMain_txtDistance').disabled = true;
        document.getElementById('ctl00_cphMain_cboLocation').disabled = true;
    }
}

function OnSelectSport(sender) {
    var activityID = sender.options[sender.selectedIndex].value;

    var locations = document.getElementById('ctl00_cphMain_' + activityID + '_location');
    var locationDrop = document.getElementById('ctl00_cphMain_cboLocation');

    if (locationDrop != null) {
        while (locationDrop.options.length > 0) locationDrop.remove(0);

        if (locations != null) {
            var locationList = locations.value.split('|');

            for (var i = 0; i < locationList.length; i++) {
                var newOption = document.createElement('option');
                newOption.text = locationList[i].split(';')[1];
                newOption.value = locationList[i].split(';')[0];
                locationDrop.appendChild(newOption);
            }
        }
    }
    
    var routes = document.getElementById('ctl00_cphMain_' + activityID + '_route');
    var routeDrop = document.getElementById('ctl00_cphMain_cboRoute');

    if (routeDrop != null) {
        while (routeDrop.options.length > 0) routeDrop.remove(0);

        if (routes != null) {
            var routeList = routes.value.split('|');

            for (i = 0; i < routeList.length; i++) {
                var newRoute = document.createElement('option');
                newRoute.text = routeList[i].split(';')[1];
                newRoute.value = routeList[i].split(';')[0];
                routeDrop.appendChild(newRoute);
            }
        }
    }
    
    var kit = document.getElementById('ctl00_cphMain_' + activityID + '_kit');
    var kitDrop = document.getElementById('ctl00_cphMain_cboKit');

    if (kitDrop != null) {
        while (kitDrop.options.length > 0) kitDrop.remove(0);

        if (kit != null) {
            var kitList = kit.value.split('|');

            for (i = 0; i < kitList.length; i++) {
                var newKit = document.createElement('option');
                newKit.text = kitList[i].split(';')[1];
                newKit.value = kitList[i].split(';')[0];
                kitDrop.appendChild(newKit);
            }
        }
    }
}

function AddKit() {
    if (document.getElementById('ctl00_cphMain_txtNewKit').style.display == 'none' || document.getElementById('ctl00_cphMain_txtNewKit').style.display == '') {
        document.getElementById('ctl00_cphMain_txtNewKit').style.display = 'inline';
        document.getElementById('ctl00_cphMain_cboKit').disabled = true;

    } else {
        document.getElementById('ctl00_cphMain_txtNewKit').style.display = 'none';
        document.getElementById('ctl00_cphMain_cboKit').disabled = false;
    }
}

function AddLocation() {
    if (document.getElementById('ctl00_cphMain_txtNewLocation').style.display == 'none' || document.getElementById('ctl00_cphMain_txtNewLocation').style.display == '') {
        document.getElementById('ctl00_cphMain_txtNewLocation').style.display = 'inline';
        document.getElementById('ctl00_cphMain_cboLocation').disabled = true;
        
    } else {
        document.getElementById('ctl00_cphMain_txtNewLocation').style.display = 'none';
        document.getElementById('ctl00_cphMain_cboLocation').disabled = false;        
    }
}