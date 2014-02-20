"use strict";
/* global dynamicReportApplication */
dynamicReportApplication.directive('reportTypeContainer', function () {
    function renderDropdown(scope, element, attrs) {
        /* jshint unused: vars */
        if (scope.reportTypeList) {
            var dropdown = element.find("#reporttype");
            dropdown.empty();
            var optionHtml = '';

            var reportTypeLength = scope.reportTypeList.length;
            for (var i = 0; i < reportTypeLength; i++) {
                var reportType = scope.reportTypeList[i];
                optionHtml += '<option value="' + reportType.Id + '">' + reportType.Name + '</option>';
            }

            dropdown.append(optionHtml);
            element.change(function () {
                scope.updateSelectedReportType(dropdown.find('option:selected').val());
            });
        }
    }
    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                /* jshint unused: vars */
                renderDropdown(scope, element, attrs);
            });
        }
    };
});