function ValidateString(stringControlId, stringDescription) {
    //Not Empty
    if (document.getElementById(stringControlId).value == '') {
        alert(stringDescription + ' cannot be empty');
        document.getElementById(stringControlId).focus();
        return false;
    }

    return true;
}

function ValidateStringWithLength(stringControlId, stringDescription, length) {
    if (!ValidateString(stringControlId, stringDescription)) { return false; }
    if (!CheckLength(document.getElementById(stringControlId).value, stringControlId, stringDescription, length)) { return false; }

    return true;
}

function ValidateStringWithFixedLength(stringControlId, stringDescription, length) {
    if (!ValidateString(stringControlId, stringDescription)) { return false; }
    if (!CheckFixedLength(document.getElementById(stringControlId).value, stringControlId, stringDescription, length, true)) { return false; }

    return true;
}

function ValidateNumber(numberControlId, numberDescription) {
    //Check it is a number
    if (isNaN(document.getElementById(numberControlId).value)) {
        alert(numberDescription + ' is not a valid number');
        document.getElementById(numberControlId).focus();
        return false;
    }
    return true;
}

function CheckDate(dateControlId, dateToValidate, dateDescription) {
    //Check length == 10
    if (dateToValidate.length != 10) {
        alert(dateDescription + ' must be in the format dd/mm/yyyy');
        document.getElementById(dateControlId).focus();
        return false;
    }

    try {
        var d = Number(dateToValidate.toString().substring(0, 2));
        var m = Number(dateToValidate.toString().substring(3, 5));
        var y = Number(dateToValidate.toString().substring(6, 10));

        if (isNaN(d) || isNaN(m) || isNaN(y)) {
            alert(dateDescription + ' must be in the format dd/mm/yyyy.');
            document.getElementById(dateControlId).focus();
            return false;
        }

        //Check valid day
        if (d > 31 || (d > 30 && (m == 9 || m == 4 || m == 6 || m == 11)) || (d > 29 && m == 2) || (d > 28 && m == 2 && !isLeap(y))) {
            alert(dateDescription + ' has an invalid day value');
            document.getElementById(dateControlId).focus();
            return false;
        }

        //Check valid month
        if (m < 1 || m > 12) {
            alert(dateDescription + ' has an invalid month value.  Must be 1-12.');
            document.getElementById(dateControlId).focus();
            return false;
        }

        //Check valid year
        if (y < 1950 || y > 2200) {
            alert(dateDescription + ' has an invalid year value.  Must be 1950-2200');
            document.getElementById(dateControlId).focus();
            return false;
        }
        return true;
    } catch (err) {
        alert(dateDescription + ' must be in the format dd/mm/yyyy');
        document.getElementById(dateControlId).focus();
        return false;
    }
}

function CheckTime(timeControlId, timeToValidate, timeDescription) {
    if (timeToValidate.length != 8) {
        alert(timeDescription + ' must be in the format hh:mm:ss');
        document.getElementById(timeControlId).focus();
        return false;
    }

    try {
        var h = Number(timeToValidate.toString().substring(0, 2));
        var m = Number(timeToValidate.toString().substring(3, 5));
        var s = Number(timeToValidate.toString().substring(6, 8));

        if (isNaN(h) || isNaN(m) || isNaN(s)) {
            alert(timeDescription + ' must be in the format hh:mm:ss.');
            document.getElementById(timeControlId).focus();
            return false;
        }

        //Check valid h
        if (h < 0 || h > 23) {
            alert(timeDescription + ' has an invalid hour value.  Must be 0-23.');
            document.getElementById(timeControlId).focus();
            return false;
        }

        //Check valid m
        if (m < 0 || m > 59) {
            alert(timeDescription + ' has an invalid minute value.  Must be 0-59.');
            document.getElementById(timeControlId).focus();
            return false;
        }

        //Check valid s
        if (s < 0 || s > 59) {
            alert(timeDescription + ' has an invalid second value.  Must be 0-59.');
            document.getElementById(timeControlId).focus();
            return false;
        }
        return true;
    } catch (err) {
        alert(timeDescription + ' must be in the format hh:mm:ss.');
        document.getElementById(timeControlId).focus();
        return false;
    }
}

function ValidateDate(dateControlId, dateDescription) {
    var dateToValidate = document.getElementById(dateControlId).value;

    return CheckDate(dateControlId, dateToValidate, dateDescription);
}

function ValidateDateTime(dateControlId, dateDescription) {
    //Check length == 19
    if (document.getElementById(dateControlId).value.length != 19) {
        alert(dateDescription + ' date & time must be in the format dd/mm/yyyy hh:mm:ss');
        document.getElementById(dateControlId).focus();
        return false;
    }

    try {
        var dateToValidate = document.getElementById(dateControlId).value.substring(0, 10);
        var timeToValidate = document.getElementById(dateControlId).value.substring(11, 19);

        if (!CheckDate(dateControlId, dateToValidate, dateDescription + " date")) { return false; }

        if (!CheckTime(dateControlId, timeToValidate, dateDescription + " time")) { return false; }

        return true;
    } catch (err) {
        alert(dateDescription + ' date & time must be in the format dd/mm/yyyy hh:mm:ss');
        document.getElementById(dateControlId).focus();
        return false;
    }
}

function ValidateNoSpaces(stringControlId, stringDescription) {
    if (document.getElementById(stringControlId).value.indexOf(" ") > -1) {
        alert(stringDescription + ' cannot contain spaces.');
        document.getElementById(stringControlId).focus();
        return false;
    }
    return true;
}

function ValidateEmail(emailControlID, emailDescription) {
    //Check the @ symbol
    if (document.getElementById(emailControlID).value.indexOf("@") == -1) {
        alert(emailDescription + ' is badly formed.  There is no "@" character.');
        document.getElementById(emailControlID).focus();
        return false;
    }

    //Check the . character after the @ character
    if (document.getElementById(emailControlID).value.indexOf(".", document.getElementById(emailControlID).value.indexOf("@") + 1) == -1) {
        alert(emailDescription + ' is badly formed.  There is no "." character after the "@".');
        document.getElementById(emailControlID).focus();
        return false;
    }

    return true;
}

function isLeap(y) {
    if (y % 4 == 0) {
        if (y % 100 == 0 && y % 400 != 0) {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return false;
    }
}

function CheckLength(stringToValidate, stringControlID, stringDescription, len) {
    if (stringToValidate.length > Number(len)) {
        alert(stringDescription + ' must be less than or equal to ' + String(len) + ' characters in length.');
        document.getElementById(stringControlID).focus();
        return false;
    }
    return true;
}

function CheckFixedLength(stringToValidate, stringControlID, stringDescription, len) {
    if (stringToValidate.length == Number(len)) {
        alert(stringDescription + ' must be ' + String(len) + ' characters in length.');
        document.getElementById(stringControlID).focus();
        return false;
    }
    return true;
}

function CheckNoQuotes(stringToValidate, stringControlID, stringDescription) {
    if (stringToValidate.indexOf("\"") > -1) {
        alert(stringDescription + ' cannot contain quotation marks.');
        document.getElementById(stringControlID).focus();
        return false;
    }
    return true;
}

function CheckNoSquareBrackets(stringToValidate, stringControlID, stringDescription) {
    if (stringToValidate.indexOf("[") > -1 || stringToValidate.indexOf("]") > -1) {
        alert(stringDescription + ' cannot contain square brackets.');
        document.getElementById(stringControlID).focus();
        return false;
    }
    return true;
}

function CheckInvalidStartCharacters(stringToValidate, stringControlID, stringDescription) {
    var l = stringToValidate.substring(0, 1);
    if (l == "@" || l == "\\" || l == "{" || l == "}" || l == "." || l == "+" || l == "," || l == "'" || l == "-" || l == "-" || l == "_" || l == "=" || l == "|" || l == "<") {
        alert(stringDescription + ' cannot begin with the character ' + l + ' or any of the characters @\{}.+,\'-_=|<');
        document.getElementById(stringControlID).focus();
        return false;
    }
    return true;
}

function CheckNoLeadingSpaces(stringToValidate, stringControlID, stringDescription) {
    var l = stringToValidate.substring(0, 1);
    if (l == " ") {
        alert(stringDescription + ' cannot begin with a space');
        document.getElementById(stringControlID).focus();
        return false;
    }
    return true;
}

function CheckUniqueName(controlID, objectType, parentID) {
    var objectName = document.getElementById(controlID).value;
    var objectID;

    if (document.getElementById("ctl00_cphMain_hidSelectedRow") == null || document.getElementById("ctl00_cphMain_hidSelectedRow").value == "") 
        objectID = "0";    
    else
        objectID = document.getElementById("ctl00_cphMain_hidSelectedRow").value.replace("ctl00_cphMain_", "");

    window.ob_post.AddParam("objectID", objectID);
    window.ob_post.AddParam("objectName", objectName);
    window.ob_post.AddParam("parentID", parentID);

    var resp = window.ob_post.post(null, "Get" + objectType);

    if (resp != "0") {
        alert("Duplicate " + objectType + " not permitted");
        document.getElementById(controlID).focus();
        return false;
    }
    return true;
}

function ValidateComment(stringControlID, stringDescription) {
    var stringToValidate = document.getElementById(stringControlID).value;

    //Not Empty
    if (stringToValidate == '') {
        alert(stringDescription + ' cannot be empty');
        document.getElementById(stringControlID).focus();
        return false;
    }

    //Contains single quote
    if (stringToValidate.toString().indexOf("'") > -1) {
        alert(stringDescription + ' cannot contain single quotes.');
        document.getElementById(stringControlID).focus();
        return false;
    }

    //Contains double quote
    if (stringToValidate.toString().indexOf('"') > -1) {
        alert(stringDescription + ' cannot contain double quotes.');
        document.getElementById(stringControlID).focus();
        return false;
    }

    //Contains ampersands
    if (stringToValidate.toString().indexOf("&") > -1) {
        alert(stringDescription + ' cannot contain ampersands.');
        document.getElementById(stringControlID).focus();
        return false;
    }

    //Contains + signs
    if (stringToValidate.toString().indexOf("+") > -1) {
        alert(stringDescription + ' cannot contain plus signs.');
        document.getElementById(stringControlID).focus();
        return false;
    }

    return true;
}