namespace Fanex.CRM.Web.View.AppStart
{
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    public static class ReportBundles
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterFeedbackStatisticBundles(bundles);
            RegisterFeedbackStatisticDetailBundles(bundles);

            RegisterLogStatisticsBundles(bundles);
            RegisterDeletedFeedbackStatisticsBundles(bundles);
        }

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

        private static void RegisterLogStatisticsBundles(BundleCollection bundles)
        {
            AddAndInclude(
                bundles,
                "~/bundles/Report/LogStatisticsStyles",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/Report/LogStatistics/Styles/LogStatistics.less",
                "~/Areas/Report/DailyStatisticsReport/Styles/DailyStatisticsReport.less");

            bundles.Add(new ScriptBundle("~/bundles/Report/LogStatisticsScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/MultiSelect.js",
                "~/Scripts/ShareScripts/Directives/DropdownList.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Report/LogStatistics/Scripts/LogStatisticsApplication.js",
                "~/Areas/Report/LogStatistics/Scripts/LogStatisticsService.js",
                "~/Areas/Report/LogStatistics/Scripts/LogStatisticsOptions.js",
                "~/Areas/Report/LogStatistics/Scripts/LogStatisticsInitialPickers.js",
                "~/Areas/Report/LogStatistics/Scripts/LogStatisticsController.js",
                "~/Areas/Report/LogStatistics/Scripts/Directives/*.js"));
        }

        private static void RegisterDeletedFeedbackStatisticsBundles(BundleCollection bundles)
        {
            AddAndInclude(
                bundles,
                "~/bundles/Report/DeletedFeedbackStatisticsStyles",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/Report/DeletedFeedbackStatistics/Styles/deletedFeedbackStatistics.less");

            bundles.Add(new ScriptBundle("~/bundles/Report/DeletedFeedbackStatisticsScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/Directives/DatePicker.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Report/DeletedFeedbackStatistics/Scripts/DeletedFeedbackStatisticsApplication.js",
                "~/Areas/Report/DeletedFeedbackStatistics/Scripts/DeletedFeedbackStatisticsService.js",
                "~/Areas/Report/DeletedFeedbackStatistics/Scripts/DeletedFeedbackStatisticsController.js",
                "~/Areas/Report/DeletedFeedbackStatistics/Scripts/Directives/*.js"));
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