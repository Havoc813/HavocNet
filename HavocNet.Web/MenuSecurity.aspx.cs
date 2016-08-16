using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using Core.Repositories;
using Phoenix.Core;
using Phoenix.Core.Servers;

namespace HavocNet.Web
{
    public partial class MenuSecurity : Page
    {
        private HavocNetMaster _myMaster;
        private readonly UserRepository _aRepository = new UserRepository(new HavocServer());

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainAdministration");

            this.lblTitle.Text = @"Manage Users";

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

            this.cmdAdd.Click += cmdAdd_Click;
            this.cmdDelete.Click += cmdDelete_Click;
            this.cmdCancel.Click += cmdCancel_Click;
            this.cmdEdit.Click += cmdEdit_Click;
            this.cmdExportToExcel.Click += cmdExportToExcel_Click;
            this.cmdSave.Click += cmdSave_Click;
            this.cmdUpdate.Click += cmdUpdate_Click;
        }

        private Table MakeTable(string action)
        {
            var tbl = new Table();
            tbl.Style["margin-left"] = "1px"; //For Firefox
            tbl.Style["width"] = "100%"; 
            tbl.Rows.Add(MakeHeaderRow());

            foreach (var aUser in _aRepository.GetAll().Values)
            {
                tbl.Rows.Add(MakeRow(aUser, action));
            }

            if (action == "Add") tbl.Rows.Add(MakeNewRow());

            BindButtons(action);

            return tbl;
        }

        private TableRow MakeHeaderRow()
        {
            var aRow = new TableRow();

            aRow.Cells.Add(new FSTableCell("Username", 120, true));
            aRow.Cells.Add(new FSTableCell("Password", 200, true));
            aRow.Cells.Add(new FSTableCell("First Name", 120, true));
            aRow.Cells.Add(new FSTableCell("Surname", 120, true));
            aRow.Cells.Add(new FSTableCell("Enabled", 60, true, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell("Admin", 60, true, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell("Email Address", true));

            return aRow;
        }

        private TableRow MakeNewRow()
        {
            var aRow = new TableRow { BackColor = Color.PaleGoldenrod };

            aRow.Cells.Add(new FSTableCell(new TextBox {Width = 114, ID = "txtNewUsername"}, 120));
            aRow.Cells.Add(new FSTableCell(new TextBox { Width = 96, ID = "txtNewPassword", TextMode = TextBoxMode.Password}));
            aRow.Cells[aRow.Cells.Count - 1].Controls.Add(new TextBox { Width = 96, ID = "txtNewPasswordConfirm", TextMode = TextBoxMode.Password });
            aRow.Cells.Add(new FSTableCell(new TextBox {Width = 114, ID = "txtNewFirstName"}, 120));
            aRow.Cells.Add(new FSTableCell(new TextBox {Width = 114, ID = "txtNewSurname"}, 120));
            aRow.Cells.Add(new FSTableCell(new CheckBox {ID = "chkNewEnabled"}, 60, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell(new CheckBox {ID = "chkNewAdmin"}, 50, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell(new TextBox {Width = 250, ID = "txtNewEmailAddress"}));

            return aRow;
        }

        private TableRow MakeRow(User aUser, string action)
        {
            var aRow = new TableRow();
            aRow.Attributes["onmouseover"] = "javascript:aTableManage.OnMouseOver(this);";
            aRow.Attributes["onmouseout"] = "javascript:aTableManage.OnMouseOut(this);";
            aRow.Attributes["onclick"] = "javascript:aTableManage.OnRowClick(this);";
            aRow.ID = aUser.ID.ToString("");

            if (action == "Edit" && aUser.ID.ToString("") == Request["ctl00$cphMain$hidSelectedRow"].Replace("ctl00_cphMain_",""))
            {
                aRow.BackColor = Color.PaleGoldenrod;

                aRow.Cells.Add(new FSTableCell(new TextBox { Width = 114, ID = "txtEditUsername", Text = aUser.Username }, 120));
                aRow.Cells.Add(new FSTableCell(new TextBox { Width = 96, ID = "txtEditPassword", TextMode = TextBoxMode.Password}));
                aRow.Cells[aRow.Cells.Count - 1].Controls.Add(new TextBox { Width = 96, ID = "txtEditPasswordConfirm", TextMode = TextBoxMode.Password });
                aRow.Cells.Add(new FSTableCell(new TextBox { Width = 114, ID = "txtEditFirstName", Text = aUser.FirstName }, 120));
                aRow.Cells.Add(new FSTableCell(new TextBox { Width = 114, ID = "txtEditSurname", Text = aUser.Surname }, 120));
                aRow.Cells.Add(new FSTableCell(new CheckBox { ID = "chkEditEnabled", Checked = aUser.Enabled }, 60, HorizontalAlign.Center));
                aRow.Cells.Add(new FSTableCell(new CheckBox { ID = "chkEditAdmin", Checked = aUser.Admin }, 60, HorizontalAlign.Center));
                aRow.Cells.Add(new FSTableCell(new TextBox { Width = 234, ID = "txtEditEmailAddress", Text = aUser.EmailAddress }));

                this.hidEditingRow.Value = Request["ctl00$cphMain$hidSelectedRow"];
            }
            else
            {
                aRow.Cells.Add(new FSTableCell(aUser.Username, 120, false));
                aRow.Cells.Add(new FSTableCell("**********", 120, false));
                aRow.Cells.Add(new FSTableCell(aUser.FirstName, 120, false));
                aRow.Cells.Add(new FSTableCell(aUser.Surname, 120, false));
                aRow.Cells.Add(new FSTableCell(aUser.Enabled.ToString(), 60, false, HorizontalAlign.Center));
                aRow.Cells.Add(new FSTableCell(aUser.Admin.ToString(), 50, false, HorizontalAlign.Center));
                aRow.Cells.Add(new FSTableCell(aUser.EmailAddress, false));
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
            
            _aRepository.Delete(int.Parse(Request["ctl00$cphMain$hidSelectedRow"].Replace("ctl00_cphMain_", "")), _myMaster.aUser);

            this.phTable.Controls.Add(MakeTable("Delete"));
        }

        protected void cmdSave_Click(object sender, ImageClickEventArgs e)
        {
            this.phTable.Controls.Clear();

            _aRepository.Save(
                Request["ctl00$cphMain$txtNewUsername"],
                Request["ctl00$cphMain$txtNewPassword"],
                Request["ctl00$cphMain$txtNewFirstName"],
                Request["ctl00$cphMain$txtNewSurname"],
                Request["ctl00$cphMain$chkNewEnabled"],
                Request["ctl00$cphMain$chkNewAdmin"],
                Request["ctl00$cphMain$txtNewEmailAddress"],
                _myMaster.aUser
                );

            this.phTable.Controls.Add(MakeTable("Save"));
        }

        protected void cmdUpdate_Click(object sender, ImageClickEventArgs e)
        {
            this.phTable.Controls.Clear();

            _aRepository.Update(
                int.Parse(Request["ctl00$cphMain$hidEditingRow"].Replace("ctl00_cphMain_", "")),
                Request["ctl00$cphMain$txtEditUsername"],
                Request["ctl00$cphMain$txtEditPassword"],
                Request["ctl00$cphMain$txtEditFirstName"],
                Request["ctl00$cphMain$txtEditSurname"],
                Request["ctl00$cphMain$chkEditEnabled"],
                Request["ctl00$cphMain$chkEditAdmin"],
                Request["ctl00$cphMain$txtEditEmailAddress"],
                _myMaster.aUser
                );

            this.phTable.Controls.Add(MakeTable("Update"));
        }
    }
}