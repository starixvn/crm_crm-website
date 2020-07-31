namespace Fanex.CRM.Web.View.AppStart
{
    using System.Web.Optimization;

    public static class FeedbackBundles
    {
        private const string BlockUI = "~/Scripts/ShareScripts/Applications/BlockUI.js";
        private const string HotKeyManager = "~/Scripts/ShareScripts/HotKeyManager.js";
        private const string DirectiveManager = "~/Scripts/ShareScripts/DirectiveManager.js";
        private const string DropdownList = "~/Scripts/ShareScripts/Directives/DropdownList.js";
        private const string RegisterTemplate = "~/Scripts/ShareScripts/Directives/RegisterTemplate.js";
        private const string MultiLevelDropdown = "~/Scripts/Directives/multileveldropdown/multileveldropdown.js";
        private const string FanexGridCss = "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.css";
        private const string CustomizeFanexGridLess = "~/Content/site/customizeFanexGridV2.less";
        private const string FanexPaginationCss = "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css";
        private const string DatePicker = "~/Scripts/ShareScripts/Directives/DatePicker.js";
        private const string DefaultFocus = "~/Scripts/ShareScripts/Directives/DefaultFocus.js";
        private const string SettingManager = "~/Scripts/ShareScripts/SettingManager.js";
        private const string RawHtml = "~/Scripts/ShareScripts/Directives/RawHtml.js";
        private const string RelatedCustomerResult = "~/Scripts/ShareScripts/Directives/RelatedCustomerResult.js";

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
            RegisterQuickUpdateFeedbackBundles(bundles);
        }

        private static void RegisterManageFeedbackBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/ManageFeedback/ManageFeedbackScripts").Include(
                BlockUI,
                DirectiveManager,
                DropdownList,
                DatePicker,
                DefaultFocus,
                RegisterTemplate,
                "~/wwwroot/dist/js/manage.feedback.js"));
        }

        private static void RegisterAddCustomerFeedbackBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/AddCustomerFeedback/AddCustomerFeedbackScripts")
                .Include(
                    "~/Scripts/AngularJS/angular-sanitize.js",
                    BlockUI,
                    ScriptPathConstant.MomentScriptPath,
                    SettingManager,
                    HotKeyManager,
                    DirectiveManager,
                    DropdownList,
                    "~/Scripts/ShareScripts/Directives/DropdownSite.js",
                    DatePicker,
                    "~/Scripts/ShareScripts/Directives/DateTimePicker.js",
                    "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                    "~/Scripts/ShareScripts/Directives/HtmlArea.js",
                    RawHtml,
                    RelatedCustomerResult,
                    MultiLevelDropdown,
                    DefaultFocus,
                    RegisterTemplate,
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
                    "~/Scripts/AngularJS/angular-sanitize.js",
                    BlockUI,
                    ScriptPathConstant.MomentScriptPath,
                    HotKeyManager,
                    DirectiveManager,
                    DropdownList,
                    DatePicker,
                    "~/Scripts/ShareScripts/Directives/DateTimePicker.js",
                    "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                    "~/Scripts/ShareScripts/Directives/HtmlArea.js",
                    RawHtml,
                    RelatedCustomerResult,
                    MultiLevelDropdown,
                    DefaultFocus,
                    RegisterTemplate,
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
                BlockUI,
                RegisterTemplate,
                "~/Areas/Feedbacks/LatestFeedback/Scripts/LatestFeedbackApplication.js",
                "~/Areas/Feedbacks/LatestFeedback/Scripts/LatestFeedbackController.js",
                "~/Areas/Feedbacks/LatestFeedback/Scripts/LatestFeedbackService.js",
                "~/Areas/Feedbacks/LatestFeedback/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Feedbacks/LatestFeedback/LatestFeedbackStyles",
                "~/Scripts/Tipsy-Tooltip/tipsy.css",
                FanexGridCss,
                CustomizeFanexGridLess,
                "~/Areas/Feedbacks/LatestFeedback/Styles/StyleSheet.less");
        }

        private static void RegisterDeletedFeedbackBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/DeletedFeedback/DeletedFeedbackScripts").Include(
                BlockUI,
                DirectiveManager,
                DropdownList,
                DatePicker,
                DefaultFocus,
                RegisterTemplate,
                "~/Areas/Feedbacks/DeletedFeedback/Scripts/DeletedFeedbackApplication.js",
                "~/Areas/Feedbacks/DeletedFeedback/Scripts/DeletedFeedbackController.js",
                "~/Areas/Feedbacks/DeletedFeedback/Scripts/DeletedFeedbackService.js",
                "~/Areas/Feedbacks/DeletedFeedback/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Feedbacks/DeletedFeedback/DeletedFeedbackStyles",
                FanexGridCss,
                CustomizeFanexGridLess,
                FanexPaginationCss,
                "~/Areas/Feedbacks/DeletedFeedback/Styles/StyleSheet.less");
        }

        private static void RegisterUpdateCustomerFeedbackBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
               "~/Areas/Feedbacks/UpdateCustomerFeedback/UpdateCustomerFeedbackStyles",
               "~/Areas/Feedbacks/UpdateCustomerFeedback/Styles/Styles.less");

            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/UpdateCustomerFeedback/UpdateCustomerFeedbackScripts")
                .Include(
                    "~/Scripts/AngularJS/angular-sanitize.js",
                    BlockUI,
                    "~/Scripts/ShareScripts/Applications/AlertBox.js",
                    ScriptPathConstant.MomentScriptPath,
                    HotKeyManager,
                    SettingManager,
                    DirectiveManager,
                    "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                    "~/Scripts/ShareScripts/Directives/HtmlArea.js",
                    RawHtml,
                    DefaultFocus,
                    DropdownList,
                    RelatedCustomerResult,
                    RegisterTemplate,
                    MultiLevelDropdown,
                    "~/Areas/Feedbacks/UpdateCustomerFeedback/Scripts/UpdateCustomerFeedbackApplication.js",
                    "~/Areas/Feedbacks/UpdateCustomerFeedback/Scripts/UpdateCustomerFeedbackService.js",
                    "~/Areas/Feedbacks/UpdateCustomerFeedback/Scripts/UpdateCustomerFeedbackController.js",
                    "~/Areas/Feedbacks/UpdateCustomerFeedback/Scripts/Directives/*.js"));
        }

        private static void RegisterCreateMultipleLogsBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/CreateMultipleLogsScripts").Include(
                ScriptPathConstant.MomentScriptPath,
                "~/Scripts/jHtmlArea/jHtmlArea-0.7.0.js",
                "~/Scripts/jHtmlArea/jHtmlArea.ColorPickerMenu-0.7.0.js",
                "~/Scripts/Jlinq/jlinq/jlinq.jquery.js",
                "~/Scripts/autocomplete/jquery.highlight-3.js",
                "~/Scripts/autocomplete/jquery.autocomplete.client.js",
                "~/Scripts/common.js",
                BlockUI,
                HotKeyManager,
                SettingManager,
                DirectiveManager,
                DropdownList,
                RegisterTemplate,
                "~/Scripts/Directives/selectwithsearch/selectwithsearch.js",
                MultiLevelDropdown));

            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/CreateMultipleLogsDirectiveScripts").Include(
                "~/wwwroot/sources/js/pages/feedbacks/create-multiple-logs/directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Feedbacks/CreateMultipleLogsStyles",
                "~/Scripts/jHtmlArea/jHtmlArea.css",
                "~/Scripts/jHtmlArea/jHtmlArea.ColorPickerMenu.css",
                "~/Scripts/autocomplete/autocomplete.css",
                "~/Scripts/Directives/selectwithsearch/selectwithsearch.css",
                "~/Scripts/Directives/multileveldropdown/multileveldropdown.css");
        }

        private static void RegisterEditMultipleLogsBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/EditMultipleLogsScripts").Include(
                BlockUI,
                HotKeyManager,
                SettingManager,
                DirectiveManager,
                DropdownList,
                RegisterTemplate,
                RelatedCustomerResult,
                RawHtml,
                "~/Scripts/Directives/selectwithsearch/selectwithsearch.js",
                MultiLevelDropdown,
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
                BlockUI,
                HotKeyManager,
                SettingManager,
                DirectiveManager,
                RegisterTemplate,
                RelatedCustomerResult,
                "~/Areas/Feedbacks/EditMultipleLogStatuses/Scripts/EditMultipleLogStatusesApplication.js",
                "~/Areas/Feedbacks/EditMultipleLogStatuses/Scripts/EditMultipleLogStatusesService.js",
                "~/Areas/Feedbacks/EditMultipleLogStatuses/Scripts/EditMultipleLogStatusesController.js",
                "~/Areas/Feedbacks/EditMultipleLogStatuses/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Feedbacks/EditMultipleLogStatusesStyles",
                "~/Areas/Feedbacks/EditMultipleLogStatuses/Styles/EditMultipleLogStatuses.less");
        }

        private static void RegisterQuickUpdateFeedbackBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Feedbacks/QuickUpdateFeedbackScripts").Include(
                BlockUI,
                HotKeyManager,
                SettingManager,
                DirectiveManager,
                RegisterTemplate,
                RelatedCustomerResult,
                "~/Areas/Feedbacks/QuickUpdate/Scripts/QuickUpdateFeedbackApplication.js",
                "~/Areas/Feedbacks/QuickUpdate/Scripts/QuickUpdateFeedbackController.js",
                "~/Areas/Feedbacks/QuickUpdate/Scripts/QuickUpdateFeedbackService.js"));
        }
    }
}