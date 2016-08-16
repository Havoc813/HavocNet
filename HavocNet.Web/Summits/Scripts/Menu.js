var selectedMenu = 'World';
var selectedSubMenu = 'Highpointed';
var user = 'Demo';
var pageType = 'Countries';

function InitMenu(map, type) {
    var centerControlDiv = document.createElement('div');
    centerControlDiv.style.width = '1238px';
    centerControlDiv.style["pointer-events"] = 'none';
    centerControlDiv.index = 1;

    pageType = type;
    
    var centerControl;
    if (type == 'Countries') {
        centerControl = new CountryMenu(centerControlDiv, map);
    } else {
        centerControl = new CountyMenu(centerControlDiv, map);
    }

    return centerControlDiv;
}

function CountryMenu(controlDiv, map) {
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
    
    ul.appendChild(CreateMenuItem('World', 'All the countries of the world', map, true));
    ul.appendChild(CreateMenuItem('Europe', 'Climb all of Europe', map, false));
    ul.appendChild(CreateMenuItem('Africa', 'Climb all of Africa', map, false));
    ul.appendChild(CreateMenuItem('Asia', 'Climb all of Asia', map, false));
    ul.appendChild(CreateMenuItem('North America', 'Climb all of North America', map, false));
    ul.appendChild(CreateMenuItem('South America', 'Climb all of South America', map, false));
    ul.appendChild(CreateMenuItem('Australasia', 'Climb all of Australasia', map, false));
    ul.appendChild(CreateMenuItem('Antarctica', 'Climb all of Antarctica', map, false));    

    ctrl.appendChild(ul);

    cell.appendChild(ctrl);
    
    var subCtrl = document.createElement('div');
    subCtrl.style.textAlign = 'left';
    subCtrl.style.width = "163px";

    var subUL = document.createElement('ul');
    subUL.className = "menu";
    subUL.id = "subMenu";

    subUL.appendChild(CreateSubMenuItem('Visited', map, false));
    subUL.appendChild(CreateSubMenuItem('Highpointed', map, true));
    
    subCtrl.appendChild(subUL);

    cell.appendChild(subCtrl);

    var statsDiv = document.createElement('div');
    statsDiv.className = "statsCell";

    var piechart = document.createElement('div');
    piechart.id = "piechart";

    statsDiv.appendChild(piechart);

    cell.appendChild(statsDiv);
    
    row.appendChild(cell);
    
    tbl.appendChild(row);

    controlDiv.appendChild(tbl);
}

function CountyMenu(controlDiv, map) {
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

    ul.appendChild(CreateMenuItem('England', 'English County Highpoints', map, true));
    ul.appendChild(CreateMenuItem('Scotland', 'Scottish County Highpoints', map, false));
    ul.appendChild(CreateMenuItem('Wales', 'Wales County Highpoints', map, false));
    ul.appendChild(CreateMenuItem('Northern Ireland', 'N.I. County Highpoints', map, false));
    ul.appendChild(CreateMenuItem('Ireland', 'Ireland County Highpoints', map, false));
    ul.appendChild(CreateMenuItem('USA', 'U.S. State Highpoints', map, false));
    ul.appendChild(CreateMenuItem('Canada', 'Canadian State Highpoints', map, false));

    ctrl.appendChild(ul);

    cell.appendChild(ctrl);

    var subCtrl = document.createElement('div');
    subCtrl.style.textAlign = 'left';
    subCtrl.style.width = "163px";

    var subUL = document.createElement('ul');
    subUL.className = "menu";
    subUL.id = "subMenu";

    subUL.appendChild(CreateSubMenuItem('Visited', map, false));
    subUL.appendChild(CreateSubMenuItem('Highpointed', map, true));

    subCtrl.appendChild(subUL);

    cell.appendChild(subCtrl);

    var statsDiv = document.createElement('div');
    statsDiv.className = "statsCell";

    var piechart = document.createElement('div');
    piechart.id = "piechart";

    statsDiv.appendChild(piechart);

    cell.appendChild(statsDiv);
    
    row.appendChild(cell);

    tbl.appendChild(row);

    controlDiv.appendChild(tbl);
}

function CreateMenuItem(name, description, map, selected) {
    var li = document.createElement('li');
    li.className = 'menuCell';
    if (selected) {
        li.className += 'On';
        selectedMenu = name;
    }

    var anchor = document.createElement('a');
    
    var desc = document.createElement('div');
    desc.innerHTML = description;
    desc.className = 'description';
    anchor.text = name;
    anchor.appendChild(desc);

    li.appendChild(anchor);

    li.id = "cmdMenu" + name;

    if (pageType == 'Countries') {
        li.addEventListener('click', function () {
            ChangeMap(name, map);
        });
    } else {
        li.addEventListener('click', function () {
            ChangeMapCounty(name, map);
        });
    }
    
    return li;
}

function CreateSubMenuItem(name, map, selected) {
    var li = document.createElement('li');
    li.className = 'subMenuCell';
    if (selected) {
        li.className += 'On';
        selectedSubMenu = name;
    }
    
    var anchor = document.createElement('a');
    anchor.text = name;
    li.appendChild(anchor);
    
    li.id = "cmdSubMenu" + name;

    li.addEventListener('click', function () {
        ChangeMapSubMenu(name, map);
    });

    if (selected)
        SetMapSubMenu(name, map);

    return li;
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