"use strict";
/* global editLogApplication */
editLogApplication.directive('dropdownExtension', function () {
    function renderDropdown(scope, element, attrs) {
        /* jshint unused: vars */
        if (scope.extensionList) {
            var dropdown = element.find("#ext");
            dropdown.empty();
            /* jshint camelcase: false */
            var extensionListLength = scope.extensionList.length;
            for (var i = 0; i < extensionListLength; i++) {
                var extension = scope.extensionList[i];
                dropdown.append(
                    '<option value="' + extension.Id +'"' +
                        (extension.Id === scope.communicationLog.Extension.Id ?
                        ' selected="selected"' :
                        '') +
                        '>' + extension.Value + '</option>');
            }

            element.change(function () {
                scope.updateSelectedExtension(dropdown.find('option:selected').val());
            });
        }
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                /* jshint unused: vars */
                renderDropdown(scope, element, attrs);
            });
        }
    };
});