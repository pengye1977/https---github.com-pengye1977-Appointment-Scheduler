// ***********************************************************************
// Assembly         : SBAS_Web
// Author           : Ye Peng
// Created          : 05-21-2014
//
// Last Modified By : Ye Peng
// Last Modified On : 08-10-2014
// ***********************************************************************
// <copyright file="BundleConfig.cs" company="PENN STATE MASTERS PROGRAM">
//     Copyright (c) PENN STATE MASTERS PROGRAM. All rights reserved.
// </copyright>
// <summary> 
//   This file is used to setup all the scripts and the css references
// </summary>
// ***********************************************************************

using System.Web;
using System.Web.Optimization;
/// <summary>
/// The SBAS_Web namespace.
/// </summary>
namespace SBAS_Web
{
    /// <summary>
    /// Class BundleConfig.
    /// </summary>
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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

            bundles.Add(new ScriptBundle("~/bundles/kendojs").Include(
                "~/Scripts/kendo/2014.2.716/kendo.all.min.js",
                "~/Scripts/kendo/2014.2.716/kendo.timezones.min.js",
                "~/Scripts/kendo/2014.2.716/kendo.aspnetmvc.min.js",
                "~/Scripts/kendo.modernizr.custom.js"));

            //http://medialize.github.io/jQuery-contextMenu/index.html
            bundles.Add(new ScriptBundle("~/bundles/SBASJs").Include(
                "~/Scripts/SBAS.js",
                "~/Scripts/tablesorter/jquery.tablesorter.js",
                "~/Scripts/tablesorter/tables.js"));

            bundles.Add(new ScriptBundle("~/bundles/jsconfirm").Include(
                "~/Scripts/jquery.confirm.js"));


            //http://myclabs.github.io/jquery.confirm/
            bundles.Add(new ScriptBundle("~/bundles/jq-ContextMenu").Include(
                "~/Scripts/jquery.ui.position.js",
                "~/Scripts/jquery.contextMenu.js"));

            //http://zurb.com/playground/reveal-modal-plugin
            bundles.Add(new ScriptBundle("~/bundles/reveal-modal").Include(
                "~/Scripts/jquery.reveal.js"));

            //http://dinbror.dk/blog/bPopup/
            bundles.Add(new ScriptBundle("~/bundles/bpopup").Include(
                "~/Scripts/bpopup/jquery.bpopup.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/sb-admin/bootstrap.css",
                "~/Content/sb-admin/sb-admin.css",
                "~/Content/font-awesome/css/font-awesome.min.css",
                "~/Content/jquery.contextMenu.css",
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/kendocss").Include(
                "~/Content/kendo/2014.2.716/kendo.common.min.css",
                "~/Content/kendo/2014.2.716/kendo.dataviz.min.css",
                "~/Content/kendo/2014.2.716/kendo.bootstrap.min.css",
                "~/Content/kendo/2014.2.716/kendo.dataviz.bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/reveal-css").Include(
                "~/Content/reveal/reveal.css"));

            bundles.Add(new StyleBundle("~/Content/SBASCSS").Include(
                "~/Content/site.css"));


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}