// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Web.View.App_Start
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            // bundles.Add(new ScriptBundle("~/bundles/js").Include("~/Scripts/*.js"));
            bundles.AddAndInclude(
                "~/site/common",
                "~/content/bootstrap/css/bootstrap.css",
                "~/Content/site/common.less",
                "~/Content/site/image.less",
                "~/Content/site/input.less",
                "~/Content/site/Message.less",
                "~/Content/site/table.less");

            bundles.Add(new ScriptBundle("~/scripts/headload").Include(
                "~/Scripts/head.load.min.js"));

            bundles.Add(new ScriptBundle("~/scripts/respond").Include(
                "~/Scripts/respond.js",
                "~/Scripts/head.js"));

            bundles.Add(new ScriptBundle("~/Scripts/CommomScripts").Include(
                "~/Scripts/jquery.js",
                "~/Scripts/json2.js",
                "~/Scripts/SharedScriptManager.js"));

            bundles.Add(new StyleBundle("~/Views/Home/Styles").Include(
                "~/Scripts/Tipsy-Tooltip/tipsy.css"));

            bundles.Add(new ScriptBundle("~/Views/Home/Scripts").Include(
                 "~/Scripts/Tipsy-Tooltip/jquery.tipsy.js",
                "~/Views/Home/Scripts/CRMConstants.js",
                "~/Views/Home/Scripts/MenuManager.js",
                "~/Views/Home/Scripts/LeftPanelManager.js"));

            bundles.Add(new ScriptBundle("~/Areas/CustomerInfo/Scripts").Include(
                "~/Areas/Customer/Scripts/CustomerManager.js",
                "~/Areas/Customer/Scripts/CustomerApplication.js",
                "~/Areas/Customer/Scripts/CustomerService.js",
                "~/Areas/Customer/Scripts/CustomerController.js"));

            bundles.Add(new StyleBundle("~/Areas/CustomerInfo/Styles").Include(
                "~/Areas/Customer/Styles/StyleSheet.less"));

            bundles.Add(new ScriptBundle("~/Scripts/AgularJS").Include(
                "~/Scripts/AgularJS/angular.min.js"));
        }
    }

    /// <summary>
    /// Bundle Collection extension.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Need to review here.")]
    internal static class BundleExtensions
    {
        /// <summary>
        /// Creates a bundle, include the virtual paths to the created bundle,
        /// and add it to the bundle collection.
        /// </summary>
        /// <param name="bundles">The bundle collection which the virtual paths are included.</param>
        /// <param name="bundleVirtualPath">Virtual path of the created bundle.</param>
        /// <param name="virtualPaths">The virtual paths that needed to add to bundle collection.</param>
        public static void AddAndInclude(
            this BundleCollection bundles,
            string bundleVirtualPath,
            params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new CssTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }

        /// <summary>
        /// Gets relative path from absolute path.
        /// </summary>
        /// <param name="context">The context using to get the path.</param>
        /// <param name="path">The absolute path.</param>
        /// <returns>The relative path.</returns>
        public static string RelativeFromAbsolutePath(this HttpContextBase context, string path)
        {
            var request = context.Request;
            var applicationPath = request.PhysicalApplicationPath;
            var virtualDir = request.ApplicationPath;
            virtualDir = virtualDir == "/" ? virtualDir : (virtualDir + "/");
            return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
        }
    }
}