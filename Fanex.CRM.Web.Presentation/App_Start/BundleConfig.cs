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
            #region common - style
            bundles.AddAndInclude(
                "~/site/common",
                "~/content/bootstrap/css/bootstrap.css",
                "~/Content/site/common.less",
                "~/Content/site/image.less",
                "~/Content/site/input.less",
                "~/Content/site/Message.less",
                "~/Content/site/table.less",
                "~/Content/site/ui-darkness/jquery-ui-1.8.14.custom.css");
            #endregion            

            #region head js
            bundles.Add(new ScriptBundle("~/scripts/headload").Include(
                "~/Scripts/head.load.min.js"));

            bundles.Add(new ScriptBundle("~/scripts/respond").Include(
                "~/Scripts/respond.js",
                "~/Scripts/head.js"));
            #endregion

            #region popup - js
            bundles.Add(new ScriptBundle("~/Scripts/PopUp").Include(
                "~/Scripts/showhidepopup.js"));
            #endregion

            #region commont - js
            bundles.Add(new ScriptBundle("~/Scripts/CommomScripts").Include(
                "~/Scripts/JQuery/jquery-1.8.2.js",
                "~/Scripts/json2.js",
                "~/Scripts/jquery-ui-1.8.14.custom.min.js",
                "~/Scripts/ShareScripts/CRMConstants.js",
                "~/Scripts/ShareScripts/SharedScriptManager.js",
                "~/Scripts/ShareScripts/KeyboardManager.js"));
            #endregion

            #region Home - style/js
            bundles.Add(new StyleBundle("~/Views/Home/Styles").Include(
                "~/Scripts/Tipsy-Tooltip/tipsy.css"));

            bundles.Add(new ScriptBundle("~/Views/Home/Scripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Views/Home/Scripts/MenuManager.js",
                "~/Views/Home/Scripts/LeftPanelManager.js",
                "~/Views/Home/Scripts/HomeManager.js"));
            #endregion

            #region Customer - Left panel
            bundles.Add(new ScriptBundle("~/Areas/ShortCustomerInfo/Scripts").Include(
                "~/Scripts/jscookies.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Customer/ShortInfo/Scripts/CustomerManager.js",
                "~/Areas/Customer/ShortInfo/Scripts/CustomerApplication.js",
                "~/Areas/Customer/ShortInfo/Scripts/CustomerService.js",
                "~/Areas/Customer/ShortInfo/Scripts/CustomerController.js"));

            bundles.Add(new StyleBundle("~/Areas/ShortCustomerInfo/Styles").Include(
                "~/Areas/Customer/ShortInfo/Styles/StyleSheet.less"));
            #endregion            

            #region AngularJS
            bundles.Add(new ScriptBundle("~/Scripts/AgularJS").Include(
                "~/Scripts/AgularJS/angular.js"));
            #endregion

            #region multiple select control
            bundles.Add(new ScriptBundle("~/Styles/MultipleSelect").Include(
                "~/Scripts/MultipleSelect/fanex.multiSelect.css"));
            bundles.Add(new ScriptBundle("~/Scripts/MultipleSelect").Include(
                "~/Scripts/MultipleSelect/fanex.multiSelect.js"));
            #endregion

            #region Customer - Detail tab
            bundles.Add(new ScriptBundle("~/Areas/Customer/DetailInfo/Scripts").Include(
                "~/Scripts/jscookies.js",
                "~/Areas/Customer/DetailInfo/Scripts/CustomerDetailApplication.js",
                "~/Areas/Customer/DetailInfo/Scripts/CustomerDetailService.js",
                "~/Areas/Customer/DetailInfo/Scripts/CustomerDetailController.js",
                "~/Areas/Customer/DetailInfo/Scripts/CustomerDetailDirective.js"));

            bundles.Add(new ScriptBundle("~/Views/Home/Dashboard/Scripts").Include(
                "~/Views/Home/Scripts/DashboardManager.js"));
            #endregion
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