(function () {
    "use strict";
    angular.module('communicationlog-edit-application').directive('dropdownExtension', function () {
        function renderDropdown(scope, element) {
            if (scope.extensionList) {
                var dropdown = element.find("#ext");
                dropdown.empty();
                var extensionListLength = scope.extensionList.length;
                for (var i = 0; i < extensionListLength; i++) {
                    var extension = scope.extensionList[i];
                    dropdown.append(
                    '<option value="' + extension.Id + '"' +
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
                    /* jshint unused: false */
                    renderDropdown(scope, element);
                });
            }
        };
    });
})();