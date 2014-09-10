// --------------------------------------------------------------------------
// <copyright file="FeedbackBundles.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Bundles
{
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// Feedback Bundles.
    /// </summary>
    public static class FeedbackBundles
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterManageFeedbackBundles(bundles);
            RegisterLatestFeedbackBundles(bundles);
        }

        /// <summary>
        /// Registers the manage feedback bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterManageFeedbackBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/ManageFeedback/ManageFeedbackScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Feedbacks/ManageFeedback/Scripts/ManageFeedbackApplication.js",
                "~/Areas/Feedbacks/ManageFeedback/Scripts/ManageFeedbackController.js",
                "~/Areas/Feedbacks/ManageFeedback/Scripts/ManageFeedbackService.js",
                "~/Areas/Feedbacks/ManageFeedback/Scripts/Directives/*.js"));

            AddAndInclude(
                bundles,
                "~/Areas/Feedbacks/ManageFeedback/ManageFeedbackStyles",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/Feedbacks/ManageFeedback/Styles/StyleSheet.less");
        }

        /// <summary>
        /// Registers the latest feedback bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterLatestFeedbackBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/LatestFeedback/LatestFeedbackScripts").Include(
                "~/Scripts/Tipsy-Tooltip/jquery.tipsy.js",
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Areas/Feedbacks/LatestFeedback/Scripts/LatestFeedbackApplication.js",
                "~/Areas/Feedbacks/LatestFeedback/Scripts/LatestFeedbackController.js",
                "~/Areas/Feedbacks/LatestFeedback/Scripts/LatestFeedbackService.js",
                "~/Areas/Feedbacks/LatestFeedback/Scripts/Directives/*.js"));

            AddAndInclude(
                bundles,
                "~/Areas/Feedbacks/LatestFeedback/LatestFeedbackStyles",
                "~/Scripts/Tipsy-Tooltip/tipsy.css",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Areas/Feedbacks/LatestFeedback/Styles/StyleSheet.less");
        }

        private static void AddAndInclude(BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new CssTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }
    }
}
