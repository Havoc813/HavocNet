using System;
using System.Web.UI.HtmlControls;

namespace Phoenix.Core
{
    public class FSMenu
    {
        public static HtmlGenericControl MakeMenuRow(string menuName, string menuPage, string menuDescription, string menuPageParams, Boolean selected)
        {
            var aLi = new HtmlGenericControl("li");
            aLi.Attributes["class"] = "menuCell";
            if (selected) aLi.Attributes["class"] += "On";
            aLi.InnerHtml = "<a href='" + menuPage + "" + (menuPageParams == "" ? "" : "?" + menuPageParams) + "' ID='menuItem" + menuPage + "'>" + menuName + "<div class='description'>" + menuDescription + "</div></a>";

            return aLi;
        }

        public static HtmlGenericControl MakeSubMenuRow(string subMenuName, string subMenuPage, string subMenuPageParams, Boolean selected)
        {
            var aLi = new HtmlGenericControl("li");
            aLi.Attributes["class"] = "subMenuCell";
            if (selected) { aLi.Attributes["class"] += "On"; }
            aLi.InnerHtml = "<a href='" + subMenuPage + "" + (subMenuPageParams == "" ? "" : "?" + subMenuPageParams) + "' ID='subMenuItem" + subMenuName.Replace(" ", "") + "'>" + subMenuName + "</a>";

            return aLi;
        }
    }
}
