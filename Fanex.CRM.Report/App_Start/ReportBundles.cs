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
        /// Registers the bundles customer report.
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

        private static void AddAndInclude(
            BundleCollection bundles,
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
