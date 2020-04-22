using System.Configuration;
using System.Web;
using System.Web.Optimization;

namespace QRCodeManagement
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/qr_code").Include(
                "~/dist/js/angular.min.js",
                "~/Scripts/Applications/App.QR.js",
                "~/Scripts/Pages/Page.CreateQR.js",
                "~/Scripts/Pages/Page.Template.js",
                "~/Scripts/Pages/Page.Logo.js",
                "~/bower_components/jquery-minicolors-master/jquery.minicolors.js",
                "~/Scripts/Pages/Page.CreateQR.RenderColorPicker.js",
                "~/Scripts/Pages/Page.CreateQR.VCardApp.js",
                "~/Scripts/Pages/Page.CreateQR.CreateFile.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/manage_qr_code").Include(
               "~/Scripts/Applications/App.QR.js",
               "~/Scripts/Pages/Page.ManageQR.js"
               ));

            bundles.Add(new ScriptBundle("~/manage_account/js").Include(
              "~/Scripts/Pages/Page.UserManager.js"
              ));

            bundles.Add(new ScriptBundle("~/login/js").Include(
               "~/Scripts/Pages/Page.Login.js"
               ));

            bundles.Add(new ScriptBundle("~/main_app/js").Include(
               "~/bower_components/jquery/dist/jquery.min.js",
               "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
               "~/bower_components/metisMenu/dist/metisMenu.min.j",
               "~/bower_components/datatables/media/js/jquery.dataTables.min.js",
               "~/bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.min.js",
               "~/dist/js/sb-admin-2.js",
               "~/dist/js/bootstrap-dialog.js",
               "~/dist/js/validator.js",
               "~/bower_components/bootstrap-simple-prompts-master/bootstrap-simple-prompts.js",
               "~/Scripts/Applications/App.Modal.js"
               ));

            bundles.Add(new ScriptBundle("~/file_upload/js").Include(
             "~/dist/js/jquery.ui.widget.js",
             "~/dist/js/jquery.fileupload.js"
             ));

            bundles.Add(new ScriptBundle("~/mobile-detection/js").Include(
            "~/dist/js/mobile-detect.js",
            "~/Scripts/Pages/Page.CheckingQR.js"
            ));

            bundles.Add(new ScriptBundle("~/chart-page/js").Include(
            "~/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
            "~/bower_components/flot/excanvas.min.js",
            "~/bower_components/flot/jquery.flot.js",
            "~/bower_components/flot/jquery.flot.pie.js",
            "~/bower_components/flot/jquery.flot.resize.js",
            "~/bower_components/flot/jquery.flot.time.js",
            "~/bower_components/flot.tooltip/js/jquery.flot.tooltip.min.js",
            "~/Scripts/Pages/Page.Chart.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/QRCodeMangement/css").Include(
                      "~/bower_components/jquery-minicolors-master/jquery.minicolors.css"
                      ));

            bundles.Add(new StyleBundle("~/main_app/css").Include(
                      "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      "~/bower_components/metisMenu/dist/metisMenu.min.css",
                      "~/bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.css",
                      "~/bower_components/datatables-responsive/css/dataTables.responsive.css",
                      "~/bower_components/font-awesome/css/font-awesome.min.css",
                      "~/dist/css/jquery.fileupload.css"
                      ));

            bundles.Add(new StyleBundle("~/custom/css").Include(
                      "~/dist/css/sb-admin-2.css",
                      "~/dist/css/bootstrap-dialog.css"
                      ));

            BundleTable.EnableOptimizations = ConfigurationManager.AppSettings["EnableOptimizationsBundle"].ToString() == "true";
        }
    }
}
