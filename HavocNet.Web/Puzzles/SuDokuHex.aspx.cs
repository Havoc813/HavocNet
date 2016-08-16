using System;
using System.Drawing;
using System.Web.UI;
using Core;
using HavocNet.Puzzles;

namespace HavocNet.Web.Puzzles
{
    public partial class SuDokuHexPage : Page
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

            MessageHelper.SetLabel(ref lblStatus,"", Color.Green);

            this.phPuzzle.Controls.Clear();
            if (!Page.IsPostBack) LoadPage();
        }

        private void LoadPage()
        {
            var aSuDoku = new SuDokuHex(new int[16,16]);
            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private void cmdDemo_Click(object sender, ImageClickEventArgs e)
        {
            var aDemo = new HexDemo();
            var aSuDoku = new SuDokuHex(aDemo.DemoConditions,"demo");
            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private void cmdClear_Click(object sender, ImageClickEventArgs e)
        {
            var aSuDoku = new SuDokuHex(new int[16, 16]);
            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private void cmdSolve_Click(object sender, ImageClickEventArgs e)
        {
            var aSuDoku = new SuDokuHex(SuDoku.MakeInitialConditions(16), "solved");
            aSuDoku.Solve();

            if (aSuDoku.SuDokuStatus == "errored")
                MessageHelper.SetLabel(ref lblStatus,"<span style='color:black; font-weight:bold'>Solve Results:</span><p>An error has occurred during puzzle solution. </p>" + aSuDoku.Error, Color.Red);
            
            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }
    }
}