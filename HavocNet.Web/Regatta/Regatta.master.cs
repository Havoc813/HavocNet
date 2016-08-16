using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Core;
using Core.Repositories;

namespace HavocNet.Web.Regatta
{
    public partial class Regatta : MasterPage
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

            var pageName = Page.Request.Url.PathAndQuery;
            if (pageName.IndexOf("/Main", StringComparison.Ordinal) >= 0)
            {
                BindMenu(navMenuName, "", "");
            }
            else if (pageName.IndexOf("/Menu", StringComparison.Ordinal) >= 0)
            {
                BindSubMenu(navMenuName, URLHelper.GetPageFromURL(Page.Request.Url), URLHelper.GetQueryFromURL(Page.Request.Url),"","");
            }
            else
            {
                var menuName = aRepository.GetPageName(URLHelper.GetPageFromURL(Page.Request.Url), URLHelper.GetQueryFromURL(Page.Request.Url));

                if(menuName != "") BindSubMenu(navMenuName, menuName.Split('_')[0], menuName.Split('_')[1], URLHelper.GetPageFromURL(Page.Request.Url), URLHelper.GetQueryFromURL(Page.Request.Url));
            }
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
            foreach (var aItem in aRepository.GetMenuItems(navMenuName, menuPage, menuParams).Values)
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

        public void ClearMenus()
        {
            this.tblMenu.Style["display"] = "none";
        }

        public void ExportToExcel(string exportName, Table tbl)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + exportName + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/ms-excel";

            var stringWrite = new System.IO.StringWriter();
            var htmlWrite = new HtmlTextWriter(stringWrite);
            tbl.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());
            Response.End();
        }
    }
}
