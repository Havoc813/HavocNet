using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Athletica;
using Athletica.Repositories;
using Core;
using Core.Repositories;
using Phoenix.Core.Logging;
using MenuItem = Core.MenuItem;

namespace HavocNet.Web.Athletica
{
    public partial class Athletica : MasterPage
    {
        public User aUser;
        private readonly HavocServer aServer = new HavocServer();
        private readonly HavocLogin aLogin = new HavocLogin();
        private MenuItemRepository aRepository;
        public int PageAccess;
        public int SelectedSport;

        public bool WriteAccess
        {
            get { return (PageAccess == 2); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void LoadPageMenus(string navMenuName)
        {
            var pageName = Page.Request.Url.PathAndQuery;
            if (pageName.IndexOf("/Main", StringComparison.Ordinal) >= 0)
            {
                BindMenu(navMenuName, "", "");
            }
            else if (pageName.IndexOf("/Menu", StringComparison.Ordinal) >= 0)
            {
                BindSubMenu(navMenuName, URLHelper.GetPageFromURL(Page.Request.Url), URLHelper.GetQueryFromURL(Page.Request.Url), "", "");
            }
            else
            {
                var menuName = aRepository.GetPageName(URLHelper.GetPageFromURL(Page.Request.Url), URLHelper.GetQueryFromURL(Page.Request.Url));

                if (menuName != "") BindSubMenu(navMenuName, menuName.Split('_')[0], menuName.Split('_')[1], URLHelper.GetPageFromURL(Page.Request.Url), URLHelper.GetQueryFromURL(Page.Request.Url));
            }
        }

        public void LoadPage(string navMenuName, bool withMenus = true)
        {
            LoadPageNoAuth(navMenuName, withMenus);

            PageAccess = aUser.GetAcccess(URLHelper.GetPageIdentifier(Page.Request.Url));

            if (PageAccess == 0) Response.Redirect(@"~\Errors\NoAccess.aspx?aspxerrorpath=" + Request.Url.PathAndQuery);
        }

        public void LoadPageNoAuth(string navMenuName, bool withMenus)
        {
            Response.AddHeader("pragma", "no-cache");
            Response.AddHeader("cache-control", "private");
            Response.CacheControl = "no-cache";

            aUser = aLogin.FetchUser(aServer);

            BindButtons();

            aRepository = new MenuItemRepository(aServer, aUser);

            BindNavMenu(navMenuName);

           if(withMenus) LoadPageMenus(navMenuName);
        }

        private void BindButtons()
        {
            this.lblUser.Text = aUser.FirstName;
            this.cmdAccount.Visible = (aUser.Username != "public");
            this.cmdLogout.Visible = (aUser.Username != "public");
            this.cmdLogin.Visible = (aUser.Username == "public");
            this.cmdSignUp.Visible = (aUser.Username == "public");
        }

        private void BindNavMenu(string aNavMenu)
        {
            foreach (HtmlGenericControl menuCtrl in navMenu.Controls.OfType<HtmlGenericControl>())
            {
                ((HyperLink)menuCtrl.Controls[1]).CssClass = menuCtrl.ID == "cmd" + aNavMenu ? "on" : "";

                menuCtrl.Visible = (aUser.GetAcccess(menuCtrl.ID.Replace("cmd","") + ".aspx_") > 0);
            }
        }

        private void BindMenu(string navMenuName, string menuPage, string menuParams)
        {
            this.phMenu.Controls.Clear();
            foreach (var aItem in aRepository.GetMenuItems("Athletica_" + navMenuName, menuPage, menuParams).Values)
            {
                if(aUser.GetAcccess(aItem.Identifier) > 0) this.phMenu.Controls.Add(aItem.MakeMenuRow());
            }

            if (this.phMenu.Controls.Count > 0) this.tblMenu.Attributes["class"] = "menuContainer";
        }

        private void BindSubMenu(string navMenuName, string menuPage, string menuParams, string subPageName, string subPageParams)
        {
            BindMenu(navMenuName, menuPage, menuParams);

            this.phSubMenu.Controls.Clear();
            foreach (var aItem in aRepository.GetSubMenuItems(menuPage, menuParams, subPageName, subPageParams).Values)
            {
                if (aUser.GetAcccess(aItem.Identifier) > 0) this.phSubMenu.Controls.Add(aItem.MakeSubMenuRow());
            }
        }

        public void BindTypeMenu(string navMenuName, string menuPage, string menuParams, string description)
        {
            this.phMenu.Controls.Clear();
            foreach (var aItem in aRepository.GetTypeMenus(new AthleticaServer(), "Athletica_" + navMenuName, menuPage, menuParams, description).Values)
            {
                if (aUser.GetAcccess(aItem.Identifier) > 0) this.phMenu.Controls.Add(aItem.MakeMenuRow());
            }

            if (this.phMenu.Controls.Count > 0) this.tblMenu.Attributes["class"] = "menuContainer";
        }

        public void BindAnalyse(string menuPage, string menuParams)
        {
            this.phSubMenu.Controls.Add(
                new SubMenuItem("MainAnalyse","", "Data", @"Athletica\DataAnalysis.aspx", menuParams, (menuPage == @"Athletica\DataAnalysis.aspx")).MakeSubMenuRow()
                );
            this.phSubMenu.Controls.Add(
                new SubMenuItem("MainAnalyse", "", "Graphs", @"Athletica\GraphicAnalysis.aspx", menuParams, (menuPage == @"Athletica\GraphicAnalysis.aspx")).MakeSubMenuRow()
                );
        }

        public void BindDashboard(string menuPage, string menuParams)
        {
            this.phSubMenu.Controls.Add(
                new SubMenuItem("MainDashboard", "", "Activity Feed", @"Athletica\ActivityFeed.aspx", menuParams, (menuPage == @"Athletica\ActivityFeed.aspx")).MakeSubMenuRow()
                );
            this.phSubMenu.Controls.Add(
                new SubMenuItem("MainDashboard", "", "Calendar", @"Athletica\Calendar.aspx", menuParams, (menuPage == @"Athletica\Calendar.aspx")).MakeSubMenuRow()
                );
        }

        public void ClearMenus()
        {
            this.tblMenu.Style["display"] = "none";
        }

        public void Audit(string description, string data)
        {
            var audit = new FSAudit(this.aServer, this.aUser.Username, description, data);
            audit.Create();
        }

        public void BindSports()
        {
            var athleticaServer = new AthleticaServer();
            athleticaServer.Open();

            athleticaServer.SQLParams.Add(this.aUser.ID);
            athleticaServer.SQLParams.Add("All");

            var menuPage = URLHelper.GetPageIdentifier(Page.Request.Url);
            var first = URLHelper.GetQueryFromURL(Request.Url) == "";

            foreach (var sport in new SportRepository().GetByUser(athleticaServer))
            {
                this.phMenu.Controls.Add(
                    new MenuItem("MainCapture", sport.Value, @"Athletica\MainCapture.aspx", "Record a Workout", "sport=" + sport.Key, first || (menuPage == @"Athletica/MainCapture.aspx_sport=" + sport.Key)).MakeMenuRow()
                    );
                if (first || (menuPage == @"Athletica/MainCapture.aspx_sport=" + sport.Key)) SelectedSport = sport.Key;

                first = false;
            }

            athleticaServer.Close();

            if (this.phMenu.Controls.Count > 0) this.tblMenu.Attributes["class"] = "menuContainer";
        }
    }
}
