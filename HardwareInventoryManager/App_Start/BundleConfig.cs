using System.Web;
using System.Web.Optimization;

namespace HardwareInventoryManager
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            // Toastr notifications
            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                      "~/Scripts/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-route.js",
                "~/Scripts/angular-resource.js"));

            bundles.Add(new ScriptBundle("~/bundles/angulardatatables").Include(
                "~/Scripts/DataTables/angular-datatables.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularbootstrap").Include(
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js"));
              
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/toastr.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui.theme.css"));

            // Datatables Bundles
            bundles.Add(new StyleBundle("~/Content/datatablescss").Include(
                       "~/Content/jquery.dataTables.css",
                       "~/Content/dataTables.responsive.css"));
            
            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                        "~/Scripts/jquery.dataTables.js",
                        "~/Scripts/dataTables.responsive.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").IncludeDirectory(
                        "~/Scripts/App/", "*.js", true));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
