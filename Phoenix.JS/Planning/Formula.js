function OnBeforeUpdateFormula() {
    if (confirm("Commit Formula Changes?")) {
        try {
            updateFormula();

            document.getElementById('ctl00_cphMain_lblFormulaStatus').innerHTML = "<span style='color:green'>Formula Updated Successfully</span>"; 
        } catch (e) {
            document.getElementById('ctl00_cphMain_lblFormulaStatus').innerHTML = "<span style='color:red'>An error occurred updating the Formula</span>";
        }
    }

    return false;
}