namespace Fanex.CRM.Web.View.AppStart
{
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    public static class InformHeadlineBundles
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterViewListBundles(bundles);
            RegisterNewInformBundles(bundles);
            RegisterUpdateInformBundles(bundles);
            RegisterContactInformBundles(bundles);
        }

        private static void RegisterViewListBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Bundles/InformHeadline/ViewListScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Content/bootstrap/js/bootstrap.min.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/InformHeadline/ViewList/Scripts/ViewListApplication.js",
                "~/Areas/InformHeadline/ViewList/Scripts/ViewListService.js",
                "~/Areas/InformHeadline/ViewList/Scripts/ViewListController.js",
                "~/Areas/InformHeadline/ViewList/Scripts/Directives/*.js",
                "~/wwwroot/dist/js/inform.headline.logs.js"));

            AddAndInclude(
                bundles,
                "~/Areas/InformHeadline/ViewListStyles",
                "~/wwwroot/dist/styles/inform.headline.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Areas/InformHeadline/ViewList/Styles/ViewList.less");
        }

        private static void RegisterNewInformBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Bundles/InformHeadline/NewInformScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Content/bootstrap/js/bootstrap.min.js",
                "~/Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.min.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/InformHeadline/NewInform/Scripts/ViewListApplication.js",
                "~/Areas/InformHeadline/NewInform/Scripts/ViewListService.js",
                "~/Areas/InformHeadline/NewInform/Scripts/ViewListController.js",
                "~/Areas/InformHeadline/NewInform/Scripts/Directives/*.js"));

            AddAndInclude(
                bundles,
                "~/Areas/InformHeadline/NewInformStyles",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css",
                "~/Areas/InformHeadline/NewInform/Styles/ViewList.less");
        }

        private static void RegisterContactInformBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Bundles/InformHeadline/ContactInformScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Content/bootstrap/js/bootstrap.min.js",
                "~/Scripts/AngularJS/angular-sanitize.min.js",
                "~/Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.min.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/InformHeadline/ContactInform/Scripts/ViewListApplication.js",
                "~/Areas/InformHeadline/ContactInform/Scripts/ViewListService.js",
                "~/Areas/InformHeadline/ContactInform/Scripts/ViewListController.js",
                "~/Areas/InformHeadline/ContactInform/Scripts/Directives/*.js"));

            AddAndInclude(
                bundles,
                "~/Areas/InformHeadline/ContactInformStyles",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css",
                "~/Areas/InformHeadline/ContactInform/Styles/ViewList.less");
        }

        private static void RegisterUpdateInformBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Bundles/InformHeadline/UpdateInformScripts").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Content/bootstrap/js/bootstrap.min.js",
                "~/Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.min.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Scripts/ShareScripts/MessageResult.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/InformHeadline/UpdateInform/Scripts/ViewListApplication.js",
                "~/Areas/InformHeadline/UpdateInform/Scripts/ViewListService.js",
                "~/Areas/InformHeadline/UpdateInform/Scripts/ViewListController.js",
                "~/Areas/InformHeadline/UpdateInform/Scripts/Directives/*.js"));

            AddAndInclude(
                bundles,
                "~/Areas/InformHeadline/UpdateInformStyles",
                "~/wwwroot/sources/libs/FanexGrid/Fanex.Pagination.css",
                "~/Areas/InformHeadline/UpdateInform/Styles/ViewList.less");
        }

        private static void AddAndInclude(BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new StyleTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }
    }
}