using System;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using Phoenix.Core;
using Phoenix.Core.Tables;
using Phoenix.JS;

namespace HavocNet.Web.Administration
{
    public partial class AuditLog : Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainAdministration");

            Validation.Include(Page.ClientScript);

            var aServer = new HavocServer();
            aServer.Open();

            BindUsers(aServer);
            BindDates();
            BindAction(aServer);
            this.phAudits.Controls.Add(BindAuditList(aServer));

            aServer.Close();

            this.cmdExportToExcel.Click += cmdExportToExcel_Click;
        }

        private Table BindAuditList(HavocServer aServer)
        {
            aServer.SQLParams.Clear();
            aServer.SQLParams.Add(this.txtDateFrom.Text);
            aServer.SQLParams.Add(this.txtDateTo.Text);
            aServer.SQLParams.Add(this.cboUsers.SelectedValue);
            aServer.SQLParams.Add(this.cboAction.SelectedValue);

            var strSQL = @"SELECT 
                Username,
                Timestamp,
                isnull(Action,'UNKNOWN') AS Name,
                ISNULL(Data,'') AS Data 
                FROM 
                dbo.tblAudits 
                WHERE 
                Timestamp > convert(datetime,@Param0,103) 
                AND Timestamp < dateadd(day,1,convert(datetime,@Param1,103)) ";

            if (this.cboUsers.SelectedValue != "All") { strSQL += " AND UserID = @Param2"; }

            if (this.cboAction.SelectedValue != "All") { strSQL += " AND Action = @Param3"; }

            strSQL += " ORDER BY Timestamp DESC";

            var tbl = new Table();
            tbl.Style["word-break"] = "break-all";
            tbl.Style["width"] = "100%";

            tbl.Rows.Add(MakeHeaderRow());

            var aReader = aServer.ExecuteReader(strSQL);
            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    tbl.Rows.Add(MakeRow(aReader));
                }
            }
            else
            {
                tbl.Rows.Add(MakeFooterRow("No Actions Occurred With These Filters"));
            }
            aReader.Close();

            return tbl;
        }

        private TableRow MakeHeaderRow()
        {
            _myMaster.aFSDataTable.AddColumn(new FSColumn("Username", typeof(string)));
            _myMaster.aFSDataTable.AddColumn(new FSColumn("Timestamp", typeof(string)));
            _myMaster.aFSDataTable.AddColumn(new FSColumn("Name", typeof(string)));
            _myMaster.aFSDataTable.AddColumn(new FSColumn("Data", typeof(string)));

            return _myMaster.aFSDataTable.HTMLHeader();
        }

        private TableRow MakeFooterRow(string strFooter)
        {
            var aRow = new TableRow();

            aRow.Cells.Add(new FSTableCell(strFooter, 1000, true, HorizontalAlign.Center));
            aRow.Cells[0].ColumnSpan = 4;

            return aRow;
        }

        private TableRow MakeRow(DbDataReader aReader)
        {
            var aRow = new TableRow();

            aRow.Cells.Add(new FSTableCell(aReader["Username"].ToString(), 110, false));
            aRow.Cells.Add(new FSTableCell(aReader["Timestamp"].ToString(), 130, false));
            aRow.Cells.Add(new FSTableCell(aReader["Name"].ToString(), 250, false));
            aRow.Cells.Add(new FSTableCell(FSStringHelper.ToHTML(aReader["Data"].ToString()), 700, false));
            aRow.Cells[aRow.Cells.Count - 1].Wrap = true;

            _myMaster.aFSDataTable.DT.Rows.Add(
                aReader["Username"].ToString(),
                aReader["Timestamp"].ToString(),
                aReader["Name"].ToString(),
                aReader["Data"].ToString()
                );

            return aRow;
        }

        private void BindUsers(HavocServer aServer)
        {
            var aReader = aServer.ExecuteReader("SELECT 'All' AS Value, 'All' AS Text UNION ALL SELECT DISTINCT Username AS Value, Username AS Text FROM dbo.tblUsers");

            this.cboUsers.DataSource = aReader;
            this.cboUsers.DataValueField = "Value";
            this.cboUsers.DataTextField = "Text";
            this.cboUsers.DataBind();

            aReader.Close();

            if (!Page.IsPostBack)
            {
                this.cboUsers.Items[0].Selected = true;
            }
            else
            {
                this.cboUsers.Items.FindByValue(Request["ctl00$cphMain$cboUsers"]).Selected = true;
            }
        }

        private void BindDates()
        {
            if (!Page.IsPostBack)
            {
                this.txtDateFrom.Text = FSFormat.BasicDate(DateTime.Now.AddDays(-1));
                this.txtDateTo.Text = FSFormat.BasicDate(DateTime.Now);
            }
            else
            {
                this.txtDateFrom.Text = Request["ctl00$cphMain$txtDateFrom"];
                this.txtDateTo.Text = Request["ctl00$cphMain$txtDateTo"];
            }
        }

        private void BindAction(HavocServer aServer)
        {
            aServer.SQLParams.Add(this.txtDateFrom.Text);
            aServer.SQLParams.Add(this.txtDateTo.Text);
            aServer.SQLParams.Add(this.cboUsers.SelectedValue);

            var strSQL = @"SELECT 'All' AS Name 
                UNION ALL 
                SELECT DISTINCT Action AS Name 
                FROM dbo.tblAudits 
                WHERE 
                Timestamp > convert(datetime,@Param0,103) 
                AND Timestamp < dateadd(day,1,convert(datetime,@Param1,103)) ";

            if (this.cboUsers.SelectedValue != "All") { strSQL += " AND Username = @Param2 "; }

            var aReader = aServer.ExecuteReader(strSQL, aServer.SQLParams);

            this.cboAction.Items.Add("All");
            this.cboAction.DataSource = aReader;
            this.cboAction.DataTextField = "Name";
            this.cboAction.DataValueField = "Name";
            this.cboAction.DataBind();

            aReader.Close();

            if (!Page.IsPostBack)
            {
                if (this.cboAction.Items.Count > 0)
                    this.cboAction.Items[0].Selected = true;
            }
            else
            {
                if (this.cboAction.Items.Contains(new ListItem(Request["ctl00$cphMain$cboAction"])))
                    this.cboAction.Items.FindByValue(Request["ctl00$cphMain$cboAction"]).Selected = true;
            }
        }

        private void cmdExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            _myMaster.ExportToExcel("Audit Log");
        }
    }
}