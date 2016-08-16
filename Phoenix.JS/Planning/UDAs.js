var pre = "ctl00_cphMain_";

function selectUDATo(sender) {
    if (sender.className == "UDATableCell") {
        sender.className = "UDATableCellOn";
        document.getElementById(pre + 'hidUDATo').value += sender.innerHTML + '|';
    }
    else {
        sender.className = "UDATableCell";
        document.getElementById(pre + 'hidUDATo').value = document.getElementById(pre + 'hidUDATo').value.replace(sender.innerHTML + '|', '');
    }
}

function selectUDAFrom(sender) {
    if (sender.className == "UDATableCell") {
        sender.className = "UDATableCellOn";
        document.getElementById(pre + 'hidUDAFrom').value += sender.innerHTML + '|';
    }
    else {
        sender.className = "UDATableCell";
        document.getElementById(pre + 'hidUDAFrom').value = document.getElementById(pre + 'hidUDAFrom').value.replace(sender.innerHTML + '|', '');
    }
}

function moveTo(needsUpdate) {
    var udas = document.getElementById(pre + 'hidUDATo').value.split('|');
    var udaSelected = document.getElementById(pre + 'tblSelectedUDAs');
    var i;
    for (i = 0; i < udas.length - 1; i++) {
        if (udaSelected.innerHTML.indexOf(udas[i]) == -1) {
            var newRow = udaSelected.insertRow(udaSelected.rows.length);

            var newCell = newRow.insertCell(0);
            newCell.className = "UDATableCell";
            newCell.innerHTML = udas[i];
            newCell.onclick = function () { selectUDAFrom(this); };
        }
    }

    document.getElementById(pre + 'hidUDATo').value = "";

    var allUDAs = document.getElementById(pre + 'tblAllUDAs');

    for (i = 0; i < allUDAs.rows.length; i++) {
        allUDAs.rows[i].cells[0].className = "UDATableCell";
    }

    if (udaSelected.rows[0].cells[0].innerHTML == "None" && udaSelected.rows.length > 1) {
        udaSelected.deleteRow(0);
    }

    if (needsUpdate) {
        //Change color of update button
        if (document.getElementById(pre + 'cmdUpdateUDAs') != null) {
            document.getElementById(pre + 'cmdUpdateUDAs').style.backgroundColor = 'navy';
            document.getElementById(pre + 'cmdUpdateUDAs').style.color = 'white';
        }
    }

    return false;
}

function moveFrom() {
    var udas = document.getElementById(pre + 'hidUDAFrom').value;
    var udaSelected = document.getElementById(pre + 'tblSelectedUDAs');

    for (var i = udaSelected.rows.length - 1; i >= 0 ; i--) {
        if (udas.indexOf(udaSelected.rows[i].cells[0].innerHTML) > -1) udaSelected.deleteRow(i);
    }

    document.getElementById(pre + 'hidUDAFrom').value = "";

    if (udaSelected.rows.length == 0) {
        var newRow = udaSelected.insertRow(0);

        var newCell = newRow.insertCell(0);
        newCell.className = "UDATableCell";
        newCell.innerHTML = "None";
    }

    if (document.getElementById(pre + 'cmdUpdateUDAs') != null) {
        //Change color of update button
        document.getElementById(pre + 'cmdUpdateUDAs').style.backgroundColor = 'navy';
        document.getElementById(pre + 'cmdUpdateUDAs').style.color = 'white';
    }
    return false;
}

function clearUDAs() {
    var udaSelected = document.getElementById(pre + 'tblSelectedUDAs');

    for (var i = udaSelected.rows.length - 1; i >= 0 ; i--) {
        udaSelected.deleteRow(i);
    }
    document.getElementById(pre + 'hidUDAFrom').value = "";

    var newRow = udaSelected.insertRow(0);

    var newCell = newRow.insertCell(0);
    newCell.className = "UDATableCell";
    newCell.innerHTML = "None";
}

function OnBeforeUpdateUDAs() {
    if (confirm("Commit UDA Changes?")) {
        try {
            updateUDAs();

            document.getElementById(pre + 'lblUDAStatus').innerHTML = "<span style='color:green'>UDAs Updated Successfully</span>";
        } catch (e) {
            document.getElementById(pre + 'lblUDAStatus').innerHTML = "<span style='color:red'>An error occurred updating the UDAs</span>";
        }
    }
    return false;
}