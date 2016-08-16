using System.Web.UI;

[assembly: WebResource("Phoenix.JS.Planning.DimensionManage.js", "application/x-javascript")]
[assembly: WebResource("Phoenix.JS.Planning.Formula.js", "application/x-javascript")]
[assembly: WebResource("Phoenix.JS.Planning.Properties.js", "application/x-javascript")]
[assembly: WebResource("Phoenix.JS.Planning.UDAs.js", "application/x-javascript")]

namespace Phoenix.JS
{
    public static class Planning
    {
        public static void Include(ClientScriptManager manager)
        {
            IncludeJavaScript(manager, "Phoenix.JS.Planning.DimensionManage.js");
            IncludeJavaScript(manager, "Phoenix.JS.Planning.Formula.js");
            IncludeJavaScript(manager, "Phoenix.JS.Planning.Properties.js");
            IncludeJavaScript(manager, "Phoenix.JS.Planning.UDAs.js");
        }

        private static void IncludeJavaScript (ClientScriptManager manager, string resourceName)
        {
            var type = typeof(Validation);
            manager.RegisterClientScriptResource(type, resourceName);
        }
    }
}