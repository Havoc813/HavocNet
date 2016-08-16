//[
//        new Trip(0, "Bologna", "18/08/2016", "23/08/2016", [new Mountain(163, "Monte Titano", 12.452222, 43.928333, 13)]),
//        new Trip(1, "Austria Take 2", "18/08/2015", "23/08/2015", [new Mountain(44, "Großglockner", 12.69525, 47.074867, 13)]),
//        new Trip(2, "Balkans", "18/08/2015", "23/08/2015", [
//            new Mountain(79, "Mount Korab", 20.546667, 41.790278, 13),
//            new Mountain(93, "Zla Kolata", 19.897222, 42.485, 13),
//            new Mountain(122, "Dinara", 16.383403, 44.0626, 13),
//            new Mountain(100, "Maglic", 18.733468, 43.28108, 13),
//            new Mountain(84, "Djeravica", 20.140833, 42.532778, 13)
//        ]),
//        new Trip(3, "Andorra", "18/08/2015", "23/08/2015", [new Mountain(70, "Coma Pedrosa", 1.443708, 42.591725, 13)]),
//        new Trip(4, "Austria", "18/08/2015", "23/08/2015", [
//            new Mountain(619, "Studlhutte", 12.681182, 47.054666, 13),
//            new Mountain(618, "Hinter Grauspitz", 9.587417, 47.054803, 13)
//        ]),
//        new Trip(5, "Romania Take 2", "18/08/2015", "23/08/2015", [new Mountain(92, "Moldoveanu", 24.736061, 45.59976, 13)]),
//        new Trip(6, "Greece", "18/08/2015", "23/08/2015", [new Mountain(73, "Mount Olympus", 22.358611, 40.085556, 13)]),
//        new Trip(7, "Russia", "18/08/2015", "23/08/2015", [new Mountain(19, "Mount Elbrus", 42.437622, 43.352597, 13)]),
//        new Trip(8, "Romania", "18/08/2015", "23/08/2015", [new Mountain(617, "Vistea Mare", 24.735975, 45.602899, 13)]),
//        new Trip(9, "Scandinavia", "18/08/2015", "23/08/2015", [
//            new Mountain(96, "Galdhøpiggen", 8.3125, 61.636389, 13),
//            new Mountain(111, "Kebnekaise", 18.517899, 67.900906, 13),
//            new Mountain(136, "Halti", 21.272222, 69.307778, 13)
//        ]),
//        new Trip(10, "Portugal", "18/08/2015", "23/08/2015", [new Mountain(101, "Torre", -7.612967, 40.321867, 13)]),
//        new Trip(11, "Brussels", "18/08/2015", "23/08/2015", [
//            new Mountain(178, "Vaalserberg", 6.020833, 50.754722, 13),
//            new Mountain(168, "Kneiff", 6.037, 50.157317, 13),
//            new Mountain(166, "Signal De Botrange", 6.092778, 50.501667, 13)
//        ]),
//        new Trip(12, "Alps", "18/08/2015", "23/08/2015", [
//            new Mountain(273, "Allalinhorn", 7.894806, 46.046139, 13),
//            new Mountain(336, "Weissmies", 8.011944, 46.127778, 13),
//            new Mountain(29, "Mont Blanc", 6.865, 45.833611, 13)
//        ]),
//        new Trip(13, "Eastern Europe", "18/08/2015", "23/08/2015", [
//            new Mountain(127, "Snežka", 15.740278, 50.736111, 13),
//            new Mountain(95, "Rysy", 20.088056, 49.179444, 13),
//            new Mountain(85, "Gerlachovský štít", 20.135617111145, 49.164135265215, 13),
//            new Mountain(44, "Kékes", 20.009462608459, 47.872498708988, 13)
//        ])
//];

function GetTrips() {
    var trips = JSON.parse(document.getElementById("ctl00_cphMain_data").innerHTML);
    var newTrips = [];
    
    for (var i = 0; i < trips.length; i++) {
        var mountains = [];

        for (var j = 0; j < trips[i].Mountains.length; j++) {
            mountains[j] = new Mountain(
                trips[i].Mountains[j].ID,
                trips[i].Mountains[j].Name,
                trips[i].Mountains[j].Longitude,
                trips[i].Mountains[j].Latitude,
                trips[i].Mountains[j].Zoom
                );
        }   

        newTrips[i] = new Trip(
            trips[i].ID,
            trips[i].Name,
            trips[i].StartDate,
            trips[i].EndDate,
            mountains
            );
    }
    
    return newTrips;
}

function Trip(id, name, startDate, endDate, mountains) {
    this.id = id;
    this.name = name;
    this.startDate = startDate;
    this.endDate = endDate;
    this.mountains = mountains;

    this.dateRange = function () {
        return this.startDate + ' - ' + this.endDate;
    };
}

function Mountain(id, name, longitude, latitude, zoom) {
    this.id = id;
    this.name = name;
    this.longitude = longitude;
    this.latitude = latitude;
    this.zoom = zoom;
    
    //Path
    
    //Summit photo
    
    //Photo Link
    
    //Wiki Link
    
    //Summit Post Link
    
    //Climb Date
}

