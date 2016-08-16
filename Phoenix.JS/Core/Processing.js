function ShowProcessing() {
    window.setTimeout('ShowProcessingDelay();', 50);
}

function HideButton(ctrlName) {
    document.getElementById(ctrlName).style.display = 'none';
}

function ShowProcessingDelay() {
    document.getElementById('divProcessing').style.display = 'block';
    document.getElementById('imgProcessing').src = document.getElementById('imgProcessing').src;
}

function HideProcessing() {
    document.getElementById('divProcessing').style.display = 'none';
    document.getElementById('imgProcessing').src = document.getElementById('imgProcessing').src;
}

function ShowButton(ctrlName) {
    document.getElementById(ctrlName).style.display = 'inline';
}