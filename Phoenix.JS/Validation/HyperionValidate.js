
function ValidateHyperionWord(wordControlID, wordDescription, appID, wordType, wordID, dimensionID) {
    var wordToValidate = document.getElementById(wordControlID).value;

    // Not Empty
    if (!ValidateString(wordControlID, wordDescription)) { return false; }

    // <80 Characters
    if (!CheckLength(wordToValidate, wordControlID, wordDescription, 80)) { return false; }

    // No Quotes
    if (!CheckNoQuotes(wordToValidate, wordControlID, wordDescription)) { return false; }

    // No Brackets
    if (!CheckNoSquareBrackets(wordToValidate, wordControlID, wordDescription)) { return false; }

    // Start Character not (@,\,{,},.,+,,,',-,_,=,|,<)
    if (!CheckInvalidStartCharacters(wordToValidate, wordControlID, wordDescription)) { return false; }

    // No leading spaces
    if (!CheckNoLeadingSpaces(wordToValidate, wordControlID, wordDescription)) { return false; }

    // Isn't used anywhere else
    if (!CheckMemberName(wordToValidate, wordControlID, wordDescription, appID, wordType, wordID, dimensionID)) { return false; }

    // No Reserved Words
    if (!CheckReservedWords(wordToValidate, wordControlID, wordDescription)) { return false; }

    return true;
}

function ValidateHyperionUDA(wordControlID, wordDescription, appID) {
    var wordToValidate = document.getElementById(wordControlID).value;

    // Not Empty
    if (!ValidateString(wordControlID, wordDescription)) { return false; }

    // <80 Characters
    if (!CheckLength(wordToValidate, wordControlID, wordDescription, 80)) { return false; }

    // No Quotes
    if (!CheckNoQuotes(wordToValidate, wordControlID, wordDescription)) { return false; }

    // No Brackets
    if (!CheckNoSquareBrackets(wordToValidate, wordControlID, wordDescription)) { return false; }

    // Start Character not (@,\,{,},.,+,,,',-,_,=,|,<)
    if (!CheckInvalidStartCharacters(wordToValidate, wordControlID, wordDescription)) { return false; }

    // No leading spaces
    if (!CheckNoLeadingSpaces(wordToValidate, wordControlID, wordDescription)) { return false; }

    // Not an existing UDA
    if (!CheckUDAName(wordToValidate, wordControlID, wordDescription, appID)) { return false; }

    // No Reserved Words
    if (!CheckReservedWords(wordToValidate, wordControlID, wordDescription)) { return false; }

    return true;
}

function ValidateProperty(propertyControlID, propertyDescription, accType, hasFormula) {
    var propertyToValidate = document.getElementById(propertyControlID).value;

    // Not Blank
    if (ValidateString(propertyControlID, propertyDescription) == false) { return false; }

    // First character must be the aggregator
    var c = propertyToValidate.toString().charAt(0);
    if (c != '%' && c != '*' && c != '-' && c != '+' && c != '~' && c != '/') {
        alert(propertyDescription + ' start character is invalid. Must be one of %*+-/~');
        document.getElementById(propertyControlID).focus();
        return false;
    }

    // Cant contain invalid characters (Depends on whether its an accounts dim)
    var i;
    for (i = 1; i < propertyToValidate.length; i++) {
        c = propertyToValidate.toString().charAt(i);
        if (c != 'N' && c != 'O' && c != 'T' && c != 'V' && c != 'X' && c != 'S') {
            if (accType == 'Accounts') {
                if (c != 'A' && c != 'B' && c != 'E' && c != 'F' && c != 'L' && c != 'M' && c != 'Z') {
                    alert(propertyDescription + ' contains invalid characters. Must contain only ABEFLMNOTVXZS');
                    document.getElementById(propertyControlID).focus();
                    return false;
                }
            }
            else {
                alert(propertyDescription + ' contains invalid characters. Must contain only NOTVXS');
                document.getElementById(propertyControlID).focus();
                return false;
            }
        }
    }

    // Cant contain incorrect cominations of characters
    // One of XOSNV
    var d = 0;
    if (propertyToValidate.toString().indexOf('X') > -1) { d += 1; }
    if (propertyToValidate.toString().indexOf('S') > -1) { d += 1; }
    if (propertyToValidate.toString().indexOf('O') > -1) { d += 1; }
    if (propertyToValidate.toString().indexOf('N') > -1) { d += 1; }
    if (propertyToValidate.toString().indexOf('V') > -1) { d += 1; }

    if (d > 1) {
        alert(propertyDescription + ' cannot contain more than one storage type');
        document.getElementById(propertyControlID).focus();
        return false;
    }

    // One of FLA   
    d = 0;
    if (propertyToValidate.toString().indexOf('F') > -1) { d += 1; }
    if (propertyToValidate.toString().indexOf('L') > -1) { d += 1; }
    if (propertyToValidate.toString().indexOf('A') > -1) { d += 1; }

    if (d > 1) {
        alert(propertyDescription + ' cannot contain more than one time balance setting');
        document.getElementById(propertyControlID).focus();
        return false;
    }

    // One of ZMB with one of FLA
    var e = 0;

    if (propertyToValidate.toString().indexOf('Z') > -1) { e += 1; }
    if (propertyToValidate.toString().indexOf('M') > -1) { e += 1; }
    if (propertyToValidate.toString().indexOf('B') > -1) { e += 1; }

    if (e > 1) {
        alert(propertyDescription + ' cannot contain more than one skip option');
        document.getElementById(propertyControlID).focus();
        return false;
    }

    if (d == 0 && e == 1) {
        alert(propertyDescription + ' cannot contain a skip option without a time balance setting');
        document.getElementById(propertyControlID).focus();
        return false;
    }

    // If S then only 2 chars
    if (propertyToValidate.toString().indexOf("S") > -1 && propertyToValidate.toString().length > 2) {
        alert('Member is shared and can therefore only contain an aggregator in addition to the S flag');
        document.getElementById(propertyControlID).focus();
        return false;
    }

    // Bottom Level Dynamic Calc members should have formulae associated with them
    if (!parent.ob_hasChildren(parent.document.getElementById(parent.tree_selected_id)) && propertyToValidate.toString().indexOf('X') > -1 && hasFormula == false) {
        alert('Bottom level dynamic calc members should have a formula associated with them');
        document.getElementById(propertyControlID).focus();
        return false;
    }
    return true;
}

function ValidateFormula(formulaControlID, formulaDescription, appID) {
    var formulaToValidate = document.getElementById(formulaControlID).value;

    //If length > 2000, prevent saving
    if (!CheckLength(formulaToValidate, formulaControlID, formulaDescription, 2000)) {
        return 'Formula length exceeds the database maximum.';
    }

    //Contains an "="
    if (formulaToValidate.toString().indexOf('=') == -1) {
        return 'Formula must contain an "="';
    }

    //Check there are an even number of brackets
    var bracketCount = 0;
    for (var i = 0; i < formulaToValidate.toString().length; i++) {
        if (formulaToValidate.toString().substring(i, i + 1) == '(') {
            bracketCount += 1;
        }
        else if (formulaToValidate.toString().substring(i, i + 1) == ')') {
            bracketCount -= 1;
        }
    }
    if (bracketCount > 0) {
        return 'Opening Bracket with no Closing Bracket.';
    }
    if (bracketCount < 0) {
        return 'Closing Bracket with no Opening Bracket.';
    }

    //Member names have "'s round them
    //Check that there are an even number of "'s
    var quoteCount = 0;
    for (i = 0; i < formulaToValidate.toString().length; i++) {
        if (formulaToValidate.toString().substring(i, i + 1) == '"') {
            quoteCount += 1;
        }
    }
    if (quoteCount % 2 != 0) {
        return 'Opening Quotation Mark with no Closing Quotation Mark.';
    }

    //Add the member names to an array
    var members = '';
    var functions = '';

    var j = 0;
    var start = 0;

    while (j < formulaToValidate.toString().length) {
        if (formulaToValidate.toString().substring(j, j + 1) == '"' && start == 0) {
            start = j;
        }
        else if (formulaToValidate.toString().substring(j, j + 1) == '"') {
            members += formulaToValidate.toString().substring(start + 1, j) + "|";
            start = 0;
        }

        if (formulaToValidate.toString().substring(j, j + 1) == '@' && start == 0) {
            start = j;
        }
        else if (formulaToValidate.toString().substring(j, j + 1) == '(' && start > 0) {
            functions += formulaToValidate.toString().substring(start, j) + "|";
            start = 0;
        }
        j++;
    }

    var arr = members.split("|");

    for (i = 0; i < arr.length - 1; i++) {
        if (CheckMemberNameNoAlert(arr[i], appID, "Member", "0")) {
            return 'Member Name ' + arr[i] + ' Invalid';
        }
    }

    arr = functions.split("|");

    for (i = 0; i < arr.length - 1; i++) {
        if (!CheckFunctionNameNoAlert(arr[i])) {
            return 'Function Name ' + arr[i] + ' Invalid';
        }
    }

    //semi colons at the end of statements


    return 'Valid';
}


function CheckReservedWords(stringToValidate, stringControlID, stringDescription) {
    window.ob_post.AddParam("stringToValidate", stringToValidate);

    var sData = window.ob_post.post("Validate.aspx", "CheckReservedWords");

    if (sData != "0") {
        alert(stringDescription + ' cannot be a reserved word.');
        document.getElementById(stringControlID).focus();
        return false;
    }        

    return true;
}

function CheckMemberName(memberToValidate, memberNameControlID, memberDescription, appID, memberType, memberID) {
    window.ob_post.AddParam("appID", appID);
    window.ob_post.AddParam("mbrName", memberToValidate);
    window.ob_post.AddParam("objectType", memberType);
    window.ob_post.AddParam("objectID", memberID);

    var resp = window.ob_post.post("Validate.aspx", "GetExistingMembers");

    if (resp != "0") {
        if (resp.toString().indexOf('dimension member') > -1) {
            if (!confirm(memberDescription + ' conflicts with a member name in a different dimension.  \n\nViolation occurred in a ' + resp + '. \n\nContinue to save?')) {
                document.getElementById(memberNameControlID).focus();
                return false;
            }
        }
        else {
            alert(memberDescription + ' cannot be an existing Member Name, Dimension Name, Alias, SmartList or SmartList Member.\n\nViolation occurred in a ' + resp + '.');
            document.getElementById(memberNameControlID).focus();
            return false;
        }
    }
    return true;
}

function CheckMemberNameNoAlert(memberToValidate, appID, memberType, memberID) {
    window.ob_post.AddParam("appID", appID);
    window.ob_post.AddParam("mbrName", memberToValidate);
    window.ob_post.AddParam("objectType", memberType);
    window.ob_post.AddParam("objectID", memberID);

    var resp = window.ob_post.post("Validate.aspx", "GetExistingMembers");
    
    return (resp == "0");
}

function CheckFunctionNameNoAlert(functionToValidate) {
    window.ob_post.AddParam("functionName", functionToValidate);

    var resp = window.ob_post.post("Validate.aspx", "GetExistingFunction");
    
    return (resp == "0");
}

function CheckUDAName(aUDAToValidate, aUDAControlID, aUDADescription, appID) {
    window.ob_post.AddParam("appID", appID);
    window.ob_post.AddParam("udaName", aUDAToValidate);

    var resp = window.ob_post.post("Validate.aspx", "GetExistingUDAs");

    if (resp != "0") {
        alert(aUDADescription + ' cannot be an existing UDA Name.');
        document.getElementById(aUDAControlID).focus();
        return false;
    }
    return true;
}

function CheckSmartListMemberID(smartListMemberID, smartListID, appID, smartListMemberControlID, oldSmartListMemberID) {
    window.ob_post.AddParam("appID", appID);
    window.ob_post.AddParam("smartListID", smartListID);
    window.ob_post.AddParam("smartListMemberID", smartListMemberID);
    window.ob_post.AddParam("oldSmartListMemberID", oldSmartListMemberID);

    var resp = window.ob_post.post("Validate.aspx", "GetSmartListMemberID");

    if (resp != "0") {
        alert('SmartList Member cannot have the same ID as another member.');
        document.getElementById(smartListMemberControlID).focus();
        return false;
    }

    return true;
}