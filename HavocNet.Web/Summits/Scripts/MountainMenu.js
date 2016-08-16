var selectedMenu;
var selectedSubMenu;
var user = 'Demo';
var currentLayer;
var version = '501';
var cursor = 10;
var menuTrips = [];

function InitMountainMenu(map) {
    var centerControlDiv = document.createElement('div');
    centerControlDiv.style.width = '1238px';
    centerControlDiv.style["pointer-events"] = 'none';
    centerControlDiv.index = 1;

    var centerControl = new MountainMenu(centerControlDiv, map);

    return centerControlDiv;
}

function MountainMenu(controlDiv, map) {
    var tbl = document.createElement('table');
    tbl.style["pointer-events"] = 'auto';

    var row = document.createElement('tr');
    var cell = document.createElement('td');
    cell.style.backgroundColor = 'black';
    cell.style.padding = '0px';
    cell.style.width = '163px';

    var ctrl = document.createElement('div');
    ctrl.style.textAlign = 'left';
    ctrl.style.width = '163px';

    var ul = document.createElement('ul');
    ul.className = "menu";

    menuTrips = GetTrips();

    var max = menuTrips.length;
    if (max > 10) max = 10;

    ul.appendChild(CreateArrow('Up', menuTrips.length));

    for (var i = 0; i < menuTrips.length; i++) {
        ul.appendChild(CreateMenuItem(i, menuTrips[i].id, menuTrips[i].name, menuTrips[i].dateRange(), map, i == 0, i <= max));
    }

    ul.appendChild(CreateArrow('Down', menuTrips.length));

    ctrl.appendChild(ul);

    cell.appendChild(ctrl);

    var subCtrl = document.createElement('div');
    subCtrl.style.textAlign = 'left';
    subCtrl.style.width = "163px";

    var subUL = document.createElement('ul');
    subUL.className = "menu";
    subUL.id = "subMenu";

    for (var j = 0; j < menuTrips[0].mountains.length; j++) {
        subUL.appendChild(CreateSubMenuItem(menuTrips[0].mountains[j].name, map, j == 0, menuTrips[0].mountains[j]));
    }

    subCtrl.appendChild(subUL);

    cell.appendChild(subCtrl);

    row.appendChild(cell);

    tbl.appendChild(row);

    controlDiv.appendChild(tbl);
}

function CreateMenuItem(tripPosition, id, name, description, map, selected, visible) {
    var li = document.createElement('li');
    li.className = 'menuCell';
    if (!visible) li.style.display = "none";
    if (selected) {
        li.className += 'On';
        selectedMenu = id;
    }

    var anchor = document.createElement('a');
    anchor.style["cursor"] = "pointer";

    var desc = document.createElement('div');
    desc.innerHTML = description;
    desc.className = 'description';
    anchor.text = name;
    anchor.appendChild(desc);

    li.appendChild(anchor);

    li.id = "cmdMenu" + id;

    li.addEventListener('click', function () {
        ChangeTripMap(tripPosition, id, map);
    });

    return li;
}

function CreateSubMenuItem(name, map, selected, mountain) {
    var li = document.createElement('li');
    li.className = 'subMenuCell';
    if (selected) {
        li.className += 'On';
        selectedSubMenu = name;
    }

    var anchor = document.createElement('a');
    anchor.text = name;
    anchor.style["cursor"] = "pointer";
    li.appendChild(anchor);

    li.id = "cmdSubMenu" + name;

    li.addEventListener('click', function () {
        ChangeTripMapSub(name, map, mountain);
    });

    if (selected)
        ChangeTripMapSub(name, map, mountain);

    return li;
}

function CreateArrow(direction, max) {
    var li = document.createElement('li');
    li.className = 'menuCellArrow ' + direction;
    li.id = "cmdMenu" + name;

    li.addEventListener('click', function () {
        MoveMenu(direction, max);
    });

    return li;
}

function MoveMenu(direction, max) {
    var showItem = cursor + 1;
    var hideItem = cursor - 10;

    if (direction == 'Up') {
        if (cursor - 10 < 1) return;

        showItem = cursor - 11;
        hideItem = cursor;

        cursor--;
    } else {
        if (cursor + 1 >= max) return;
        cursor++;
    }

    document.getElementById("cmdMenu" + menuTrips[hideItem].id).style.display = 'none';

    document.getElementById("cmdMenu" + menuTrips[showItem].id).style.display = 'block';
}

function SetSelectedMenu(name) {
    if (document.getElementById("cmdMenu" + selectedMenu) != null)
        document.getElementById("cmdMenu" + selectedMenu).className = "menuCell";

    if (document.getElementById("cmdMenu" + name) != null)
        document.getElementById("cmdMenu" + name).className = "menuCellOn";

    selectedMenu = name;
}

function SetSelectedSubMenu(name) {
    if (document.getElementById("cmdSubMenu" + selectedSubMenu) != null)
        document.getElementById("cmdSubMenu" + selectedSubMenu).className = "subMenuCell";

    if (document.getElementById("cmdSubMenu" + selectedSubMenu) != null)
        document.getElementById("cmdSubMenu" + name).className = "subMenuCellOn";

    selectedSubMenu = name;
}


function ChangeTripMap(tripPosition, id, map, mountain) {
    SetSelectedMenu(id);

    //SetPositionMap(mountain.longitude, mountain.latitude, mountain.zoom);

    ClearSubMenus();

    AddSubMenus(tripPosition);
}

function ClearSubMenus() {
    var items = document.getElementById('subMenu').childNodes;

    while (items.length > 0) items[0].remove();
}

function AddSubMenus(tripPosition, map) {
    var items = document.getElementById('subMenu');

    for (var j = 0; j < menuTrips[tripPosition].mountains.length; j++) {
        items.appendChild(CreateSubMenuItem(menuTrips[tripPosition].mountains[j].name, map, j == 0, menuTrips[tripPosition].mountains[j]));
    }
}

function ChangeTripMapSub(name, map, mountain) {
    SetPositionMap(mountain.latitude, mountain.longitude, mountain.zoom);
    
    //if (currentLayer != null) currentLayer.setMap(null);

    SetSelectedSubMenu(name);

    //var layer = new google.maps.KmlLayer('http://www.jamesmeares.co.uk/SummitsKML/' + user + '/Mountains/' + selectedSubMenu + '_borders.kml?Ver=' + version,
    //{
    //    streetViewControl: false,
    //    suppressInfoWindows: false,
    //    preserveViewport: true
    //});

    //google.maps.event.addListener(layer, 'status_changed', function () {
    //    if (layer.getStatus() != 'OK') {
    //        alert('KML loading problem - ' + layer.getStatus());
    //    }
    //});

    //layer.setMap(map);

    //currentLayer = layer;
}