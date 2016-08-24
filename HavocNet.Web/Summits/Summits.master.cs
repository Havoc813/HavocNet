using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Core;
using Core.Repositories;

namespace HavocNet.Web.Summits
{
    public partial class Summits : MasterPage
    {
        public User aUser;
        private readonly HavocServer aServer = new HavocServer();
        private readonly HavocLogin aLogin = new HavocLogin();
        private MenuItemRepository aRepository;
        public int PageAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void LoadPage(string navMenuName)
        {
            LoadPageNoAuth(navMenuName);

            PageAccess = aUser.GetAcccess(URLHelper.GetPageIdentifier(Page.Request.Url));

            if (PageAccess == 0) Response.Redirect(@"~\Errors\NoAccess.aspx?aspxerrorpath=" + Request.Url.PathAndQuery);
        }

        public void LoadPageNoAuth(string navMenuName)
        {
            Response.AddHeader("pragma", "no-cache");
            Response.AddHeader("cache-control", "private");
            Response.CacheControl = "no-cache";

            aUser = aLogin.FetchUser(aServer);

            BindButtons();

            aRepository = new MenuItemRepository(aServer, aUser);

            BindNavMenu(navMenuName);

            SetVersion();
            SetUserID();
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

        public void SetVersion()
        {
            this.hidVersion.Value = aUser.Version.ToString(CultureInfo.InvariantCulture);
        }

        public void SetUserID()
        {
            this.hidUserID.Value = aUser.ID.ToString(CultureInfo.InvariantCulture);
        }
    }
}
