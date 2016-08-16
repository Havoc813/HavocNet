using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HavocNet.Web.Puzzles
{
    public partial class ColourConvert : Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainPuzzle");

            BindNamedColours();

            this.cmdConvertHexColour.Click +=cmdConvertHexColour_Click;
            this.cmdConvertNamedColour.Click +=cmdConvertNamedColour_Click;
            this.cmdConvertRGBColour.Click +=cmdConvertRGBColour_Click;
        }

        private void BindNamedColours()
        {
            this.cboNamedColours.Items.Clear();

            foreach (var aColour in Enum.GetNames(typeof(KnownColor)))
            {
                var theColour = Color.FromName(aColour);

                if (!theColour.IsSystemColor)
                    this.cboNamedColours.Items.Add(aColour);
            }

            if (!string.IsNullOrEmpty(Request["ctl00$cphMain$cboNamedColours"]))
            {
                this.cboNamedColours.Items.FindByValue(Request["ctl00$cphMain$cboNamedColours"]).Selected = true;
            }
        }


        protected void cmdConvertNamedColour_Click(object sender, ImageClickEventArgs e)
        {
            var colourToConvert = Color.FromName(Request["ctl00$cphMain$cboNamedColours"]);

            var aTable = new Table();
            aTable.Style["margin-left"] = "15px";
            aTable.Style["margin-top"] = "4px";

            var aRow = new TableRow();

            aRow.Cells.Add(new TableCell {Text = "Hex: "});
            
            var aCell2 = new TableCell();
            var txtHex = new TextBox {Width = 120, Text = "#" + colourToConvert.ToArgb().ToString("X").Substring(2)};

            txtHex.Style["margin-right"] = "10px";
            aCell2.Controls.Add(txtHex);
            aRow.Cells.Add(aCell2);
            
            aRow.Cells.Add(new TableCell {Text = "R: "});
            
            var aCell4 = new TableCell();
            var txtR = new TextBox {Width = 20};
            txtR.Style["text-align"] = "center";
            txtR.Text = Convert.ToString(colourToConvert.R);
            aCell4.Controls.Add(txtR);
            aRow.Cells.Add(aCell4);
            
            aRow.Cells.Add(new TableCell {Text = "G: "});
            
            var aCell6 = new TableCell();
            var txtG = new TextBox {Width = 20};
            txtG.Style["text-align"] = "center";
            txtG.Text = Convert.ToString(colourToConvert.G);
            aCell6.Controls.Add(txtG);
            aRow.Cells.Add(aCell6);
            
            aRow.Cells.Add(new TableCell {Text = "B: "});

            var aCell8 = new TableCell();
            var txtB = new TextBox {Width = 20};
            txtB.Style["text-align"] = "center";
            txtB.Text = Convert.ToString(colourToConvert.B);
            aCell8.Controls.Add(txtB);

            aRow.Cells.Add(aCell8);

            var aCell9 = new TableCell();
            var txtColour = new TextBox
                {
                    Width = 30,
                    BackColor = colourToConvert,
                    BorderColor = Color.Black,
                    BorderStyle = BorderStyle.Solid,
                    BorderWidth = 1
                };
            txtColour.Style["margin-left"] = "10px";
            aCell9.Controls.Add(txtColour);
            aRow.Cells.Add(aCell9);

            aTable.Rows.Add(aRow);

            this.phNamedColourConvert.Controls.Add(aTable);
        }

        protected void cmdConvertHexColour_Click(object sender, ImageClickEventArgs e)
        {
            var colourToConvert = ColorTranslator.FromHtml(Request["ctl00$cphMain$txtHexColour"]);

            var aTable = new Table();
            aTable.Style["margin-left"] = "15px";
            aTable.Style["margin-top"] = "4px";

            var aRow = new TableRow();

            aRow.Cells.Add(new TableCell {Text = "Named: "});

            var aCell2 = new TableCell();
            var txtNamed = new TextBox {Width = 120, Text = "Unknown"};

            foreach (string aColour in Enum.GetNames(typeof(KnownColor)))
            {
                var theColour = Color.FromName(aColour);

                if (theColour.ToArgb() == colourToConvert.ToArgb())
                    txtNamed.Text = theColour.Name;
            }

            txtNamed.Style["margin-right"] = "10px";
            aCell2.Controls.Add(txtNamed);

            aRow.Cells.Add(aCell2);
            aRow.Cells.Add(new TableCell {Text = "R: "});

            var aCell4 = new TableCell();
            var txtR = new TextBox {Width = 20};
            txtR.Style["text-align"] = "center";
            txtR.Text = Convert.ToString(colourToConvert.R);
            aCell4.Controls.Add(txtR);
            aRow.Cells.Add(aCell4);

            aRow.Cells.Add(new TableCell {Text = "G: "});

            var aCell6 = new TableCell();
            var txtG = new TextBox {Width = 20};
            txtG.Style["text-align"] = "center";
            txtG.Text = Convert.ToString(colourToConvert.G);
            aCell6.Controls.Add(txtG);
            aRow.Cells.Add(aCell6);

            aRow.Cells.Add(new TableCell {Text = "B: "});

            var aCell8 = new TableCell();
            var txtB = new TextBox {Width = 20};
            txtB.Style["text-align"] = "center";
            txtB.Text = Convert.ToString(colourToConvert.B);
            aCell8.Controls.Add(txtB);
            aRow.Cells.Add(aCell8);

            var aCell9 = new TableCell();
            var txtColour = new TextBox
                {
                    Width = 30,
                    BackColor = colourToConvert,
                    BorderColor = Color.Black,
                    BorderStyle = BorderStyle.Solid,
                    BorderWidth = 1
                };
            txtColour.Style["margin-left"] = "10px";
            aCell9.Controls.Add(txtColour);
            aRow.Cells.Add(aCell9);


            aTable.Rows.Add(aRow);

            this.phHexColourConvert.Controls.Add(aTable);
        }

        protected void cmdConvertRGBColour_Click(object sender, ImageClickEventArgs e)
        {
            var colourToConvert = Color.FromArgb(Convert.ToInt32(Request["ctl00$cphMain$txtColourR"]), Convert.ToInt32(Request["ctl00$cphMain$txtColourG"]), Convert.ToInt32(Request["ctl00$cphMain$txtColourB"]));

            var aTable = new Table();
            aTable.Style["margin-left"] = "15px";
            aTable.Style["margin-top"] = "4px";

            var aRow = new TableRow();

            aRow.Cells.Add(new TableCell {Text = "Hex: "});

            var aCell2 = new TableCell();
            var txtHex = new TextBox
                {
                    Width = 120,
                    Text = "#" + colourToConvert.ToArgb().ToString("X").Substring(2)
                };
            txtHex.Style["margin-right"] = "10px";
            aCell2.Controls.Add(txtHex);

            aRow.Cells.Add(aCell2);

            aRow.Cells.Add(new TableCell {Text = "Named: "});

            var aCell4 = new TableCell();
            var txtNamed = new TextBox {Width = 120, Text = "Unknown"};

            foreach (var aColour in Enum.GetNames(typeof(KnownColor)))
            {
                var theColour = Color.FromName(aColour);

                if (theColour.ToArgb() == colourToConvert.ToArgb())
                    txtNamed.Text = theColour.Name;
            }

            txtNamed.Style["margin-right"]= "10px";
            aCell4.Controls.Add(txtNamed);
            aRow.Cells.Add(aCell4);

            var aCell9 = new TableCell();
            var txtColour = new TextBox
                {
                    Width = 30,
                    BackColor = colourToConvert,
                    BorderColor = Color.Black,
                    BorderStyle = BorderStyle.Solid,
                    BorderWidth = 1
                };
            txtColour.Style["margin-left"] = "10px";
            aCell9.Controls.Add(txtColour);
            aRow.Cells.Add(aCell9);

            aTable.Rows.Add(aRow);

            this.phRGBColourConvert.Controls.Add(aTable);
        }
    }
}