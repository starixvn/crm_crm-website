// --------------------------------------------------------------------------
// <copyright file="InformHeadlineBundles.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline
{
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// Bundle Config of Report.
    /// </summary>
    public static class InformHeadlineBundles
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterInformHeadlineViewListBundles(bundles);
            RegisterInformHeadlineNewInformBundles(bundles);    
        }

        /// <summary>
        /// Register View List Bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterInformHeadlineViewListBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Bundles/InformHeadline/ViewListScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/InformHeadline/ViewList/Scripts/ViewListApplication.js",
                "~/Areas/InformHeadline/ViewList/Scripts/ViewListService.js",
                "~/Areas/InformHeadline/ViewList/Scripts/ViewListController.js",
                "~/Areas/InformHeadline/ViewList/Scripts/ViewListController_Options.js",
                "~/Areas/InformHeadline/ViewList/Scripts/ViewListController_Update.js",
                "~/Areas/InformHeadline/ViewList/Scripts/ViewListController_HotKey.js",
                "~/Areas/InformHeadline/ViewList/Scripts/Directives/*.js"));

            AddAndInclude(
                bundles,                
                "~/Areas/InformHeadline/ViewListStyles",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/InformHeadline/ViewList/Styles/ViewList.less");
        }

        /// <summary>
        /// Register New Inform Bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterInformHeadlineNewInformBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Bundles/InformHeadline/NewInformScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                 "~/Scripts/ShareScripts/Directives/MultiSelect.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/InformHeadline/NewInform/Scripts/ViewListApplication.js",
                "~/Areas/InformHeadline/NewInform/Scripts/ViewListService.js",
                "~/Areas/InformHeadline/NewInform/Scripts/ViewListController.js",                
                "~/Areas/InformHeadline/NewInform/Scripts/Directives/*.js"));

            AddAndInclude(
                bundles,                
                "~/Areas/InformHeadline/NewInformStyles",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/InformHeadline/NewInform/Styles/ViewList.less");
        }

        private static void AddAndInclude(BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new StyleTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }
    }
}
