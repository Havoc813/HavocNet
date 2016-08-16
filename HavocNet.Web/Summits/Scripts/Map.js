var currentLayer;
var version = '501';

function ChangeMap(name, map) {
    SetSelectedMenu(name);

    SetPosition(map);

    DrawChart();
}

function DrawChart() {
    var data = google.visualization.arrayToDataTable(GetStatsByName(selectedMenu, selectedSubMenu, GetStats()));

    var options = {
        legend: { position: 'none' },
        chartArea: { left: 0, top: 10, width: 163, height: 140 },
        height: 160,
        colors: ['green', 'orange', 'whitesmoke'],
        slices: [{}, {}, { textStyle: { color: 'black' } }],
        tooltip: { isHtml: false, trigger: 'selection' }
    };

    chart.draw(data, options);
}

function ChangeMapCounty(name, map) {
    SetSelectedMenu(name);

    SetPosition(map);

    ChangeMapSubMenu(selectedSubMenu, map);
}

function ChangeMapSubMenu(name, map) {
    SetMapSubMenu(name, map);

    DrawChart();
}

function SetMapSubMenu(name, map) {
    if (currentLayer != null) currentLayer.setMap(null);

    SetSelectedSubMenu(name);

    var layerName = pageType;
    if (pageType == 'Counties') layerName = selectedMenu.replace(" ","");

    var layer = new google.maps.KmlLayer('http://havocnet.wreakhavoc.co.uk/Summits/KML/' + user + '/' + layerName + '_' + selectedSubMenu + '_borders.kml?v=' + version,
    {
        streetViewControl: false,
        suppressInfoWindows: false,
        preserveViewport: true
    });

    google.maps.event.addListener(layer, 'status_changed', function () {
        if (layer.getStatus() != 'OK') {
            alert('KML loading problem - ' + layer.getStatus());
        }
    });

    layer.setMap(map);

    currentLayer = layer;
}

function ClearSubMenus() {
    var items = document.getElementById('subMenu').childNodes;

    while (items.length > 0) items[0].remove();
}

function AddSubMenus(name, map) {
    var items = document.getElementById('subMenu');
    
    items.appendChild(CreateSubMenuItem('Visited', map, false));
    items.appendChild(CreateSubMenuItem('Highpointed', map, true));
}

function SetPosition(map) {
    var Long, Lat, Zoom;

    switch(selectedMenu) {
        case "Europe":
            Long = 9.18457;
            Lat = 50.401515;
            Zoom = 4;
            break;
        case 'Asia':
            Long = 85.18457;
            Lat = 25.401515;
            Zoom = 4;
            break;
        case 'Africa':
            Long = 19.18457;
            Lat = -5.401515;
            Zoom = 4;
            break;
        case 'North America':
            Long = -110;
            Lat = 40.401515;
            Zoom = 4;
            break;
        case 'South America':
            Long = -60;
            Lat = -28;
            Zoom = 4;
            break;
        case 'Australasia':
            Long = 135;
            Lat = -28;
            Zoom = 4;
            break;
        case 'Antarctica':
            Long = 10;
            Lat = -75;
            Zoom = 3;
            break;
        case "England":
            Long = -1.42822;
            Lat = 52.84259;
            Zoom = 6;
            break;
        case "Scotland":
            Long = -4.39453;
            Lat = 56.873;
            Zoom = 6;
            break;
        case "Wales":
            Long = -3.82324;
            Lat = 52.38231;
            Zoom = 7;
            break;
        case "Northern Ireland":
            Long = -6.56982;
            Lat = 54.60707;
            Zoom = 8;
            break;
        case "Ireland":
            Long = -8.130081555;
            Lat = 53.17828495;
            Zoom = 7;
            break;
        case "USA":
            Long = -112.4487648;
            Lat = 45.68286948;
            Zoom = 3;
            break;
        case "Canada":
            Long = -98.33708832;
            Lat = 61.31668142;
            Zoom = 3;
            break;
        default:
            Long = 0;
            Lat = 25;
            Zoom = 3;
            break;
    }

    SetPositionMap(Lat, Long, Zoom);
}

function SetPositionMap(Lat, Long, Zoom) {
    map.panTo(new google.maps.LatLng(Lat, Long));
    map.setZoom(Zoom);
}
