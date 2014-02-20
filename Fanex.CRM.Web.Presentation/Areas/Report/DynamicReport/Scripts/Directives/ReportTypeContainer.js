"use strict";
/* global dynamicReportApplication */
dynamicReportApplication.directive('periodTypeContainer', function () {
    function renderDropdown(scope, element, attrs) {
        /* jshint unused: vars */
        if (scope.periodTypeList) {
            var dropdown = element.find("#periodtype");
            dropdown.empty();
            var optionHtml = '';

            var periodTypeLength = scope.periodTypeList.length;
            for (var i = 0; i < periodTypeLength; i++) {
                var periodType = scope.periodTypeList[i];
                optionHtml += '<option value="' + periodType.Id + '">' + periodType.Name + '</option>';
            }

            dropdown.append(optionHtml);
            element.change(function () {
                scope.updateSelectedPeriodType(dropdown.find('option:selected').val());
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