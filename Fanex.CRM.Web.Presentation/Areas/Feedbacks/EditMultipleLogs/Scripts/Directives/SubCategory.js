(function () {
    "use strict";
    angular.module('edit-multiple-logs-application').directive('dropdownSubCategory', function () {
        function renderDropdown(scope, element) {
            if (scope.subCategoryList) {
                var dropdown = element.find("#subCategory");
                dropdown.empty();
                var subCategoryListLength = scope.subCategoryList.length;
                for (var i = 0; i < subCategoryListLength; i++) {
                    var subCategory = scope.subCategoryList[i];
                    dropdown.append(
                    '<option value="' + subCategory.Id + '"' +
                        (scope.multipleLogs.SubCategory.Name !== '' && subCategory.Name === scope.multipleLogs.SubCategory.Name ?
                            ' selected="selected"' :
                            '') +
                        '>' + subCategory.Name + '</option>');
                }

                element.change(function () {
                    scope.updateSelectedSubCategory(dropdown.find('option:selected').val());
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