namespace Fanex.CRM.Web.View.AppStart
{
    using System.Web.Optimization;

    public static class AdminToolBundles
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterFeedbackSubjectManagementBundles(bundles);
            RegisterDeleteFeedbackSubjectTemplateBundles(bundles);
            RegisterEditFeedbackSubjectTemplateBundles(bundles);
            RegisterInsertFeedbackSubjectTemplateBundles(bundles);

            RegisterSettingRelationViewListBundles(bundles);
            RegisterAddSettingRelationBundles(bundles);

            RegisterSettingRelationCN88ViewListBundles(bundles);
            RegisterAddSettingRelationCN88Bundles(bundles);

            RegisterSettingManagementViewListBundles(bundles);
            RegisterSettingManagementInsertSettingBundles(bundles);
            RegisterSettingManagementInsertSubSettingBundles(bundles);
            RegisterSettingManagementDeleteSettingBundles(bundles);
            RegisterSettingManagementEditSettingBundles(bundles);

            RegisterSettingManagementCN88ViewListBundles(bundles);
            RegisterSettingManagementCN88InsertSettingBundles(bundles);
            RegisterSettingManagementCN88InsertSubSettingBundles(bundles);
            RegisterSettingManagementCN88DeleteSettingBundles(bundles);
            RegisterSettingManagementCN88EditSettingBundles(bundles);

            RegisterCommunicationSettingBundles(bundles);
        }

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

            bundles.AddAndInclude(
                "~/Areas/Admin/FeedbackSubjectManagement/ViewListStyles",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ViewList/Styles/ViewList.less");
        }

        private static void RegisterDeleteFeedbackSubjectTemplateBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/FeedbackSubjectManagement/ChangeTemplateStateScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ChangeTemplateState/Scripts/ChangeTemplateStateApplication.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ChangeTemplateState/Scripts/ChangeTemplateStateService.js",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ChangeTemplateState/Scripts/ChangeTemplateStateController.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/FeedbackSubjectManagement/ChangeTemplateStateStyles",
                "~/Areas/AdminTool/FeedbackSubjectManagement/ChangeTemplateState/Styles/ChangeTemplateState.less");
        }

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

            bundles.AddAndInclude(
                "~/Areas/Admin/FeedbackSubjectManagement/EditTemplateStyles",
                "~/Areas/AdminTool/FeedbackSubjectManagement/EditTemplate/Styles/EditTemplate.less");
        }

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

            bundles.AddAndInclude(
                "~/Areas/Admin/FeedbackSubjectManagement/InsertTemplateStyles",
                "~/Areas/AdminTool/FeedbackSubjectManagement/InsertTemplate/Styles/InsertTemplate.less");
        }

        private static void RegisterSettingRelationViewListBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingRelation/ViewListScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/SettingRelation/ViewList/Scripts/ViewListApplication.js",
                "~/Areas/AdminTool/SettingRelation/ViewList/Scripts/ViewListService.js",
                "~/Areas/AdminTool/SettingRelation/ViewList/Scripts/ViewListController.js",
                "~/Areas/AdminTool/SettingRelation/ViewList/Scripts/ViewListController_Options.js",
                "~/Areas/AdminTool/SettingRelation/ViewList/Scripts/ViewListController_Update.js",
                "~/Areas/AdminTool/SettingRelation/ViewList/Scripts/ViewListController_HotKey.js",
                "~/Areas/AdminTool/SettingRelation/ViewList/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingRelation/ViewListStyles",
                "~/Areas/AdminTool/SettingRelation/ViewList/Styles/ViewList.less");
        }

        private static void RegisterAddSettingRelationBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingRelation/AddRelationScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/SettingRelation/AddRelation/Scripts/AddRelationApplication.js",
                "~/Areas/AdminTool/SettingRelation/AddRelation/Scripts/AddRelationService.js",
                "~/Areas/AdminTool/SettingRelation/AddRelation/Scripts/AddRelationController.js",
                "~/Areas/AdminTool/SettingRelation/AddRelation/Scripts/AddRelationController_Options.js",
                "~/Areas/AdminTool/SettingRelation/AddRelation/Scripts/AddRelationController_Update.js",
                "~/Areas/AdminTool/SettingRelation/AddRelation/Scripts/AddRelationController_HotKey.js",
                "~/Areas/AdminTool/SettingRelation/AddRelation/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                 "~/Areas/Admin/SettingRelation/AddRelationStyles",
                "~/Areas/AdminTool/SettingRelation/AddRelation/Styles/AddRelation.less");
        }

        private static void RegisterSettingManagementViewListBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingManagement/ViewListScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/SettingManagement/ViewList/Scripts/ViewListApplication.js",
                "~/Areas/AdminTool/SettingManagement/ViewList/Scripts/ViewListService.js",
                "~/Areas/AdminTool/SettingManagement/ViewList/Scripts/ViewListController.js",
                "~/Areas/AdminTool/SettingManagement/ViewList/Scripts/ViewListController_Options.js",
                "~/Areas/AdminTool/SettingManagement/ViewList/Scripts/ViewListController_Update.js",
                "~/Areas/AdminTool/SettingManagement/ViewList/Scripts/ViewListController_HotKey.js",
                "~/Areas/AdminTool/SettingManagement/ViewList/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingManagement/ViewListStyles",
                "~/Areas/AdminTool/SettingManagement/ViewList/Styles/ViewList.less");
        }

        private static void RegisterSettingManagementInsertSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingManagement/InsertSettingScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/SettingManagement/InsertSetting/Scripts/InsertSettingApplication.js",
                "~/Areas/AdminTool/SettingManagement/InsertSetting/Scripts/InsertSettingService.js",
                "~/Areas/AdminTool/SettingManagement/InsertSetting/Scripts/InsertSettingController.js",
                "~/Areas/AdminTool/SettingManagement/InsertSetting/Scripts/InsertSettingController_Options.js",
                "~/Areas/AdminTool/SettingManagement/InsertSetting/Scripts/InsertSettingController_Update.js",
                "~/Areas/AdminTool/SettingManagement/InsertSetting/Scripts/InsertSettingController_HotKey.js",
                "~/Areas/AdminTool/SettingManagement/InsertSetting/Scripts/Directives/ParentSettingDirective.js",
                "~/Areas/AdminTool/SettingManagement/InsertSetting/Scripts/Directives/KeyPressDirective.js",
                "~/Areas/AdminTool/SettingManagement/InsertSetting/Scripts/Directives/RegisterSharedDirectives.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingManagement/InsertSettingStyles",
                "~/Areas/AdminTool/SettingManagement/InsertSetting/Styles/InsertSetting.less");
        }

        private static void RegisterSettingManagementInsertSubSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingManagement/InsertSubSettingScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Scripts/InsertSubSettingApplication.js",
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Scripts/InsertSubSettingService.js",
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Scripts/InsertSubSettingController.js",
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Scripts/InsertSubSettingController_Options.js",
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Scripts/InsertSubSettingController_Update.js",
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Scripts/InsertSubSettingController_HotKey.js",
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Scripts/Directives/ParentSettingDirective.js",
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Scripts/Directives/KeyPressDirective.js",
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Scripts/Directives/RegisterSharedDirectives.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingManagement/InsertSubSettingStyles",
                "~/Areas/AdminTool/SettingManagement/InsertSubSetting/Styles/InsertSubSetting.less");
        }

        private static void RegisterSettingManagementDeleteSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingManagement/DeleteSettingScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Areas/AdminTool/SettingManagement/DeleteSetting/Scripts/DeleteSettingApplication.js",
                "~/Areas/AdminTool/SettingManagement/DeleteSetting/Scripts/DeleteSettingService.js",
                "~/Areas/AdminTool/SettingManagement/DeleteSetting/Scripts/DeleteSettingController.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingManagement/DeleteSettingStyles",
                "~/Areas/AdminTool/SettingManagement/DeleteSetting/Styles/DeleteSetting.less");
        }

        private static void RegisterSettingManagementEditSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingManagement/EditSettingScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/SettingManagement/EditSetting/Scripts/EditSettingApplication.js",
                "~/Areas/AdminTool/SettingManagement/EditSetting/Scripts/EditSettingService.js",
                "~/Areas/AdminTool/SettingManagement/EditSetting/Scripts/EditSettingController.js",
                "~/Areas/AdminTool/SettingManagement/EditSetting/Scripts/EditSettingController_Options.js",
                "~/Areas/AdminTool/SettingManagement/EditSetting/Scripts/EditSettingController_Update.js",
                "~/Areas/AdminTool/SettingManagement/EditSetting/Scripts/EditSettingController_HotKey.js",
                "~/Areas/AdminTool/SettingManagement/EditSetting/Scripts/Directives/KeyPressDirective.js",
                "~/Areas/AdminTool/SettingManagement/EditSetting/Scripts/Directives/RegisterSharedDirectives.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingManagement/EditSettingStyles",
                "~/Areas/AdminTool/SettingManagement/EditSetting/Styles/EditSetting.less");
        }

        private static void RegisterSettingManagementCN88EditSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingManagementCN88/EditSettingScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/SettingManagementCN88/EditSetting/Scripts/EditSettingApplication.js",
                "~/Areas/AdminTool/SettingManagementCN88/EditSetting/Scripts/EditSettingService.js",
                "~/Areas/AdminTool/SettingManagementCN88/EditSetting/Scripts/EditSettingController.js",
                "~/Areas/AdminTool/SettingManagementCN88/EditSetting/Scripts/EditSettingController_Options.js",
                "~/Areas/AdminTool/SettingManagementCN88/EditSetting/Scripts/EditSettingController_Update.js",
                "~/Areas/AdminTool/SettingManagementCN88/EditSetting/Scripts/EditSettingController_HotKey.js",
                "~/Areas/AdminTool/SettingManagementCN88/EditSetting/Scripts/Directives/KeyPressDirective.js",
                "~/Areas/AdminTool/SettingManagementCN88/EditSetting/Scripts/Directives/RegisterSharedDirectives.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingManagementCN88/EditSettingStyles",
                "~/Areas/AdminTool/SettingManagementCN88/EditSetting/Styles/EditSetting.less");
        }

        private static void RegisterSettingRelationCN88ViewListBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingRelationCN88/ViewListScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/SettingRelationCN88/ViewList/Scripts/ViewListApplication.js",
                "~/Areas/AdminTool/SettingRelationCN88/ViewList/Scripts/ViewListService.js",
                "~/Areas/AdminTool/SettingRelationCN88/ViewList/Scripts/ViewListController.js",
                "~/Areas/AdminTool/SettingRelationCN88/ViewList/Scripts/ViewListController_Options.js",
                "~/Areas/AdminTool/SettingRelationCN88/ViewList/Scripts/ViewListController_Update.js",
                "~/Areas/AdminTool/SettingRelationCN88/ViewList/Scripts/ViewListController_HotKey.js",
                "~/Areas/AdminTool/SettingRelationCN88/ViewList/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingRelationCN88/ViewListStyles",
                "~/Areas/AdminTool/SettingRelationCN88/ViewList/Styles/ViewList.less");
        }

        private static void RegisterAddSettingRelationCN88Bundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingRelationCN88/AddRelationScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/SettingRelationCN88/AddRelation/Scripts/AddRelationApplication.js",
                "~/Areas/AdminTool/SettingRelationCN88/AddRelation/Scripts/AddRelationService.js",
                "~/Areas/AdminTool/SettingRelationCN88/AddRelation/Scripts/AddRelationController.js",
                "~/Areas/AdminTool/SettingRelationCN88/AddRelation/Scripts/AddRelationController_Options.js",
                "~/Areas/AdminTool/SettingRelationCN88/AddRelation/Scripts/AddRelationController_Update.js",
                "~/Areas/AdminTool/SettingRelationCN88/AddRelation/Scripts/AddRelationController_HotKey.js",
                "~/Areas/AdminTool/SettingRelationCN88/AddRelation/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingRelationCN88/AddRelationStyles",
                "~/Areas/AdminTool/SettingRelationCN88/AddRelation/Styles/AddRelation.less");
        }

        private static void RegisterSettingManagementCN88ViewListBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingManagementCN88/ViewListScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/SettingManagementCN88/ViewList/Scripts/ViewListApplication.js",
                "~/Areas/AdminTool/SettingManagementCN88/ViewList/Scripts/ViewListService.js",
                "~/Areas/AdminTool/SettingManagementCN88/ViewList/Scripts/ViewListController.js",
                "~/Areas/AdminTool/SettingManagementCN88/ViewList/Scripts/ViewListController_Options.js",
                "~/Areas/AdminTool/SettingManagementCN88/ViewList/Scripts/ViewListController_Update.js",
                "~/Areas/AdminTool/SettingManagementCN88/ViewList/Scripts/ViewListController_HotKey.js",
                "~/Areas/AdminTool/SettingManagementCN88/ViewList/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingManagementCN88/ViewListStyles",
                "~/Areas/AdminTool/SettingManagementCN88/ViewList/Styles/ViewList.less");
        }

        private static void RegisterSettingManagementCN88InsertSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingManagementCN88/InsertSettingScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSetting/Scripts/InsertSettingApplication.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSetting/Scripts/InsertSettingService.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSetting/Scripts/InsertSettingController.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSetting/Scripts/InsertSettingController_Options.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSetting/Scripts/InsertSettingController_Update.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSetting/Scripts/InsertSettingController_HotKey.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSetting/Scripts/Directives/ParentSettingDirective.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSetting/Scripts/Directives/KeyPressDirective.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSetting/Scripts/Directives/RegisterSharedDirectives.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingManagementCN88/InsertSettingStyles",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSetting/Styles/InsertSetting.less");
        }

        private static void RegisterSettingManagementCN88InsertSubSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingManagementCN88/InsertSubSettingScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSubSetting/Scripts/InsertSubSettingApplication.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSubSetting/Scripts/InsertSubSettingService.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSubSetting/Scripts/InsertSubSettingController.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSubSetting/Scripts/InsertSubSettingController_Options.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSubSetting/Scripts/InsertSubSettingController_Update.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSubSetting/Scripts/InsertSubSettingController_HotKey.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSubSetting/Scripts/Directives/ParentSettingDirective.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSubSetting/Scripts/Directives/KeyPressDirective.js",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSubSetting/Scripts/Directives/RegisterSharedDirectives.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingManagementCN88/InsertSubSettingStyles",
                "~/Areas/AdminTool/SettingManagementCN88/InsertSubSetting/Styles/InsertSubSetting.less");
        }

        private static void RegisterSettingManagementCN88DeleteSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Admin/SettingManagementCN88/DeleteSettingScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Areas/AdminTool/SettingManagementCN88/DeleteSetting/Scripts/DeleteSettingApplication.js",
                "~/Areas/AdminTool/SettingManagementCN88/DeleteSetting/Scripts/DeleteSettingService.js",
                "~/Areas/AdminTool/SettingManagementCN88/DeleteSetting/Scripts/DeleteSettingController.js"));

            bundles.AddAndInclude(
                "~/Areas/Admin/SettingManagementCN88/DeleteSettingStyles",
                "~/Areas/AdminTool/SettingManagementCN88/DeleteSetting/Styles/DeleteSetting.less");
        }

        private static void RegisterCommunicationSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Bundles/CommToolSettingScripts").Include(
                "~/Scripts/ShareScripts/Applications/ResultMessage.js",
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/Directives/multileveldropdown/multileveldropdown.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js"));
        }
    }
}