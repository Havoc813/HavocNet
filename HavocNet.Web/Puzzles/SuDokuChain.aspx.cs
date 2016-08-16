using System;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using Core;
using HavocNet.Puzzles;

namespace HavocNet.Web.Puzzles
{
    public partial class SuDokuPolyPage : Page
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

            SetupRadios();

            if (!Page.IsPostBack) LoadPage();
        }

        private void SetupRadios()
        {
            this.rdoColours1.Checked = true;
            this.rdoColours1.Attributes["onclick"] = "javascript:document.getElementById('ctl00_cphMain_hidColour').value = '#7cfc00';";
            this.rdoColours2.Attributes["onclick"] = "javascript:document.getElementById('ctl00_cphMain_hidColour').value = '#dc143c';";
            this.rdoColours3.Attributes["onclick"] = "javascript:document.getElementById('ctl00_cphMain_hidColour').value = '#4682b4';";
            this.rdoColours4.Attributes["onclick"] = "javascript:document.getElementById('ctl00_cphMain_hidColour').value = '#ffff00';";
            this.rdoColours5.Attributes["onclick"] = "javascript:document.getElementById('ctl00_cphMain_hidColour').value = '#ee82ee';";
            this.rdoColours6.Attributes["onclick"] = "javascript:document.getElementById('ctl00_cphMain_hidColour').value = '#ff4500';";
            this.rdoColours7.Attributes["onclick"] = "javascript:document.getElementById('ctl00_cphMain_hidColour').value = '#00ffff';";
            this.rdoColours8.Attributes["onclick"] = "javascript:document.getElementById('ctl00_cphMain_hidColour').value = '#ffc0cb';";
            this.rdoColours9.Attributes["onclick"] = "javascript:document.getElementById('ctl00_cphMain_hidColour').value = '#daa520';";

            this.hidColour.Value = "#7cfc00";
        }

        private void LoadPage()
        {
            var aSuDoku = new SuDokuPoly(new int[9,9], new int[9,9]);
            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private void cmdDemo_Click(object sender, ImageClickEventArgs e)
        {
            var aDemo = new ChainDemo();
            var aSuDoku = new SuDokuPoly(aDemo.PolyConditions, aDemo.DemoConditions);

            this.hidPolyConditions.Value = aSuDoku.MakePolyHidden();

            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private void cmdClear_Click(object sender, ImageClickEventArgs e)
        {
            var aSuDoku = new SuDokuPoly(new int[9,9], new int[9,9]);
            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private void cmdSolve_Click(object sender, ImageClickEventArgs e)
        {
            var aSuDoku = new SuDokuPoly(MakePolyConditions(), SuDoku.MakeInitialConditions(9), "solved");
            aSuDoku.Solve();

            if (aSuDoku.SuDokuStatus == "errored")
                MessageHelper.SetLabel(ref lblStatus, "<span style='color:black; font-weight:bold'>Solve Results:</span><p>An error has occurred during puzzle solution. </p>" + aSuDoku.Error, Color.Red);
            
            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private int[,] MakePolyConditions()
        {
            var polyConditions = new int[9, 9];

            if (string.IsNullOrEmpty(Request["ctl00$cphMain$hidPolyConditions"])) return polyConditions;

            var i = 0;
            foreach (var aStr1 in Request["ctl00$cphMain$hidPolyConditions"].Split('|').Where(aStr1 => !string.IsNullOrEmpty(aStr1)))
            {
                var j = 0;
                foreach (var aStr2 in aStr1.Split(';').Where(aStr2 => !string.IsNullOrEmpty(aStr2)))
                {
                    polyConditions[i, j] = int.Parse(aStr2);
                    j++;
                }
                i++;
            }

            return polyConditions;
        }
    }
}