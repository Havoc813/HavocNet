using System;
using System.Data.Common;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using Phoenix.Core;
using Phoenix.Core.Servers;

namespace HavocNet.Web.Puzzles
{
    public partial class CharacterRef : Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainPuzzle");

            BindCharacterType();

            cmdExportToExcel.Click +=cmdExportToExcel_Click;

            this.phColourReferences.Controls.Clear();
            this.phColourReferences.Controls.Add(MakeTable());
        }

        private Table MakeTable()
        {
            var aTable = new Table();
            aTable.Style["width"] = "100%";

            aTable.Rows.Add(MakeHeaderRow());

            var aServer = new HavocServer();
            aServer.Open();

            var strWhere = "";

            if (this.cboCharacterType.SelectedValue != "All Characters") strWhere = "WHERE CharacterType = '" + this.cboCharacterType.SelectedValue + "'";
            
            var strSQL = "SELECT " +
                            "CharacterName, " +
                            "isnull(ASCII,'') AS ASCII, " +
                            "isnull(javascript,'') AS javascript, " +
                            "isnull(morse,'') AS Morse " +
                            "FROM " +
                            "dbo.tblCharacterReference " +
                            "" + strWhere + " " +
                            "ORDER BY Ordering";

            var aReader = aServer.ExecuteReader(strSQL);
            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    aTable.Rows.Add(MakeRow(aReader));
                }
            }
            aReader.Close();

            aServer.Close();

            return aTable;
        }

        private TableRow MakeHeaderRow()
        {
            var aRow = new TableRow {BackColor = Color.PaleGoldenrod};

            aRow.Cells.Add(new FSTableCell("Character", 100, true, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell("ASCII", 100, true, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell("Javascript", 100, true, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell("Morse", 100, true, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell("Wingding", 100, true, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell("Symbol", 100, true, HorizontalAlign.Center));

            return aRow;
        }

        private TableRow MakeRow(DbDataReader aReader)
        {
            var aRow = new TableRow();

            aRow.Cells.Add(new FSTableCell(aReader["CharacterName"].ToString().Replace("<", "&lt").Replace(">", "&gt"), 100, false, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell(aReader["ASCII"].ToString(), 100, false, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell(aReader["javascript"].ToString(), 100, false, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell(aReader["Morse"].ToString(), 100, false, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell(aReader["CharacterName"].ToString().Replace("<", "&lt").Replace(">", "&gt"), 100, false, HorizontalAlign.Center));
            aRow.Cells[aRow.Cells.Count - 1].Font.Name = "Wingdings";
            aRow.Cells.Add(new FSTableCell(aReader["CharacterName"].ToString().Replace("<", "&lt").Replace(">", "&gt"), 100, false, HorizontalAlign.Center));
            aRow.Cells[aRow.Cells.Count - 1].Font.Name = "Symbol";

            return aRow;
        }

        protected void cmdExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            //_myMaster.ExportToExcel("CharacterReference", MakeTable());
        }

        private void BindCharacterType()
        {
            this.cboCharacterType.Items.Clear();

            this.cboCharacterType.Items.Add("All Characters");
            this.cboCharacterType.Items.Add("Upper Case Characters");
            this.cboCharacterType.Items.Add("Lower Case Characters");
            this.cboCharacterType.Items.Add("Numeric Characters");
            this.cboCharacterType.Items.Add("Symbols");
            this.cboCharacterType.Items.Add("Function Characters");

            if (!Page.IsPostBack)
            {
                this.cboCharacterType.Items.FindByValue("All Characters").Selected = true;
            }
            else
            {
                if ((Request["ctl00$cphMain$cboCharacterType"] != null))
                {
                    this.cboCharacterType.Items.FindByValue(Request["ctl00$cphMain$cboCharacterType"]).Selected = true;
                }
                else
                {
                    this.cboCharacterType.Items.FindByValue("All Characters").Selected = true;
                }
            }

        }
    }
}