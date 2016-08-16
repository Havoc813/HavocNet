/* These functions are called from the ob_events_2037.js file when a tree function is invoked */
var pre = 'ctl00_cphMain_';

function selectMember(id) {
    window.memberID = id;

    //Fetch member properties
    window.ob_post.AddParam("appID", window.appID);
    window.ob_post.AddParam("dimID", window.dimID);
    window.ob_post.AddParam("memberID", memberID);

    var propertyString = window.ob_post.post(null, 'GetProperties');

    if (propertyString != "") {
        var properties = propertyString.split('|');

        document.getElementById(pre + 'txtMemberName').value = properties[0];
        document.getElementById(pre + 'txtMemberAlias').value = properties[1];
        document.getElementById(pre + 'txtMemberProperty').value = properties[2];
        document.getElementById(pre + 'txtMemberLevel').value = properties[3];

        document.getElementById(pre + 'chkFormula').checked = (properties[4] != "");

        document.getElementById(pre + 'txtFormula').value = properties[4];

        document.getElementById(pre + 'chkUDAs').checked = (properties[5] != "");

        document.getElementById(pre + 'chk1').checked = (properties[6] == "True");
        if (properties[6] == "True") {
            document.getElementById(pre + 'cbo1').value = properties[7];
            document.getElementById(pre + 'cbo1').disabled = document.getElementById(pre + 'txtMemberName').disabled;
        } else {
            document.getElementById(pre + 'cbo1').value = "+";
            document.getElementById(pre + 'cbo1').disabled = true;
        }

        document.getElementById(pre + 'chk2').checked = (properties[8] == "True");
        if (properties[8] == "True") {
            document.getElementById(pre + 'cbo2').value = properties[9];
            document.getElementById(pre + 'cbo2').disabled = document.getElementById(pre + 'txtMemberName').disabled;
        } else {
            document.getElementById(pre + 'cbo2').value = "+";
            document.getElementById(pre + 'cbo2').disabled = true;
        }

        document.getElementById(pre + 'chk3').checked = (properties[10] == "True");
        if (properties[10] == "True") {
            document.getElementById(pre + 'cbo3').value = properties[11];
            document.getElementById(pre + 'cbo3').disabled = document.getElementById(pre + 'txtMemberName').disabled;
        } else {
            document.getElementById(pre + 'cbo3').value = "+";
            document.getElementById(pre + 'cbo3').disabled = true;
        }

        try {
            document.getElementById(pre + 'cboSourcePlanType').value = properties[12];
        } catch (e) { } 
        try {
            document.getElementById(pre + 'cboAccountType').value = properties[13];
        } catch (e) { }
        try {
            document.getElementById(pre + 'cboDataType').value = properties[14];
        } catch (e) { }
        try {
            document.getElementById(pre + 'cboCurrency').value = properties[15];
        } catch (e) { }
        try {
            document.getElementById(pre + 'txtBaseCurrency').value = properties[16];
        } catch (e) { }
        try {
            document.getElementById(pre + 'cboSmartList').value = properties[17];
        } catch (e) { }

        clearUDAs();

        document.getElementById(pre + 'hidUDATo').value = properties[5].split(',').join('|');

        moveTo(false);

        document.getElementById(pre + 'lblPropertyStatus').innerHTML = '';
        document.getElementById(pre + 'lblFormulaStatus').innerHTML = '';
        document.getElementById(pre + 'lblUDAStatus').innerHTML = '';
    }
}

function deselectMember() {
    window.memberID = '';

    document.getElementById(pre + 'txtMemberName').value = "";
    document.getElementById(pre + 'txtMemberAlias').value = "";
    document.getElementById(pre + 'txtMemberProperty').value = "";
    document.getElementById(pre + 'txtMemberLevel').value = "";
    document.getElementById(pre + 'chkFormula').checked = false;
    document.getElementById(pre + 'txtFormula').value = "";
    document.getElementById(pre + 'chkUDAs').checked = false;
    document.getElementById(pre + 'chk1').checked = true;
    document.getElementById(pre + 'cbo1').value = "+";
    document.getElementById(pre + 'cbo1').disabled = false;
    document.getElementById(pre + 'chk2').checked = true;
    document.getElementById(pre + 'cbo2').value = "+";
    document.getElementById(pre + 'cbo2').disabled = false;
    document.getElementById(pre + 'chk3').checked = true;
    document.getElementById(pre + 'cbo3').value = "+";
    document.getElementById(pre + 'cbo3').disabled = false;

    try {
        document.getElementById(pre + 'cboSourcePlanType').value = "";
    } catch (e) { }
    try {
        document.getElementById(pre + 'cboAccountType').value = "";
    } catch (e) { }
    try {
        document.getElementById(pre + 'cboDataType').value = "";
    } catch (e) { }
    try {
        document.getElementById(pre + 'cboCurrency').value = "";
    } catch (e) { }
    try {
        document.getElementById(pre + 'txtBaseCurrency').value = "";
    } catch (e) { }
    try {
        document.getElementById(pre + 'cboSmartList').value = "";
    } catch (e) { }

    clearUDAs();

    document.getElementById(pre + 'lblPropertyStatus').innerHTML = '';
    document.getElementById(pre + 'lblFormulaStatus').innerHTML = '';
    document.getElementById(pre + 'lblUDAStatus').innerHTML = '';
}

function addMember(parentID) {
    if (tree_selected_id != null) {
        var memberID = document.getElementById(pre + 'hidNewMemberID').value;
        var memberName = 'NewMember';
        var memberAlias = 'New Member';
        var i = 2;

        while (!CheckMemberNameNoAlert(memberName, window.appID, 'Member', -1)) {
            memberName = 'NewMember_' + i;
            memberAlias = 'New Member (' + i + ')';
            i++;
        }

        if (document.getElementById(pre + 'rdoDisplay_1').checked) {
            ob_t2_Add(parentID, memberID, memberAlias, memberName, memberAlias, false);
        } else {
            ob_t2_Add(parentID, memberID, memberName, memberName, memberAlias, false);
        }

        document.getElementById(memberID).innerHTML += '<span style="color:maroon"> [+N]</span>';

        changeParentNumber(memberID, 1);

        document.getElementById(pre + 'hidNewMemberID').value = String(parseInt(memberID) + 1);

        document.getElementById(memberID).click();

    } else {
        alert('No node has been selected.  Select a member to add a sibling to.');
    }
}

function addMemberSibling() {
    addMember(ob_getParentOfNode(document.getElementById(tree_selected_id)).id);
}

function addMemberChild() {
    addMember(tree_selected_id);
}

function deleteMember() {
    if (tree_selected_id != null && tree_selected_id != 'root_tree1') {
        var msg = 'Are you sure you wish to delete this member';
        if (ob_hasChildren(document.getElementById(tree_selected_id))) msg += ' (and all its children)';

        if (confirm(msg + '?')) {
            changeParentNumber(tree_selected_id, -1);

            ob_t2_Remove(tree_selected_id, true);
        }
    }
    else if (tree_selected_id == 'root_tree1') {
        alert('Cannot delete the top level dimension member!');
    }
    else {
        alert('No node has been selected!');
    }
}

function moveUp() {
    return ob_t2_UpDown('up', tree_selected_id);
}

function moveDown() {
    return ob_t2_UpDown('down', tree_selected_id);
}

function drop(src, dst) {
    changeParentNumber(src, -1);

    changeMemberNumber(dst, 1);
}

function updateProperties() {
    var ctrls = document.getElementsByTagName('input');
    var aliases = '';

    for (var i = 0; i < ctrls.length; i++) {
        if (ctrls[i].type == 'text') {
            if (ctrls[i].id.substring(0, 22) == pre + 'txtAlias') {
                aliases += ctrls[i].value + "|";
            }
        }
    }

    window.ob_post.AddParam("appID", window.appID);
    window.ob_post.AddParam("dimID", window.dimID);
    window.ob_post.AddParam("mbrID", window.memberID);
    window.ob_post.AddParam("memberName", document.getElementById(pre + "txtMemberName").value);
    window.ob_post.AddParam("memberAlias", document.getElementById(pre + "txtMemberAlias").value);
    window.ob_post.AddParam("moreAliases", aliases);

    if (document.getElementById(pre + "txtMemberProperty") != null)
        window.ob_post.AddParam("memberProperty", document.getElementById(pre + "txtMemberProperty").value);
    else {
        window.ob_post.AddParam("memberProperty", "");
    }
    if (document.getElementById(pre + "cboSourcePlanType") != null) {
        window.ob_post.AddParam("sourcePlanType", document.getElementById(pre + "cboSourcePlanType").value);
    } else {
        window.ob_post.AddParam("sourcePlanType", "");
    }
    if (document.getElementById(pre + "cboAccountType") != null) {
        window.ob_post.AddParam("accountType", document.getElementById(pre + "cboAccountType").value);
    } else {
        window.ob_post.AddParam("accountType", "");
    }
    if (document.getElementById(pre + "cboDataType") != null) {
        window.ob_post.AddParam("dataType", document.getElementById(pre + "cboDataType").value);
    } else {
        window.ob_post.AddParam("dataType", "");
    }
    if (document.getElementById(pre + "cboCurrency") != null) {
        window.ob_post.AddParam("exchangeRate", document.getElementById(pre + "cboCurrency").value);
    } else {
        window.ob_post.AddParam("exchangeRate", "");
    }
    if (document.getElementById(pre + "cboSmartList") != null) {
        window.ob_post.AddParam("smartList", document.getElementById(pre + "cboSmartList").value);
    } else {
        window.ob_post.AddParam("smartList", "");
    }
    if (document.getElementById(pre + "txtBaseCurrency") != null) {
        window.ob_post.AddParam("baseCurrency", document.getElementById(pre + "txtBaseCurrency").value);
    } else {
        window.ob_post.AddParam("baseCurrency", "");
    }
    
    window.ob_post.AddParam("validForPlan1", document.getElementById(pre + "chk1").checked);
    window.ob_post.AddParam("validForPlan2", document.getElementById(pre + "chk2").checked);
    window.ob_post.AddParam("validForPlan3", document.getElementById(pre + "chk3").checked);
    window.ob_post.AddParam("plan1Aggregator", document.getElementById(pre + "cbo1").value);
    window.ob_post.AddParam("plan2Aggregator", document.getElementById(pre + "cbo2").value);
    window.ob_post.AddParam("plan3Aggregator", document.getElementById(pre + "cbo3").value);

    window.ob_post.post(null, 'UpdateMember');

    //Update the tree
    SetMemberDescription();

    return false;
}

function updateFormula() {
    window.ob_post.AddParam("appID", window.appID);
    window.ob_post.AddParam("dimID", window.dimID);
    window.ob_post.AddParam("mbrID", window.memberID);
    window.ob_post.AddParam("memberFormula", document.getElementById(pre + 'txtFormula').value);

    window.ob_post.post(null, "UpdateFormula");

    //Update the tree
    SetMemberDescription();

    return false;
}

function updateUDAs() {
    var UDAs = '';
    var UDASelected = document.getElementById(pre + 'tblSelectedUDAs');

    for (var i = UDASelected.rows.length - 1; i >= 0 ; i--) {
        if (UDASelected.rows[i].cells[0].innerHTML != "None") {
            UDAs += ',' + UDASelected.rows[i].cells[0].innerHTML;
        }
    }

    window.ob_post.AddParam("appID", window.appID);
    window.ob_post.AddParam("dimID", window.dimID);
    window.ob_post.AddParam("mbrID", window.memberID);
    window.ob_post.AddParam("memberUDA", UDAs.substring(1));

    window.ob_post.post(null, "UpdateUDAs");

    //Update the tree
    SetMemberDescription();

    return false;
}

function search() {
    var txt = document.getElementById(pre + 'txtSearch').value;
    var again;

    if (txt == null) {
        return false;
    }
    var oNode;

    if (tree_selected_id == null) {
        oNode = ob_getFirstNodeOfTree();
        again = 0;
    }
    else {
        oNode = ob_getNodeDown(document.getElementById(tree_selected_id));
        again = 1;
    }

    while (oNode != null && oNode.innerHTML.toString().indexOf(txt) == -1) {
        oNode = ob_getNodeDown(oNode, "all");
    }

    if (oNode != null) {
        // select the node:
        ob_t26(oNode.id);

        // scroll to the node:
        document.getElementById(oNode.id).scrollIntoView(true);
    }
    else {
        if (again == 1) {
            oNode = ob_getFirstNodeOfTree();

            while (oNode != null && oNode.innerHTML.toString().indexOf(txt) == -1) {
                oNode = ob_getNodeDown(oNode, "all");
            }

            if (oNode != null) {
                // select the node:
                ob_t26(oNode.id);

                // scroll to the node:
                document.getElementById(oNode.id).scrollIntoView(true);
            }
        }
    }
    return false;
}

function changeParentNumber(memberID, incDec) {
    var parentID = ob_getParentOfNode(document.getElementById(memberID)).id;

    changeMemberNumber(parentID, incDec);
}

function changeMemberNumber(memberID, incDec) {
    if (memberID != 'root_tree1') {
        var par = document.getElementById(memberID).innerHTML;

        if (Number(par.substring(par.indexOf('&lt;') + 4, par.indexOf('&gt;'))) + incDec != 0) {
            if (par.indexOf('&lt;') >= 0) {
                document.getElementById(memberID).innerHTML = par.substring(0, par.indexOf('&lt;')) + "<span style='color:navy'><" + String(Number(par.substring(par.indexOf('&lt;') + 4, par.indexOf('&gt;'))) + incDec) + "></span>";
            }
            else {
                document.getElementById(memberID).innerHTML = par + "<span style='color:navy'><1></span>";
            }            
        } else {
            document.getElementById(memberID).innerHTML = par.substring(0, par.indexOf('&lt;'));
        }
    }
}

function SetMemberDescription() {
    var desc;

    alert(document.getElementById(pre + 'rdoDisplay').value);

    if (document.getElementById(pre + 'rdoDisplay').value == '0') {
        desc = document.getElementById(pre + "txtMemberName").value + " <span style='color:maroon'>[" + document.getElementById(pre + "txtMemberProperty").value + "]</span>";
    } else {
        desc = document.getElementById(pre + "txtMemberAlias").value + " <span style='color:maroon'>[" + document.getElementById(pre + "txtMemberProperty").value + "]</span>";
    }

    if (document.getElementById(pre + 'tblSelectedUDAs').rows.length >= 1 && document.getElementById(pre + 'tblSelectedUDAs').rows[0].cells[0].innerHTML != "None") {
        desc += "<span style='COLOR:green'> [UDA]</span>";
    }
    if (document.getElementById(pre + "txtFormula").value != "") {
        desc += "<span style='COLOR:green'> [Formula]</span>";
    }
    if (ob_hasChildren(document.getElementById(tree_selected_id))) {
        desc += "<span style='color:navy'><" + ob_getChildCount(document.getElementById(tree_selected_id)) + "></span>";
    }
    document.getElementById(memberID).innerHTML = desc;
}