namespace Fanex.CRM.Web.View.AppStart
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/site/common",
                "~/content/bootstrap/css/bootstrap.css",
                "~/Content/site/common.less",
                "~/Content/site/Message.less",
                "~/Content/vendors/jqueryui/jquery-ui.min.css",
                "~/Content/site/customizeJqueryUI.less");

            bundles.Add(new ScriptBundle("~/scripts/headload").Include("~/Scripts/head.load.js"));

            bundles.Add(new ScriptBundle("~/scripts/respond").Include("~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/Scripts/PopUp").Include(
                "~/Scripts/showhidepopup.js",
                "~/Scripts/dialogbox.js"));

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

            bundles.AddAndInclude(
                "~/Views/HomeStyles",
                "~/Scripts/Tipsy-Tooltip/tipsy.css");

            bundles.Add(new ScriptBundle("~/Views/HomeScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Views/Home/Scripts/MenuManager.js",
                "~/Views/Home/Scripts/LeftPanelManager.js",
                "~/Views/Home/Scripts/HomeManager.js",
                "~/wwwroot/sources/libs/FanexGrid/moment.js"));

            bundles.Add(new ScriptBundle("~/Scripts/AngularJSLibrary").Include(
                "~/Scripts/AngularJS/angular.min.js",
                "~/Scripts/AngularJS/ui-bootstrap-tpls-1.0.3.min.js"));

            bundles.AddAndInclude(
                "~/Styles/FanexMultipleSelect",
                "~/Scripts/MultipleSelect/fanex.multiSelect.css",
                "~/Content/site/customizeMultiSelect.less",
                "~/Areas/Customer/DetailInfo/Styles/StyleSheet.less");

            bundles.Add(new ScriptBundle("~/Scripts/FanexMultipleSelect").Include(
                "~/Scripts/MultipleSelect/fanex.multiSelect.js"));

            bundles.Add(new ScriptBundle("~/Scripts/FanexGridScripts").Include(
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.js",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.js",
                "~/wwwroot/sources/libs/FanexGrid/moment.js"));

            bundles.Add(new ScriptBundle("~/Views/Home/Dashboard/Scripts").Include(
                "~/Views/Home/Scripts/DashboardManager.js"));

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

            FeedbackBundles.RegisterAll(bundles);
            ReportBundles.RegisterBundles(bundles);
            ReportBundles.RegisterBundles(bundles);
            ReportBundles.RegisterBundles(bundles);

            AdminToolBundles.RegisterBundles(bundles);
            InformHeadlineBundles.RegisterBundles(bundles);

            RegisterBundles_FanexGrid(bundles);
        }

        private static void RegisterBundles_Customer_ShortInfo(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/ShortCustomerInfo/Scripts").Include(
                "~/Scripts/jscookies.js",
                "~/Scripts/ShareScripts/SettingManager.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
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
                "~/Scripts/ShareScripts/HotKeyManager.js",
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
                "~/Scripts/ShareScripts/HotKeyManager.js",
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
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/SettingManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/Directives/multileveldropdown/multileveldropdown.js",
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
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/Directives/RelatedCustomerResult.js",
                "~/Scripts/Directives/multileveldropdown/multileveldropdown.js",
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
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/SettingManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/DropdownSite.js",
                "~/Scripts/ShareScripts/Directives/HtmlArea.js",
                "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                "~/Scripts/ShareScripts/Directives/DateTimePicker.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/Directives/RelatedCustomerResult.js",
                "~/Scripts/Directives/multileveldropdown/multileveldropdown.js",
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
                "~/Scripts/ShareScripts/HotKeyManager.js",
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
                "~/Scripts/ShareScripts/HotKeyManager.js",
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
                "~/Scripts/ShareScripts/HotKeyManager.js",
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
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/Applications/AlertBox.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/RawHtml.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js"));

            bundles.AddAndInclude(
                "~/Areas/Remark/RemarkStyles",
                "~/Areas/Remark/Styles/remark.less");
        }

        private static void RegisterBundles_ActivityLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/ActionLog/ActivityLogScripts").Include(
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.js",
                "~/wwwroot/sources/libs/FanexGrid/moment.js",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/ActionLog/Common/Scripts/*.js",
                "~/Areas/ActionLog/Common/Scripts/Directives/*.js",
                "~/Areas/ActionLog/ActivityLog/Scripts/ActivityLogApplication.js",
                "~/Areas/ActionLog/ActivityLog/Scripts/ActivityLogService.js",
                "~/Areas/ActionLog/ActivityLog/Scripts/ActivityLogController.js",
                "~/Areas/ActionLog/ActivityLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/ActionLog/ActivityLogStyles",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css",
                "~/Areas/ActionLog/ActivityLog/Styles/activityLog.less");
        }

        private static void RegisterBundles_SettingLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/ActionLog/SettingLogScripts").Include(
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.js",
                "~/wwwroot/sources/libs/FanexGrid/moment.js",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/ActionLog/Common/Scripts/*.js",
                "~/Areas/ActionLog/Common/Scripts/Directives/*.js",
                "~/Areas/ActionLog/SettingLog/Scripts/SettingLogApplication.js",
                "~/Areas/ActionLog/SettingLog/Scripts/SettingLogService.js",
                "~/Areas/ActionLog/SettingLog/Scripts/SettingLogController.js",
                "~/Areas/ActionLog/SettingLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/ActionLog/SettingLogStyles",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css",
                "~/Areas/ActionLog/SettingLog/Styles/settingLog.less");
        }

        private static void RegisterBundles_Dashboard_ViewInfo(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Dashboard/ViewInfoScripts").Include(
                "~/Scripts/AngularJS/angular-sanitize.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
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
                "~/Scripts/ShareScripts/HotKeyManager.js",
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
                "~/Scripts/ShareScripts/SettingManager.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/WeekPicker/WeekPickerManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/DropdownSite.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/WeekPicker.js",
                "~/Scripts/ShareScripts/Directives/MonthPicker.js",
                "~/Scripts/ShareScripts/Directives/YearPicker.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
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
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/DropdownSite.js",
                "~/Scripts/MultipleSelect/fanex.multiSelect.js",
                "~/Scripts/ShareScripts/Directives/MultiSelect.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Report/RedemptionStatistics/Scripts/RedemptionStatisticsApplication.js",
                "~/Areas/Report/RedemptionStatistics/Scripts/RedemptionStatisticsService.js",
                "~/Areas/Report/RedemptionStatistics/Scripts/RedemptionStatisticsController.js",
                "~/Areas/Report/RedemptionStatistics/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Report/RedemptionStatisticsStyles",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Areas/Report/RedemptionStatistics/Styles/RedemptionStatistics.less");
        }

        private static void RegisterBundles_Report_CustomerReport(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/CustomerReportScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/MultiSelect.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
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
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/MultiSelect.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
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
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/YearPicker.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
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
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/MonthPicker.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
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
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.js",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.js",
                "~/wwwroot/sources/libs/FanexGrid/moment.js"));

            bundles.AddAndInclude(
                "~/wwwroot/sources/libs/FanexGrid/FanexGridStyles",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.css",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css",
                "~/Content/site/customizeFanexGridV2.less");
        }

        private static void RegisterBundles_Report_WagerReport(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/WagerReportScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Report/WagerReport/Scripts/WagerReportApplication.js",
                "~/Areas/Report/WagerReport/Scripts/WagerReportController.js",
                "~/Areas/Report/WagerReport/Scripts/WagerReportService.js",
                "~/Areas/Report/WagerReport/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Report/WagerReportStyles",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css",
                "~/Areas/Report/WagerReport/Styles/WagerReport.less");
        }

        private static void RegisterBundles_Report_SummaryReport(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/SummaryReportScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/MultiSelect.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Report/SummaryReport/Scripts/SummaryReportApplication.js",
                "~/Areas/Report/SummaryReport/Scripts/SummaryReportController.js",
                "~/Areas/Report/SummaryReport/Scripts/SummaryReportService.js",
                "~/Areas/Report/SummaryReport/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Report/SummaryReportStyles",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css",
                "~/Areas/Report/SummaryReport/Styles/SummaryReport.less");
        }

        private static void RegisterBundles_PicManagement(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/PicAndHeadline/PicManagement/ViewList/PicManagementScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/Directives/multileveldropdown/multileveldropdown.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js"));
        }

        private static void RegisterBundles_PicOfHeadline(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/PicAndHeadline/PicOfHeadlineScripts").Include(
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/MultiSelect.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/PicOfHeadlineApplication.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/PicOfHeadlineService.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/PicOfHeadlineController.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/PicOfHeadlineController_HotKey.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/assignPicController.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/removeAssignmentController.js",
                "~/Areas/PicAndHeadline/PicOfHeadline/Scripts/Directives/*.js",
                "~/Scripts/Select2/select2.js"
                ));

            bundles.AddAndInclude(
                "~/Areas/PicAndHeadline/PicOfHeadlineStyles",
                "~/Areas/PicAndHeadline/PicOfHeadline/Styles/select2.css",
                "~/Areas/PicAndHeadline/PicOfHeadline/Styles/PicOfHeadline.less");
        }
    }
}