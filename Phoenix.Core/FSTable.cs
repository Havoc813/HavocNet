using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Phoenix.Core
{
    public class FSTable : Table
    {
        public void AddRow(IEnumerable<TableItem> items)
        {
            var aRow = new TableRow();

            foreach (var tableItem in items)
            {
                aRow.Cells.Add(tableItem);
            }

            base.Rows.Add(aRow);
        }
    }

    public class TableItem : TableCell
    {
        public TableItem(string itemText)
        {
            base.Text = itemText;
            base.CssClass = "DefaultCell";
        }

        public TableItem(string itemText, string cssClass)
        {
            base.Text = itemText;
            base.CssClass = cssClass;
        }

        public TableItem(string itemText, string cssClass, int width)
        {
            base.Text = itemText;
            base.CssClass = cssClass;
            base.Width = width;
        }

        public TableItem(string itemText, int width)
        {
            base.Text = itemText;
            base.CssClass = "DefaultCell";
            base.Width = width;
        }
    }

    public class FSTableCell : TableCell
    {
        public void BaseSetup()
        {
            base.Style["padding"] = "4px";
            base.BorderColor = Color.Black;
            base.BorderWidth = 1;
            base.BorderStyle = BorderStyle.Solid;
            base.Wrap = true;
        }

        public FSTableCell(string itemText, int width, bool bold, HorizontalAlign align)
        {
            BaseSetup();

            base.Text = itemText;
            base.HorizontalAlign = align;
            base.Width = width;
            base.Font.Bold = bold;
        }

        public FSTableCell(string itemText, int width, bool bold)
        {
            BaseSetup();

            base.Text = itemText;
            base.Font.Bold = bold;
            base.Width = width;
        }

        public FSTableCell(string itemText, bool bold, HorizontalAlign align)
        {
            BaseSetup();

            base.Text = itemText;
            base.HorizontalAlign = align;
            base.Font.Bold = bold;
        }

        public FSTableCell(string itemText, bool bold)
        {
            BaseSetup();

            base.Text = itemText;
            base.Font.Bold = bold;
        }

        public FSTableCell(Control ctrl, int width)
        {
            BaseSetup();

            base.Controls.Add(ctrl);
            base.Width = width;
        }

        public FSTableCell(Control ctrl, int width, HorizontalAlign align)
        {
            BaseSetup();

            base.Controls.Add(ctrl);
            base.Width = width;
            base.HorizontalAlign = align;
        }

        public FSTableCell(Control ctrl)
        {
            BaseSetup();

            base.Controls.Add(ctrl);
        }

        public FSTableCell(string itemText, bool bold, HorizontalAlign align, Color forecolour)
        {
            BaseSetup();

            base.Text = itemText;
            base.HorizontalAlign = align;
            base.Font.Bold = bold;
            base.ForeColor = forecolour;
        }
    }
}
