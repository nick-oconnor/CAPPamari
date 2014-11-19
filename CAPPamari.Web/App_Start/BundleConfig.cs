using System.Web;
using System.Web.Optimization;

namespace CAPPamari.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/jqueryui/css").Include(
                "~/Content/jqueryui/jquery-ui.css",
                "~/Content/jqueryui/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include("~/Scripts/knockout.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include("~/Scripts/Main.js"));
        }
    }
}