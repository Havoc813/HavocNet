using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using Phoenix.Core;

namespace HavocNet.Web.Puzzles
{
    public partial class ColourRef : Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainPuzzle");

            this.cmdExportToExcel.Click +=cmdExportToExcel_Click;

            this.phColourReferences.Controls.Clear();
            this.phColourReferences.Controls.Add(MakeTable());
        }

        private Table MakeTable()
        {
            var aTable = new Table();
            aTable.Style["width"] = "100%";

            aTable.Rows.Add(MakeHeaderRow());

            foreach (var aColour in Enum.GetNames(typeof(KnownColor)))
            {
                var theColour = Color.FromName(aColour);

                if (!theColour.IsSystemColor | this.chkInclude.Checked)
                {
                    aTable.Rows.Add(MakeRow(theColour));
                }
            }

            return aTable;
        }

        private TableRow MakeHeaderRow()
        {
            var aRow = new TableRow {BackColor = Color.PaleGoldenrod};

            aRow.Cells.Add(new FSTableCell("Name",400, true));
            aRow.Cells.Add(new FSTableCell("Hex", 120, true, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell("RGB", 200, true, HorizontalAlign.Center));
            aRow.Cells[aRow.Cells.Count - 1].ColumnSpan = 6;
            aRow.Cells.Add(new FSTableCell("Sample", 110, true, HorizontalAlign.Center));

            return aRow;
        }

        private TableRow MakeRow(Color aColour)
        {
            var aRow = new TableRow();

            aRow.Cells.Add(new FSTableCell(aColour.Name, false));
            aRow.Cells.Add(new FSTableCell("#" + aColour.ToArgb().ToString("X").Substring(2), 120, false, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell("R: ", false, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell(aColour.R.ToString(""), false, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell("G: ", false, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell(aColour.G.ToString(""), false, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell("B: ", false, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell(aColour.B.ToString(""), false, HorizontalAlign.Center));
            aRow.Cells.Add(new FSTableCell("", 110, false, HorizontalAlign.Center));
            aRow.Cells[aRow.Cells.Count - 1].BackColor = aColour;

            return aRow;
        }

        protected void cmdExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            //_myMaster.ExportToExcel("ColourReference", MakeTable());
        }
    }
}