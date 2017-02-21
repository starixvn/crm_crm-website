(function () {
    "use strict";
    angular.module('edit-multiple-logs-application').directive('dropdownMainCategory', function () {
        function renderDropdown(scope, element) {
            if (scope.extensionList) {
                var dropdown = element.find("#mainCategory");
                dropdown.empty();
                var mainCategoryListLength = scope.mainCategoryList.length;
                for (var i = 0; i < mainCategoryListLength; i++) {
                    var mainCategory = scope.mainCategoryList[i];
                    dropdown.append(
                    '<option value="' + mainCategory.Id + '"' +
                         (scope.multipleLogs.MainCategory.Name !== '' && mainCategory.Name === scope.multipleLogs.MainCategory.Name ?
                            ' selected="selected"' :
                            '') +
                    '>' + mainCategory.Name + '</option>');
                }

                element.change(function () {
                    scope.updateSelectedMainCategory(dropdown.find('option:selected').val());
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