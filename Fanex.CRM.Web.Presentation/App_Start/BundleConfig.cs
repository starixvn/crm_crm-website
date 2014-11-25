// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Web.View.AppStart
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;
    using Fanex.CRM.AdminTool;
    using Fanex.CRM.Customer.BundleConfigs;
    using Fanex.CRM.Feedback.Bundles;
    using Fanex.CRM.Report;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    public static class BundleConfig
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
                "~/Scripts/head.load.js"));

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
                "~/Views/Home/Scripts/HomeManager.js",
                "~/Scripts/FanexGridV2/moment.js"));

            bundles.Add(new ScriptBundle("~/Scripts/AngularJSLibrary").Include(
                "~/Scripts/AngularJS/angular.js"));

            bundles.AddAndInclude(
                "~/Styles/FanexMultipleSelect",
                "~/Scripts/MultipleSelect/fanex.multiSelect.css",
                "~/Content/site/customizeMultiSelect.less",
                "~/Areas/Customer/DetailInfo/Styles/StyleSheet.less");

            bundles.Add(new ScriptBundle("~/Scripts/FanexMultipleSelect").Include(
                "~/Scripts/MultipleSelect/fanex.multiSelect.js"));

            bundles.Add(new ScriptBundle("~/Scripts/FanexGridScripts").Include(
                "~/Scripts/FanexGridV2/Fanex.Pagination.js",
                "~/Scripts/FanexGridV2/Fanex.Grid.js",
                "~/Scripts/FanexGridV2/moment.js"));

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
            RegisterBundles_Report_CustomerReport(ref bundles);
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

            FeedbackBundles.RegisterAll(bundles);
            ReportBundles.RegisterBundles(bundles);
            ReportBundles.RegisterBundles(bundles);
            ReportBundles.RegisterBundles(bundles);

            InformHeadlinesBundles.RegisterAll(bundles);
            AdminToolBundles.RegisterBundles(bundles);

            RegisterBundles_FanexGrid(bundles);
        }

        /// <summary>
        /// Registers the bundles customer short information.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_Customer_ShortInfo(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/ShortCustomerInfo/Scripts").Include(
                "~/Scripts/jscookies.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
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

        /// <summary>
        /// Registers the bundles communication log view log.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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

        /// <summary>
        /// Registers the bundles communication log edit.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_CommunicationLog_Edit(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Communication/EditLogScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
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
        private static void RegisterBundles_CommunicationLog_Insert(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Communication/InsertLogScripts").Include(
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Communication/InsertLog/Scripts/InsertLogApplication.js",
                "~/Areas/Communication/InsertLog/Scripts/InsertLogService.js",
                "~/Areas/Communication/InsertLog/Scripts/InsertLogController.js",
                "~/Areas/Communication/InsertLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Communication/InsertLogStyles",
                "~/Areas/Communication/InsertLog/Styles/InsertCommunicationLog.less");
        }

        /// <summary>
        /// Registers the bundles communication log view detail by identify.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_CommunicationLog_ViewDetailById(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Communication/ViewDetailByIdScripts").Include(
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/DropdownSite.js",
                "~/Scripts/ShareScripts/Directives/HtmlArea.js",
                "~/Scripts/ShareScripts/Directives/SubjectAutoComplete.js",
                "~/Scripts/ShareScripts/Directives/DateTimePicker.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Communication/ViewDetailById/Scripts/ViewDetailByIdApplication.js",
                "~/Areas/Communication/ViewDetailById/Scripts/ViewDetailByIdService.js",
                "~/Areas/Communication/ViewDetailById/Scripts/ViewDetailByIdController.js",
                "~/Areas/Communication/ViewDetailById/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Communication/ViewDetailByIdStyles",
                "~/Areas/Communication/ViewDetailById/Styles/ViewDetailById.less");
        }

        /// <summary>
        /// Registers the bundles redemption view log.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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

        /// <summary>
        /// Registers the bundles redemption insert log.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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

        /// <summary>
        /// Registers the bundles redemption edit log.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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

        /// <summary>
        /// Registers the bundles remark.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_Remark(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Remark/RemarkScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/Applications/AlertBox.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/RawHtml.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Remark/Scripts/RemarkApplication.js",
                "~/Areas/Remark/Scripts/RemarkService.js",
                "~/Areas/Remark/Scripts/RemarkController.js",
                "~/Areas/Remark/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Remark/RemarkStyles",
                "~/Areas/Remark/Styles/remark.less");
        }

        /// <summary>
        /// Registers the activity log bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_ActivityLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/ActionLog/ActivityLogScripts").Include(
                "~/Scripts/FanexGridV2/Fanex.Grid.js",
                "~/Scripts/FanexGridV2/moment.js",
                "~/Scripts/FanexGridV2/Fanex.Pagination.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/ActionLog/ActivityLog/Scripts/ActivityLogApplication.js",
                "~/Areas/ActionLog/ActivityLog/Scripts/ActivityLogService.js",
                "~/Areas/ActionLog/ActivityLog/Scripts/ActivityLogController.js",
                "~/Areas/ActionLog/ActivityLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/ActionLog/ActivityLogStyles",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/ActionLog/ActivityLog/Styles/activityLog.less");
        }

        /// <summary>
        /// Registers the bundles_ setting log.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_SettingLog(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/ActionLog/SettingLogScripts").Include(
                "~/Scripts/FanexGridV2/Fanex.Grid.js",
                "~/Scripts/FanexGridV2/moment.js",
                "~/Scripts/FanexGridV2/Fanex.Pagination.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/ActionLog/SettingLog/Scripts/SettingLogApplication.js",
                "~/Areas/ActionLog/SettingLog/Scripts/SettingLogService.js",
                "~/Areas/ActionLog/SettingLog/Scripts/SettingLogController.js",
                "~/Areas/ActionLog/SettingLog/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/ActionLog/SettingLogStyles",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/ActionLog/SettingLog/Styles/settingLog.less");
        }

        /// <summary>
        /// Registers the bundles dashboard view information.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_Dashboard_ViewInfo(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Dashboard/ViewInfoScripts").Include(
                "~/Scripts/AngularJS/sanitize.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Dashboard/ViewInfo/Scripts/DashboardApplication.js",
                "~/Areas/Dashboard/ViewInfo/Scripts/DashboardService.js",
                "~/Areas/Dashboard/ViewInfo/Scripts/DashboardController.js",
                "~/Areas/Dashboard/ViewInfo/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Dashboard/ViewInfoStyles",
                "~/Areas/Dashboard/ViewInfo/Styles/StyleSheet.less");
        }

        /// <summary>
        /// Registers the bundles dashboard verify flag.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_Dashboard_VerifyFlag(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Dashboard/VerifyFlagScripts").Include(
                "~/Scripts/FanexGridV2/numeral.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Dashboard/VerifyFlag/Scripts/VerifyFlagApplication.js",
                "~/Areas/Dashboard/VerifyFlag/Scripts/VerifyFlagService.js",
                "~/Areas/Dashboard/VerifyFlag/Scripts/VerifyFlagController.js",
                "~/Areas/Dashboard/VerifyFlag/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Dashboard/VerifyFlagStyles",
                "~/Areas/Dashboard/VerifyFlag/Styles/VerifyFlag.less");
        }

        /// <summary>
        /// Registers the bundles dynamic report.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_Report_DynamicReport(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/DynamicReportScripts").Include(
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
                "~/Areas/Report/DynamicReport/Scripts/DynamicReportApplication.js",
                "~/Areas/Report/DynamicReport/Scripts/DynamicReportService.js",
                "~/Areas/Report/DynamicReport/Scripts/DynamicReportController.js",
                "~/Areas/Report/DynamicReport/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Report/DynamicReportStyles",
                "~/Areas/Report/DynamicReport/Styles/DynamicReport.less");
        }

        /// <summary>
        /// Registers the bundles customer report.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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

        /// <summary>
        /// Registers the bundles chart.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_Chart(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/CRM/Chart/Scripts").Include(
                "~/Scripts/Chart/ChartJs/ChartJS.js",
                "~/Scripts/Chart/ChartJs/CRMChart.js"));
        }

        /// <summary>
        /// Registers the user log in bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_User_LogIn(ref BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/Areas/User/LogInStyles",
                "~/Areas/User/LogIn/Styles/login.less");
        }

        /// <summary>
        /// Registers the bundles download file.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_DownLoadFile(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/CRM/DownLoadFile/Scripts").Include(
                "~/Scripts/JQuery/jquery.fileDownload.js",
                "~/Scripts/ShareScripts/Directives/DownloadFile.js"));
        }

        /// <summary>
        /// Registers the settlement report bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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

        /// <summary>
        /// Registers the statistic report bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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

        /// <summary>
        /// Registers the result message bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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
            bundles.Add(new ScriptBundle("~/Scripts/FanexGridV2/NumeralScripts").Include(
                "~/Scripts/FanexGridV2/numeral.js"));
        }

        /// <summary>
        /// Registers the bundles FANEX grid.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBundles_FanexGrid(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/FanexGridV2/FanexGridScripts").Include(
                "~/Scripts/FanexGridV2/Fanex.Pagination.js",
                "~/Scripts/FanexGridV2/Fanex.Grid.js",
                "~/Scripts/FanexGridV2/moment.js"));

            bundles.AddAndInclude(
                "~/Scripts/FanexGridV2/FanexGridStyles",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Content/site/customizeFanexGridV2.less");
        }

        /// <summary>
        /// Registers the bundles_ report_ wager report.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/Report/WagerReport/Styles/WagerReport.less");
        }

        /// <summary>
        /// Registers the bundles_ report_ summary report.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/Report/SummaryReport/Styles/SummaryReport.less");
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
    }
}