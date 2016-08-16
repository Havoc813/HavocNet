using System;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using Core.Repositories;
using Phoenix.Core.Servers;

namespace HavocNet.Web.Administration
{
    public partial class Filters : Page
    {
        private HavocNetMaster _myMaster;
        private UserRepository _aRepository;
        private MenuItemRepository _aMenuItemRepository;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainAdministration");

            _aRepository = new UserRepository(new HavocServer());
            _aMenuItemRepository = new MenuItemRepository(new HavocServer(), _myMaster.aUser);

            this.lblTitle.Text = @"User Filters";

            LoadPage();

            this.cmdUpdate.Click += cmdUpdate_click;
        }

        private void LoadPage()
        {
            BindUsers();

            BindUserAccess();
        }

        private void BindUserAccess()
        {
            var aTree = new FSTree();

            var aUser = _aRepository.Get(int.Parse(this.cboUsers.SelectedValue));

            aTree.AddDropDownNode("root", "Regatta", "Regatta", "MainRegatta.aspx_", MakeOptions(aUser.GetAcccess("MainRegatta.aspx_")));
            aTree.AddDropDownNode("Regatta", "Events", "Events", "MainEvents.aspx_", MakeOptions(aUser.GetAcccess("MainEvents.aspx_")));
            aTree.AddDropDownNode("Regatta", "Series", "Series", "MainSeries.aspx_", MakeOptions(aUser.GetAcccess("MainSeries.aspx_")));
            aTree.AddDropDownNode("Regatta", "Maintain", "Maintain", "MainMaintain.aspx_", MakeOptions(aUser.GetAcccess("MainMaintain.aspx_")));
            
            aTree.AddDropDownNode("root", "Summits", "Summits", "MainSummits.aspx_", MakeOptions(aUser.GetAcccess("MainSummits.aspx_")));
            aTree.AddDropDownNode("Summits", "Maps", "Maps", "MainMaps.aspx_", MakeOptions(aUser.GetAcccess("MainMaps.aspx_")));
            aTree.AddDropDownNode("Summits", "Mountains", "Mountains", "MainMountains.aspx_", MakeOptions(aUser.GetAcccess("MainMountains.aspx_")));
            aTree.AddDropDownNode("Summits", "Trips", "Trips", "MainTrips.aspx_", MakeOptions(aUser.GetAcccess("MainTrips.aspx_")));

            aTree.AddDropDownNode("root", "HavocNet_MainPuzzle", "Puzzler", "MainPuzzle.aspx_", MakeOptions(aUser.GetAcccess("MainPuzzle.aspx_")));
            MakeSubMenues(aTree, aUser, "MainPuzzle", "HavocNet");

            aTree.AddDropDownNode("root", "Athletica", "Athletica", "Athletica/MainAthletica.aspx_", MakeOptions(aUser.GetAcccess("Athletica/MainAthletica.aspx_")));
            aTree.AddDropDownNode("Athletica", "Athletica_MainCapture", "Capture", "Athletica/MainCapture.aspx_", MakeOptions(aUser.GetAcccess("Athletica/MainCapture.aspx_")));
            aTree.AddDropDownNode("Athletica", "Athletica_MainDashboard", "Dashboard", "Athletica/MainDashboard.aspx_", MakeOptions(aUser.GetAcccess("Athletica/MainDashboard.aspx_")));
            aTree.AddDropDownNode("Athletica", "Athletica_MainAnalyse", "Analyse", "Athletica/MainAnalyse.aspx_", MakeOptions(aUser.GetAcccess("Athletica/MainAnalyse.aspx_")));
            aTree.AddDropDownNode("Athletica", "Athletica_MainTools", "Tools", "Athletica/MainTools.aspx_", MakeOptions(aUser.GetAcccess("Athletica/MainTools.aspx_")));
            MakeSubMenues(aTree, aUser, "MainTools", "Athletica");

            aTree.AddDropDownNode("root", "Financials", "Financials", "MainFinancials.aspx_", MakeOptions(aUser.GetAcccess("MainFinancials.aspx_")));
            aTree.AddDropDownNode("Financials", "Upload", "Upload", "MainUpload.aspx_", MakeOptions(aUser.GetAcccess("MainUpload.aspx_")));
            aTree.AddDropDownNode("Financials", "Detail", "Detail", "MainDetail.aspx_", MakeOptions(aUser.GetAcccess("MainDetail.aspx_")));
            aTree.AddDropDownNode("Financials", "Analyse", "Analyse", "MainAnalyse.aspx_", MakeOptions(aUser.GetAcccess("MainAnalyse.aspx_")));
            aTree.AddDropDownNode("Financials", "Maintain", "Maintain", "MainMaintain.aspx_", MakeOptions(aUser.GetAcccess("MainMaintain.aspx_")));

            aTree.AddDropDownNode("root", "HavocNet_MainAdministration", "Administration", "MainAdministration.aspx_", MakeOptions(aUser.GetAcccess("MainAdministration.aspx_")));
            MakeSubMenues(aTree, aUser, "MainAdministration", "HavocNet");

            FilterTree.Text = aTree.GetList;
        }

        private void MakeSubMenues(FSTree aTree, User aUser, string name, string system)
        {
            foreach (var aMenuItem in _aMenuItemRepository.GetMenuItems(system + "_" + name, "", ""))
            {
                aTree.AddDropDownNode(aMenuItem.Value.NavMenu, "MI" + aMenuItem.Key.ToString(""), aMenuItem.Value.Name, aMenuItem.Value.Identifier, MakeOptions(aUser.GetAcccess(aMenuItem.Value.Identifier)));

                foreach (var aSubMenuItem in _aMenuItemRepository.GetSubMenuItems(aMenuItem.Value.Page, aMenuItem.Value.Params, "", ""))
                {
                    aTree.AddDropDownNode("MI" + aMenuItem.Key.ToString(""), "SMI" + aSubMenuItem.Key.ToString(""), aSubMenuItem.Value.Name, aSubMenuItem.Value.Identifier, MakeOptions(aUser.GetAcccess(aSubMenuItem.Value.Identifier)));
                }
            }
        }

        private string MakeOptions(int access)
        {
            return "0;-;" + (access == 0 ? "selected" : "") + "|" +
                   "1;Read;" + (access == 1 ? "selected" : "") + "|" +
                   "2;Write;" + (access == 2 ? "selected" : "") + "";
        }

        private void BindUsers()
        {
            this.cboUsers.Items.Clear();

            this.cboUsers.Items.Add(new ListItem("public", "0"));

            foreach (var aUser in _aRepository.GetAll().Values)
            {
                this.cboUsers.Items.Add(new ListItem(aUser.FullName, aUser.ID.ToString("")));
            }

            if (!Page.IsPostBack)
            {
                if(cboUsers.Items.Count > 0 ) this.cboUsers.Items[0].Selected = true;
            }
            else
            {
                if (cboUsers.Items.FindByValue(Request["ctl00$cphMain$cboUsers"]) != null)
                {
                    this.cboUsers.Items.FindByValue(Request["ctl00$cphMain$cboUsers"]).Selected = true;
                }
                else
                {
                    if (cboUsers.Items.Count > 0) this.cboUsers.Items[0].Selected = true;
                }
            }
        }

        private void cmdUpdate_click(object sender, ImageClickEventArgs e)
        {
            foreach (var strAccess in Request["ctl00$cphMain$hidUserAccess"].Split(';').Where(strAccess => strAccess != ""))
            {
                _aMenuItemRepository.Update(int.Parse(Request["ctl00$cphMain$cboUsers"]), strAccess.Split('|')[1], strAccess.Split('|')[0]);
            }

            if (int.Parse(Request["ctl00$cphMain$cboUsers"]) == _myMaster.aUser.ID)
            {
                _myMaster.LoadPage("MainAdministration");
            }

            this.lblStatus.Text = "Filters successfully updated";
            this.lblStatus.ForeColor = Color.Green;

            LoadPage();
        }
    }
}