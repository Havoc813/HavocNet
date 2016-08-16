function ValidateString(stringControlId, stringDescription) {
    return returnValidate(document.getElementById(stringControlId).value == '', stringControlId, stringDescription + ' cannot be empty');
}

function ValidateNumber(numberControlId, numberDescription) {
    return returnValidate(isNaN(document.getElementById(numberControlId).value), numberControlId, numberDescription + ' must be a valid number');
}

function ValidateTime(hoursControlID, minutesControlID, secondsControlID) {
    var hours = document.getElementById(hoursControlID);
    var minutes = document.getElementById(minutesControlID);
    var seconds = document.getElementById(secondsControlID);

    if (!ValidateNumber(hoursControlID, "Hours")) { return false; }
    if (!ValidateNumber(minutesControlID, "Minutes")) { return false; }
    if (!ValidateNumber(secondsControlID, "Seconds")) { return false; }

    if (!returnValidate(hours > 23 || hours < 0, hoursControlID, "Hours must be between 0 and 23")) { return false; }
    if (!returnValidate(minutes > 59 || minutes < 0, minutesControlID, "Minutes must be between 0 and 59")) { return false; }
    if (!returnValidate(seconds > 59 || seconds < 0, secondsControlID, "Seconds must be between 0 and 59")) { return false; }

    return true;
}

function ValidateDate(dateControlId, dateDescription) {
    var dateToValidate = document.getElementById(dateControlId).value;

    if(!returnValidate(dateToValidate.length != 10, dateControlId, dateDescription + ' must be in the format dd/mm/yyyy')) {return false;}
    
    var d = Number(dateToValidate.substring(0, 2));
    var m = Number(dateToValidate.substring(3, 5));
    var y = Number(dateToValidate.substring(6, 10));

    if(!returnValidate(d > 31 || (d > 30 && (m == 9 || m == 4 || m == 6 || m == 11)) || (d > 29 && m == 2) || (d > 28 && m == 2 && !isLeap(y)), 
        dateControlId, dateDescription + ' has an invalid day value')) {return false;}
    if(!returnValidate(m < 1 || m > 12, dateControlId, dateDescription + ' has an invalid month value.  Must be 1-12')) {return false;}
    if(!returnValidate(y < 1950 || y > 2200, dateControlId, dateDescription + ' has an invalid year value.  Must be 1950-2200')) {return false;}

    return true;
}

function ValidateEmail(emailToValidate, emailControlID, emailDescription) {
    if(!returnValidate(emailToValidate.indexOf("@") == -1, emailControlID, emailDescription + ' is badly formed.  There is no "@" character')) {return false;}
    if (!returnValidate(emailToValidate.indexOf(".", emailToValidate.indexOf("@") + 1) == -1, emailControlID, emailDescription + ' is badly formed.  There is no "." character after the "@"')) { return false; }
    
    return true;
}

function ValidatePassword(stringToValidate, stringControlID, stringDescription) {
    if (!returnValidate(CheckLength(stringToValidate, 8), stringControlID, stringDescription + ' must be >= 8 characters')) { return false; }

    var containsUpper = CheckCharacters(stringToValidate, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
    var containsLower = CheckCharacters(stringToValidate, "abcdefghijklmnopqrstuvwxyz");
    var containsNumber = CheckCharacters(stringToValidate, "1234567890");
    var containsSymbol = CheckCharacters(stringToValidate, "!£$%^&*()_[]{}:;'@#~<>,.?/|");

    if (!returnValidate(containsLower + containsNumber + containsSymbol + containsUpper < 3, stringControlID, "Upper, Lower, Numbers, Symbols; " + stringDescription + " must contain 3 of 4")) { return false; }

    return true;
}

function CheckLength(stringToValidate, length) {
    return stringToValidate.length < length;
}

function isLeap(y) {
    if (y % 4 == 0) {
        if (y % 100 == 0 && y % 400 != 0) {
            return false;
        }
        return true;
    }
    return false;
}

function returnValidate(failCondition, controlID, description) {
    if (failCondition) {
        alert(description);
        document.getElementById(controlID).focus();
        return false;
    }
    return true;
}

function CheckCharacters(stringToValidate, charsToValidate) {
    for (var i = 0; i < charsToValidate.length; i++) {
        if (stringToValidate.indexOf(charsToValidate[i]) > -1) return 1;
    }
    return 0;
}


function CheckNumberSizeMax(numberToValidate, numberControlID, numberDescription, size) {
    if (numberToValidate > Number(size)) {
        alert(numberDescription + ' must be less than ' + String(size) + '.');
        document.getElementById(numberControlID).focus();
        return false;
    }
    return true;
}

function CheckNumberSizeMin(numberToValidate, numberControlID, numberDescription, size) {
    if (numberToValidate < Number(size)) {
        alert(numberDescription + ' must be greater than ' + String(size) + '.');
        document.getElementById(numberControlID).focus();
        return false;
    }
    return true;
}

function CheckFirstCharacter(stringToValidate, stringControlID, stringDescription, firstCharacter) {
    if (stringToValidate.substring(0, 1) != firstCharacter) {
        alert(stringDescription + ' cannot begin with a "' + stringToValidate.substring(0, 1) + '".  Must be a "' + firstCharacter + '".');
        document.getElementById(stringControlID).focus();
        return false;
    }
    return true;
}