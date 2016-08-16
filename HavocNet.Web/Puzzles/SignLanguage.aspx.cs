using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Image = System.Web.UI.WebControls.Image;

namespace HavocNet.Web.Puzzles
{
    public partial class SignLanguage : Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainPuzzle");

            this.phSigns.Controls.Clear();
            this.phSigns.Controls.Add(MakeSignsReference());

            this.phSignAnswer.Controls.Clear();

            this.cmdMorseToText.Click += cmdConvert_Click;
        }

        private Table MakeSignsReference()
        {
            var aTable = new Table {BorderColor = Color.Gray, BorderStyle = BorderStyle.Solid, BorderWidth = 1};

            var aRow1 = new TableRow();
            for (var i = 0; i <= 12; i++)
            {
                var aCell = new TableCell();
                var img = new Image {ImageUrl = "~\\App_Themes\\HavocNet\\Images\\Signs\\Sign_" + (char) (65 + i) + ".png"};
                aCell.Controls.Add(img);
                aRow1.Cells.Add(aCell);
            }

            var aRow2 = new TableRow();
            for (var i = 13; i <= 25; i++)
            {
                var aCell = new TableCell();
                aCell.Controls.Add(new Image {ImageUrl = "~\\App_Themes\\HavocNet\\Images\\Signs\\Sign_" + (char) (65 + i) + ".png"});
                aRow2.Cells.Add(aCell);
            }

            var aRow3 = new TableRow();
            for (var i = 0; i <= 12; i++)
            {
                var aCell = new TableCell();
                if (i < 10)
                {
                    aCell.Controls.Add(new Image { ImageUrl = "~\\App_Themes\\HavocNet\\Images\\Signs\\Sign_" + Convert.ToString(i) + ".png" });
                }
                else
                {
                    aCell.Text = "";
                }
                aRow3.Cells.Add(aCell);
            }

            aTable.Rows.Add(aRow1);
            aTable.Rows.Add(aRow2);
            aTable.Rows.Add(aRow3);

            return aTable;
        }

        protected void cmdConvert_Click(object sender, EventArgs e)
        {
            var aTable = new Table {BorderColor = Color.Gray, BorderStyle = BorderStyle.Solid, BorderWidth = 1};

            const string referenceStr = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var txtPlain = Request["ctl00$cphMain$txtPlainText"];
            var aRow = new TableRow();

            for (var i = 0; i <= txtPlain.Length - 1; i++)
            {
                var aCell = new TableCell();
                var img = new Image();

                if (referenceStr.IndexOf(txtPlain.Substring(i, 1), StringComparison.Ordinal) >= 0)
                {
                    img.ImageUrl = "~\\App_Themes\\HavocNet\\Images\\Signs\\Sign_" + txtPlain.Substring(i, 1) + ".png";
                }
                else
                {
                    img.ImageUrl = "~\\App_Themes\\HavocNet\\Images\\Signs\\Sign_Blank.png";
                }

                aCell.Controls.Add(img);
                aRow.Cells.Add(aCell);
            }

            aTable.Rows.Add(aRow);

            this.phSignAnswer.Controls.Clear();
            this.phSignAnswer.Controls.Add(aTable);
        }
    }
}