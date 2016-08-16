function TableManage(type) {
    this.TableType = type;

    this.OnBeforeClick = OnBeforeClick;
    this.OnRowClick = OnRowClick;
    this.OnMouseOut = OnMouseOut;
    this.OnMouseOver = OnMouseOver;
    this.OnBeforeDelete = OnBeforeDelete;
    this.OnBeforeUpdate = OnBeforeUpdate;
    this.OnBeforeSave = OnBeforeSave;

    this.ColourOn = '#F0F8FF'; //AliceBlue
    this.ColourOff = '#FFFFFF'; //White
}

function OnRowClick(sender) {
    var selectedRowHid = document.getElementById("ctl00_cphMain_hidSelectedRow");

    if (sender.style.backgroundColor == this.ColourOn) {
        sender.style.backgroundColor = this.ColourOff;
        selectedRowHid.value = "";
    }
    else {
        if (selectedRowHid.value != "") {
            var selectedRow = document.getElementById(document.getElementById("ctl00_cphMain_hidSelectedRow").value);
            if (selectedRow != null) { selectedRow.style.backgroundColor = this.ColourOff; }
        }

        sender.style.backgroundColor = this.ColourOn;
        selectedRowHid.value = sender.id;
    }
}

function OnMouseOver(sender) {
    if (sender.id != document.getElementById("ctl00_cphMain_hidSelectedRow").value) {
        sender.style.backgroundColor = this.ColourOn;
    }
}

function OnMouseOut(sender) {
    if (sender.id != document.getElementById("ctl00_cphMain_hidSelectedRow").value && 
        sender.id != document.getElementById("ctl00_cphMain_hidEditingRow").value)
    {
        sender.style.backgroundColor = this.ColourOff;
    }
}

function OnBeforeDelete() {
    if (document.getElementById("ctl00_cphMain_hidSelectedRow").value != "") {
        return confirm("Delete this " + this.TableType + "?");
    }
    else {
        alert("No " + this.TableType + " Selected.  Please select a " + this.TableType + " to delete.");
        return false;
    }
}

function OnBeforeClick(action) {
    if (document.getElementById("ctl00_cphMain_hidSelectedRow").value == "") {
        alert("No " + this.TableType + " Selected.  Please select a " + this.TableType + " to " + action + ".");
        return false;
    }
    else {
        if (action == 'edit') {
            document.getElementById("ctl00_cphMain_hidEditingRow").value = document.getElementById("ctl00_cphMain_hidSelectedRow").value;
        } else {
            document.getElementById("ctl00_cphMain_hidEditingRow").value = "";
        }
        return true;
    }
}

function OnBeforeUpdate() {
    return true;
}

function OnBeforeSave() {
    return true;
}