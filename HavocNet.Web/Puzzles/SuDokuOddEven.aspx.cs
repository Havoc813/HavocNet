using System;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using Core;
using HavocNet.Puzzles;

namespace HavocNet.Web.Puzzles
{
    public partial class SuDokuOddEvenPage : Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainPuzzle");

            this.cmdSolve.Click += cmdSolve_Click;
            this.cmdDemo.Click += cmdDemo_Click;
            this.cmdClear.Click += cmdClear_Click;

            MessageHelper.SetLabel(ref lblStatus, "", Color.Green);
            
            this.phPuzzle.Controls.Clear();

            if (!Page.IsPostBack) LoadPage();
        }

        private void LoadPage()
        {
            var aSuDoku = new SuDokuOddEven(new int[9, 9], new int[9, 9]);
            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private void cmdDemo_Click(object sender, ImageClickEventArgs e)
        {
            var aDemo = new OddEvenDemo();
            var aSuDoku = new SuDokuOddEven(aDemo.DemoConditions, aDemo.OddEvenConditions, "demo");

            this.hidOddCells.Value = aSuDoku.MakeOddEvenHidden();

            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private void cmdClear_Click(object sender, ImageClickEventArgs e)
        {
            var aSuDoku = new SuDokuOddEven(new int[9,9], new int[9,9]);
            this.phPuzzle.Controls.Add(aSuDoku.Render());

            MessageHelper.SetLabel(ref lblStatus, "", Color.Green);
        }

        private void cmdSolve_Click(object sender, ImageClickEventArgs e)
        {
            var aSuDoku = new SuDokuOddEven(SuDoku.MakeInitialConditions(9), MakeOddEvenConditions(), "solved");
            aSuDoku.Solve();

            if (aSuDoku.SuDokuStatus == "errored")
                MessageHelper.SetLabel(ref lblStatus, "<span style='color:black; font-weight:bold'>Solve Results:</span><p>An error has occurred during puzzle solution. </p>" + aSuDoku.Error, Color.Red);
            
            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private int[,] MakeOddEvenConditions()
        {
            var oddEvenConditions = new int[9, 9];

            if (!string.IsNullOrEmpty(Request["ctl00$cphMain$hidOddCells"]))
            {
                foreach (var aStr in Request["ctl00$cphMain$hidOddCells"].Split(';').Where(aStr => !string.IsNullOrEmpty(aStr)))
                {
                    oddEvenConditions[Convert.ToInt32(aStr.Substring(aStr.Length - 2, 2)), Convert.ToInt32(aStr.Substring(0, 2))] = 1;
                }
            }

            return oddEvenConditions;
        }
    }
}