using System.Web.Optimization;

namespace CAPPamari.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/jquery-ui").IncludeDirectory("~/Content/themes/jquery-ui", "*.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/font-awesome").IncludeDirectory("~/Content/themes/font-awesome", "*.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include("~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include("~/Scripts/knockout.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include("~/Scripts/Main.js"));
        }
    }
}