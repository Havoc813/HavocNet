using System.Web.UI;

[assembly: WebResource("Phoenix.JS.Validation.Validate.js", "application/x-javascript")]
[assembly: WebResource("Phoenix.JS.Validation.HyperionValidate.js", "application/x-javascript")]
[assembly: WebResource("Phoenix.JS.Validation.rFrameValidate.js", "application/x-javascript")]

namespace Phoenix.JS
{
    public static class Validation
    {
        public static void Include(ClientScriptManager manager)
        {
            IncludeJavaScript(manager, "Phoenix.JS.Validation.Validate.js");
        }

        public static void Include_rFrame(ClientScriptManager manager)
        {
            Include(manager);

            IncludeJavaScript(manager, "Phoenix.JS.Validation.rFrameValidate.js");
        }

        public static void Include_Hyperion(ClientScriptManager manager)
        {
            Include(manager);

            IncludeJavaScript(manager, "Phoenix.JS.Validation.HyperionValidate.js");
        }

        private static void IncludeJavaScript (ClientScriptManager manager, string resourceName)
        {
            var type = typeof(Validation);
            manager.RegisterClientScriptResource(type, resourceName);
        }
    }
}