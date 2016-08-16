
function ValidateOrigMatPer(stringControlId, stringDescription) {
    var stringToValidate = document.getElementById(stringControlId).value;

    // If empty, skip validation
    if (stringToValidate == '') { return true; }

    // Length = 4
    if (!CheckLength(stringToValidate, stringControlId, stringDescription, 4)) { return false; }

    // First Character = Y,M or D
    if (stringToValidate.toString().charAt(0) != 'Y' && stringToValidate.toString().charAt(0) != 'M' && stringToValidate.toString().charAt(0) != 'D') {
        alert('The first character of ' + stringDescription + ' must be Y,M or D');
        document.getElementById(stringControlId).focus();
        return false;
    }

    // Last 3 Characters all digits
    var test = Number(stringToValidate.toString().substring(1));
    if (isNaN(test)) {
        alert('The last three characters of ' + stringDescription + ' must be the digits 0-9');
        document.getElementById(stringControlId).focus();
        return false;
    }

    return true;
}
