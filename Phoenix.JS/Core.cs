using System.Web.UI;

[assembly: WebResource("Phoenix.JS.Core.Processing.js", "application/x-javascript")]
[assembly: WebResource("Phoenix.JS.Core.TableManage.js", "application/x-javascript")]
[assembly: WebResource("Phoenix.JS.Core.URL.js", "application/x-javascript")]

namespace Phoenix.JS
{
    public static class Core
    {
        public static void Include_TableManage(ClientScriptManager manager)
        {
            IncludeJavaScript(manager, "Phoenix.JS.Core.TableManage.js");
        }

        public static void Include_Processing(ClientScriptManager manager)
        {
            IncludeJavaScript(manager, "Phoenix.JS.Core.Processing.js");
        }

        public static void Include_URL(ClientScriptManager manager)
        {
            IncludeJavaScript(manager, "Phoenix.JS.Core.URL.js");
        }

        private static void IncludeJavaScript (ClientScriptManager manager, string resourceName)
        {
            var type = typeof(Core);
            manager.RegisterClientScriptResource(type, resourceName);
        }
    }
}