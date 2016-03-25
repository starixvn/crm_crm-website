(function () {
    "use strict";
    /* global crmConstants,
    viewResources */
    angular.module('communicationlog-edit-application').directive('dropdownSite', function () {
        function renderDropdown(scope, element) {
            if (scope.siteList) {
                var dropdown = element.find("#site");
                dropdown.empty();
                var optionHtml =
                '<option value="' + crmConstants.Enums.Site.NotApply + '"' +
                     (crmConstants.Enums.Site.NotApply === scope.communicationLog.CommunicationTool.Id ?
                        ' selected="selected"' :
                        '') +
                '>' + viewResources.Select + '</option>';

                var siteGroupLength = scope.siteList.length;

                for (var i = 0; i < siteGroupLength; i++) {
                    var siteGroup = scope.siteList[i];
                    var siteLength = siteGroup.ChildSites ? siteGroup.ChildSites.length : 0;
                    if (siteLength === 0) {
                        optionHtml +=
                        '<option value="' + siteGroup.Id + '" selected="selected"' +
                        '>' + siteGroup.Name + '</option>';
                    }
                    else {
                        optionHtml += '<optgroup label="' + siteGroup.Name +
                        '" value="' + siteGroup.Id + '">';

                        for (var j = 0; j < siteLength; j++) {
                            var site = siteGroup.ChildSites[j];
                            optionHtml += '<option value="' + site.Id + '"' +
                            (site.Id === scope.communicationLog.SiteInfo.Id ?
                            ' selected="selected"' :
                            '') +
                            '>' + site.Name + '</option>';
                        }
                        optionHtml += '</optgroup>';
                    }
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