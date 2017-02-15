(function () {
    "use strict";
    /* global viewResources,
    crmConstants */
    angular.module('edit-multiple-logs-application').directive('dropdownSite', function () {
        function renderDropdown(scope, element) {
            if (scope.siteList) {
                var dropdown = element.find("#site");
                dropdown.empty();
                var optionHtml =
                '<option value="' + crmConstants.Enums.Site.NotApply + '">' +
                    viewResources.Select +
                  '</option>';

                var siteGroupLength = scope.siteList.length;
                for (var i = 0; i < siteGroupLength; i++) {
                    var siteGroup = scope.siteList[i];
                    optionHtml += '<optgroup label="' + siteGroup.Name +
                    '" value="' + siteGroup.Id + '">';
                    var siteLength = siteGroup.ChildSites.length;
                    for (var j = 0; j < siteLength; j++) {
                        var site = siteGroup.ChildSites[j];
                        optionHtml +=
                    '<option value="' + site.Id + '">' +
                        site.Name +
                    '</option>';
                    }

                    optionHtml += '</optgroup>';
                }

                dropdown.append(optionHtml);
                element.change(function () {
                    scope.updateSelectedSite(dropdown.find('option:selected').val());
                });
            }
        }
        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    /* jshint unused: false */
                    renderDropdown(scope, element);
                });
            }
        };
    });
})();