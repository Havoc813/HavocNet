function SuDokuKeys(width, height) {
    this.sudokuWidth = width;
    this.sudokuHeight = height;

    this.OnKeyPress = OnKeyPress;
    this.OnKeyPressStub = OnKeyPressStub;
}

function OnKeyPress(sender) {
    return OnKeyPressStub(sender,this.sudokuWidth, this.sudokuHeight);
}

function OnKeyPressStub(sender, width, height) {
    var keyPressed = window.event.keyCode;
    var col = sender.id.toString().substring(17, 19);
    var row = sender.id.toString().substring(19, 21);

    switch (keyPressed) {
        case 37: //Left Arrow Key
            OnKeyLeft(sender, row, col);
            break;
        case 38: //Up Arrow Key
            OnKeyUp(sender, row, col);
            break;
        case 39: //Right Arrow Key
            OnKeyRight(sender, row, col, width);
            break;
        case 40: //Down Arrow Key
            OnKeyDown(sender, row, col, height);
            break;
        case 8:  //Backspace
        case 9:  //Tab
        case 16: //Shift
        case 49:
        case 50:
        case 51:
        case 52:
        case 53:
        case 54:
        case 55:
        case 56:
        case 57: //Numerics
            break;
        case 65:
        case 66:
        case 67:
        case 68:
        case 69:
        case 70:
        case 71:
        case 72:
        case 73:
        case 74:
        case 75:
        case 76:
        case 77:
        case 78:
        case 79:
        case 80:
        case 81:
        case 82:
        case 83:
        case 84:
        case 85:
        case 86:
        case 87:
        case 88:
        case 89:
        case 90: //Alphas
            return (keyPressed - height < 56);
        default:
            return false;
    }
    return true;
}

function OnKeyUp(sender, row, col)
{
    if (Number(row) != 0) {
        document.getElementById('ctl00_cphMain_txt' + col + Right('0' + String(Number(row) - 1), 2)).focus();
    }
}

function OnKeyDown(sender, row, col, height)
{
    if (Number(row) != height - 1) {
        document.getElementById('ctl00_cphMain_txt' + col + Right('0' + String(Number(row) + 1), 2)).focus();
    }
}

function OnKeyLeft(sender, row, col)
{
    if (Number(col) != 0) {
        document.getElementById('ctl00_cphMain_txt' + Right('0' + String(Number(col) - 1), 2) + row).focus();
    }
}

function OnKeyRight(sender, row, col, width)
{
    if (Number(col) != width - 1) {
        document.getElementById('ctl00_cphMain_txt' + Right('0' + String(Number(col) + 1), 2) + row).focus();
    }
}

function Right(str, strLen)
{
    return str.substring(str.length - strLen);
}