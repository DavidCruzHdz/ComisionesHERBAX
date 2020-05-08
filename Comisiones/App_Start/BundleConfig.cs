namespace Comisiones
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                     "~/Scripts/jquery-{version}.js",
                     "~/Scripts/jquery-ui.js",
                     "~/Scripts/jquery-latest.js",
                     "~/Scripts/jquery-ui.js",
                     "~/Scripts/jquery.dataTables.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/Comisiones.js",
                      "~/Scripts/jquery.dataTables.min.js",
                       "~/Scripts/moment.js"
                      ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                       "~/Content/jquery.dataTables_themeroller.css",
                       "~/Content/jquery.dataTables.css",
                       "~/Content/demo_table_jui.css",
                       "~/Content/demo_table.css",
                       "~/Content/demo_page.css",
                      "~/Content/site.css"));




        }
    }
}
