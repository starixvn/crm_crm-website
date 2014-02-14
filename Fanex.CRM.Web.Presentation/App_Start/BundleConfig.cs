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
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/site/common",
                "~/content/bootstrap/css/bootstrap.css",
                "~/Content/site/common.less",
                "~/Content/site/Message.less",
                "~/Content/site/ui-darkness/jquery-ui-1.8.14.custom.css",
                "~/Content/site/customizeJqueryUI.less");

            bundles.Add(new ScriptBundle("~/scripts/headload").Include(
                "~/Scripts/head.load.min.js"));

            bundles.Add(new ScriptBundle("~/scripts/respond").Include(
                "~/Scripts/respond.js",
                "~/Scripts/head.js"));

            bundles.Add(new ScriptBundle("~/Scripts/PopUp").Include(
                "~/Scripts/showhidepopup.js",
                "~/Scripts/dialogbox.js"));

            bundles.Add(new ScriptBundle("~/Scripts/CommomScripts").Include(
                "~/Scripts/JQuery/jquery-1.8.2.js",
                "~/Scripts/json2.js",
                "~/Scripts/jquery-ui-1.8.14.custom.js",
                "~/Scripts/ShareScripts/CRMConstants.js",
                "~/Scripts/ShareScripts/SharedScriptManager.js",
                "~/Scripts/ShareScripts/KeyboardManager.js"));

            bundles.AddAndInclude(
                "~/Views/HomeStyles",
                "~/Scripts/Tipsy-Tooltip/tipsy.css");

            bundles.Add(new ScriptBundle("~/Views/HomeScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Views/Home/Scripts/MenuManager.js",
                "~/Views/Home/Scripts/LeftPanelManager.js",
                "~/Views/Home/Scripts/HomeManager.js"));

            bundles.Add(new ScriptBundle("~/Areas/ShortCustomerInfo/Scripts").Include(
                "~/Scripts/jscookies.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Customer/ShortInfo/Scripts/CustomerApplication.js",
                "~/Areas/Customer/ShortInfo/Scripts/CustomerService.js",
                "~/Areas/Customer/ShortInfo/Scripts/CustomerController.js",
                "~/Areas/Customer/ShortInfo/Scripts/CustomerManager.js"));

            bundles.AddAndInclude(
                "~/Areas/ShortCustomerInfo/Styles",
                "~/Areas/Customer/ShortInfo/Styles/StyleSheet.less");

            bundles.Add(new ScriptBundle("~/Scripts/AngularJSLibrary").Include(
                "~/Scripts/AngularJS/angular.js"));

            bundles.Add(new ScriptBundle("~/Styles/FanexMultipleSelect").Include(
                "~/Scripts/MultipleSelect/fanex.multiSelect.css",
                "~/Areas/Customer/DetailInfo/Styles/StyleSheet.less"));
            bundles.Add(new ScriptBundle("~/Scripts/FanexMultipleSelect").Include(
                "~/Scripts/MultipleSelect/fanex.multiSelect.js"));

            bundles.Add(new ScriptBundle("~/Areas/DetailCustomerInfo/Scripts").Include(
                "~/Scripts/jscookies.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Customer/DetailInfo/Scripts/CustomerDetailApplication.js",
                "~/Areas/Customer/DetailInfo/Scripts/CustomerDetailService.js",
                "~/Areas/Customer/DetailInfo/Scripts/CustomerDetailController.js",
                "~/Areas/Customer/DetailInfo/Scripts/CustomerDetailDirective.js"));

            bundles.Add(new ScriptBundle("~/Views/Home/Dashboard/Scripts").Include(
                "~/Views/Home/Scripts/DashboardManager.js"));

            RegisterBundles_TimePicker(ref bundles);
            RegisterBundles_CommunicationLog_ViewLog(ref bundles);
            RegisterBundles_CommunicationLog_Edit(ref bundles);
            RegisterBundles_CommunicationLog_Insert(ref bundles);
            RegisterBundles_Redemption_ViewLog(ref bundles);
            RegisterBundles_Redemption_InsertLog(ref bundles);
            RegisterBundles_Redemption_EditLog(ref bundles);
            RegisterBundles_Remark(ref bundles);
            RegisterBundles_Dashboard_ViewInfo(ref bundles);
            RegisterBundles_Dashboard_VerifyFlag(ref bundles);
        }

        public static void RegisterBundles_TimePicker(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/TimePicker").Include(
                     "~/Scripts/jquery-ui-timepicker-addon.js"));
        }

        /// <summary>
        /// Registers the bundles communication log view log.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles_CommunicationLog_ViewLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Communication/ViewLogScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Communication/ViewLog/Scripts/CommunicationApplication.js",
                "~/Areas/Communication/ViewLog/Scripts/CommunicationService.js",
                "~/Areas/Communication/ViewLog/Scripts/CommunicationController.js",
                "~/Areas/Communication/ViewLog/Scripts/CommunicationDirective.js"));

            bundles.AddAndInclude(
                "~/Areas/Communication/ViewLogStyles",
                "~/Areas/Communication/ViewLog/Styles/Communication.less");
        }

        /// <summary>
        /// Registers the bundles communication log edit.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles_CommunicationLog_Edit(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Communication/EditLogScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Communication/EditLog/Scripts/EditLogApplication.js",
                "~/Areas/Communication/EditLog/Scripts/EditLogService.js",
                "~/Areas/Communication/EditLog/Scripts/EditLogController.js",
                "~/Areas/Communication/EditLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Communication/EditLogStyles",
                "~/Areas/Communication/EditLog/Styles/EditCommunicationLog.less");
        }

        /// <summary>
        /// Registers the bundles communication log insert.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles_CommunicationLog_Insert(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Communication/InsertLogScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Communication/InsertLog/Scripts/InsertLogApplication.js",
                "~/Areas/Communication/InsertLog/Scripts/InsertLogService.js",
                "~/Areas/Communication/InsertLog/Scripts/InsertLogController.js",
                "~/Areas/Communication/InsertLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Communication/InsertLogStyles",
                "~/Areas/Communication/InsertLog/Styles/InsertCommunicationLog.less");
        }

        /// <summary>
        /// Registers the bundles redemption view log.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles_Redemption_ViewLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Redemption/ViewLogScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Redemption/ViewLog/Scripts/RedemptionViewLogApplication.js",
                "~/Areas/Redemption/ViewLog/Scripts/RedemptionViewLogService.js",
                "~/Areas/Redemption/ViewLog/Scripts/RedemptionViewLogController.js",
                "~/Areas/Redemption/ViewLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Redemption/ViewLogStyles",
                "~/Areas/Redemption/ViewLog/Styles/Redemption.less");
        }

        /// <summary>
        /// Registers the bundles redemption insert log.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles_Redemption_InsertLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Redemption/InsertLogScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Redemption/InsertLog/Scripts/InsertLogApplication.js",
                "~/Areas/Redemption/InsertLog/Scripts/InsertLogService.js",
                "~/Areas/Redemption/InsertLog/Scripts/InsertLogController.js",
                "~/Areas/Redemption/InsertLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Redemption/InsertLogStyles",
                "~/Areas/Redemption/InsertLog/Styles/Redemption.less");
        }

        /// <summary>
        /// Registers the bundles redemption edit log.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles_Redemption_EditLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Redemption/EditLogScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Redemption/EditLog/Scripts/EditLogApplication.js",
                "~/Areas/Redemption/EditLog/Scripts/EditLogService.js",
                "~/Areas/Redemption/EditLog/Scripts/EditLogController.js",
                "~/Areas/Redemption/EditLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Redemption/EditLogStyles",
                "~/Areas/Redemption/EditLog/Styles/Redemption.less");
        }

        public static void RegisterBundles_Remark(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Remark/RemarkScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Remark/Scripts/RemarkApplication.js",
                "~/Areas/Remark/Scripts/RemarkService.js",
                "~/Areas/Remark/Scripts/RemarkController.js",
                "~/Areas/Remark/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Remark/Styles",
                "~/Areas/Remark/Styles/remark.less");
        }

        /// <summary>
        /// Registers the bundles dashboard view information.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles_Dashboard_ViewInfo(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Dashboard/ViewInfoScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Dashboard/ViewInfo/Scripts/DashboardApplication.js",
                "~/Areas/Dashboard/ViewInfo/Scripts/DashboardService.js",
                "~/Areas/Dashboard/ViewInfo/Scripts/DashboardController.js",
                "~/Areas/Dashboard/ViewInfo/Scripts/Directives/*.js"));

            bundles.Add(new StyleBundle("~/Areas/Dashboard/ViewInfoStyles").Include(
                "~/Areas/Dashboard/ViewInfo/Styles/StyleSheet.less"));
        }

        /// <summary>
        /// Registers the bundles dashboard verify flag.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles_Dashboard_VerifyFlag(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Dashboard/VerifyFlagScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Dashboard/VerifyFlag/Scripts/VerifyFlagApplication.js",
                "~/Areas/Dashboard/VerifyFlag/Scripts/VerifyFlagService.js",
                "~/Areas/Dashboard/VerifyFlag/Scripts/VerifyFlagController.js",
                "~/Areas/Dashboard/VerifyFlag/Scripts/Directives/*.js"));

            bundles.Add(new StyleBundle("~/Areas/Dashboard/VerifyFlagStyles").Include(
                "~/Areas/Dashboard/VerifyFlag/Styles/VerifyFlag.less"));
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