//var stats = [
//    new Stat("World", 226, 43, 47, 35, 37),
//    new Stat("Europe", 56, 34, 37, 31, 32),
//    new Stat("North America", 33, 1, 1, 0, 0),
//    new Stat("South America", 15, 0, 0, 0, 0),
//    new Stat("Asia", 42, 0, 0, 0, 0),
//    new Stat("Australasia", 20, 0, 0, 0, 0),
//    new Stat("Africa", 59, 8, 9, 4, 5),
//    new Stat("Antarctica", 1, 0, 0, 0, 0),
//    new Stat("England", 39, 20, 22, 11, 13),
//    new Stat("Scotland", 33, 0, 0, 0, 0),
//    new Stat("Wales", 13, 0, 0, 0, 0),
//    new Stat("Northern Ireland", 6, 0, 0, 0, 0),
//    new Stat("Ireland", 32, 0, 0, 0, 0),
//    new Stat("USA", 50, 0, 0, 0, 0),
//    new Stat("Canada", 10, 0, 0, 0, 0)
//];

function GetStats() {
    var stats = JSON.parse(document.getElementById("ctl00_cphMain_data").innerHTML);
    var total = 0, visited = 0, steppedIn = 0, highpointed = 0, climbed = 0;

    for (var i = 0; i < stats.length - 1; i++) {
        stats[i] = new Stat(stats[i].name, stats[i].total, stats[i].visited, stats[i].steppedIn, stats[i].highpointed, stats[i].climbed);
        total += stats[i].total,
        visited += stats[i].visited,
        steppedIn += stats[i].steppedIn,
        highpointed += stats[i].highpointed,
        climbed += stats[i].climbed;
    }
    stats[stats.length - 1] = new Stat("World", total, visited, steppedIn, highpointed, climbed);

    return stats;
}

function Stat(name, total, visited, steppedIn, highpointed, climbed) {
    this.name = name;
    this.total = total;
    this.visited = visited;
    this.steppedIn = steppedIn;
    this.highpointed = highpointed;
    this.climbed = climbed;

    this.MakeDataSet = function (type) {
        if (type == "Highpointed") {
            return [
                ['Completed', 'Number'],
                ['Highpointed', this.highpointed],
                ['Climbed', this.climbed - this.highpointed],
                ['Unclimbed', this.total - this.climbed]
            ];
        } else {
            return [
                ['Visited', 'Number'],
                ['Visited', this.visited],
                ['Stepped In', this.steppedIn - this.visited],
                ['Not Visited', this.total - this.visited]
            ];
        }
    };
}

function GetStatsByName(name, type, stats) {
    for (var i = 0; i < stats.length; i++) {
        if (stats[i].name == name) return stats[i].MakeDataSet(type);
    }
    return stats[0].MakeDataSet(type);
}