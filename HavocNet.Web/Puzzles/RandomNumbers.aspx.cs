using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HavocNet.Web.Puzzles
{
    public partial class RandomNumbers : Page
    {
        private HavocNetMaster _myMaster;
        private static Random _r = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainPuzzle");

            this.cmdGenerate.Click += cmdGenerate_Click;

            if (Page.IsPostBack) this.lblHistory.Text = Request["ctl00$cphMain$txtNumber"] + "<br />" + this.lblHistory.Text;
        }

        void cmdGenerate_Click(object sender, EventArgs e)
        {
            var tbl = new Table();
            
            var aRow = new TableRow();
            var aCell1 = new TableCell();
            var txt = new TextBox {Text = _r.Next().ToString(""), Width = 120};
            txt.Style["text-align"] = "center";
            txt.ID = "txtNumber";
            aCell1.Controls.Add(txt);
            aRow.Cells.Add(aCell1);
            tbl.Rows.Add(aRow);

            this.phRandom.Controls.Clear();
            this.phRandom.Controls.Add(tbl);
        }
    }
}