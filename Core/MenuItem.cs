using System;
using System.Web.UI.HtmlControls;

namespace Core
{
    public class MenuItem
    {
        private readonly string _navMenu;
        private readonly string _name;
        private readonly string _page;
        private readonly string _description;
        private readonly string _params;
        private readonly Boolean _selected;

        public MenuItem(string navMenu, string menuName, string menuPage, string menuDescription, string menuPageParams, Boolean selected)
        {
            _navMenu = navMenu;
            _name = menuName;
            _page = menuPage;
            _description = menuDescription;
            _params = menuPageParams;
            _selected = selected;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Page
        {
            get { return _page; }
        }

        public string Params
        {
            get { return _params; }
        }

        public string Identifier
        {
            get { return _page + "_" + _params; }
        }

        public string NavMenu
        {
            get { return _navMenu; }
        }

        public HtmlGenericControl MakeMenuRow()
        {
            var aLi = new HtmlGenericControl("li");
            aLi.Attributes["class"] = "menuCell";
            if (_selected) aLi.Attributes["class"] += "On";
            aLi.InnerHtml = "<a href='/" + Page + "" + (Params == "" ? "" : "?" + Params) + "' ID='menuItem" + Page + "'>" + Name + "<div class='description'>" + _description + "</div></a>";

            return aLi;
        }
    }

    public class SubMenuItem
    {
        private readonly string _menuItem;
        private readonly string _menuItemParams;
        private readonly string _name;
        private readonly string _page;
        private readonly string _params;
        private readonly Boolean _selected;

        public SubMenuItem(string menuItem, string menuItemParams, string subMenuName, string subMenuPage, string subMenuPageParams, Boolean selected)
        {
            _menuItem = menuItem;
            _menuItemParams = menuItemParams;
            _name = subMenuName;
            _page = subMenuPage;
            _params = subMenuPageParams;
            _selected = selected;
            _menuItemParams = menuItemParams;
        }

        public string MenuItem
        {
            get { return _menuItem; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Page
        {
            get { return _page; }
        }

        public string Params
        {
            get { return _params; }
        }

        public bool Selected
        {
            get { return _selected; }
        }

        public string MenuItemParams
        {
            get { return _menuItemParams; }
        }

        public string Identifier
        {
            get { return _page + "_" + _params; }
        }

        public HtmlGenericControl MakeSubMenuRow()
        {
            var aLi = new HtmlGenericControl("li");
            aLi.Attributes["class"] = "subMenuCell";
            if (_selected) { aLi.Attributes["class"] += "On"; }
            aLi.InnerHtml = "<a href='/" + Page + "" + (Params == "" ? "" : "?" + Params) + "' ID='subMenuItem" + Name.Replace(" ", "") + "'>" + Name + "</a>";

            return aLi;
        }
    }
}
