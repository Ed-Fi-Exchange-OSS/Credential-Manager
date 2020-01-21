using System.Web;
using System.Web.Optimization;

namespace Ed_Fi.Credential.MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-confirm.min.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/timeout-dialog.js",
                "~/Scripts/select2-4.0.10/js/select2.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                   "~/Scripts/umd/popper.js",
                   "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/bootstrap4-glyphicons/css/bootstrap-glyphicons.min.css", new CssRewriteUrlTransformWrapper())
                .Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/core.css",
                "~/Content/jquery-confirm.min.css",
                "~/Content/timeout-dialog.css",
                "~/Content/select2-4.0.10/select2.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/dt")
                .Include("~/Scripts/DataTables/JSZip-2.5.0/jszip.js")
                .Include("~/Scripts/DataTables/pdfmake-0.1.36/pdfmake.js")
                .Include("~/Scripts/DataTables/pdfmake-0.1.36/vfs_fonts.js")
                .Include("~/Scripts/DataTables/DataTables-1.10.18/js/jquery.dataTables.js")
                .Include("~/Scripts/DataTables/Buttons-1.5.6/js/dataTables.buttons.js")
                .Include("~/Scripts/DataTables/Buttons-1.5.6/js/buttons.colVis.js")
                .Include("~/Scripts/DataTables/Buttons-1.5.6/js/buttons.html5.js")
                .Include("~/Scripts/DataTables/FixedColumns-3.2.5/js/dataTables.fixedColumns.js")
                .Include("~/Scripts/DataTables/FixedHeader-3.1.4/js/dataTables.fixedHeader.js")
                .Include("~/Scripts/DataTables/RowGroup-1.1.0/js/dataTables.rowGroup.js")
                .Include("~/Scripts/DataTables/moment.js")
            );

            bundles.Add(new StyleBundle("~/Content/dt")
                .Include("~/Content/DataTables/DataTables-1.10.18/css/jquery.dataTables.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/DataTables/Buttons-1.5.6/css/buttons.dataTables.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/DataTables/FixedColumns-3.2.5/css/fixedColumns.dataTables.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/DataTables/FixedHeader-3.1.4/css/fixedHeader.dataTables.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/DataTables/RowGroup-1.1.0/css/rowGroup.dataTables.css", new CssRewriteUrlTransformWrapper())
            );

        }
    }

    //https://stackoverflow.com/questions/19765238/cssrewriteurltransform-with-or-without-virtual-directory
    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}
