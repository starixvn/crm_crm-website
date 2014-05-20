// --------------------------------------------------------------------------
// <copyright file="InformHeadlinesBundleRegister.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.12">Created</change>
// </history>

namespace Fanex.CRM.Web.View.App_Start
{
    using System.Web.Optimization;

    /// <summary>
    /// Inform Headlines Bundle Register.
    /// </summary>
    public class InformHeadlinesBundleRegister
    {
        public static void RegisterAll(ref BundleCollection bundles)
        {
            RegisterStatistic(ref bundles);
            RegisterCreateTitle(ref bundles);
            RegisterUpdateInforms(ref bundles);
        }

        /// <summary>
        /// Registers the bundles_ view inform.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterStatistic(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Customer/InformHeadlines/InformStatisticScript").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/Applications/AlertBox.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/ShareScripts/Directives/DefaultFocus.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Customer/InformHeadlines/InformStatistic/Scripts/InformStatisticApplication.js",
                "~/Areas/Customer/InformHeadlines/InformStatistic/Scripts/InformStatisticService.js",
                "~/Areas/Customer/InformHeadlines/InformStatistic/Scripts/InformStatisticController.js",
                "~/Areas/Customer/InformHeadlines/InformStatistic/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Customer/InformHeadlines/InformStatisticStyle",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/Customer/InformHeadlines/InformStatistic/Styles/InformStatistic.less");
        }

        /// <summary>
        /// Registers the bundles create inform.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCreateTitle(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Customer/InformHeadlines/CreateTitleScript").Include(
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/Applications/AlertBox.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Areas/Customer/InformHeadlines/CreateTitle/Scripts/CreateTitleApplication.js",
                "~/Areas/Customer/InformHeadlines/CreateTitle/Scripts/CreateTitleService.js",
                "~/Areas/Customer/InformHeadlines/CreateTitle/Scripts/CreateTitleController.js",
                "~/Areas/Customer/InformHeadlines/CreateTitle/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Customer/InformHeadlines/CreateTitleStyle",
                "~/Areas/Customer/InformHeadlines/CreateTitle/Styles/CreateTitle.less");
        }

        /// <summary>
        /// Registers the bundles update inform.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterUpdateInforms(ref BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Areas/Customer/InformHeadlines/UpdateInformScript").Include(
                "~/Scripts/ShareScripts/SharedScriptManager.js",
                "~/Scripts/ShareScripts/Applications/BlockUI.js",
                "~/Scripts/ShareScripts/Applications/AlertBox.js",
                "~/Scripts/ShareScripts/HotKeyManager.js",
                "~/Scripts/ShareScripts/DirectiveManager.js",
                "~/Scripts/MultipleSelect/fanex.multiSelect.js",
                "~/Scripts/ShareScripts/Directives/MultiSelect.js",
                "~/Scripts/ShareScripts/Directives/RegisterTemplate.js",
                "~/Areas/Customer/InformHeadlines/UpdateInform/Scripts/UpdateInformApplication.js",
                "~/Areas/Customer/InformHeadlines/UpdateInform/Scripts/UpdateInformService.js",
                "~/Areas/Customer/InformHeadlines/UpdateInform/Scripts/UpdateInformController.js",
                "~/Areas/Customer/InformHeadlines/UpdateInform/Scripts/Directives/*.js"));

            bundles.AddAndInclude(
                "~/Areas/Customer/InformHeadlines/UpdateInformStyle",
                "~/Scripts/FanexGridV2/Fanex.Grid.css",
                "~/Content/site/customizeFanexGridV2.less",
                "~/Scripts/FanexGridV2/Fanex.Pagination.css",
                "~/Areas/Customer/InformHeadlines/UpdateInform/Styles/UpdateInform.less");
        }
    }
}