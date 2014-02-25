"use strict";
/* global insertLogApplication,
crmConstants */
insertLogApplication.directive('dropdownCommunicationTool', function () {
    function renderDropdown(scope, element, attrs) {
        /* jshint unused: vars */
        if (scope.communicationToolList) {
            var dropdown = element.find("#communicationTool");
            dropdown.empty();
            /* jshint camelcase: false */
            var communicationToolListLength = scope.communicationToolList.length;
            for (var i = 0; i < communicationToolListLength; i++) {
                var communicationTool = scope.communicationToolList[i];
                if (communicationTool.Id === crmConstants.Enums.CommunicationTool.NotApply) {
                    continue;
                }

                dropdown.append(
                    '<option value="' + communicationTool.Id + '">' + communicationTool.Name + '</option>');
            }

            element.change(function () {
                scope.updateSelectedCommunicationTool(dropdown.find('option:selected').val());
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