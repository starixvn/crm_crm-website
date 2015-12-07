// --------------------------------------------------------------------------
// <copyright file="ReportBundles.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.01">Created</change>
// </history>

namespace Fanex.CRM.Report
{
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// Bundle Config of Report.
    /// </summary>
    public static class ReportBundles
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterCasinoDailyStatisticsBundles(bundles);
            RegisterCasinoWeeklyBundles(bundles);

            RegisterDailyStatisticsReportBundles(bundles);
            RegisterWeeklyStatisticsReportBundles(bundles);
            
            RegisterFeedbackStatisticBundles(bundles);
            RegisterFeedbackStatisticDetailBundles(bundles);

            RegisterLogStatisticBundles(bundles);
        }

        /// <summary>
        /// Registers the casino daily statistics bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCasinoDailyStatisticsBundles(BundleCollection bundles)
        {
            AddAndInclude(
                bundles,
                "~/bundles/Report/CasinoDailyStyles",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/Report/CasinoDailyStatistics/Styles/casinodaily.less");

            bundles.Add(new ScriptBundle("~/bundles/Report/CasinoDailyScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Report/CasinoDailyStatistics/Scripts/CasinoDailyApplication.js",
                "~/Areas/Report/CasinoDailyStatistics/Scripts/CasinoDailyService.js",
                "~/Areas/Report/CasinoDailyStatistics/Scripts/CasinoDailyController.js",
                "~/Areas/Report/CasinoDailyStatistics/Scripts/Directives/*.js"));
        }

        /// <summary>
        /// Registers the casino weekly bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCasinoWeeklyBundles(BundleCollection bundles)
        {
            AddAndInclude(
                bundles,
                "~/bundles/Report/CasinoWeeklyStyles",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/Report/CasinoWeeklyStatistics/Styles/casinoweekly.less");

            bundles.Add(new ScriptBundle("~/bundles/Report/CasinoWeeklyScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Report/CasinoWeeklyStatistics/Scripts/CasinoWeeklyApplication.js",
                "~/Areas/Report/CasinoWeeklyStatistics/Scripts/CasinoWeeklyService.js",
                "~/Areas/Report/CasinoWeeklyStatistics/Scripts/CasinoWeeklyController.js",
                "~/Areas/Report/CasinoWeeklyStatistics/Scripts/Directives/*.js"));
        }

        /// <summary>
        /// Registers the daily statistics report bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterDailyStatisticsReportBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/DailyStatisticsReportScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Report/DailyStatisticsReport/Scripts/DailyStatisticsReportApplication.js",
                "~/Areas/Report/DailyStatisticsReport/Scripts/DailyStatisticsReportService.js",
                "~/Areas/Report/DailyStatisticsReport/Scripts/DailyStatisticsReportController_Options.js",
                "~/Areas/Report/DailyStatisticsReport/Scripts/DailyStatisticsReportController_Update.js",
                "~/Areas/Report/DailyStatisticsReport/Scripts/DailyStatisticsReportController_HotKey.js",
                "~/Areas/Report/DailyStatisticsReport/Scripts/DailyStatisticsReportController.js",
                "~/Areas/Report/DailyStatisticsReport/Scripts/Directives/*.js"));

            AddAndInclude(
                bundles,
                "~/Areas/Report/DailyStatisticsReportStyles",
                "~/Areas/Report/DailyStatisticsReport/Styles/DailyStatisticsReport.less");
        }

        /// <summary>
        /// Registers the weekly statistics report bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterWeeklyStatisticsReportBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Report/WeeklyStatisticsReportScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Report/WeeklyStatisticsReport/Scripts/WeeklyStatisticsReportApplication.js",
                "~/Areas/Report/WeeklyStatisticsReport/Scripts/WeeklyStatisticsReportService.js",
                "~/Areas/Report/WeeklyStatisticsReport/Scripts/WeeklyStatisticsReportController_Options.js",
                "~/Areas/Report/WeeklyStatisticsReport/Scripts/WeeklyStatisticsReportController_Update.js",
                "~/Areas/Report/WeeklyStatisticsReport/Scripts/WeeklyStatisticsReportController_HotKey.js",
                "~/Areas/Report/WeeklyStatisticsReport/Scripts/WeeklyStatisticsReportController.js",
                "~/Areas/Report/WeeklyStatisticsReport/Scripts/Directives/*.js"));

            AddAndInclude(
                bundles,
                "~/Areas/Report/WeeklyStatisticsReportStyles",
                "~/Areas/Report/WeeklyStatisticsReport/Styles/WeeklyStatisticsReport.less");
        }
        
        /// <summary>
        /// Registers the feedback statistic bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterFeedbackStatisticBundles(BundleCollection bundles)
        {
            AddAndInclude(
                bundles,
                "~/bundles/Report/FeedbackStatisticStyles",
                "~/Areas/Report/FeedbackStatistics/Styles/styles.less");

            bundles.Add(new ScriptBundle("~/bundles/Report/FeedbackStatisticScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/Applications/AlertBox.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Report/FeedbackStatistics/Scripts/FeedbackStatisticApplication.js",
                "~/Areas/Report/FeedbackStatistics/Scripts/FeedbackStatisticService.js",
                "~/Areas/Report/FeedbackStatistics/Scripts/FeedbackStatisticController.js",
                "~/Areas/Report/FeedbackStatistics/Scripts/Directives/*.js"));
        }

        /// <summary>
        /// Registers the feedback statistic detail bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterFeedbackStatisticDetailBundles(BundleCollection bundles)
        {
            AddAndInclude(
                bundles,
                "~/bundles/Report/FeedbackStatisticDetailStyles",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/Report/FeedbackStatistics/FeedbackStatisticDetail/Styles/styles.less");

            bundles.Add(new ScriptBundle("~/bundles/Report/FeedbackStatisticDetailScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/Applications/AlertBox.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Report/FeedbackStatistics/FeedbackStatisticDetail/Scripts/FeedbackStatisticDetailApplication.js",
                "~/Areas/Report/FeedbackStatistics/FeedbackStatisticDetail/Scripts/FeedbackStatisticDetailService.js",
                "~/Areas/Report/FeedbackStatistics/FeedbackStatisticDetail/Scripts/FeedbackStatisticDetailController.js",
                "~/Areas/Report/FeedbackStatistics/FeedbackStatisticDetail/Scripts/Directives/*.js"));
        }

        /// <summary>
        /// Registers the log statistics bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterLogStatisticBundles(BundleCollection bundles)
        {
            AddAndInclude(
                bundles,
                "~/bundles/Report/LogStatisticStyles",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/Report/LogStatistic/Styles/DailyStatisticsReport.less");

            bundles.Add(new ScriptBundle("~/bundles/Report/LogStatisticScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/MultiSelect.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Report/LogStatistic/Scripts/LogStatisticApplication.js",
                "~/Areas/Report/LogStatistic/Scripts/LogStatisticService.js",
                "~/Areas/Report/LogStatistic/Scripts/LogStatisticController.js",
                "~/Areas/Report/LogStatistic/Scripts/Directives/*.js"));
        }

        private static void AddAndInclude(
            BundleCollection bundles,
            string bundleVirtualPath,
            params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new StyleTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }
    }
}
