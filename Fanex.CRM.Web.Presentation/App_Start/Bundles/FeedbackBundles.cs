namespace Fanex.CRM.Web.View.AppStart
{
    using System.Web.Optimization;

    public static class FeedbackBundles
    {
        public static void RegisterAll(BundleCollection bundles)
        {
            RegisterManageFeedbackBundles(bundles);
            RegisterLatestFeedbackBundles(bundles);
            RegisterAddCustomerFeedbackBundles(bundles);
            RegisterAddGuestFeedbackBundles(bundles);
            RegisterDeletedFeedbackBundles(bundles);
            RegisterUpdateCustomerFeedbackBundles(bundles);
            RegisterCreateMultipleLogsBundles(bundles);
            RegisterEditMultipleLogsBundles(bundles);
            RegisterEditMultipleLogStatusesBundles(bundles);
        }

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

            bundles.AddAndInclude(
                "~/Areas/Feedbacks/ManageFeedback/ManageFeedbackStyles",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css",
                "~/Areas/Feedbacks/ManageFeedback/Styles/StyleSheet.less");
        }

        private static void RegisterAddCustomerFeedbackBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/AddCustomerFeedback/AddCustomerFeedbackScripts")
                .Include(
                    "~/Scripts/AngularJS/sanitize.js",
                    "~/Scripts/ShareScripts/Applications/BlockUI.js",
                    "~/wwwroot/sources/libs/FanexGrid/moment.js",
                    "~/Scripts/ShareScripts/SettingManager.js",
                    "~/Scripts/ShareScripts/HotKeyManager.js",
                    "~/Scripts/ShareScripts/DirectiveManager.js",
                    "~/Scripts/ShareScripts/Directives/DropdownList.js",
                    "~/Scripts/ShareScripts/Directives/DropdownSite.js",
                    "~/Scripts/ShareScripts/Directives/DatePicker.js",
                    "~/Scripts/ShareScripts/Directives/DateTimePicker.js",
                    "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                    "~/Scripts/ShareScripts/Directives/HtmlArea.js",
                    "~/Scripts/ShareScripts/Directives/RawHtml.js",
                    "~/Scripts/ShareScripts/Directives/TipsyDirective.js",
                    "~/Scripts/ShareScripts/Directives/RelatedCustomerResult.js",
                    "~/Scripts/Directives/multileveldropdown/multileveldropdown.js",
                    "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                    "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                    "~/Areas/Feedbacks/AddCustomerFeedback/Scripts/AddCustomerFeedbackApplication.js",
                    "~/Areas/Feedbacks/AddCustomerFeedback/Scripts/AddCustomerFeedbackService.js",
                    "~/Areas/Feedbacks/AddCustomerFeedback/Scripts/AddCustomerFeedbackController_Options.js",
                    "~/Areas/Feedbacks/AddCustomerFeedback/Scripts/AddCustomerFeedbackController_Update.js",
                    "~/Areas/Feedbacks/AddCustomerFeedback/Scripts/AddCustomerFeedbackController_HotkeyFunctions.js",
                    "~/Areas/Feedbacks/AddCustomerFeedback/Scripts/AddCustomerFeedbackController.js",
                    "~/Areas/Feedbacks/AddCustomerFeedback/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
               "~/Areas/Feedbacks/AddCustomerFeedback/AddCustomerFeedbackStyles",
               "~/Areas/Feedbacks/AddCustomerFeedback/Styles/AddCustomerFeedback.less");
        }

        private static void RegisterAddGuestFeedbackBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/Areas/Feedbacks/AddGuestFeedback/AddGuestFeedbackScripts")
                .Include(
                    "~/Scripts/AngularJS/sanitize.js",
                    "~/Scripts/ShareScripts/Applications/BlockUI.js",
                    "~/wwwroot/sources/libs/FanexGrid/moment.js",
                    "~/Scripts/ShareScripts/HotKeyManager.js",
                    "~/Scripts/ShareScripts/DirectiveManager.js",
                    "~/Scripts/ShareScripts/Directives/DropdownList.js",
                    "~/Scripts/ShareScripts/Directives/DatePicker.js",
                    "~/Scripts/ShareScripts/Directives/DateTimePicker.js",
                    "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                    "~/Scripts/ShareScripts/Directives/HtmlArea.js",
                    "~/Scripts/ShareScripts/Directives/RawHtml.js",
                    "~/Scripts/ShareScripts/Directives/TipsyDirective.js",
                    "~/Scripts/ShareScripts/Directives/RelatedCustomerResult.js",
                    "~/Scripts/Directives/multileveldropdown/multileveldropdown.js",
                    "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                    "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                    "~/Scripts/ShareScripts/HotKeyManager.js",
                    "~/Areas/Feedbacks/AddGuestFeedback/Scripts/AddGuestFeedbackApplication.js",
                    "~/Areas/Feedbacks/AddGuestFeedback/Scripts/AddGuestFeedbackService.js",
                    "~/Areas/Feedbacks/AddGuestFeedback/Scripts/AddGuestFeedbackController_Options.js",
                    "~/Areas/Feedbacks/AddGuestFeedback/Scripts/AddGuestFeedbackController_Update.js",
                    "~/Areas/Feedbacks/AddGuestFeedback/Scripts/AddGuestFeedbackController_HotkeyFunctions.js",
                    "~/Areas/Feedbacks/AddGuestFeedback/Scripts/AddGuestFeedbackController.js",
                    "~/Areas/Feedbacks/AddGuestFeedback/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
               "~/Areas/Feedbacks/AddGuestFeedback/AddGuestFeedbackStyles",
               "~/Areas/Feedbacks/AddGuestFeedback/Styles/AddGuestFeedback.less");
        }

        private static void RegisterLatestFeedbackBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/LatestFeedback/LatestFeedbackScripts").Include(
                "~/Scripts/Tipsy-Tooltip/jquery.tipsy.js",
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Feedbacks/LatestFeedback/Scripts/LatestFeedbackApplication.js",
                "~/Areas/Feedbacks/LatestFeedback/Scripts/LatestFeedbackController.js",
                "~/Areas/Feedbacks/LatestFeedback/Scripts/LatestFeedbackService.js",
                "~/Areas/Feedbacks/LatestFeedback/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Feedbacks/LatestFeedback/LatestFeedbackStyles",
                "~/Scripts/Tipsy-Tooltip/tipsy.css",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Areas/Feedbacks/LatestFeedback/Styles/StyleSheet.less");
        }

        private static void RegisterDeletedFeedbackBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/DeletedFeedback/DeletedFeedbackScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Feedbacks/DeletedFeedback/Scripts/DeletedFeedbackApplication.js",
                "~/Areas/Feedbacks/DeletedFeedback/Scripts/DeletedFeedbackController.js",
                "~/Areas/Feedbacks/DeletedFeedback/Scripts/DeletedFeedbackService.js",
                "~/Areas/Feedbacks/DeletedFeedback/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Feedbacks/DeletedFeedback/DeletedFeedbackStyles",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css",
                "~/Areas/Feedbacks/DeletedFeedback/Styles/StyleSheet.less");
        }

        private static void RegisterUpdateCustomerFeedbackBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
               "~/Areas/Feedbacks/UpdateCustomerFeedback/UpdateCustomerFeedbackStyles",
               "~/Areas/Feedbacks/UpdateCustomerFeedback/Styles/Styles.less");

            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/UpdateCustomerFeedback/UpdateCustomerFeedbackScripts")
                .Include(
                    "~/Scripts/AngularJS/sanitize.js",
                    "~/Scripts/ShareScripts/Applications/BlockUI.js",
                    "~/Scripts/ShareScripts/Applications/AlertBox.js",
                    "~/wwwroot/sources/libs/FanexGrid/moment.js",
                    "~/Scripts/ShareScripts/HotKeyManager.js",
                    "~/Scripts/ShareScripts/SettingManager.js",
                    "~/Scripts/ShareScripts/DirectiveManager.js",
                    "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                    "~/Scripts/ShareScripts/Directives/HtmlArea.js",
                    "~/Scripts/ShareScripts/Directives/RawHtml.js",
                    "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                    "~/Scripts/ShareScripts/Directives/DropdownList.js",
                    "~/Scripts/ShareScripts/Directives/RelatedCustomerResult.js",
                    "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                    "~/Scripts/Directives/multileveldropdown/multileveldropdown.js",
                    "~/Areas/Feedbacks/UpdateCustomerFeedback/Scripts/UpdateCustomerFeedbackApplication.js",
                    "~/Areas/Feedbacks/UpdateCustomerFeedback/Scripts/UpdateCustomerFeedbackService.js",
                    "~/Areas/Feedbacks/UpdateCustomerFeedback/Scripts/UpdateCustomerFeedbackController.js",
                    "~/Areas/Feedbacks/UpdateCustomerFeedback/Scripts/Directives/*.js"));
        }

        private static void RegisterCreateMultipleLogsBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/CreateMultipleLogsScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/SettingManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/Directives/RelatedCustomerResult.js",
                "~/Scripts/Directives/selectwithsearch/selectwithsearch.js",
                "~/Scripts/Directives/multileveldropdown/multileveldropdown.js",
                "~/Areas/Feedbacks/CreateMultipleLogs/Scripts/CreateMultipleLogsApplication.js",
                "~/Areas/Feedbacks/CreateMultipleLogs/Scripts/CreateMultipleLogsOption.js",
                "~/Areas/Feedbacks/CreateMultipleLogs/Scripts/CreateMultipleLogsService.js",
                "~/Areas/Feedbacks/CreateMultipleLogs/Scripts/CreateMultipleLogsController.js",
                "~/Areas/Feedbacks/CreateMultipleLogs/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Feedbacks/CreateMultipleLogsStyles",
                "~/Areas/Feedbacks/CreateMultipleLogs/Styles/CreateMultipleLogs.less");
        }

        private static void RegisterEditMultipleLogsBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/EditMultipleLogsScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/SettingManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/Directives/RelatedCustomerResult.js",
                "~/Scripts/Directives/selectwithsearch/selectwithsearch.js",
                "~/Scripts/Directives/multileveldropdown/multileveldropdown.js",
                "~/Areas/Feedbacks/EditMultipleLogs/Scripts/EditMultipleLogsApplication.js",
                "~/Areas/Feedbacks/EditMultipleLogs/Scripts/EditMultipleLogsOption.js",
                "~/Areas/Feedbacks/EditMultipleLogs/Scripts/EditMultipleLogsService.js",
                "~/Areas/Feedbacks/EditMultipleLogs/Scripts/EditMultipleLogsController.js",
                "~/Areas/Feedbacks/EditMultipleLogs/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Feedbacks/EditMultipleLogsStyles",
                "~/Areas/Feedbacks/EditMultipleLogs/Styles/EditMultipleLogs.less");
        }

        private static void RegisterEditMultipleLogStatusesBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/EditMultipleLogStatusesScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/SettingManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/Directives/RelatedCustomerResult.js",
                "~/Areas/Feedbacks/EditMultipleLogStatuses/Scripts/EditMultipleLogStatusesApplication.js",
                "~/Areas/Feedbacks/EditMultipleLogStatuses/Scripts/EditMultipleLogStatusesService.js",
                "~/Areas/Feedbacks/EditMultipleLogStatuses/Scripts/EditMultipleLogStatusesController.js",
                "~/Areas/Feedbacks/EditMultipleLogStatuses/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Feedbacks/EditMultipleLogStatusesStyles",
                "~/Areas/Feedbacks/EditMultipleLogStatuses/Styles/EditMultipleLogStatuses.less");
        }
    }
}