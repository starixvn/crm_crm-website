// --------------------------------------------------------------------------
// <copyright file="AdminBundles.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.01">Created</change>
// </history>

namespace Fanex.CRM.AdminTool.App_Start
{
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// Bundle Config of Report.
    /// </summary>
    public static class AdminBundles
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterFeedbackSubjectManagementBundles(bundles);
            RegisterDeleteFeedbackSubjectTemplateBundles(bundles);
            RegisterEditFeedbackSubjectTemplateBundles(bundles);
            RegisterInsertFeedbackSubjectTemplateBundles(bundles);
        }

        /// <summary>
        /// Register Feedback Subject Management Bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterFeedbackSubjectManagementBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/FeedbackSubjectManagement/ViewListScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ViewList/Scripts/ViewListApplication.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ViewList/Scripts/ViewListService.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ViewList/Scripts/ViewListController_HotKeyFunctions.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ViewList/Scripts/ViewListController_Options.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ViewList/Scripts/ViewListController_Update.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ViewList/Scripts/ViewListController.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ViewList/Scripts/Directives/*.js"));

            AddAndInclude(
                bundles,
                "~/Areas/Admin/FeedbackSubjectManagement/ViewListStyles",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ViewList/Styles/ViewList.less");
        }

        /// <summary>
        /// Register Delete Feedback Subject Template Bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterDeleteFeedbackSubjectTemplateBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/FeedbackSubjectManagement/DeleteTemplateScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/DeleteTemplate/Scripts/DeleteTemplateApplication.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/DeleteTemplate/Scripts/DeleteTemplateService.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/DeleteTemplate/Scripts/DeleteTemplateController.js"));

            AddAndInclude(
                bundles,
                "~/Areas/Admin/FeedbackSubjectManagement/DeleteTemplateStyles",
                "~/Areas/AdminTool/FeedbackSubjectManagement/DeleteTemplate/Styles/DeleteTemplate.less");
        }

        /// <summary>
        /// Register Edit Feedback Subject Template Bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterEditFeedbackSubjectTemplateBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/FeedbackSubjectManagement/EditTemplateScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/EditTemplate/Scripts/EditTemplateApplication.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/EditTemplate/Scripts/EditTemplateService.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/EditTemplate/Scripts/EditTemplateController.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/EditTemplate/Scripts/Directives/*.js"));

            AddAndInclude(
                bundles,
                "~/Areas/Admin/FeedbackSubjectManagement/EditTemplateStyles",
                "~/Areas/AdminTool/FeedbackSubjectManagement/EditTemplate/Styles/EditTemplate.less");
        }

        /// <summary>
        /// Register Insert Feedback Subject Template Bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterInsertFeedbackSubjectTemplateBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/FeedbackSubjectManagement/InsertTemplateScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/InsertTemplate/Scripts/InsertTemplateApplication.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/InsertTemplate/Scripts/InsertTemplateService.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/InsertTemplate/Scripts/InsertTemplateController.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/InsertTemplate/Scripts/Directives/*.js"));

            AddAndInclude(
                bundles,
                "~/Areas/Admin/FeedbackSubjectManagement/InsertTemplateStyles",
                "~/Areas/AdminTool/FeedbackSubjectManagement/InsertTemplate/Styles/InsertTemplate.less");
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
