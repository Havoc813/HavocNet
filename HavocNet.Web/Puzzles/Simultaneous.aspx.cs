using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using HavocNet.Puzzles;

namespace HavocNet.Web.Puzzles
{
    public partial class SimultaneousPage : Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainPuzzle");

            this.cmdSolve.Click += cmdSolve_Click;
            this.cmdDemo.Click += cmdDemo_Click;

            MessageHelper.SetLabel(ref lblStatus, "", Color.Green);

            BindDegree();
            BindDescription();
            BindImage();

            this.phPuzzleEntry.Controls.Clear();
            this.phPuzzleEntry.Controls.Add(MakePuzzleEntry());
        }

        private void BindDescription()
        {
            this.lblDescription.Text = "Simultaneous equations in " + this.cboDegree.SelectedItem.Value + " variables have the form:";
            this.lblInstructions.Text = "Please enter the values for each coefficient below:";
            this.lblDescription2.Text = "";
        }

        private void BindImage()
        {
            if (int.Parse(this.cboDegree.SelectedValue) <= 4)
            {
                this.imgForm.ImageUrl = @"~\App_Themes\HavocNet\Images\Maths\SimultaneousOrder" + this.cboDegree.SelectedItem.Value + ".png";
            }
            else
            {
                this.imgForm.ImageUrl = @"~\App_Themes\HavocNet\Images\Maths\SimultaneousOrderN.png";
                this.lblDescription.Text = "Simultaneous equations in " + this.cboDegree.SelectedValue + " variables have the form below (n = " + this.cboDegree.SelectedValue + "):";
            }
        }

        private Table MakePuzzleEntry()
        {
            var aTable = new Table();

            for (var j = 1; j <= int.Parse(this.cboDegree.SelectedValue); j++)
            {
                var aRow = new TableRow();

                for (var i = 1; i <= int.Parse(this.cboDegree.SelectedValue); i++)
                {
                    var aCell1 = new TableCell();
                    var txt = new TextBox {Width = 30, ID = "txt" + i + j};
                    txt.Style["text-align"] = "center";
                    aCell1.Controls.Add(txt);
                    aRow.Cells.Add(aCell1);

                    aRow.Cells.Add(new TableCell {Text = "X" + i + " + "});
                }

                var aCell4 = new TableCell();
                var txt2 = new TextBox
                    {
                        Width = 30,
                        ID = "txt" + Convert.ToString(int.Parse(this.cboDegree.SelectedItem.Value) + 1) + j
                    };
                txt2.Style["text-align"] = "center";
                aCell4.Controls.Add(txt2);
                aRow.Cells.Add(aCell4);

                aRow.Cells.Add(new TableCell { Text = "= 0 " });

                aTable.Rows.Add(aRow);
            }

            return aTable;
        }

        private void BindDegree()
        {
            this.cboDegree.Items.Clear();
            this.cboDegree.Items.Add(new ListItem("Order 2", "2"));
            this.cboDegree.Items.Add(new ListItem("Order 3", "3"));
            this.cboDegree.Items.Add(new ListItem("Order 4", "4"));
            this.cboDegree.Items.Add(new ListItem("Order 5", "5"));
            this.cboDegree.Items.Add(new ListItem("Order 6", "6"));
            this.cboDegree.Items.Add(new ListItem("Order 7", "7"));
            this.cboDegree.Items.Add(new ListItem("Order 8", "8"));

            if (!Page.IsPostBack)
            {
                this.cboDegree.SelectedIndex = 0;
            }
            else
            {
                if ((Request["ctl00$cphMain$cboDegree"] != null))
                {
                    this.cboDegree.Items.FindByValue(Request["ctl00$cphMain$cboDegree"]).Selected = true;
                }
                else
                {
                    this.cboDegree.SelectedIndex = 0;
                }
            }
        }

        protected void cmdDemo_Click(object sender, ImageClickEventArgs e)
        {
            switch (this.cboDegree.SelectedValue)
            {
                case "2":
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[0].Controls[0]).Text = "2";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[2].Controls[0]).Text = "-2";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[4].Controls[0]).Text = "-3";

                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[1].Cells[0].Controls[0]).Text = "-7";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[1].Cells[2].Controls[0]).Text = "8";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[1].Cells[4].Controls[0]).Text = "-2";
                    break;
                case "3":
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[0].Controls[0]).Text = "1";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[2].Controls[0]).Text = "-3";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[4].Controls[0]).Text = "2";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[6].Controls[0]).Text = "2";

                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[1].Cells[0].Controls[0]).Text = "1";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[1].Cells[2].Controls[0]).Text = "-3";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[1].Cells[4].Controls[0]).Text = "2";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[1].Cells[6].Controls[0]).Text = "2";

                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[2].Cells[0].Controls[0]).Text = "1";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[2].Cells[2].Controls[0]).Text = "-3";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[2].Cells[4].Controls[0]).Text = "2";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[2].Cells[6].Controls[0]).Text = "2";
                    break;
            }
        }

        private void SetStatus(string strMessage, Color aColour)
        {
            this.lblStatus.Text = strMessage;
            this.lblStatus.ForeColor = aColour;
        }

        protected void cmdSolve_Click(object sender, ImageClickEventArgs e)
        {
            var aHelper = new Simultaneous();
            var aMatrix = new double[int.Parse(this.cboDegree.SelectedValue) + 1, int.Parse(this.cboDegree.SelectedValue) + 1];

            for (var j = 1; j <= aMatrix.GetUpperBound(0) + 1; j++)
            {
                for (var i = 1; i <= aMatrix.GetUpperBound(1) + 1; i++)
                {
                    if (Request["ctl00$cphMain$txt" + j + i] == null) continue;
                    aMatrix[j - 1, i - 1] = double.Parse(Request["ctl00$cphMain$txt" + j + i]);
                }
            }

            var aInverse = aHelper.MatrixGetInverse(aMatrix);

            var ansMatrix = aHelper.MatrixMultiply(aMatrix, aInverse);

            this.phPuzzleSolution.Controls.Add(MakeSolution(ansMatrix));
        }

        private Table MakeSolution(double[,] ansMatrix)
        {
            var tbl = new Table();
            var aRow = new TableRow();

            for (var i = 1; i <= int.Parse(this.cboDegree.SelectedValue); i++)
            {
                aRow.Cells.Add(new TableCell {Text = "X" + i + " = " + ansMatrix[1, i]});
            }

            tbl.Rows.Add(aRow);

            return tbl;
        }
    }
}