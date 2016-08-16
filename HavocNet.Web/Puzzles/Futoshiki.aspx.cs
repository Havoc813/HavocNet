using System;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using HavocNet.Puzzles;

namespace HavocNet.Web.Puzzles
{
    public partial class FutoshikiPage : Page
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
            this.cboOrder.SelectedIndexChanged += cboOrder_SelectedIndexChanged;

            MessageHelper.SetLabel(ref lblStatus, "", Color.Green);

            this.phPuzzle.Controls.Clear();

            BindOrder();

            if (!Page.IsPostBack) LoadPage();
        }

        private void LoadPage()
        {
            var aSuDoku = new Futoshiki(
                "", 
                new int[9,9], 
                int.Parse(this.cboOrder.SelectedValue),
                this.chkUseSuDokuConditions.Checked
                );
            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private void cmdDemo_Click(object sender, ImageClickEventArgs e)
        {
            var aDemo = new FutoshikiDemo();
            var aSuDoku = new Futoshiki(aDemo.GTLTConditions(),aDemo.DemoConditions,7,false);

            this.hidInitialConditions.Value = aDemo.GTLTConditions();

            this.cboOrder.ClearSelection();
            this.cboOrder.Items.FindByValue("7").Selected = true;

            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private void cmdClear_Click(object sender, ImageClickEventArgs e)
        {
            LoadPage();
        }

        private void cmdSolve_Click(object sender, ImageClickEventArgs e)
        {
            var aSuDoku = new Futoshiki(
                Request["ctl00$cphMain$hidInitialConditions"],
                SuDoku.MakeInitialConditions(int.Parse(this.cboOrder.SelectedValue)),
                int.Parse(this.cboOrder.SelectedValue),
                this.chkUseSuDokuConditions.Checked, 
                "solved"
                );
            aSuDoku.Solve();

            if (aSuDoku.SuDokuStatus == "errored")
                MessageHelper.SetLabel(ref lblStatus, "<span style='color:black; font-weight:bold'>Solve Results:</span><p>An error has occurred during puzzle solution. </p>" + aSuDoku.Error, Color.Red);
            
            this.phPuzzle.Controls.Add(aSuDoku.Render());
        }

        private void BindOrder()
        {
            this.cboOrder.Items.Clear();
            this.cboOrder.Items.Add(new ListItem("5"));
            this.cboOrder.Items.Add(new ListItem("6"));
            this.cboOrder.Items.Add(new ListItem("7"));
            this.cboOrder.Items.Add(new ListItem("8"));
            this.cboOrder.Items.Add(new ListItem("9"));

            if (!Page.IsPostBack)
            {
                this.cboOrder.Items[0].Selected = true;
            }
            else
            {
                if (this.cboOrder.Items.FindByValue(Request["ctl00$cphMain$cboOrder"]) != null)
                {
                    this.cboOrder.Items.FindByValue(Request["ctl00$cphMain$cboOrder"]).Selected = true;
                }
                else
                {
                    this.cboOrder.Items[0].Selected = true;
                }
            }
            
            this.chkUseSuDokuConditions.Visible = (this.cboOrder.SelectedValue == "9");
        }

        private void cboOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPage();
        }
    }
}