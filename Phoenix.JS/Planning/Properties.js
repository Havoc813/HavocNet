function ChangePlanValidity(id) {
    var cbo = document.getElementById('ctl00_cphMain_cbo' + id);
    var chk = document.getElementById('ctl00_cphMain_chk' + id);
    var listItem;

    cbo.disabled = !chk.checked;

    var cboSource = document.getElementById('ctl00_cphMain_txtSourcePlanType');

    if (cboSource != null) {
        while (cboSource.length > 0) { cboSource.remove(0); }

        if (document.getElementById('ctl00_cphMain_chk1').checked) {
            listItem = document.createElement('option');
            listItem.value = "Plan1";
            listItem.text = "Plan1";
            cboSource.options.add(listItem);
        }
        if (document.getElementById('ctl00_cphMain_chk2').checked) {
            listItem = document.createElement('option');
            listItem.value = "Plan2";
            listItem.text = "Plan2";
            cboSource.options.add(listItem);
        }
        if (document.getElementById('ctl00_cphMain_chk3').checked) {
            listItem = document.createElement('option');
            listItem.value = "Plan3";
            listItem.text = "Plan3";
            cboSource.options.add(listItem);
        }
    }
}

function ChangePlanAggregator(id) {
    var cbo = document.getElementById('ctl00_cphMain_cbo' + id);

    if (id == 1 || (id == 2 && !document.getElementById('ctl00_cphMain_chk1').checked) || (id == 3 && !document.getElementById('ctl00_cphMain_chk1').checked && !document.getElementById('ctl00_cphMain_chk2').checked)) {
        document.getElementById('ctl00_cphMain_txtMemberProperty').value = cbo.value + document.getElementById('ctl00_cphMain_txtMemberProperty').value.substring(1);
    }
}

function ChangeAccountType() {
    var txt = document.getElementById('ctl00_cphMain_txtAccountType');

    if (txt.value == 'Expense') {
        document.getElementById('ctl00_cphMain_txtMemberProperty').value = document.getElementById('ctl00_cphMain_txtMemberProperty').value.toString().replace("E", "") + "E";
    }
    else {
        document.getElementById('ctl00_cphMain_txtMemberProperty').value = document.getElementById('ctl00_cphMain_txtMemberProperty').value.toString().replace("E", "");
    }
}

function BuildProperty() {
    var prpty = document.getElementById('ctl00_cphMain_txtMemberProperty').value;
    prpty = prpty.replace("~", "tilde");
    prpty = prpty.replace("+", "plus");
    prpty = prpty.replace("/", "slash");
    prpty = prpty.replace("*", "times");
    prpty = prpty.replace("%", "perc");
    prpty = prpty.replace("-", "minus");

    window.open("BuildProperty.aspx?Prpty=" + prpty, "BuildAProperty", "status=0,toolbar=0,location=0,menubar=0,directories=0,resizable=0,scrollbars=0,height=240,width=350,top=100,left=100");

    return false;
}

function OnBeforeUpdateProperties() {
    try {
        if (!ValidateProperty(pre + 'txtMemberProperty', 'Member Property', document.getElementById(pre + 'hidAccType').value, document.getElementById(pre + 'chkFormula').checked)) { return false; }

        if (document.getElementById(pre + 'txtMemberProperty').value.toString().indexOf('S') > -1) {
            window.ob_post.AddParam("appID", window.appID);
            window.ob_post.AddParam("dimID", window.dimID);
            window.ob_post.AddParam("mbrID", window.memberID);
            window.ob_post.AddParam('mbrName', document.getElementById(pre + 'txtMemberName').value);
            var a = window.ob_post.post(null, 'GetBaseMember');

            if (a == "0") {
                alert('Member cannot be shared without a corresponding base member in the same dimension');
                document.getElementById(pre + 'txtMemberProperty').focus();
                return false;
            }

            // Shared members cannot have children
            if (ob_hasChildren(document.getElementById(tree_selected_id))) {
                alert('Shared Member cannot have children.');
                document.getElementById(pre + 'txtMemberProperty').focus();
                return false;
            }
        }
        else {
            //Validate name
            if (!ValidateHyperionWord(pre + 'txtMemberName', 'Member Name', window.appID, 'Member', tree_selected_id)) { return false; }

            //Validate alias
            if (!ValidateHyperionWord(pre + 'txtMemberAlias', 'Member Alias', window.appID, 'Member', tree_selected_id)) { return false; }

            //Validate that Expenses have E in the property
            if (document.getElementById(pre + 'txtAccountType') != null) {
                if (document.getElementById(pre + 'txtAccountType').value == "Expense" && document.getElementById(pre + 'txtMemberProperty').value.toString().indexOf('E') == -1) {
                    alert('Expense Account Types must have "E" in the property.');
                    document.getElementById(pre + "txtMemberProperty").focus();

                    return false;
                }
                if (document.getElementById(pre + 'txtAccountType').value != "Expense" && document.getElementById(pre + 'txtMemberProperty').value.toString().indexOf('E') != -1) {
                    alert('Members with "E" in the property must have an Account Type of Expense.');
                    document.getElementById(pre + "txtAccountType").focus();

                    return false;
                }
            }

            //For each textbox, if id like 'txtAlias', validate like a member name
            var ctrls = document.getElementsByTagName('input');

            for (var i = 0; i < ctrls.length; i++) {
                if (ctrls[i].type == 'text') {
                    if (ctrls[i].id.substring(0, 22) == pre + 'txtAlias') {
                        if (!ValidateHyperionWord(ctrls[i].id, 'Member Alias', window.appID, 'Member', tree_selected_id)) { return false; }
                    }
                }
            }
        }
    } catch (e) {
        alert(e.message);

        document.getElementById('ctl00_cphMain_lblPropertyStatus').innerHTML = "<span style='color:red'>An error occurred during validation of the Properties</span>";
    }

    if (confirm("Commit Property Changes?")) {
        try {
            updateProperties();

            document.getElementById('ctl00_cphMain_lblPropertyStatus').innerHTML = "<span style='color:green'>Properties Updated Successfully</span>";
        } catch (e) {
            alert(e.message);

            document.getElementById('ctl00_cphMain_lblPropertyStatus').innerHTML = "<span style='color:red'>An error occurred updating the Properties</span>";
        }
    }
    return false;
}

function CheckShared() {
    if (document.getElementById('ctl00_cphMain_txtMemberProperty').value.indexOf("S") > 0) {
        document.getElementById('ctl00_cphMain_txtMemberAlias').disabled = true;
        if (document.getElementById('ctl00_cphMain_txtSourcePlanType') != null) { document.getElementById('ctl00_cphMain_txtSourcePlanType').disabled = true; }
        if (document.getElementById('ctl00_cphMain_txtAccountType') != null) { document.getElementById('ctl00_cphMain_txtAccountType').disabled = true; }
        if (document.getElementById('ctl00_cphMain_txtDataType') != null) { document.getElementById('ctl00_cphMain_txtDataType').disabled = true; }
        if (document.getElementById('ctl00_cphMain_txtExchangeRate') != null) { document.getElementById('ctl00_cphMain_txtExchangeRate').disabled = true; }
        if (document.getElementById('ctl00_cphMain_txtSmartList') != null) { document.getElementById('ctl00_cphMain_txtSmartList').disabled = true; }
        if (document.getElementById('ctl00_cphMain_txtBaseCurrency') != null) { document.getElementById('ctl00_cphMain_txtBaseCurrency').disabled = true; }
    }

    if (parent.document.getElementById(parent.tree_selected_id).innerHTML != document.getElementById('ctl00_cphMain_hidNewTreeName').value) {
        parent.document.getElementById(parent.tree_selected_id).innerHTML = document.getElementById('ctl00_cphMain_hidNewTreeName').value;
    }
}

function SharedChange() {
    if (document.getElementById('ctl00_cphMain_txtMemberProperty').value.indexOf("S") > 0) {
        document.getElementById('ctl00_cphMain_txtMemberAlias').disabled = true;
        if (document.getElementById('ctl00_cphMain_txtSourcePlanType') != null) { document.getElementById('ctl00_cphMain_txtSourcePlanType').disabled = true; }
        if (document.getElementById('ctl00_cphMain_txtAccountType') != null) { document.getElementById('ctl00_cphMain_txtAccountType').disabled = true; }
        if (document.getElementById('ctl00_cphMain_txtDataType') != null) { document.getElementById('ctl00_cphMain_txtDataType').disabled = true; }
        if (document.getElementById('ctl00_cphMain_txtExchangeRate') != null) { document.getElementById('ctl00_cphMain_txtExchangeRate').disabled = true; }
        if (document.getElementById('ctl00_cphMain_txtSmartList') != null) { document.getElementById('ctl00_cphMain_txtSmartList').disabled = true; }
        if (document.getElementById('ctl00_cphMain_txtBaseCurrency') != null) { document.getElementById('ctl00_cphMain_txtBaseCurrency').disabled = true; }
    }
    else {
        document.getElementById('ctl00_cphMain_txtMemberAlias').disabled = false;
        if (document.getElementById('ctl00_cphMain_txtSourcePlanType') != null) { document.getElementById('ctl00_cphMain_txtSourcePlanType').disabled = false; }
        if (document.getElementById('ctl00_cphMain_txtAccountType') != null) { document.getElementById('ctl00_cphMain_txtAccountType').disabled = false; }
        if (document.getElementById('ctl00_cphMain_txtDataType') != null) { document.getElementById('ctl00_cphMain_txtDataType').disabled = false; }
        if (document.getElementById('ctl00_cphMain_txtExchangeRate') != null) { document.getElementById('ctl00_cphMain_txtExchangeRate').disabled = false; }
        if (document.getElementById('ctl00_cphMain_txtSmartList') != null) { document.getElementById('ctl00_cphMain_txtSmartList').disabled = false; }
        if (document.getElementById('ctl00_cphMain_txtBaseCurrency') != null) { document.getElementById('ctl00_cphMain_txtBaseCurrency').disabled = false; }
    }

    if (parent.document.getElementById(parent.tree_selected_id).innerHTML != document.getElementById('ctl00_cphMain_hidNewTreeName').value) {
        parent.document.getElementById(parent.tree_selected_id).innerHTML = document.getElementById('ctl00_cphMain_hidNewTreeName').value;
    }
}