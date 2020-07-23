namespace Fanex.CRM.Web.View.AppStart
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        private const string BlockUI = "~/Scripts/ShareScripts/Applications/BlockUI.js";
        private const string HotKeyManager = "~/Scripts/ShareScripts/HotKeyManager.js";
        private const string Moment = "~/wwwroot/sources/libs/FanexGrid/moment.js";
        private const string FanexPagination = "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.js";
        private const string FanexGrid = "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.js";
        private const string SettingManager = "~/Scripts/ShareScripts/SettingManager.js";
        private const string DirectiveManager = "~/Scripts/ShareScripts/DirectiveManager.js";
        private const string DropdownList = "~/Scripts/ShareScripts/Directives/DropdownList.js";
        private const string RegisterTemplate = "~/Scripts/ShareScripts/Directives/RegisterTemplate.js";
        private const string MultiLevelDropdown = "~/Scripts/Directives/multileveldropdown/multileveldropdown.js";
        private const string FanexGridCss = "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.css";
        private const string CustomizeFanexGridLess = "~/Content/site/customizeFanexGridV2.less";
        private const string FanexPaginationCss = "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css";
        private const string DatePicker = "~/Scripts/ShareScripts/Directives/DatePicker.js";
        private const string DefaultFocus = "~/Scripts/ShareScripts/Directives/DefaultFocus.js";
        private const string MultiSelect = "~/Scripts/ShareScripts/Directives/MultiSelect.js";
        private const string MesageResult = "~/Scripts/ShareScripts/MessageResult.js";

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/headload").Include("~/Scripts/head.load.js"));

            bundles.Add(new ScriptBundle("~/scripts/respond").Include("~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/Scripts/PopUp").Include(
                "~/Scripts/showhidepopup.js",
                "~/Scripts/dialogbox.js"));

            bundles.AddAndInclude(
                "~/Views/HomeStyles",
                "~/Scripts/Tipsy-Tooltip/tipsy.css");

            bundles.Add(new ScriptBundle("~/Views/HomeScripts").Include(
                HotKeyManager,
                "~/Views/Home/Scripts/MenuManager.js",
                "~/Views/Home/Scripts/LeftPanelManager.js",
                "~/Views/Home/Scripts/HomeManager.js",
                Moment));

            bundles.Add(new ScriptBundle("~/Scripts/AngularJSLibrary").Include(
                "~/Scripts/AngularJS/angular.min.js",
                "~/Scripts/AngularJS/ui-bootstrap-tpls-1.0.3.min.js"));

            bundles.AddAndInclude(
                "~/Styles/FanexMultipleSelect",
                "~/wwwroot/sources/libs/MultiSelect/fanex.multiSelect.css",
                "~/Content/site/customizeMultiSelect.less",
                "~/Areas/Customer/DetailInfo/Styles/StyleSheet.less");

            bundles.Add(new ScriptBundle("~/Scripts/FanexMultipleSelect").Include(
                "~/wwwroot/sources/libs/MultiSelect/fanex.multiSelect.js"));

            bundles.Add(new ScriptBundle("~/Scripts/FanexGridScripts").Include(
                FanexPagination,
                FanexGrid,
                Moment));

            bundles.Add(new ScriptBundle("~/Views/Home/Dashboard/Scripts").Include(
                "~/Views/Home/Scripts/DashboardManager.js"));

            RegisterBundles_Common(ref bundles);
            RegisterBundles_Customer_ShortInfo(ref bundles);
            RegisterBundles_Customer_DetailInfo(ref bundles);
            RegisterBundles_TimePicker(ref bundles);
            RegisterBundles_CommunicationLog_ViewLog(ref bundles);
            RegisterBundles_CommunicationLog_Edit(ref bundles);
            RegisterBundles_CommunicationLog_Insert(ref bundles);
            RegisterBundles_CommunicationLog_ViewDetailById(ref bundles);

            RegisterBundles_Redemption_ViewLog(ref bundles);
            RegisterBundles_Redemption_InsertLog(ref bundles);
            RegisterBundles_Redemption_EditLog(ref bundles);
            RegisterBundles_Remark(ref bundles);
            RegisterBundles_ActivityLog(ref bundles);
            RegisterBundles_SettingLog(ref bundles);
            RegisterBundles_Dashboard_ViewInfo(ref bundles);
            RegisterBundles_Dashboard_VerifyFlag(ref bundles);
            RegisterBundles_Report_DynamicReport(ref bundles);
            RegisterBundles_Report_RedemptionStatistics(ref bundles);
            RegisterBundles_Report_CustomerReport(ref bundles);
            RegisterBundles_Report_DirectCustomerReport(ref bundles);
            RegisterBundles_Chart(ref bundles);
            RegisterBundles_User_LogIn(ref bundles);

            RegisterBundles_DownLoadFile(ref bundles);
            RegisterBundles_Report_StatisticReport(ref bundles);
            RegisterBundles_Report_SettlementReport(ref bundles);

            RegisterBundles_ResultMessage(ref bundles);
            RegisterBundles_Numeral(ref bundles);
            RegisterBundles_Report_SettlementReport(ref bundles);
            RegisterBundles_Report_StatisticReport(ref bundles);
            RegisterBundles_Report_WagerReport(ref bundles);
            RegisterBundles_Report_SummaryReport(ref bundles);

            RegisterBundles_PicManagement(ref bundles);
            RegisterBundles_PicOfHeadline(ref bundles);
            RegisterBundles_ChangeLog(ref bundles);

            FeedbackBundles.RegisterAll(bundles);
            ReportBundles.RegisterBundles(bundles);
            ReportBundles.RegisterBundles(bundles);
            ReportBundles.RegisterBundles(bundles);

            AdminToolBundles.RegisterBundles(bundles);
            InformHeadlineBundles.RegisterBundles(bundles);

            RegisterBundles_FanexGrid(bundles);
        }

        private static void RegisterBundles_Common(ref BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/site/common",
                "~/content/bootstrap/css/bootstrap.css",
                "~/Content/site/common.less",
                "~/Content/site/Message.less",
                "~/Content/vendors/jqueryui/jquery-ui.min.css",
                "~/Content/site/customizeJqueryUI.less");

            bundles.Add(new ScriptBundle("~/Scripts/CommonScripts").Include(
                "~/Scripts/JQuery/jquery-1.8.2.js",
                "~/Scripts/json2.js",
                "~/Scripts/jquery-ui-1.8.14.custom.js",
                "~/Scripts/ShareScripts/CRMConstants.js",
                "~/Scripts/ShareScripts/SharedScriptManager.js",
                "~/Scripts/ShareScripts/NameMapping.js",
                "~/Scripts/ShareScripts/FilterScripts.js",
                "~/Scripts/ShareScripts/AutoTextScripts.js",
                "~/Scripts/ShareScripts/KeyboardManager.js"));
        }

        private static void RegisterBundles_Customer_ShortInfo(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/ShortCustomerInfo/Scripts").Include(
                "~/Scripts/jscookies.js",
                SettingManager,
                HotKeyManager,
                "~/Scripts/Directives/selectwithsearch/selectwithsearch.js",
                "~/Areas/Customer/ShortInfo/Scripts/CustomerApplication.js",
                "~/Areas/Customer/ShortInfo/Scripts/CustomerService.js",
                "~/Areas/Customer/ShortInfo/Scripts/CustomerController.js",
                "~/Areas/Customer/ShortInfo/Scripts/Directives/*.js",
                "~/Areas/Customer/ShortInfo/Scripts/CustomerManager.js"));

            bundles.AddAndInclude(
                "~/Areas/ShortCustomerInfo/Styles",
                "~/Content/site/customizeMultiSelect.less",
                "~/Areas/Customer/ShortInfo/Styles/StyleSheet.less");
        }

        private static void RegisterBundles_Customer_DetailInfo(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/DetailCustomerInfo/Scripts").Include(
                "~/Scripts/jscookies.js",
                HotKeyManager,
                "~/Scripts/Directives/selectwithsearch/selectwithsearch.js",
                "~/Areas/Customer/DetailInfo/Scripts/CustomerDetailApplication.js",
                "~/Areas/Customer/DetailInfo/Scripts/CustomerDetailService.js",
                "~/Areas/Customer/DetailInfo/Scripts/CustomerDetailController.js",
                "~/Areas/Customer/DetailInfo/Scripts/CustomerDetailDirective.js",
                "~/Areas/Customer/DetailInfo/Scripts/Directives/*.js"));
        }

        private static void RegisterBundles_TimePicker(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/TimePicker").Include(
                     "~/Scripts/jquery-ui-timepicker-addon.js"));
        }

        private static void RegisterBundles_CommunicationLog_ViewLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Communication/ViewLogScripts").Include(
                HotKeyManager,
                "~/Areas/Communication/ViewLog/Scripts/CommunicationApplication.js",
                "~/Areas/Communication/ViewLog/Scripts/CommunicationService.js",
                "~/Areas/Communication/ViewLog/Scripts/CommunicationController.js",
                "~/Areas/Communication/ViewLog/Scripts/CommunicationDirective.js",
                "~/Areas/Communication/ViewLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Communication/ViewLogStyles",
                "~/Areas/Communication/ViewLog/Styles/Communication.less");
        }

        private static void RegisterBundles_CommunicationLog_Edit(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Communication/EditLogScripts").Include(
                BlockUI,
                HotKeyManager,
                SettingManager,
                DirectiveManager,
                DropdownList,
                RegisterTemplate,
                MultiLevelDropdown,
                "~/Scripts/ShareScripts/Directives/RelatedCustomerResult.js",
                "~/Areas/Communication/EditLog/Scripts/EditLogApplication.js",
                "~/Areas/Communication/EditLog/Scripts/EditLogOption.js",
                "~/Areas/Communication/EditLog/Scripts/EditLogService.js",
                "~/Areas/Communication/EditLog/Scripts/EditLogController.js",
                "~/Areas/Communication/EditLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Communication/EditLogStyles",
                "~/Areas/Communication/EditLog/Styles/EditCommunicationLog.less");
        }

        private static void RegisterBundles_CommunicationLog_Insert(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Communication/InsertLogScripts").Include(
                BlockUI,
                HotKeyManager,
                DirectiveManager,
                DropdownList,
                RegisterTemplate,
                "~/Scripts/ShareScripts/Directives/RelatedCustomerResult.js",
                MultiLevelDropdown,
                "~/Areas/Communication/InsertLog/Scripts/InsertLogApplication.js",
                "~/Areas/Communication/InsertLog/Scripts/InsertLogOption.js",
                "~/Areas/Communication/InsertLog/Scripts/InsertLogService.js",
                "~/Areas/Communication/InsertLog/Scripts/InsertLogController.js",
                "~/Areas/Communication/InsertLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Communication/InsertLogStyles",
                "~/Areas/Communication/InsertLog/Styles/InsertCommunicationLog.less");
        }

        private static void RegisterBundles_CommunicationLog_ViewDetailById(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Communication/ViewDetailByIdScripts").Include(
                "~/Scripts/AngularJS/angular-sanitize.js",
                BlockUI,
                SettingManager,
                DirectiveManager,
                DropdownList,
                "~/Scripts/ShareScripts/Directives/DropdownSite.js",
                "~/Scripts/ShareScripts/Directives/HtmlArea.js",
                "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                "~/Scripts/ShareScripts/Directives/DateTimePicker.js",
                RegisterTemplate,
                "~/Scripts/ShareScripts/Directives/RelatedCustomerResult.js",
                MultiLevelDropdown,
                "~/Areas/Communication/ViewDetailById/Scripts/ViewDetailByIdApplication.js",
                "~/Areas/Communication/ViewDetailById/Scripts/ViewDetailByIdOption.js",
                "~/Areas/Communication/ViewDetailById/Scripts/ViewDetailByIdService.js",
                "~/Areas/Communication/ViewDetailById/Scripts/ViewDetailByIdController.js",
                "~/Areas/Communication/ViewDetailById/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Communication/ViewDetailByIdStyles",
                "~/Areas/Communication/ViewDetailById/Styles/ViewDetailById.less");
        }

        private static void RegisterBundles_Redemption_ViewLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Redemption/ViewLogScripts").Include(
                HotKeyManager,
                "~/Areas/Redemption/ViewLog/Scripts/RedemptionViewLogApplication.js",
                "~/Areas/Redemption/ViewLog/Scripts/RedemptionViewLogService.js",
                "~/Areas/Redemption/ViewLog/Scripts/RedemptionViewLogController.js",
                "~/Areas/Redemption/ViewLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Redemption/ViewLogStyles",
                "~/Areas/Redemption/ViewLog/Styles/Redemption.less");
        }

        private static void RegisterBundles_Redemption_InsertLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Redemption/InsertLogScripts").Include(
                HotKeyManager,
                "~/Areas/Redemption/InsertLog/Scripts/InsertLogApplication.js",
                "~/Areas/Redemption/InsertLog/Scripts/InsertLogService.js",
                "~/Areas/Redemption/InsertLog/Scripts/InsertLogController.js",
                "~/Areas/Redemption/InsertLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Redemption/InsertLogStyles",
                "~/Areas/Redemption/InsertLog/Styles/Redemption.less");
        }

        private static void RegisterBundles_Redemption_EditLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Redemption/EditLogScripts").Include(
                HotKeyManager,
                "~/Areas/Redemption/EditLog/Scripts/EditLogApplication.js",
                "~/Areas/Redemption/EditLog/Scripts/EditLogService.js",
                "~/Areas/Redemption/EditLog/Scripts/EditLogController.js",
                "~/Areas/Redemption/EditLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Redemption/EditLogStyles",
                "~/Areas/Redemption/EditLog/Styles/Redemption.less");
        }

        private static void RegisterBundles_Remark(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Remark/RemarkScripts").Include(
                BlockUI,
                "~/Scripts/ShareScripts/Applications/AlertBox.js",
                HotKeyManager,
                DirectiveManager,
                "~/Scripts/ShareScripts/Directives/RawHtml.js",
                RegisterTemplate));

            bundles.AddAndInclude(
                "~/Areas/Remark/RemarkStyles",
                "~/Areas/Remark/Styles/remark.less");
        }

        private static void RegisterBundles_ActivityLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/ActionLog/ActivityLogScripts").Include(
                FanexGrid,
                Moment,
                FanexPagination,
                HotKeyManager,
                "~/Areas/ActionLog/Common/Scripts/*.js",
                "~/Areas/ActionLog/Common/Scripts/Directives/*.js",
                "~/Areas/ActionLog/ActivityLog/Scripts/ActivityLogApplication.js",
                "~/Areas/ActionLog/ActivityLog/Scripts/ActivityLogService.js",
                "~/Areas/ActionLog/ActivityLog/Scripts/ActivityLogController.js",
                "~/Areas/ActionLog/ActivityLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/ActionLog/ActivityLogStyles",
                FanexGridCss,
                CustomizeFanexGridLess,
                FanexPaginationCss,
                "~/Areas/ActionLog/ActivityLog/Styles/activityLog.less");
        }

        private static void RegisterBundles_SettingLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/ActionLog/SettingLogScripts").Include(
                FanexGrid,
                Moment,
                FanexPagination,
                HotKeyManager,
                "~/Areas/ActionLog/Common/Scripts/*.js",
                "~/Areas/ActionLog/Common/Scripts/Directives/*.js",
                "~/Areas/ActionLog/SettingLog/Scripts/SettingLogApplication.js",
                "~/Areas/ActionLog/SettingLog/Scripts/SettingLogService.js",
                "~/Areas/ActionLog/SettingLog/Scripts/SettingLogController.js",
                "~/Areas/ActionLog/SettingLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/ActionLog/SettingLogStyles",
                FanexGridCss,
                CustomizeFanexGridLess,
                FanexPaginationCss,
                "~/Areas/ActionLog/SettingLog/Styles/settingLog.less");
        }

        private static void RegisterBundles_Dashboard_ViewInfo(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Dashboard/ViewInfoScripts").Include(
                "~/Scripts/AngularJS/angular-sanitize.js",
                HotKeyManager,
                "~/Areas/Dashboard/ViewInfo/Scripts/DashboardApplication.js",
                "~/Areas/Dashboard/ViewInfo/Scripts/DashboardService.js",
                "~/Areas/Dashboard/ViewInfo/Scripts/DashboardController.js",
                "~/Areas/Dashboard/ViewInfo/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Dashboard/ViewInfoStyles",
                "~/Areas/Dashboard/ViewInfo/Styles/StyleSheet.less");
        }

        private static void RegisterBundles_Dashboard_VerifyFlag(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Dashboard/VerifyFlagScripts").Include(
                "~/wwwroot/sources/libs/FanexGrid/numeral.js",
                HotKeyManager,
                "~/Areas/Dashboard/VerifyFlag/Scripts/VerifyFlagApplication.js",
                "~/Areas/Dashboard/VerifyFlag/Scripts/VerifyFlagService.js",
                "~/Areas/Dashboard/VerifyFlag/Scripts/VerifyFlagController.js",
                "~/Areas/Dashboard/VerifyFlag/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Dashboard/VerifyFlagStyles",
                "~/Areas/Dashboard/VerifyFlag/Styles/VerifyFlag.less");
        }

        private static void RegisterBundles_Report_DynamicReport(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/DynamicReportScripts").Include(
                SettingManager,
                HotKeyManager,
                "~/Scripts/WeekPicker/WeekPickerManager.js",
                DirectiveManager,
                DropdownList,
                "~/Scripts/ShareScripts/Directives/DropdownSite.js",
                DatePicker,
                "~/Scripts/ShareScripts/Directives/WeekPicker.js",
                "~/Scripts/ShareScripts/Directives/MonthPicker.js",
                "~/Scripts/ShareScripts/Directives/YearPicker.js",
                DefaultFocus,
                "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                RegisterTemplate,
                "~/Scripts/Directives/selectwithsearch/selectwithsearch.js",
                "~/Areas/Report/DynamicReport/Scripts/DynamicReportApplication.js",
                "~/Areas/Report/DynamicReport/Scripts/DynamicReportOption.js",
                "~/Areas/Report/DynamicReport/Scripts/DynamicReportService.js",
                "~/Areas/Report/DynamicReport/Scripts/DynamicReportController.js",
                "~/Areas/Report/DynamicReport/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Report/DynamicReportStyles",
                "~/Areas/Report/DynamicReport/Styles/DynamicReport.less");
        }

        private static void RegisterBundles_Report_RedemptionStatistics(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/RedemptionStatisticsScripts").Include(
                "~/Scripts/ShareScripts/Applications/AlertBox.js",
                HotKeyManager,
                DirectiveManager,
                DropdownList,
                "~/Scripts/ShareScripts/Directives/DropdownSite.js",
                "~/wwwroot/sources/libs/MultiSelect/fanex.multiSelect.js",
                MultiSelect,
                DatePicker,
                DefaultFocus,
                "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                RegisterTemplate,
                "~/Areas/Report/RedemptionStatistics/Scripts/RedemptionStatisticsApplication.js",
                "~/Areas/Report/RedemptionStatistics/Scripts/RedemptionStatisticsService.js",
                "~/Areas/Report/RedemptionStatistics/Scripts/RedemptionStatisticsController.js",
                "~/Areas/Report/RedemptionStatistics/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Report/RedemptionStatisticsStyles",
                CustomizeFanexGridLess,
                "~/Areas/Report/RedemptionStatistics/Styles/RedemptionStatistics.less");
        }

        private static void RegisterBundles_Report_CustomerReport(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/CustomerReportScripts").Include(
                HotKeyManager,
                DirectiveManager,
                DropdownList,
                DatePicker,
                DefaultFocus,
                MultiSelect,
                RegisterTemplate,
                "~/Areas/Report/CustomerReport/Scripts/CustomerReportApplication.js",
                "~/Areas/Report/CustomerReport/Scripts/CustomerReportService.js",
                "~/Areas/Report/CustomerReport/Scripts/CustomerReportController.js",
                "~/Areas/Report/CustomerReport/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Report/CustomerReportStyles",
                "~/Areas/Report/CustomerReport/Styles/CustomerReport.less");
        }

        private static void RegisterBundles_Report_DirectCustomerReport(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/DirectCustomerReportScripts").Include(
                HotKeyManager,
                DirectiveManager,
                DropdownList,
                DatePicker,
                DefaultFocus,
                MultiSelect,
                RegisterTemplate,
                "~/Areas/Report/DirectCustomerReport/Scripts/DirectCustomerReportApplication.js",
                "~/Areas/Report/DirectCustomerReport/Scripts/DirectCustomerReportService.js",
                "~/Areas/Report/DirectCustomerReport/Scripts/DirectCustomerReportController.js",
                "~/Areas/Report/DirectCustomerReport/Scripts/Directives/*.js"
                ));

            bundles.AddAndInclude(
                "~/Areas/Report/DirectCustomerReportStyles",
                "~/Areas/Report/DirectCustomerReport/Styles/DirectCustomerReport.less");
        }

        private static void RegisterBundles_Chart(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/CRM/Chart/Scripts").Include(
                "~/Scripts/Chart/ChartJs/ChartJS.js",
                "~/Scripts/Chart/ChartJs/CRMChart.js"));
        }

        private static void RegisterBundles_User_LogIn(ref BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/Areas/User/LogInStyles",
                "~/Areas/User/LogIn/Styles/login.less");
        }

        private static void RegisterBundles_DownLoadFile(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/CRM/DownLoadFile/Scripts").Include(
                "~/Scripts/JQuery/jquery.fileDownload.js",
                "~/Scripts/ShareScripts/Directives/DownloadFile.js"));
        }

        private static void RegisterBundles_Report_SettlementReport(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/SettlementReportScripts").Include(
                BlockUI,
                HotKeyManager,
                DirectiveManager,
                "~/Scripts/ShareScripts/Directives/YearPicker.js",
                DefaultFocus,
                RegisterTemplate,
                "~/Areas/Report/SettlementReport/Scripts/SettlementReportApplication.js",
                "~/Areas/Report/SettlementReport/Scripts/SettlementReportController.js",
                "~/Areas/Report/SettlementReport/Scripts/SettlementReportService.js",
                "~/Areas/Report/SettlementReport/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Report/SettlementReportStyles",
                "~/Areas/Report/SettlementReport/Styles/SettlementReport.less");
        }

        private static void RegisterBundles_Report_StatisticReport(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/StatisticReportScripts").Include(
                BlockUI,
                HotKeyManager,
                DirectiveManager,
                DropdownList,
                DatePicker,
                "~/Scripts/ShareScripts/Directives/MonthPicker.js",
                DefaultFocus,
                RegisterTemplate,
                "~/Areas/Report/StatisticReport/Scripts/StatisticReportApplication.js",
                "~/Areas/Report/StatisticReport/Scripts/StatisticReportController.js",
                "~/Areas/Report/StatisticReport/Scripts/StatisticReportService.js",
                "~/Areas/Report/StatisticReport/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Report/StatisticReportStyles",
                "~/Areas/Report/StatisticReport/Styles/StatisticReport.less");
        }

        private static void RegisterBundles_ResultMessage(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/ShareScripts/Applications/ResultMessageScripts").Include(
                "~/Scripts/ShareScripts/Applications/ResultMessage.js"));

            bundles.AddAndInclude(
                "~/Content/site/ResultMessageStyles",
                "~/Content/site/resultMessage.less");
        }

        private static void RegisterBundles_Numeral(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/wwwroot/sources/libs/FanexGrid/NumeralScripts").Include(
                "~/wwwroot/sources/libs/FanexGrid/numeral.js"));
        }

        private static void RegisterBundles_FanexGrid(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/wwwroot/sources/libs/FanexGrid/FanexGridScripts").Include(
                FanexPagination,
                FanexGrid,
                Moment));

            bundles.AddAndInclude(
                "~/wwwroot/sources/libs/FanexGrid/FanexGridCss",
                FanexGridCss,
                FanexPaginationCss,
                CustomizeFanexGridLess);
        }

        private static void RegisterBundles_Report_WagerReport(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/WagerReportScripts").Include(
                BlockUI,
                HotKeyManager,
                DirectiveManager,
                DefaultFocus,
                RegisterTemplate,
                "~/Areas/Report/WagerReport/Scripts/WagerReportApplication.js",
                "~/Areas/Report/WagerReport/Scripts/WagerReportController.js",
                "~/Areas/Report/WagerReport/Scripts/WagerReportService.js",
                "~/Areas/Report/WagerReport/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Report/WagerReportStyles",
                FanexGridCss,
                CustomizeFanexGridLess,
                FanexPaginationCss,
                "~/Areas/Report/WagerReport/Styles/WagerReport.less");
        }

        private static void RegisterBundles_Report_SummaryReport(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/SummaryReportScripts").Include(
                BlockUI,
                HotKeyManager,
                DirectiveManager,
                DefaultFocus,
                MultiSelect,
                RegisterTemplate,
                "~/Areas/Report/SummaryReport/Scripts/SummaryReportApplication.js",
                "~/Areas/Report/SummaryReport/Scripts/SummaryReportController.js",
                "~/Areas/Report/SummaryReport/Scripts/SummaryReportService.js",
                "~/Areas/Report/SummaryReport/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Report/SummaryReportStyles",
                FanexGridCss,
                CustomizeFanexGridLess,
                FanexPaginationCss,
                "~/Areas/Report/SummaryReport/Styles/SummaryReport.less");
        }

        private static void RegisterBundles_PicManagement(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/PicAndHeadline/PicManagement/ViewList/PicManagementScripts").Include(
                BlockUI,
                MultiLevelDropdown,
                MesageResult,
                RegisterTemplate));
        }

        private static void RegisterBundles_PicOfHeadline(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/PicAndHeadline/PicOfHeadlineScripts").Include(
                MesageResult,
                BlockUI,
                DirectiveManager,
                MultiSelect,
                RegisterTemplate,
                "~/wwwroot/sources/js/shared/focus-directive.js",
                "~/wwwroot/sources/js/shared/movable-modal-directive.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/PicOfHeadlineApplication.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/PicOfHeadlineService.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/PicOfHeadlineController.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/assignPicController.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/removeAssignmentController.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/setDefaultPicController.js",
                "~/Scripts/Select2/select2.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/Directives/*.js"
                ));
        }

        private static void RegisterBundles_ChangeLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/PicAndHeadline/ChangeLogScripts").Include(
                DirectiveManager,
                BlockUI,
                MultiLevelDropdown,
                MesageResult,
                RegisterTemplate,
                DatePicker));
        }
    }
}