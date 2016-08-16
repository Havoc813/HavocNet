using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using Core;
using HavocNet.Puzzles;

namespace HavocNet.Web.Puzzles
{
    public partial class SuDokuKillerPage : Page
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
            this.phPuzzle.Controls.Add(new SuDokuKiller(new List<KillerHelper>()).Render());
        }

        private void cmdDemo_Click(object sender, ImageClickEventArgs e)
        {
            var aDemo = new KillerDemo();
            var aSuDoku = new SuDokuKiller(aDemo.KillerConditions);

            this.hidInitialConditions.Value = aSuDoku.MakeKillerHidden();
            this.hidConditionCount.Value = "81";

            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private void cmdClear_Click(object sender, ImageClickEventArgs e)
        {
            this.phPuzzle.Controls.Add(new SuDokuKiller(new List<KillerHelper>()).Render());
        }

        private void cmdSolve_Click(object sender, ImageClickEventArgs e)
        {
            var aSuDokuOrig = new SuDokuKiller(MakeKillerConditions());
            this.phPuzzle.Controls.Add(aSuDokuOrig.Render());

            var aSuDoku = new SuDokuKiller(MakeKillerConditions(), "solved");
            aSuDoku.Solve();

            if (aSuDoku.SuDokuStatus == "errored")
                MessageHelper.SetLabel(ref lblStatus, "<span style='color:black; font-weight:bold'>Solve Results:</span><p>An error has occurred during puzzle solution. </p>" + aSuDoku.Error, Color.Red);
            
            this.phPuzzle2.Controls.Add(aSuDoku.Render());

            this.imgNext.Visible = true;
        }

        private List<KillerHelper> MakeKillerConditions()
        {
            var killerConditions = new List<KillerHelper>();

            foreach (var aElement in Request["ctl00$cphMain$hidInitialConditions"].Split('&'))
            {
                if (string.IsNullOrEmpty(aElement)) continue;

                var aKillerHelper = new KillerHelper(int.Parse(aElement.Split(',')[1]));

                foreach (var aIJPair in aElement.Split(',')[0].Split('|').Where(aIJPair => !string.IsNullOrEmpty(aIJPair)))
                {
                    aKillerHelper.ijPairs.Add(new [] { int.Parse(aIJPair.Split(';')[0]), int.Parse(aIJPair.Split(';')[1]) });
                }

                killerConditions.Add(aKillerHelper);
            }

            return killerConditions;
        }
    }
}