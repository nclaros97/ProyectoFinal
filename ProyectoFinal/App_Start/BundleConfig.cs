using System.Web;
using System.Web.Optimization;

namespace ProyectoFinal
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/fontaw").Include(
                      "~/Scripts/fontawesome/all.js"
                      
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/fontawesome.min.css",
                      "~/Content/fontawesome.all.css",
                      "~/Content/fontawesome.all.min.css",
                      "~/Content/fontawesome.css",
                      "~/Content/regular.min.css",
                      "~/Content/regular.css",
                      "~/Content/solid.css",
                      "~/Content/solid.min.css",
                      "~/Content/svg-with-js",
                      "~/Content/svg-with-js.min.css",
                      "~/Content/v4-shims.css",
                      "~/Content/v4-shims.min.css"
                      ));
        }
    }
}
