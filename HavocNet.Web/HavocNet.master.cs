using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Core;
using Core.Repositories;
using Phoenix.Common.Excel;
using Phoenix.Core;
using Phoenix.Core.Tables;
using URLHelper = Core.URLHelper;

namespace HavocNet.Web
{
    public partial class HavocNetMaster : MasterPage
    {
        public User aUser;
        private readonly HavocServer aServer = new HavocServer();
        private readonly HavocLogin aLogin = new HavocLogin();
        private MenuItemRepository aRepository;
        public int PageAccess;
        public FSDataTable aFSDataTable = new FSDataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public bool WriteAccess()
        {
            return PageAccess == 2;
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
            foreach (var aItem in aRepository.GetMenuItems("HavocNet_" + navMenuName, menuPage, menuParams).Values)
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


        public void CreateExcel(string exportSheetName, bool filters = false)
        {
            CreateExcel(this.ExportFilename(), exportSheetName, filters);
        }

        private string CreateExcel(string exportFileName, string exportSheetName, bool filters = false)
        {
            var exportFile = Server.MapPath(@"~\App_Data\Exports\" + exportFileName);

            var excelDoc = new FSExcelDocument(exportFile);

            var excelSheet = new FSExcelWorksheet(excelDoc, exportSheetName, this.aFSDataTable.TableColumns);
            excelSheet.AddWorksheet(this.aFSDataTable.DT, filters);

            excelDoc.SaveDocument();

            return exportFile;
        }

        public void ExportToExcel(string exportSheetName, bool filters = false)
        {
            ExportToExcel(this.ExportFilename(), exportSheetName, filters);
        }

        public void ExportToExcel(string exportFileName, string exportSheetName, bool filters = false)
        {
            ModifyResponse(CreateExcel(exportFileName, exportSheetName, filters), exportFileName);
        }

        public void ModifyResponse(string exportFile, string exportFileName)
        {
            Response.AddHeader("content-disposition", "attachment;filename=" + exportFileName);
            Response.ContentType = "application/vnd.xls";
            Response.Charset = "";
            Response.TransmitFile(exportFile);
            Response.AddHeader("Content-Length", new System.IO.FileInfo(exportFile).Length.ToString(""));
        }

        public string ExportFilename()
        {
            return URLHelper.GetPageNameFromURL(Page.Request.Url) + "_" + DateTime.Now.ToString(FSFormat.DateStamp) + ".xlsx";
        }
    }
}
