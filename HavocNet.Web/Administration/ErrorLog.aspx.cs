using System;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using Phoenix.Core;
using Phoenix.Core.Tables;
using Phoenix.JS;

namespace HavocNet.Web.Administration
{
    public partial class ErrorLog : Page
    {
        private int _rowCount = 1;
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
            this.phErrors.Controls.Add(BindErrorList(aServer, "none"));

            aServer.Close();

            this.cmdExportToExcel.Click += cmdExportToExcel_Click;
        }

        private Table BindErrorList(HavocServer aServer, string show)
        {
            aServer.SQLParams.Add(this.txtDateFrom.Text);
            aServer.SQLParams.Add(this.txtDateTo.Text);
            aServer.SQLParams.Add(this.cboUsers.SelectedValue);

            var strSQL = @"SELECT 
                Username,
                Timestamp,
                isnull(ErrorMessage,'UNKNOWN') AS Name,
                isnull(ErrorURL + ' ' + ErrorStackTrace,'') AS Data 
                FROM 
                dbo.tblErrors 
                WHERE 
                Timestamp > convert(datetime,@Param0,103) 
                AND Timestamp < dateadd(day,1,convert(datetime,@Param1,103)) ";

            if (this.cboUsers.SelectedValue != "All") { strSQL += " AND Username = @Param2"; }

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
                    tbl.Rows.Add(MakeDetailRow(aReader["Data"].ToString(), show));
                }
            }
            else
            {
                tbl.Rows.Add(MakeFooterRow("No Errors Occurred With These Filters"));
            }
            aReader.Close();

            return tbl;
        }

        private TableRow MakeHeaderRow()
        {
            _myMaster.aFSDataTable.AddColumn(new FSColumn("Username", typeof(string), 110));
            _myMaster.aFSDataTable.AddColumn(new FSColumn("Timestamp", typeof(string)));
            _myMaster.aFSDataTable.AddColumn(new FSColumn("Error", typeof(string)));
            _myMaster.aFSDataTable.AddColumn(new FSColumn("Detail", typeof(string), FSAlignment.Centre, 40));

            return _myMaster.aFSDataTable.HTMLHeader();
        }

        private TableRow MakeFooterRow(string strFooter)
        {
            var aRow = new TableRow();

            aRow.Cells.Add(new FSTableCell(strFooter, 1000, true, HorizontalAlign.Center ));
            aRow.Cells[0].ColumnSpan = 4;
            
            return aRow;
        }

        private TableRow MakeRow(DbDataReader aReader)
        {
            var aRow = new TableRow();

            aRow.Cells.Add(new FSTableCell(aReader["Username"].ToString(), 110, false));
            aRow.Cells.Add(new FSTableCell(aReader["Timestamp"].ToString(), 130, false));
            aRow.Cells.Add(new FSTableCell(aReader["Name"].ToString(), 660, false));

            var cmd = new LinkButton
                {
                    Text = @"[+]",
                    ForeColor = WebFormat.SuccessColour,
                    Font = {Underline = false},
                    OnClientClick = "javascript:return UnhideDetail(this," + _rowCount + ");"
                };

            aRow.Cells.Add(new FSTableCell(cmd, 40, HorizontalAlign.Center));

            _myMaster.aFSDataTable.DT.Rows.Add(
                aReader["Username"].ToString(),
                aReader["Timestamp"].ToString(),
                aReader["Name"].ToString(),
                ""
                );

            return aRow;
        }

        private TableRow MakeDetailRow(string detail, string show)
        {
            var aRow = new TableRow();

            aRow.Cells.Add(new FSTableCell(detail.Replace(" at", "<br/> at"), 1000, false));
            aRow.Cells[0].ColumnSpan = 4;
            aRow.Style["display"] = show;
            aRow.ID = "row" + _rowCount;

            _myMaster.aFSDataTable.DT.Rows.Add(detail, "", "", "");

            _rowCount += 1;

            return aRow;
        }

        private void BindUsers(HavocServer aServer)
        {
            var aReader = aServer.ExecuteReader("SELECT 'All' AS Value, 'All' AS Text UNION ALL SELECT DISTINCT Username AS Value, Username AS Text FROM dbo.tblErrors");

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

        private void cmdExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            _myMaster.ExportToExcel("Error Log");
        }
    }
}