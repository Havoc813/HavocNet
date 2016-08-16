using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;

namespace Phoenix.Core.Tables
{
    public class FSDataTable
    {
        public readonly FSTableColumns TableColumns = new FSTableColumns();
        public readonly DataTable DT = new DataTable();
        private Table _table = new Table();
        private readonly List<int> _hiddenColumns = new List<int>();

        public FSDataTable(bool fullWidth = true)
        {
            if (fullWidth) _table.Style["width"] = "100%";
            _table.Style["font-size"] = "8pt";
        }

        public void AddColumn(FSColumn aColumn)
        {
            TableColumns.Items.Add(aColumn);
            DT.Columns.Add(aColumn.Text, aColumn.DataType);
        }

        public TableRow HTMLHeader(string[] excludes = null)
        {
            var aRow = new TableRow { BackColor = Color.AliceBlue };

            for (var i = 0; i < TableColumns.Items.Count; i++)
            {
                if (excludes != null)
                    if (excludes.Contains(TableColumns.Items[i].Text)) _hiddenColumns.Add(i);

                aRow.Cells.Add(new FSTableCell(TableColumns.Items[i].Text, TableColumns.Items[i].PixelWidth, true,
                    HTMLAlignment.FSAlignToHTMLAlign(TableColumns.Items[i].HorizontalAlignment)));
            }

            return aRow;
        }

        public void AddRow(TableRow aRow)
        {
            _table.Rows.Add(aRow);

            var values = new object[aRow.Cells.Count];
            for (var j = 0; j < aRow.Cells.Count; j++)
            {
                if (this.TableColumns.Items[j].DataType == typeof(DateTime))
                {
                    values[j] = aRow.Cells[j].Text == "" ? "01/01/1900 00:00:00" : aRow.Cells[j].Text;
                }
                else
                {
                    values[j] = aRow.Cells[j].Text;
                }
            }
            DT.Rows.Add(values);
        }

        public Table Publish(string[] excludes = null, string noneRecord = "NONE")
        {
            _table.Rows.AddAt(0, HTMLHeader(excludes));

            if (_table.Rows.Count == 1) _table.Rows.Add(MakeBlankRow(noneRecord, _table.Rows[0].Cells.Count));

            if (excludes == null) return _table;

            foreach (TableRow aRow in _table.Rows)
            {
                for (var i = 0; i <= aRow.Cells.Count - 1; i++)
                {
                    if (_hiddenColumns.Contains(i)) aRow.Cells[i].Visible = false;
                }
            }

            return _table;
        }

        public TableRow MakeBlankRow(string none, int count)
        {
            var aRow = new TableRow();

            aRow.Cells.Add(new FSTableCell(none, false, HorizontalAlign.Center));
            aRow.Cells[0].ColumnSpan = count;

            return aRow;
        }
    }
}
