using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using HavocNet.Puzzles;

namespace HavocNet.Web.Puzzles
{
    public partial class Polynomial : Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainPuzzle");

            this.cmdSolve.Click += cmdSolve_Click;
            this.cmdDemo.Click += cmdDemo_Click;
            this.cboDegree.SelectedIndexChanged += cboDegree_SelectedIndexChanged;

            MessageHelper.SetLabel(ref lblStatus, "", Color.Green);

            BindDegree();
            BindDescription();
            BindImage();
            BindSolutionMethod();

            this.phPuzzleEntry.Controls.Clear();
            this.phPuzzleEntry.Controls.Add(MakePuzzleEntry());
        }

        private void BindDescription()
        {
            this.lblDescription.Text = @"Polynomial equations of order " + this.cboDegree.SelectedItem.Value + @" have the form:";
            this.lblInstructions.Text = @"Please enter the values for the " + this.cboDegree.SelectedItem.Value + 1 + @" coefficients below:";
            this.lblDescription2.Text = "";
        }

        private void BindImage()
        {
            this.imgForm.ImageUrl = @"~\App_Themes\HavocNet\Images\Maths\PolynomialOrder" + this.cboDegree.SelectedItem.Value + ".png";
        }

        private void BindSolutionMethod()
        {
            var lbl = new Label{ Text = @"Order " + this.cboDegree.SelectedValue + @" polynomials have solutions defined by:<br />" };

            var img = new System.Web.UI.WebControls.Image { ImageUrl = "~\\App_Themes\\HavocNet\\Images\\Maths\\PolynomialOrder2Solution.png" };
            img.Style["margin-top"] = "10px";
            img.Style["margin-left"] = "5px";

            this.phSolutionMethod.Controls.Clear();

            if (this.cboDegree.SelectedValue == "2")
            {
                this.phSolutionMethod.Controls.Add(lbl);
                this.phSolutionMethod.Controls.Add(img);
            }
        }

        private Table MakePuzzleEntry()
        {
            var aTable = new Table();
            aTable.Style["margin-left"] = "5px";

            var aRow = new TableRow();

            for (var i = int.Parse(this.cboDegree.SelectedItem.Value); i >= 0; i += -1)
            {
                var aCell1 = new TableCell();
                var txt = new TextBox {Width = 20};
                txt.Style["text-align"] = "center";
                txt.Style["margin-right"] = "3px";
                txt.ID = "txt" + Convert.ToString(int.Parse(this.cboDegree.SelectedItem.Value) - i + 1);
                aCell1.Controls.Add(txt);

                var aCell2 = new TableCell {Text = (char)(97 + int.Parse(this.cboDegree.SelectedItem.Value) - i) + @" ="};

                aRow.Cells.Add(aCell2);
                aRow.Cells.Add(aCell1);
            }

            aTable.Rows.Add(aRow);

            return aTable;
        }

        private Table MakePuzzleSolution(string[] solution)
        {
            this.lblDescription2.Text = @"The solutions to this order " + this.cboDegree.SelectedValue + @" polynomial with resepect to x are as follows:";

            var factorisable = false;

            var aTable = new Table();
            
            for (var i = 0; i <= int.Parse(this.cboDegree.SelectedValue) - 1; i++)
            {
                var aRow2 = new TableRow();
                var aCell2 = new TableCell { Text = @"<span style='font-family:Times New Roman; font-size:14px; font-style:italic'>x</span> = " + solution[i] };
                aCell2.Style["padding-left"] = "10px";
                aRow2.Cells.Add(aCell2);
                aTable.Rows.Add(aRow2);

                int j;
                if (int.TryParse(solution[i], out j) && Convert.ToInt32(solution[i]) - Convert.ToDouble(solution[i]) == 0)
                {
                    factorisable = true;
                }
                else
                {
                    factorisable = false;
                }
            }

            if (factorisable)
            {
                var aRow3 = new TableRow();
                var aCell3 = new TableCell();
                aCell3.Style["padding-left"] = "10px";
                aCell3.Text = @"[factorises ";

                for (var i = 0; i <= int.Parse(this.cboDegree.SelectedValue) - 1; i++)
                {
                    if (int.Parse(solution[0]) > 0)
                    {
                        aCell3.Text += @"(<span style='font-family:Times New Roman; font-size:14px; font-style:italic'>x</span> - " + Convert.ToString(Convert.ToInt32(solution[i])) + @")";
                    }
                    else
                    {
                        aCell3.Text += @"(<span style='font-family:Times New Roman; font-size:14px; font-style:italic'>x</span> + " + Convert.ToString(Convert.ToInt32(solution[i])) + @")";
                    }
                }

                aCell3.Text += @" = 0]";

                aRow3.Cells.Add(aCell3);
                aTable.Rows.Add(aRow3);
            }

            return aTable;
        }

        private void BindDegree()
        {
            this.cboDegree.Items.Clear();
            this.cboDegree.Items.Add(new ListItem("Order 2", "2"));
            this.cboDegree.Items.Add(new ListItem("Order 3", "3"));
            this.cboDegree.Items.Add(new ListItem("Order 4", "4"));

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
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[1].Controls[0]).Text = @"1";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[3].Controls[0]).Text = @"-3";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[5].Controls[0]).Text = @"2";
                    break;
                case "3":
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[1].Controls[0]).Text = @"1";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[3].Controls[0]).Text = @"9";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[5].Controls[0]).Text = @"26";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[7].Controls[0]).Text = @"24";
                    break;
                case "4":
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[1].Controls[0]).Text = @"2";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[3].Controls[0]).Text = @"3";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[5].Controls[0]).Text = @"4";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[7].Controls[0]).Text = @"5";
                    ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[9].Controls[0]).Text = @"6";
                    break;
            }
        }

        protected void cmdSolve_Click(object sender, ImageClickEventArgs e)
        {
            switch (this.cboDegree.SelectedValue)
            {
                case "2":
                    var aQuadratic = new QuadraticSolve(double.Parse(Request["ctl00$cphMain$txt1"]), double.Parse(Request["ctl00$cphMain$txt2"]), double.Parse(Request["ctl00$cphMain$txt3"]));
                    aQuadratic.QuadSolveStandard();

                    this.phPuzzleSolution.Controls.Clear();
                    this.phPuzzleSolution.Controls.Add(MakePuzzleSolution(aQuadratic.SolutionsFormatted));
                    break;
                case "3":
                    var aCubic = new CubicSolve(double.Parse(Request["ctl00$cphMain$txt1"]), double.Parse(Request["ctl00$cphMain$txt2"]), double.Parse(Request["ctl00$cphMain$txt3"]), double.Parse(Request["ctl00$cphMain$txt4"]));
                    aCubic.SolveStandard();

                    this.phPuzzleSolution.Controls.Clear();
                    this.phPuzzleSolution.Controls.Add(MakePuzzleSolution(aCubic.SolutionsFormatted));
                    break;
                case "4":
                    SetStatus("Quartic Equation Solver not yet implemented", Color.Red);
                    break;
                default:
                    SetStatus("Unknown polynomial order!", Color.Red);
                    break;
            }
        }

        private void SetStatus(string strMessage, Color aColour)
        {
            this.lblStatus.Text = strMessage;
            this.lblStatus.ForeColor = aColour;
        }

        protected void cboDegree_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[1].Controls[0]).Text = "";
            ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[3].Controls[0]).Text = "";
            ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[5].Controls[0]).Text = "";
            if (((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells.Count > 6)
                ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[7].Controls[0]).Text = "";
            if (((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells.Count > 8)
                ((TextBox)((Table)this.phPuzzleEntry.Controls[0]).Rows[0].Cells[9].Controls[0]).Text = "";

            SetStatus("", Color.Green);
        }
    }
}