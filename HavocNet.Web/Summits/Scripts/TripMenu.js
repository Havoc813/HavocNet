var selectedMenu;
var selectedSubMenu;
var user = 'Demo';
var currentLayer;
var version = '501';
var cursor = 5;
var menuTrips = [];

function InitTripMenu() {
    var centerControlDiv = document.createElement('div');
    centerControlDiv.style.width = '1238px';
    centerControlDiv.style["pointer-events"] = 'none';
    centerControlDiv.index = 1;

    var centerControl = new TripMenu(centerControlDiv);
    
    return centerControlDiv;
}

function TripMenu(controlDiv) {
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
    if (max > 5) max = 5;

    ul.appendChild(CreateMenuItem(0, "new", "New", "Create New Trip...", true, true));

    ul.appendChild(CreateArrow('Up', menuTrips.length));

    for (var i = 0; i < menuTrips.length; i++) {
        ul.appendChild(CreateMenuItem(i, menuTrips[i].id, menuTrips[i].name, menuTrips[i].dateRange(), false, i <= max));
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

    subUL.appendChild(CreateSubMenuItem("Details", true));
    subUL.appendChild(CreateSubMenuItem("Itinerary", false));
    subUL.appendChild(CreateSubMenuItem("Mountains", false));
    
    subCtrl.appendChild(subUL);

    cell.appendChild(subCtrl);

    row.appendChild(cell);

    tbl.appendChild(row);

    controlDiv.appendChild(tbl);
}

function CreateMenuItem(tripPosition, id, name, description, selected, visible) {
    var li = document.createElement('li');
    li.className = 'menuCell';
    if(!visible) li.style.display = "none";
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
    anchor.url = name + ".aspx";

    li.appendChild(anchor);

    li.id = "cmdMenu" + id;
    
    li.addEventListener('click', function () {
        SelectTrip();
    });

    return li;
}

function CreateSubMenuItem(name, selected) {
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
        SelectTripSubMenu();
    });

    return li;
}

function SelectTrip() {
    //Set Selected Trip
    
}

function SelectTripSubMenu() {
    //Set Selected Sub Menu
    
}

function CreateArrow(direction, max) {
    var li = document.createElement('li');
    li.className = 'menuCellArrow ' + direction;
    li.id = "cmdArrow" + direction;

    li.addEventListener('click', function () {
        MoveMenu(direction, max);
    });

    return li;
}

function MoveMenu(direction, max) {
    var showItem = cursor + 1;
    var hideItem = cursor - 5;
    
    if (direction == 'Up') {
        if (cursor - 5 < 1) return;

        showItem = cursor - 6;
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