using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using HavocNet.Repositories;
using Phoenix.Core;
using Phoenix.Core.Servers;

namespace HavocNet.Web.Puzzles
{
    public partial class Dictionaries : Page
    {
        private HavocNetMaster _myMaster;
        private DictionaryRepository _aRepository;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainPuzzle");

            _aRepository = new DictionaryRepository(new HavocServer(), _myMaster.aUser);

            this.phTable.Controls.Clear();
            this.phTable.Controls.Add(MakeTable(""));

            this.hidSelectedRow.Value = "";
            this.hidEditingRow.Value = "";
        }

        private void BindButtons(string action)
        {
            this.cmdSave.Visible = (action == "Add") && (_myMaster.PageAccess == 2);
            this.cmdAdd.Visible = (action != "Add") && (_myMaster.PageAccess == 2);
            this.cmdDelete.Visible = (action != "Edit" && action != "Add") && (_myMaster.PageAccess == 2);
            this.cmdCancel.Visible = (action == "Edit" || action == "Add") && (_myMaster.PageAccess == 2);
            this.cmdEdit.Visible = (action != "Edit") && (_myMaster.PageAccess == 2);
            this.cmdUpdate.Visible = (action == "Edit") && (_myMaster.PageAccess == 2);
            this.cmdUp.Visible = _myMaster.PageAccess == 2;
            this.cmdDown.Visible = _myMaster.PageAccess == 2;
            this.cmdClear.Visible = _myMaster.PageAccess == 2;
            this.upWords.Visible = _myMaster.PageAccess == 2;
            this.cmdUpload.Visible = _myMaster.PageAccess == 2;

            this.cmdAdd.Click += cmdAdd_Click;
            this.cmdDelete.Click += cmdDelete_Click;
            this.cmdCancel.Click += cmdCancel_Click;
            this.cmdEdit.Click += cmdEdit_Click;
            this.cmdExportToExcel.Click += cmdExportToExcel_Click;
            this.cmdSave.Click += cmdSave_Click;
            this.cmdUpdate.Click += cmdUpdate_Click;
            this.cmdUp.Click += cmdUp_Click;
            this.cmdDown.Click += cmdDown_Click;

            this.cmdClear.Style["display"] = "none";
            this.upWords.Style["display"] = "none";
            this.cmdUpload.Style["display"] = "none";
            this.cmdClear.Click += cmdClear_Click;
            this.cmdUpload.Click += cmdUpload_Click;
        }

        private Table MakeTable(string action)
        {
            var tbl = new Table();
            tbl.Style["margin-left"] = "1px"; //For Firefox
            tbl.Style["width"] = "100%"; 
            tbl.Rows.Add(MakeHeaderRow());

            foreach (var aDictionary in _aRepository.GetAll())
            {
                tbl.Rows.Add(MakeRow(aDictionary, action));
            }

            if (action == "Add") tbl.Rows.Add(MakeNewRow());

            BindButtons(action);

            return tbl;
        }

        private TableRow MakeHeaderRow()
        {
            var aRow = new TableRow { BackColor = Color.PaleGoldenrod};

            aRow.Cells.Add(new FSTableCell("Name", 120, true));
            aRow.Cells.Add(new FSTableCell("Description", 400, true));
            aRow.Cells.Add(new FSTableCell("Loaded", 120, true));

            return aRow;
        }

        private TableRow MakeNewRow()
        {
            var aRow = new TableRow { BackColor = Color.PaleGoldenrod };

            aRow.Cells.Add(new FSTableCell(new TextBox {ID = "txtNewDictionaryName", CssClass = "wideTextBox"}, 120));
            aRow.Cells.Add(new FSTableCell(new TextBox { ID = "txtNewDictionaryDescription", CssClass = "wideTextBox" }, 400));
            aRow.Cells.Add(new FSTableCell(new CheckBox { ID = "chkLoaded", Enabled = false }, 120));

            return aRow;
        }

        private TableRow MakeRow(WordDictionary aDictionary, string action)
        {
            var aRow = new TableRow();
            aRow.Attributes["onmouseover"] = "javascript:aTableManage.OnMouseOver(this);";
            aRow.Attributes["onmouseout"] = "javascript:aTableManage.OnMouseOut(this);";
            aRow.Attributes["onclick"] = "javascript:aTableManage.OnRowClick(this); ChangeButtons('" + aDictionary.ID + "');";
            aRow.ID = aDictionary.ID.ToString("");

            if (action == "Edit" && aDictionary.ID.ToString("") == Request["ctl00$cphMain$hidSelectedRow"].Replace("ctl00_cphMain_", ""))
            {
                aRow.BackColor = Color.PaleGoldenrod;

                aRow.Cells.Add(new FSTableCell(new TextBox { ID = "txtEditDictionaryName", Text = aDictionary.Name, CssClass = "wideTextBox" }, 120));
                aRow.Cells.Add(new FSTableCell(new TextBox { ID = "txtEditDictionaryDescription", Text = aDictionary.Description, CssClass = "wideTextBox" }, 400));
                aRow.Cells.Add(new FSTableCell(new CheckBox { ID = "chkLoaded", Checked = aDictionary.Loaded, Enabled = false}, 120));
                aRow.Cells[aRow.Cells.Count - 1].Controls.Add(new HiddenField { ID = "hid" + aDictionary.ID, Value = aDictionary.IsLoaded });

                this.hidEditingRow.Value = Request["ctl00$cphMain$hidSelectedRow"];
            }
            else
            {
                aRow.Cells.Add(new FSTableCell(aDictionary.Name, 120, false));
                aRow.Cells.Add(new FSTableCell(aDictionary.Description, 120, false));
                aRow.Cells.Add(new FSTableCell(new Label{ Text = aDictionary.IsLoaded }, 120));
                aRow.Cells[aRow.Cells.Count - 1].Controls.Add(new HiddenField { ID = "hid" + aDictionary.ID, Value = aDictionary.IsLoaded });
            }
            
            return aRow;
        }

        protected void cmdAdd_Click(object sender, ImageClickEventArgs e)
        {
            this.phTable.Controls.Clear();
            this.phTable.Controls.Add(MakeTable("Add"));
        }

        protected void cmdCancel_Click(object sender, ImageClickEventArgs e)
        {
            this.phTable.Controls.Clear();
            this.phTable.Controls.Add(MakeTable("Cancel"));
        }

        protected void cmdEdit_Click(object sender, ImageClickEventArgs e)
        {
            this.phTable.Controls.Clear();
            this.phTable.Controls.Add(MakeTable("Edit"));
        }

        protected void cmdExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            //_myMaster.ExportToExcel("Users", MakeTable(""));
        }

        protected void cmdDelete_Click(object sender, ImageClickEventArgs e)
        {
            this.phTable.Controls.Clear();
            
            _aRepository.Delete(int.Parse(Request["ctl00$cphMain$hidSelectedRow"].Replace("ctl00_cphMain_", "")));

            this.phTable.Controls.Add(MakeTable("Delete"));
        }

        protected void cmdSave_Click(object sender, ImageClickEventArgs e)
        {
            this.phTable.Controls.Clear();

            _aRepository.Save(
                Request["ctl00$cphMain$txtNewDictionaryName"],
                Request["ctl00$cphMain$txtNewDictionaryDescription"]
                );

            this.phTable.Controls.Add(MakeTable("Save"));
        }

        protected void cmdUpdate_Click(object sender, ImageClickEventArgs e)
        {
            this.phTable.Controls.Clear();

            _aRepository.Update(
                int.Parse(Request["ctl00$cphMain$hidEditingRow"].Replace("ctl00_cphMain_", "")),
                Request["ctl00$cphMain$txtEditDictionaryName"],
                Request["ctl00$cphMain$txtEditDictionaryDescription"]
                );

            this.phTable.Controls.Add(MakeTable("Update"));
        }

        void cmdDown_Click(object sender, ImageClickEventArgs e)
        {
            this.phTable.Controls.Clear();

            _aRepository.MoveDown(int.Parse(Request["ctl00$cphMain$hidSelectedRow"].Replace("ctl00_cphMain_", "")));

            this.phTable.Controls.Add(MakeTable("MoveDown"));
        }

        void cmdUp_Click(object sender, ImageClickEventArgs e)
        {
            this.phTable.Controls.Clear();

            _aRepository.MoveUp(int.Parse(Request["ctl00$cphMain$hidSelectedRow"].Replace("ctl00_cphMain_", "")));

            this.phTable.Controls.Add(MakeTable("MoveUp"));
        }

        void cmdUpload_Click(object sender, ImageClickEventArgs e)
        {
            this.phTable.Controls.Clear();
            var filename = Server.MapPath(@"\App_Data\Lexicon\") + this.upWords.FileName;

            this.upWords.SaveAs(filename);

            _aRepository.LoadDictionary(
                int.Parse(Request["ctl00$cphMain$hidSelectedRow"].Replace("ctl00_cphMain_", "")),
                filename
                );

            this.phTable.Controls.Add(MakeTable("LoadDictionary"));
        }

        void cmdClear_Click(object sender, ImageClickEventArgs e)
        {
            this.phTable.Controls.Clear();

            _aRepository.ClearWords(int.Parse(Request["ctl00$cphMain$hidSelectedRow"].Replace("ctl00_cphMain_", "")));

            this.phTable.Controls.Add(MakeTable("ClearWords"));
        }
    }
}