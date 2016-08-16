function AddParameter(param, value) {
    param = encodeURI(param);
    value = encodeURI(value);

    var kvp = document.location.search.substr(1).split('&');

    var i = kvp.length;
    var x;

    while (i--) {
        x = kvp[i].split('=');

        if (x[0] == param) {
            x[1] = value;
            kvp[i] = x.join('=');
            break;
        }
    }

    if (i < 0) { kvp[kvp.length] = [param, value].join('='); }

    if (kvp[0] == '')
        document.location.search = kvp[1];
    else
        document.location.search = kvp.join('&');
}

function GetParameter(param) {
    return decodeURIComponent((new RegExp('[?|&]' + param + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || null;
}