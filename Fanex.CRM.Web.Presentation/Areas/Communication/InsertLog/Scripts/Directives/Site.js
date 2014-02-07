"use strict";
/* global insertLogApplication,
viewResources,
crmConstants */
insertLogApplication.directive('dropdownSite', function () {
    return {
        restrict: 'A',
        template: '<option value="' + crmConstants.Enums.Site.NotApply + '"' +
                     ' data-ng-click="updateSelectedSite();">' + viewResources.Select + '</option>\
                   <optgroup data-ng-repeat="siteGroup in siteList track by $index" label="{{ siteGroup.Name }}" value="{{ siteGroup.Id }}">\
                        <option data-ng-repeat="site in siteGroup.ChildSites track by $index"' +
                            ' data-ng-click="updateSelectedSite(site);"' +
                            ' value="{{ site.Id }}">{{ site.Name }}</option>\
                   </optgroup>',
        replace: false,
        priority: 0,
        transclude: false,
        scope: true,
        terminal: false,
        require: false,
        compile: function compile(tElement, tAttrs, transclude) {
            /* jshint unused: vars */
        },
        link: function (scope, elem, attrs) {
            /* jshint unused: vars */
        }
    };
});